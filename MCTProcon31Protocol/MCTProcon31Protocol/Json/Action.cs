using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon31Protocol.Json
{
    public enum ActionType
    {
        Unknown = 0, Move, Remove, Stay
    }

    public enum Appliment
    {
        Unknown = 0, Fail, Coflict, Success
    }

    [JsonObject]
    public class Action
    {
        [JsonProperty("agentID")]
        public int AgentID { get; set; }

        [JsonProperty("type")]
        public string __type { get; set; }

        [JsonIgnore]
        public ActionType Type => __type switch
        {
            "move" => ActionType.Move,
            "remove" => ActionType.Remove,
            "stay" => ActionType.Stay,
            _ => ActionType.Unknown
        };

        [JsonProperty("dx")]
        public int Dx { get; set; }

        [JsonProperty("dy")]
        public int Dy { get; set; }

        [JsonProperty("turn")]
        public int Turn { get; set; }

        [JsonProperty("apply")]
        public int __apply { get; set; }

        [JsonIgnore]
        public Appliment Apply => __apply switch
        {
            -1 => Appliment.Fail,
            0 => Appliment.Coflict,
            1 => Appliment.Success,
            _ => Appliment.Unknown,
        };
    }
}
