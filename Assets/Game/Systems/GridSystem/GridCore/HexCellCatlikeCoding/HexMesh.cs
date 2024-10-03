using System.Collections.Generic;
using UnityEngine;

namespace GameLab.GridSystem
{


    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class HexMesh : MonoBehaviour
    {
        [SerializeField] Mesh hexMesh;
        MeshCollider meshCollider;
        List<Vector3> vertices;
        List<int> triangles;
        List<Color> colors;
        private void Awake()
        {

            GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
            meshCollider = gameObject.AddComponent<MeshCollider>();
            hexMesh.name = "Hex Mesh";
            vertices = new List<Vector3>();
            triangles = new List<int>();
            colors = new();
        }

        public void Triangulate(HexCell[] cells)
        {
            hexMesh.Clear();
            vertices.Clear();

            colors.Clear();
            triangles.Clear();
            for (int i = 0; i < cells.Length; i++)
            {
                Triangulate(cells[i]);
            }
            hexMesh.vertices = vertices.ToArray();
            hexMesh.colors = colors.ToArray();
            hexMesh.triangles = triangles.ToArray();
            hexMesh.RecalculateNormals();
            meshCollider.sharedMesh = hexMesh;

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
            center + cell.GetFirstSolidCorner(direction),
            center + cell.GetSecondSolidCorner(direction)
            );
            TriangulateEdgeFan(center, e, cell.GetCellColor());

            //Vector3 e1 = Vector3.Lerp(v1, v2, 1f / 3f);
            //Vector3 e2 = Vector3.Lerp(v1, v2, 2f / 3f);

            //AddTriangle(center, v1, e1);
            //AddTriangleColor(cell.GetCellColor());
            //AddTriangle(center, e1, e2);
            //AddTriangleColor(cell.GetCellColor());
            //AddTriangle(center, e2, v2);
            //AddTriangleColor(cell.GetCellColor());


            if (direction <= HexCellDirections.SE)
            {
                TriangulateConnection(direction, cell, e);
            }

        }

        void TriangulateConnection(HexCellDirections direction, HexCell cell, EdgeVertices e1)
        {
            var directionalNeighbor = HexGridVisualSystem.Instance.GetHexCell(cell.GetHexCellNeighborGridPosition(direction));
            if (directionalNeighbor == null) return;
            //rectangular bridge between hexagon tiles
            var directionalBridge = cell.GetBridge(direction);
            directionalBridge.y = directionalNeighbor.transform.position.y - cell.transform.position.y;
            EdgeVertices e2 = new EdgeVertices(
                e1.v1 + directionalBridge,
                e1.v4 + directionalBridge
            );
            //Vector3 directionalNeighborVectorLeft = homeCellLeftVector + directionalBridge;
            //Vector3 directionalNeighborVectorRight = homeCellRightVector + directionalBridge;
            //directionalNeighborVectorLeft.y = directionalNeighborVectorRight.y = directionalNeighbor.transform.position.y;

            //Vector3 e3 = Vector3.Lerp(directionalNeighborVectorLeft, directionalNeighborVectorRight, 1f / 3f);
            //Vector3 e4 = Vector3.Lerp(directionalNeighborVectorLeft, directionalNeighborVectorRight, 2f / 3f);

            if (HexMetric.GetEdgeType(cell.GetEdgeType(direction)) == HexEdgeType.Slope)
            {
                TriangulateEdgeTerraces(direction, cell, e1.v1, e1.v4, directionalNeighbor, e2.v1, e2.v4);
            }
            else
            {
                TriangulateEdgeStrip(e1, cell.GetCellColor(), e2, directionalNeighbor.GetCellColor());
                //AddQuad(e1.v1, e1, e2.v1, e3);
                //AddQuadColor(cell.GetCellColor(), directionalNeighbor.GetCellColor());
                //AddQuad(e1, e2, e3, e4);
                //AddQuadColor(cell.GetCellColor(), directionalNeighbor.GetCellColor());
                //AddQuad(e2, homeCellRightVector, e4, directionalNeighborVectorRight);
                //AddQuadColor(cell.GetCellColor(), directionalNeighbor.GetCellColor());
            }


            HexCell nextDirectionalNeighbor = HexGridVisualSystem.Instance.GetHexCell(cell.GetNextDirectionHexNeighbor(direction));
            if (direction <= HexCellDirections.E && nextDirectionalNeighbor != null)
            {
                var nextDirectionalNeighborBridgeVector = e1.v4 + cell.GetBridge(HexMetric.GetNextDirection(direction));
                nextDirectionalNeighborBridgeVector.y = nextDirectionalNeighbor.transform.position.y;


                if (cell.GetElevation() <= directionalNeighbor.GetElevation())
                {
                    if (cell.GetElevation() <= nextDirectionalNeighbor.GetElevation())
                    {
                        TriangulateCorner
                        (
                            e1.v4, cell,
                            e2.v4, directionalNeighbor,
                            nextDirectionalNeighborBridgeVector, nextDirectionalNeighbor
                        );
                    }
                    else
                    {
                        TriangulateCorner
                        (
                            nextDirectionalNeighborBridgeVector, nextDirectionalNeighbor,
                            e1.v4, cell,
                            e2.v4, directionalNeighbor
                        );
                    }
                }
                else if (directionalNeighbor.GetElevation() <= nextDirectionalNeighbor.GetElevation())
                {
                    TriangulateCorner
                    (
                        e2.v4, directionalNeighbor,
                        nextDirectionalNeighborBridgeVector, nextDirectionalNeighbor,
                        e1.v4, cell

                    );
                }
                else
                {
                    TriangulateCorner
                    (
                        nextDirectionalNeighborBridgeVector, nextDirectionalNeighbor,
                         e1.v4, cell,
                         e2.v4, directionalNeighbor
                    );
                }
            }
        }

