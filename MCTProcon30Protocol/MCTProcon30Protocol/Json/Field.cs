using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MCTProcon30Protocol.Json
{
    public class Field
    {
        [DataMember(Name = "width")]
        public int Width { get; set; }

        [DataMember(Name = "height")]
        public int Height { get; set; }

        [DataMember(Name = "points")]
        public int[,] Point { get; set; }

        [DataMember(Name = "startedAtUnixTime")]
        public int StartedAtUnixTime { get; set; }

        [DataMember(Name = "turn")]
        public int Turn { get; set; }
        
        [DataMember(Name = "tiled")]
        public int[,] Tiled { get; set; }

        [DataMember(Name = "teams")]
        public Team[] Teams { get; set; }

        [DataMember(Name = "actions")]
        public Action[] Actions { get; set; }
    }
}
