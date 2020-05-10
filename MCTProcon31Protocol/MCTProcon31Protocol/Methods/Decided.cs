using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon31Protocol.Methods
{
    [MessagePackObject]
    public class Decided
    {
        [Key(0)]
        public List<Decision> Data { get; set; }

        [IgnoreMember]
        public Decision this[int index] {
            get => Data[index];
            set => Data[index] = value;
        }

        public Decided()
        {
            Data = new List<Decision>(2);
        }

        public void Clear() => Data.Clear();
        [IgnoreMember]
        public int Count => Data.Count;
        public void Add(Decision data) => Data.Add(data);
        public void Sort() => Data.Sort();
        public void Sort(IComparer<Decision> comp) => Data.Sort(comp);
        public void Sort(Comparison<Decision> comp) => Data.Sort(comp);
    }
}
