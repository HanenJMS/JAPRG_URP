using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
namespace GameLab.GridSystem
{
    public class HexGridChunk : MonoBehaviour
    {
        HexCell[] hexCells;
        [SerializeField] HexMesh terrain, rivers, roads, water, waterShore;
        
        private void Awake()
        {
            hexCells = new HexCell[HexMetric.chunkSizeX * HexMetric.chunkSizeZ];
            enabled = false;
        }

        private void LateUpdate()
        {
            if(enabled)
                Triangulate();
            enabled = false;
        }
        public void InitializeCells()
        {
            foreach (var item in hexCells)
            {
                item.SetElevation(0);
                item.InitlializeCell();
                item.SetColor(Color.white);
            }
        }

        //Triangulation methods
        
        //initialization
        public void Triangulate()
        {
            terrain.Clear();
            rivers.Clear();
            roads.Clear();
            water.Clear();
            waterShore.Clear();
            Triangulate(hexCells);
            terrain.Apply();
            rivers.Apply();
            roads.Apply();
            water.Apply();
            waterShore.Apply();
        }
        public void Triangulate(HexCell[] cells)
        {
            
            for (int i = 0; i < cells.Length; i++)
            {
                Triangulate(cells[i]);
            }

        }
        void Triangulate(HexCell cell)
        {
            for (HexCellDirections d = HexCellDirections.NE; d <= HexCellDirections.NW; d++)
            {
                Triangulate(d, cell);
            }
        }
        void Triangulate(HexCellDirections direction, HexCell cell)
        {
            //Inner Hexagon
            Vector3 center = cell.transform.localPosition;
            EdgeVertices e = new EdgeVertices
            (
            center + HexMetric.GetFirstSolidCorner(direction),
            center + HexMetric.GetSecondSolidCorner(direction)
            );

            if (cell.HasRiver)
            {
                if (cell.HasRiverThroughEdge(direction))
                {
                    e.v3.y = cell.StreamBedY;
                    if (cell.HasRiverBeginOrEnd)
                    {
                        TriangulateWithRiverBeginOrEnd(direction, cell, center, e);
                    }
                    else
                    {
                        TriangulateWithRiver(direction, cell, center, e);
                    }
                }
                else
                {
                    TriangulateAdjacentToRiver(direction, cell, center, e);
                }
            }
            else
            {
                TriangulateWithoutRiver(direction, cell, center, e);
            }
            if (direction <= HexCellDirections.SE)
            {
                TriangulateConnection(direction, cell, e);
            }
            if (cell.IsUnderwater)
            {
                TriangulateWater(direction, cell, center);
            }
        }

