using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MCTProcon30Protocol.Json
{
    public class Ping
    {
        [DataMember(Name = "status")]
        public string Status { get; set; }
    }
}
