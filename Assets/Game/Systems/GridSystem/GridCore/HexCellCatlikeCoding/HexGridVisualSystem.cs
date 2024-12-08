using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GameLab.GridSystem
{
    public class HexGridVisualSystem : MonoBehaviour
    {
        public static HexGridVisualSystem Instance { get; private set; }

        [SerializeField] Transform hexCellPrefab;
        [SerializeField] Transform gridPositionChunkPrefab;
        Dictionary<GridPosition, HexCell> gridPositionHexCellDictionary;
        List<HexCell> cells;
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
            HexMetric.colors = colors;
            HexMetric.noiseSource = noiseSource;
            HexMetric.InitializeHashGrid(seed);
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
        public void DestroyMap()
        {
            LevelHexGridSystem.Instance.DestroyGrid();
            if (chunks != null)
            {
                foreach (var item in chunks)
                {
                    Destroy(item.gameObject);
                }
            }
            if (gridPositionHexCellDictionary != null)
                gridPositionHexCellDictionary.Clear();
            if (cells != null)
                cells.Clear();
        }
        public bool CreateMap(int x, int z)
        {
            if (x <= 0 || x % HexMetric.chunkSizeX != 0 || z <= 0 || z % HexMetric.chunkSizeZ != 0)
            {
                Debug.LogError("Unsupported map size.");
                return false;
            }
            DestroyMap();
            cellCountX = x;
            cellCountZ = z;
            chunkCountX = cellCountX / HexMetric.chunkSizeX;
            chunkCountZ = cellCountZ / HexMetric.chunkSizeZ;
            LevelHexGridSystem.Instance.CreateGrid(cellCountX, cellCountZ);
            CreateChunks();
            CreateCells();
            RefreshChunks();
            return true;
        }
        private void Start()
        {
            CreateMap(25, 25);
        }
        private void CreateCells()
        {
            gridPositionHexCellDictionary = new();
            cells = new List<HexCell>();
            foreach (GridPosition gridPosition in LevelHexGridSystem.Instance.GetAllGridPositions())
            {
                Transform hexCellObject = Instantiate(hexCellPrefab, LevelHexGridSystem.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                var hexCell = hexCellObject.GetComponent<HexCell>();
                hexCell.SetGridPosition(gridPosition);
                gridPositionHexCellDictionary.Add(gridPosition, hexCell);
                cells.Add(hexCell);
            }
            hexCells = cells.ToArray();
            foreach (var item in gridPositionHexCellDictionary)
            {
                AddCellToChunk(item.Key.x, item.Key.z, item.Value);
            }
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

        public void Save(BinaryWriter writer)
        {
            writer.Write(cellCountX);
            writer.Write(cellCountZ);
            for (int i = 0; i < hexCells.Length; i++)
            {
                hexCells[i].Save(writer);
            }

        }

        public void Load(BinaryReader reader, int header)
        {
            int x = 20, z = 15;
            if (header >= 1)
            {
                x = reader.ReadInt32();
                z = reader.ReadInt32();
            }
            if (x != cellCountX || z != cellCountZ)
            {
                if (!CreateMap(x, z))
                {
                    return;
                }
            }
            CreateMap(reader.ReadInt32(), reader.ReadInt32());
            for (int i = 0; i < hexCells.Length; i++)
            {
                hexCells[i].Load(reader);
            }
            RefreshChunks();
        }

        public void RefreshChunks()
        {
            if (chunks == null) return;
            foreach (var item in chunks)
            {
                item.Refresh();
            }
        }
    }
}

