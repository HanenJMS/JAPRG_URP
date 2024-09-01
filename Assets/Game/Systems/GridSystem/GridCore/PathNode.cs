namespace GameLab.GridSystem
{
    public class PathNode
    {
        GridPosition gridPosition;
        int gCost, hCost, fCost;

        PathNode prevNode;
        bool isPathBlocked = false;
        public PathNode(GridPosition gridPosition)
        {
            this.gridPosition = gridPosition;
        }
        public override string ToString()
        {
            return gridPosition.ToString();
        }
        public int GetgCost()
        {
            return gCost;
        }
        public int GethCost()
        {
            return hCost;
        }
        public int GetfCost()
        {
            return fCost;
        }
        public GridPosition GetGridPosition()
        {
            return gridPosition;
        }
        public void SetgCost(int gCost)
        {
            this.gCost = gCost;
        }
        public void SethCost(int hCost)
        {
            this.hCost = hCost;
        }
        public void SetfCost(int fCost)
        {
            this.fCost = fCost;
        }
        public bool GetIsPathBlocked()
        {
            return isPathBlocked;
        }
        public void SetIsBlocked(bool isBlocking)
        {
            isPathBlocked = isBlocking;
        }
        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
        public void ResetPreviousNode()
        {
            prevNode = null;
        }
        public void SetPreviosNode(PathNode prevNode)
        {
            this.prevNode = prevNode;
        }
        public PathNode GetPreviousNode()
        {
            return prevNode;
        }
    }
}

