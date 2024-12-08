using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.GridSystem
{
    public class HexGridSystem<TGridObject>
    {
        private const float hexVerticalOffsetMultiplier = 0.75f;
        private const float hexHorizontalOffsetMultiplier = 0.5f;

        float innerRadiusConstant = 0.866025404f;
        float innerRadius;
        int width, height;
        float cellSize;
        

        Dictionary<GridPosition, TGridObject> grid;
        List<GridPosition> allGridPositions;
        List<GridDebugObject> debugObjects;
        GameObject parent;
        public HexGridSystem(int width, int height, float cellSize, Func<HexGridSystem<TGridObject>, GridPosition, TGridObject> CreateGridObject)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.grid = new Dictionary<GridPosition, TGridObject>();
            this.allGridPositions = new();

            innerRadius = cellSize / 2 * innerRadiusConstant;
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);

                    grid.Add(gridPosition, CreateGridObject(this, gridPosition));
                    allGridPositions.Add(gridPosition);
                }
            }

        }
        public Vector3 GetWorldPosition(GridPosition gridPosition)
        {
            float x = (gridPosition.x + gridPosition.z * 0.5f - (int)(gridPosition.z / 2)) * (innerRadius * 2f);
            float z = gridPosition.z * (cellSize / 2 * 1.5f);
            return new(x, 0, z);
        }
        public GridPosition GetGridPosition(Vector3 worldPosition)
        {

            int z = Mathf.RoundToInt(worldPosition.z / (cellSize / 2 * 1.5f));

            int x = Mathf.RoundToInt
                (
                    ((((worldPosition.x / (innerRadius * 2f)) + (int)(z / 2)) / 0.5f) - z) * 0.5f
                );
            var gp = new GridPosition(x, z);

            var closestGp = gp;
            foreach (var item in LevelHexGridSystem.Instance.GetGridObject(gp).GetGridHexCellNeighbors())
            {
                
                if (Vector3.Distance(LevelHexGridSystem.Instance.GetWorldPosition(item.Value), worldPosition) < Vector3.Distance(LevelHexGridSystem.Instance.GetWorldPosition(closestGp), worldPosition))
                {
                    closestGp = item.Value;
                }
            }
            return closestGp;
            //float x = worldPosition.x / (innerRadiusConstant * 2f * cellSize/2);
            //float y = -x;
            //float offset = worldPosition.z / ((cellSize / 2) * 3f);
            //x -= offset;
            //y -= offset;
            //int iX = Mathf.RoundToInt(x);
            //int iY = Mathf.RoundToInt(y);
            //int iZ = Mathf.RoundToInt(-x - y);
            //if (iX + iY + iZ != 0)
            //{
            //    float dX = Mathf.Abs(x - iX);
            //    float dY = Mathf.Abs(y - iY);
            //    float dZ = Mathf.Abs(-x - y - iZ);

            //    if (dX > dY && dX > dZ)
            //    {
            //        iX = -iY - iZ;
            //    }
            //    else if (dZ > dY)
            //    {
            //        iZ = -iX - iY;
            //    }
            //}
            //return new(iX, iZ);
        }
        public void CreateDebugObject(Transform debugPrefab)
        {
            debugObjects = new();
            parent = new("Grid");
            foreach (KeyValuePair<GridPosition, TGridObject> gridPosition in grid)
            {
                Transform debugobject = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition.Key), Quaternion.identity, parent.transform);
                GridDebugObject gridDebugObject = debugobject.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(gridPosition.Value);
                debugObjects.Add(gridDebugObject);
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
        public List<GridDebugObject> GetDebugObjects()
        {
            return debugObjects;
        }
        public GameObject GetCurrentWorld()
        {
            return parent;
                
        }
    }

}
