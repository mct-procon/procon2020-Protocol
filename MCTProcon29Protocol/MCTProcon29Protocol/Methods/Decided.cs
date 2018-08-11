using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon29Protocol.Methods
{
    [MessagePackObject]
    public class Decided
    {
        [Key(0)]
        public Point MeAgent1 { get; set; }

        [Key(1)]
        public Point MeAgent2 { get; set; }


        public Decided(Point agent1, Point agent2)
        {
            MeAgent1 = agent1;
            MeAgent2 = agent2;
        }
    }
}
