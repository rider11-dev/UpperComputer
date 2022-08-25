using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modbus.Protocol
{
    public class Utils
    {

        /// <summary>
        /// 字节=>hex
        /// </summary>
        /// <param name="bytes"></param>
        public static string BytesToHexString(IEnumerable<byte> bytes)
        {
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                sb.Append(b.ToString("X2")).Append(" ");
            }
            return sb.ToString();
        }

    }
}
