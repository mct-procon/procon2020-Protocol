using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon30Protocol.Methods
{
    [Obsolete]
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
        public ColoredBoardNormalSmaller MeColoredBoard { get; set; }

        [Key(6)]
        public ColoredBoardNormalSmaller EnemyColoredBoard { get; set; }

        [Key(7)]
        public int LimitationTime { get; set; }

        [Key(8)]
        public byte AgentsCount { get; set; }

        public Resume(byte turn, byte currentTurn, int boardHeight, int boardWidth, sbyte[,] board, ColoredBoardNormalSmaller meColoredBoard, ColoredBoardNormalSmaller enemyColoredBoard, byte agentsCount)
        {
            Turns = turn;
            CurrentTurn = currentTurn;
            BoardHeight = boardHeight;
            BoardWidth = boardWidth;
            Board = board;
            MeColoredBoard = meColoredBoard;
            EnemyColoredBoard = enemyColoredBoard;
            AgentsCount = agentsCount;
        }

        // DO NOT ERASE
        public Resume() { }
    }
}