        //hex triangulation base construction
        void TriangulateEdgeFan(Vector3 center, EdgeVertices edge, Color color)
        {
            terrain.AddTriangle(center, edge.v1, edge.v2);
            terrain.AddTriangleColor(color);
            terrain.AddTriangle(center, edge.v2, edge.v3);
            terrain.AddTriangleColor(color);
            terrain.AddTriangle(center, edge.v3, edge.v4);
            terrain.AddTriangleColor(color);
            terrain.AddTriangle(center, edge.v4, edge.v5);
            terrain.AddTriangleColor(color);
        }
        void TriangulateConnection(HexCellDirections direction, HexCell cell, EdgeVertices e1)
        {
            var directionalNeighbor = HexGridVisualSystem.Instance.GetHexCell(cell.GetHexCellNeighborGridPosition(direction));
            if (directionalNeighbor == null) return;
            //rectangular bridge between hexagon tiles
            var directionalBridge = HexMetric.GetBridge(direction);
            directionalBridge.y = directionalNeighbor.transform.position.y - cell.transform.position.y;
            EdgeVertices e2 = new EdgeVertices(
                e1.v1 + directionalBridge,
                e1.v5 + directionalBridge
            );
            if (cell.HasRiverThroughEdge(direction))
            {
                e2.v3.y = directionalNeighbor.StreamBedY;
                if (!cell.IsUnderwater)
                {
                    if (!directionalNeighbor.IsUnderwater)
                    {
                        TriangulateRiverQuad(
                            e1.v2, e1.v4, e2.v2, e2.v4,
                            cell.RiverSurfaceY, directionalNeighbor.RiverSurfaceY, 0.8f,
                            cell.HasIncomingRiver && cell.IncomingRiver == direction
                        );
                    }
                    else if (cell.GetElevation() > directionalNeighbor.WaterLevel)
                    {
                        TriangulateWaterfallInWater(
                            e1.v2, e1.v4, e2.v2, e2.v4,
                            cell.RiverSurfaceY, directionalNeighbor.RiverSurfaceY,
                            directionalNeighbor.WaterSurfaceY
                        );
                    }
                }
                else if (!directionalNeighbor.IsUnderwater && directionalNeighbor.GetElevation() > cell.WaterLevel) 
                {
                    TriangulateWaterfallInWater(
                        e2.v4, e2.v2, e1.v4, e1.v2,
                        directionalNeighbor.RiverSurfaceY, cell.RiverSurfaceY,
                        cell.WaterSurfaceY
                    );
                }
            }
            if (HexMetric.GetEdgeType(cell.GetEdgeType(direction)) == HexEdgeType.Slope)
            {
                //TriangulateEdgeTerraces(direction, cell, e1.v1, e1.v4, directionalNeighbor, e2.v1, e2.v4);
                TriangulateEdgeTerraces(e1, cell, e2, directionalNeighbor, cell.HasRoadThroughEdge(direction));
            }
            else
            {
                TriangulateEdgeStrip(e1, cell.GetCellColor(), e2, directionalNeighbor.GetCellColor(), cell.HasRoadThroughEdge(direction));
            }


            HexCell nextDirectionalNeighbor = HexGridVisualSystem.Instance.GetHexCell(cell.GetNextDirectionHexNeighbor(direction));
            if (direction <= HexCellDirections.E && nextDirectionalNeighbor != null)
            {
                var nextDirectionalNeighborBridgeVector = e1.v5 + HexMetric.GetBridge(HexMetric.GetNextDirection(direction));
                nextDirectionalNeighborBridgeVector.y = nextDirectionalNeighbor.transform.position.y;


                if (cell.GetElevation() <= directionalNeighbor.GetElevation())
                {
                    if (cell.GetElevation() <= nextDirectionalNeighbor.GetElevation())
                    {
                        TriangulateCorner
                        (
                            e1.v5, cell,
                            e2.v5, directionalNeighbor,
                            nextDirectionalNeighborBridgeVector, nextDirectionalNeighbor
                        );
                    }
                    else
                    {
                        TriangulateCorner
                        (
                            nextDirectionalNeighborBridgeVector, nextDirectionalNeighbor,
                            e1.v5, cell,
                            e2.v5, directionalNeighbor
                        );
                    }
                }
                else if (directionalNeighbor.GetElevation() <= nextDirectionalNeighbor.GetElevation())
                {
                    TriangulateCorner
                    (
                        e2.v5, directionalNeighbor,
                        nextDirectionalNeighborBridgeVector, nextDirectionalNeighbor,
                        e1.v5, cell

                    );
                }
                else
                {
                    TriangulateCorner
                    (
                        nextDirectionalNeighborBridgeVector, nextDirectionalNeighbor,
                         e1.v5, cell,
                         e2.v5, directionalNeighbor
                    );
                }
            }
        }
        void TriangulateCorner(Vector3 bottom, HexCell bottomCell, Vector3 left, HexCell leftCell, Vector3 right, HexCell rightCell)
        {
            HexEdgeType leftEdgeType = bottomCell.GetEdgeType(leftCell);
            HexEdgeType rightEdgeType = bottomCell.GetEdgeType(rightCell);

            if (HexMetric.GetEdgeType(leftEdgeType) == HexEdgeType.Slope)
            {
                if (rightEdgeType == leftEdgeType)
                {
                    TriangulateCornerTerraces
                    (
                        bottom, bottomCell, left, leftCell, right, rightCell
                    );
                    return;
                }
                if (HexMetric.GetEdgeType(rightEdgeType) == HexEdgeType.Flat)
                {
                    TriangulateCornerTerraces
                    (
                        left, leftCell, right, rightCell, bottom, bottomCell
                    );
                    return;
                }
                if (leftEdgeType == HexEdgeType.RisingSlope)
                {
                    if (rightEdgeType == HexEdgeType.DescendingSlope)
                    {
                        TriangulateCornerSlopeCliffTerraces(right, rightCell, bottom, bottomCell, left, leftCell);
                        return;
                    }
                    if (HexMetric.GetEdgeType(rightEdgeType) == HexEdgeType.Cliff)
                    {
                        TriangulateCornerSlopeCliffTerraces
                        (
                            bottom, bottomCell, left, leftCell, right, rightCell
                        );
                        return;
                    }

                }

            }
            if (HexMetric.GetEdgeType(rightEdgeType) == HexEdgeType.Slope)
            {
                if (rightEdgeType == leftEdgeType)
                {
                    TriangulateCornerTerraces
                    (
                        bottom, bottomCell, left, leftCell, right, rightCell
                    );
                    return;
                }
                if (HexMetric.GetEdgeType(leftEdgeType) == HexEdgeType.Flat)
                {
                    TriangulateCornerTerraces
                    (
                        right, rightCell, bottom, bottomCell, left, leftCell
                    );
                    return;
                }
                if (rightEdgeType == HexEdgeType.RisingSlope)
                {
                    if (HexMetric.GetEdgeType(leftEdgeType) == HexEdgeType.Cliff)
                    {
                        TriangulateCornerCliffSlopeTerraces
                        (
                            bottom, bottomCell, left, leftCell, right, rightCell
                        );
                        return;
                    }
                }

            }
            if (HexMetric.GetEdgeType(leftEdgeType) == HexEdgeType.Cliff)
            {
                if (HexMetric.GetEdgeType(rightEdgeType) == HexEdgeType.Cliff)
                {
                    TriangulateCornerCliffCliff
                    (
                        bottom, bottomCell, left, leftCell, right, rightCell
                    );
                    return;
                }
            }
            if (HexMetric.GetEdgeType(rightEdgeType) == HexEdgeType.Cliff)
            {

            }
            terrain.AddTriangle(bottom, left, right);
            terrain.AddTriangleColor(bottomCell.GetCellColor(), leftCell.GetCellColor(), rightCell.GetCellColor());
        }

