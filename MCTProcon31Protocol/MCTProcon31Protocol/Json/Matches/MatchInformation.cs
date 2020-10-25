using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon31Protocol.Json.Matches
{
    [JsonObject]
    public class MatchInformation
    {
        [JsonProperty("matchID")]
        public int Id { get; set; }

        [JsonProperty("teams")]
        public TeamInformation[] Teams { get; set; }

        [JsonProperty("turns")]
        public int Turns { get; set; }

        [JsonProperty("operationMillis")]
        public int OperationMilliseconds { get; set; }

        [JsonProperty("transitionMillis")]
        public int TransitionMilliseconds { get; set; }
    }
}
