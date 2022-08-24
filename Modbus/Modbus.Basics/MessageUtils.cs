using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modbus.Basics
{
    public class MessageUtils
    {
        public const ushort CRCLength = 2;
        /// <summary>
        /// 获取读取数据请求报文
        /// </summary>
        /// <param name="slaveId">从站地址</param>
        /// <param name="readType">读取模式</param>
        /// <param name="startAddr">起始地址</param>
        /// <param name="length">读取长度</param>
        /// <returns></returns>
        public static byte[] BuildReadMessage(byte slaveId, ReadType readType, ushort startAddr, ushort length)
        {
            /*
             * 报文格式
             *      站地址 功能码 起始地址(高位) 起始地址(低位) 读取数量(高位) 读取数量(低位) CRC16校验码
             * 字节   1      1      1            1             1             1           2
             */
            var bytes = new List<byte>();

            bytes.Add(slaveId);//从站地址
            bytes.Add((byte)readType);//功能码

            //起始地址
            var start = BitConverter.GetBytes(startAddr);
            var count = BitConverter.GetBytes(length);
            //小端存储时,第一个byte是低位字节
            //modbusrtu需要高位字节在前,故做一下反转
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(start);
                Array.Reverse(count);
            }
            bytes.AddRange(start);
            bytes.AddRange(count);

            var crcBytes = CheckSum.CRC16(bytes.ToArray());
            bytes.AddRange(crcBytes);

            Console.WriteLine($"=====================slaveId:{slaveId},readType:{readType}==========================");
            Console.WriteLine("发送的报文:");
            for (int i = 0; i < bytes.Count; i++)
            {
                Console.Write(bytes[i].ToString("X2") + " ");
            }
            return bytes.ToArray();
        }

        public static void ParseResult(byte[] receiveBytes, bool @float = false)
        {
            Console.WriteLine("接收到的消息:");
            for (int i = 0; i < receiveBytes.Length; i++)
            {
                Console.Write($"{receiveBytes[i].ToString("X2")} ");
            }
            Console.WriteLine();
            //校验验证码
            var crcReceive = CheckSum.CRC16(receiveBytes.Take(receiveBytes.Length - CRCLength).ToArray());
            if (!Enumerable.SequenceEqual(receiveBytes.TakeLast(CRCLength), crcReceive))
            {
                Console.WriteLine("校验码错误");
                return;
            }
            var optCode = receiveBytes[1];
            switch (optCode)
            {
                case (byte)ReadType.Read01:
                case (byte)ReadType.Read02:
                    ParseRead01OrRead02(receiveBytes);
                    break;
                case (byte)ReadType.Read03:
                case (byte)ReadType.Read04:
                    ParseRead03OrRead4(receiveBytes, @float);
                    break;
                default:
                    Console.WriteLine($"{nameof(ParseResult)},TODO:{optCode}");
                    break;
            }
        }
        #region 解析读取结果
        /// <summary>
        /// 读线圈或离散输入
        /// </summary>
        private static void ParseRead01OrRead02(byte[] receiveBytes)
        {
            //校验验证码
            var crcReceive = CheckSum.CRC16(receiveBytes.Take(receiveBytes.Length - 2).ToArray());
            if (Enumerable.SequenceEqual(receiveBytes.TakeLast(2), crcReceive))
            {
                //由于读取的是线圈/离散值，返回的值其实是一个个的布尔量，也就是位，所以需要换算为二进制才能看到确定的读数
                //TODO:这里按读10个数据位处理的，10个数据位需要2个字节=>Int16
                char[] chr = Convert.ToString(BitConverter.ToInt16(receiveBytes, 3), 2).ToArray();//读2字节（ToInt16）数据内容,转换成2进制字符(0、1)
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(chr);
                }
                //获取状态
                var bitArray = new BitArray(receiveBytes.Skip(3).Take(2).ToArray());
                for (int i = 0; i < chr.Length; i++)
                {
                    Console.WriteLine($"{chr[i]} —— {bitArray[i]}");
                }
                Console.WriteLine($"接收到消息的二进制:{new string(chr)}");
            }
        }


        /// <summary>
        /// 读保持型寄存器/输入型寄存器
        /// </summary>
        static void ParseRead03OrRead4(byte[] receiveBytes, bool @float = false)
        {
            //获取字节数
            int count = Convert.ToInt32(receiveBytes[2]);
            if (@float)
            {
                int index = 0;
                var sb = new StringBuilder();
                for (int i = 3; i < count + 3; i += 4)
                {
                    sb.Length = 0;
                    index++;
                    var temp = BitConverter.IsLittleEndian ? new byte[] { receiveBytes[i + 3], receiveBytes[i + 2], receiveBytes[i + 1], receiveBytes[i] } :
                        new byte[] { receiveBytes[i], receiveBytes[i + 1], receiveBytes[i + 2], receiveBytes[i + 3] };
                    float result = BitConverter.ToSingle(temp, 0);
                    sb.Append($"{index:00} : ");
                    for (int j = 0; j < temp.Length; j++)
                    {
                        sb.Append($"{temp[j].ToString("X2")} ");
                    }
                    sb.Append($"-- {result}");
                    Console.WriteLine(sb.ToString());
                }
            }
            else
            {
                int index = 0;
                //从第4个字节开始
                for (int i = 3; i < count + 3; i += 2)
                {
                    index++;
                    var temp = BitConverter.IsLittleEndian ? new byte[] { receiveBytes[i + 1], receiveBytes[i] } : new byte[] { receiveBytes[i], receiveBytes[i + 1] };
                    var result = BitConverter.ToInt16(temp, 0);
                    Console.WriteLine($"{index:00} : {receiveBytes[i].ToString("X2")} {receiveBytes[i + 1].ToString("X2")} -- {result}");
                }
            }
            Console.WriteLine();
        }
        #endregion
    }
}
