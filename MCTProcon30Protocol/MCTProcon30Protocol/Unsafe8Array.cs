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
        public T Agent1;
        public T Agent2;
        public T Agent3;
        public T Agent4;
        public T Agent5;
        public T Agent6;
        public T Agent7;
        public T Agent8;
        public uint Count {
            get; private set;
        }

        public unsafe T this[int index] {
            get {
#if DEBUG
                if (Count >= (uint)index) throw new IndexOutOfRangeException();
#endif
                T* ary = (T*)Unsafe.AsPointer(ref this);
                return ary[index];
            }
            set {
#if DEBUG
                if (Count >= (uint)index) throw new IndexOutOfRangeException();
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
            result.Count = (uint)agents.Count;
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
            result.Count = (uint)agents.Length;
            int i = 0;
            for (; i < agents.Length; ++i)
                result[i] = agents[i];
            return result;
        }
    }
}
