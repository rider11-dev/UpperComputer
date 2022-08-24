﻿using Modbus.Basics;
using System.Collections;
using System.Text;

namespace Modbus.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, Modbus!");
            byte slaveId = 1;
            ushort start = 0;
            ushort length = 10;
            MessageUtils.BuildReadMessage(slaveId, ReadType.Read01, start, length);
            MessageUtils.ParseResult(new byte[] { 0x01, 0x01, 0x02, 0x02, 0x00, 0xB8, 0x9C });

            MessageUtils.BuildReadMessage(slaveId, ReadType.Read02, start, length);
            MessageUtils.ParseResult(new byte[] { 0x01, 0x02, 0x02, 0xD6, 0x00, 0xE7, 0xD8 });

            MessageUtils.BuildReadMessage(slaveId, ReadType.Read03, start, length);
            MessageUtils.ParseResult(new byte[] { 0x01, 0x03, 0x14, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xEC, 0x63 });

            //整型
            MessageUtils.BuildReadMessage(slaveId, ReadType.Read04, start, length);
            MessageUtils.ParseResult(new byte[] { 0x01, 0x04, 0x14, 0x00, 0x00, 0x00, 0x42, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x35 });

            //浮点数
            MessageUtils.BuildReadMessage(slaveId, ReadType.Read04, start, length);
            MessageUtils.ParseResult(new byte[] { 0x01, 0x04, 0x14, 0x41, 0xBC, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xD3, 0x14 }, true);

            Console.ReadLine();
        }

    }
}