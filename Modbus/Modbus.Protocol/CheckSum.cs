﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modbus.Protocol
{
    public class CheckSum
    {
        public static byte[] CRC16(byte[] data)
        {
            int len = data.Length;
            if (len > 0)
            {
                ushort crc = 0xFFFF;//16位
                for (int i = 0; i < len; i++)
                {
                    crc = (ushort)(crc ^ (data[i]));//低8位异或
                    for (int j = 0; j < 8; j++)
                    {
                        crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                    }
                }
                byte high = (byte)((crc & 0xFF00) >> 8);//高位
                byte low = (byte)(crc & 0x00FF);//低位
                return BitConverter.IsLittleEndian ? new byte[] { low, high } : new byte[] { high, low };
            }
            return new byte[] { 0, 0 };
        }
    }
}
