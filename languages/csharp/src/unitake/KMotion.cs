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

using Kingpool.Utility;
using Kingpool.Core;

namespace Kingpool.AutoTab
{

    public enum AXIS_MECHNIC
    {
        UNKNOWN = (-1),
        FREE = 0,
        LINEAR = 1,
        ROTATION = 2,
    }

    public enum AXIS_DRIVER
    {
        UNKNOWN = (-1),
        STEP = 0,
        SERVO = 1,
        ECAT = 2,
    }
    //位置计数器
    public enum AXIS_COUNTER
    {
        DEFAULT = 0,  // 规划位置
        ENCODER = 1,  // 编码器
        RASTER = 2,   // 光栅尺
    }

    //停止模式
    public enum STOP_MODE
    {
        IMMEDIATE = 0,//立即停止
        SMOOTH = 1,//平滑停止
    }

    public enum LOGIC
    {
        LOW = 0,//立即停止
        HI = 1,//平滑停止
    }


    public class KMotion
    {
        public const int GOOGL_ECAT_IO_BIT_OFFS = 10;   //GPIO大地址
        public const string GOOGOL_ECAT_NAME = "GoogolEcat";
        public const string MOTION_TIME_NAME = "MotionTime";
        public const string ZMOTION_NAME = "Zmotion";
        public const string LSDMC5K_NAME = "LeisaiDmc5000";


        //四轴
        public const int A4_X = 1;
        public const int A4_Y = 2;
        public const int A4_Z = 3;
        public const int A4_Y2 = 4;
        public const int A4_R = 4;

        //六轴 
        public const int A6_X = 1;
        public const int A6_Y = 2;
        public const int A6_Z = 3;
        public const int A6_Y2 = 4;
        public const int A6_R = 5;

        //8轴
        //第1个坐标系
        public const int A8_X1 = 1;
        public const int A8_Y1 = 2;
        public const int A8_Z1 = 3;
        public const int A8_R1 = 4;

        //第2个坐标系
        public const int A8_X2 = 5;
        public const int A8_Y2 = 6;
        public const int A8_Z2 = 7;
        public const int A8_R2 = 8;


        //运动控制器专用
        //MotionTime @250121
        public const int MT_FLAG_INV_GIN = 1;
        public const int MT_FLAG_INV_GOUT = (1 << 1);
        //End of @250121


        //轴专用IO
        //轴专用,  ID,配置函数使用
        //public const int SIO_HOME_INDEX  = (1);   //回零索引 
        public const int SIO_HOME = (1 << 1);
        public const int SIO_HARD_NEGLMT = (1 << 2);
        public const int SIO_HARD_POSLMT = (1 << 3);
        public const int SIO_SOFT_NEGLMT = (1 << 4);
        public const int SIO_SOFT_POSLMT = (1 << 5);
        public const int SIO_ENABLE_SERVO = (1 << 6); //伺服使能
        public const int SIO_ALARM_IN = (1 << 7); //伺服报警
        public const int SIO_CLEAR_ALARM = (1 << 8);//清除伺服报警
        public const int SIO_SERVO_READY_IN = (1 << 9); //驱动器已使能
        public const int SIO_BROKE = (1 << 10); //刹车


        //功能IO
        public const int MAX_FIO_COUNT = 10;

        public const int FI_READY = 1001;  //送料
        public const int FI_RESET = 1002;
        public const int FI_START = 1003;
        public const int FI_STOP = 1004;
        public const int FI_PAUSE = 1005;

        //FIO OUTPUT
        public const int FO_LAMP_GREEN = (-101);
        public const int FO_LAMP_YELLOW = (-102);
        public const int FO_LAMP_RED = (-103);
        public const int FO_BEEP = (-104);
        public const int FO_TOOL = (-105);

        //运动方向: 正方向， 负方向
        public const int DIR_POS = 1;
        public const int DIR_NEG = (-1);

        //电平:LOW 低,  HI 高
        public const int LOW = 0;
        public const int HI = 1;

        //LS DMC 5000相关参数
        //@260120
        //回零速度类型，DMC5K
        public const int DMC5_HS_LOW = 0;//低速回零
        public const int DMC5_HS_HIGH = 1;//高速回零
        //回零类型
        public const int DMC5_FIND_ONCE = 0;//一次回零
        public const int DMC5_FIND_BACK = 1;//：一次回零加回找
        public const int DMC5_FIND_TWICE = 2;//：二次回零
        public const int DMC5_ORIGIN_EZ = 3;//3：原点加同向EZ
        public const int DMC5_EZ = 4;      //4：单独记一个EZ
        public const int DMC5_ORIGIN_REZ = 5; // 5：原点加反向EZ
        public const int DMC5_LOCK_ORIGIN = 6;//6：原点锁存
        public const int DMC5_LOCK_ORIGIN_EZ = 7; //原点锁存加同向EZ
        public const int DMC5_LOCK_EZ = 8;//单独记一个EZ锁存
        public const int DMC5_LOCK_ORIGIN_REZ = 9;//9：原点锁存加反向EZ
        public const int DMC5_LIMIT = 10;//10. 一次限位回零
        public const int DMC5_LIMIT_BACK = 11;//11. 一次限位回零加反找
        public const int DMC5_LIMIT_TWICE = 12; // 12. 二次限位回零
                                                //end of @260120



        //googol专用轴状态
        public const int ALM_POS_LMT = 1;         //正限位报警
        public const int ALM_NEG_LMT = (1 << 1);  //负限位报警
        public const int ALM_DRV_ALM = (1 << 2);  //驱动报警
        public const int ALM_ENA_SRV = (1 << 3);  //伺服使能
        public const int ALM_EXC_TOL = (1 << 4);  //随动误差超出限制
        public const int ALM_EM_STP = (1 << 5);  //急停
        public const int ALM_SM_STP = (1 << 6);  //缓停
        public const int ALM_IN_RUN = (1 << 7);  //运行状态

