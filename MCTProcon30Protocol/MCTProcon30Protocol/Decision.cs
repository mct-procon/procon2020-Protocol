using MessagePack;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MCTProcon30Protocol
{
    /// <summary>
    /// A Structure for Decision.
    /// </summary>
    [MessagePackObject]
    public class Decision
    {
        private Unsafe8Array<VelocityPoint> agents;
        
        /// <summary>
        /// Agents' mvoes
        /// </summary>
        [Key(0)]
        public Unsafe8Array<VelocityPoint> Agents {
            get => agents;
            set => agents = value;
        }

        /// <summary>
        /// Board Score
        /// </summary>
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

        public override unsafe bool Equals(object obj)
        {
            Decision other = obj as Decision;
            if (other is null) return false;
            if (other.GetHashCode() != this.GetHashCode()) return false;
            return Unsafe8Array<VelocityPoint>.Equals(this.agents, other.agents, 16);
        }

        public override unsafe int GetHashCode()
        {
            int* start = (int*)Unsafe.AsPointer(ref agents);
            return start[0] ^ start[1] ^ start[2] ^ start[3];
        }

        /// <summary>
        /// Compare Equality. <strong>NOTICE: This doesn't compare Score.</strong>
        /// </summary>
        public static unsafe bool operator ==(Decision x, Decision y)
        {
            if ((x is null) && (y is null)) return true;
            if (x is null) return false;
            if (y is null) return false;
            return Unsafe8Array<VelocityPoint>.Equals(x.agents, y.agents, 16);
        }

        /// <summary>
        /// Compare Equality. <strong>NOTICE: This doesn't compare Score.</strong>
        /// </summary>
        public static unsafe bool operator !=(Decision x, Decision y)
        {
            if ((x is null) && (y is null)) return false;
            if (x is null) return true;
            if (y is null) return true;
            return !Unsafe8Array<VelocityPoint>.Equals(x.agents, y.agents, 16);
        }

        public override string ToString() => $"Agent1 = {agents.Agent1}, Agent2 = {agents.Agent2}, Agent3 = {agents.Agent3}, Agent4 = {agents.Agent4}, Agent5 = {agents.Agent5}, Agent6 = {agents.Agent6}, Agent7 = {agents.Agent7}, Agent8 = {agents.Agent8}, Score = {Score}";
    }
}
