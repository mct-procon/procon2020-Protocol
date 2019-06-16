using MessagePack;
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

        [IgnoreMember]
        public unsafe T this[int index] {
            get {
#if DEBUG
                if ((uint)index >= 8) throw new IndexOutOfRangeException();
#endif
                T* ary = (T*)Unsafe.AsPointer(ref this);
                return ary[index];
            }
            set {
#if DEBUG
                if ((uint)index >= 8) throw new IndexOutOfRangeException();
#endif
                T* ary = (T*)Unsafe.AsPointer(ref this);
                ary[index] = value;
            }
        }

        public static Unsafe8Array<T1> Create<T1>(IReadOnlyList<T1> agents) where T1 : unmanaged {
#if DEBUG
            if ((uint)(agents.Count - 1) > 7) throw new ArgumentOutOfRangeException();
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
            if ((uint)(agents.Length - 1) > 7) throw new ArgumentOutOfRangeException();
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
    }
}
