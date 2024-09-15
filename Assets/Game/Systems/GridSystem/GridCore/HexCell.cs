using System.Collections.Generic;
using UnityEditor.Build.Pipeline;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameLab.GridSystem
{

    public class HexCell : MonoBehaviour
    {
        Vector3[] corners;
        float outerRadius;
        float innerRadiusConstant = 0.866025404f;
        GridPosition gridPosition;
        Dictionary<HexCellDirections, GridPosition> hexCellNeighbors;
        Color color;
        float solidFactor = 0.75f;

        float blendFactor;
        /// <summary>
        /// Set corners and neighbor cells
        /// </summary>
        public void InitlializeCell()
        {
            blendFactor = 1f - solidFactor;
            InitializeCorners();
            InitializeNeighbors();
        }

        public void InitializeCorners()
        {
            outerRadius = LevelHexGridSystem.Instance.GetGridCellSize() / 2;
            float innerRadiusCalculated = outerRadius * innerRadiusConstant;
            Vector3[] corners =
            {
                    //0 North
                    new Vector3(0f, 0f, outerRadius),

                    //NorthEast
                    new Vector3(innerRadiusCalculated, 0f, 0.5f * outerRadius),
                    //120 SouthEast
                    new Vector3(innerRadiusCalculated, 0f, -0.5f * outerRadius),
                    //180 South
                    new Vector3(0f, 0f, -outerRadius),
                    //240 SouthWest
                    new Vector3(-innerRadiusCalculated, 0f, -0.5f * outerRadius),
                    //300 NorthWest
                    new Vector3(-innerRadiusCalculated, 0f, 0.5f * outerRadius),

                    //north edge cases
                    new Vector3(0f, 0f, outerRadius)
            };
            this.corners = corners;
        }

        public Vector3[] GetCorners()
        {
            return corners;
        }
        public void SetGridPosition(GridPosition gp)
        {
            gridPosition = new(gp.x, gp.z);
        }
        public void SetColor(Color color)
        {
            this.color = color;
        }
        public Color GetCellColor() => color;
        public void InitializeNeighbors()
        {
            hexCellNeighbors = new();
            bool isOdd = gridPosition.z % 2 == 1;
            var W = new GridPosition(-1, 0) + this.gridPosition;
            var E = new GridPosition(+1, 0) + this.gridPosition;
            //northern neighbors
            var NE = new GridPosition(1, 1) + this.gridPosition;
            var SE = new GridPosition(1, -1) + this.gridPosition;
            var SW = new GridPosition(0, -1) + this.gridPosition;
            var NW = new GridPosition(0, 1) + this.gridPosition;
            if (!isOdd)
            {
                NE = new GridPosition(0, 1) + this.gridPosition;
                SE = new GridPosition(0, -1) + this.gridPosition;
                SW = new GridPosition(-1, -1) + this.gridPosition;
                NW = new GridPosition(-1, 1) + this.gridPosition;
            }

            hexCellNeighbors.Add(HexCellDirections.NE, NE );
            hexCellNeighbors.Add(HexCellDirections.E, E);
            hexCellNeighbors.Add(HexCellDirections.SE, SE);
            hexCellNeighbors.Add(HexCellDirections.SW, SW);
            hexCellNeighbors.Add(HexCellDirections.W, W);
            hexCellNeighbors.Add(HexCellDirections.NW, NW);
            //Dictionary<HexCellDirections, GridPosition> tempPositions = new();
            //foreach (var item in hexCellNeighbors)
            //{
            //    if (LevelHexGridSystem.Instance.GridPositionIsValid(item.Value))
            //    {
            //        tempPositions.Add(item.Key, item.Value);
            //    }
            //}
            //hexCellNeighbors.Clear();
            //hexCellNeighbors = tempPositions;
        }
        public GridPosition GetHexCellNeighborGridPosition(HexCellDirections direction)
        {
            return hexCellNeighbors[direction];
        }
        public Vector3 GetFirstCorner(HexCellDirections direction)
        {
            return corners[(int)direction];
        }
        public Vector3 GetSecondCorner(HexCellDirections direction)
        {
            return corners[((int)direction + 1)];
        }

        public Vector3 GetFirstSolidCorner(HexCellDirections direction)
        {
            return corners[(int)direction] * solidFactor;
        }

        public Vector3 GetSecondSolidCorner(HexCellDirections direction)
        {
            return corners[(int)direction + 1] * solidFactor;
        }

        public GridPosition GetPreviousDirectionHexNeighbor(HexCellDirections directions)
        {
            var PrevDirection = GetPreviousDirection(directions);
            return hexCellNeighbors[PrevDirection];
        }
        public GridPosition GetNextDirectionHexNeighbor(HexCellDirections directions)
        {
            var nextDirection = GetNextDirection(directions);
            return hexCellNeighbors[nextDirection];
        }
        public HexCellDirections GetNextDirection(HexCellDirections directions)
        {
             return (directions == HexCellDirections.NW ? HexCellDirections.NE : directions + 1);
        }
        public HexCellDirections GetPreviousDirection(HexCellDirections directions)
        {
            return directions == HexCellDirections.NE ? HexCellDirections.NW : directions - 1;
        }
        public  Vector3 GetBridge(HexCellDirections direction)
        {
            return (corners[(int)direction] + corners[(int)direction + 1]) * blendFactor;
        }
    }
}

