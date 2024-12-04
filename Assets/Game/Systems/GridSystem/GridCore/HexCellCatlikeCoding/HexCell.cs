using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameLab.GridSystem
{
    //Set default properties

    public class HexCell : MonoBehaviour
    {
        //cell boundaries
        float outerRadius;
        float innerRadiusConstant = 0.866025404f;
        int elevation;
        GridPosition gridPosition;

        Dictionary<HexCellDirections, GridPosition> hexCellNeighbors;
        int terrainTypeIndex;
        float solidFactor = 0.75f;
        int chunkIndex = int.MinValue;
        HexGridChunk chunk;


        bool hasIncomingRiver, hasOutgoingRiver;
        int waterLevel;
        [SerializeField] bool[] roads;
        HexCellDirections incomingRiver, outgoingRiver;

        int specialIndex;

        public HexCellDirections RiverBeginOrEndDirection => hasIncomingRiver ? incomingRiver : outgoingRiver;
        public float RiverSurfaceY => transform.position.y + HexMetric.waterElevationOffset;
        public float WaterSurfaceY => HexMetric.waterElevationOffset + WaterLevel;
        public int WaterLevel
        {
            get => waterLevel;
            set
            {
                if (waterLevel == value)
                {
                    return;
                }
                waterLevel = value;
                ValidateRivers();
                Refresh();
            }
        }
        public bool IsUnderwater => waterLevel > elevation;
        public float StreamBedY => transform.position.y + HexMetric.streamBedElevationOffset;
        public void InitlializeCell()
        {
            InitializeNeighbors();
            if (terrainTypeIndex < 0)
            {
                terrainTypeIndex = 0;
            }
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
            RefreshPosition();
            ValidateRivers();
            for (int i = 0; i < roads.Length; i++)
            {
                if (roads[i] && GetElevationDifference((HexCellDirections)i) > 1)
                {
                    SetRoad(i, false);
                }
            }

            Refresh();

        }
        void RefreshPosition()
        {
            Vector3 position = transform.localPosition;
            position.y = elevation * HexMetric.elevationStep;
            position.y +=
                (HexMetric.SampleNoise(position).y * 2f - 1f) *
                HexMetric.elevationPerturbStrength;
            transform.localPosition = position;
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

        public Color GetCellColor()
        {
            return HexMetric.colors[terrainTypeIndex];
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
        public HexCell GetHexCellNeighbor(HexCellDirections directions)
        {
            return HexGridVisualSystem.Instance.GetHexCell(GetHexCellNeighborGridPosition(directions));
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
        public bool HasIncomingRiver => hasIncomingRiver;
        public bool HasOutgoingRiver => hasOutgoingRiver;
        public HexCellDirections IncomingRiver => incomingRiver;
        public HexCellDirections OutgoingRiver => outgoingRiver;
        public bool HasRiver => hasIncomingRiver || hasOutgoingRiver;
        public bool HasRiverBeginOrEnd => hasIncomingRiver != hasOutgoingRiver;
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

            HexCell neighbor = GetHexCellNeighbor(outgoingRiver);
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
            if (hasOutgoingRiver && outgoingRiver == direction) return;
            HexCell neighbor = GetHexCellNeighbor(direction);
            HexCellDirections neighborFacingDirection = direction.GetOppositeDirection();
            if (!IsValidRiverDestination(neighbor))
            {
                return;
            }
            RemoveOutgoingRiver();
            if (hasIncomingRiver && incomingRiver == direction)
            {
                RemoveIncomingRiver();
            }

            outgoingRiver = direction;
            specialIndex = 0;
            hasOutgoingRiver = true;
            //RefreshSelfOnly();
            neighbor.RemoveIncomingRiver();
            neighbor.incomingRiver = neighborFacingDirection;
            neighbor.specialIndex = 0;
            neighbor.hasIncomingRiver = true;
            //neighbor.RefreshSelfOnly();

            SetRoad((int)direction, false);
        }
        public bool HasRoadThroughEdge(HexCellDirections direction)
        {
            return roads[(int)direction];
        }
        public bool HasRoads
        {
            get
            {
                for (int i = 0; i < roads.Length; i++)
                {
                    if (roads[i])
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        void SetRoad(int index, bool state)
        {
            roads[index] = state;
            var neighbor = GetHexCellNeighbor((HexCellDirections)index);
            neighbor.roads[(int)((HexCellDirections)index).GetOppositeDirection()] = state;
            neighbor.RefreshSelfOnly();
            RefreshSelfOnly();
        }
        public void AddRoad(HexCellDirections direction)
        {
            if (!roads[(int)direction] && !HasRiverThroughEdge(direction) && !IsSpecial && !GetHexCellNeighbor(direction).IsSpecial && GetElevationDifference(direction) <= 1)
            {
                SetRoad((int)direction, true);
            }
        }
        public void RemoveRoads()
        {
            for (int i = 0; i <= (int)HexCellDirections.NW; i++)
            {
                SetRoad(i, false);
            }
        }
        public int GetElevationDifference(HexCellDirections direction)
        {
            int difference = elevation - GetHexCellNeighbor(direction).elevation;
            return difference >= 0 ? difference : -difference;
        }
        bool IsValidRiverDestination(HexCell neighbor)
        {
            return neighbor && (
                elevation >= neighbor.elevation || waterLevel == neighbor.elevation
            );
        }
        void ValidateRivers()
        {
            if (
                hasOutgoingRiver &&
                !IsValidRiverDestination(GetHexCellNeighbor(outgoingRiver))
            )
            {
                RemoveOutgoingRiver();
            }
            if (
                hasIncomingRiver &&
                !GetHexCellNeighbor(incomingRiver).IsValidRiverDestination(this)
            )
            {
                RemoveIncomingRiver();
            }
        }
        public int UrbanLevel
        {
            get => urbanLevel;
            set
            {
                if (urbanLevel != value)
                {
                    urbanLevel = value;
                    RefreshSelfOnly();
                }
            }
        }
        public int FarmLevel
        {
            get
            {
                return farmLevel;
            }
            set
            {
                if (farmLevel != value)
                {
                    farmLevel = value;
                    RefreshSelfOnly();
                }
            }
        }

        public int PlantLevel
        {
            get
            {
                return plantLevel;
            }
            set
            {
                if (plantLevel != value)
                {
                    plantLevel = value;
                    RefreshSelfOnly();
                }
            }
        }
        int urbanLevel, farmLevel, plantLevel;
        public bool Walled
        {
            get
            {
                return walled;
            }
            set
            {
                if (walled != value)
                {
                    walled = value;
                    Refresh();
                }
            }
        }

        bool walled;

        public int SpecialIndex
        {
            get
            {
                return specialIndex;
            }
            set
            {
                if (specialIndex != value)
                {
                    if (specialIndex != value && !HasRiver)
                    {
                        specialIndex = value;
                        RemoveRoads();
                        RefreshSelfOnly();
                    }
                }
            }
        }
        public bool IsSpecial
        {
            get
            {
                return specialIndex > 0;
            }
        }
        public int TerrainTypeIndex
        {
            get
            {
                return terrainTypeIndex;
            }
            set
            {
                if (terrainTypeIndex != value)
                {
                    terrainTypeIndex = value;
                    Refresh();
                }
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write((byte)terrainTypeIndex);
            writer.Write((byte)elevation);
            writer.Write((byte)waterLevel);
            writer.Write((byte)urbanLevel);
            writer.Write((byte)farmLevel);
            writer.Write((byte)plantLevel);
            writer.Write((byte)specialIndex);
            writer.Write(walled);

            if (hasIncomingRiver)
            {
                writer.Write((byte)(incomingRiver + 128));
            }
            else
            {
                writer.Write((byte)0);
            }

            if (hasOutgoingRiver)
            {
                writer.Write((byte)(outgoingRiver + 128));
            }
            else
            {
                writer.Write((byte)0);
            }


            int roadFlags = 0;
            for (int i = 0; i < roads.Length; i++)
            {
                if (roads[i])
                {
                    roadFlags |= 1 << i;
                }
            }
            writer.Write((byte)roadFlags);
        }

        public void Load(BinaryReader reader)
        {
            terrainTypeIndex = reader.ReadByte();
            elevation = reader.ReadByte();
            RefreshPosition();
            waterLevel = reader.ReadByte();
            urbanLevel = reader.ReadByte();
            farmLevel = reader.ReadByte();
            plantLevel = reader.ReadByte();
            specialIndex = reader.ReadByte();
            walled = reader.ReadBoolean();

            byte riverData = reader.ReadByte();
            if (riverData >= 128)
            {
                hasIncomingRiver = true;
                incomingRiver = (HexCellDirections)(riverData - 128);
            }
            else
            {
                hasIncomingRiver = false;
            }
            riverData = reader.ReadByte();
            if (riverData >= 128)
            {
                hasOutgoingRiver = true;
                outgoingRiver = (HexCellDirections)(riverData - 128);
            }
            else
            {
                hasOutgoingRiver = false;
            }
            int roadFlags = reader.ReadByte();
            for (int i = 0; i < roads.Length; i++)
            {
                roads[i] = (roadFlags & (1 << i)) != 0;
            }
        }
    }
}

