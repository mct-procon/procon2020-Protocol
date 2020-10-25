using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon31Protocol.Json.Matches
{
    [JsonObject]
    public class Team
    {
        [JsonProperty("teamID")]
        public int Id { get; set; }

        [JsonProperty("agent")]
        public int AgentCount { get; set; }

        [JsonProperty("agents")]
        public Agent[] Agents { get; set; }

        [JsonProperty("areaPoint")]
        public int AreaPoint { get; set; }

        [JsonProperty("wallPoint")]
        public int WallPoint { get; set; }
    }
}
