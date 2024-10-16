using System;

namespace GameLab.GridSystem
{
    public struct HexGridCoordinates : IEquatable<HexGridCoordinates>
    {
        public int x;
        public int z;
        public int y;
        public HexGridCoordinates(int x, int z, int y = 0)
        {
            this.x = x;
            this.z = z;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            return obj is HexGridCoordinates position && x == position.x && z == position.z;
        }

        public bool Equals(HexGridCoordinates other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, z);
        }

        public override string ToString()
        {
            return $"({x}, {z})\n" +
                        $"  {y}";
        }
        public static bool operator ==(HexGridCoordinates a, HexGridCoordinates b)
        {
            return a.x == b.x && a.z == b.z;
        }
        public static bool operator !=(HexGridCoordinates a, HexGridCoordinates b)
        {
            return !(a == b);
        }

        public static HexGridCoordinates operator +(HexGridCoordinates a, HexGridCoordinates b)
        {
            return new HexGridCoordinates(a.x + b.x, a.z + b.z);
        }
        public static HexGridCoordinates operator -(HexGridCoordinates a, HexGridCoordinates b)
        {
            return new HexGridCoordinates(a.x - b.x, a.z - b.z);
        }
    }
}

