using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MCTProcon30Protocol
{
    public unsafe class Unsafe8ArrayEnumerator<T> : IEnumerator<T> where T : unmanaged
    {
        public T Current => Unsafe.AsRef<Unsafe8Array<T>>((void *)parent)[currentIndex];

        object IEnumerator.Current => Unsafe.AsRef<Unsafe8Array<T>>((void*)parent)[currentIndex];

        private IntPtr parent;
        private int count;
        private int currentIndex;

        public Unsafe8ArrayEnumerator(IntPtr ary, int Count)
        {
            parent = ary;
            count = Count;
            currentIndex = 0;
        }

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
