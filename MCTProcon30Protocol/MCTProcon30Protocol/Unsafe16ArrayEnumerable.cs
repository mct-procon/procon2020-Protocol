using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon30Protocol
{
    public unsafe class Unsafe16ArrayEnumerable<T> : IEnumerable<T> where T : unmanaged
    {
        private Unsafe16Array<T> parent;
        private int count;

        private Unsafe16ArrayEnumerable() { }
        internal Unsafe16ArrayEnumerable(Unsafe16Array<T> ary, int Count)
        {
            parent = ary;
            count = Count;
        }

        public IEnumerator<T> GetEnumerator() => new Unsafe16ArrayEnumerator<T>(parent, count);
        IEnumerator IEnumerable.GetEnumerator() => new Unsafe16ArrayEnumerator<T>(parent, count);
    }
}
