using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MCTProcon30Protocol
{
    public unsafe class Unsafe8ArrayEnumerator<T> : IEnumerator<T> where T : unmanaged
    {
        public T Current => parent[currentIndex];

        object IEnumerator.Current => parent[currentIndex];

        private Unsafe8Array<T> parent;
        private int count;
        private int currentIndex;

        internal Unsafe8ArrayEnumerator(Unsafe8Array<T> ary, int Count)
        {
            parent = ary;
            count = Count;
            currentIndex = 0;
        }

        private Unsafe8ArrayEnumerator() { }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            currentIndex++;
            if (count >= currentIndex)
            {
                currentIndex--;
                return false;
            }
            return true;
        }

        public void Reset()
        {
            currentIndex = 0;
        }
    }
}
