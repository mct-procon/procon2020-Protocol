using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon31Protocol.Json.Matches
{
    [JsonObject]
    public class AgentAction
    {
        [JsonProperty("x")]
        public int X { get; set; }

        [JsonProperty("y")]
        public int Y { get; set; }

        [JsonProperty("type")]
        public string _actionType { get; set; }

        [JsonIgnore]
        public ActionType ActionType => _actionType switch {
            "put" => ActionType.Put,
            "stay" => ActionType.Stay,
            "move" => ActionType.Move,
            "remove" => ActionType.Remove,
            _ => ActionType.Unknown
        };

        [JsonProperty("turn", Required = Required.DisallowNull)]
        public int Turn { get; set; }

        [JsonProperty("agentID")]
        public int AgentId { get; set; }

        [JsonProperty("apply", Required = Required.DisallowNull)]
        public sbyte? _apply { get; set; }

        [JsonIgnore]
        public Appliment Apply => _apply.HasValue ? (Appliment)_apply : Appliment.None;
    }

    public enum ActionType : byte
    {
        Unknown = 0,
        Put,
        Stay,
        Move,
        Remove
    }

    public enum Appliment : sbyte
    {
        Invalid = -1,
        Race = 0,
        Success = 1,
        None = 2
    }
}
