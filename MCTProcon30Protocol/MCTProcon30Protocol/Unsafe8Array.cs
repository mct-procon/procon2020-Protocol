﻿using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MCTProcon30Protocol
{
    [MessagePackObject]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Unsafe8Array<T> where T : unmanaged
    {
        [Key(0)]
        public T Agent1;
        [Key(1)]
        public T Agent2;
        [Key(2)]
        public T Agent3;
        [Key(3)]
        public T Agent4;
        [Key(4)]
        public T Agent5;
        [Key(5)]
        public T Agent6;
        [Key(6)]
        public T Agent7;
        [Key(7)]
        public T Agent8;
        [Key(8)]
        public T Agent9;
        [Key(9)]
        public T Agent10;
        [Key(10)]
        public T Agent11;
        [Key(11)]
        public T Agent12;
        [Key(12)]
        public T Agent13;
        [Key(13)]
        public T Agent14;
        [Key(14)]
        public T Agent15;
        [Key(15)]
        public T Agent16;

        [IgnoreMember]
        public unsafe T this[int index] {
            get {
#if DEBUG
                if ((uint)index >= 16) throw new IndexOutOfRangeException();
#endif
                T* ary = (T*)Unsafe.AsPointer(ref this);
                return ary[index];
            }
            set {
#if DEBUG
                if ((uint)index >= 16) throw new IndexOutOfRangeException();
#endif
                T* ary = (T*)Unsafe.AsPointer(ref this);
                ary[index] = value;
            }
        }

        public static Unsafe8Array<T1> Create<T1>(IReadOnlyList<T1> agents) where T1 : unmanaged {
#if DEBUG
            if ((uint)(agents.Count - 1) > 15) throw new ArgumentOutOfRangeException();
#endif
            Unsafe8Array<T1> result = new Unsafe8Array<T1>();
            int i = 0;
            for (; i < agents.Count; ++i)
                result[i] = agents[i];
            return result;
        }

        public static Unsafe8Array<T1> Create<T1>(params T1[] agents) where T1 : unmanaged
        {
#if DEBUG
            if ((uint)(agents.Length - 1) > 15) throw new ArgumentOutOfRangeException();
#endif
            Unsafe8Array<T1> result = new Unsafe8Array<T1>();
            int i = 0;
            for (; i < agents.Length; ++i)
                result[i] = agents[i];
            return result;
        }

        public ulong LongGetHashCode()
        {
            int count = sizeof(T);
            ulong succ = 0;
            ulong* ary = (ulong*)Unsafe.AsPointer(ref this);
            for (int i = 0; i < count; ++i)
                unchecked { succ += *ary; ary++; }
            return succ;
        }

        public override int GetHashCode()
        {
            ulong r = this.LongGetHashCode();
            return unchecked((int)((uint)(r >> 32) ^ (uint)r));
        }

        public static unsafe bool Equals(Unsafe8Array<Point> x, Unsafe8Array<Point> y, int size)
            => EqualsBase((ulong*)Unsafe.AsPointer(ref x), (ulong*)Unsafe.AsPointer(ref y), size);

        public static unsafe bool Equals(Unsafe8Array<VelocityPoint> x, Unsafe8Array<VelocityPoint> y, int size)
            => EqualsBase((ulong*)Unsafe.AsPointer(ref x), (ulong*)Unsafe.AsPointer(ref y), size);

        public static unsafe bool EqualsBase(ulong* x, ulong* y, int size)
        {
            while (size > 4)
            {
                if (*x != *y) return false;
                x++;
                y++;
                size -= 4;
            }
            ulong mask = ulong.MaxValue >> ((4-size) * (8 * 2));
            if ((*x & mask) != (*y & mask)) return false;
            return true;
        }

        public Unsafe8ArrayEnumerable<T> GetEnumerable(int Count) => new Unsafe8ArrayEnumerable<T>(this, Count);
    }
}
