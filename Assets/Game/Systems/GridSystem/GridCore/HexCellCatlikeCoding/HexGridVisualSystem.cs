using System.Collections.Generic;
using UnityEngine;
namespace GameLab.GridSystem
{
    public class HexGridVisualSystem : MonoBehaviour
    {
        public static HexGridVisualSystem Instance { get; private set; }

        [SerializeField] Transform hexCellPrefab;
        [SerializeField] Transform gridPositionChunkPrefab;
        Dictionary<GridPosition, HexCell> gridPositionVisualList;
        Dictionary<GridPosition, Transform> gridObjectList;
        HexMesh hexMesh;
        //for HexGrid references
        float outerRadius;
        float innerRadiusConstant = 0.866025404f;
        public Color defaultColor = Color.white;
        public Color touchedColor = Color.magenta;
        HexGridChunk[] chunks;
        HexCell[] hexCells;
        int cellCountX, cellCountZ;
        int chunkCountX = 4, chunkCountZ = 3;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            Instance = this;
            gridPositionVisualList = new();
            gridObjectList = new();
            
        }
        void AddCellToChunk(int x, int z, HexCell cell)
        {
            int chunkX = x / HexMetric.chunkSizeX;
            int chunkZ = z / HexMetric.chunkSizeZ;
            HexGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];

            int localX = x - chunkX * HexMetric.chunkSizeX;
            int localZ = z - chunkZ * HexMetric.chunkSizeZ;
            chunk.AddCell(localX + localZ * HexMetric.chunkSizeX, cell);
            cell.SetCellChunkIndex(chunkX + chunkZ * chunkCountX);
            cell.SetCellChunk(chunk);
        }
        private void Start()
        {
            LevelHexGridSystem.Instance.CreateGrid(chunkCountX * HexMetric.chunkSizeX, chunkCountZ * HexMetric.chunkSizeZ);
            CreateChunks();
            foreach (GridPosition gridPosition in LevelHexGridSystem.Instance.GetAllGridPositions())
            {
                Transform hexCellObject = Instantiate(hexCellPrefab, LevelHexGridSystem.Instance.GetWorldPosition(gridPosition), Quaternion.identity, this.transform);
                var hexCell = hexCellObject.GetComponent<HexCell>();
                hexCell.SetGridPosition(gridPosition);
                AddCellToChunk(gridPosition.x, gridPosition.z, hexCell);
                gridPositionVisualList.Add(gridPosition, hexCell);
                gridObjectList.Add(gridPosition, hexCellObject.transform);
                
            }

            List<HexCell> cells = new List<HexCell>();
            foreach (var item in gridPositionVisualList)
            {
                cells.Add(item.Value);
            }
            hexCells = cells.ToArray();

            foreach (var item in hexCells)
            {
                item.SetElevation(0);
                item.InitlializeCell();
            }
            //Material hexMeshMaterial = hexMesh.GetComponent<Renderer>().material;
            foreach (var item in hexCells)
            {
                //item.SetMaterial(hexMeshMaterial);
                item.SetColor(defaultColor);
            }
            InitializeElevation();
            //InitializeElevation();


        }
        void CreateChunks()
        {
            chunks = new HexGridChunk[chunkCountX * chunkCountZ];

            for (int z = 0, i = 0; z < chunkCountZ; z++)
            {
                for (int x = 0; x < chunkCountX; x++)
                {
                    HexGridChunk chunk = chunks[i++] = Instantiate(gridPositionChunkPrefab, this.transform).GetComponent<HexGridChunk>();
                }
            }
        }
        public void Refresh(int chunkIndex)
        {
            chunks[chunkIndex].Refresh();
        }
        public void InitializeElevation()
        {
            foreach (var item in gridObjectList)
            {
                SetHexCellElevation(item.Key);
            }
        }
        public void SetHexCellElevation(GridPosition gp)
        {
            var hexCell = gridPositionVisualList[gp];
            Vector3 pos = hexCell.transform.position;
            pos.y = hexCell.GetElevation();
            gridObjectList[gp].transform.position = pos;

        }
        public void SetHexCellColor(GridPosition gp, Color color)
        {
            if (gridPositionVisualList.ContainsKey(gp))
            {
                gridPositionVisualList[gp].SetColor(color);
            }
            hexMesh.Triangulate(hexCells);
        }

        public HexCell GetHexCell(GridPosition gp)
        {
            if (gridPositionVisualList.ContainsKey(gp))
            {
                return gridPositionVisualList[gp];
            }
            return null;
        }
    }
}

