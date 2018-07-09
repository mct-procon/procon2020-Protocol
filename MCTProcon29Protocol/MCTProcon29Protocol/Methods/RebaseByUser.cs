using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon29Protocol.Methods
{
    [MessagePackObject]
    public class RebaseByUser
    {
        [Key(0)]
        public Point Agent1 { get; set; }
        [Key(1)]
        public Point Agent2 { get; set; }
    }
}
