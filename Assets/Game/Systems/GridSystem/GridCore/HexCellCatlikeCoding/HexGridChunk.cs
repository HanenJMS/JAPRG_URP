using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameLab.GridSystem
{
    public class HexGridChunk : MonoBehaviour
    {
        HexCell[] hexCells;
        [SerializeField] HexMesh terrain, rivers;
        
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
            Triangulate(hexCells);
            terrain.Apply();
            rivers.Apply();
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
                TriangulateEdgeFan(center, e, cell.GetCellColor());
            }



            if (direction <= HexCellDirections.SE)
            {
                TriangulateConnection(direction, cell, e);
            }

        }
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
                TriangulateRiverQuad
                (
                    e1.v2, e1.v4, e2.v2, e2.v4,
                    cell.RiverSurfaceY, directionalNeighbor.RiverSurfaceY, 0.8f,
                    cell.HasIncomingRiver && cell.IncomingRiver == direction
                );
            }
            if (HexMetric.GetEdgeType(cell.GetEdgeType(direction)) == HexEdgeType.Slope)
            {
                //TriangulateEdgeTerraces(direction, cell, e1.v1, e1.v4, directionalNeighbor, e2.v1, e2.v4);
                TriangulateEdgeTerraces(e1, cell, e2, directionalNeighbor);
            }
            else
            {
                TriangulateEdgeStrip(e1, cell.GetCellColor(), e2, directionalNeighbor.GetCellColor());
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
        void TriangulateEdgeTerraces(EdgeVertices start, HexCell startCell, EdgeVertices end, HexCell endCell)
        {
            Vector3 v3 = HexMetric.TerraceLerp(start.v1, end.v1, 1);
            Vector3 v4 = HexMetric.TerraceLerp(start.v5, end.v5, 1);
            Color c2 = HexMetric.TerraceLerp(startCell.GetCellColor(), endCell.GetCellColor(), 1);

            EdgeVertices e2 = EdgeVertices.TerraceLerp(start, end, 1);
            TriangulateEdgeStrip(start, startCell.GetCellColor(), e2, c2);
            for (int i = 2; i < HexMetric.terraceSteps; i++)
            {
                EdgeVertices e1 = e2;
                Color c1 = c2;
                e2 = EdgeVertices.TerraceLerp(start, end, i);
                c2 = HexMetric.TerraceLerp(startCell.GetCellColor(), endCell.GetCellColor(), i);
                TriangulateEdgeStrip(e1, c1, e2, c2);
            }
            TriangulateEdgeStrip(e2, c2, end, endCell.GetCellColor());
        }
        void TriangulateCorner
            (
                Vector3 bottom, HexCell bottomCell,
                Vector3 left, HexCell leftCell,
                Vector3 right, HexCell rightCell
            )
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

        private void TriangulateCornerCliffCliff
        (
            Vector3 bottom, HexCell bottomCell,
            Vector3 left, HexCell leftCell,
            Vector3 right, HexCell rightCell
        )
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
        void TriangulateCornerTerraces
        (
            Vector3 begin, HexCell beginCell,
            Vector3 left, HexCell leftCell,
            Vector3 right, HexCell rightCell
        )
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

        void TriangulateCornerSlopeCliffTerraces
        (
            Vector3 start, HexCell startCell,
            Vector3 left, HexCell leftCell,
            Vector3 right, HexCell rightCell
        )
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
        void TriangulateCornerCliffSlopeTerraces
    (
        Vector3 begin, HexCell beginCell,
        Vector3 left, HexCell leftCell,
        Vector3 right, HexCell rightCell
    )
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

        void TriangulateBoundaryTriangle
        (
            Vector3 begin, HexCell beginCell,
            Vector3 left, HexCell leftCell,
            Vector3 boundary, Color boundaryColor
        )
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
        void TriangulateBoundaryTriangleOther
        (
            //right
            Vector3 begin, HexCell beginCell,
            //beginning
            Vector3 left, HexCell leftCell,
            Vector3 boundary, Color boundaryColor
        )
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

        void TriangulateEdgeStrip
        (
            EdgeVertices e1, Color c1,
            EdgeVertices e2, Color c2
        )
        {
            terrain.AddQuad(e1.v1, e1.v2, e2.v1, e2.v2);
            terrain.AddQuadColor(c1, c2);
            terrain.AddQuad(e1.v2, e1.v3, e2.v2, e2.v3);
            terrain.AddQuadColor(c1, c2);
            terrain.AddQuad(e1.v3, e1.v4, e2.v3, e2.v4);
            terrain.AddQuadColor(c1, c2);
            terrain.AddQuad(e1.v4, e1.v5, e2.v4, e2.v5);
            terrain.AddQuadColor(c1, c2);
        }
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
            bool reversed = cell.IncomingRiver == direction;
            TriangulateRiverQuad(
                centerL, centerR, m.v2, m.v4, cell.RiverSurfaceY, 0.4f, reversed
            );
            TriangulateRiverQuad(
                m.v2, m.v4, e.v2, e.v4, cell.RiverSurfaceY, 0.6f, reversed
            );
        }

        void TriangulateAdjacentToRiver
        (
            HexCellDirections direction,
            HexCell cell,
            Vector3 center,
            EdgeVertices e
        )
        {
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
        void TriangulateWithRiverBeginOrEnd
        (
            HexCellDirections direction,
            HexCell cell,
            Vector3 center,
            EdgeVertices e
        )
        {
            EdgeVertices m = new EdgeVertices
            (
                Vector3.Lerp(center, e.v1, 0.5f),
                Vector3.Lerp(center, e.v5, 0.5f)
            );

            m.v3.y = e.v3.y;
            TriangulateEdgeStrip(m, cell.GetCellColor(), e, cell.GetCellColor());
            TriangulateEdgeFan(center, m, cell.GetCellColor());
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
        void TriangulateRiverQuad
        (
            Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4,
            float y, float v, bool reversed
        )
        {
            TriangulateRiverQuad(v1, v2, v3, v4, y, y, v, reversed);
        }
        void TriangulateRiverQuad
        (
            Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4,
            float y1, float y2, float v, bool reversed
        )
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