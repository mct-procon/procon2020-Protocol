using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;
using System.Numerics;

namespace MCTProcon29Protocol
{
    /// <summary>
    /// x-y平面座標での場所を示す構造体
    /// </summary>
    [MessagePackObject]
    public struct Point
    {
        /// <summary>
        /// x座標（ushort分しか使わない）
        /// </summary>
        [Key(0)]
        public uint X { get; set; }

        /// <summary>
        /// y座標（ushort分しか使わない）
        /// </summary>
        [Key(1)]
        public uint Y { get; set; }

        public Point(uint x, uint y)
        {
            X = x; Y = y;
        }

        public override int GetHashCode()
            => (int)(X << 16) + (int)Y;

        public override bool Equals(object obj)
        {
            if (obj is Point)
                return this == (Point)obj;
            else
                return false;
        }

        public static bool operator ==(Point x, Point y) => x.X == y.X && x.Y == y.Y;
        public static bool operator !=(Point x, Point y) => x.X != y.X || x.Y != y.Y;

        public static Point operator +(Point x, (int x, int y) y)
        {
            Vector<int> _x = new Vector<int>(new[] { (int)x.X, (int)x.Y }), _y = new Vector<int>(new int[] { y.x, y.y });
            var ret = _x + _y;
            return new Point((uint)ret[0], (uint)ret[1]);
        }

        public static Point operator -(Point x, (int x, int y) y)
        {
            Vector<int> _x = new Vector<int>(new[] { (int)x.X, (int)x.Y }), _y = new Vector<int>(new int[] { y.x, y.y });
            var ret = _x - _y;
            return new Point((uint)ret[0], (uint)ret[1]);
        }

        public static Point operator +(Point x, (uint x, uint y) y)
        {
            Vector<uint> _x = new Vector<uint>(new[] { x.X, x.Y }), _y = new Vector<uint>(new [] { y.x, y.y });
            var ret = _x + _y;
            return new Point(ret[0], ret[1]);
        }

        public static Point operator -(Point x, (uint x, uint y) y)
        {
            Vector<uint> _x = new Vector<uint>(new[] { x.X, x.Y }), _y = new Vector<uint>(new[] { y.x, y.y });
            var ret = _x - _y;
            return new Point(ret[0], ret[1]);
        }

        public static Point operator +(Point x, Point y)
        {
            Vector<uint> _x = new Vector<uint>(new[] { x.X, x.Y }), _y = new Vector<uint>(new[] { y.X, y.Y });
            var ret = _x + _y;
            return new Point(ret[0], ret[1]);
        }

        public static Point operator -(Point x, Point y)
        {
            Vector<uint> _x = new Vector<uint>(new[] { x.X, x.Y }), _y = new Vector<uint>(new[] { y.X, y.Y });
            var ret = _x - _y;
            return new Point(ret[0], ret[1]);
        }

        public static Point operator +(Point x, VelocityPoint y)
        {
            Vector<int> _x = new Vector<int>(new[] { (int)x.X, (int)x.Y }), _y = new Vector<int>(new int[] { y.X, y.Y });
            var ret = _x + _y;
            return new Point((uint)ret[0], (uint)ret[1]);
        }

        public static Point operator -(Point x, VelocityPoint y)
        {
            Vector<int> _x = new Vector<int>(new[] { (int)x.X, (int)x.Y }), _y = new Vector<int>(new int[] { y.X, y.Y });
            var ret = _x - _y;
            return new Point((uint)ret[0], (uint)ret[1]);
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }
}
