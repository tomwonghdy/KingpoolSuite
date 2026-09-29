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
    public class KMoptions
    {
        //KMotion Choi Profile
        public const int MTIME_PROFILE_T = 0; //		MO_PT_T=1,
        public const int MTIME_PROFILE_S = 1; //		MO_PT_T=1,
        public const int INVALID_SCENE_ID = 0;

        //队列暂停模式
        public const int QUEUE_PAUSE_MODE_DEFAULT = 0;//使用运动卡方法暂停
        public const int QUEUE_PAUSE_MODE_REARRANGE = 1; //重新组织继续
        public const int QUEUE_PAUSE_MODE_HALT = 2; //终止运行

        //通用
        public const string SPEED_NAME = "Speed";   //fix, double
        public const string ACC_NAME = "Acc";    //fix, double
        public const string DEC_NAME = "Dec";    //fix, double
        public const string PULSE_COUNTER_SOURCE_NAME = "PulseCounterSource"; //int
        public const string METRIC_IN_PULSE_NAME = "MetricInPulse";  //bool
        public const string HOME_MIN_SPEED_NAME = "MinHomeSpeed"; //fix, double
        public const string HOME_MAX_SPEED_NAME = "MaxHomeSpeed";//fix, double
        public const string HOME_CREEP_SPEED_NAME = "CreepSpeed"; //fix, double
        public const string ORIGIN_OFFSET_NAME = "OriginOffset";  //double
        public const string HOME_METHOD_NAME = "HomeMethod"; //int
        public const string PRIME_AXIS_NAME = "PrimeAxis";  // int
        public const string STOP_MODE_NAME = "StopMode";   //fix, int
        public const string HOME_LOGIC_NAME = "HomeLogic";  //fix, int
                                                            //正负硬限位，HOME位， 刹车
        public const string BITNUM_NAME = "bit";  //int
        public const string LOGIC_NAME = "logic"; //int
        public const string ENABLE_NAME = "enable"; //bool
        public const string VALUE_NAME = "value";   //double

        //pwm channel
        public const string PWM_BIT_NAME = "PwmBit";   //double

        public const string FORCE_SOFTLIMIT_OFF_NAME = "ForceSoftLimitOff";

        //队列暂停模式 
        public const string QUEUE_PAUSE_MODE_NAME = "QueuePauseMode";

        //伺服
        //伺服使能
        public const string SERVO_BIT_NAME = "ServoBit";    //fix, int
        public const string SERVO_LOGIC_NAME = "ServoLogic";    //fix, int
        public const string SERVO_ENABLE_NAME = "ServoEnable";   //fix, bool
        //伺服就绪
        public const string DRIVER_READY_BIT_NAME = "ReadyBit";      //fix, int
        public const string DRIVER_READY_LOGIC_NAME = "ReadyLogic";   //fix, int
        public const string DRIVER_READY_ENABLE_NAME = "ReadyEnable";   //fix, bool
        //驱动报警
        public const string ALARM_IN_BIT_NAME = "AlarmInBit";     //fix, int
        public const string ALARM_IN_LOGIC_NAME = "AlarmInLogic";    //fix, int
        public const string ALARM_IN_ENABLE_NAME = "AlarmInEnable";   //fix, bool
                                                                      //清除报警
        public const string CLEAR_ALARM_BIT_NAME = "ClearAlarmBit";     //fix, int
        public const string CLEAR_ALARM_LOGIC_NAME = "ClearAlarmLogic";    //fix, int: ROB_LOW or ROB_HI
        public const string CLEAR_ALARM_ENABLE_NAME = "ClearAlarmEnable";    //fix, bool

        //jog相关参数, zheng motion
        public const string ZMOT_INIT_SPEED_NAME = "InitSpeed"; //double
        public const string ZMOT_SRAMP_TIME_NAME = "SrampTime"; //double , 加减速时间
        //googol
        public const string GGOL_EMERG_DEC_NAME = "EmergDec";  //急停减速度
        //motion time
        public const string MTIM_ACC2_NAME = "acc2";    //加加速度, double
        public const string MTIM_DEC2_NAME = "dec2";  //减减速度, double
        public const string MTIM_PROFILE_NAME = "profile";   //规划曲线, int

        public const string MOPT_MIN_SPEED_NAME = "MinSpeed";      // double (in second)
        public const string MOPT_MAX_SPEED_NAME = "MaxSpeed";    // double (in second)
        public const string MOPT_STOP_SPEED_NAME = "StopSpeed";     // double (in second)


        //Leisai DMC5000
        public const string DMC5_HOME_DIRECTION_NAME = "HomeDirection";    //回零方向, int ROB_DIR_NEG or ROB_DIR_POS
        public const string DMC5_HOME_SPEED_TYPE_NAME = "HomeSpeedType";       //回零速度, int  
        public const string DMC5_ACC_TIME_NAME = "AccTime";       //加速时间  double (in second)
        public const string DMC5_DEC_TIME_NAME = "DecTime";       //减速时间 double (in second)
        public const string DMC5_VENDOR_MODEL_NAME = "VendorModel";       // text
        public const string DMC5400_NAME = "Dmc5400";             //属性值 of DMC5_VENDOR_MODEL_NAME
        public const string DMC5_FILTER_TIME_NAME = "FilterTime";

        //pwm跟随
        public const string DMC5_PWM_FOLLOW_SPEED_MODE = "PwmFollowSpeedMode";  //pwd速度跟随模式
        public const string DMC5_PWM_FOLLOW_MAX_SPEED = "PwmFollowMaxSpeed";  //pwd速度跟随模式
        public const string DMC5_PWM_FOLLOW_MAX_VAL = "PwmFollowMaxValue";  //pwd速度跟随模式
        public const string DMC5_PWM_FOLLOW_OUTVAL = "PwmFollowOutValue";  //pwd速度跟随模式
        public const string DMC5_PWM_FOLLOW_CHANNEL = "PwmFollowChannel";   //pwd速度跟随模式

        //连续插补暂停输出
        public const string DMC5_PAUSE_QUEUE_OUTPUT_HANDLE = "PauseQueueOutputHandle";
        public const string DMC5_PAUSE_QUEUE_OUTPUT_MASK = "PauseQueueOutputMask";
        public const string DMC5_LOOK_AHEAD_MODE = "LookAheadMode";


        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniCreateOptions();
        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniCreateOptionsEx(double speed, double acc, double dec);
        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniCloneOptions(IntPtr hOptions);

        [DllImport("UniDevice.dll")]
        private static extern void uniDestroyOptions(IntPtr hOptions);
        [DllImport("UniDevice.dll")]
        private static extern void uniOptionsCopyFrom(IntPtr hOptions, IntPtr hSource);
        [DllImport("UniDevice.dll")]
        private static extern bool uniOptionSetInt(IntPtr hOptions, string strName, int val);
        [DllImport("UniDevice.dll")]
        private static extern bool uniOptionSetDouble(IntPtr hOptions, string strName, double val);
        [DllImport("UniDevice.dll")]
        private static extern bool uniOptionSetBool(IntPtr hOptions, string strName, bool val);

        [DllImport("UniDevice.dll")]
        private static extern double uniOptionGetSpeed(IntPtr hOptions);
        [DllImport("UniDevice.dll")]
        private static extern double uniOptionGetAcc(IntPtr hOptions);
        [DllImport("UniDevice.dll")]
        private static extern double uniOptionGetDec(IntPtr hOptions);
        [DllImport("UniDevice.dll")]
        private static extern void uniOptionSetSpeed(IntPtr hOptions, double speed);
        [DllImport("UniDevice.dll")]
        private static extern void uniOptionSetAcc(IntPtr hOptions, double acc);
        [DllImport("UniDevice.dll")]
        private static extern void uniOptionSetDec(IntPtr hOptions, double dec);

        //返回脉冲
        [DllImport("UniDevice.dll")]
        private static extern double uniOptionGetSpeedE1(IntPtr hOptions, double lead, int motorReso); //V2.3
        [DllImport("UniDevice.dll")]
        private static extern double uniOptionGetAccE1(IntPtr hOptions, double lead, int motorReso);   //V2.3
        [DllImport("UniDevice.dll")]
        private static extern double uniOptionGetDecE1(IntPtr hOptions, double lead, int motorReso);   //V2.3
        [DllImport("UniDevice.dll")]
        private static extern int uniOptionGetStopMode(IntPtr hOptions);          //V2.3
        [DllImport("UniDevice.dll")]
        private static extern int uniOptionSetStopMode(IntPtr hOptions, int mode);  //V2.3
        [DllImport("UniDevice.dll")]

        private static extern int uniOptionGetExtentParaCount(IntPtr hOptions);   //V2.3
        [DllImport("UniDevice.dll")]
        private static extern void uniOptionRemoveAllExtentParas(IntPtr hOptions); //V2.3
        [DllImport("UniDevice.dll")]
        private static extern bool uniOptionSet(IntPtr hOptions, string strName, string strValue);    //V2.3	
        [DllImport("UniDevice.dll")]
        private static extern bool uniOptionSetE1(IntPtr hOptions, string strName, double nValue);    //V2.3	
        [DllImport("UniDevice.dll")]
        private static extern bool uniOptionSetE2(IntPtr hOptions, string strName, bool bValue);  //V2.3
        [DllImport("UniDevice.dll")]
        private static extern bool uniOptionSetE3(IntPtr hOptions, string strName, int nValue);//V2.3
        [DllImport("UniDevice.dll")]
        private static extern RvBool uniOptionGet(IntPtr hOptions, string strName, StringBuilder strValue, int maxLength);        //V2.3
        [DllImport("UniDevice.dll")]
        private static extern RvBool uniOptionGetE1(IntPtr hOptions, string strName, ref double nValue);    //V2.3
        [DllImport("UniDevice.dll")]
        private static extern RvBool uniOptionGetE2(IntPtr hOptions, string strName, ref bool bValue);  //V2.3
        [DllImport("UniDevice.dll")]
        private static extern RvBool uniOptionGetE3(IntPtr hOptions, string strName, ref int nValue);       //V2.3
        [DllImport("UniDevice.dll")]

        private static extern bool uniOptionIsParaExist(IntPtr hOptions, string strName);    //V2.3	 
        [DllImport("UniDevice.dll")]
        //快捷方式，用于SIO
        private static extern RvBool uniOptionIsEnabled(IntPtr hOptions, string strName);   //V2.3
        [DllImport("UniDevice.dll")]
        //RvBool IsReversed();
        //返回-1表示错误
        private static extern int uniOptionGetBitNum(IntPtr hOptions, string strName); //V2.3
        [DllImport("UniDevice.dll")]
        private static extern int uniOptionGetLogic(IntPtr hOptions, string strName);  //V2.3
        [DllImport("UniDevice.dll")]
        //获得回零类型
        private static extern RvBool uniOptionGetHomeType(IntPtr hOptions, ref int type);      //V2.3

        //转置信号
        //该函数有限获得strName参数的逻辑值， 
        //如果值为ROB_HI, 那么转置level
        //其它情况level保持不变
        [DllImport("UniDevice.dll")]
        private static extern bool uniOptionToppleLevel(IntPtr hOptions, string strName, ref int level);  //V2.3

        //辅助函数，无需类实例
        //lead: 导程, reso: 细分
        [DllImport("UniDevice.dll")]
        private static extern double uniOptionCalcPulseInMetric(double metric, double lead, double reso);  //V2.3
                                                                                                           //使用脉冲当量时的情况
        [DllImport("UniDevice.dll")]
        private static extern double uniOptionCalcPulseInMetricEx(bool bUseEqual, double metric, double lead, double reso);  //V2.3
        [DllImport("UniDevice.dll")]
        private static extern double uniOptionCalcMetricInPulse(double pulse, double lead, double reso);  //V2.3
        [DllImport("UniDevice.dll")]                                                                                          //脉冲当量计算mm
        private static extern double uniOptionCalcMetricInPulseE1(double pulse, double ppu/* pulse per unit*/);  //V2.3
        [DllImport("UniDevice.dll")]
        //计算脉冲当量,导程>=1
        private static extern double uniOptionCalcPulseEqual(double lead, double reso);  //V2.3
        [DllImport("UniDevice.dll")]
        //速度与时间的关系： V =U + A*t
        //V为目标速度， U为初速度， A为加速度， t为加速时间
        //如果U初速度为0， t = V/A
        private static extern double uniOptionCalcAccTime(double init, double speed, double acc); //V2.3

        //速度与时间的关系： V =U - D*t
        //V为目标速度， U为初速度， D为减速度， t为减速时间
        //如果U初速度为0， t = V/A
        [DllImport("UniDevice.dll")]
        private static extern double uniOptionCalcDecTime(double speed, double final, double dec); //V2.3

        //计算加速度，减速度
        [DllImport("UniDevice.dll")]
        private static extern double uniOptionCalcAccSpeed(double init, double speed, double dura); //V2.3
        [DllImport("UniDevice.dll")]
        private static extern double uniOptionCalcDecSpeed(double speed, double final, double dura);//V2.3

        ~KMoptions()
        {
            Destroy();
        }
        public KMoptions()
        {
            Create();
        }

        public KMoptions(IntPtr handle, bool bRelease = false)
        {
            if (bRelease)
            {
                m_bAttached = false;
                m_handle = handle;
            }
            else
            {
                Attach(handle);
            }
        }
        public void Attach(IntPtr handle)
        {
            if (m_handle != IntPtr.Zero && m_bAttached == false)
            {
                Destroy();
            }

            m_bAttached = true;
            m_handle = handle;
        }

        public IntPtr GetHandle()
        {
            return m_handle;
        }

        private IntPtr m_handle = IntPtr.Zero;
        private bool m_bAttached = false;

        public bool Create(/*string strModel*/)
        {
            if (m_handle != IntPtr.Zero)
            {
                uniDestroyOptions(m_handle);
            }

            m_handle = uniCreateOptions(/*strModel*/);
            return (IntPtr.Zero != m_handle);
        }

        public void Destroy()
        {
            if (IntPtr.Zero != m_handle && !m_bAttached)
            {
                uniDestroyOptions(m_handle);
            }

            m_handle = IntPtr.Zero;
        }

        public bool Create(/*string strModel, */int sceneId, double speed, double acc, double dec)
        {
            if (m_handle != IntPtr.Zero)
            {
                throw new Exception("Object exist already!");
            }

            m_handle = uniCreateOptionsEx(/*strModel,*/   speed, acc, dec);
            return (IntPtr.Zero != m_handle);
        }

        public KMoptions Clone()
        {
            if (m_handle == IntPtr.Zero)
            {
                return null;
            }

            IntPtr h = uniCloneOptions(m_handle);
            if (h == IntPtr.Zero)
            {
                return null;
            }

            KMoptions opt = new KMoptions(h, true);

            return opt;
        }


        //public int GetSceneId()
        //{
        //    if (m_handle != IntPtr.Zero)
        //    {
        //        return uniOptionGetSceneId(m_handle);
        //    }

        //    //invalid scene id
        //    return 0;
        //}

        //public void SetSceneId(int id)
        //{
        //    if (m_handle != IntPtr.Zero)
        //    {
        //        uniOptionSetSceneId(m_handle, id);
        //    }
        //}


        //public void Reset()
        //{
        //    Destroy();
        //}

        public bool Set(string strName, double val)
        {
            if (IntPtr.Zero == m_handle)
            {
                return false;
            }

            return uniOptionSetE1(m_handle, strName, val);
        }

        public bool Set(string strName, int val)
        {
            if (IntPtr.Zero == m_handle)
            {
                return false;
            }

            return uniOptionSetE3(m_handle, strName, val);
        }

        public bool Set(string strName, bool val)
        {
            if (IntPtr.Zero == m_handle)
            {
                return false;
            }

            return uniOptionSetE2(m_handle, strName, val);
        }


        public bool SetDouble(string strName, double val)
        {
            if (IntPtr.Zero == m_handle)
            {
                return false;
            }

            return uniOptionSetDouble(m_handle, strName, val);
        }

        public bool SetInt(string strName, int val)
        {
            if (IntPtr.Zero == m_handle)
            {
                return false;
            }

            return uniOptionSetInt(m_handle, strName, val);
        }

        public bool SetBool(string strName, bool val)
        {
            if (IntPtr.Zero == m_handle)
            {
                return false;
            }

            return uniOptionSetBool(m_handle, strName, val);
        }

        public double GetSpeed()
        {
            if (IntPtr.Zero == m_handle)
            {
                return 0;
            }
            return uniOptionGetSpeed(m_handle);
        }
        public double GetAcc()
        {
            if (IntPtr.Zero == m_handle)
            {
                return 0;
            }
            return uniOptionGetAcc(m_handle);
        }

        public double GetDec()
        {
            if (IntPtr.Zero == m_handle)
            {
                return 0;
            }
            return uniOptionGetDec(m_handle);
        }

        public void SetSpeed(double speed)
        {
            if (IntPtr.Zero == m_handle)
            {
                return;
            }
            uniOptionSetSpeed(m_handle, speed);
        }

        public void SetAcc(double acc)
        {
            if (IntPtr.Zero == m_handle)
            {
                return;
            }
            uniOptionSetAcc(m_handle, acc);
        }

        public void SetDec(double dec)
        {
            if (IntPtr.Zero == m_handle)
            {
                return;
            }
            uniOptionSetDec(m_handle, dec);
        }

        //public string GetModelName()
        //{
        //    if (m_handle != IntPtr.Zero)
        //    {
        //        IntPtr hName = uniOptionGetModelName(m_handle);

        //        if (hName != IntPtr.Zero)
        //        {
        //            return Marshal.PtrToStringAnsi(hName);
        //        }
        //    }
        //    return null;
        //}
        //public int GetModelId()
        //{
        //    if (m_handle != IntPtr.Zero)
        //    {
        //        return uniOptionGetModelId(m_handle);
        //    }
        //    return 0;
        //}

        //public bool IsMetricsInPulse()
        //{
        //    if (m_handle != IntPtr.Zero)
        //    {
        //        return uniOptionIsMetricsInPulse(m_handle);
        //    }
        //    return false;
        //}

        //public void  SetMetricsInPulse(bool flag)
        //{
        //    if (m_handle != IntPtr.Zero)
        //    {
        //        uniOptionSetMetricsInPulse(m_handle, flag);
        //    } 
        //}

        //public void SetPrimeAxis(ushort axis)
        //{
        //    if (m_handle != IntPtr.Zero)
        //    {
        //        uniOptionSetPrimeAxis(m_handle,   axis);
        //    }
        //}

        //public ushort GetPrimeAxis( )
        //{
        //    if (m_handle != IntPtr.Zero)
        //    {
        //        return uniOptionGetPrimeAxis(m_handle);
        //    }
        //    return 0;
        //}

        //返回脉冲
        public double GetSpeed(double lead, int motorReso)
        {
            if (m_handle != IntPtr.Zero)
            {
                return uniOptionGetSpeedE1(m_handle, lead, motorReso);
            }
            return 0;
        }
        public double GetAcc(double lead, int motorReso)
        {
            if (m_handle != IntPtr.Zero)
            {
                return uniOptionGetAccE1(m_handle, lead, motorReso);
            }
            return 0;
        }
        public double GetDec(double lead, int motorReso)
        {
            if (m_handle != IntPtr.Zero)
            {
                return uniOptionGetDecE1(m_handle, lead, motorReso);
            }
            return 0;
        }
        public int GetStopMode()
        {
            if (m_handle != IntPtr.Zero)
            {
                return uniOptionGetStopMode(m_handle);
            }
            return 0;
        }

        public int SetStopMode(int mode)
        {
            if (m_handle != IntPtr.Zero)
            {
                return uniOptionSetStopMode(m_handle, mode);
            }
            return 0;
        }

        public int GetExtentParaCount()
        {
            if (m_handle != IntPtr.Zero)
            {
                return uniOptionGetExtentParaCount(m_handle);
            }
            return 0;
        }

        public void RemoveAllExtentParas()
        {
            if (m_handle != IntPtr.Zero)
            {
                uniOptionRemoveAllExtentParas(m_handle);
            }
        }

        public bool Set(string strName, string strValue)
        {
            if (m_handle != IntPtr.Zero)
            {
                return uniOptionSet(m_handle, strName, strValue);
            }
            return false;
        }

        public bool SetE1(string strName, double nValue)
        {
            if (m_handle != IntPtr.Zero)
            {
                return uniOptionSetE1(m_handle, strName, nValue);
            }
            return false;
        }

        public bool SetE2(string strName, bool bValue)
        {
            if (m_handle != IntPtr.Zero)
            {
                return uniOptionSetE2(m_handle, strName, bValue);
            }
            return false;
        }

        public bool SetE3(string strName, int nValue)
        {
            if (m_handle != IntPtr.Zero)
            {
                return uniOptionSetE3(m_handle, strName, nValue);
            }
            return false;
        }

        public RvBool Get(string strName, ref string strValue)
        {
            if (m_handle != IntPtr.Zero)
            {
                StringBuilder sb = new StringBuilder(1204);
                RvBool ret = uniOptionGet(m_handle, strName, sb, 1204);

                if (ret == RvBool.True)
                {
                    strValue = sb.ToString();
                }
                return ret;
            }
            return RvBool.Fuzzy;
        }

        public RvBool Get(string strName, ref double nValue)
        {
            if (m_handle != IntPtr.Zero)
            {
                return uniOptionGetE1(m_handle, strName, ref nValue);
            }
            return RvBool.Fuzzy;
        }

        public RvBool Get(string strName, ref bool bValue)
        {
            if (m_handle != IntPtr.Zero)
            {
                return uniOptionGetE2(m_handle, strName, ref bValue);
            }
            return RvBool.Fuzzy;
        }

        public RvBool Get(string strName, ref int nValue)
        {
            if (m_handle != IntPtr.Zero)
            {
                return uniOptionGetE3(m_handle, strName, ref nValue);
            }
            return RvBool.Fuzzy;
        }

        public bool IsParaExist(string strName)
        {
            if (m_handle != IntPtr.Zero)
            {
                return uniOptionIsParaExist(m_handle, strName);
            }
            return false;
        }

        public RvBool IsEnabled(string strName)
        {
            if (m_handle != IntPtr.Zero)
            {
                return uniOptionIsEnabled(m_handle, strName);
            }
            return RvBool.Fuzzy;
        }
        public int GetBitNum(string strName)
        {
            if (m_handle != IntPtr.Zero)
            {
                return uniOptionGetBitNum(m_handle, strName);
            }
            return -1;
        }
        public int GetLogic(string strName)
        {
            if (m_handle != IntPtr.Zero)
            {
                return uniOptionGetLogic(m_handle, strName);
            }
            return -1;
        }

        public RvBool GetHomeType(string strName, ref int type)
        {
            if (m_handle != IntPtr.Zero)
            {
                return uniOptionGetHomeType(m_handle, ref type);
            }
            return RvBool.Fuzzy;
        }

        public bool ToppleLevel(string strName, ref int level)
        {
            if (m_handle != IntPtr.Zero)
            {
                return uniOptionToppleLevel(m_handle, strName, ref level);
            }
            return false;
        }

        public static double CalcPulseInMetric(double metric, double lead, double reso)
        {
            return uniOptionCalcPulseInMetric(metric, lead, reso);
        }

        public static double CalcPulseInMetricEx(bool bUseEqual, double metric, double lead, double reso)
        {
            return uniOptionCalcPulseInMetricEx(bUseEqual, metric, lead, reso);
        }

        public static double CalcMetricInPulse(double pulse, double lead, double reso)
        {
            return uniOptionCalcMetricInPulse(pulse, lead, reso);
        }

        public static double CalcMetricInPulseE1(double pulse, double ppu)
        {
            return uniOptionCalcMetricInPulseE1(pulse, ppu);
        }
        public static double CalcPulseEqual(double lead, double reso)
        {
            return uniOptionCalcPulseEqual(lead, reso);
        }

        public static double CalcAccTime(double init, double speed, double acc)
        {
            return uniOptionCalcAccTime(init, speed, acc);
        }

        public static double CalcDecTime(double speed, double final, double dec)
        {
            return uniOptionCalcDecTime(speed, final, dec);
        }

        public static double CalcAccSpeed(double init, double speed, double dura)
        {
            return uniOptionCalcAccSpeed(init, speed, dura);
        }

        public static double CalcDecSpeed(double speed, double final, double dura)
        {
            return uniOptionCalcDecSpeed(speed, final, dura);
        }

    }
}
