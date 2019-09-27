using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon30Protocol.Json
{
    [JsonObject]
    public class Ping
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
