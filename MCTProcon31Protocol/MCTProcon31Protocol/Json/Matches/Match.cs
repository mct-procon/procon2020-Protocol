using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon31Protocol.Json.Matches
{
    [JsonObject]
    public class Match
    {
        [JsonProperty("turn")]
        public int Turn { get; set; }

        [JsonProperty("startedAtUnixTime")]
        public int _startAtUnixTime { get; set; }

        [JsonIgnore]
        public DateTime StartAt => DateTime.UnixEpoch + TimeSpan.FromSeconds(_startAtUnixTime);

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("teams")]
        public Team[] Teams { get; set; }

        [JsonProperty("walls")]
        public int[,] Walls { get; set; }

        [JsonProperty("areas")]
        public int[,] Areas { get; set; }

        [JsonProperty("points")]
        public int[,] FieldPoint { get; set; }

        [JsonProperty("actions")]
        public AgentAction[] Actions { get; set; }
    }
}
