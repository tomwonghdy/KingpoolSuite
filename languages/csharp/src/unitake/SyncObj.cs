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

using Kingpool.Core;

namespace Kingpool.AutoTab
{

    public class Signal
    {
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr uniCreateSignal(string strName, bool initState, bool bAutoReset);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern void uniDestroySignal(IntPtr hSignal);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern void uniSignalExcite(IntPtr hSignal);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern void uniSignalReset(IntPtr hSignal);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern bool uniSignalCapture(IntPtr hSignal, int timeout);

        private IntPtr m_hSignal = IntPtr.Zero;

        ~Signal()
        {
            Destroy();
        }

        public bool Create(string strName, bool initState, bool bAutoReset)
        {
            if (m_hSignal != IntPtr.Zero)
            {
                Destroy();
            }

            m_hSignal = uniCreateSignal(strName, initState, bAutoReset);

            return (m_hSignal != IntPtr.Zero);
        }
        public void Destroy()
        {
            if (m_hSignal != IntPtr.Zero)
            {
                uniDestroySignal(m_hSignal);
                m_hSignal = IntPtr.Zero;
            }
        }

        public void Reset()
        {
            if (m_hSignal != IntPtr.Zero)
            {
                uniSignalReset(m_hSignal);
            }
        }

        public void Excite()
        {
            if (m_hSignal != IntPtr.Zero)
            {
                uniSignalExcite(m_hSignal);
            }
        }

