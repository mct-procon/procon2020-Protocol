using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon29Protocol.Methods
{
    [MessagePackObject]
    public class Decided
    {
        [Key(0)]
        public VelocityPoint MeAgent1 { get; set; }

        [Key(1)]
        public VelocityPoint MeAgent2 { get; set; }

        [Key(2)]
        public int Score { get; set; }

        public Decided(VelocityPoint agent1, VelocityPoint agent2)
        {
            MeAgent1 = agent1;
            MeAgent2 = agent2;
            Score = 0;
        }

        public Decided(VelocityPoint agent1, VelocityPoint agent2, int score)
        {
            MeAgent1 = agent1;
            MeAgent2 = agent2;
            Score = score;
        }

        // DO NOT ERASE
        public Decided() { }

        public override bool Equals(object obj)
        {
            Decided other = obj as Decided;
            if (other is null) return false;
            return this.MeAgent1 == other.MeAgent1 && this.MeAgent2 == other.MeAgent2;
        }

        public override int GetHashCode()
        {
            return MeAgent1.GetHashCode() | (MeAgent2.GetHashCode() << 16);
        }

        public static bool operator ==(Decided x, Decided y) => ((x is null) && (y is null)) || (x.MeAgent1 == y.MeAgent1 && x.MeAgent2 == y.MeAgent2);
        public static bool operator !=(Decided x, Decided y) => !((x is null) && (y is null)) || x.MeAgent1 != y.MeAgent1 || x.MeAgent2 != y.MeAgent2;

        public override string ToString() => $"Agent1 = {MeAgent1}, Agent2 = {MeAgent2}, Score = {Score}";
    }
}
