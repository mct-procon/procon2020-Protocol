using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon30Protocol.Methods
{
    [MessagePackObject]
    public class TurnStart
    {
        [Key(0)]
        public byte Turn { get; set; }

        [Key(1)]
        public int WaitMiliSeconds { get; set; }

        [Key(2)]
        public Unsafe8Array<Point> MyAgents { get; set; }

        [Key(3)]
        public Unsafe8Array<Point> EnemyAgents { get; set; }

        [Key(5)]
        public ColoredBoardSmallBigger MeColoredBoard { get; set; }

        [Key(6)]
        public ColoredBoardSmallBigger EnemyColoredBoard { get; set; }

        [Key(7)]
        public bool IsAgent1Moved { get; set; }

        [Key(8)]
        public bool IsAgent2Moved { get; set; }

        public TurnStart(byte turn, int waitMiliSecs, in Unsafe8Array<Point> myAgents, in Unsafe8Array<Point> enemyAgents, in ColoredBoardSmallBigger meColoredBoard, in ColoredBoardSmallBigger enemyColoredBoard, bool isAgent1Moved, bool isAgent2Moved)
        {
            Turn = turn;
            WaitMiliSeconds = waitMiliSecs;
            MyAgents = myAgents;
            EnemyAgents = enemyAgents;
            MeColoredBoard = meColoredBoard;
            EnemyColoredBoard = enemyColoredBoard;
            IsAgent1Moved = isAgent1Moved;
            IsAgent2Moved = isAgent2Moved;
        }

        // DO NOT ERASE
        public TurnStart() { }
    }
}