        //AUTOTAB轴状态
        public const int GAS_UNKNOWN = (-1);
        public const int GAS_IDLE = 0;  //无运动，无错误
        public const int GAS_MOVING = 1;  // 运动 
        public const int GAS_PAUSE = 2; //暂停中 
        public const int GAS_ERROR = 4; //错误停止中
        public const int GAS_JOGING = (11);//JOG运动中
        public const int GAS_SINGLE_MOVEING = (12); //单轴运动中
        public const int GAS_MULTI_MOVING = (13); //插补运动中
        public const int GAS_HOMING = (14);//回零运动中
        public const int GAS_JOG_PAUSE = (21); //JOG暂停中
        public const int GAS_STEP_PAUSE = (22);   //单轴暂停中
        public const int GAS_SYNC_PAUSE = (23);  //插补暂停中
        public const int GAS_HOME_PAUSE = (24);  //回零暂停中
        public const int GAS_STOP_IN_EMERG = (43);//急停停止
        public const int GAS_STOP_IN_ALARM = (44);//轴告警错误停止 
        public const int GAS_STOP_WHEN_HOME = (41);//回零时停止 
        public const int GAS_STOP_WHEN_STEP = (42); //单轴移动时停止 
        public const int GAS_STOP_WHEN_MULTIMOVE = (45); //插补移动时停止 


        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniCreateMotion(string strModel, int axisCount, int dinCount, int doutCount, string strOptions, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern void uniDestroyMotion(IntPtr hMotion);
        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniMotionGetModelName(IntPtr hMotion);
        [DllImport("UniDevice.dll")]
        private static extern void uniMotionSetSioValid(IntPtr hMotion, ushort axis, int ids, bool flag);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionIsSioValid(IntPtr hMotion, ushort axis, int id);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionSetSioLevel(IntPtr hMotion, ushort axis, int id, int level, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetSioLevel(IntPtr hMotion, ushort axis, int id, ref int level, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetSioBit(IntPtr hMotion, ushort axis, int id, ref int bit, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionSetSioBit(IntPtr hMotion, ushort axis, int type, int bit, ref int pErrCode);

        //使能SIO,如软限位
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionEnableSio(IntPtr hMotion, ushort axis, int id, bool flag, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionIsSioEnabled(IntPtr hMotion, ushort axis, int id, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetSioLogic(IntPtr hMotion, ushort axis, int id, ref int level, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionSetSioLogic(IntPtr hMotion, ushort axis, int id, int level, ref int pErrCode);


        //v2.3
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionConfigOrigin(IntPtr hMotion, ushort axis, IntPtr hOption, bool bReadOnly, ref int pErrCode);
        //v2.3
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionConfigHardLimit(IntPtr hMotion, ushort axis, int sioId, IntPtr hOption, bool bReadOnly, ref int pErrCode);
        //v2.3
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionConfigSoftLimit(IntPtr hMotion, ushort axis, int sioId, IntPtr hOption, bool bReadOnly, ref int pErrCode);
        //v2.3
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionConfigEmergence(IntPtr hMotion, ushort axis, IntPtr hOption, bool bReadOnly, ref int pErrCode);

        //高级扩展
        //v2.3
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionConfigServo(IntPtr hMotion, ushort axis, IntPtr hOption, bool bReadOnly, ref int pErrCode); //servo
        [DllImport("UniDevice.dll")]                                                                                                                        //v2.3
        private static extern bool uniMotionConfigEtherCAT(IntPtr hMotion, ushort axis, IntPtr hOption, bool bReadOnly, ref int pErrCode); //etherCAT
        [DllImport("UniDevice.dll")]                                                                                                                           //v2.3
        private static extern bool uniMotionConfigCounter(IntPtr hMotion, ushort axis, IntPtr hOption, bool bReadOnly, ref int pErrCode);  //pulse counter

        //刹车
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionAddBrake(IntPtr hMotion, ushort axis, int bit, int logic, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern void uniMotionRemoveBrake(IntPtr hMotion, ushort axis);

        //检查SIO是否可用
        [DllImport("UniDevice.dll")]
        private static extern RvBool uniMotionIsSioValid(IntPtr hMotion, ushort axis, int id, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionCheckHomeType(IntPtr hMotion, ushort axis, IntPtr hOption, ref int type);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionIsDeviceReady(IntPtr hMotion);
        [DllImport("UniDevice.dll")]
        private static extern int uniMotionGetAxisCurState(IntPtr hMotion, ushort axis, ref int pLastCode);
        [DllImport("UniDevice.dll")]
        private static extern int uniMotionGetAxisAbnormalCode(IntPtr hMotion, ushort axis);
        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniMotionGetStateDesc(int state);
        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniMotionCreateOptions(IntPtr hMotion, double speed, double acc, double dec);
        [DllImport("UniDevice.dll")]
        private static extern void uniMotionDestroyOptions(IntPtr hMotion, IntPtr hOptions);

        //[DllImport("UniDevice.dll")]
        //private static extern bool uniMotionGetHomeLevel(IntPtr hMotion, ushort axis, ref int level, ref int pErrCode);

        //[DllImport("UniDevice.dll")]
        //private static extern bool uniMotionGetEmergLevel(IntPtr hMotion, ref int level, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionSetHomeType(IntPtr hMotion, ushort axis, int type, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetHomeType(IntPtr hMotion, ushort axis, ref int type, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionSetPulseMode(IntPtr hMotion, ushort axis, int mode, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetPulseMode(IntPtr hMotion, ushort axis, ref int mode, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionSetAxisVendorType(IntPtr hMotion, ushort axis, int type, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetAxisVendorType(IntPtr hMotion, ushort axis, ref int type, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionSetPulseEqual(IntPtr hMotion, ushort axis, double units, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetPulseEqual(IntPtr hMotion, ushort axis, ref double units, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionEnterEmergState(IntPtr hMotion, IntPtr hOptions, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetCurSpeed(IntPtr hMotion, ushort axis, ref double speed, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionChangeSpeed(IntPtr hMotion, ushort axis, double speed, double accOrDec, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionChangeSpeedE1(IntPtr hMotion, uint csid, double speed, double accOrDec, ref int pErrCode);


        [DllImport("UniDevice.dll")]
        private static extern int uniMotionGetAxisCount(IntPtr hMotion);

        [DllImport("UniDevice.dll")]
        private static extern RvBool uniMotionIsAxisReady(IntPtr hMotion, ushort axis);

        [DllImport("UniDevice.dll")]
        private static extern int uniMotionGetVendorId(IntPtr hMotion);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionSetAxisReady(IntPtr hMotion, ushort axis, bool flag, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern void uniMotionSetDPR(IntPtr hMotion, ushort axis, double val);

        [DllImport("UniDevice.dll")]
        private static extern double uniMotionGetDPR(IntPtr hMotion, ushort axis);

        [DllImport("UniDevice.dll")]
        private static extern void uniMotionSetMotorReso(IntPtr hMotion, ushort axis, int val);

        [DllImport("UniDevice.dll")]
        private static extern int uniMotionGetMotorReso(IntPtr hMotion, ushort axis);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetAxisCurPos(IntPtr hMotion, ushort axis, ref double curpos, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionClearAxisPos(IntPtr hMotion, ushort axis, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetAxisEncoderPos(IntPtr hMotion, ushort axis, ref double curpos, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetAxisRasterPos(IntPtr hMotion, ushort axis, ref double curpos, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetAxisCurPosWithType(IntPtr hMotion, ushort axis, ref double curpos, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetAxisState(IntPtr hMotion, ushort axis, ref int state, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetHomeState(IntPtr hMotion, ushort axis, ref int state, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetCrdState(IntPtr hMotion, uint csid, IntPtr axes, ref int state, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionIsAxisServoEnabled(IntPtr hMotion, ushort axis, ref int state, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionClearAxisServoAlarm(IntPtr hMotion, ushort axis, int duration, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionDelay(IntPtr hMotion, int duration, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionHome(IntPtr hMotion, ushort axis, IntPtr options, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionHomeAny2(IntPtr hMotion, int axisX, int axisY, IntPtr options, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionHomeAny3(IntPtr hMotion, IntPtr axes, IntPtr options, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionHomeAny4(IntPtr hMotion, IntPtr axes, IntPtr options, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionHomeAny(IntPtr hMotion, IntPtr axes, IntPtr options, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionStopHome(IntPtr hMotion, ushort axis, IntPtr options, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionStopHomeAny2(IntPtr hMotion, IntPtr axes, IntPtr options, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionStopHomeAny3(IntPtr hMotion, IntPtr axes, IntPtr options, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionStopHomeAny4(IntPtr hMotion, IntPtr axes, IntPtr options, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionJog(IntPtr hMotion, ushort axis, IntPtr options, int dir, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionStopJog(IntPtr hMotion, ushort axis, IntPtr options, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionMove(IntPtr hMotion, ushort axis, double step, IntPtr options, bool bAbsolute, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionStopAxis(IntPtr hMotion, ushort axis, IntPtr options, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionStopAll(IntPtr hMotion, IntPtr options, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionMoveLine(IntPtr hMotion, uint csid, ushort axisX, ushort axisY, double posX, double posY, IntPtr options, bool bAbsolute, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionMoveLine3d(IntPtr hMotion, uint csid, ushort axisX, ushort axisY, ushort axisZ, double x, double y, double z, IntPtr options, bool bAbsolute, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionMoveLine4d(IntPtr hMotion, uint csid, ushort axisX, ushort axisY, ushort axisZ, ushort axisR, double x, double y, double z, double r, IntPtr options, bool bAbsolute, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionMoveLineXd(IntPtr hMotion, uint csid, IntPtr axes, IntPtr posArray, int count, IntPtr options, bool bAbsolute, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionMoveArc(IntPtr hMotion, ushort axisX, ushort axisY, double startX, double startY, double endX, double endY, double radius, int direct, IntPtr options, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionMoveArcTo(IntPtr hMotion, uint csid, ushort axisX, ushort axisY, double cx, double cy, double ex, double ey, int direct, IntPtr options, ref int pErrCode);


        //[DllImport("UniDevice.dll")]
        //private static extern bool uniMotionStopAxisAny3(IntPtr hMotion, IntPtr axes, IntPtr options, ref int pErrCode);

        //[DllImport("UniDevice.dll")]
        //private static extern bool uniMotionStopAxisAny4(IntPtr hMotion, IntPtr axes, IntPtr options, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionStopAny3(IntPtr hMotion, uint csid, ushort axisX, ushort axisY, ushort axisZ, IntPtr options, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionStopAny4(IntPtr hMotion, uint csid, ushort axisX, ushort axisY, ushort axisZ, ushort axisR, IntPtr options, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionStopAny2(IntPtr hMotion, uint csid, ushort axisX, ushort axisY, IntPtr options, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern int uniMotionGetEmergBit(IntPtr hMotion);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionSetEmergBit(IntPtr hMotion, int bit, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        //急停电平是否为有输入
        private static extern RvBool uniMotionIsEmergOn(IntPtr hMotion, ref int pErrCode);


        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetQueueState(IntPtr hMotion, IntPtr hQueue, ref int state, ref int linenum, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionStopQueue(IntPtr hMotion, IntPtr hQueue, IntPtr hOptions, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionWaitQueue(IntPtr hMotion, IntPtr hQueue, int timeout, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionExecuteQueue(IntPtr hMotion, IntPtr hQueue, int startIndex, int count, IntPtr hOptions, ref int pErrCode);


        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionSetOutputLevel(IntPtr hMotion, int bit, int level, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetOutputLevel(IntPtr hMotion, int bit, ref int level, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetInputLevel(IntPtr hMotion, int bit, ref int level, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionSetBatchOutputLevel(IntPtr hMotion, long levels, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetBatchOutoutLevel(IntPtr hMotion, ref long pLevels, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetBatchInputLevel(IntPtr hMotion, ref long pLevels, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetAuxState(IntPtr hMotion, ushort axis, ref int state, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniMotionGetVersion(IntPtr hMotion);

        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniMotionGetVendorDesc(IntPtr hMotion);

        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniMotionGetErrorDesc(IntPtr hMotion, int errCode);

        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniMotionGetErrorDescE1(string strModel, int errCode);

        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniMotionGetSerialNum(IntPtr hMotion, StringBuilder strOut, int size);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionSetOutputLevelE1(IntPtr hMotion, int offset, int bit, int level, ref int errCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetOutputLevelE1(IntPtr hMotion, int offset, int bit, ref int level, ref int errCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetInputLevelE1(IntPtr hMotion, int offset, int bit, ref int level, ref int errCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionWaitHomeAxis(IntPtr hMotion, ushort axis, int timeout, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionWaitMoveAxis(IntPtr hMotion, ushort axis, int timeout, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern void uniMotionSetPulseEqualValid(IntPtr hMotion, ushort axis, bool flag);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionIsPulseEqualValid(IntPtr hMotion, ushort axis);

        [DllImport("UniDevice.dll")]
        private static extern int uniMotionGetAxisMachType(IntPtr hMotion, ushort axis);
        [DllImport("UniDevice.dll")]
        private static extern void uniMotionSetAxisMachType(IntPtr hMotion, ushort axis, int type);
        [DllImport("UniDevice.dll")]
        private static extern int uniMotionGetMotorDriverType(IntPtr hMotion, ushort axis);
        [DllImport("UniDevice.dll")]
        private static extern void uniMotionSetMotorDriverType(IntPtr hMotion, ushort axis, int type);
        [DllImport("UniDevice.dll")]   //是否为垂直轴
        private static extern void uniMotionSetAxisVertical(IntPtr hMotion, ushort axis, bool flag);
        [DllImport("UniDevice.dll")]  //如果轴号不存在，返回RV_FUZZY
        private static extern RvBool uniMotionIsAxisVertical(IntPtr hMotion, ushort axis);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionOpenPwmChannel(IntPtr hMotion, int channel, int hiDura, int lowDura, ref int errCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionSetPwmOutput(IntPtr hMotion, int channel, int level, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionClosePwmChannel(IntPtr hMotion, int channel, ref int errCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionConfigPwm(IntPtr hMotion, int channel, IntPtr hOptions, bool bReadOnly, ref int pErrCode);
        // private static extern	bool uniMotionSetPwmPeriod(IntPtr hMotion, int channel, int lowTime, int highTime, ref int errCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionEnableSoftLimit(IntPtr hMotion, ushort axis, int id, bool flag, ref int errCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionIsSoftLimitEnabled(IntPtr hMotion, ushort axis, int id, ref int errCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionIsSoftLimitValid(IntPtr hMotion, ushort axis, int id, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern int uniMotionSetSoftLimit(IntPtr hMotion, ushort axis, double lower, double upper, ref int errCode);
        [DllImport("UniDevice.dll")]
        private static extern int uniMotionGetSoftLimit(IntPtr hMotion, ushort axis, ref double lower, ref double upper, ref int errCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionSetSoftLimitE1(IntPtr hMotion, ushort axis, int id, double pos, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionGetSoftLimitE1(IntPtr hMotion, ushort axis, int id, ref double pos, ref int pErrCode);


        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionMoveMany2(IntPtr hMotion, ushort axisX, ushort axisY, double x, double y, IntPtr options, bool bAbsolute, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionStopMany2(IntPtr hMotion, ushort axisX, ushort axisY, IntPtr options, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionWaitMany(IntPtr hMotion, IntPtr hAxes, int timeout, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionWaitCrd(IntPtr hMotion, uint csid, IntPtr hAxes, int timeout, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniMotionIsPwmChannelOpened(IntPtr hMotion, int channel, ref int pErrCode);

        public KMotion()
        {

        }

        public KMotion(IntPtr hMotion, bool bDestroy = false)
        {
            m_hMotion = hMotion;

            if (m_hMotion != IntPtr.Zero)
            {
                m_bReferHandle = (!bDestroy);
            }
        }

        ~KMotion()
        {
            Destroy();
        }

        public static ushort AXIS(int index)
        {
            return (ushort)(index + 1);
        }

        public IntPtr GetHandle() { return m_hMotion; }

        //声明一个空句柄
        private IntPtr m_hMotion = IntPtr.Zero;
        private bool m_bReferHandle = false;

        public void FromHandle(IntPtr h)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                throw new Exception("KMotion instance exists already!");
            }

            m_hMotion = h;
            m_bReferHandle = true;
        }


        //************************************************************************************************
        //创建运动
        public bool Create(string strModel, int axisCount, int dinCount, int doutCount, string strOptions, ref int err)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                throw new Exception("Object exist already!");
            }

            m_hMotion = uniCreateMotion(strModel, axisCount, dinCount, doutCount, strOptions, ref err);

            return (m_hMotion != IntPtr.Zero);
        }

        //删除运动
        public void Destroy()
        {
            //引用的句柄不能删除
            if (m_bReferHandle)
            {
                m_hMotion = IntPtr.Zero;
                m_bReferHandle = false;
                return;
            }

            if (m_hMotion != IntPtr.Zero)
            {
                uniDestroyMotion(m_hMotion);
                m_hMotion = IntPtr.Zero;
            }
        }

        public bool IsValid()
        {
            return (m_hMotion != IntPtr.Zero);
        }



        //public bool SetSoftLimits(ushort axis, int item, double pos, ref int pErrCode)
        //{
        //    if (m_hMotion != IntPtr.Zero)
        //    {
        //        bool result = uniMotionSetSoftLimits(m_hMotion, axis, item, pos, ref pErrCode);
        //        return result;
        //    }
        //    return false;
        //}
        //public bool GetSoftLimits(ushort axis, int item, ref double pos, ref int pErrCode)
        //{
        //    if (m_hMotion != IntPtr.Zero)
        //    {
        //        bool result = uniMotionGetSoftLimits(m_hMotion, axis, item, ref pos, ref pErrCode);
        //        return result;
        //    }
        //    return false;
        //}

        //public bool GetHardLimitLevel(ushort axis, int item, ref int level, ref int pErrCode)
        //{
        //    if (m_hMotion != IntPtr.Zero)
        //    {
        //        bool result = uniMotionGetHardLimitLevel(m_hMotion, axis, item, ref level, ref pErrCode);
        //        return result;
        //    }
        //    return false;
        //}

        //public bool EnableSoftLimit(ushort axis, int item, bool flag, ref int pErrCode)
        //{
        //    if (m_hMotion != IntPtr.Zero)
        //    {
        //        bool result = uniMotionEnableSoftLimit(m_hMotion, axis, item, flag, ref pErrCode);
        //        return result;
        //    }
        //    return false;
        //}

        //public bool IsSoftLimitEnabled(ushort axis, int item, ref bool flag, ref int pErrCode)
        //{
        //    if (m_hMotion != IntPtr.Zero)
        //    {
        //        bool result = uniMotionIsSoftLimitEnabled(m_hMotion, axis, item, ref flag, ref pErrCode);
        //        return result;
        //    }
        //    return false;
        //}

        //public bool EnableHardLimit(ushort axis, int item, bool flag, ref int pErrCode)
        //{
        //    if (m_hMotion != IntPtr.Zero)
        //    {
        //        bool result = uniMotionEnableHardLimit(m_hMotion, axis, item, flag, ref pErrCode);
        //        return result;
        //    }
        //    return false;
        //}

        //public bool IsHardLimitEnabled(ushort axis, int item, ref bool flag, ref int pErrCode)
        //{
        //    if (m_hMotion != IntPtr.Zero)
        //    {
        //        bool result = uniMotionIsHardLimitEnabled(m_hMotion, axis, item, ref flag, ref pErrCode);
        //        return result;
        //    }
        //    return false;
        //}

        //// 获取负限位等级
        //public bool GetNegativeLimitLevel(ushort axis, ref int level, ref int pErrCode)
        //{
        //    if (m_hMotion != IntPtr.Zero)
        //    {
        //        return uniMotionGetNeglmtLevel(m_hMotion, axis, ref level, ref pErrCode);
        //    }
        //    return false;
        //}

        // 获取Home等级
        //public bool GetHomeLevel(ushort axis, ref int level, ref int pErrCode)
        //{
        //    if (m_hMotion != IntPtr.Zero)
        //    {
        //        return uniMotionGetHomeLevel(m_hMotion, axis, ref level, ref pErrCode);
        //    }
        //    return false;
        //}

        //// 获取紧急等级
        //public bool GetEmergencyLevel(ref int level, ref int pErrCode)
        //{
        //    if (m_hMotion != IntPtr.Zero)
        //    {
        //        return uniMotionGetEmergLevel(m_hMotion, ref level, ref pErrCode);
        //    }
        //    return false;
        //}

        public IntPtr CreateOptions(double speed, double acc, double dec)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionCreateOptions(m_hMotion, speed, acc, dec);
            }
            return IntPtr.Zero;
        }
        public void DestroyOptions(IntPtr hOptions)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                uniMotionDestroyOptions(m_hMotion, hOptions);
            }
        }


        // 设置Home类型
        public bool SetHomeType(ushort axis, int type, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionSetHomeType(m_hMotion, axis, type, ref pErrCode);
            }
            return false;
        }

        // 获取Home类型
        public bool GetHomeType(ushort axis, ref int type, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetHomeType(m_hMotion, axis, ref type, ref pErrCode);
            }
            return false;
        }
        // 设置脉冲模式
        public bool SetPulseMode(ushort axis, int mode, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionSetPulseMode(m_hMotion, axis, mode, ref pErrCode);
            }
            return false;
        }

        // 获取脉冲模式
        public bool GetPulseMode(ushort axis, ref int mode, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetPulseMode(m_hMotion, axis, ref mode, ref pErrCode);
            }
            return false;
        }

        // 设置轴供应商类型
        public bool SetAxisVendorType(ushort axis, int type, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionSetAxisVendorType(m_hMotion, axis, type, ref pErrCode);
            }
            return false;
        }

        // 获取轴供应商类型
        public bool GetAxisVendorType(ushort axis, ref int type, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetAxisVendorType(m_hMotion, axis, ref type, ref pErrCode);
            }
            return false;
        }

        // 设置轴单位
        public bool SetPulseEqual(ushort axis, double units, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionSetPulseEqual(m_hMotion, axis, units, ref pErrCode);
            }
            return false;
        }

        // 获取轴单位
        public bool GetPulseEqual(ushort axis, ref double units, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetPulseEqual(m_hMotion, axis, ref units, ref pErrCode);
            }
            return false;
        }

        public void SetPulseEqualValid(ushort axis, bool flag)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                uniMotionSetPulseEqualValid(m_hMotion, axis, flag);
            }
        }

        public bool IsPulseEqualValid(ushort axis)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionIsPulseEqualValid(m_hMotion, axis);
            }
            return false;
        }


        // 进入紧急状态
        public bool EnterEmergencyState(IntPtr hOptions, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionEnterEmergState(m_hMotion, hOptions, ref pErrCode);
            }
            return false;
        }

        public int GetEmergBit()
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetEmergBit(m_hMotion);
            }
            return 0;
        }
        public bool SetEmergBit(int bit, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionSetEmergBit(m_hMotion, bit, ref pErrCode);
            }
            return false;
        }

        public RvBool IsEmergOn(ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionIsEmergOn(m_hMotion, ref pErrCode);
            }
            return RvBool.Fuzzy;
        }

        // 获取当前速度
        public bool GetCurSpeed(ushort axis, ref double speed, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetCurSpeed(m_hMotion, axis, ref speed, ref pErrCode);
            }
            return false;
        }
        //改变单轴速度
        public bool ChangeSpeed(ushort axis, double speed, double accOrDec, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionChangeSpeed(m_hMotion, axis, speed, accOrDec, ref pErrCode);
            }
            return false;
        }

        public bool ChangeSpeed(uint csid, double speed, double accOrDec, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionChangeSpeedE1(m_hMotion, csid, speed, accOrDec, ref pErrCode);
            }
            return false;
        }


        // 获取轴数量
        public int GetAxisCount()
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetAxisCount(m_hMotion);
            }
            return 0;
        }

        // 轴是否就绪
        public RvBool IsAxisReady(ushort axis)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionIsAxisReady(m_hMotion, axis);
            }
            return RvBool.Fuzzy;
        }

        // 获取供应商ID
        public int GetVendorId()
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetVendorId(m_hMotion);
            }
            return 0;
        }

        // 设置轴就绪状态
        public bool SetAxisReady(ushort axis, bool flag, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionSetAxisReady(m_hMotion, axis, flag, ref pErrCode);
            }
            return false;
        }

        // 设置DPR
        public void SetDPR(ushort axis, double val)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                uniMotionSetDPR(m_hMotion, axis, val);
            }
        }

        // 获取DPR
        public double GetDPR(ushort axis)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetDPR(m_hMotion, axis);
            }
            return 0.0;
        }
        public void SetMotorReso(ushort axis, int val)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                uniMotionSetMotorReso(m_hMotion, axis, val);
            }
        }

        // 获取电机分辨率
        public int GetMotorReso(ushort axis)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetMotorReso(m_hMotion, axis);
            }
            return 0;
        }

        // 获取轴当前位置
        public bool GetAxisCurPos(ushort axis, ref double curpos, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetAxisCurPos(m_hMotion, axis, ref curpos, ref pErrCode);
            }
            return false;
        }

        // 清除轴位置
        public bool ClearAxisPos(ushort axis, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionClearAxisPos(m_hMotion, axis, ref pErrCode);
            }
            return false;
        }

        // 获取轴编码器位置
        public bool GetAxisEncoderPos(ushort axis, ref double curpos, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetAxisEncoderPos(m_hMotion, axis, ref curpos, ref pErrCode);
            }
            return false;
        }

        // 获取轴光栅位置
        public bool GetAxisRasterPos(ushort axis, ref double curpos, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetAxisRasterPos(m_hMotion, axis, ref curpos, ref pErrCode);
            }
            return false;
        }

        // 获取轴当前位置（带类型）
        public bool GetAxisCurPosWithType(ushort axis, ref double curpos, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetAxisCurPosWithType(m_hMotion, axis, ref curpos, ref pErrCode);
            }
            return false;
        }

        // 获取轴状态
        public bool GetAxisState(ushort axis, ref int state, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetAxisState(m_hMotion, axis, ref state, ref pErrCode);
            }
            return false;
        }

        // 获取Home状态
        public bool GetHomeState(ushort axis, ref int state, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetHomeState(m_hMotion, axis, ref state, ref pErrCode);
            }
            return false;
        }

        public bool GetCrdState(uint csid, KMaxis axes, ref int state, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetCrdState(m_hMotion, csid, axes.GetHandle(), ref state, ref pErrCode);
            }
            return false;
        }

        // 轴伺服是否启用
        public bool IsAxisServoEnabled(ushort axis, ref int state, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionIsAxisServoEnabled(m_hMotion, axis, ref state, ref pErrCode);
            }
            return false;
        }

        // 清除轴伺服报警
        public bool ClearAxisServoAlarm(ushort axis, int duration, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionClearAxisServoAlarm(m_hMotion, axis, duration, ref pErrCode);
            }
            return false;
        }

        // 延时
        public bool Delay(int duration, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionDelay(m_hMotion, duration, ref pErrCode);
            }
            return false;
        }

        // Home
        public bool Home(ushort axis, KMoptions options, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionHome(m_hMotion, axis, options.GetHandle(), ref pErrCode);
            }
            return false;
        }

        // HomeAny2
        public bool HomeAny2(ushort axisX, ushort axisY, KMoptions options, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionHomeAny2(m_hMotion, axisX, axisY, options.GetHandle(), ref pErrCode);
            }
            return false;
        }

        // HomeAny3
        public bool HomeAny3(ushort axisX, ushort axisY, ushort axisZ, KMoptions options, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                KMaxis axes = new KMaxis(axisX, axisY, axisZ);

                return uniMotionHomeAny3(m_hMotion, axes.GetHandle(), options.GetHandle(), ref pErrCode);
            }
            return false;
        }

        // HomeAny4
        public bool HomeAny4(ushort axisX, ushort axisY, ushort axisZ, ushort axisR, KMoptions options, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                KMaxis axes = new KMaxis(axisX, axisY, axisZ, axisR);

                return uniMotionHomeAny4(m_hMotion, axes.GetHandle(), options.GetHandle(), ref pErrCode);
            }
            return false;
        }

        // HomeAny
        public bool HomeAny(KMaxis axes, KMoptions options, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionHomeAny(m_hMotion, axes.GetHandle(), options.GetHandle(), ref pErrCode);
            }
            return false;
        }

        // StopHome
        public bool StopHome(ushort axis, KMoptions options, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionStopHome(m_hMotion, axis, options.GetHandle(), ref pErrCode);
            }
            return false;
        }

        // StopHomeAny2
        public bool StopHomeAny2(KMaxis axes, KMoptions options, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionStopHomeAny2(m_hMotion, axes.GetHandle(), options.GetHandle(), ref pErrCode);
            }
            return false;
        }
        public bool StopHomeAny3(KMaxis axes, KMoptions options, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionStopHomeAny3(m_hMotion, axes.GetHandle(), options.GetHandle(), ref pErrCode);
            }
            return false;
        }

        // 停止任意Home4
        public bool StopHomeAny4(KMaxis axes, KMoptions options, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionStopHomeAny4(m_hMotion, axes.GetHandle(), options.GetHandle(), ref pErrCode);
            }
            return false;
        }

        // Jog
        public bool Jog(ushort axis, KMoptions options, int dir, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionJog(m_hMotion, axis, options.GetHandle(), dir, ref pErrCode);
            }
            return false;
        }

