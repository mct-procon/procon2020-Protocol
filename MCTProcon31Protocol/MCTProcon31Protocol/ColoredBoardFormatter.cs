﻿using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;
using MessagePack.Formatters;

namespace MCTProcon31Protocol
{
    //public class ColoredBoardSmallBiggerFormatter : IMessagePackFormatter<ColoredBoardSmallBigger>
    //{
    //    public unsafe ColoredBoardSmallBigger Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
    //    {
    //        var startoffset = offset;
    //        var width = MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
    //        offset += readSize;
    //        var height = MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
    //        offset += readSize;
    //        var result = new ColoredBoardSmallBigger(width, height);
    //        for(int i = 0; i < ColoredBoardSmallBigger.BoardSize; ++i)
    //        {
    //            result.board[i] = MessagePackBinary.ReadUInt16(bytes, offset, out readSize);
    //            offset += readSize;
    //        }
    //        readSize = offset - startoffset;
    //        return result;
    //    }

    //    public unsafe int Serialize(ref byte[] bytes, int offset, ColoredBoardSmallBigger value, IFormatterResolver formatterResolver)
    //    {
    //        var startoffset = offset;
    //        offset += MessagePackBinary.WriteUInt32(ref bytes, offset, value.Width);
    //        offset += MessagePackBinary.WriteUInt32(ref bytes, offset, value.Height);
    //        for (int i = 0; i < ColoredBoardSmallBigger.BoardSize; ++i)
    //            offset += MessagePackBinary.WriteUInt16(ref bytes, offset, value.board[i]);
    //        return offset - startoffset;
    //    }
    //}
    public class ColoredBoardNormalSmallerFormatter : IMessagePackFormatter<ColoredBoardNormalSmaller>
    {
        public unsafe ColoredBoardNormalSmaller Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            //var width = MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
            //offset += readSize;
            //var height = MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
            //offset += readSize;
            //var result = new ColoredBoardNormalSmaller(width, height);
            var result = new ColoredBoardNormalSmaller();
            for (int i = 0; i < ColoredBoardNormalSmaller.BoardSize; ++i)
                result.board[i] = reader.ReadUInt32();
            return result;
        }

        public  unsafe void Serialize(ref MessagePackWriter writer, ColoredBoardNormalSmaller value, MessagePackSerializerOptions options)
        {
            //offset += MessagePackBinary.WriteUInt32(ref bytes, offset, value.Width);
            //offset += MessagePackBinary.WriteUInt32(ref bytes, offset, value.Height);
            for (int i = 0; i < ColoredBoardNormalSmaller.BoardSize; ++i)
                writer.WriteUInt32(value.board[i]);
        }
    }
}
