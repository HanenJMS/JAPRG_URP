using System.Collections.Generic;
using UnityEngine;
namespace GameLab.GridSystem
{
    public class GridPositionVisual : MonoBehaviour
    {
        public static GridPositionVisual Instance { get; private set; }

        [SerializeField] Transform gridPositionVisual;
        Dictionary<GridPosition, MeshRenderer> gridPositionVisualList;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            Instance = this;
            gridPositionVisualList = new();
        }
        private void Start()
        {
            foreach (GridPosition gridPosition in LevelGridSystem.Instance.GetAllGridPositions())
            {
                Transform gridVisualTransform = Instantiate(gridPositionVisual, LevelGridSystem.Instance.GetWorldPosition(gridPosition), Quaternion.identity, this.transform);

                Vector3 newVisualScale = new(LevelGridSystem.Instance.GetGridCellSize(), 1, LevelGridSystem.Instance.GetGridCellSize());
                gridVisualTransform.localScale = newVisualScale;
                gridPositionVisualList.Add(gridPosition, gridVisualTransform.GetComponentInChildren<MeshRenderer>());
            }
            //UnitActionSystem.Instance.onSelectedUnit += ShowSelectedUnitSelectedActionVisual;
            //LevelGridSystem.Instance.onUpdateGridPosition += ShowSelectedUnitSelectedActionVisual;
            //UnitActionSystem.Instance.onSelectedAction += ShowSelectedUnitSelectedActionVisual;
            HideAllGridPosition();
        }
        void HideAllGridPosition()
        {
            foreach (KeyValuePair<GridPosition, MeshRenderer> grid in gridPositionVisualList)
            {
                grid.Value.enabled = false;
            }
        }
        void ShowSelectedUnitSelectedActionVisual()
        {
            //if (UnitActionSystem.Instance.GetSelectedAction() == null) return;
            //ShowGridPositions(UnitActionSystem.Instance.GetSelectedAction().GetValidTargetInRange());
        }
        public void ShowGridPositions(List<GridPosition> positions)
        {
            HideAllGridPosition();
            foreach (GridPosition gridPosition in positions)
            {
                if (gridPositionVisualList.ContainsKey(gridPosition))
                {
                    gridPositionVisualList[gridPosition].enabled = true;
                }
            }
        }
    }
}

