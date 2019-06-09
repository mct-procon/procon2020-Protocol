using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace MCTProcon30Protocol
{
    [MessagePackObject]
    public unsafe struct Unsafe4Array<T> where T : unmanaged
    {
        public T Agent1;
        public T Agent2;
        public T Agent3;
        public T Agent4;
        public readonly uint Count;

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

        public Unsafe4Array(IReadOnlyList<T> agents) {
#if DEBUG
            if ((uint)(agents.Count - 1) > 3) throw new ArgumentOutOfRangeException();
#endif
            Count = (uint)agents.Count;
            Agent1 = agents[0];
            if (agents.Count > 1)
            {
                Agent2 = agents[1];
                if (agents.Count > 2)
                {
                    Agent3 = agents[2];
                    if (agents.Count > 3)
                    {
                        Agent4 = agents[3];
                    }
                    else
                    {
                        Agent4 = new T();
                    }
                }
                else
                {
                    Agent3 = Agent4 = new T();
                }
            }
            else
            {
                Agent2 = Agent3 = Agent4 = new T();
            }
        }

        public Unsafe4Array(params T[] agents) {
#if DEBUG
            if((uint)(agents.Length - 1) > 3 ) throw new ArgumentOutOfRangeException();
#endif
            Count = (uint)agents.Length;
            Agent1 = agents[0];
            if(agents.Length > 1)
            {
                Agent2 = agents[1];
                if(agents.Length > 2)
                {
                    Agent3 = agents[2];
                    if(agents.Length > 3)
                    {
                        Agent4 = agents[3];
                    }
                    else
                    {
                        Agent4 = new T();
                    }
                }
                else
                {
                    Agent3 = Agent4 = new T();
                }
            }
            else
            {
                Agent2 = Agent3 = Agent4 = new T();
            }
        }
    }
}
