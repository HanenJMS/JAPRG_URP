using UnityEngine;

namespace GameLab.GridSystem
{
    public struct EdgeVertices
    {
        public Vector3 v1, v2, v3, v4;
        public EdgeVertices(Vector3 corner1, Vector3 corner2)
        {
            v1 = corner1;
            v2 = Vector3.Lerp(corner1, corner2, 1f / 3f);
            v3 = Vector3.Lerp(corner1, corner2, 2f / 3f);
            v4 = corner2;
        }
        public static EdgeVertices TerraceLerp(
            EdgeVertices a, EdgeVertices b, int step)
        {
            EdgeVertices result;
            result.v1 = HexMetric.TerraceLerp(a.v1, b.v1, step);
            result.v2 = HexMetric.TerraceLerp(a.v2, b.v2, step);
            result.v3 = HexMetric.TerraceLerp(a.v3, b.v3, step);
            result.v4 = HexMetric.TerraceLerp(a.v4, b.v4, step);
            return result;
        }
    }
    public enum HexEdgeType
    {
        Flat, Slope, DescendingSlope, RisingSlope, DescendingCliff, RisingCliff, Cliff
    }
    public static class HexMetric
    {
        public static float solidFactor = 0.8f;
        public static float elevationStep = 3f;
        public static float blendFactor = 1f - solidFactor;
        public static int terracesPerSlope = 2;
        public static int terraceSteps = terracesPerSlope * 2 + 1;
        public static float horizontalTerraceStepSize = 1f / terraceSteps;
        public static float verticalTerraceStepSize = 1f / (terracesPerSlope + 1);
        public static Texture2D noiseSource;
        public const float cellPerturbStrength = 4f;
        public const float elevationPerturbStrength = 1.5f;
        public const float noiseScale = 0.0033f;
        public const int chunkSizeX = 5, chunkSizeZ = 5;
        public static Vector4 SampleNoise(Vector3 position)
        {
            return noiseSource.GetPixelBilinear(
                position.x * noiseScale,
                position.z * noiseScale
            );
        }

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
            if (delta >= 2)
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


