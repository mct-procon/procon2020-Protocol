using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MCTProcon30Protocol.Methods;

namespace MCTProcon30Protocol.AIFramework
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

        public sbyte[,] ScoreBoard { get; set; }
        public Unsafe8Array<Point> MyAgents { get; set; }
        public Unsafe8Array<bool> IsAgentsMoved { get; set; }
        public Unsafe8Array<Point> EnemyAgents { get; set; }


        public ColoredBoardNormalSmaller MyBoard { get; set; }
        public ColoredBoardNormalSmaller EnemyBoard { get; set; }

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
            timer.Elapsed += this.EndSolve;
            timer.AutoReset = false;
        }

        public virtual void StartSync(int port, bool isWriteLog = false, bool isWriteBoard = false)
        {
#pragma warning disable CS4014
            Start(port, isWriteLog, isWriteBoard);
#pragma warning restore CS4014
            SynchronizeStopper.Wait();
        }
        public virtual async Task Start(int port, bool isWriteLog = false, bool isWriteBoard = false)
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
            MyAgents = init.MyAgents;
            EnemyAgents = init.EnemyAgents;
            TurnCount = init.Turns;
            AgentsCount = init.AgentsCount;
        }

        public void OnTurnStart(TurnStart turn)
        {
            MyBoard = turn.MeColoredBoard;
            EnemyBoard = turn.EnemyColoredBoard;
            MyAgents = turn.MyAgents;
            EnemyAgents = turn.EnemyAgents;
            IsAgentsMoved = turn.IsAgentsMoved;
            CurrentTurn = turn.Turn;
            SendingFinished = false;

            Log("[IPC] Receive TurnStart turn = {0}", turn.Turn);
            DumpBoard(turn.MeColoredBoard, turn.EnemyColoredBoard, AgentsCount, MyAgents, EnemyAgents);

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

        protected virtual void DumpBoard(in ColoredBoardNormalSmaller MyBoard, in ColoredBoardNormalSmaller EnemyBoard, int AgentsCount, Unsafe8Array<Point> MyAgents, Unsafe8Array<Point> EnemyAgents )
        {
            if (!IsWriteBoard) return;
            lock (LogSyncRoot)
            {
                for (uint y = 0; y < ScoreBoard.GetLength(1); ++y)
                {
                    for (uint x = 0; x < ScoreBoard.GetLength(0); ++x)
                    {
                        int pack = Point.Pack((byte)x, (byte)y);
                        bool flag = false;
                        Console.ForegroundColor = ConsoleColor.White;
                        for (int i = 0; i < AgentsCount; ++i)
                        {
                            if(MyAgents[i].GetHashCode() == pack)
                            {
                                Console.BackgroundColor = ConsoleColor.Red;
                                flag = true;
                                break;
                            }
                            if (EnemyAgents[i].GetHashCode() == pack)
                            {
                                Console.BackgroundColor = ConsoleColor.Blue;
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            if (MyBoard[x, y])
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                            else if (EnemyBoard[x, y])
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                            else if (((x + y) & 1) == 0)
                                Console.BackgroundColor = ConsoleColor.Black;
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.BackgroundColor = ConsoleColor.White;
                            }
                        }
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
        protected virtual void EndSolve(object sender, EventArgs e)
        {
            timer.Enabled = false;
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
