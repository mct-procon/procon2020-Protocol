using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon30Protocol
{
    [MessagePackObject]
    public class Decision
    {
        [Key(0)]
        public VelocityPoint MeAgent1 { get; set; }

        [Key(1)]
        public VelocityPoint MeAgent2 { get; set; }

        [Key(2)]
        public int Score { get; set; }

        public Decision(VelocityPoint agent1, VelocityPoint agent2)
        {
            MeAgent1 = agent1;
            MeAgent2 = agent2;
            Score = 0;
        }

        public Decision(VelocityPoint agent1, VelocityPoint agent2, int score)
        {
            MeAgent1 = agent1;
            MeAgent2 = agent2;
            Score = score;
        }

        // DO NOT ERASE
        public Decision() { }

        public override bool Equals(object obj)
        {
            Decision other = obj as Decision;
            if (other is null) return false;
            return this.MeAgent1 == other.MeAgent1 && this.MeAgent2 == other.MeAgent2;
        }

        public override int GetHashCode()
        {
            return MeAgent1.GetHashCode() | (MeAgent2.GetHashCode() << 16);
        }

        public static bool operator ==(Decision x, Decision y)
        {
            if ((x is null) && (y is null)) return true;
            if (x is null) return false;
            if (y is null) return false;
            return x.MeAgent1 == y.MeAgent1 && x.MeAgent2 == y.MeAgent2;
        }
        public static bool operator !=(Decision x, Decision y)
        {
            if ((x is null) && (y is null)) return false;
            if (x is null) return true;
            if (y is null) return true;
            return x.MeAgent1 != y.MeAgent1 || x.MeAgent2 != y.MeAgent2;
        }

        public override string ToString() => $"Agent1 = {MeAgent1}, Agent2 = {MeAgent2}, Score = {Score}";
    }
}
