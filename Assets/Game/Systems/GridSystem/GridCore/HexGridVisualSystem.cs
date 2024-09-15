using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameLab.GridSystem
{
    public class HexGridVisualSystem : MonoBehaviour
    {
        public static HexGridVisualSystem Instance { get; private set; }

        [SerializeField] Transform gridPositionVisual;
        Dictionary<GridPosition, HexCell> gridPositionVisualList;
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
            hexMesh = GetComponentInChildren<HexMesh>();
        }
        private void Start()
        {
            foreach (GridPosition gridPosition in LevelHexGridSystem.Instance.GetAllGridPositions())
            {
                Transform gridVisualTransform = Instantiate(gridPositionVisual, LevelHexGridSystem.Instance.GetWorldPosition(gridPosition), Quaternion.identity, this.transform);

                var hexCell = gridVisualTransform.GetComponent<HexCell>();
                gridPositionVisualList.Add(gridPosition, hexCell);
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
            
            

            hexMesh.Triangulate(hexCells);
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

