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
    public class KCmdQueue
    {
        [DllImport("UniDevice.dll")]
        public static extern IntPtr uniCreateQueue(uint bufferId, uint slaveId);

        [DllImport("UniDevice.dll")]
        public static extern void uniDestroyQueue(IntPtr hQueue);
        [DllImport("UniDevice.dll")]
        public static extern void uniQueueClear(IntPtr hQueue);
        [DllImport("UniDevice.dll")]
        public static extern int uniQueueGetCount(IntPtr hQueue);

        [DllImport("UniDevice.dll")]
        public static extern void uniQueueRemoveAt(IntPtr hQueue, int index);

        [DllImport("UniDevice.dll")]
        public static extern void uniQueueRemoveHead(IntPtr hQueue);
        [DllImport("UniDevice.dll")]
        public static extern int uniQueueMoveTo(IntPtr hQueue, ushort axis, double n, IntPtr hOptions);

        [DllImport("UniDevice.dll")]
        public static extern int uniQueueMoveToXY(IntPtr hQueue, ushort axisX, ushort axisY, double x, double y, IntPtr hOptions);
        [DllImport("UniDevice.dll")]
        public static extern int uniQueueMoveToXYZ(IntPtr hQueue, ushort axisX, ushort axisY, ushort axisZ, double x, double y, double z, IntPtr hOptions);
        [DllImport("UniDevice.dll")]
        public static extern int uniQueueLineTo(IntPtr hQueue, ushort axisX, ushort axisY, double x, double y, IntPtr hOptions);
        [DllImport("UniDevice.dll")]
        public static extern int uniQueueLineToXYZ(IntPtr hQueue, ushort axisX, ushort axisY, ushort axisZ, double x, double y, double z, IntPtr hOptions);

        [DllImport("UniDevice.dll")]
        public static extern int uniQueueArcTo(IntPtr hQueue, ushort axisX, ushort axisY, double cx, double cy, double ex, double ey, int dir, IntPtr hOptions);
        [DllImport("UniDevice.dll")]
        public static extern int uniQueuePwmput(IntPtr hQueue, int channel, int level);
        [DllImport("UniDevice.dll")]
        public static extern int uniQueueOutput(IntPtr hQueue, int bit, int level);
        [DllImport("UniDevice.dll")]
        public static extern int uniQueueDelay(IntPtr hQueue, int duration);

        [DllImport("UniDevice.dll")]
        public static extern void uniQueueSetAllAxesE1(IntPtr hQueue, ushort axisX, ushort axisY);
        [DllImport("UniDevice.dll")]
        public static extern void uniQueueSetAllAxesE2(IntPtr hQueue, ushort axisX, ushort axisY, ushort axisZ);



        private IntPtr m_hQueue = IntPtr.Zero;
        //////////////////////////////////////////

        ~KCmdQueue()
        {
            Destroy();
        }

        public IntPtr GetHandle() { return m_hQueue; }
        public void Create(uint bufferId, uint slaveId)
        {
            if (m_hQueue != IntPtr.Zero)
            {
                throw new Exception("Object exist already!");
            }

            m_hQueue = uniCreateQueue(bufferId, slaveId);
        }
        public void Destroy()
        {
            if (m_hQueue != IntPtr.Zero)
            {
                uniDestroyQueue(m_hQueue);
                m_hQueue = IntPtr.Zero;
            }
        }

        public void Clear()
        {
            if (m_hQueue != IntPtr.Zero)
            {
                uniQueueClear(m_hQueue);

            }
        }
        public int GetCount()
        {
            if (m_hQueue != IntPtr.Zero)
            {
                return uniQueueGetCount(m_hQueue);

            }
            return 0;
        }
        public void RemoveAt(int index)
        {
            if (m_hQueue != IntPtr.Zero)
            {
                uniQueueRemoveAt(m_hQueue, index);

            }
        }
        public void RemoveHead()
        {
            if (m_hQueue != IntPtr.Zero)
            {
                uniQueueRemoveHead(m_hQueue);

            }
        }

        public int MoveTo(ushort axis, double n, KMoptions options)
        {
            if (m_hQueue != IntPtr.Zero)
            {
                return uniQueueMoveTo(m_hQueue, axis, n, (options != null ? options.GetHandle() : IntPtr.Zero));

            }
            return 0;
        }

        public int MoveTo(ushort axisX, ushort axisY, double x, double y, KMoptions options)
        {
            if (m_hQueue != IntPtr.Zero)
            {
                return uniQueueMoveToXY(m_hQueue, axisX, axisY, x, y, (options != null ? options.GetHandle() : IntPtr.Zero));

            }
            return 0;
        }
        public int MoveTo(ushort axisX, ushort axisY, ushort axisZ, double x, double y, double z, KMoptions options)
        {
            if (m_hQueue != IntPtr.Zero)
            {
                return uniQueueMoveToXYZ(m_hQueue, axisX, axisY, axisZ, x, y, z, (options != null ? options.GetHandle() : IntPtr.Zero));

            }
            return 0;
        }

        public int LineTo(ushort axisX, ushort axisY, double x, double y, KMoptions options)
        {
            if (m_hQueue != IntPtr.Zero)
            {
                return uniQueueLineTo(m_hQueue, axisX, axisY, x, y, (options != null ? options.GetHandle() : IntPtr.Zero));

            }
            return 0;
        }

        public int LineTo(ushort axisX, ushort axisY, ushort axisZ, double x, double y, double z, KMoptions options)
        {
            if (m_hQueue != IntPtr.Zero)
            {
                return uniQueueLineToXYZ(m_hQueue, axisX, axisY, axisZ, x, y, z, (options != null ? options.GetHandle() : IntPtr.Zero));

            }
            return 0;
        }

        public int ArcTo(ushort axisX, ushort axisY, double cx, double cy, double ex, double ey, int dir, KMoptions options)
        {
            if (m_hQueue != IntPtr.Zero)
            {
                return uniQueueArcTo(m_hQueue, axisX, axisY, cx, cy, ex, ey, dir, (options != null ? options.GetHandle() : IntPtr.Zero));

            }

            return 0;
        }

        public void SetAllAxes(ushort axisX, ushort axisY)
        {
            if (m_hQueue != IntPtr.Zero)
            {
                uniQueueSetAllAxesE1(m_hQueue, axisX, axisY);
            }

        }
        public void SetAllAxes(ushort axisX, ushort axisY, ushort axisZ)
        {
            if (m_hQueue != IntPtr.Zero)
            {
                uniQueueSetAllAxesE2(m_hQueue, axisX, axisY, axisZ);
            }

        }

        public int Pwmput(int channel, int level)
        {
            if (m_hQueue != IntPtr.Zero)
            {
                return uniQueuePwmput(m_hQueue, channel, level);
            }

            return 0;
        }

        public int Output(int bit, int level)
        {
            if (m_hQueue != IntPtr.Zero)
            {
                return uniQueueOutput(m_hQueue, bit, level);
            }

            return 0;
        }
        public int Delay(int duration)
        {
            if (m_hQueue != IntPtr.Zero)
            {
                return uniQueueDelay(m_hQueue, duration);
            }

            return 0;
        }


    }
}
