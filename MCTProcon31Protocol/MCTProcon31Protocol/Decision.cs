using MessagePack;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MCTProcon31Protocol
{
    /// <summary>
    /// A Structure for Decision.
    /// </summary>
    [MessagePackObject]
    public class Decision
    {
        private Unsafe16Array<Point> agents;
        
        /// <summary>
        /// Agents' mvoes
        /// </summary>
        [Key(0)]
        public Unsafe16Array<Point> Agents {
            get => agents;
            set => agents = value;
        }

        /// <summary>
        /// Board Score
        /// </summary>
        [Key(1)]
        public int Score { get; set; }

        /// <summary>
        /// Count of agents.
        /// </summary>
        [Key(2)]
        public byte AgentsCount { get; set; }


        /// <summary>
        /// State of agents.
        /// </summary>
        [Key(3)]
        public Unsafe16Array<AgentState> AgentsState { get; set; }

        public Decision(byte agentsCount, in Unsafe16Array<Point> agents, in Unsafe16Array<AgentState> agentsState, int score)
        {
            AgentsCount = agentsCount;
            Agents = agents;
            Score = score;
        }

        public Decision(byte agentsCount, in Unsafe16Array<Point> agents, in Unsafe16Array<AgentState> agentsState) : this(agentsCount, agents, agentsState, 0) { }

        // DO NOT ERASE
        public Decision() { }

        public override unsafe bool Equals(object obj)
        {
            Decision other = obj as Decision;
            if (other is null) return false;
            if (other.GetHashCode() != this.GetHashCode()) return false;
            return Unsafe16Array.Equals(this.agents, other.agents, 16);
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
            if (x.AgentsCount != y.AgentsCount) return false;
            return Unsafe16Array.Equals(x.agents, y.agents, x.AgentsCount);
        }

        /// <summary>
        /// Compare Equality. <strong>NOTICE: This doesn't compare Score.</strong>
        /// </summary>
        public static unsafe bool operator !=(Decision x, Decision y)
        {
            if ((x is null) && (y is null)) return false;
            if (x is null) return true;
            if (y is null) return true;
            if (x.AgentsCount != y.AgentsCount) return false;
            return !Unsafe16Array.Equals(x.agents, y.agents, x.AgentsCount);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("AgentsCount = ");
            sb.Append(AgentsCount.ToString());
            sb.Append(", ");
            for (int i = 0; i < AgentsCount; ++i)
            {
                sb.Append("Agent");
                sb.Append((i + 1).ToString());
                sb.Append(" = ");
                sb.Append(Agents[i].ToString());
                sb.Append(", ");
            }
            sb.Append("Score = ");
            sb.Append(Score.ToString());
            return sb.ToString();
        }
    }
}
