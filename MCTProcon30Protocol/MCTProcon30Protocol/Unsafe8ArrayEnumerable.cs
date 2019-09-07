using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon30Protocol
{
    public unsafe class Unsafe8ArrayEnumerable<T> : IEnumerable<T> where T : unmanaged
    {
        private Unsafe8Array<T> parent;
        private int count;

        private Unsafe8ArrayEnumerable() { }
        internal Unsafe8ArrayEnumerable(Unsafe8Array<T> ary, int Count)
        {
            parent = ary;
            count = Count;
        }

        public IEnumerator<T> GetEnumerator() => new Unsafe8ArrayEnumerator<T>(parent, count);
        IEnumerator IEnumerable.GetEnumerator() => new Unsafe8ArrayEnumerator<T>(parent, count);
    }
}
