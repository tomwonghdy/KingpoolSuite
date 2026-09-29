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
using System.Runtime.InteropServices;

namespace Kingpool.AutoTab
{
    public class KMaxis
    {
        [DllImport("UniDevice.dll")]
        public static extern IntPtr uniCreateAxes();

        [DllImport("UniDevice.dll")]
        public static extern void uniDestroyAxes(IntPtr haxes);
        [DllImport("UniDevice.dll")]
        public static extern void uniAxesMakeX(IntPtr haxes, ushort axisX);
        [DllImport("UniDevice.dll")]
        public static extern void uniAxesMakeXY(IntPtr haxes, ushort axisX, ushort axisY);
        [DllImport("UniDevice.dll")]
        public static extern void uniAxesMakeXYZ(IntPtr haxes, ushort axisX, ushort axisY, ushort axisZ);
        [DllImport("UniDevice.dll")]
        public static extern void uniAxesMakeXYZR(IntPtr haxes, ushort axisX, ushort axisY, ushort axisZ, ushort axisR);

        [DllImport("UniDevice.dll")]
        public static extern void uniAxesSetAxisCount(IntPtr haxes, int count);
        [DllImport("UniDevice.dll")]
        public static extern void uniAxesSetAxisAt(IntPtr haxes, int index, ushort axis);


        private IntPtr m_hAxes = IntPtr.Zero;


        /******************************************************************************/
        public KMaxis()
        {
            Create();
        }

        public KMaxis(ushort axisX, ushort axisY)
        {
            Create(axisX, axisY);
        }
        public KMaxis(ushort axisX, ushort axisY, ushort axisZ)
        {
            Create(axisX, axisY, axisZ);
        }
        public KMaxis(ushort axisX, ushort axisY, ushort axisZ, ushort axisR)
        {
            Create(axisX, axisY, axisZ, axisR);
        }

        ~KMaxis()
        {
            Destroy();
        }

        public IntPtr GetHandle() { return m_hAxes; }



        public void Create()
        {
            if (m_hAxes != IntPtr.Zero)
            {
                throw new Exception("Object exist already!");
            }

            m_hAxes = uniCreateAxes();

            MakeX(1);
        }

        public void Create(ushort axisX, ushort axisY)
        {
            if (m_hAxes != IntPtr.Zero)
            {
                throw new Exception("Object exist already!");
            }

            m_hAxes = uniCreateAxes();

            MakeXY(axisX, axisY);

        }
        public void Create(ushort axisX, ushort axisY, ushort axisZ)
        {
            if (m_hAxes != IntPtr.Zero)
            {
                throw new Exception("Object exist already!");
            }

            m_hAxes = uniCreateAxes();

            MakeXYZ(axisX, axisY, axisZ);

        }
        public void Create(ushort axisX, ushort axisY, ushort axisZ, ushort axisR)
        {
            if (m_hAxes != IntPtr.Zero)
            {
                throw new Exception("Object exist already!");
            }

            m_hAxes = uniCreateAxes();

            MakeXYZR(axisX, axisY, axisZ, axisR);
        }

        public void Destroy()
        {
            if (m_hAxes != IntPtr.Zero)
            {
                uniDestroyAxes(m_hAxes);
                m_hAxes = IntPtr.Zero;
            }
        }

        public void MakeX(ushort axisX)
        {
            if (m_hAxes != IntPtr.Zero)
            {
                uniAxesMakeX(m_hAxes, axisX);

            }
        }
        public void MakeXY(ushort axisX, ushort axisY)
        {
            if (m_hAxes != IntPtr.Zero)
            {
                uniAxesMakeXY(m_hAxes, axisX, axisY);
            }
        }
        public void MakeXYZ(ushort axisX, ushort axisY, ushort axisZ)
        {
            if (m_hAxes != IntPtr.Zero)
            {
                uniAxesMakeXYZ(m_hAxes, axisX, axisY, axisZ);

            }
        }
        public void MakeXYZR(ushort axisX, ushort axisY, ushort axisZ, ushort axisR)
        {
            if (m_hAxes != IntPtr.Zero)
            {
                uniAxesMakeXYZR(m_hAxes, axisX, axisY, axisZ, axisR);

            }
        }

        public void SetAxisCount(int coount)
        {
            if (m_hAxes != IntPtr.Zero)
            {
                uniAxesSetAxisCount(m_hAxes, coount);

            }
        }
        public void SetAxisAt(int index, ushort axisX)
        {
            if (m_hAxes != IntPtr.Zero)
            {
                uniAxesSetAxisAt(m_hAxes, index, axisX);

            }
        }
    }
}
