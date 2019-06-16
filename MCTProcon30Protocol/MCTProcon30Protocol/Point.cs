using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;
using System.Numerics;
using System.Runtime.InteropServices;

namespace MCTProcon30Protocol
{
    /// <summary>
    /// x-y平面座標での場所を示す構造体
    /// </summary>
    [MessagePackObject]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Point
    {
        /// <summary>
        /// x座標
        /// </summary>
        [Key(0)]
        public byte X { get; set; }

        /// <summary>
        /// y座標
        /// </summary>
        [Key(1)]
        public byte Y { get; set; }

        public Point(byte x, byte y)
        {
            this.X = x;
            this.Y = y;
        }

        public override int GetHashCode()
            => X | (Y << 8);

        public override bool Equals(object obj)
        {
            if (obj is Point)
                return this == (Point)obj;
            else
                return false;
        }

        public static bool operator ==(Point x, Point y) => x.GetHashCode() == y.GetHashCode();
        public static bool operator !=(Point x, Point y) => x.GetHashCode() != y.GetHashCode();

        public static Point operator +(Point x, (int x, int y) y)
        {
            x.X = (byte)((int)x.X + y.x);
            x.Y = (byte)((int)x.Y + y.y);
            return x;
        }

        public static Point operator -(Point x, (int x, int y) y)
        {
            x.X = (byte)((int)x.X - y.x);
            x.Y = (byte)((int)x.Y - y.y);
            return x;
        }

        public static Point operator +(Point x, (byte x, byte y) y)
        {
            x.X += y.x;
            x.Y += y.y;
            return x;
        }

        public static Point operator -(Point x, (byte x, byte y) y)
        {
            x.X -= y.x;
            x.Y -= y.y;
            return x;
        }

        public static Point operator +(Point x, Point y)
        {
            x.X += y.X;
            x.Y += y.Y;
            return x;
        }

        public static Point operator -(Point x, Point y)
        {
            x.X -= y.X;
            x.Y -= y.Y;
            return x;
        }

        public static Point operator +(Point x, VelocityPoint y)
        {
            unchecked
            {
                x.X = (byte)(x.X + y.X);
                x.Y = (byte)(x.Y + y.Y);
            }
            return x;
        }

        public static Point operator -(Point x, VelocityPoint y)
        {
            unchecked
            {
                x.X = (byte)(x.X - y.X);
                x.Y = (byte)(x.Y - y.Y);
            }
            return x;
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }
}
