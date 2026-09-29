//**************************************************************************************************
//Copyright (c) 2022-2026 
//Shenzhen SoftStorm Technology Co., Limited  
//All rights reserved.

//Licensed under the MIT license. See LICENCE file in the project root for full license information.
//***************************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Kingpool.Communication
{

    public enum ScommParity
    {
        None = 0,
        Mark = 1,
        Space = 2,
        Odd = 3,
        Even = 4,
    }
    public enum ScommStopBits
    {
        One = 0,
        OneHalf = 1,
        Two = 2,
    }
    public enum ScommDataBits
    {
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
    }

    public struct ScommOptions
    {
        public int portIndex; //端口号: 从0开始,如COM1为0
        public int baudRate;  //波特率: 9600, ...
        public ScommDataBits dataBits;  //数据位: 8, ...
        public ScommStopBits stopBits;  //停止位: ONE_STOPBIT , ...
        public ScommParity parity;    //校验位: NO_PARITY, ...

        public ScommOptions(int port = 0, int rate = 9600, ScommDataBits _dataBits = ScommDataBits.Eight, ScommStopBits _stopBits = ScommStopBits.One, ScommParity _parity = ScommParity.None)
        {
            portIndex = port;
            baudRate = rate;
            dataBits = _dataBits;
            stopBits = _stopBits;
            parity = _parity;
        }
    }
    //协议， 串口
    public enum ScommProtocol
    {
        ModbusRtuSlave = 1,
        ModbusRtuMaster = 2,
        ModbusAsciiSlave = 3,
        ModbusAsciiMaster = 4,
        FreeAsciiMaster = 119, //自定义协议主站, ASCII
        FreeAsciiSlave = 120, //自定义协议从站,ASCII 
        FreeBinaryMaster = 121,
        FreeBinarySlave = 122,
    }

    //协议，网口 
    public enum EnetProtocol
    {
        ModbusRtuTcp = 310,  //Modbus RTU TCP
        FreeAsciiUdp = 301,  //ethernet ascii,slave, 
        FreeTcpSlave = 302,  //ethernet ascii,slave
        FreeBinaryUdp = 303,//主站UDP,CLIENT
        FreeTcpMaster = 304,//主站TCP,CLIENT
    }

    public enum ModbusCommand
    {
        //Modbus RTU , read
        ReadCoilStatus = 1, // Read Coil Status  
        ReadInputStatus = 2, // Read Input Status  
        ReadHoldingRegisters = 3, // Read Holding Registers  
        ReadInputRegisters = 4, // Read Input Registers  
        //Modbus RTU, write
        ForceSingleCoil = 5, // Force Single Coil  
        PresetSingleRegister = 6, // Preset Single Register  
        ForceMultipleCoils = 15,// Force Multiple Coils  
        PresetMultipleRegisters = 16,// Preset Multiple Registers  
    }

    public enum PlcDataType
    {
        //数据类型
        Bit = 0,  //位类型(包括位块: 按位存储,往字节低位对齐)
        Byte = 1, //字节(1字节: char存储格式)
        Word = 2, //字(2字节: short存储格式)
        Dword = 3,    //双字
        Float = 4,    //四字节单精度浮点数
    }


    //4字节单精度浮点数据类型
    public enum FloatByteOrder
    {
        ABCD = 0,
        DCBA = 1,
        BADC = 2,
        CDAB = 3,
    }


    public struct Net4Options
    {
        public string strArress;
        public int port;
    }

    //数据到达回调函数
    //  typedef void (WINAPI* JP_DATA_RECEIVED_FUNC) (const char*, UINT, void*);
    public delegate void DataReceivedDelegate(IntPtr data, uint size, IntPtr pUserData);

    //modbus tcp data  
    //serial, command, memaddr, int len, void* pData, int size, int* errcode, void* userdata
    //return: true or false
    //pData in char(byte) for JP_MB_RCS(1), JP_MB_RIS(2), in short for JP_MB_RHR(3) , JP_MB_RIR(4)

    //typedef bool(WINAPI* JP_MODBUS_RTU_DATA_FUNC)(UINT, int, int, int, void*, int, int*, void*);
    //modbus rtu data callback
    public delegate bool ModbusRtuDataDelegate(uint serial, int cmdId, int address, int length, IntPtr hData, int size, ref int error, IntPtr pUserData);

    //typedef void (WINAPI* JP_DATA_SENT_FUNC) (const char*, UINT, void*);
    public delegate void DataSentDelegate(IntPtr data, uint size, IntPtr pUserData);

    public class AnyComm
    {
        //public const int SCOMM_MIN_DATA_BITS = 5;

        //串口停止位
        //public static int ONE_STOPBIT = 0;
        //public static int ONE5_STOPBIT = 1;
        //public static int TWO_STOPBIT = 2;

        //public static int NO_PARITY = 0;
        //public static int ODD_PARITY = 1;
        //public static int EVEN_PARITY = 2;
        //public static int MARK_PARITY = 3;
        //public static int SPACE_PARITY = 4;


        [DllImport("UniComm.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr ucomCreatePlcCommEx(int module, int sub, bool bWorkSlave);

        [DllImport("UniComm.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr ucomCreatePlcComm(int module, int sub);

        [DllImport("UniComm.dll")]
        private static extern void ucomDestroyPlcComm(IntPtr hComm);
        [DllImport("UniComm.dll")]
        private static extern void ucomSetTimeout(IntPtr hComm, int timeout);
        [DllImport("UniComm.dll")]
        private static extern int ucomGetTimeout(IntPtr hComm);

        [DllImport("UniComm.dll")]
        private static extern void ucomSetDevID(IntPtr hComm, int id);

        [DllImport("UniComm.dll")]
        private static extern int ucomGetDevID(IntPtr hComm);
        [DllImport("UniComm.dll")]
        private static extern void ucomSetDestID(IntPtr hComm, int id);
        [DllImport("UniComm.dll")]
        private static extern int ucomGetDestID(IntPtr hComm);
        [DllImport("UniComm.dll")]
        private static extern bool ucomSetTcpipOptions(IntPtr hComm, string strIp, int port, bool bReconnect);
        [DllImport("UniComm.dll")]
        private static extern bool ucomSetSerialCommOptions(IntPtr hComm, ScommOptions options);

        [DllImport("UniComm.dll")]
        private static extern bool ucomOpen(IntPtr hComm);

        [DllImport("UniComm.dll")]
        private static extern bool ucomIsOpened(IntPtr hComm);

        [DllImport("UniComm.dll")]
        private static extern void ucomClose(IntPtr hComm);
        [DllImport("UniComm.dll")]
        private static extern bool ucomWriteBitE1(IntPtr hComm, int memtype, int address, byte nValue);

        [DllImport("UniComm.dll")]
        private static extern bool ucomWriteShort(IntPtr hComm, int memtype, int address, short nValue);
        [DllImport("UniComm.dll")]
        private static extern bool ucomWriteFloat(IntPtr hComm, int memtype, int address, float nValue);

        [DllImport("UniComm.dll")]
        private static extern bool ucomWriteBitArray(IntPtr hComm, int memtype, int address, [MarshalAs(UnmanagedType.LPArray)] byte[] valueArray, int count);

        [DllImport("UniComm.dll")]
        private static extern bool ucomWriteWordArray(IntPtr hComm, int memtype, int address, [MarshalAs(UnmanagedType.LPArray)] short[] valueArray, int count);

        [DllImport("UniComm.dll")]
        private static extern bool ucomReadBitE1(IntPtr hComm, int memtype, int address, ref byte pValue);

        [DllImport("UniComm.dll")]
        private static extern bool ucomReadShort(IntPtr hComm, int memtype, int address, ref short pValue);
        [DllImport("UniComm.dll")]
        private static extern bool ucomReadFloat(IntPtr hComm, int memtype, int address, ref float pValue);

        [DllImport("UniComm.dll")]
        private static extern bool ucomReadBitArray(IntPtr hComm, int memtype, int address, [MarshalAs(UnmanagedType.LPArray)] byte[] pValueArray, int count);
        [DllImport("UniComm.dll")]
        private static extern bool ucomReadWordArray(IntPtr hComm, int memtype, int address, [MarshalAs(UnmanagedType.LPArray)] short[] pValueArray, int count);
        [DllImport("UniComm.dll")]
        private static extern bool ucomReadText(IntPtr hComm, int memtype, int address, int dataType, StringBuilder strOut, int size);
        [DllImport("UniComm.dll")]
        private static extern bool ucomWriteText(IntPtr hComm, int memtype, int address, int dataType, string strText);

        [DllImport("UniComm.dll")]
        private static extern bool ucomWriteBuffer(IntPtr hComm, IntPtr pData, int size);
        [DllImport("UniComm.dll")]
        private static extern bool ucomReadBuffer(IntPtr hComm, IntPtr pData, ref int size);


        [DllImport("UniComm.dll")]
        private static extern void ucomSetByteOrder(IntPtr hComm, int dataType, int order);
        [DllImport("UniComm.dll")]
        private static extern int ucomGetByteOrder(IntPtr hComm, int dataType);
        [DllImport("UniComm.dll")]
        private static extern void ucomSetDataReceivedCallback(IntPtr hComm, DataReceivedDelegate fnDataReceivedDelegate, IntPtr pUserData);
        [DllImport("UniComm.dll")]
        private static extern void ucomSetDataSentCallback(IntPtr hComm, DataSentDelegate fnDataSentDelegate, IntPtr pUserData);
        [DllImport("UniComm.dll")]
        private static extern void ucomSetModbusRtuDataCallback(IntPtr hComm, ModbusRtuDataDelegate fnModbusRtuDataDelegate, IntPtr pUserData);
        [DllImport("UniComm.dll")]
        private static extern bool ucomReadInt(IntPtr hComm, int memtype, int address, ref int pValue);

        [DllImport("UniComm.dll")]
        private static extern short ucomCalcCrcWord(IntPtr pData, int size);

        private IntPtr m_hComm = IntPtr.Zero;

        ~AnyComm()
        {
            Destroy();
        }

        public static PlcDataType GetCmdDataType(ModbusCommand _cmd)
        {
            PlcDataType type;
            if (_cmd == ModbusCommand.ReadCoilStatus/*.MB_RCS*/ ||
                _cmd == ModbusCommand.ReadInputStatus/*MB_RIS*/ ||
                _cmd == ModbusCommand.ForceSingleCoil/*MB_FSC*/ ||
                _cmd == ModbusCommand.ForceMultipleCoils/*MB_FMC*/)
            {
                type = PlcDataType.Bit;
            }
            else
            {
                type = PlcDataType.Word;
            }

            return type;
        }

        public bool Create(int module, int sub, bool bWorkSlave)
        {
            if (m_hComm != IntPtr.Zero)
            {
                throw new Exception("This object created already");
                // return false;
            }
            m_hComm = ucomCreatePlcCommEx(module, sub, bWorkSlave);
            if (m_hComm == IntPtr.Zero) return false;

            return true;
        }

        public bool Create(int module, int sub = 0)
        {
            if (m_hComm != IntPtr.Zero)
            {
                throw new Exception("This object created already");
                // return false;
            }

            m_hComm = ucomCreatePlcComm(module, sub);
            if (m_hComm == IntPtr.Zero) return false;

            return true;
        }

        public void Destroy()
        {
            if (m_hComm != IntPtr.Zero)
            {
                ucomDestroyPlcComm(m_hComm);
                m_hComm = IntPtr.Zero;
            }
        }

        public bool Open()
        {
            if (m_hComm != IntPtr.Zero)
            {
                return ucomOpen(m_hComm);
            }

            return false;
        }

        public void Close()
        {
            if (m_hComm != IntPtr.Zero)
            {
                ucomClose(m_hComm);
            }
        }

        public void SetTimeOut(int timeout)
        {
            if (m_hComm != IntPtr.Zero)
            {
                ucomSetTimeout(m_hComm, timeout);
            }

        }
        public int GetTimeOut()
        {
            if (m_hComm != IntPtr.Zero)
            {
                return ucomGetTimeout(m_hComm);
            }
            return 0;
        }
        public void SetLocalID(int id)
        {
            if (m_hComm != IntPtr.Zero)
            {
                ucomSetDevID(m_hComm, id);
            }

        }

        public int GetLocalID()
        {
            if (m_hComm != IntPtr.Zero)
            {
                return ucomGetDevID(m_hComm);
            }

            return -1;
        }

        public void SetRemoteID(int id)
        {
            if (m_hComm != IntPtr.Zero)
            {
                ucomSetDestID(m_hComm, id);
            }

        }

        public int GetRemoteID()
        {
            if (m_hComm != IntPtr.Zero)
            {
                return ucomGetDestID(m_hComm);
            }

            return -1;
        }

        public bool SetTcpipOptions(string strIp, int port, bool bReconnect)
        {
            if (m_hComm != IntPtr.Zero)
            {
                return ucomSetTcpipOptions(m_hComm, strIp, port, bReconnect);
            }

            return false;
        }

        public bool SetSerialCommOptions(ScommOptions options)
        {
            if (m_hComm != IntPtr.Zero)
            {
                return ucomSetSerialCommOptions(m_hComm, options);
            }

            return false;
        }


        public bool IsOpened()
        {
            if (m_hComm != IntPtr.Zero)
            {
                return ucomIsOpened(m_hComm);
            }

            return false;
        }

        public void SetByteOrder(int dataType, int order)
        {
            if (m_hComm != IntPtr.Zero)
            {
                ucomSetByteOrder(m_hComm, dataType, order);
            }

        }
        public int GetByteOrder(int dataType)
        {
            if (m_hComm != IntPtr.Zero)
            {
                return ucomGetByteOrder(m_hComm, dataType);
            }

            return -1;
        }


        public bool WriteBit(int memtype, int address, byte nValue)
        {
            if (m_hComm != IntPtr.Zero)
            {
                if (!IsOpened())
                {
                    return false;
                }

                return ucomWriteBitE1(m_hComm, memtype, address, nValue);
            }
            return false;
        }

        public bool WriteWord(int memtype, int address, short nValue)
        {
            if (m_hComm != IntPtr.Zero)
            {
                if (!IsOpened())
                {
                    return false;
                }

                return ucomWriteShort(m_hComm, memtype, address, nValue);
            }
            return false;
        }

        public bool WriteFloat(int memtype, int address, float nValue)
        {
            if (m_hComm != IntPtr.Zero)
            {
                if (!IsOpened())
                {
                    return false;
                }
                return ucomWriteFloat(m_hComm, memtype, address, nValue);
            }

            return false;
        }

        public bool WriteBitArray(int memtype, int address, byte[] valueArray, int count)
        {
            if (m_hComm != IntPtr.Zero)
            {
                if (!IsOpened())
                {
                    return false;
                }

                return ucomWriteBitArray(m_hComm, memtype, address, valueArray, count);
            }
            return false;
        }

        public bool WriteWordArray(int memtype, int address, short[] valueArray, int count)
        {
            if (m_hComm != IntPtr.Zero)
            {
                if (!IsOpened())
                {
                    return false;
                }

                return ucomWriteWordArray(m_hComm, memtype, address, valueArray, count);
            }
            return false;
        }

        public bool ReadBit(int memtype, int address, ref byte nValue)
        {
            if (m_hComm != IntPtr.Zero)
            {
                if (!IsOpened())
                {
                    return false;
                }

                return ucomReadBitE1(m_hComm, memtype, address, ref nValue);
            }
            return false;
        }

        public bool ReadWord(int memtype, int address, ref short nValue)
        {
            if (m_hComm != IntPtr.Zero)
            {
                if (!IsOpened())
                {
                    return false;
                }

                return ucomReadShort(m_hComm, memtype, address, ref nValue);
            }
            return false;
        }



        public bool ReadFloat(int memtype, int address, ref float nValue)
        {
            if (m_hComm != IntPtr.Zero)
            {
                if (!IsOpened())
                {
                    return false;
                }

                return ucomReadFloat(m_hComm, memtype, address, ref nValue);
            }
            return false;
        }


        public bool ReadBitArray(int memtype, int address, byte[] valueArray, int count)
        {
            if (m_hComm != IntPtr.Zero)
            {
                if (!IsOpened())
                {
                    return false;
                }

                return ucomReadBitArray(m_hComm, memtype, address, valueArray, count); ;
            }
            return false;
        }

        public bool ReadWordArray(int memtype, int address, short[] valueArray, int count)
        {
            if (m_hComm != IntPtr.Zero)
            {
                if (!IsOpened())
                {
                    return false;
                }

                return ucomReadWordArray(m_hComm, memtype, address, valueArray, count);
            }
            return false;
        }

        public bool ReadText(int memtype, int address, int dataType, int maxLen, ref string strOut)
        {
            if (m_hComm != IntPtr.Zero)
            {
                if (!IsOpened())
                {
                    return false;
                }

                // const int maxlen = 2408;
                StringBuilder sb = new StringBuilder(maxLen + 1);

                if (ucomReadText(m_hComm, memtype, address, dataType, sb, maxLen))
                {
                    strOut = sb.ToString();
                    return true;
                }
            }
            return false;
        }

        public bool WriteText(int memtype, int address, int dataType, string strText)
        {
            if (m_hComm != IntPtr.Zero)
            {
                if (!IsOpened())
                {
                    return false;
                }
                return ucomWriteText(m_hComm, memtype, address, dataType, strText);
            }
            return false;
        }
        public void SetDataReceivedCallback(DataReceivedDelegate fnDataReceivedDelegate, IntPtr pUserData)
        {
            if (m_hComm != IntPtr.Zero)
            {
                ucomSetDataReceivedCallback(m_hComm, fnDataReceivedDelegate, pUserData);
            }
        }
        public void SetDataSentCallback(DataSentDelegate fnDataSentDelegate, IntPtr pUserData)
        {
            if (m_hComm != IntPtr.Zero)
            {
                ucomSetDataSentCallback(m_hComm, fnDataSentDelegate, pUserData);
            }
        }
        public void SetModbusRtuDataCallback(ModbusRtuDataDelegate fnModbusRtuDataDelegate, IntPtr pUserData)
        {
            if (m_hComm != IntPtr.Zero)
            {
                ucomSetModbusRtuDataCallback(m_hComm, fnModbusRtuDataDelegate, pUserData);
            }
        }

        public bool ReadInt(int memtype, int address, ref int pValue)
        {
            if (m_hComm != IntPtr.Zero)
            {
                return ucomReadInt(m_hComm, memtype, address, ref pValue);
            }

            return false;
        }

        public static short CalcCrcWord(byte[] data)
        {
            if (data.Length == 0) return 0;

            IntPtr p = Marshal.AllocHGlobal(data.Length);

            Marshal.Copy(data, 0, p, data.Length);

            short n = ucomCalcCrcWord(p, data.Length);

            Marshal.FreeHGlobal(p);

            return n;

        }
        public bool WriteBuffer(byte[] pData, int size)
        {
            if (m_hComm != IntPtr.Zero)
            {
                if (!IsOpened())
                {
                    return false;
                }

                IntPtr ptr = Marshal.AllocHGlobal(size);

                Marshal.Copy(pData, 0, ptr, size);
                bool ret = ucomWriteBuffer(m_hComm, ptr, size);

                Marshal.FreeHGlobal(ptr);

                return ret;
            }
            return false;
        }

        public bool ReadBuffer(byte[] pData, ref int size)
        {
            if (m_hComm != IntPtr.Zero)
            {
                if (!IsOpened())
                {
                    return false;
                }
                IntPtr ptr = Marshal.AllocHGlobal(size);

                int _sz = size;
                bool ret = ucomReadBuffer(m_hComm, ptr, ref _sz);

                if (ret)
                {
                    Marshal.Copy(ptr, pData, 0, _sz);
                    size = _sz;
                }

                Marshal.FreeHGlobal(ptr);

                return ret;
            }
            return false;
        }
    }
}

