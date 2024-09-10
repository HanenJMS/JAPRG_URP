using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.GridSystem
{
    public class GridSystem<TGridObject>
    {
        private const float hexVerticalOffsetMultiplier = 0.75f;
        private const float hexHorizontalOffsetMultiplier = 0.5f;
        int width, height;
        float cellSize;
        Dictionary<GridPosition, TGridObject> grid;
        List<GridPosition> allGridPositions;
        public GridSystem(int width, int height, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> CreateGridObject)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.grid = new Dictionary<GridPosition, TGridObject>();
            this.allGridPositions = new();
            for (int x = 0; x < width; x++)
            {
                int posX = x * (int)cellSize;
                for (int z = 0; z < height; z++)
                {
                    int posZ = z * (int)cellSize;
                    GridPosition gridPosition = new GridPosition(x, z);

                    grid.Add(gridPosition, CreateGridObject(this, gridPosition));
                    allGridPositions.Add(gridPosition);
                }
            }

        }
        public Vector3 GetWorldPosition(GridPosition gridPosition)
        {
            return new Vector3(gridPosition.x, 0, 0) * cellSize +
                    new Vector3(0, 0, gridPosition.z) * cellSize * hexVerticalOffsetMultiplier +
                (((gridPosition.z % 2) == 1) ? new Vector3(1, 0, 0) * cellSize * hexHorizontalOffsetMultiplier : Vector3.zero);
        }
        public GridPosition GetGridPosition(Vector3 worldPosition)
        {
            var roughGridPosition = new GridPosition(Mathf.RoundToInt(worldPosition.x / cellSize),
                                    Mathf.RoundToInt(worldPosition.z / cellSize / hexVerticalOffsetMultiplier));

            bool isOdd = roughGridPosition.z % 2 == 1;
            List<GridPosition> neighborGridPositionlist = new();

            var northEastGrid = new GridPosition(0, +1) + roughGridPosition;
            var northWestGrid = new GridPosition(isOdd ? +1 : -1, +1) + roughGridPosition;

            var westGrid = new GridPosition(-1, 0) + roughGridPosition;
            var eastGrid = new GridPosition(+1, 0) + roughGridPosition;


            var southWestGrid = new GridPosition(0, -1) + roughGridPosition;


            var gp6 = new GridPosition(isOdd ? +1 : -1, -1) + roughGridPosition;

            neighborGridPositionlist.Add(westGrid);
            neighborGridPositionlist.Add(eastGrid);
            neighborGridPositionlist.Add(northEastGrid);
            neighborGridPositionlist.Add(southWestGrid);
            neighborGridPositionlist.Add(northWestGrid);
            neighborGridPositionlist.Add(gp6);

            GridPosition currentClosestGridPosition = roughGridPosition;
            foreach (var item in neighborGridPositionlist)
            {
                if (Vector3.Distance(GetWorldPosition(item), worldPosition) < Vector3.Distance(GetWorldPosition(currentClosestGridPosition), worldPosition))
                {
                    currentClosestGridPosition = item;
                }
            }
            return currentClosestGridPosition;
        }
        public void CreateDebugObject(Transform debugPrefab)
        {
            GameObject parent = new("Grid");
            foreach (KeyValuePair<GridPosition, TGridObject> gridPosition in grid)
            {
                Transform debugobject = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition.Key), Quaternion.identity, parent.transform);
                GridDebugObject gridDebugObject = debugobject.GetComponent<GridDebugObject>();
                debugobject.localScale = new(cellSize / 2, 1, cellSize / 2);
                gridDebugObject.SetGridObject(gridPosition.Value);
            }
        }
        public TGridObject GetGridObject(GridPosition gridPosition)
        {
            return grid[gridPosition];
        }
        public bool isValidGridPosition(GridPosition gridPosition)
        {
            return grid.ContainsKey(gridPosition);
        }
        public List<GridPosition> GetAllGridPositions()
        {
            return allGridPositions;
        }
        public float GetGridCellSize()
        {
            return cellSize;
        }
    }

}
