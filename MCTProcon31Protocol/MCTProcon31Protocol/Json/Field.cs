using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon31Protocol.Json
{
    [JsonObject]
    public class Field
    {
        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("points")]
        public int[,] Point { get; set; }

        [JsonProperty("startedAtUnixTime")]
        public int StartedAtUnixTime { get; set; }

        [JsonProperty("turn")]
        public int Turn { get; set; }
        
        [JsonProperty("tiled")]
        public int[,] Tiled { get; set; }

        [JsonProperty("teams")]
        public Team[] Teams { get; set; }

        [JsonProperty("actions")]
        public Action[] Actions { get; set; }
    }
}
