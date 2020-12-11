﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MCTProcon31Protocol.Methods;

namespace MCTProcon31Protocol.AIFramework
{
    public abstract class AIBase : IIPCClientReader
    {
        private IPCManager ipc;
        protected CancellationTokenSource Canceller;
        protected CancellationToken CancellationToken;

        protected Decided SolverResultList = new Decided();
        protected Decision SolverResult {
            get {
                if (SolverResultList == null) return null;
                if (SolverResultList.Count == 0) return null;
                return SolverResultList[0];
            }
            set {
                if (SolverResultList == null) SolverResultList = new Decided();
                if (SolverResultList.Count == 0) SolverResultList.Add(value);
                else SolverResultList[0] = value;
            }
        }

        protected Task SolverTask;

        private System.Timers.Timer timer;

        public bool IsWriteLog { get; set; } = false;
        public bool IsWriteBoard { get; set; } = false;
        public bool IsAutoSend { get; set; } = true;

        public sbyte[,] ScoreBoard { get; set; }
        public Unsafe16Array<Point> MyAgents { get; set; }
        public Unsafe16Array<bool> IsAgentsMoved { get; set; }
        public Unsafe16Array<Point> EnemyAgents { get; set; }

        public Unsafe16Array<AgentState> MyAgentsState { get; set; }
        public Unsafe16Array<AgentState> EnemyAgentsState { get; set; }

        public ColoredBoardNormalSmaller MyBoard { get; set; }
        public ColoredBoardNormalSmaller EnemyBoard { get; set; }
        public ColoredBoardNormalSmaller MySurroundedBoard { get; set; }
        public ColoredBoardNormalSmaller EnemySurroundedBoard { get; set; }
        public int AgentsCount { get; set; }

        public int CurrentTurn { get; set; }
        public int TurnCount { get; set; }

        public bool IsEnableTimer { get; set; } = true;

        private volatile bool SendingFinished = false;

        public object LogSyncRoot = new object();

        private ManualResetEventSlim SynchronizeStopper = new ManualResetEventSlim(false);

        public AIBase()
        {
            ipc = new IPCManager(this);
            timer = new System.Timers.Timer();
            timer.Elapsed += this.timerElasped;
            timer.AutoReset = false;
        }

        private void timerElasped(object sender, EventArgs args)
        {
            this.timer.Enabled = false;
            EndSolve();
        }

        public virtual void StartSync(int port, bool isWriteLog = false, bool isWriteBoard = false)
        {
#pragma warning disable CS4014
            Start(port, isWriteLog, isWriteBoard);
#pragma warning restore CS4014
            SynchronizeStopper.Wait();
        }
        public virtual async Task Start(int port, bool isWriteLog = false, bool isWriteBoard = false, bool isAutoSend = true)
        {
            SynchronizeStopper.Reset();
            IsWriteLog = isWriteLog;
            IsWriteBoard = isWriteBoard;
            await ipc.Connect(port);
            var proc = System.Diagnostics.Process.GetCurrentProcess();
            ipc.Write(DataKind.Connect, new Connect(ProgramKind.AI) { ProcessId = proc.Id });
            proc.Dispose();
            Log("[IPC] Sended Connect");
            await ipc.StartAsync();
        }

        public virtual void End()
        {
            ipc?.Shutdown();
            SynchronizeStopper.Set();
        }

        public void OnGameInit(GameInit init)
        {
            Log("[IPC] Receive GameInit");

            ScoreBoard = init.Board;
            TurnCount = init.Turns;
            AgentsCount = init.AgentsCount;
        }

        public void OnTurnStart(TurnStart turn)
        {
            MyBoard = turn.MyColoredBoard;
            EnemyBoard = turn.EnemyColoredBoard;
            MyAgents = turn.MyAgents;
            EnemyAgents = turn.EnemyAgents;
            IsAgentsMoved = turn.Turn == 0 ? Unsafe16Array.Create(Enumerable.Range(0, 16).Select(x => true).ToArray()) : turn.IsAgentsMoved;
            CurrentTurn = turn.Turn;
            MySurroundedBoard = turn.MySurroundedBoard;
            EnemySurroundedBoard = turn.EnemySurroundedBoard;
            MyAgentsState = turn.MyAgentsState;
            EnemyAgentsState = turn.EnemyAgentsState;
            SendingFinished = false;

            Log("[IPC] Receive TurnStart turn = {0}", turn.Turn);
            DumpBoard(turn.MyColoredBoard, turn.EnemyColoredBoard, turn.MySurroundedBoard, turn.EnemySurroundedBoard, AgentsCount, MyAgents, EnemyAgents, MyAgentsState, EnemyAgentsState);

            StartSolve();
            if (IsEnableTimer)
            {
                timer.Interval = CalculateTimerMiliSconds(turn.WaitMiliSeconds);
                timer.Enabled = true;
            }
        }

        public void OnTurnEnd(TurnEnd turn)
        {
            if (IsWriteLog)
                Console.WriteLine("[IPC] Receive TurnEnd");
            Canceller?.Cancel();
        }

        public void Log(string str)
        {
            if (!IsWriteLog) return;
            lock (LogSyncRoot)
                Console.WriteLine(str);
        }

