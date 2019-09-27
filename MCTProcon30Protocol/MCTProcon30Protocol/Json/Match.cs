using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MCTProcon30Protocol.Json
{
    public class Match
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "intervalMillis")]
        public int IntervalMilliseconds { get; set; }

        [DataMember(Name = "matchTo")]
        public string MatchTo { get; set; }

        [DataMember(Name = "teamID")]
        public int TeamId { get; set; }

        [DataMember(Name = "turnMills")]
        public int TurnMilliseconds { get; set; }

        [DataMember(Name = "turns")]
        public int Turns { get; set; }
    }
}
