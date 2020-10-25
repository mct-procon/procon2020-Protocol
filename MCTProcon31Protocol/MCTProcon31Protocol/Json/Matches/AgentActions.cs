﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon31Protocol.Json.Matches
{
    [JsonObject]
    public class AgentActions
    {
        [JsonProperty("actions")]
        public AgentAction[] _value { get; set; }

        public AgentAction this[int i] {
            get => _value[i];
            set => _value[i] = value;
        }

        public int Length => _value.Length;
    }
}
