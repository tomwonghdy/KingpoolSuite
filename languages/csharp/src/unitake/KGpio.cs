//**************************************************************************************************
//Copyright (c) 2022-2026 
//Shenzhen SoftStorm Technology Co., Limited  
//All rights reserved.

//Licensed under the MIT license. See LICENCE file in the project root for full license information.
//***************************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Kingpool.AutoTab
{
    public class KGpio
    {
        public const int IN = 0;
        public const int OUT = 1;

        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniCreateGpio(int type, int bit, bool bReverse);
        [DllImport("UniDevice.dll")]
        private static extern void uniDestroyGpio(IntPtr hGpio);
        [DllImport("UniDevice.dll")]
        private static extern void uniGpioSetMotion(IntPtr hGpio, IntPtr hMotion);
        [DllImport("UniDevice.dll")]
        private static extern void uniGpioSetBit(IntPtr hGpio, int bit);
        [DllImport("UniDevice.dll")]
        private static extern int uniGpioGetBit(IntPtr hGpio);
        [DllImport("UniDevice.dll")]
        private static extern bool uniGpioSetLevel(IntPtr hGpio, int level, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniGpioGetLevel(IntPtr hGpio, ref int pLevel, ref int pErrCode);
        //输出适用
        [DllImport("UniDevice.dll")]
        private static extern bool uniGpioTopple(IntPtr hGpio, ref int pErrCode);


        private IntPtr m_hGpio = IntPtr.Zero;
        /******************************************************************************/
        ~KGpio()
        {
            Destroy();
        }

        public KGpio(int type, int bit, bool bReverse)
        {
            Create(type, bit, bReverse);
        }

        public IntPtr GetHandle() { return m_hGpio; }

        public void Create(int type, int bit, bool bReverse)
        {
            if (m_hGpio != IntPtr.Zero)
            {
                Destroy();
            }
            m_hGpio = uniCreateGpio(type, bit, bReverse);
        }

        public void Destroy()
        {
            if (m_hGpio != IntPtr.Zero)
            {
                uniDestroyGpio(m_hGpio);
                m_hGpio = IntPtr.Zero;
            }
        }

        public void SetMotion(KMotion motion)
        {
            if (m_hGpio != IntPtr.Zero)
            {
                uniGpioSetMotion(m_hGpio, motion.GetHandle());
            }
        }

        public void SetBit(int bit)
        {
            if (m_hGpio != IntPtr.Zero)
            {
                uniGpioSetBit(m_hGpio, bit);
            }
        }
        public int GetBit(int bit)
        {
            if (m_hGpio != IntPtr.Zero)
            {
                return uniGpioGetBit(m_hGpio);
            }
            return 0;
        }

        public bool SetLevel(int level, ref int pErrCode)
        {
            if (m_hGpio != IntPtr.Zero)
            {
                return uniGpioSetLevel(m_hGpio, level, ref pErrCode);
            }
            return false;
        }

        public bool GetLevel(ref int pLevel, ref int pErrCode)
        {
            if (m_hGpio != IntPtr.Zero)
            {
                return uniGpioGetLevel(m_hGpio, ref pLevel, ref pErrCode);
            }
            return false;
        }

        public bool Topple(ref int pErrCode)
        {
            if (m_hGpio != IntPtr.Zero)
            {
                return uniGpioTopple(m_hGpio, ref pErrCode);
            }
            return false;
        }

    }
}
