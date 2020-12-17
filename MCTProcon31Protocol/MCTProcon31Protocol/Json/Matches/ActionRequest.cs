using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon31Protocol.Json.Matches
{
    [JsonObject]
    public class ActionRequest
    {
        [JsonProperty("x")]
        public int X { get; set; }

        [JsonProperty("y")]
        public int Y { get; set; }

        [JsonProperty("type")]
        public string _actionType { get; set; }

        [JsonIgnore]
        public ActionType ActionType => _actionType switch
        {
            "put" => ActionType.Put,
            "stay" => ActionType.Stay,
            "move" => ActionType.Move,
            "remove" => ActionType.Remove,
            _ => ActionType.Unknown
        };

        [JsonProperty("agentID")]
        public int AgentId { get; set; }

        public ActionRequest() { }
        public ActionRequest(int x, int y, ActionType at, int id)
        {
            X = x;
            Y = y;
            _actionType = at switch
            {
                ActionType.Put => "put",
                ActionType.Stay => "stay",
                ActionType.Move => "move",
                ActionType.Remove => "remove",
                _ => "move"
            };
            AgentId = id;
        }
    }
}
