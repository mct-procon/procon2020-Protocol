using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon29Protocol.Methods
{
    [MessagePackObject]
    public class GameInit
    {
        [Key(0)]
        public byte TeamId { get; set; }

        [Key(1)]
        public byte BoardHeight { get; set; }

        [Key(2)]
        public byte BoardWidth { get; set; }

        [Key(3)]
        public sbyte[,] Board { get; set; }

        [Key(4)]
        public Point InitialPositionAgent1 { get; set; }

        [Key(5)]
        public Point InitialPositionAgent2 { get; set; }

        [Key(6)]
        public byte Turns { get; set; }
    }
}
