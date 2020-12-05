using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon31Protocol.Methods
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
        public ColoredBoardNormalSmaller MyColoredBoard { get; set; }

        [Key(6)]
        public ColoredBoardNormalSmaller EnemyColoredBoard { get; set; }

        [Key(7)]
        public int LimitationTime { get; set; }

        [Key(8)]
        public byte AgentsCount { get; set; }

        [Key(9)]
        public ColoredBoardNormalSmaller MySurroundedBoard { get; set; }

        [Key(10)]
        public ColoredBoardNormalSmaller EnemySurroundedBoard { get; set; }

        public Resume(byte turn, byte currentTurn, int boardHeight, int boardWidth, sbyte[,] board, ColoredBoardNormalSmaller myColoredBoard, ColoredBoardNormalSmaller enemyColoredBoard, byte agentsCount, ColoredBoardNormalSmaller mySurroundedBoard, ColoredBoardNormalSmaller enemySurroundedBoard)
        {
            Turns = turn;
            CurrentTurn = currentTurn;
            BoardHeight = boardHeight;
            BoardWidth = boardWidth;
            Board = board;
            MyColoredBoard = myColoredBoard;
            EnemyColoredBoard = enemyColoredBoard;
            AgentsCount = agentsCount;
            MySurroundedBoard = mySurroundedBoard;
            EnemySurroundedBoard = enemySurroundedBoard;
        }

        // DO NOT ERASE
        public Resume() { }
    }
}