        public void Log(string format, params object[] objects)
        {
            if (!IsWriteLog) return;
            lock (LogSyncRoot)
                Console.WriteLine(format, objects);
        }

        public virtual void OnGameEnd(GameEnd end)
        {
            Log("[IPC] Received GameEnd");
        }

        public virtual void OnPause(Pause pause)
        {
            Log("[IPC] Received Pause");
        }

        public virtual void OnInterrupt(Interrupt interrupt)
        {
            Log("[IPC] Received Interrupt isError = {0}", interrupt.IsError);
            if (interrupt.IsError)
            {
                //DoSomething.
            }

            End();
        }

        public void OnRebaseByUser(RebaseByUser rebase)
        {
            Log("[IPC] Received Rebase By User");
        }

        public void OnRequestAnswer(RequestAnswer requestAnswer)
        {
            Log("[IPC] Received RequestAnswer");
            SendingFinished = false;
            EndSolve();
        }

        private void StartSolve()
        {
            Canceller = new CancellationTokenSource();
            CancellationToken = Canceller.Token;
            SolverResultList.Clear();
            SolverTask = Task.Run((Action)Solve, CancellationToken);
            SolverTask.ContinueWith(ContinuationAction);
            Log("[SOLVER] Solver Started.");
        }

        private void ContinuationAction(Task prevTask) {
            if (SendingFinished) return;
            if (!prevTask.IsCompleted || prevTask.IsCanceled) return;
            Log("[SOLVER] Solver Finished.");
            if (SolverTask.IsFaulted)
            {
                lock (LogSyncRoot)
                {
                    Console.WriteLine("[SOLVER] An exception is thrown.====");
                    Console.WriteLine(SolverTask.Exception);
                    Console.WriteLine("======");
                }
            }
            else
            {
                Log("[SOLVER] State is {0}", SolverTask.Status);
            }
            SendDecided();
        }

        protected virtual void DumpBoard(in ColoredBoardNormalSmaller MyPaintedBoard, in ColoredBoardNormalSmaller EnemyPaintedBoard, in ColoredBoardNormalSmaller MySurroundedBoard, in ColoredBoardNormalSmaller EnemySurroundedBoard, int AgentsCount, Unsafe16Array<Point> MyAgents, Unsafe16Array<Point> EnemyAgents, Unsafe16Array<AgentState> MyAgentsState, Unsafe16Array<AgentState> EnemyAgentsState)
        {
            if (!IsWriteBoard) return;
            lock (LogSyncRoot)
            {
                for (uint y = 0; y < ScoreBoard.GetLength(1); ++y)
                {
                    for (uint x = 0; x < ScoreBoard.GetLength(0); ++x)
                    {
                        int pack = Point.Pack((byte)x, (byte)y);
                        Console.ForegroundColor = ConsoleColor.White;
                        for (int i = 0; i < AgentsCount; ++i)
                        {
                            if(MyAgentsState[i] != AgentState.NonPlaced && MyAgents[i].GetHashCode() == pack)
                            {
                                Console.BackgroundColor = ConsoleColor.Red;
                                goto writeChr;
                            }
                            if (EnemyAgentsState[i] != AgentState.NonPlaced && EnemyAgents[i].GetHashCode() == pack)
                            {
                                Console.BackgroundColor = ConsoleColor.Blue;
                                goto writeChr;
                            }
                        }
                        if (MyPaintedBoard[x, y])
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                        else if (EnemyPaintedBoard[x, y])
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                        else if (MySurroundedBoard[x, y])
                            Console.BackgroundColor = ConsoleColor.Magenta;
                        else if (EnemySurroundedBoard[x, y])
                            Console.BackgroundColor = ConsoleColor.Cyan;
                        else if (((x + y) & 1) == 0)
                            Console.BackgroundColor = ConsoleColor.Black;
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;
                        }
                        writeChr:
                        string str = ScoreBoard[x, y].ToString();
                        if (str.Length != 3)
                            Console.Write(new string(' ', 3 - str.Length));
                        Console.Write(str);
                    }
                    Console.WriteLine();
                }
            }
        }

        protected abstract void EndGame(GameEnd end);
        protected abstract void Solve();
        protected virtual int CalculateTimerMiliSconds(int miliseconds) => miliseconds - 1500;
        protected virtual void EndSolve()
        {
            if (SendingFinished) return;
            if (SolverTask.IsFaulted)
            {
                lock (LogSyncRoot)
                {
                    Console.WriteLine("[SOLVER] An exception is thrown.====");
                    Console.WriteLine(SolverTask.Exception);
                    Console.WriteLine("======");
                }
            }
            else
            {
                Log("[SOLVER] State is {0}", SolverTask.Status);
                if (SolverTask.IsCompleted)
                    Canceller?.Dispose();
                else
                    Canceller?.Cancel();
                SendDecided();
            }
            Log("[SOLVER] Thinking Stop.");
        }


        protected virtual void SendDecided()
        {
            SendingFinished = true;
            if (SolverResult != null)
            {
                ipc.Write<Methods.Decided>(DataKind.Decided, SolverResultList);
                Log("[IPC] Decided Sended");
            }
            else
                Log("[SOLVER] Decision is NULL!!\n[IPC] Decided Sending Failed");
        }
    }
}
