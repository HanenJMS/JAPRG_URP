using UnityEngine;

namespace GameLab.GridSystem
{
    public struct EdgeVertices
    {
        public Vector3 v1, v2, v3, v4, v5;
        public EdgeVertices(Vector3 corner1, Vector3 corner2)
        {
            v1 = corner1;
            v2 = Vector3.Lerp(corner1, corner2, 0.25f);
            v3 = Vector3.Lerp(corner1, corner2, 0.5f);
            v4 = Vector3.Lerp(corner1, corner2, 0.75f);
            v5 = corner2;
        }
        public EdgeVertices(Vector3 corner1, Vector3 corner2, float outerStep)
        {
            v1 = corner1;
            v2 = Vector3.Lerp(corner1, corner2, outerStep);
            v3 = Vector3.Lerp(corner1, corner2, 0.5f);
            v4 = Vector3.Lerp(corner1, corner2, 1f - outerStep);
            v5 = corner2;
        }
        public static EdgeVertices TerraceLerp(
            EdgeVertices a, EdgeVertices b, int step)
        {
            EdgeVertices result;
            result.v1 = HexMetric.TerraceLerp(a.v1, b.v1, step);
            result.v2 = HexMetric.TerraceLerp(a.v2, b.v2, step);
            result.v3 = HexMetric.TerraceLerp(a.v3, b.v3, step);
            result.v4 = HexMetric.TerraceLerp(a.v4, b.v4, step);
            result.v5 = HexMetric.TerraceLerp(a.v5, b.v5, step);
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
        public const float streamBedElevationOffset = -1.25f;
        public const float innerRadiusConstant = 0.866025404f;
        public const float outerToInner = 0.866025404f;
        public const float innerToOuter = 1f / outerToInner;
        //gridsize 10 * 0.5. this is the radius as defined half of a single cell size.
        static float outerRadius = 10 / 2;
        static float innerRadiusCalculated = outerRadius * innerRadiusConstant;
        public const float waterElevationOffset = -0.5f;
        public const float waterFactor = 0.6f;
        public const float waterBlendFactor = 1f - waterFactor;
        public const float wallHeight = 4f;
        public const float wallThickness = 0.75f;
        public const float wallYOffset = -1f;
        public static float wallElevationOffset = verticalTerraceStepSize;
        public const float wallTowerThreshold = 0.5f;
        public const float bridgeDesignLength = 7f;
        public static Color[] colors;
        static Vector3[] corners =
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
        public static Vector3 GetFirstCorner(HexCellDirections direction)
        {
            return corners[(int)direction];
        }
        public static Vector3 GetSecondCorner(HexCellDirections direction)
        {
            return corners[(int)direction + 1];
        }
        public static Vector3 GetFirstSolidCorner(HexCellDirections direction)
        {
            return corners[(int)direction] * solidFactor;
        }
        public static Vector3 GetSecondSolidCorner(HexCellDirections direction)
        {
            return corners[(int)direction + 1] * solidFactor;
        }
        public static Vector3 GetFirstWaterCorner(HexCellDirections direction)
        {
            return corners[(int)direction] * waterFactor;
        }

        public static Vector3 GetSecondWaterCorner(HexCellDirections direction)
        {
            return corners[(int)direction + 1] * waterFactor;
        }

        public static Vector3 Perturb(Vector3 position)
        {
            Vector4 sample = SampleNoise(position);
            position.x += (sample.x * 2f - 1f) * cellPerturbStrength;
            position.z += (sample.z * 2f - 1f) * cellPerturbStrength;
            return position;
        }

        public static Vector3 GetBridge(HexCellDirections direction)
        {
            return (corners[(int)direction] + corners[(int)direction + 1]) * blendFactor;
        }
        public static Vector4 SampleNoise(Vector3 position)
        {
            return noiseSource.GetPixelBilinear(
                position.x * noiseScale,
                position.z * noiseScale
            );
        }

        public static HexCellDirections GetNextDirection(this HexCellDirections directions)
        {
            return (directions == HexCellDirections.NW ? HexCellDirections.NE : directions + 1);
        }
        public static HexCellDirections GetPreviousDirection(this HexCellDirections directions)
        {
            return directions == HexCellDirections.NE ? HexCellDirections.NW : directions - 1;
        }
        public static HexCellDirections Previous2(this HexCellDirections direction)
        {
            direction -= 2;
            return direction >= HexCellDirections.NE ? direction : (direction + 6);
        }
        public static HexCellDirections Next2(this HexCellDirections direction)
        {
            direction += 2;
            return direction <= HexCellDirections.NW ? direction : (direction - 6);
        }
        public static HexCellDirections GetOppositeDirection(this HexCellDirections direction)
        {
            var dir = (int)direction + 3;
            if (dir > (int)HexCellDirections.NW)
            {
                dir = dir - 6;
            }
            return (HexCellDirections)dir;
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


        public static Vector3 GetSolidEdgeMiddle(HexCellDirections direction)
        {
            return
                (corners[(int)direction] + corners[(int)direction + 1]) *
                (0.5f * solidFactor);
        }

        public static Vector3 GetWaterBridge(HexCellDirections direction)
        {
            return (corners[(int)direction] + corners[(int)direction + 1]) *
                waterBlendFactor;
        }


        public const int hashGridSize = 256;
        public const float hashGridScale = 0.25f;
        static HexHash[] hashGrid;
        public static void InitializeHashGrid(int seed)
        {
            hashGrid = new HexHash[hashGridSize * hashGridSize];
            Random.State currentState = Random.state;
            Random.InitState(seed);
            for (int i = 0; i < hashGrid.Length; i++)
            {
                hashGrid[i] = HexHash.Create();
            }
            Random.state = currentState;
        }
        public static HexHash SampleHashGrid(Vector3 position)
        {
            int x = (int)(position.x * hashGridScale) % hashGridSize;
            if (x < 0)
            {
                x += hashGridSize;
            }
            int z = (int)(position.z * hashGridScale) % hashGridSize;
            if (z < 0)
            {
                z += hashGridSize;
            }
            return hashGrid[x + z * hashGridSize];
        }
        static float[][] featureThresholds = 
        {
            new float[] {0.0f, 0.0f, 0.4f},
            new float[] {0.0f, 0.4f, 0.6f},
            new float[] {0.4f, 0.6f, 0.8f}
        };

        public static float[] GetFeatureThresholds(int level)
        {
            return featureThresholds[level];
        }
        public static Vector3 WallThicknessOffset(Vector3 near, Vector3 far)
        {
            Vector3 offset;
            offset.x = far.x - near.x;
            offset.y = 0f;
            offset.z = far.z - near.z;
            return offset.normalized * (wallThickness * 0.5f);
        }
        public static Vector3 WallLerp(Vector3 near, Vector3 far)
        {
            near.x += (far.x - near.x) * 0.5f;
            near.z += (far.z - near.z) * 0.5f;
            float v =
                near.y < far.y ? wallElevationOffset : (1f - wallElevationOffset);
            near.y += (far.y - near.y) * v + wallYOffset;
            return near;
        }
    }
}


