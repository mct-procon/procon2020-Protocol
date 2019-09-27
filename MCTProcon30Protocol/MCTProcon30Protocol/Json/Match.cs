using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon30Protocol.Json
{
    [JsonObject]
    public class Match
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("intervalMillis")]
        public int IntervalMilliseconds { get; set; }

        [JsonProperty("matchTo")]
        public string MatchTo { get; set; }

        [JsonProperty("teamID")]
        public int TeamId { get; set; }

        [JsonProperty("turnMills")]
        public int TurnMilliseconds { get; set; }

        [JsonProperty("turns")]
        public int Turns { get; set; }
    }
}