        //hex terrain triangulation
        void TriangulateEdgeTerraces(EdgeVertices start, HexCell startCell, EdgeVertices end, HexCell endCell, bool hasRoad)
        {
            Vector3 v3 = HexMetric.TerraceLerp(start.v1, end.v1, 1);
            Vector3 v4 = HexMetric.TerraceLerp(start.v5, end.v5, 1);
            Color c2 = HexMetric.TerraceLerp(startCell.GetCellColor(), endCell.GetCellColor(), 1);

            EdgeVertices e2 = EdgeVertices.TerraceLerp(start, end, 1);
            TriangulateEdgeStrip(start, startCell.GetCellColor(), e2, c2, hasRoad);
            for (int i = 2; i < HexMetric.terraceSteps; i++)
            {
                EdgeVertices e1 = e2;
                Color c1 = c2;
                e2 = EdgeVertices.TerraceLerp(start, end, i);
                c2 = HexMetric.TerraceLerp(startCell.GetCellColor(), endCell.GetCellColor(), i);
                TriangulateEdgeStrip(e1, c1, e2, c2, hasRoad);
            }
            TriangulateEdgeStrip(e2, c2, end, endCell.GetCellColor(), hasRoad);
        }
        private void TriangulateCornerCliffCliff(Vector3 bottom, HexCell bottomCell,Vector3 left, HexCell leftCell,Vector3 right, HexCell rightCell)
        {
            if (leftCell.GetEdgeType(rightCell) == HexEdgeType.DescendingSlope)
            {
                TriangulateCornerSlopeCliffTerraces
                (
                     left, leftCell, right, rightCell, bottom, bottomCell
                );
            }
            if (leftCell.GetEdgeType(rightCell) == HexEdgeType.RisingSlope)
            {
                TriangulateCornerCliffSlopeTerraces
                (
                    right, rightCell, bottom, bottomCell, left, leftCell
                );
            }
            else
            {
                terrain.AddTriangle(bottom, left, right);
                terrain.AddTriangleColor(bottomCell.GetCellColor(), leftCell.GetCellColor(), rightCell.GetCellColor());
            }
        }
        void TriangulateCornerTerraces(Vector3 begin, HexCell beginCell,Vector3 left, HexCell leftCell,Vector3 right, HexCell rightCell)
        {
            Vector3 v3 = HexMetric.TerraceLerp(begin, left, 1);
            Vector3 v4 = HexMetric.TerraceLerp(begin, right, 1);
            Color c3 = HexMetric.TerraceLerp(beginCell.GetCellColor(), leftCell.GetCellColor(), 1);
            Color c4 = HexMetric.TerraceLerp(beginCell.GetCellColor(), rightCell.GetCellColor(), 1);

            terrain.AddTriangle(begin, v3, v4);
            terrain.AddTriangleColor(beginCell.GetCellColor(), c3, c4);

            for (int i = 2; i < HexMetric.terraceSteps; i++)
            {
                Vector3 v1 = v3;
                Vector3 v2 = v4;
                Color c1 = c3;
                Color c2 = c4;
                v3 = HexMetric.TerraceLerp(begin, left, i);
                v4 = HexMetric.TerraceLerp(begin, right, i);
                c3 = HexMetric.TerraceLerp(beginCell.GetCellColor(), leftCell.GetCellColor(), i);
                c4 = HexMetric.TerraceLerp(beginCell.GetCellColor(), rightCell.GetCellColor(), i);
                terrain.AddQuad(v1, v2, v3, v4);
                terrain.AddQuadColor(c1, c2, c3, c4);
            }

            terrain.AddQuad(v3, v4, left, right);
            terrain.AddQuadColor(c3, c4, leftCell.GetCellColor(), rightCell.GetCellColor());
        }
        void TriangulateCornerSlopeCliffTerraces(Vector3 start, HexCell startCell, Vector3 left, HexCell leftCell, Vector3 right, HexCell rightCell)
        {

            float b = 1f / (rightCell.GetElevation() - startCell.GetElevation());
            b = Mathf.Abs(b);
            Vector3 boundary = Vector3.Lerp(HexMetric.Perturb(start), HexMetric.Perturb(right), b);
            Color boundaryColor = Color.Lerp(startCell.GetCellColor(), rightCell.GetCellColor(), b);

            TriangulateBoundaryTriangle
            (
                start, startCell, left, leftCell, boundary, boundaryColor
            );
            if (HexMetric.GetEdgeType(leftCell.GetEdgeType(rightCell)) == HexEdgeType.Slope)
            {
                TriangulateBoundaryTriangle(
                    left, leftCell, right, rightCell, boundary, boundaryColor
                );
            }
            else
            {
                terrain.AddTriangleUnperturbed(HexMetric.Perturb(left), HexMetric.Perturb(right), boundary);
                terrain.AddTriangleColor(leftCell.GetCellColor(), rightCell.GetCellColor(), boundaryColor);
            }
        }
        void TriangulateCornerCliffSlopeTerraces(Vector3 begin, HexCell beginCell,Vector3 left, HexCell leftCell,Vector3 right, HexCell rightCell)
        {
            float b = 1f / (leftCell.GetElevation() - beginCell.GetElevation());
            b = Mathf.Abs(b);
            Vector3 boundary = Vector3.Lerp(HexMetric.Perturb(begin), HexMetric.Perturb(left), b);

            Color boundaryColor = Color.Lerp(beginCell.GetCellColor(), leftCell.GetCellColor(), b);

            TriangulateBoundaryTriangle
            (
                right, rightCell, begin, beginCell, boundary, boundaryColor
            );
            var leftToRight = leftCell.GetEdgeType(rightCell);
            if (HexMetric.GetEdgeType(leftToRight) == HexEdgeType.Slope)
            {
                TriangulateBoundaryTriangle(
                     left, leftCell, right, rightCell, boundary, boundaryColor
                );
            }
            else
            {
                terrain.AddTriangleUnperturbed(HexMetric.Perturb(left), HexMetric.Perturb(right), boundary);
                terrain.AddTriangleColor(leftCell.GetCellColor(), rightCell.GetCellColor(), boundaryColor);
            }
        }
        void TriangulateBoundaryTriangle(Vector3 begin, HexCell beginCell,Vector3 left, HexCell leftCell,Vector3 boundary, Color boundaryColor)
        {
            Vector3 v2 = HexMetric.Perturb(HexMetric.TerraceLerp(begin, left, 1));
            Color c2 = HexMetric.TerraceLerp(beginCell.GetCellColor(), leftCell.GetCellColor(), 1);

            terrain.AddTriangleUnperturbed(HexMetric.Perturb(begin), v2, boundary);
            terrain.AddTriangleColor(beginCell.GetCellColor(), c2, boundaryColor);

            for (int i = 2; i < HexMetric.terraceSteps; i++)
            {
                Vector3 v1 = v2;
                Color c1 = c2;
                v2 = HexMetric.Perturb(HexMetric.TerraceLerp(begin, left, i));
                c2 = HexMetric.TerraceLerp(beginCell.GetCellColor(), leftCell.GetCellColor(), i);
                terrain.AddTriangleUnperturbed(v1, v2, boundary);
                terrain.AddTriangleColor(c1, c2, boundaryColor);
            }

            terrain.AddTriangleUnperturbed(v2, HexMetric.Perturb(left), boundary);
            terrain.AddTriangleColor(c2, leftCell.GetCellColor(), boundaryColor);
        }
        void TriangulateBoundaryTriangleOther(Vector3 begin, HexCell beginCell,Vector3 left, HexCell leftCell,Vector3 boundary, Color boundaryColor)
        {
            Vector3 v2 = HexMetric.TerraceLerp(begin, left, 1);
            Color c2 = HexMetric.TerraceLerp(beginCell.GetCellColor(), leftCell.GetCellColor(), 1);
            terrain.AddTriangle(begin, v2, boundary);
            terrain.AddTriangleColor(beginCell.GetCellColor(), c2, boundaryColor);
            for (int i = 2; i < HexMetric.terraceSteps; i++)
            {
                Vector3 v1 = v2;
                Color c1 = c2;
                v2 = HexMetric.TerraceLerp(begin, left, i);
                c2 = HexMetric.TerraceLerp(beginCell.GetCellColor(), leftCell.GetCellColor(), i);
                terrain.AddTriangle(v1, v2, boundary);
                terrain.AddTriangleColor(c1, c2, boundaryColor);
            }

            terrain.AddTriangle(v2, left, boundary);
            terrain.AddTriangleColor(c2, leftCell.GetCellColor(), boundaryColor);
        }
        void TriangulateEdgeStrip (EdgeVertices e1, Color c1, EdgeVertices e2, Color c2, bool hasRoad = false)
        {
            terrain.AddQuad(e1.v1, e1.v2, e2.v1, e2.v2);
            terrain.AddQuadColor(c1, c2);
            terrain.AddQuad(e1.v2, e1.v3, e2.v2, e2.v3);
            terrain.AddQuadColor(c1, c2);
            terrain.AddQuad(e1.v3, e1.v4, e2.v3, e2.v4);
            terrain.AddQuadColor(c1, c2);
            terrain.AddQuad(e1.v4, e1.v5, e2.v4, e2.v5);
            terrain.AddQuadColor(c1, c2);
            if (hasRoad)
            {
                TriangulateRoadSegment(e1.v2, e1.v3, e1.v4, e2.v2, e2.v3, e2.v4);
            }
        }

