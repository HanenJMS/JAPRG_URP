using System.Collections.Generic;
using UnityEngine;
namespace GameLab.GridSystem
{
    public class HexGridVisualSystem : MonoBehaviour
    {
        public static HexGridVisualSystem Instance { get; private set; }

        [SerializeField] Transform hexCellPrefab;
        [SerializeField] Transform gridPositionChunkPrefab;
        Dictionary<GridPosition, HexCell> gridPositionHexCellDictionary;
        //for HexGrid references
        float outerRadius;
        float innerRadiusConstant = 0.866025404f;
        public Color defaultColor = Color.white;
        public Color touchedColor = Color.magenta;
        public Color[] colors;
        HexGridChunk[] chunks;
        HexCell[] hexCells;
        int cellCountX, cellCountZ;
        int chunkCountX = 4, chunkCountZ = 3;
        [SerializeField] Texture2D noiseSource;
        [SerializeField] int seed;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            Instance = this;
            gridPositionHexCellDictionary = new();
            HexMetric.noiseSource = noiseSource;
            HexMetric.InitializeHashGrid(seed);
            HexMetric.colors = colors;
        }
        void OnEnable()
        {
            if (!HexMetric.noiseSource)
            {
                HexMetric.noiseSource = noiseSource;
                HexMetric.InitializeHashGrid(seed);
                HexMetric.colors = colors;
            }
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
                Transform hexCellObject = Instantiate(hexCellPrefab, LevelHexGridSystem.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                var hexCell = hexCellObject.GetComponent<HexCell>();
                hexCell.SetGridPosition(gridPosition);
                gridPositionHexCellDictionary.Add(gridPosition, hexCell);

            }

            List<HexCell> cells = new List<HexCell>();
            foreach (var item in gridPositionHexCellDictionary)
            {
                cells.Add(item.Value);
                AddCellToChunk(item.Key.x, item.Key.z, item.Value);
            }
            hexCells = cells.ToArray();

            foreach (var item in chunks)
            {
                item.InitializeCells();
            }


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
            foreach (var item in gridPositionHexCellDictionary)
            {
                SetHexCellElevation(item.Key);
            }
        }
        public void SetHexCellElevation(GridPosition gp)
        {
            var hexCell = gridPositionHexCellDictionary[gp];
            Vector3 pos = hexCell.transform.position;
            pos.y = hexCell.GetElevation();
            gridPositionHexCellDictionary[gp].transform.position = pos;

        }


        public HexCell GetHexCell(GridPosition gp)
        {
            if (gridPositionHexCellDictionary.ContainsKey(gp))
            {
                return gridPositionHexCellDictionary[gp];
            }
            return null;
        }
    }
}

