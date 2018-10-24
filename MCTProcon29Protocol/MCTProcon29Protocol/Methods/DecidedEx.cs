using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon29Protocol.Methods
{
    [MessagePackObject]
    public class DecidedEx
    {
        [Key(0)]
        public List<Decided> Data { get; set; }

        [IgnoreMember]
        public Decided this[int index] {
            get => Data[index];
            set => Data[index] = value;
        }

        public DecidedEx()
        {
            Data = new List<Decided>(2);
        }

        public void Clear() => Data.Clear();
        [IgnoreMember]
        public int Count => Data.Count;
        public void Add(Decided data) => Data.Add(data);
        public void Sort() => Data.Sort();
        public void Sort(IComparer<Decided> comp) => Data.Sort(comp);
        public void Sort(Comparison<Decided> comp) => Data.Sort(comp);
    }
}
