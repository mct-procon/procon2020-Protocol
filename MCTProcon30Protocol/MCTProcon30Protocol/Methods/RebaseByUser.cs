using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon30Protocol.Methods
{
    [MessagePackObject]
    public class RebaseByUser
    {
        [Key(0)]
        public Unsafe8Array<Point> Agents;

        public RebaseByUser(in Unsafe8Array<Point> agents)
        {
            Agents = agents;
        }

        // DO NOT ERASE
        public RebaseByUser() { }
    }
}
