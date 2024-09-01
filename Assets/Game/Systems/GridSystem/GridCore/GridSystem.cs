
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameLab.GridSystem
{
    public class GridSystem<TGridObject>
    {
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
                for(int z = 0; z< height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                   
                    grid.Add(gridPosition, CreateGridObject(this, gridPosition));
                    allGridPositions.Add(gridPosition);
                }
            }

        }
        public Vector3 GetWorldPosition(GridPosition gridPosition)
        {
            return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
        }
        public GridPosition GetGridPosition(Vector3 worldPosition)
        {
            return new GridPosition(Mathf.RoundToInt(worldPosition.x / cellSize),
                                    Mathf.RoundToInt(worldPosition.z / cellSize));
        }
        public void CreateDebugObject(Transform debugPrefab)
        {
            GameObject parent = new("Grid");
            foreach (KeyValuePair<GridPosition, TGridObject> gridPosition in grid)
            {
                Transform debugobject = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition.Key), Quaternion.identity, parent.transform);
                GridDebugObject gridDebugObject = debugobject.GetComponent<GridDebugObject>();
                debugobject.localScale = new(cellSize/2, 1, cellSize/2);
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
