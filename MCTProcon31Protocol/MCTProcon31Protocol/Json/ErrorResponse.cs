using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon31Protocol.Json
{
    [JsonObject]
    public class ErrorResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("startAt")]
        public Nullable<DateTime> StartAt { get; set; }
    }
}
