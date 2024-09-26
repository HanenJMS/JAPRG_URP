using GameLab.GridSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GameLab.GridSystem
{
    public enum HexEdgeType
    {
        Flat, Slope, DescendingSlope, RisingSlope, DescendingCliff, RisingCliff, Cliff
    }
    public static class HexMetric
    {
        public static float solidFactor = 0.75f;
        public static float elevationStep = 5f;
        public static float blendFactor = 1f - solidFactor;
        public static int terracesPerSlope = 2;
        public static int terraceSteps = terracesPerSlope * 2 + 1;
        public static float horizontalTerraceStepSize = 1f / terraceSteps;
        public static float verticalTerraceStepSize = 1f / (terracesPerSlope + 1);

        public static HexCellDirections GetNextDirection(HexCellDirections directions)
        {
            return (directions == HexCellDirections.NW ? HexCellDirections.NE : directions + 1);
        }
        public static HexCellDirections GetPreviousDirection(HexCellDirections directions)
        {
            return directions == HexCellDirections.NE ? HexCellDirections.NW : directions - 1;
        }

        public static Vector3 TerraceLerp(Vector3 a, Vector3 b, int step)
        {
            float h = step * horizontalTerraceStepSize;
            a.x += (b.x - a.x) * h;
            a.z += (b.z - a.z) * h;
            float v = ((step + 1) / 2) * verticalTerraceStepSize;
            a.y += (b.y - a.y) * v;
            return a;
        }
        public static Color TerraceLerp(Color a, Color b, int step)
        {
            float h = step * horizontalTerraceStepSize;
            return Color.Lerp(a, b, h);
        }
        public static HexEdgeType GetEdgeType(int homeElevation, int otherElevation)
        {
            if (homeElevation == otherElevation)
            {
                return HexEdgeType.Flat;
            }
            int delta = otherElevation - homeElevation;
            if (delta == 1)
            {
                return HexEdgeType.RisingSlope;
            }
            if (delta == -1)
            {
                return HexEdgeType.DescendingSlope;
            }
            if(delta >= 2)
            {
                return HexEdgeType.RisingCliff;
            }
            return HexEdgeType.DescendingCliff;
        }
        public static HexEdgeType GetEdgeType(HexEdgeType edgeType)
        {
            if (edgeType == HexEdgeType.RisingSlope || edgeType == HexEdgeType.DescendingSlope)
            {
                return HexEdgeType.Slope;
            }
            if (edgeType == HexEdgeType.RisingCliff || edgeType == HexEdgeType.DescendingCliff)
            {
                return HexEdgeType.Cliff;
            }
            return edgeType;
        }
    }
}


