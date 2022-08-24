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

    /// <summary>
    /// 读取模式
    /// </summary>
    public enum ReadType
    {
        /// <summary>
        /// 功能码01,读线圈（布尔值）
        /// </summary>
        Read01 = 0x01,
        /// <summary>
        /// 功能码2,读离散输入（布尔值）
        /// </summary>
        Read02 = 0x02,
        /// <summary>
        /// 功能码03,读保持型寄存器
        /// </summary>
        Read03 = 0x03,
        /// <summary>
        /// 功能码04,读输入寄存器
        /// </summary>
        Read04 = 0x04
    }
}