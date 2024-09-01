using GameLab.UnitSystem;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.GridSystem
{
    public class GridObject
    {
        GridSystem<GridObject> gridSystem;
        GridPosition gridPosition;
        List<object> objectList;
        List<GridPosition> neighborGridPositions = new();
        bool isOccupied;
        public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)        {
            this.gridSystem = gridSystem;
            this.gridPosition = gridPosition;
            objectList = new();
            isOccupied = false;


        }
        public List<GridPosition> GetNeighborGridPositions() => neighborGridPositions;
        public void AddObjectToGrid(object gridObject)
        {
            objectList.Add(gridObject);
        }
        public void RemoveObjectFromGrid(object gridObject)
        {
            objectList.Remove(gridObject);
        }
        public bool HasObjectOnGrid()
        {
            return objectList.Count > 0;
        }
        public bool GetIsBlocked()
        {
            return isOccupied;
        }
        public void SetIsBlocked(bool isBlocked)
        {
            this.isOccupied = isBlocked;
        }
        public void SetObjectPriorityOnGrid(object newGridResident, int index)
        {
            if (objectList.Contains(newGridResident))
            {
                objectList.Remove(newGridResident);
            }
            objectList.Insert(index, newGridResident);
        }
        public List<object> GetObjectList()
        {
            return objectList;
        }
        public void InitializeNeighborGridPositionList()
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    var gp = new GridPosition(x, z);
                    var gpTest = gp + gridPosition;
                    if (gpTest == this.gridPosition) continue;
                    if (!LevelGridSystem.Instance.GridPositionIsValid(gpTest)) continue;
                    neighborGridPositions.Add(gp + gridPosition);

                }
            }
        }
        public override string ToString()
        {
            string unitOnGrid = "";
            foreach (object unit in objectList)
            {
                if (unit is Unit)
                {
                    unitOnGrid += (unit as Unit).gameObject.name + "\n";
                }
            }
            return gridPosition.ToString() + $"\n{unitOnGrid}";
        }
    }
}

