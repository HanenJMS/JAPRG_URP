using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace GameLab.GridSystem
{
    public class HexGridVisualSystem : MonoBehaviour
    {
        public static HexGridVisualSystem Instance { get; private set; }

        [SerializeField] Transform gridPositionVisual;
        Dictionary<GridPosition, HexCell> gridPositionVisualList;
        Dictionary<GridPosition, Transform> gridObjectList;
        HexMesh hexMesh;
        //for HexGrid references
        float outerRadius;
        float innerRadiusConstant = 0.866025404f;
        public Color defaultColor = Color.white;
        public Color touchedColor = Color.magenta;
        HexCell[] hexCells;


        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            Instance = this;
            gridPositionVisualList = new();
            gridObjectList = new();
            hexMesh = GetComponentInChildren<HexMesh>();
        }
        private void Start()
        {
            foreach (GridPosition gridPosition in LevelHexGridSystem.Instance.GetAllGridPositions())
            {
                Transform gridVisualTransform = Instantiate(gridPositionVisual, LevelHexGridSystem.Instance.GetWorldPosition(gridPosition), Quaternion.identity, this.transform);

                var hexCell = gridVisualTransform.GetComponent<HexCell>();
                gridPositionVisualList.Add(gridPosition, hexCell);
                gridObjectList.Add(gridPosition, gridVisualTransform);
                hexCell.SetGridPosition(gridPosition);
            }

            List<HexCell> cells = new List<HexCell>();
            foreach (var item in gridPositionVisualList)
            {
                cells.Add(item.Value);
            }
            hexCells = cells.ToArray();

            foreach (var item in hexCells)
            {
                item.InitlializeCell();
            }
            //Material hexMeshMaterial = hexMesh.GetComponent<Renderer>().material;
            foreach(var item in hexCells)
            {
                //item.SetMaterial(hexMeshMaterial);
                item.SetColor(defaultColor);
            }
            InitializeElevation();
            Refresh();
            //InitializeElevation();


        }
        public void Refresh()
        {
            hexMesh.Triangulate(hexCells);
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
            if(gridPositionVisualList.ContainsKey(gp))
            {
                gridPositionVisualList[gp].SetColor(color);
            }
            hexMesh.Triangulate(hexCells);
        }

        public HexCell GetHexCell(GridPosition gp)
        {
            if(gridPositionVisualList.ContainsKey(gp))
            {
                return gridPositionVisualList[gp];
            }
            return null;
        }
    }
}

