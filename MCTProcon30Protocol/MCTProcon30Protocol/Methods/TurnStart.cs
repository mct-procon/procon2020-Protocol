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
        public ColoredBoardNormalSmaller MeColoredBoard { get; set; }

        [Key(6)]
        public ColoredBoardNormalSmaller EnemyColoredBoard { get; set; }

        [Key(7)]
        public Unsafe8Array<bool> IsAgentsMoved { get; set; }

        public TurnStart(byte turn, int waitMiliSecs, in Unsafe8Array<Point> myAgents, in Unsafe8Array<Point> enemyAgents, in ColoredBoardNormalSmaller meColoredBoard, in ColoredBoardNormalSmaller enemyColoredBoard, in Unsafe8Array<bool> isAgentsMoved)
        {
            Turn = turn;
            WaitMiliSeconds = waitMiliSecs;
            MyAgents = myAgents;
            EnemyAgents = enemyAgents;
            MeColoredBoard = meColoredBoard;
            EnemyColoredBoard = enemyColoredBoard;
            IsAgentsMoved = isAgentsMoved;
        }

        // DO NOT ERASE
        public TurnStart() { }
    }
}
