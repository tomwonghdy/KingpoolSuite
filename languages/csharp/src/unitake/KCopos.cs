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

    public class KCopos
    {

        [DllImport("UniDevice.dll")]
        public static extern IntPtr uniCreatePos();
        [DllImport("UniDevice.dll")]
        public static extern void uniDestroyPos(IntPtr hPos);
        [DllImport("UniDevice.dll")]
        public static extern void uniPosSetXY(IntPtr hPos, double x, double y);
        [DllImport("UniDevice.dll")]
        public static extern void uniPosSetXYZ(IntPtr hPos, double x, double y, double z);
        [DllImport("UniDevice.dll")]
        public static extern void uniPosSetXYZR(IntPtr hPos, double x, double y, double z, double r);
        [DllImport("UniDevice.dll")]
        public static extern int uniPosSetArray(IntPtr hPos, double[] posArr, int count);

        [DllImport("UniDevice.dll")]
        public static extern int uniPosGetCount(IntPtr hPos);


        [DllImport("UniDevice.dll")]
        public static extern double uniPosGetAt(IntPtr hPos, int index);
        [DllImport("UniDevice.dll")]
        public static extern IntPtr uniPosSetAt(IntPtr hPos, int index, double pos);

        private IntPtr m_hPos = IntPtr.Zero;
        /******************************************************************************/
        ~KCopos()
        {
            Destroy();
        }
        public IntPtr GetHandle() { return m_hPos; }
        public void Create()
        {
            if (m_hPos != IntPtr.Zero)
            {
                throw new Exception("Object exist already!");
            }

            m_hPos = uniCreatePos();
        }
        public void Destroy()
        {
            if (m_hPos != IntPtr.Zero)
            {
                uniDestroyPos(m_hPos);
                m_hPos = IntPtr.Zero;
            }
        }

        public void SetXY(double x, double y)
        {
            if (m_hPos != IntPtr.Zero)
            {
                uniPosSetXY(m_hPos, x, y);

            }

        }
        public void SetXYZ(double x, double y, double z)
        {
            if (m_hPos != IntPtr.Zero)
            {
                uniPosSetXYZ(m_hPos, x, y, z);

            }

        }
        public void SetXYZR(double x, double y, double z, double r)
        {
            if (m_hPos != IntPtr.Zero)
            {
                uniPosSetXYZR(m_hPos, x, y, z, r);
            }

        }
        public int SetArray(double[] posArr, int count)
        {
            if (m_hPos != IntPtr.Zero)
            {
                return uniPosSetArray(m_hPos, posArr, count);
            }
            return 0;

        }

        public int GetCount()
        {
            if (m_hPos != IntPtr.Zero)
            {
                return uniPosGetCount(m_hPos);
            }
            return 0;

        }
        public double GetAt(int index)
        {
            if (m_hPos != IntPtr.Zero)
            {
                return uniPosGetAt(m_hPos, index);
            }
            return 0;

        }
        public void SetAt(int index, double pos)
        {
            if (m_hPos != IntPtr.Zero)
            {
                uniPosSetAt(m_hPos, index, pos);
            }

        }

    }
}
