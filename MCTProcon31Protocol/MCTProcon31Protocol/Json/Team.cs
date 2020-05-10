using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon31Protocol.Json
{
    [JsonObject]
    public class Team
    {
        [JsonProperty("teamID")]
        public int TeamID { get; set; }

        [JsonProperty("agents")]
        public Agent[] Agents { get; set; }

        [JsonProperty("tilePoint")]
        public int TilePoint { get; set; }

        [JsonProperty("areaPoint")]
        public int AreaPoint { get; set; }
    }
}
