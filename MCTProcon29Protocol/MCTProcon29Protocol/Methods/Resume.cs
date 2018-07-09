using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon29Protocol.Methods
{
    [MessagePackObject]
    public class Resume
    {
        [Key(0)]
        public byte Turns { get; set; }

        [Key(1)]
        public byte CurrentTurn { get; set; }

        [Key(2)]
        public int BoardHeight { get; set; }
        
        [Key(3)]
        public int BoardWidth { get; set; }

        [Key(4)]
        public sbyte[,] Board { get; set; }

        [Key(5)]
        public ushort[] MeColoredMap { get; set; }

        [Key(6)]
        public ushort[] EnemyColoredMap { get; set; }

        [Key(7)]
        public int LimitationTime { get; set; }
    }
}
