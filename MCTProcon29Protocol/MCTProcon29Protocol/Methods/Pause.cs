using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon29Protocol.Methods
{
    [MessagePackObject]
    public class Pause
    {
        [Key(0)]
        public bool IsEnter { get; set; }
    }
}
