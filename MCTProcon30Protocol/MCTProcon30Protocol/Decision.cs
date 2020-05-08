using MessagePack;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MCTProcon30Protocol
{
    /// <summary>
    /// A Structure for Decision.
    /// </summary>
    [MessagePackObject]
    public class Decision
    {
        private Unsafe16Array<VelocityPoint> agents;
        
        /// <summary>
        /// Agents' mvoes
        /// </summary>
        [Key(0)]
        public Unsafe16Array<VelocityPoint> Agents {
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

        public Decision(in Unsafe16Array<VelocityPoint> agents, int score)
        {
            Agents = agents;
            Score = score;
        }

        public Decision(in Unsafe16Array<VelocityPoint> agents) : this(agents, 0) { }

        // DO NOT ERASE
        public Decision() { }

        public override unsafe bool Equals(object obj)
        {
            Decision other = obj as Decision;
            if (other is null) return false;
            if (other.GetHashCode() != this.GetHashCode()) return false;
            return Unsafe16Array<VelocityPoint>.Equals(this.agents, other.agents, 16);
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
            return Unsafe16Array<VelocityPoint>.Equals(x.agents, y.agents, x.AgentsCount);
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
            return !Unsafe16Array<VelocityPoint>.Equals(x.agents, y.agents, x.AgentsCount);
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
