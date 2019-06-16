using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MCTProcon30Protocol.Json
{
    public class Team
    {
        [DataMember(Name = "teamID")]
        public int TeamID { get; set; }

        [DataMember(Name = "agents")]
        public Agent[] Agents { get; set; }

        [DataMember(Name ="tilePoint")]
        public int TilePoint { get; set; }

        [DataMember(Name = "areaPoint")]
        public int AreaPoint { get; set; }
    }
}
