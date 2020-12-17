using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon31Protocol.Json.Matches
{
    [JsonObject]
    public class Agent : IComparable<Agent>, IComparable
    {
        [JsonProperty("agentID")]
        public int Id { get; set; }

        [JsonProperty("x")]
        public int X { get; set; }

        [JsonProperty("y")]
        public int Y { get; set; }

        public int CompareTo(Agent other) => this.Id - other.Id;

        public int CompareTo(object obj) => (obj is Agent) ? this.Id - ((Agent)obj).Id : 1;
    }
}
