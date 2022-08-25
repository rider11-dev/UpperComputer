using Modbus.Basics;
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
            Console.WriteLine("=======================读=========================");
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

            Console.WriteLine("=======================写=========================");
            MessageUtils.BuildWriteMessage(slaveId, start, true);
            MessageUtils.BuildWriteMessage(slaveId, start, false);
            MessageUtils.BuildWriteMessage(slaveId, start, 300);
            var bools = new List<bool> { true, false, true, true, true };
            //var data = MessageUtils.GetBitArray(bools);
            //Console.WriteLine($"16进制:{data.ToString("X2")}");
            //Console.WriteLine($"2进制:{Convert.ToString(data, 2)}");
            MessageUtils.BuildWriteMessage(slaveId, start, bools);
            MessageUtils.BuildWriteMessage(slaveId, start, new List<short> { 10, 20, 30, 40, 50 });

            Console.ReadLine();
        }

    }
}