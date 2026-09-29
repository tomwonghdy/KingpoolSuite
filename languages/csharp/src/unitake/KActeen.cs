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

using Kingpool.Utility;
using Kingpool.Core;


namespace Kingpool.AutoTab
{
    public class KActeen
    {
        public const int AC_FREE = 0;  //自由执行
        public const int AC_INITIATIVE = 1;  //主动执行(默认)
        public const int AC_SUBMISSIVE = 2;  //被动执行
        public const int AC_HYBRID = 3;  //混合的执行
        public const int AC_EXCLUSIVE = 4; //排它主动执行

        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniCreateActionJog(uint id, ushort axis);
        [DllImport("UniDevice.dll")]
        private static extern void uniDestroyAction(IntPtr hAction);
        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniCreateActionHome(uint id, ushort axis);
        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniCreateActionMove(uint id, ushort axis, double pos, bool bAbsolute);
        [DllImport("UniDevice.dll")]
        private static extern void uniActionSetId(IntPtr hAction, uint id);
        [DllImport("UniDevice.dll")]
        private static extern uint uniActionGetId(IntPtr hAction);

        [DllImport("UniDevice.dll")]
        private static extern void uniActionSetConcurrence(IntPtr hAction, int type);
        //  [DllImport("UniDevice.dll")]
        //  private static extern  void    uniActionSetRobot(IntPtr hAction, IntPtr hRobot);
        //  [DllImport("UniDevice.dll")]
        //  private static extern  void    uniActionSetOptions(IntPtr hAction, IntPtr hOptions);
        //  [DllImport("UniDevice.dll")]
        //   private static extern  IntPtr    uniActionGetOptions(IntPtr hAction);

        [DllImport("UniDevice.dll")]
        private static extern void uniActionSetAxis(IntPtr hAction, ushort axis);
        [DllImport("UniDevice.dll")]
        private static extern void uniActionSetAxisE1(IntPtr hAction, ushort axisX, ushort axisY);
        [DllImport("UniDevice.dll")]
        private static extern void uniActionSetAxisE2(IntPtr hAction, ushort axisX, ushort axisY, ushort axisZ);
        [DllImport("UniDevice.dll")]
        private static extern void uniActionSetAxisE3(IntPtr hAction, IntPtr pAxisArray, int axisCount);
        [DllImport("UniDevice.dll")]
        private static extern int uniActionGetAxisCount(IntPtr hAction);
        [DllImport("UniDevice.dll")]
        private static extern void uniActionGetAxisAt(IntPtr hAction, int index, ref ushort pAxisOut);
        [DllImport("UniDevice.dll")]
        private static extern ushort uniActionGetAxisAtE1(IntPtr hAction, int index);

        [DllImport("UniDevice.dll")]
        private static extern void uniActionSetPos(IntPtr hAction, double n);
        [DllImport("UniDevice.dll")]
        private static extern void uniActionSetPosE1(IntPtr hAction, double x, double y);
        [DllImport("UniDevice.dll")]
        private static extern void uniActionSetPosE2(IntPtr hAction, double x, double y, double z);
        [DllImport("UniDevice.dll")]
        private static extern void uniActionSetPosE3(IntPtr hAction, IntPtr pPosArray, int posCount);
        [DllImport("UniDevice.dll")]
        private static extern int uniActionGetPosCount(IntPtr hAction);
        [DllImport("UniDevice.dll")]
        private static extern void uniActionGetPosAt(IntPtr hAction, int index, ref double pPosOut);
        [DllImport("UniDevice.dll")]
        private static extern double uniActionGetPosAtE1(IntPtr hAction, int index);

