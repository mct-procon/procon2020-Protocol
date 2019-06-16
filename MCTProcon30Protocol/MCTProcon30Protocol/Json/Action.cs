using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MCTProcon30Protocol.Json
{
    public enum ActionType
    {
        Unknown = 0, Move, Remove, Stay
    }

    public enum Appliment
    {
        Unknown = 0, Fail, Coflict, Success
    }

    public class Action
    {
        [DataMember(Name = "agentID")]
        public int AgentID { get; set; }

        [DataMember(Name ="type")]
        public string __type { get; set; }

        [IgnoreDataMember]
        public ActionType Type {
            get {
                switch (__type) {
                    case "move": return ActionType.Move;
                    case "remove": return ActionType.Remove;
                    case "stay": return ActionType.Stay;
                    default: return ActionType.Unknown;
                }
            }
        }

        [DataMember(Name = "dx")]
        public int Dx { get; set; }

        [DataMember(Name ="dy")]
        public int Dy { get; set; }

        [DataMember(Name ="turn")]
        public int Turn { get; set; }

        [DataMember(Name = "apply")]
        public int __apply { get; set; }

        [IgnoreDataMember]
        public Appliment Apply {
            get {
                switch (__apply)
                {
                    case -1: return Appliment.Fail;
                    case 0: return Appliment.Coflict;
                    case 1: return Appliment.Success;
                    default: return Appliment.Unknown;
                }
            }
        }
    }
}