        //hex river exclusion terrain triangulation
        void TriangulateWithoutRiver(HexCellDirections direction, HexCell cell, Vector3 center, EdgeVertices e)
        {
            TriangulateEdgeFan(center, e, cell.GetCellColor());

            if (cell.HasRoads)
            {
                Vector2 interpolators = GetRoadInterpolators(direction, cell);
                TriangulateRoad(
                    center,
                    Vector3.Lerp(center, e.v1, interpolators.x),
                    Vector3.Lerp(center, e.v5, interpolators.y),
                    e,
                    cell.HasRoadThroughEdge(direction)
                );
            }
        }

        //hex river triangulation
        void TriangulateWithRiver(HexCellDirections direction, HexCell cell,Vector3 center,EdgeVertices e)
        {
            Vector3 centerL, centerR;
            if (cell.HasRiverThroughEdge(direction.GetOppositeDirection()))
            {
                centerL = center +
                    HexMetric.GetFirstSolidCorner(direction.GetPreviousDirection()) * 0.25f;
                centerR = center +
                    HexMetric.GetSecondSolidCorner(direction.GetNextDirection()) * 0.25f;
            }
            else if (cell.HasRiverThroughEdge(direction.GetNextDirection()))
            {
                centerL = center;
                centerR = Vector3.Lerp(center, e.v5, 2f / 3f);
            }
            else if (cell.HasRiverThroughEdge(direction.GetPreviousDirection()))
            {
                centerL = Vector3.Lerp(center, e.v1, 2f / 3f);
                centerR = center;
            }
            else if (cell.HasRiverThroughEdge(direction.Next2()))
            {
                centerL = center;
                centerR = center +
                    HexMetric.GetSolidEdgeMiddle(direction.GetNextDirection()) * (0.5f * HexMetric.innerToOuter); ;
            }
            else
            {
                centerL = center +
                HexMetric.GetSolidEdgeMiddle(direction.GetPreviousDirection()) * (0.5f * HexMetric.innerToOuter); ;
                centerR = center;
            }

            center = Vector3.Lerp(centerL, centerR, 0.5f);
            EdgeVertices m = new EdgeVertices
            (
                Vector3.Lerp(centerL, e.v1, 0.5f),
                Vector3.Lerp(centerR, e.v5, 0.5f),
                1f / 6f
            );

            m.v3.y = center.y = e.v3.y;

            TriangulateEdgeStrip(m, cell.GetCellColor(), e, cell.GetCellColor());
            terrain.AddTriangle(centerL, m.v1, m.v2);
            terrain.AddTriangleColor(cell.GetCellColor());
            terrain.AddQuad(centerL, center, m.v2, m.v3);
            terrain.AddQuadColor(cell.GetCellColor());
            terrain.AddQuad(center, centerR, m.v3, m.v4);
            terrain.AddQuadColor(cell.GetCellColor());
            terrain.AddTriangle(centerR, m.v4, m.v5);
            terrain.AddTriangleColor(cell.GetCellColor());
            if (!cell.IsUnderwater)
            {
                bool reversed = cell.IncomingRiver == direction;
                TriangulateRiverQuad(
                    centerL, centerR, m.v2, m.v4, cell.RiverSurfaceY, 0.4f, reversed
                );
                TriangulateRiverQuad(
                    m.v2, m.v4, e.v2, e.v4, cell.RiverSurfaceY, 0.6f, reversed
                ); 
            }
        }
        void TriangulateAdjacentToRiver(HexCellDirections direction,HexCell cell,Vector3 center,EdgeVertices e)
        {
            if (cell.HasRoads)
            {
                TriangulateRoadAdjacentToRiver(direction, cell, center, e);
            }
            if (cell.HasRiverThroughEdge(direction.GetNextDirection()))
            {
                if (cell.HasRiverThroughEdge(direction.GetPreviousDirection()))
                {
                    center += HexMetric.GetSolidEdgeMiddle(direction) *
                        (HexMetric.innerToOuter * 0.5f);
                }
                else if (cell.HasRiverThroughEdge(direction.Previous2()))
                {
                    center += HexMetric.GetFirstSolidCorner(direction) * 0.25f;
                }
            }
            else if (cell.HasRiverThroughEdge(direction.GetPreviousDirection()) && cell.HasRiverThroughEdge(direction.Next2()))
            {
                center += HexMetric.GetSecondSolidCorner(direction) * 0.25f;
            }
            EdgeVertices m = new EdgeVertices
            (
                Vector3.Lerp(center, e.v1, 0.5f),
                Vector3.Lerp(center, e.v5, 0.5f)
            );

            TriangulateEdgeStrip(m, cell.GetCellColor(), e, cell.GetCellColor());
            TriangulateEdgeFan(center, m, cell.GetCellColor());
        }
        void TriangulateWithRiverBeginOrEnd(HexCellDirections direction,HexCell cell,Vector3 center,EdgeVertices e)
        {
            EdgeVertices m = new EdgeVertices
            (
                Vector3.Lerp(center, e.v1, 0.5f),
                Vector3.Lerp(center, e.v5, 0.5f)
            );

            m.v3.y = e.v3.y;
            TriangulateEdgeStrip(m, cell.GetCellColor(), e, cell.GetCellColor());
            TriangulateEdgeFan(center, m, cell.GetCellColor());
            if (!cell.IsUnderwater)
            {
                bool reversed = cell.HasIncomingRiver;
                TriangulateRiverQuad(
                    m.v2, m.v4, e.v2, e.v4, cell.RiverSurfaceY, 0.6f, reversed
                );
                center.y = m.v2.y = m.v4.y = cell.RiverSurfaceY;
                rivers.AddTriangle(center, m.v2, m.v4);
                if (reversed)
                {
                    rivers.AddTriangleUV(
                        new Vector2(0.5f, 0.4f),
                        new Vector2(1f, 0.2f), new Vector2(0f, 0.2f)
                    );
                }
                else
                {
                    rivers.AddTriangleUV(
                        new Vector2(0.5f, 0.4f),
                        new Vector2(0f, 0.6f), new Vector2(1f, 0.6f)
                    );
                }
            }
        }
        void TriangulateRiverQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4,float y, float v, bool reversed)
        {
            TriangulateRiverQuad(v1, v2, v3, v4, y, y, v, reversed);
        }
        void TriangulateRiverQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4,float y1, float y2, float v, bool reversed)
        {
            v1.y = v2.y = y1;
            v3.y = v4.y = y2;
            rivers.AddQuad(v1, v2, v3, v4);
            if (reversed)
            {
                rivers.AddQuadUV(1f, 0f, 0.8f - v, 0.6f - v);
            }
            else
            {
                rivers.AddQuadUV(0f, 1f, v, v + 0.2f);
            }
        }

        //hex river and road triangulation
        void TriangulateRoadAdjacentToRiver(HexCellDirections direction, HexCell cell, Vector3 center, EdgeVertices e)
        {
            bool hasRoadThroughEdge = cell.HasRoadThroughEdge(direction);
            bool previousHasRiver = cell.HasRiverThroughEdge(direction.GetPreviousDirection());
            bool nextHasRiver = cell.HasRiverThroughEdge(direction.GetNextDirection());
            Vector2 interpolators = GetRoadInterpolators(direction, cell);
            Vector3 roadCenter = center;
            if (cell.HasRiverBeginOrEnd)
            {
                roadCenter += HexMetric.GetSolidEdgeMiddle(
                    cell.RiverBeginOrEndDirection.GetOppositeDirection()
                ) * (1f / 3f);
            }
            else if (cell.IncomingRiver == cell.OutgoingRiver.GetOppositeDirection())
            {
                Vector3 corner;
                if (previousHasRiver)
                {
                    if (!hasRoadThroughEdge && !cell.HasRoadThroughEdge(direction.GetNextDirection()))
                    {
                        return;
                    }
                    corner = HexMetric.GetSecondSolidCorner(direction);
                }
                else
                {
                    if (!hasRoadThroughEdge && !cell.HasRoadThroughEdge(direction.GetPreviousDirection())
)
                    {
                        return;
                    }
                    corner = HexMetric.GetFirstSolidCorner(direction);
                }
                roadCenter += corner * 0.5f;
                center += corner * 0.25f;
            }
            else if (cell.IncomingRiver == cell.OutgoingRiver.GetPreviousDirection())
            {
                roadCenter -= HexMetric.GetSecondCorner(cell.IncomingRiver) * 0.2f;
            }
            else if (cell.IncomingRiver == cell.OutgoingRiver.GetNextDirection())
            {
                roadCenter -= HexMetric.GetFirstCorner(cell.IncomingRiver) * 0.2f;
            }
            else if (previousHasRiver && nextHasRiver)
            {
                if (!hasRoadThroughEdge)
                {
                    return;
                }
                Vector3 offset = HexMetric.GetSolidEdgeMiddle(direction) * HexMetric.innerToOuter;
                roadCenter += offset * 0.7f;
                center += offset * 0.5f;
            }
            else
            {
                HexCellDirections middle;
                if (previousHasRiver)
                {
                    middle = direction.GetNextDirection();
                }
                else if (nextHasRiver)
                {
                    middle = direction.GetPreviousDirection();
                }
                else
                {
                    middle = direction;
                }
                if (!cell.HasRoadThroughEdge(middle) && !cell.HasRoadThroughEdge(middle.GetPreviousDirection()) && !cell.HasRoadThroughEdge(middle.GetNextDirection())
)
                {
                    return;
                }
                roadCenter += HexMetric.GetSolidEdgeMiddle(middle) * 0.25f;
            }
            Vector3 mL = Vector3.Lerp(roadCenter, e.v1, interpolators.x);
            Vector3 mR = Vector3.Lerp(roadCenter, e.v5, interpolators.y);
            TriangulateRoad(roadCenter, mL, mR, e, hasRoadThroughEdge);
            if (previousHasRiver)
            {
                TriangulateRoadEdge(roadCenter, center, mL);
            }
            if (nextHasRiver)
            {
                TriangulateRoadEdge(roadCenter, mR, center);
            }
        }

        //hex road triangulation
        void TriangulateRoadSegment( Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector3 v5, Vector3 v6)
        {
            roads.AddQuad(v1, v2, v4, v5);
            roads.AddQuad(v2, v3, v5, v6);
            roads.AddQuadUV(0f, 1f, 0f, 0f);
            roads.AddQuadUV(1f, 0f, 0f, 0f);
        }
        void TriangulateRoad(Vector3 center, Vector3 mL, Vector3 mR, EdgeVertices e, bool hasRoadThroughCellEdge)
        {
            if (hasRoadThroughCellEdge)
            {
                Vector3 mC = Vector3.Lerp(mL, mR, 0.5f);
                TriangulateRoadSegment(mL, mC, mR, e.v2, e.v3, e.v4);
                roads.AddTriangle(center, mL, mC);
                roads.AddTriangle(center, mC, mR);
                roads.AddTriangleUV(new Vector2(1f, 0f), new Vector2(0f, 0f), new Vector2(1f, 0f));
                roads.AddTriangleUV(new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(0f, 0f));
            }
            else
            {
                TriangulateRoadEdge(center, mL, mR);
            }
        }
        void TriangulateRoadEdge(Vector3 center, Vector3 mL, Vector3 mR)
        {
            roads.AddTriangle(center, mL, mR);
            roads.AddTriangleUV(new Vector2(1f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f));
        }
        Vector2 GetRoadInterpolators(HexCellDirections direction, HexCell cell)
        {
            Vector2 interpolators;
            if (cell.HasRoadThroughEdge(direction))
            {
                interpolators.x = interpolators.y = 0.5f;
            }
            else
            {
                interpolators.x =
                    cell.HasRoadThroughEdge(direction.GetPreviousDirection()) ? 0.5f : 0.25f;
                interpolators.y =
                    cell.HasRoadThroughEdge(direction.GetNextDirection()) ? 0.5f : 0.25f;
            }
            return interpolators;
        }

        //hex river and water triangulation
        void TriangulateWaterfallInWater(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4,float y1, float y2, float waterY)
        {
            v1.y = v2.y = y1;
            v3.y = v4.y = y2;
            v1 = HexMetric.Perturb(v1);
            v2 = HexMetric.Perturb(v2);
            v3 = HexMetric.Perturb(v3);
            v4 = HexMetric.Perturb(v4);
            float t = (waterY - y2) / (y1 - y2);
            v3 = Vector3.Lerp(v3, v1, t);
            v4 = Vector3.Lerp(v4, v2, t);
            rivers.AddQuadUnperturbed(v1, v2, v3, v4);
            rivers.AddQuadUV(0f, 1f, 0.8f, 1f);
        }

        //hex water triangulation
        void TriangulateWater(HexCellDirections direction, HexCell cell, Vector3 center)
        {
            center.y = cell.WaterSurfaceY;
            Vector3 c1 = center + HexMetric.GetFirstSolidCorner(direction);
            Vector3 c2 = center + HexMetric.GetSecondSolidCorner(direction);

            HexCell neighbor = cell.GetHexCellNeighbor(direction);
            if (neighbor != null && !neighbor.IsUnderwater)
            {
                TriangulateWaterShore(direction, cell, neighbor, center);
            }
            else
            {
                TriangulateOpenWater(direction, cell, neighbor, center);
            }
        }
        void TriangulateOpenWater(HexCellDirections direction, HexCell cell, HexCell neighbor, Vector3 center)
        {
            center.y = cell.WaterSurfaceY;
            Vector3 c1 = center + HexMetric.GetFirstWaterCorner(direction);
            Vector3 c2 = center + HexMetric.GetSecondWaterCorner(direction);

            water.AddTriangle(center, c1, c2);
            if (direction <= HexCellDirections.SE && neighbor != null)
            {

                Vector3 bridge = HexMetric.GetWaterBridge(direction);
                Vector3 e1 = c1 + bridge;
                Vector3 e2 = c2 + bridge;

                water.AddQuad(c1, c2, e1, e2);

                if (direction <= HexCellDirections.E)
                {
                    HexCell nextNeighbor = cell.GetHexCellNeighbor(direction.GetNextDirection());
                    if (nextNeighbor == null || !nextNeighbor.IsUnderwater)
                    {
                        return;
                    }
                    water.AddTriangle(
                        c2, e2, c2 + HexMetric.GetWaterBridge(direction.GetNextDirection())
                    );

                }
            }
        }
        void TriangulateWaterShore(HexCellDirections direction, HexCell cell, HexCell neighbor, Vector3 center)
        {
            EdgeVertices e1 = new EdgeVertices(center + HexMetric.GetFirstWaterCorner(direction),center + HexMetric.GetSecondWaterCorner(direction));
            water.AddTriangle(center, e1.v1, e1.v2);
            water.AddTriangle(center, e1.v2, e1.v3);
            water.AddTriangle(center, e1.v3, e1.v4);
            water.AddTriangle(center, e1.v4, e1.v5);

            Vector3 center2 = neighbor.transform.position;
            center2.y = center.y;
            EdgeVertices e2 = new EdgeVertices(
                center2 + HexMetric.GetSecondSolidCorner(direction.GetOppositeDirection()),
                center2 + HexMetric.GetFirstSolidCorner(direction.GetOppositeDirection())
            );
            waterShore.AddQuad(e1.v1, e1.v2, e2.v1, e2.v2);
            waterShore.AddQuad(e1.v2, e1.v3, e2.v2, e2.v3);
            waterShore.AddQuad(e1.v3, e1.v4, e2.v3, e2.v4);
            waterShore.AddQuad(e1.v4, e1.v5, e2.v4, e2.v5);
            waterShore.AddQuadUV(0f, 0f, 0f, 1f);
            waterShore.AddQuadUV(0f, 0f, 0f, 1f);
            waterShore.AddQuadUV(0f, 0f, 0f, 1f);
            waterShore.AddQuadUV(0f, 0f, 0f, 1f);
            HexCell nextNeighbor = cell.GetHexCellNeighbor(direction.GetNextDirection());
            if (nextNeighbor != null)
            {
                Vector3 v3 = nextNeighbor.transform.position + 
                    (nextNeighbor.IsUnderwater ? 
                    HexMetric.GetFirstWaterCorner(direction.GetPreviousDirection()):
                    HexMetric.GetFirstSolidCorner(direction.GetPreviousDirection()));
                v3.y = center.y;
                waterShore.AddTriangle(e1.v5, e2.v5, v3);
                waterShore.AddTriangleUV(new Vector2(0f, 0f),new Vector2(0f, 1f),new Vector2(0f, nextNeighbor.IsUnderwater ? 0f : 1f));
            }
        }
        public void Refresh()
        {
            enabled = true;
        }
        public void AddCell(int index, HexCell cell)
        {
            hexCells[index] = cell;
            cell.transform.SetParent(transform);
        }

    }
}