        [DllImport("UniDevice.dll")]
        private static extern bool uniActionSetJogDir(IntPtr hAction, int dir);
        [DllImport("UniDevice.dll")]
        private static extern bool uniActionStartJog(IntPtr hAction, IntPtr hRobot, IntPtr hOptions, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniActionStopJog(IntPtr hAction, IntPtr hRobot, IntPtr hOptions, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern void uniActionSetMoveAbsolute(IntPtr hAction, bool flag);

        [DllImport("UniDevice.dll")]
        private static extern void uniActionSetPosCount(IntPtr hAction, int count);
        [DllImport("UniDevice.dll")]
        private static extern void uniActionSetPosAt(IntPtr hAction, int index, double pos);
        [DllImport("UniDevice.dll")]
        private static extern void uniActionSetAxisAt(IntPtr hAction, int index, ushort axis);
        [DllImport("UniDevice.dll")]
        private static extern void uniActionSetAxisCount(IntPtr hAction, int count);


        protected IntPtr m_hAction = IntPtr.Zero;
        protected bool m_bAttached = false;

        ~KActeen()
        {
            Destroy();
        }

        public KActeen()
        {
            m_hAction = IntPtr.Zero;
            m_bAttached = false;
        }

        public KActeen(IntPtr hAction, bool bDestroy = false)
        {
            m_hAction = hAction;
            if (m_hAction != IntPtr.Zero)
            {
                m_bAttached = !bDestroy;
            }
        }

        public IntPtr GetHandle() { return m_hAction; }

        public bool CreateActionJog(uint id, ushort axis)
        {
            if (m_hAction != IntPtr.Zero)
            {
                Destroy();
            }

            m_hAction = uniCreateActionJog(id, axis);
            return (m_hAction != IntPtr.Zero);
        }

        public void Destroy()
        {
            if (m_bAttached)
            {
                m_hAction = IntPtr.Zero;
                return;
            }
            else
            {
                if (m_hAction != IntPtr.Zero)
                {
                    uniDestroyAction(m_hAction);
                    m_hAction = IntPtr.Zero;
                }
            }
        }

        public void Attach(IntPtr hAction)
        {
            if (m_hAction != IntPtr.Zero)
            {
                if (m_bAttached == false)
                {
                    uniDestroyAction(m_hAction);
                }
                m_hAction = IntPtr.Zero;
            }

            m_hAction = hAction;
            m_bAttached = true;
        }

        public bool CreateActionHome(uint id, ushort axis)
        {
            if (m_hAction != IntPtr.Zero)
            {
                Destroy();
            }

            m_hAction = uniCreateActionHome(id, axis);
            return (m_hAction != IntPtr.Zero);
        }
        public bool CreateActionMove(uint id, ushort axis, double pos, bool bAbsolute)
        {
            if (m_hAction != IntPtr.Zero)
            {
                Destroy();
            }

            m_hAction = uniCreateActionMove(id, axis, pos, bAbsolute);
            return (m_hAction != IntPtr.Zero);
        }

        public void SetId(uint id)
        {
            if (m_hAction != IntPtr.Zero)
            {
                uniActionSetId(m_hAction, id);
            }
        }
        public uint GetId()
        {
            if (m_hAction != IntPtr.Zero)
            {
                return uniActionGetId(m_hAction);
            }
            return Pool.INVALID_ID;
        }


        public void SetConcurrence(int type)
        {
            if (m_hAction != IntPtr.Zero)
            {
                uniActionSetConcurrence(m_hAction, type);
            }
        }
        //public void SetRobot( IntPtr hRobot)
        //{
        //    if (m_hAction != IntPtr.Zero)
        //    {
        //        uniActionSetRobot(m_hAction, hRobot);
        //    }
        //}
        //public void SetOptions(IntPtr hOptions)
        //{
        //    if (m_hAction != IntPtr.Zero)
        //    {
        //        uniActionSetOptions(m_hAction, hOptions);
        //    }
        //}

        //public IntPtr GetOptions( )
        //{
        //    if (m_hAction != IntPtr.Zero)
        //    {
        //        return  uniActionGetOptions(m_hAction );
        //    }

        //    return IntPtr.Zero;
        //}


        public void SetAxis(ushort axis)
        {
            if (m_hAction != IntPtr.Zero)
            {
                uniActionSetAxis(m_hAction, axis);
            }
        }
        public void SetAxis(ushort axisX, ushort axisY)
        {
            if (m_hAction != IntPtr.Zero)
            {
                uniActionSetAxisE1(m_hAction, axisX, axisY);
            }
        }
        public void SetAxis(ushort axisX, ushort axisY, ushort axisZ)
        {
            if (m_hAction != IntPtr.Zero)
            {
                uniActionSetAxisE2(m_hAction, axisX, axisY, axisZ);
            }
        }

        public void SetAxis(ushort[] axisArray)
        {
            if (m_hAction != IntPtr.Zero)
            {
                IntPtr pAxisArray = IntPtr.Zero;
                int axisCount = axisArray.Length;
                Pool.Assert(false);

                uniActionSetAxisE3(m_hAction, pAxisArray, axisCount);
            }
        }

        public int GetAxisCount()
        {
            if (m_hAction != IntPtr.Zero)
            {
                return uniActionGetAxisCount(m_hAction);
            }
            return 0;
        }

        public void GetAxisAt(int index, ref ushort pAxisOut)
        {
            if (m_hAction != IntPtr.Zero)
            {
                uniActionGetAxisAt(m_hAction, index, ref pAxisOut);
            }
        }

        public ushort GetAxisAt(int index)
        {
            if (m_hAction != IntPtr.Zero)
            {
                return uniActionGetAxisAtE1(m_hAction, index);
            }
            return 0;
        }

        public void SetPos(double n)
        {
            if (m_hAction != IntPtr.Zero)
            {
                uniActionSetPos(m_hAction, n);
            }
        }
        public void SetPos(double x, double y)
        {
            if (m_hAction != IntPtr.Zero)
            {
                uniActionSetPosE1(m_hAction, x, y);
            }
        }
        public void SetPos(double x, double y, double z)
        {
            if (m_hAction != IntPtr.Zero)
            {
                uniActionSetPosE2(m_hAction, x, y, z);
            }
        }


        public void SetPos(double[] posArray)
        {
            if (m_hAction != IntPtr.Zero)
            {
                IntPtr hPosArray = IntPtr.Zero;
                int count = posArray.Length;
                Pool.Assert(false);

                uniActionSetAxisE3(m_hAction, hPosArray, count);
            }
        }

        public int GetPosCount()
        {
            if (m_hAction != IntPtr.Zero)
            {
                return uniActionGetPosCount(m_hAction);
            }

            return 0;
        }
        public void GetPosAt(int index, ref double pPosOut)
        {
            if (m_hAction != IntPtr.Zero)
            {
                uniActionGetPosAt(m_hAction, index, ref pPosOut);
            }
        }

        public double GetPosAt(int index)
        {
            if (m_hAction != IntPtr.Zero)
            {
                return uniActionGetPosAtE1(m_hAction, index);
            }
            return 0;
        }

        public bool SetJogDir(int dir)
        {
            if (m_hAction != IntPtr.Zero)
            {
                return uniActionSetJogDir(m_hAction, dir);
            }
            return false;
        }

        public bool StartJog(KRobot robot, KMoptions options, ref int pErrCode)
        {
            if (m_hAction != IntPtr.Zero)
            {
                return uniActionStartJog(m_hAction, robot.GetHandle(), options.GetHandle(), ref pErrCode);
            }
            return false;
        }

        public bool StopJog(KRobot robot, KMoptions options, ref int pErrCode)
        {
            if (m_hAction != IntPtr.Zero)
            {
                return uniActionStopJog(m_hAction, robot.GetHandle(), options.GetHandle(), ref pErrCode);
            }
            return false;
        }

        public void SetMoveAbsolute(bool flag)
        {
            if (m_hAction != IntPtr.Zero)
            {
                uniActionSetMoveAbsolute(m_hAction, flag);
            }
        }

        public void SetPosCount(int count)
        {
            if (m_hAction != IntPtr.Zero)
            {
                uniActionSetPosCount(m_hAction, count);
            }
        }
        public void SetPosAt(int index, double pos)
        {
            if (m_hAction != IntPtr.Zero)
            {
                uniActionSetPosAt(m_hAction, index, pos);
            }
        }

        public void SetAxisAt(int index, ushort axis)
        {
            if (m_hAction != IntPtr.Zero)
            {
                uniActionSetAxisAt(m_hAction, index, axis);
            }
        }

        public void SetAxisCount(int count)
        {
            if (m_hAction != IntPtr.Zero)
            {
                uniActionSetAxisCount(m_hAction, count);
            }
        }

    }
}
