using Modbus.Protocol;
using System.Collections;
using System.Text;

namespace Modbus.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, Modbus!");
            try
            {
                //ModbusProtocal_Test.Test();
                NModbus_Test.Test();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.ReadLine();
        }

    }
}