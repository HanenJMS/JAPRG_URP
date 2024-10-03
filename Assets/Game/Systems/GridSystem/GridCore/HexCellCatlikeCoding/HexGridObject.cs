using GameLab.UnitSystem;
using System.Collections.Generic;

namespace GameLab.GridSystem
{

    public class HexGridObject
    {
        HexGridSystem<HexGridObject> gridSystem;
        GridPosition gridPosition;
        List<object> objectList;
        Dictionary<HexCellDirections, GridPosition> hexCellNeighbors = new();
        bool isOccupied;
        bool isInfluenced;


        public HexGridObject(HexGridSystem<HexGridObject> gridSystem, GridPosition gridPosition)        {
            this.gridSystem = gridSystem;
            this.gridPosition = gridPosition;
            objectList = new();
            isOccupied = false;
            isInfluenced = false;

        }
        public Dictionary<HexCellDirections, GridPosition> GetGridHexCellNeighbors()
        {
            return hexCellNeighbors;
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
            var W = new GridPosition(-1, 0);
            var E = new GridPosition(+1, 0);
            //northern neighbors
            var NE = new GridPosition(1, 1);
            var SE = new GridPosition(1, -1);
            var SW = new GridPosition(0, -1);
            var NW = new GridPosition(0, 1);
            if (!isOdd)
            {
                NE = new GridPosition(0, 1);
                SE = new GridPosition(0, -1);
                SW = new GridPosition(-1, -1);
                NW = new GridPosition(-1, 1);
            }

            hexCellNeighbors.Add(HexCellDirections.NE, NE + gridPosition);
            hexCellNeighbors.Add(HexCellDirections.E, E + gridPosition);
            hexCellNeighbors.Add(HexCellDirections.SE, SE + gridPosition);
            hexCellNeighbors.Add(HexCellDirections.SW, SW + gridPosition);
            hexCellNeighbors.Add(HexCellDirections.W, W + gridPosition);
            hexCellNeighbors.Add(HexCellDirections.NW, NW + gridPosition);

            Dictionary<HexCellDirections, GridPosition> tempPositions = new();
            foreach (var item in hexCellNeighbors)
            {
                if (LevelHexGridSystem.Instance.GridPositionIsValid(item.Value))
                {
                    tempPositions.Add(item.Key, item.Value);
                }
            }
            hexCellNeighbors.Clear();
            hexCellNeighbors = tempPositions;
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

