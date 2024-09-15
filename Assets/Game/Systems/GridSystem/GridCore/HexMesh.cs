using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
            Vector3 v1 = center + cell.GetFirstSolidCorner(direction);
            Vector3 v2 = center + cell.GetSecondSolidCorner(direction);

            AddTriangle(center, v1, v2);
            AddTriangleColor(cell.GetCellColor());

            if(direction <= HexCellDirections.SE)
                TriangulateConnection(direction, cell, v1, v2);

        }
        void TriangulateConnection(HexCellDirections direction, HexCell cell, Vector3 v1, Vector3 v2)
        {
            var directionalNeighbor = HexGridVisualSystem.Instance.GetHexCell(cell.GetHexCellNeighborGridPosition(direction));
            if (directionalNeighbor == null) return;
            //rectangular bridge between hexagon tiles
            var bridge = cell.GetBridge(direction);

            Vector3 v3 = v1 + bridge;
            Vector3 v4 = v2 + bridge;

            AddBridge(direction, cell, v1, v2, v3, v4);

            var neighbor = HexGridVisualSystem.Instance.GetHexCell(cell.GetNextDirectionHexNeighbor(direction));
            if (direction <= HexCellDirections.E && neighbor != null)
                AddBridgeTriangles(direction, cell, v2, v4);
        }
        void AddBridge(HexCellDirections direction, HexCell cell, Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
        {
            var directionalNeighbor = HexGridVisualSystem.Instance.GetHexCell(cell.GetHexCellNeighborGridPosition(direction)) ?? cell;
            AddQuad(v1, v2, v3, v4);
            AddQuadColor(cell.GetCellColor(), cell.GetCellColor(), directionalNeighbor.GetCellColor(), directionalNeighbor.GetCellColor());
        }
        void AddBridgeTriangles(HexCellDirections direction, HexCell cell, Vector3 v1, Vector3 v2)
        {
            var directionalNeighbor = HexGridVisualSystem.Instance.GetHexCell(cell.GetHexCellNeighborGridPosition(direction));
            var nextDirectionalNeighbor = HexGridVisualSystem.Instance.GetHexCell(cell.GetNextDirectionHexNeighbor(direction)); 
            AddTriangle(v1, v2, v1 + cell.GetBridge(cell.GetNextDirection(direction)));
            AddTriangleColor(cell.GetCellColor(), directionalNeighbor.GetCellColor(), nextDirectionalNeighbor.GetCellColor());
        }

        void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            int vertexIndex = vertices.Count;
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);
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
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);
            vertices.Add(v4);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
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
    }
}

