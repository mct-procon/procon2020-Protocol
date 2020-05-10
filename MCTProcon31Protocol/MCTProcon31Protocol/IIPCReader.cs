using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon31Protocol
{
    public interface IIPCClientReader
    {
        void OnGameInit(Methods.GameInit init);
        void OnTurnStart(Methods.TurnStart turn);
        void OnTurnEnd(Methods.TurnEnd turn);
        void OnGameEnd(Methods.GameEnd end);
        void OnPause(Methods.Pause pause);
        void OnInterrupt(Methods.Interrupt interrupt);
        void OnRebaseByUser(Methods.RebaseByUser rebase);
        void OnRequestAnswer(Methods.RequestAnswer requestAnswer);
    }

    public interface IIPCServerReader
    {
        void OnConnect(Methods.Connect connect);
        void OnDecided(Methods.Decided decided);
        void OnInterrupt(Methods.Interrupt interrupt);
        void OnAIProcessExited(IIPCServerReader sender, EventArgs e);
    }
}
