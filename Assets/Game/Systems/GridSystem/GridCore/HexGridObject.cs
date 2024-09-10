using GameLab.UnitSystem;
using System.Collections.Generic;

namespace GameLab.GridSystem
{
    public class HexGridObject
    {
        HexGridSystem<HexGridObject> gridSystem;
        GridPosition gridPosition;
        List<object> objectList;
        List<GridPosition> neighborGridPositions = new();
        bool isOccupied;
        bool isInfluenced;


        public HexGridObject(HexGridSystem<HexGridObject> gridSystem, GridPosition gridPosition)        {
            this.gridSystem = gridSystem;
            this.gridPosition = gridPosition;
            objectList = new();
            isOccupied = false;
            isInfluenced = false;

        }
        public List<GridPosition> GetNeighborGridPositions()
        {
            return neighborGridPositions;
        }

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
            bool isOdd = gridPosition.z % 2 == 1;

            //northern neighbors
            var gp5 = new GridPosition(isOdd ? +1 : -1, +1) + gridPosition;
            var gp3 = new GridPosition(0, +1) + gridPosition;

            //center neighbor
            var gp1 = new GridPosition(-1, 0) + gridPosition;
            var gp2 = new GridPosition(+1, 0) + gridPosition;

            //south neighbor
            var gp6 = new GridPosition(isOdd ? +1 : -1, -1) + gridPosition;
            var gp4 = new GridPosition(0, -1) + gridPosition;

            neighborGridPositions.Add(gp1);
            neighborGridPositions.Add(gp2);
            neighborGridPositions.Add(gp3);
            neighborGridPositions.Add(gp4);
            neighborGridPositions.Add(gp5);
            neighborGridPositions.Add(gp6);

            List<GridPosition> tempPositions = new();
            foreach (var item in neighborGridPositions)
            {
                if (LevelHexGridSystem.Instance.GridPositionIsValid(item))
                {
                    tempPositions.Add(item);
                }
            }
            neighborGridPositions.Clear();
            neighborGridPositions = tempPositions;
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

