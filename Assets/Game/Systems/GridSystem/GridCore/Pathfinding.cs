using System.Collections.Generic;
using UnityEngine;
namespace GameLab.GridSystem
{
    public class Pathfinding : MonoBehaviour
    {
        const int Move_Straight_Cost = 10;
        const int Move_Diagonal_Cost = 14;

        public static Pathfinding Instance { get; private set; }
        [SerializeField] private Transform gridDebugObject;
        GridSystem<PathNode> gridSystem;
        [SerializeField] int width, height;
        [SerializeField] float cellSize;
        [SerializeField] LayerMask obstacleLayerMask;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            gridSystem = new GridSystem<PathNode>(width, height, cellSize, (GridSystem<PathNode> g, GridPosition gp) => new PathNode(gp));
            //gridSystem.CreateDebugObject(gridDebugObject);
        }
        private void Start()
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    Vector3 worldPosition = LevelGridSystem.Instance.GetWorldPosition(gridPosition);
                    float raycastoffsetDistance = 5f;
                    if (Physics.Raycast(worldPosition + Vector3.down * raycastoffsetDistance, Vector3.up, raycastoffsetDistance * 2, obstacleLayerMask))
                    {
                        GetNode(new(x, z)).SetIsBlocked(true);
                    }
                }
            }
        }

        public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endPosition)
        {
            List<PathNode> openList = new();
            List<PathNode> closedList = new();

            PathNode startNode = gridSystem.GetGridObject(startGridPosition);
            PathNode endNode = gridSystem.GetGridObject(endPosition);
            openList.Add(startNode);

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    GridPosition gp = new(x, z);
                    PathNode pn = gridSystem.GetGridObject(gp);

                    pn.SetgCost(int.MaxValue);
                    pn.SethCost(0);
                    pn.CalculateFCost();
                    pn.ResetPreviousNode();
                }
            }
            startNode.SetgCost(0);
            startNode.SethCost(CalculateDistance(startGridPosition, endPosition));
            startNode.CalculateFCost();

            while (openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostPathNode(openList);
                if (currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }
                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (PathNode neighborNode in GetNeighborNodeList(currentNode))
                {
                    if (closedList.Contains(neighborNode)) continue;
                    if (neighborNode.GetIsPathBlocked())
                    {
                        closedList.Add(neighborNode);
                        continue;
                    }
                    int tentativegCost = currentNode.GetgCost() + CalculateDistance(currentNode.GetGridPosition(), neighborNode.GetGridPosition());
                    if (tentativegCost < neighborNode.GetgCost())
                    {
                        neighborNode.SetPreviosNode(currentNode);
                        neighborNode.SetgCost(tentativegCost);
                        neighborNode.SethCost(CalculateDistance(neighborNode.GetGridPosition(), endPosition));
                        neighborNode.CalculateFCost();
                    }

                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
            return null;
        }

        private List<GridPosition> CalculatePath(PathNode endNode)
        {
            List<GridPosition> gridPositions = new List<GridPosition>();
            gridPositions.Add(endNode.GetGridPosition());
            PathNode currentNode = endNode;
            while (currentNode.GetPreviousNode() != null)
            {
                gridPositions.Add(currentNode.GetPreviousNode().GetGridPosition());
                currentNode = currentNode.GetPreviousNode();
            }
            gridPositions.Reverse();


            return gridPositions;
        }

        PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
        {
            PathNode LowestFCostPathNode = pathNodeList[0];
            for (int i = 0; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].GetfCost() < LowestFCostPathNode.GetfCost())
                {
                    LowestFCostPathNode = pathNodeList[i];
                }
            }
            return LowestFCostPathNode;
        }
        public PathNode GetNode(GridPosition gridPosition)
        {
            return gridSystem.GetGridObject(gridPosition);
        }
        List<PathNode> GetNeighborNodeList(PathNode currentNode)
        {
            List<PathNode> neighborPosition = new();
            GridPosition gridPosition = currentNode.GetGridPosition();

            for (int x = -1; x < 2; x++)
            {
                for (int z = -1; z < 2; z++)
                {
                    GridPosition testingGridPosition = new(gridPosition.x + x, gridPosition.z + z);
                    if (gridSystem.isValidGridPosition(testingGridPosition))
                    {
                        neighborPosition.Add(GetNode(testingGridPosition));
                    }
                }
            }
            return neighborPosition;
        }

        public int CalculateDistance(GridPosition gpA, GridPosition gpB)
        {
            GridPosition gridPositionDistance = gpA - gpB;
            int xdistance = Mathf.Abs(gridPositionDistance.x);
            int zdistance = Mathf.Abs(gridPositionDistance.z);

            int remaining = Mathf.Abs(xdistance - zdistance);
            return Move_Diagonal_Cost * Mathf.Min(xdistance, zdistance) + Move_Straight_Cost * remaining;
        }

    }
}

