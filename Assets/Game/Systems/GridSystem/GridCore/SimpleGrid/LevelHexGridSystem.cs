using GameLab.UnitSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.GridSystem
{
    public class LevelHexGridSystem : MonoBehaviour
    {
        public static LevelHexGridSystem Instance { get; private set; }
        public Action onUpdateGridPosition;
        [SerializeField] int width, height, cellsize;
        [SerializeField] Transform debugObject;
        HexGridSystem<HexGridObject> gridSystem;

        // Start is called before the first frame update
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;

            gridSystem = new HexGridSystem<HexGridObject>(width, height, cellsize, (HexGridSystem<HexGridObject> g, GridPosition gridPosition) => new(g, gridPosition));
            gridSystem.CreateDebugObject(debugObject);
            foreach (var item in GetAllGridPositions())
            {
                GetGridObject(item).InitializeNeighborGridPositionList();
            }
        }

        //Handling Unit
        public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
        {
            gridSystem.GetGridObject(gridPosition).AddObjectToGrid(unit);
        }
        public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
        {
            GetGridObject(gridPosition).RemoveObjectFromGrid(unit);
        }
        public void ChangingUnitGridPosition(GridPosition from, GridPosition to, Unit unit)
        {
            RemoveUnitAtGridPosition(from, unit);
            AddUnitAtGridPosition(to, unit);
            onUpdateGridPosition?.Invoke();
        }
        public List<object> GetObjectOnGridPosition(GridPosition gridPosition)
        {
            return gridSystem.GetGridObject(gridPosition).GetObjectList();
        }

        //GridSystem Exposed
        public HexGridObject GetGridObject(GridPosition gridPosition)
        {
            return gridSystem.GetGridObject(gridPosition);
        }
        public Vector3 GetWorldPosition(GridPosition gridPosition)
        {
            return gridSystem.GetWorldPosition(gridPosition);
        }
        public GridPosition GetGridPosition(Vector3 worldPosition)
        {
            return gridSystem.GetGridPosition(worldPosition);
        }
        public bool GridPositionIsValid(GridPosition gridPosition)
        {
            return gridSystem.isValidGridPosition(gridPosition);
        }
        public List<GridPosition> GetAllGridPositions()
        {
            return gridSystem.GetAllGridPositions();
        }
        public float GetGridCellSize()
        {
            return gridSystem.GetGridCellSize();
        }
    }
}

