using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTProcon30Protocol.Methods
{
    [MessagePackObject]
    public class RequestAnswer
    {
        [Key(0)]
        public byte Turn { get; set; }

        public RequestAnswer(byte turn)
        {
            Turn = turn;
        }

        // DO NOT ERASE
        public RequestAnswer() { }
    }
}