        // 停止Jog
        public bool StopJog(ushort axis, KMoptions options, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionStopJog(m_hMotion, axis, options.GetHandle(), ref pErrCode);
            }
            return false;
        }

        // 移动
        public bool Move(ushort axis, double step, KMoptions options, bool bAbsolute, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionMove(m_hMotion, axis, step, options.GetHandle(), bAbsolute, ref pErrCode);
            }
            return false;
        }

        // 停止轴
        public bool StopAxis(ushort axis, KMoptions options, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionStopAxis(m_hMotion, axis, options.GetHandle(), ref pErrCode);
            }
            return false;
        }

        public bool StopAll(KMoptions options, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionStopAll(m_hMotion, options.GetHandle(), ref pErrCode);
            }
            return false;
        }


        // 移动任意2
        public bool MoveLine(uint csid, ushort axisX, ushort axisY, double x, double y, KMoptions options, bool bAbsolute, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionMoveLine(m_hMotion, csid, axisX, axisY, x, y, options.GetHandle(), bAbsolute, ref pErrCode);
            }
            return false;
        }

        // 移动任意3
        public bool MoveLine3d(uint csid, ushort axisX, ushort axisY, ushort axisZ, double x, double y, double z, KMoptions options, bool bAbsolute, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {

                return uniMotionMoveLine3d(m_hMotion, csid, axisX, axisY, axisZ, x, y, z, options.GetHandle(), bAbsolute, ref pErrCode);
            }
            return false;
        }

        // 移动任意4
        public bool MoveLine4d(uint csid, ushort axisX, ushort axisY, ushort axisZ, ushort axisR,
                              double x, double y, double z, double r,
                              KMoptions options, bool bAbsolute, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionMoveLine4d(m_hMotion, csid, axisX, axisY, axisZ, axisR, x, y, z, r, options.GetHandle(), bAbsolute, ref pErrCode);
            }
            return false;
        }

        // 移动任意
        public bool MoveLineXd(uint csid, ushort[] axisArray, double[] posArray, KMoptions options, bool bAbsolute, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                if (axisArray.Length != posArray.Length) return false;
                if (axisArray.Length < 2) return false;

                KMaxis axes = new KMaxis();
                axes.SetAxisCount(axisArray.Length);
                for (int i = 0; i < axisArray.Length; i++)
                {
                    axes.SetAxisAt(i, axisArray[i]);
                }

                IntPtr pPosArr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * posArray.Length);

                Marshal.Copy(posArray, 0, pPosArr, posArray.Length);

                bool ret = uniMotionMoveLineXd(m_hMotion, csid, axes.GetHandle(), pPosArr, posArray.Length, options.GetHandle(), bAbsolute, ref pErrCode);

                axes.Destroy();
                Marshal.FreeHGlobal(pPosArr);

                return ret;
            }
            return false;
        }

        // 移动任意2
        public bool MoveArc(ushort axisX, ushort axisY, double startX, double startY, double endX, double endY,
                                double radius, int direct, KMoptions options, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionMoveArc(m_hMotion, axisX, axisY, startX, startY, endX, endY, radius, direct, options.GetHandle(), ref pErrCode);
            }
            return false;
        }

        public bool MoveArcTo(uint csid, ushort axisX, ushort axisY, double cx, double cy, double ex, double ey, int direct, KMoptions options, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionMoveArcTo(m_hMotion, csid, axisX, axisY, cx, cy, ex, ey, direct, options.GetHandle(), ref pErrCode);
            }
            return false;
        }



        // 停止轴任意2
        public bool StopAny2(uint csid, ushort axisX, ushort axisY, KMoptions options, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionStopAny2(m_hMotion, csid, axisX, axisY, options.GetHandle(), ref pErrCode);
            }
            return false;
        }

        // 停止轴任意3
        public bool StopAny3(uint csid, ushort axisX, ushort axisY, ushort axisZ, KMoptions options, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionStopAny3(m_hMotion, csid, axisX, axisY, axisZ, options.GetHandle(), ref pErrCode);
            }
            return false;
        }

        // 停止轴任意4
        public bool StopAxisAny4(uint csid, ushort axisX, ushort axisY, ushort axisZ, ushort axisR, KMoptions options, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionStopAny4(m_hMotion, csid, axisX, axisY, axisZ, axisR, options.GetHandle(), ref pErrCode);
            }
            return false;
        }

        // 获取队列状态
        public bool GetQueueState(IntPtr hQueue, ref int state, ref int linenum, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetQueueState(m_hMotion, hQueue, ref state, ref linenum, ref pErrCode);
            }
            return false;
        }

        // 停止队列
        public bool StopQueue(KCmdQueue queue, KMoptions options, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionStopQueue(m_hMotion, queue.GetHandle(), options.GetHandle(), ref pErrCode);
            }

            return false;
        }



        public bool WaitQueue(KCmdQueue queue, int timeout, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionWaitQueue(m_hMotion, queue.GetHandle(), timeout, ref pErrCode);
            }

            return false;
        }

        public bool ExecuteQueue(IntPtr hQueue, int startIndex, int count, KMoptions options, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionExecuteQueue(m_hMotion, hQueue, startIndex, count, options.GetHandle(), ref pErrCode);
            }
            return false;
        }


        // 设置输出电平
        public bool SetOutputLevel(int bit, int level, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionSetOutputLevel(m_hMotion, bit, level, ref pErrCode);
            }
            return false;
        }

        // 获取输出电平
        public bool GetOutputLevel(int bit, ref int level, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetOutputLevel(m_hMotion, bit, ref level, ref pErrCode);
            }
            return false;
        }
        // 获取输入电平
        public bool GetInputLevel(int bit, ref int level, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetInputLevel(m_hMotion, bit, ref level, ref pErrCode);
            }
            return false;
        }

        // 设置批量输出电平
        public bool SetBatchOutputLevel(long levels, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionSetBatchOutputLevel(m_hMotion, levels, ref pErrCode);
            }
            return false;
        }

        // 获取批量输出电平
        public bool GetBatchOutputLevel(ref long pLevels, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetBatchOutoutLevel(m_hMotion, ref pLevels, ref pErrCode);
            }
            return false;
        }

        // 获取批量输入电平
        public bool GetBatchInputLevel(ref long pLevels, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetBatchInputLevel(m_hMotion, ref pLevels, ref pErrCode);
            }
            return false;
        }

        public bool SetOutputLevelE1(int offset, int bit, int level, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionSetOutputLevelE1(m_hMotion, offset, bit, level, ref pErrCode);
            }
            return false;
        }

        public bool GetOutputLevelE1(int offset, int bit, ref int level, ref int errCode)
        {

            if (m_hMotion != IntPtr.Zero)
            {
                bool result = uniMotionGetOutputLevelE1(m_hMotion, offset, bit, ref level, ref errCode);
                return result;
            }
            return false;
        }

        public bool GetInputLevelE1(int offset, int bit, ref int level, ref int errCode)
        {

            if (m_hMotion != IntPtr.Zero)
            {

                bool result = uniMotionGetInputLevelE1(m_hMotion, offset, bit, ref level, ref errCode);


                return result;
            }
            return false;
        }

        public bool SetSioBit(ushort axis, int type, int bit, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionSetSioBit(m_hMotion, axis, type, bit, ref pErrCode); ;
            }
            return false;
        }


        public bool SetSioLevel(ushort axis, int id, int level, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                bool result = uniMotionSetSioLevel(m_hMotion, axis, id, level, ref pErrCode);
                return result;
            }
            return false;
        }

        public bool GetSioLevel(ushort axis, int id, ref int level, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                bool result = uniMotionGetSioLevel(m_hMotion, axis, id, ref level, ref pErrCode);
                return result;
            }
            return false;
        }

        public bool GetSioBit(ushort axis, int id, ref int bit, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetSioBit(m_hMotion, axis, id, ref bit, ref pErrCode);
            }
            return false;
        }

        //public bool IsSioReversed(ushort axis, int id, ref bool flag, ref int pErrCode)
        //{
        //    if (m_hMotion != IntPtr.Zero)
        //    {
        //        return uniMotionIsSioReversed(m_hMotion, axis, id, ref flag,   ref pErrCode);
        //    }
        //    return false;
        //}

        //public bool SetSioValue(ushort axis, int id, double value, ref int pErrCode)
        //{
        //    if (m_hMotion != IntPtr.Zero)
        //    {
        //        return uniMotionSetSioValue(m_hMotion, axis, id, value, ref pErrCode);
        //    }
        //    return false;
        //}

        //public bool GetSioValue(ushort axis, int id, ref double value, ref int pErrCode)
        //{
        //    if (m_hMotion != IntPtr.Zero)
        //    {
        //        return uniMotionGetSioValue(m_hMotion, axis, id, ref value, ref pErrCode);
        //    }
        //    return false;
        //}

        public bool EnableSio(ushort axis, int id, bool flag, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionEnableSio(m_hMotion, axis, id, flag, ref pErrCode);
            }
            return false;
        }

        public bool IsSioEnabled(ushort axis, int id, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionIsSioEnabled(m_hMotion, axis, id, ref pErrCode);
            }
            return false;
        }

        public bool GetSioLogic(ushort axis, int id, ref int level, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetSioLogic(m_hMotion, axis, id, ref level, ref pErrCode);
            }
            return false;
        }

        public bool SetSioLogic(ushort axis, int id, int level, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionSetSioLogic(m_hMotion, axis, id, level, ref pErrCode);
            }
            return false;
        }

        //// 获取轴辅助状态,obsolete
        //public bool GetAuxState(ushort axis, ref int state, ref int pErrCode)
        //{
        //    if (m_hMotion != IntPtr.Zero)
        //    {
        //        return uniMotionGetAuxState(m_hMotion, axis, ref state, ref pErrCode);
        //    }
        //    return false;
        //}
        //// 获取轴辅助状态
        //public bool GetAxisAuxState(ushort axis, ref int state, ref int pErrCode)
        //{
        //    if (m_hMotion != IntPtr.Zero)
        //    {
        //        return uniMotionGetAuxState(m_hMotion, axis, ref state, ref pErrCode);
        //    }
        //    return false;
        //}

        // 获取版本号
        public string GetVersion()
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return Marshal.PtrToStringAnsi(uniMotionGetVersion(m_hMotion));
            }
            return null;
        }

        // 获取厂商描述
        public string GetVendorDesc()
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return Marshal.PtrToStringAnsi(uniMotionGetVendorDesc(m_hMotion));
            }
            return null;
        }

        // 获取错误描述
        public string GetErrorDesc(int errCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                IntPtr hstr = uniMotionGetErrorDesc(m_hMotion, errCode);

                if (hstr != IntPtr.Zero)
                {
                    return Marshal.PtrToStringAnsi(hstr);
                }

            }
            return string.Empty;
        }
        public static string GetErrorDesc(string strModel, int errCode)
        {
            //if (m_hMotion != IntPtr.Zero)
            {
                IntPtr hstr = uniMotionGetErrorDescE1(strModel, errCode);

                if (hstr != IntPtr.Zero)
                {
                    return Marshal.PtrToStringAnsi(hstr);
                }

            }
            return string.Empty;
        }

        // 获取序列号
        public string GetSerialNum()
        {
            if (m_hMotion != IntPtr.Zero)
            {
                StringBuilder sb = new StringBuilder(1204);

                uniMotionGetSerialNum(m_hMotion, sb, 1204);

                return sb.ToString();
            }
            return null;
        }

        public string GetModelName()
        {
            if (m_hMotion != IntPtr.Zero)
            {
                IntPtr hName = uniMotionGetModelName(m_hMotion);

                if (hName != IntPtr.Zero)
                {
                    return Marshal.PtrToStringAnsi(hName);
                }
            }
            return null;
        }


        public bool IsSioValid(ushort axis, int id)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionIsSioValid(m_hMotion, axis, id);
            }

            return false;
        }

        public string GetAxisStateDesc(int state)
        {
            string str = "";

            if ((state & ALM_IN_RUN) == ALM_IN_RUN)
            {
                str = "Running";
            }
            else if (state == 0)
            {
                str = "Normal";
            }
            else if (state == ALM_ENA_SRV)
            {
                str = "Normal";
            }
            else
            {
                str = "Warning: ";
                if ((state & ALM_POS_LMT) == ALM_POS_LMT)
                {
                    str += "positive limit encountered, ";

                }
                if ((state & ALM_NEG_LMT) == ALM_NEG_LMT)
                {
                    str += "negative limit encountered, ";
                }
                if ((state & ALM_DRV_ALM) == ALM_DRV_ALM)
                {
                    str += "driver error occurred, ";
                }
                //if ((state & ALM_ENA_SRV) == ALM_ENA_SRV)
                //{
                //    str += "servo enable error, ";
                //}
                if ((state & ALM_EXC_TOL) == ALM_EXC_TOL)
                {
                    str += "tolerance exceed, ";
                }
                if ((state & ALM_EM_STP) == ALM_EM_STP)
                {
                    str += "in emergence stop, ";
                }
                if ((state & ALM_SM_STP) == ALM_SM_STP)
                {
                    str += "in normal stop, ";
                }
                if ((state & ALM_SM_STP) == ALM_SM_STP)
                {
                    str += "in normal stop, ";
                }
            }

            return str;
        }

        public static bool IsOutputFio(int type)
        {
            //if (type == KMotion.FI_EMERGENCE) return false;
            if (type == KMotion.FI_RESET) return false;
            if (type == KMotion.FI_START) return false;
            if (type == KMotion.FI_STOP) return false;
            if (type == KMotion.FI_PAUSE) return false;
            if (type == KMotion.FO_LAMP_GREEN) return true;
            if (type == KMotion.FO_LAMP_YELLOW) return true;
            if (type == KMotion.FO_LAMP_RED) return true;
            if (type == KMotion.FO_BEEP) return true;
            if (type == KMotion.FO_TOOL) return true;

            return false;
        }
        public static bool IsOutputFio(string strDesc)
        {
            if (string.Compare(strDesc, "Emergence", true) == 0) return false;
            if (string.Compare(strDesc, "Reset System", true) == 0) return false;
            if (string.Compare(strDesc, "Start KRobot", true) == 0) return false;
            if (string.Compare(strDesc, "Stop KRobot", true) == 0) return false;
            if (string.Compare(strDesc, "Pause KRobot", true) == 0) return false;
            if (string.Compare(strDesc, "Green Lamp", true) == 0) return true;
            if (string.Compare(strDesc, "Yellow Lamp", true) == 0) return true;
            if (string.Compare(strDesc, "Red Lamp", true) == 0) return true;
            if (string.Compare(strDesc, "Beep", true) == 0) return true;
            if (string.Compare(strDesc, "Tool", true) == 0) return true;

            return false;
        }
        public static string GetFioDesc(int type)
        {
            // if (type == KMotion.FI_EMERGENCE) return "Emergence";
            if (type == KMotion.FI_RESET) return "Reset System";
            if (type == KMotion.FI_START) return "Start KRobot";
            if (type == KMotion.FI_STOP) return "Stop KRobot";
            if (type == KMotion.FI_PAUSE) return "Pause KRobot";
            if (type == KMotion.FO_LAMP_GREEN) return "Green Lamp";
            if (type == KMotion.FO_LAMP_YELLOW) return "Yellow Lamp";
            if (type == KMotion.FO_LAMP_RED) return "Red Lamp";
            if (type == KMotion.FO_BEEP) return "Beep";
            if (type == KMotion.FO_TOOL) return "Tool";

            return "";
        }
        public static int GetFioType(string strDesc)
        {
            //if (string.Compare(strDesc, "Emergence", true) == 0) return KMotion.FI_EMERGENCE;
            if (string.Compare(strDesc, "Reset System", true) == 0) return KMotion.FI_RESET;
            if (string.Compare(strDesc, "Start KRobot", true) == 0) return KMotion.FI_START;
            if (string.Compare(strDesc, "Stop KRobot", true) == 0) return KMotion.FI_STOP;
            if (string.Compare(strDesc, "Pause KRobot", true) == 0) return KMotion.FI_PAUSE;
            if (string.Compare(strDesc, "Green Lamp", true) == 0) return KMotion.FO_LAMP_GREEN;
            if (string.Compare(strDesc, "Yellow Lamp", true) == 0) return KMotion.FO_LAMP_YELLOW;
            if (string.Compare(strDesc, "Red Lamp", true) == 0) return KMotion.FO_LAMP_RED;
            if (string.Compare(strDesc, "Beep", true) == 0) return KMotion.FO_BEEP;
            if (string.Compare(strDesc, "Tool", true) == 0) return KMotion.FO_TOOL;

            return 0;
        }

        public bool WaitHome(ushort axis, int timeout, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionWaitHomeAxis(m_hMotion, axis, timeout, ref pErrCode);

            }
            return false;
        }
        public bool WaitMove(ushort axis, int timeout, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionWaitMoveAxis(m_hMotion, axis, timeout, ref pErrCode);

            }
            return false;
        }

        public bool WaitMany(KMaxis axes, int timeout, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionWaitMany(m_hMotion, axes.GetHandle(), timeout, ref pErrCode);

            }
            return false;
        }

        public bool WaitCrd(uint csid, KMaxis axes, int timeout, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionWaitCrd(m_hMotion, csid, axes.GetHandle(), timeout, ref pErrCode);

            }
            return false;
        }

        public bool ConfigHome(ushort axis, KMoptions options, bool bReadOnly, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionConfigOrigin(m_hMotion, axis, options.GetHandle(), bReadOnly, ref pErrCode);
            }
            return false;
        }

        public bool ConfigHardLimit(ushort axis, int sioId, KMoptions options, bool bReadOnly, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionConfigHardLimit(m_hMotion, axis, sioId, options.GetHandle(), bReadOnly, ref pErrCode);
            }
            return false;
        }

        public bool ConfigSoftLimit(ushort axis, int sioId, KMoptions options, bool bReadOnly, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionConfigSoftLimit(m_hMotion, axis, sioId, options.GetHandle(), bReadOnly, ref pErrCode);
            }
            return false;
        }

        public bool ConfigEmergence(ushort axis, KMoptions options, bool bReadOnly, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionConfigEmergence(m_hMotion, axis, options.GetHandle(), bReadOnly, ref pErrCode);
            }
            return false;
        }

        public bool ConfigServo(ushort axis, KMoptions options, bool bReadOnly, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionConfigServo(m_hMotion, axis, options.GetHandle(), bReadOnly, ref pErrCode);
            }
            return false;
        }

        public bool ConfigEtherCAT(ushort axis, KMoptions options, bool bReadOnly, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionConfigEtherCAT(m_hMotion, axis, options.GetHandle(), bReadOnly, ref pErrCode);
            }
            return false;
        }
        public bool ConfigCounter(ushort axis, KMoptions options, bool bReadOnly, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionConfigCounter(m_hMotion, axis, options.GetHandle(), bReadOnly, ref pErrCode);
            }
            return false;
        }
        //刹车
        public bool AddBrake(ushort axis, int bit, int logic, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionAddBrake(m_hMotion, axis, bit, logic, ref pErrCode);
            }
            return false;
        }
        public void RemoveBrake(ushort axis)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                uniMotionRemoveBrake(m_hMotion, axis);
            }

        }
        public RvBool IsSioValid(ushort axis, int id, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionIsSioValid(m_hMotion, axis, id, ref pErrCode);
            }
            return RvBool.Fuzzy;
        }

        public bool CheckHomeType(ushort axis, IntPtr hOption, ref int type)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionCheckHomeType(m_hMotion, axis, hOption, ref type);
            }
            return false;
        }

        public AXIS_MECHNIC GetAxisMachType(ushort axis)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return (AXIS_MECHNIC)uniMotionGetAxisMachType(m_hMotion, axis);
            }
            return AXIS_MECHNIC.UNKNOWN;
        }

        public void SetAxisMachType(ushort axis, AXIS_MECHNIC type)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                uniMotionSetAxisMachType(m_hMotion, axis, (int)type);
            }
        }

        public AXIS_DRIVER GetMotorDriverType(ushort axis)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return (AXIS_DRIVER)uniMotionGetMotorDriverType(m_hMotion, axis);
            }
            return AXIS_DRIVER.UNKNOWN;
        }

        public void SetMotorDriverType(ushort axis, AXIS_DRIVER type)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                uniMotionSetMotorDriverType(m_hMotion, axis, (int)type);
            }
        }

        public RvBool IsAxisVertical(ushort axis)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return (RvBool)uniMotionIsAxisVertical(m_hMotion, axis);
            }
            return RvBool.Fuzzy;
        }

        public void SetAxisVertical(ushort axis, bool flag)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                uniMotionSetAxisVertical(m_hMotion, axis, flag);
            }
        }


        public bool IsDeviceReady()
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionIsDeviceReady(m_hMotion);
            }

            return false;
        }
        //pLastCode： 返回运动卡轴的错误代码
        public int GetAxisCurState(ushort axis, ref int pLastCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetAxisCurState(m_hMotion, axis, ref pLastCode);
            }

            return KRobot.INVALID_CODE;
        }
        //返回运动卡轴的错误代码
        public int GetAxisAbnormalCode(ushort axis)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetAxisAbnormalCode(m_hMotion, axis);
            }

            return KRobot.INVALID_CODE;
        }

        public string GetStateDesc(int state)
        {
            IntPtr ptr = uniMotionGetStateDesc(state);

            if (ptr != IntPtr.Zero)
            {
                return Marshal.PtrToStringAnsi(ptr);
            }

            return null;
        }

        public bool OpenPwmChannel(int channel, int hiDura, int lowDura, ref int errCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionOpenPwmChannel(m_hMotion, channel, hiDura, lowDura, ref errCode);
            }

            return false;
        }

        public bool ClosePwmChannel(int channel, ref int errCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionClosePwmChannel(m_hMotion, channel, ref errCode);
            }

            return false;
        }

        public bool IsPwmChannelOpened(int channel, ref int errCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionIsPwmChannelOpened(m_hMotion, channel, ref errCode);
            }

            return false;
        }



        public bool ConfigPwm(int channel, KMoptions options, bool bReadOnly, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionConfigPwm(m_hMotion, channel, options.GetHandle(), bReadOnly, ref pErrCode);
            }

            return false;
        }

        public bool SetPwmOutput(int channel, int level, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionSetPwmOutput(m_hMotion, channel, level, ref pErrCode);
            }

            return false;
        }

        public bool EnableSoftLimit(ushort axis, int id, bool flag, ref int errCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionEnableSoftLimit(m_hMotion, axis, id, flag, ref errCode);
            }

            return false;
        }

        public bool IsSoftLimitEnabled(ushort axis, int id, bool flag, ref int errCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionIsSoftLimitEnabled(m_hMotion, axis, id, ref errCode);
            }

            return false;
        }

        public bool IsSoftLimitValid(ushort axis, int id, bool flag, ref int errCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionIsSoftLimitValid(m_hMotion, axis, id, ref errCode);
            }

            return false;
        }


        public int SetSoftLimit(ushort axis, double lower, double upper, ref int errCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionSetSoftLimit(m_hMotion, axis, lower, upper, ref errCode);
            }

            return 0;
        }

        public int GetSoftLimit(ushort axis, ref double lower, ref double upper, ref int errCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetSoftLimit(m_hMotion, axis, ref lower, ref upper, ref errCode);
            }

            return 0;
        }



        public bool SetSoftLimit(ushort axis, int id, double pos, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionSetSoftLimitE1(m_hMotion, axis, id, pos, ref pErrCode);
            }

            return false;
        }

        public bool GetSoftLimit(ushort axis, int id, ref double pos, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionGetSoftLimitE1(m_hMotion, axis, id, ref pos, ref pErrCode);
            }

            return false;
        }


        public bool MoveMany2(ushort axisX, ushort axisY, double x, double y, KMoptions options, bool bAbsolute, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionMoveMany2(m_hMotion, axisX, axisY, x, y, options.GetHandle(), bAbsolute, ref pErrCode);
            }

            return false;
        }

        public bool StopMany2(ushort axisX, ushort axisY, KMoptions options, ref int pErrCode)
        {
            if (m_hMotion != IntPtr.Zero)
            {
                return uniMotionStopMany2(m_hMotion, axisX, axisY, options.GetHandle(), ref pErrCode);
            }

            return false;
        }



    }

}