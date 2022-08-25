using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Modbus.Protocol;
using NModbus;
using NModbus.Device;
using NModbus.Serial;

namespace Modbus.Test
{
    internal class NModbus_Test
    {
        private static IModbusMaster _master;
        private static byte _slaveAddr = 1;
        private static ushort _startAddr = 0;

        internal static void Test()
        {
            //Init_With_SerialPort();
            //Init_With_Socket();
            //Init_With_TcpClient();
            Init_With_UdpClient();

            //TestSingleCoil();
            //TestMultiCoils();

            TestSingleRegister();
            TestMultiRegisters();
        }

        private static void Init_With_SerialPort()
        {
            Console.WriteLine($"====================={nameof(NModbus_Test)},{nameof(Init_With_SerialPort)}==========================");
            var _port = new SerialPort();
            _port.PortName = "COM3";
            _port.BaudRate = 9600;
            _port.Parity = Parity.Even;
            _port.DataBits = 8;
            _port.StopBits = StopBits.One;
            _port.Open();

            var factory = new ModbusFactory();
            _master = factory.CreateRtuMaster(_port);
        }


        private static void Init_With_Socket()
        {
            Console.WriteLine($"====================={nameof(NModbus_Test)},{nameof(Init_With_Socket)}==========================");
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ip = IPAddress.Parse("127.0.0.1");
            var addr = new IPEndPoint(ip, 502);
            socket.Connect(addr);

            _master = new ModbusFactory().CreateMaster(socket);
        }

        private static void Init_With_TcpClient()
        {
            Console.WriteLine($"====================={nameof(NModbus_Test)},{nameof(Init_With_TcpClient)}==========================");
            var tcpClient = new TcpClient("127.0.0.1", 502);

            _master = new ModbusFactory().CreateMaster(tcpClient);
        }

        private static void Init_With_UdpClient()
        {
            Console.WriteLine($"====================={nameof(NModbus_Test)},{nameof(Init_With_UdpClient)}==========================");
            var tcpClient = new UdpClient("127.0.0.1", 502);

            _master = new ModbusFactory().CreateMaster(tcpClient);
        }

        private static void TestSingleCoil()
        {
            Console.WriteLine($"===={nameof(TestSingleCoil)}====");
            _master.WriteSingleCoil(_slaveAddr, _startAddr, true);
            ReadCoils(1);
        }

        private static void TestMultiCoils()
        {
            Console.WriteLine($"===={nameof(TestMultiCoils)}====");
            var data = new bool[] { false, false, true, false, true };
            _master.WriteMultipleCoils(_slaveAddr, _startAddr, data);
            ReadCoils((ushort)data.Length);
        }

        private static void TestSingleRegister()
        {
            Console.WriteLine($"===={nameof(TestSingleRegister)}====");
            _master.WriteSingleRegister(_slaveAddr, _startAddr, 250);
            ReadHoldingRegisters(1);
        }

        private static void TestMultiRegisters()
        {
            Console.WriteLine($"===={nameof(TestSingleRegister)}====");
            var data = new ushort[] { 10, 20, 30 };
            _master.WriteMultipleRegisters(_slaveAddr, _startAddr, data);

            ReadHoldingRegisters((ushort)data.Length);
        }


        private static void ReadCoils(ushort count)
        {
            var results = _master.ReadCoils(_slaveAddr, _startAddr, count);
            for (var idx = 0; idx < results.Length; idx++)
            {
                Console.WriteLine($"{idx} -- {results[idx]}");
            }
        }

        private static void ReadHoldingRegisters(ushort count)
        {
            var results = _master.ReadHoldingRegisters(_slaveAddr, _startAddr, count);
            for (var idx = 0; idx < results.Length; idx++)
            {
                Console.WriteLine($"{idx} -- {results[idx]}");
            }
        }

        private static void ReadInputRegisters(ushort count)
        {
            var results = _master.ReadInputRegisters(_slaveAddr, _startAddr, count);
            for (var idx = 0; idx < results.Length; idx++)
            {
                Console.WriteLine($"{idx} -- {results[idx]}");
            }
        }
    }
}
