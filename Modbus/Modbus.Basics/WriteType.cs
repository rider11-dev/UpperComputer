using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modbus.Basics
{
    /*
 * 功能码	作用
01	读线圈
02	读离散输入
03	读保持型寄存器
04	读输入寄存器

05	写单个线圈
06	写单个寄存器
0F	写多个线圈
10	写多个寄存器
 */

    public enum WriteType
    {
        /// <summary>
        /// 功能码05,写单个线圈
        /// </summary>
        Write05 = 0x05,
        /// <summary>
        /// 功能码06,写单个寄存器
        /// </summary>
        Write06 = 0x06,
        /// <summary>
        /// 功能码0F,写多个线圈
        /// </summary>
        Write0F = 0x0F,
        /// <summary>
        /// 功能码10,写多个寄存器
        /// </summary>
        Write10 = 0x10
    }
}
