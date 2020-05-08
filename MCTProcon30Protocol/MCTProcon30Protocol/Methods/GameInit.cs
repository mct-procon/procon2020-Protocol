using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon30Protocol.Methods
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
        public Unsafe16Array<Point> MyAgents { get; set; }

        [Key(5)]
        public Unsafe16Array<Point> EnemyAgents { get; set; }

        [Key(6)]
        public byte Turns { get; set; }

        public GameInit(byte height, byte width, sbyte[,] board, byte agentsCount, in Unsafe16Array<Point> myAgents, in Unsafe16Array<Point> enemyAgents, byte turns)
        {
            BoardHeight = height;
            BoardWidth = width;
            Board = board;
            AgentsCount = agentsCount;
            MyAgents = myAgents;
            EnemyAgents = enemyAgents;
            Turns = turns;
        }

        // DO NOT ERASE
        public GameInit() { }
    }
}