        public bool Capture(int timeout)
        {
            if (m_hSignal != IntPtr.Zero)
            {
                return uniSignalCapture(m_hSignal, timeout);
            }
            return false;
        }


    }
    public class Crition
    {
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr uniCreateCrition(string strName);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern void uniDestroyCrition(IntPtr hCrition);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern bool uniCritionLock(IntPtr hCrition, uint actId, bool condition, ref int errCode);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern void uniCritionUnlock(IntPtr hCrition, uint actId);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern void uniCritionSetRobot(IntPtr hCrition, IntPtr hRobot);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi)]
        public static extern bool uniCritionTryLock(IntPtr hCrition, uint actId, bool condition, ref int pErrCode);

        private IntPtr m_hCrition = IntPtr.Zero;

        public Crition()
        {
            Create(null);
        }

        ~Crition()
        {
            Destroy();
        }
        public bool Create(string strName)
        {
            if (m_hCrition != IntPtr.Zero)
            {
                Destroy();
            }

            m_hCrition = uniCreateCrition(strName);

            return (m_hCrition != IntPtr.Zero);
        }
        public void Destroy()
        {
            if (m_hCrition != IntPtr.Zero)
            {
                uniDestroyCrition(m_hCrition);
                m_hCrition = IntPtr.Zero;
            }
        }

        public bool Lock(uint actId, bool condition, ref int errCode)
        {
            if (m_hCrition != IntPtr.Zero)
            {
                return uniCritionLock(m_hCrition, actId, condition, ref errCode);
            }
            return false;
        }

        public bool TryLock(uint actId, bool condition, ref int errCode)
        {
            if (m_hCrition != IntPtr.Zero)
            {
                return uniCritionTryLock(m_hCrition, actId, condition, ref errCode);
            }
            return false;
        }


        public void Unlock(uint actId)
        {
            if (m_hCrition != IntPtr.Zero)
            {
                uniCritionUnlock(m_hCrition, actId);
            }
        }

        public void SetRobot(KRobot robot)
        {
            if (m_hCrition != IntPtr.Zero)
            {
                uniCritionSetRobot(m_hCrition, robot.GetHandle());
            }
        }


    }


    public class Token
    {
        [DllImport("UniSupport.dll")]
        private static extern IntPtr uniCreateToken(IntPtr hRobot);
        [DllImport("UniSupport.dll")]
        private static extern void uniDestroyToken(IntPtr hToken);
        [DllImport("UniSupport.dll")]
        private static extern void uniTokenReset(IntPtr hToken);
        [DllImport("UniSupport.dll")]
        private static extern int uniTokenDeliver(IntPtr hToken, uint actId, int count);          //发放令牌
        [DllImport("UniSupport.dll")]
        private static extern int uniTokenWait(IntPtr hToken, bool bWaitAfterFinish, int timeout);  //所有令牌返到手，返回0, 否则返回错误值
        [DllImport("UniSupport.dll")]
        private static extern int uniTokenGet(IntPtr hToken, uint actId, int timeout);   //子动作获取令牌
        [DllImport("UniSupport.dll")]
        private static extern void uniTokenBack(IntPtr hToken, uint actId);  //子动作返回令牌


        private IntPtr m_hToken = IntPtr.Zero;


        /******************************************************************************/

        ~Token()
        {
            Destroy();
        }

        public IntPtr GetHandle() { return m_hToken; }

        public void Create(IntPtr hRobot)
        {
            if (m_hToken != IntPtr.Zero)
            {
                throw new Exception("Object exist already!");
            }

            m_hToken = uniCreateToken(hRobot);
        }
        public void Destroy()
        {
            if (m_hToken != IntPtr.Zero)
            {
                uniDestroyToken(m_hToken);
                m_hToken = IntPtr.Zero;
            }
        }

        public void Reset()
        {
            if (m_hToken != IntPtr.Zero)
            {
                uniTokenReset(m_hToken);
            }
        }

        public int Deliver(uint actId, int count)
        {
            if (m_hToken != IntPtr.Zero)
            {
                return uniTokenDeliver(m_hToken, actId, count);
            }

            return 0;
        }

        public int Wait(bool bWaitAfterFinish, int timeout)
        {
            if (m_hToken != IntPtr.Zero)
            {
                return uniTokenWait(m_hToken, bWaitAfterFinish, timeout);
            }

            return -1;
        }


        public int Get(uint actId, int timeout)
        {
            if (m_hToken != IntPtr.Zero)
            {
                return uniTokenGet(m_hToken, actId, timeout);
            }

            return -1;
        }

        public void Back(uint actId)
        {
            if (m_hToken != IntPtr.Zero)
            {
                uniTokenBack(m_hToken, actId);
            }
        }

    }

    public class Gate
    {
        [DllImport("UniSupport.dll")]
        public static extern IntPtr uniCreateGate(int count);
        [DllImport("UniSupport.dll")]
        public static extern void uniDestroyGate(IntPtr hGate);
        [DllImport("UniSupport.dll")]
        public static extern int uniGateSetCount(IntPtr hGate, int count);
        [DllImport("UniSupport.dll")]
        public static extern int uniGateGetCount(IntPtr hGate);
        [DllImport("UniSupport.dll")]
        public static extern void uniGateReset(IntPtr hGate, bool bForceUnlock);
        [DllImport("UniSupport.dll")]
        public static extern void uniGateSetAll(IntPtr hGate, bool bForceUnlock);
        [DllImport("UniSupport.dll")]
        public static extern bool uniGateIsOn(IntPtr hGate, int index);
        [DllImport("UniSupport.dll")]
        public static extern bool uniGateIsOff(IntPtr hGate, int index);
        [DllImport("UniSupport.dll")]
        public static extern void uniGateSetOff(IntPtr hGate, int index);
        [DllImport("UniSupport.dll")]
        public static extern void uniGateSetOn(IntPtr hGate, int index);
        [DllImport("UniSupport.dll")]
        public static extern void uniGateSetOffE1(IntPtr hGate, int index, uint actId);
        [DllImport("UniSupport.dll")]
        public static extern void uniGateSetOnE1(IntPtr hGate, int index, uint actId);
        [DllImport("UniSupport.dll")]
        public static extern bool uniGateIsAllOff(IntPtr hGate);
        [DllImport("UniSupport.dll")]
        public static extern bool uniGateIsAllOn(IntPtr hGate);
        [DllImport("UniSupport.dll")]
        public static extern bool uniGateLock(IntPtr hGate, int index, uint actId);
        [DllImport("UniSupport.dll")]
        public static extern void uniGateUnlock(IntPtr hGate, int index, uint actId, bool bAutoSetOff);
        [DllImport("UniSupport.dll")]
        public static extern bool uniGateIsLocked(IntPtr hGate, int index);
        [DllImport("UniSupport.dll")]
        public static extern uint uniGateGetActionId(IntPtr hGate, int index);

        private IntPtr m_hGate = IntPtr.Zero;

        ~Gate()
        {
            Destroy();
        }

        public bool Create(int count)
        {
            if (m_hGate != IntPtr.Zero)
            {
                Destroy();
            }

            m_hGate = uniCreateGate(count);

            return (m_hGate != IntPtr.Zero);
        }
        public void Destroy()
        {
            if (m_hGate != IntPtr.Zero)
            {
                uniDestroyGate(m_hGate);
                m_hGate = IntPtr.Zero;
            }
        }

        public int SetCount(int count)
        {
            if (m_hGate != IntPtr.Zero)
            {
                return uniGateSetCount(m_hGate, count);
            }
            return 0;
        }

        public int GetCount()
        {
            if (m_hGate != IntPtr.Zero)
            {
                return uniGateGetCount(m_hGate);
            }
            return 0;
        }
        public void Reset(bool bForceUnlock)
        {
            if (m_hGate != IntPtr.Zero)
            {
                uniGateReset(m_hGate, bForceUnlock);
            }

        }
        public void SetAll(bool bForceUnlock)
        {
            if (m_hGate != IntPtr.Zero)
            {
                uniGateSetAll(m_hGate, bForceUnlock);
            }
        }

        public bool IsOn(int index)
        {
            if (m_hGate != IntPtr.Zero)
            {
                return uniGateIsOn(m_hGate, index);
            }
            return false;
        }
        public bool IsOff(int index)
        {
            if (m_hGate != IntPtr.Zero)
            {
                return uniGateIsOff(m_hGate, index);
            }
            return false;
        }


        public void SetOff(int index)
        {
            if (m_hGate != IntPtr.Zero)
            {
                uniGateSetOff(m_hGate, index);
            }
        }
        public void SetOn(int index)
        {
            if (m_hGate != IntPtr.Zero)
            {
                uniGateSetOn(m_hGate, index);
            }
        }

        public bool IsAllOff(int index)
        {
            if (m_hGate != IntPtr.Zero)
            {
                return uniGateIsAllOff(m_hGate);
            }

            return false;
        }
        public bool IsAllOn(int index)
        {
            if (m_hGate != IntPtr.Zero)
            {
                return uniGateIsAllOn(m_hGate);
            }
            return false;
        }

        public bool Lock(int index, uint actId)
        {
            if (m_hGate != IntPtr.Zero)
            {
                return uniGateLock(m_hGate, index, actId);
            }
            return false;
        }

        public void Unlock(int index, uint actId, bool bAutoSetOff)
        {
            if (m_hGate != IntPtr.Zero)
            {
                uniGateUnlock(m_hGate, index, actId, bAutoSetOff);
            }

        }

        public bool IsLocked(int index)
        {
            if (m_hGate != IntPtr.Zero)
            {
                return uniGateIsLocked(m_hGate, index);
            }
            return false;
        }

        public void SetOff(int index, uint actId)
        {
            if (m_hGate != IntPtr.Zero)
            {
                uniGateSetOffE1(m_hGate, index, actId);
            }
        }
        public void SetOn(int index, uint actId)
        {
            if (m_hGate != IntPtr.Zero)
            {
                uniGateSetOnE1(m_hGate, index, actId);
            }
        }

        public uint GetActionId(int index)
        {
            if (m_hGate != IntPtr.Zero)
            {
                return uniGateGetActionId(m_hGate, index);
            }

            return Pool.INVALID_ID;
        }
    }


    class Transtate
    {
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi, EntryPoint = "uniCreateTransitState")]
        private static extern IntPtr uniCreateTransitState(string strName, IntPtr hRobot);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi, EntryPoint = "uniDestroyTransitState")]
        private static extern void uniDestroyTransitState(IntPtr hTransitState);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi, EntryPoint = "uniTransitStateReset")]
        private static extern void uniTransitStateReset(IntPtr hTransitState);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi, EntryPoint = "uniTransitStateSeek")]
        private static extern void uniTransitStateSeek(IntPtr hTransitState, int state);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi, EntryPoint = "uniTransitStateWait")]
        private static extern int uniTransitStateWait(IntPtr hTransitState, uint actId, int timeout);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi, EntryPoint = "uniTransitStateIsStateAccord")]
        private static extern bool uniTransitStateIsStateAccord(IntPtr hTransitState);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi, EntryPoint = "uniTransitStateGetTargetState")]
        private static extern int uniTransitStateGetTargetState(IntPtr hTransitState);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi, EntryPoint = "uniTransitStateIsStateRequested")]
        private static extern bool uniTransitStateIsStateRequested(IntPtr hTransitState);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi, EntryPoint = "uniTransitStateAck")]
        private static extern void uniTransitStateAck(IntPtr hTransitState, int state);
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi, EntryPoint = "uniTransitStateSetRobot")]
        private static extern void uniTransitStateSetRobot(IntPtr hTransitState, IntPtr hRobot);

        private IntPtr m_hTrasitState = IntPtr.Zero;
        //////////////////////////////////////////

        public Transtate()
        {
            Create(null, IntPtr.Zero);
        }
        ~Transtate()
        {
            Destroy();
        }

        public bool Create(string strName, IntPtr hRobot)
        {
            if (m_hTrasitState != IntPtr.Zero)
            {
                Destroy();
            }
            m_hTrasitState = uniCreateTransitState(strName, hRobot);

            return (IntPtr.Zero != m_hTrasitState);

        }

        public void Destroy()
        {
            if (m_hTrasitState != IntPtr.Zero)
            {
                uniDestroyTransitState(m_hTrasitState);
                m_hTrasitState = IntPtr.Zero;
            }
        }

        public void Reset()
        {
            if (m_hTrasitState != IntPtr.Zero)
            {
                uniTransitStateReset(m_hTrasitState);
            }
        }
        //state不能为0
        public void Seek(int state)
        {
            if (m_hTrasitState != IntPtr.Zero)
            {
                uniTransitStateSeek(m_hTrasitState, state);
            }
        }
        public int Wait(uint actId, int timeout)
        {
            if (m_hTrasitState != IntPtr.Zero)
            {
                return uniTransitStateWait(m_hTrasitState, actId, timeout);
            }
            return -1;
        }

        public bool IsStateAccord()
        {
            if (m_hTrasitState != IntPtr.Zero)
            {
                return uniTransitStateIsStateAccord(m_hTrasitState);
            }
            return false;
        }

        public int GetTargetState()
        {
            if (m_hTrasitState != IntPtr.Zero)
            {
                return uniTransitStateGetTargetState(m_hTrasitState);
            }
            return 0;
        }
        public bool IsStateRequested()
        {
            if (m_hTrasitState != IntPtr.Zero)
            {
                return uniTransitStateIsStateRequested(m_hTrasitState);
            }
            return false;
        }
        public void Ack(int state)
        {
            if (m_hTrasitState != IntPtr.Zero)
            {
                uniTransitStateAck(m_hTrasitState, state);
            }
        }
        public void SetRobot(IntPtr hRobot)
        {
            if (m_hTrasitState != IntPtr.Zero)
            {
                uniTransitStateSetRobot(m_hTrasitState, hRobot);
            }
        }

    }
}
