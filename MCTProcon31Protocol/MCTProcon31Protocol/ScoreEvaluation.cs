using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon31Protocol
{
    public static class ScoreEvaluation
    {
        public static int EvaluateGameScore(ColoredBoardNormalSmaller meBoard, ColoredBoardNormalSmaller enemyBoard, sbyte[,] score)
        {
            ColoredBoardNormalSmaller colored = new ColoredBoardNormalSmaller((uint)score.GetLength(0), (uint)score.GetLength(1));
            int result = 0;

            for (uint x = 0; x < score.GetLength(0); ++x)
                for (uint y = 0; y < score.GetLength(1); ++y)
                {
                    if (meBoard[x, y])
                        result += score[x, y];
                    colored[x, y] = meBoard[x, y] || enemyBoard[x, y];
                }


            BadSpaceFill(ref meBoard, enemyBoard, (byte)score.GetLength(0), (byte)score.GetLength(1));
            BadSpaceFill(ref enemyBoard, meBoard, (byte)score.GetLength(0), (byte)score.GetLength(1), false);

            for (uint x = 0; x < score.GetLength(0); ++x)
                for (uint y = 0; y < score.GetLength(1); ++y)
                    if (!meBoard[x, y] && enemyBoard[x, y] && !colored[x, y])
                        result += Math.Abs(score[x, y]);
            return result;
        }

        /*
        public static int EvaluateFilledScore(in ColoredBoardNormalSmaller board, sbyte[,] score)
        {
            int result = 0;
            for (uint x = 0; x < score.GetLength(0); ++x)
                for (uint y = 0; y < score.GetLength(1); ++y)
                    if (board[x, y]) result += score[x, y];
            return result;
        }

        public static int EvaluateSurroundedScore(ref ColoredBoardNormalSmaller board, sbyte[,] score)
        {
            BadSpaceFill(ref board, (byte)score.GetLength(0), (byte)score.GetLength(1));
            int result = 0;
            for (uint x = 0; x < score.GetLength(0); ++x)
                for (uint y = 0; y < score.GetLength(1); ++y)
                    if (!board[x, y]) result += Math.Abs(score[x, y]);
            return result;
        }*/

        public static unsafe void BadSpaceFill(
            ref ColoredBoardNormalSmaller Checker, in ColoredBoardNormalSmaller enemyChecker, byte width, byte height, bool isFirst = true
            )
        {
            unchecked
            {
                int* DistanceX = stackalloc[] { 0, 1, 0, -1, 1, 1, -1, -1 };
                int* DistanceY = stackalloc[] { 1, 0, -1, 0, -1, 1, 1, -1 };
                Point* myStack = stackalloc Point[24 * 24];

                Point point;
                byte x, y, searchTo = 0, searchToX, searchToY, myStackSize = 0;

                if (isFirst)
                {
                    searchTo = (byte)(height - 1);
                    for (x = 0; x < width; x++)
                    {
                        if (!Checker[x, 0])
                        {
                            myStack[myStackSize++] = new Point(x, 0);
                            Checker[x, 0] = true;
                        }
                        if (!Checker[x, searchTo])
                        {
                            myStack[myStackSize++] = new Point(x, searchTo);
                            Checker[x, searchTo] = true;
                        }
                    }

                    searchTo = (byte)(width - 1);
                    for (y = 0; y < height; y++)
                    {
                        if (!Checker[0, y])
                        {
                            myStack[myStackSize++] = new Point(0, y);
                            Checker[0, y] = true;
                        }
                        if (!Checker[searchTo, y])
                        {
                            myStack[myStackSize++] = new Point(searchTo, y);
                            Checker[searchTo, y] = true;
                        }
                    }
                }
                else
                {
                    for (x = 0; x < width; x++)
                        for (y = 0; y < height; y++)
                        {
                            if (enemyChecker[x, y])
                            {
                                myStack[myStackSize++] = new Point(x, y);
                                Checker[x, y] = true;
                            }
                        }
                }
                
                while (myStackSize > 0)
                {
                    point = myStack[--myStackSize];
                    x = point.X;
                    y = point.Y;

                    for (int i = 0; i < 8; i++)
                    {
                        searchToX = (byte)(x + DistanceX[i]);
                        searchToY = (byte)(y + DistanceY[i]);
                        if (searchToX < width && searchToY < height && !Checker[searchToX, searchToY])
                        {
                            myStack[myStackSize++] = new Point(searchToX, searchToY);
                            Checker[searchToX, searchToY] = true;
                        }
                    }
                }
            }
        }
    }
}