        void TriangulateEdgeTerraces(HexCellDirections direction, HexCell startCell, Vector3 startLeft, Vector3 startRight, HexCell endCell, Vector3 endLeft, Vector3 endRight)
        {
            Vector3 v3 = HexMetric.TerraceLerp(startLeft, endLeft, 1);
            Vector3 v4 = HexMetric.TerraceLerp(startRight, endRight, 1);
            Color c2 = HexMetric.TerraceLerp(startCell.GetCellColor(), endCell.GetCellColor(), 1);
            AddQuad(startLeft, startRight, v3, v4);
            AddQuadColor(startCell.GetCellColor(), c2);
            for (int i = 2; i < startCell.GetTerraceSteps(); i++)
            {
                Vector3 v1 = v3;
                Vector3 v2 = v4;
                Color c1 = c2;
                v3 = HexMetric.TerraceLerp(startLeft, endLeft, i);
                v4 = HexMetric.TerraceLerp(startRight, endRight, i);
                c2 = HexMetric.TerraceLerp(startCell.GetCellColor(), endCell.GetCellColor(), i);
                AddQuad(v1, v2, v3, v4);
                AddQuadColor(c1, c2);
            }
            AddQuad(v3, v4, endLeft, endRight);
            AddQuadColor(c2, endCell.GetCellColor());
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
            AddTriangle(bottom, left, right);
            AddTriangleColor(bottomCell.GetCellColor(), leftCell.GetCellColor(), rightCell.GetCellColor());
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
                AddTriangle(bottom, left, right);
                AddTriangleColor(bottomCell.GetCellColor(), leftCell.GetCellColor(), rightCell.GetCellColor());
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

            AddTriangle(begin, v3, v4);
            AddTriangleColor(beginCell.GetCellColor(), c3, c4);

            for (int i = 2; i < beginCell.GetTerraceSteps(); i++)
            {
                Vector3 v1 = v3;
                Vector3 v2 = v4;
                Color c1 = c3;
                Color c2 = c4;
                v3 = HexMetric.TerraceLerp(begin, left, i);
                v4 = HexMetric.TerraceLerp(begin, right, i);
                c3 = HexMetric.TerraceLerp(beginCell.GetCellColor(), leftCell.GetCellColor(), i);
                c4 = HexMetric.TerraceLerp(beginCell.GetCellColor(), rightCell.GetCellColor(), i);
                AddQuad(v1, v2, v3, v4);
                AddQuadColor(c1, c2, c3, c4);
            }

            AddQuad(v3, v4, left, right);
            AddQuadColor(c3, c4, leftCell.GetCellColor(), rightCell.GetCellColor());
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
            Vector3 boundary = Vector3.Lerp(start, right, b);
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
                AddTriangle(left, right, boundary);
                AddTriangleColor(leftCell.GetCellColor(), rightCell.GetCellColor(), boundaryColor);
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
            Vector3 boundary = Vector3.Lerp(begin, left, b);
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
                AddTriangle(left, right, boundary);
                AddTriangleColor(leftCell.GetCellColor(), rightCell.GetCellColor(), boundaryColor);
            }
        }

        void TriangulateBoundaryTriangle
        (
            Vector3 begin, HexCell beginCell,
            Vector3 left, HexCell leftCell,
            Vector3 boundary, Color boundaryColor
        )
        {
            Vector3 v2 = HexMetric.TerraceLerp(begin, left, 1);
            Color c2 = HexMetric.TerraceLerp(beginCell.GetCellColor(), leftCell.GetCellColor(), 1);
            AddTriangle(begin, v2, boundary);
            AddTriangleColor(beginCell.GetCellColor(), c2, boundaryColor);
            for (int i = 2; i < HexMetric.terraceSteps; i++)
            {
                Vector3 v1 = v2;
                Color c1 = c2;
                v2 = HexMetric.TerraceLerp(begin, left, i);
                c2 = HexMetric.TerraceLerp(beginCell.GetCellColor(), leftCell.GetCellColor(), i);
                AddTriangle(v1, v2, boundary);
                AddTriangleColor(c1, c2, boundaryColor);
            }

            AddTriangle(v2, left, boundary);
            AddTriangleColor(c2, leftCell.GetCellColor(), boundaryColor);
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
            AddTriangle(begin, v2, boundary);
            AddTriangleColor(beginCell.GetCellColor(), c2, boundaryColor);
            for (int i = 2; i < HexMetric.terraceSteps; i++)
            {
                Vector3 v1 = v2;
                Color c1 = c2;
                v2 = HexMetric.TerraceLerp(begin, left, i);
                c2 = HexMetric.TerraceLerp(beginCell.GetCellColor(), leftCell.GetCellColor(), i);
                AddTriangle(v1, v2, boundary);
                AddTriangleColor(c1, c2, boundaryColor);
            }

            AddTriangle(v2, left, boundary);
            AddTriangleColor(c2, leftCell.GetCellColor(), boundaryColor);
        }
        void TriangulateEdgeFan(Vector3 center, EdgeVertices edge, Color color)
        {
            AddTriangle(center, edge.v1, edge.v2);
            AddTriangleColor(color);
            AddTriangle(center, edge.v2, edge.v3);
            AddTriangleColor(color);
            AddTriangle(center, edge.v3, edge.v4);
            AddTriangleColor(color);
        }
        void TriangulateEdgeStrip
        (
            EdgeVertices e1, Color c1,
            EdgeVertices e2, Color c2
        )
        {
            AddQuad(e1.v1, e1.v2, e2.v1, e2.v2);
            AddQuadColor(c1, c2);
            AddQuad(e1.v2, e1.v3, e2.v2, e2.v3);
            AddQuadColor(c1, c2);
            AddQuad(e1.v3, e1.v4, e2.v3, e2.v4);
            AddQuadColor(c1, c2);
        }
        void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            int vertexIndex = vertices.Count;
            vertices.Add(Perturb(v1));
            vertices.Add(Perturb(v2));
            vertices.Add(Perturb(v3));
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
        }

        void AddTriangleColor(Color color)
        {
            colors.Add(color);
            colors.Add(color);
            colors.Add(color);
        }
        void AddTriangleColor(Color c1, Color c2, Color c3)
        {
            colors.Add(c1);
            colors.Add(c2);
            colors.Add(c3);
        }

        void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
        {
            int vertexIndex = vertices.Count;
            vertices.Add(Perturb(v1));
            vertices.Add(Perturb(v2));
            vertices.Add(Perturb(v3));
            vertices.Add(Perturb(v4));
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex + 3);
        }
        void AddQuadColor(Color c1, Color c2, Color c3, Color c4)
        {
            colors.Add(c1);
            colors.Add(c2);
            colors.Add(c3);
            colors.Add(c4);
        }
        void AddQuadColor(Color c1, Color c2)
        {
            colors.Add(c1);
            colors.Add(c1);
            colors.Add(c2);
            colors.Add(c2);
        }
        Vector3 Perturb(Vector3 position)
        {
            Vector4 sample = HexMetric.SampleNoise(position);
            position.x += (sample.x * 2f - 1f) * HexMetric.cellPerturbStrength;
            //position.y += (sample.y * 2f - 1f) * HexMetric.cellPerturbStrength;
            position.z += (sample.z * 2f - 1f) * HexMetric.cellPerturbStrength;
            return position;
        }
    }
}

