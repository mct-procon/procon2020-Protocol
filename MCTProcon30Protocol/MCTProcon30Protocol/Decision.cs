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
        public Unsafe8Array<VelocityPoint> Agents { get; set; }

        [Key(1)]
        public int Score { get; set; }

        public Decision(in Unsafe8Array<VelocityPoint> agents, int score)
        {
            Agents = agents;
            Score = score;
        }

        public Decision(in Unsafe8Array<VelocityPoint> agents) : this(agents, 0) { }

        // DO NOT ERASE
        public Decision() { }

        // TODO: ToString

        //public override bool Equals(object obj)
        //{
        //    Decision other = obj as Decision;
        //    if (other is null) return false;
        //    return this.MeAgent1 == other.MeAgent1 && this.MeAgent2 == other.MeAgent2;
        //}

        //public override int GetHashCode()
        //{
        //    return MeAgent1.GetHashCode() | (MeAgent2.GetHashCode() << 16);
        //}

        //public static bool operator ==(Decision x, Decision y)
        //{
        //    if ((x is null) && (y is null)) return true;
        //    if (x is null) return false;
        //    if (y is null) return false;
        //    return x.MeAgent1 == y.MeAgent1 && x.MeAgent2 == y.MeAgent2;
        //}
        //public static bool operator !=(Decision x, Decision y)
        //{
        //    if ((x is null) && (y is null)) return false;
        //    if (x is null) return true;
        //    if (y is null) return true;
        //    return x.MeAgent1 != y.MeAgent1 || x.MeAgent2 != y.MeAgent2;
        //}

        //public override string ToString() => $"Agent1 = {MeAgent1}, Agent2 = {MeAgent2}, Score = {Score}";
    }
}
