using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;
using System.Numerics;
using System.Runtime.InteropServices;

namespace MCTProcon30Protocol
{
    /// <summary>
    /// x-y平面座標での移動量を示す構造体
    /// </summary>
    [MessagePackObject]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VelocityPoint
    {
        /// <summary>
        /// x座標の移動量
        /// </summary>
        [Key(0)]
        public sbyte X { get; set; }

        /// <summary>
        /// y座標の移動量
        /// </summary>
        [Key(1)]
        public sbyte Y { get; set; }

        public VelocityPoint(sbyte x, sbyte y)
        {
            X = x; Y = y;
        }

        public override int GetHashCode()
            => (int)(byte)X | ((byte)Y << 8);

        public override bool Equals(object obj)
        {
            if (obj is VelocityPoint)
                return this == (VelocityPoint)obj;
            else
                return false;
        }

        public static bool operator ==(VelocityPoint x, VelocityPoint y) => x.GetHashCode() == y.GetHashCode();
        public static bool operator !=(VelocityPoint x, VelocityPoint y) => x.X != y.X || x.Y != y.Y;

        public static VelocityPoint operator +(VelocityPoint x, (sbyte x, sbyte y) y)
        {
            unchecked
            {
                x.X = (sbyte)(x.X + y.x);
                x.Y = (sbyte)(x.Y + y.y);
            }
            return x;
        }

        public static VelocityPoint operator -(VelocityPoint x, (sbyte x, sbyte y) y)
        {
            unchecked
            {
                x.X = (sbyte)(x.X - y.x);
                x.Y = (sbyte)(x.Y - y.y);
            }
            return x;
        }

        public static implicit operator VelocityPoint((sbyte, sbyte) x) => new VelocityPoint(x.Item1, x.Item2);

        public override string ToString() => $"({X},{Y})";
    }
}
