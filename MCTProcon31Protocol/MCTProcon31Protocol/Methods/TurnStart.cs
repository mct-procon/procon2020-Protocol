using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon31Protocol.Methods
{
    [MessagePackObject]
    public class TurnStart
    {
        [Key(0)]
        public byte Turn { get; set; }

        [Key(1)]
        public int WaitMiliSeconds { get; set; }

        [Key(2)]
        public Unsafe16Array<Point> MyAgents { get; set; }

        [Key(3)]
        public Unsafe16Array<Point> EnemyAgents { get; set; }

        [Key(5)]
        public ColoredBoardNormalSmaller MyColoredBoard { get; set; }

        [Key(6)]
        public ColoredBoardNormalSmaller EnemyColoredBoard { get; set; }

        [Key(7)]
        public Unsafe16Array<bool> IsAgentsMoved { get; set; }

        [Key(8)]
        public ColoredBoardNormalSmaller MySurroundedBoard { get; set; }

        [Key(9)]
        public ColoredBoardNormalSmaller EnemySurroundedBoard { get; set; }

        [Key(10)]
        public byte MyAgentsCount { get; set; }

        [Key(11)]
        public byte EnemyAgentsCount { get; set; }

        public TurnStart(byte turn, int waitMiliSecs, in Unsafe16Array<Point> myAgents, in Unsafe16Array<Point> enemyAgents, in ColoredBoardNormalSmaller myColoredBoard, in ColoredBoardNormalSmaller enemyColoredBoard, in Unsafe16Array<bool> isAgentsMoved, in ColoredBoardNormalSmaller mySurroundedBoard, in ColoredBoardNormalSmaller enemySurroundedBoard, byte myAgentsCount, byte enemyAgentsCount)
        {
            Turn = turn;
            WaitMiliSeconds = waitMiliSecs;
            MyAgents = myAgents;
            EnemyAgents = enemyAgents;
            MyColoredBoard = myColoredBoard;
            EnemyColoredBoard = enemyColoredBoard;
            IsAgentsMoved = isAgentsMoved;
            MySurroundedBoard = mySurroundedBoard; 
            EnemySurroundedBoard = enemySurroundedBoard;
            MyAgentsCount = myAgentsCount;
            EnemyAgentsCount = enemyAgentsCount;
        }

        // DO NOT ERASE
        public TurnStart() { }
    }
}
