using System.Collections.Generic;
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
        public Color color;
        float solidFactor = 0.75f;
        int elevation;
        float elevationStep = 5f;
        float blendFactor;
        int chunkIndex = int.MinValue;
        HexGridChunk chunk;
        bool hasIncomingRiver, hasOutgoingRiver;
        HexCellDirections incomingRiver, outgoingRiver;
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
        public int GetElevation()
        {
            return elevation;
        }
        public void SetElevation(int elevation)
        {
            if (this.elevation == elevation)
                return;

            this.elevation = elevation;
            Vector3 position = transform.localPosition;
            position.y = elevation * HexMetric.elevationStep;
            position.y +=
                (HexMetric.SampleNoise(position).y * 2f - 1f) *
                HexMetric.elevationPerturbStrength;
            transform.localPosition = position;
            if (
                hasOutgoingRiver &&
                elevation < HexGridVisualSystem.Instance.GetHexCell(GetHexCellNeighborGridPosition(outgoingRiver)).GetElevation()
)
            {
                RemoveOutgoingRiver();
            }
            if (
                hasIncomingRiver &&
                elevation > HexGridVisualSystem.Instance.GetHexCell(GetHexCellNeighborGridPosition(incomingRiver)).GetElevation()
            )
            {
                RemoveIncomingRiver();
            }
            Refresh();

        }
        void Refresh()
        {
            if (chunk != null)
            {
                chunk.Refresh();
                RefreshChunks();
            }
        }

        private void RefreshChunks()
        {
            List<HexGridChunk> chunks = new();
            chunks.Add(chunk);
            foreach (var item in hexCellNeighbors)
            {
                var hexNeighbor = HexGridVisualSystem.Instance.GetHexCell(item.Value);
                if (hexNeighbor == null) continue;
                if (!chunks.Contains(hexNeighbor.GetHexGridChunk()))
                {
                    hexNeighbor.GetHexGridChunk().Refresh();
                    chunks.Add(hexNeighbor.GetHexGridChunk());
                }
            }
        }

        public void SetColor(Color color)
        {
            this.color = color;
            Refresh();
        }
        public Color GetCellColor()
        {
            return color;
        }

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

            hexCellNeighbors.Add(HexCellDirections.NE, NE);
            hexCellNeighbors.Add(HexCellDirections.E, E);
            hexCellNeighbors.Add(HexCellDirections.SE, SE);
            hexCellNeighbors.Add(HexCellDirections.SW, SW);
            hexCellNeighbors.Add(HexCellDirections.W, W);
            hexCellNeighbors.Add(HexCellDirections.NW, NW);
        }
        public GridPosition GetGridPosition()
        {
            return gridPosition;
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
            var PrevDirection = HexMetric.GetPreviousDirection(directions);
            return hexCellNeighbors[PrevDirection];
        }
        public GridPosition GetNextDirectionHexNeighbor(HexCellDirections directions)
        {
            var nextDirection = HexMetric.GetNextDirection(directions);
            return hexCellNeighbors[nextDirection];
        }

        public Vector3 GetBridge(HexCellDirections direction)
        {
            return (corners[(int)direction] + corners[(int)direction + 1]) * blendFactor;
        }


        public HexEdgeType GetEdgeType(HexCellDirections direction)
        {
            return HexMetric.GetEdgeType
            (
                elevation, HexGridVisualSystem.Instance.GetHexCell(hexCellNeighbors[direction]).GetElevation()
            );
        }
        public HexEdgeType GetEdgeType(HexCell otherCell)
        {
            return HexMetric.GetEdgeType(
                elevation, otherCell.GetElevation()
            );
        }
        public HexEdgeType GetEdgeType(HexEdgeType edgeType)
        {
            return HexMetric.GetEdgeType(edgeType);
        }
        public HexGridChunk GetHexGridChunk()
        {
            return chunk;
        }
        public int GetCellChunkIndex()
        {
            return chunkIndex;
        }
        public void SetCellChunkIndex(int index)
        {
            chunkIndex = index;
        }
        public void SetCellChunk(HexGridChunk chunk)
        {
            this.chunk = chunk;
        }

        public bool HasIncomingRiver
        {
            get
            {
                return hasIncomingRiver;
            }
        }

        public bool HasOutgoingRiver
        {
            get
            {
                return hasOutgoingRiver;
            }
        }

        public HexCellDirections IncomingRiver
        {
            get
            {
                return incomingRiver;
            }
        }

        public HexCellDirections OutgoingRiver
        {
            get
            {
                return outgoingRiver;
            }
        }
        public bool HasRiver
        {
            get
            {
                return hasIncomingRiver || hasOutgoingRiver;
            }
        }
        public bool HasRiverBeginOrEnd
        {
            get
            {
                return hasIncomingRiver != hasOutgoingRiver;
            }
        }
        public bool HasRiverThroughEdge(HexCellDirections direction)
        {
            return
                hasIncomingRiver && incomingRiver == direction ||
                hasOutgoingRiver && outgoingRiver == direction;
        }
        public void RemoveOutgoingRiver()
        {
            if (!hasOutgoingRiver)
            {
                return;
            }
            hasOutgoingRiver = false;
            RefreshSelfOnly();

            HexCell neighbor = HexGridVisualSystem.Instance.GetHexCell(GetHexCellNeighborGridPosition(outgoingRiver));
            neighbor.hasIncomingRiver = false;
            neighbor.RefreshSelfOnly();
        }
        void RefreshSelfOnly()
        {
            chunk.Refresh();
        }
        public void RemoveIncomingRiver()
        {
            if (!hasIncomingRiver)
            {
                return;
            }
            hasIncomingRiver = false;
            RefreshSelfOnly();

            HexCell neighbor = HexGridVisualSystem.Instance.GetHexCell(GetHexCellNeighborGridPosition(incomingRiver));
            neighbor.hasOutgoingRiver = false;
            neighbor.RefreshSelfOnly();
        }
        public void RemoveRiver()
        {
            RemoveOutgoingRiver();
            RemoveIncomingRiver();
        }
        public void SetOutgoingRiver(HexCellDirections direction)
        {
            if (hasOutgoingRiver && outgoingRiver == direction)
            {
                return;
            }
            HexCell neighbor = HexGridVisualSystem.Instance.GetHexCell(GetHexCellNeighborGridPosition(direction));
            if (!neighbor || elevation < neighbor.elevation)
            {
                return;
            }
            RemoveOutgoingRiver();
            if (hasIncomingRiver && incomingRiver == direction)
            {
                RemoveIncomingRiver();
            }
            hasOutgoingRiver = true;
            outgoingRiver = direction;
            RefreshSelfOnly();
            neighbor.RemoveIncomingRiver();
            neighbor.hasIncomingRiver = true;
            neighbor.incomingRiver = HexMetric.GetOppositeDirection(direction);
            neighbor.RefreshSelfOnly();
        }
    }
}

