using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon31Protocol.Methods
{
    [MessagePackObject]
    public class GameInit
    {
        [Key(0)]
        public byte BoardHeight { get; set; }

        [Key(1)]
        public byte BoardWidth { get; set; }

        [Key(2)]
        public sbyte[,] Board { get; set; }

        [Key(3)]
        public byte AgentsCount { get; set; }

        [Key(4)]
        public byte Turns { get; set; }

        public GameInit(byte height, byte width, sbyte[,] board, byte agentsCount, byte turns)
        {
            BoardHeight = height;
            BoardWidth = width;
            Board = board;
            AgentsCount = agentsCount;
            Turns = turns;
        }

        // DO NOT ERASE
        public GameInit() { }
    }
}
