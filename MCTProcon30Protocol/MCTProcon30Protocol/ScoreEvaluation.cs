using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon30Protocol
{
    public static class ScoreEvaluation
    {
        public static int EvaluateGameScore(ColoredBoardNormalSmaller board, sbyte[,] score)
        {
            int result = 0;

            for (uint x = 0; x < score.GetLength(0); ++x)
                for (uint y = 0; y < score.GetLength(1); ++y)
                    if (board[x,y])
                        result += score[x, y];

            BadSpaceFill(ref board, (byte)score.GetLength(0), (byte)score.GetLength(1));

            for (uint x = 0; x < score.GetLength(0); ++x)
                for (uint y = 0; y < score.GetLength(1); ++y)
                    if (!board[x, y])
                        result += Math.Abs(score[x, y]);
            return result;
        }

        public static int EvaluateFilledScore(in ColoredBoardNormalSmaller board, sbyte[,] score)
        {
            int result = 0;
            for (uint x = 0; x < score.GetLength(0); ++x)
                for (uint y = 0; y < score.GetLength(1); ++y)
                    if(board[x, y]) result += score[x, y];
            return result;
        }

        public static int EvaluateSurroundedScore(ref ColoredBoardNormalSmaller board, sbyte[,] score)
        {
            BadSpaceFill(ref board, (byte)score.GetLength(0), (byte)score.GetLength(1));
            int result = 0;
            for (uint x = 0; x < score.GetLength(0); ++x)
                for (uint y = 0; y < score.GetLength(1); ++y)
                    if (!board[x, y]) result += Math.Abs(score[x,y]);
            return result;
        }

        public static unsafe void BadSpaceFill(ref ColoredBoardNormalSmaller Checker, byte width, byte height)
        {
            unchecked
            {
                MCTProcon30Protocol.Point* myStack = stackalloc MCTProcon30Protocol.Point[20 * 20];

                MCTProcon30Protocol.Point point;
                byte x, y, searchTo = 0, myStackSize = 0;

                searchTo = (byte)(height - 1);
                for (x = 0; x < width; x++)
                {
                    if (!Checker[x, 0])
                    {
                        myStack[myStackSize++] = new MCTProcon30Protocol.Point(x, 0);
                        Checker[x, 0] = true;
                    }
                    if (!Checker[x, searchTo])
                    {
                        myStack[myStackSize++] = new MCTProcon30Protocol.Point(x, searchTo);
                        Checker[x, searchTo] = true;
                    }
                }

                searchTo = (byte)(width - 1);
                for (y = 0; y < height; y++)
                {
                    if (!Checker[0, y])
                    {
                        myStack[myStackSize++] = new MCTProcon30Protocol.Point(0, y);
                        Checker[0, y] = true;
                    }
                    if (!Checker[searchTo, y])
                    {
                        myStack[myStackSize++] = new MCTProcon30Protocol.Point(searchTo, y);
                        Checker[searchTo, y] = true;
                    }
                }

                while (myStackSize > 0)
                {
                    point = myStack[--myStackSize];
                    x = point.X;
                    y = point.Y;

                    //左方向
                    searchTo = (byte)(x - 1);
                    if (searchTo < width && !Checker[searchTo, y])
                    {
                        myStack[myStackSize++] = new MCTProcon30Protocol.Point(searchTo, y);
                        Checker[searchTo, y] = true;
                    }

                    //下方向
                    searchTo = (byte)(y + 1);
                    if (searchTo < height && !Checker[x, searchTo])
                    {
                        myStack[myStackSize++] = new MCTProcon30Protocol.Point(x, searchTo);
                        Checker[x, searchTo] = true;
                    }

                    //右方向
                    searchTo = (byte)(x + 1);
                    if (searchTo < width && !Checker[searchTo, y])
                    {
                        myStack[myStackSize++] = new MCTProcon30Protocol.Point(searchTo, y);
                        Checker[searchTo, y] = true;
                    }

                    //上方向
                    searchTo = (byte)(y - 1);
                    if (searchTo < height && !Checker[x, searchTo])
                    {
                        myStack[myStackSize++] = new MCTProcon30Protocol.Point(x, searchTo);
                        Checker[x, searchTo] = true;
                    }
                }
            }
        }
    }
}
