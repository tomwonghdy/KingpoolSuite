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
    //robot, action id,  options ,  notify wnd, userdata
    //返回0表示OK, <0 表示错误返回
    // typedef int ( *UNI_ACTION_IMPLEMENT_FUNC)(IntPtr, UINT, IntPtr , HWND, void*);
    public delegate int CustomActionImplementDelegate(IntPtr hRobot, uint actId, IntPtr hOptions, IntPtr hNotifyWnd, IntPtr pUserData);

    public delegate bool SupplementImplementDelegate(uint actId, IntPtr hRobot, IntPtr hOptions, IntPtr hUserParams, ref int pErrCode);

    public struct SuppleTriple
    {
        public IntPtr pUserParams;
        public SupplementImplementDelegate afterPausedDelegate;
        public SupplementImplementDelegate beforeContinueDelegate;
    }

    //触发类型
    public enum TRIGGER_TYPE
    {
        RISING = 1,
        FAILING = (1 << 1),
        HIGH = (1 << 2),
        LOW = (1 << 3),
    }

    public enum ACTION_PRIORITY
    {
        ERROR = -1,
        IDLE = 0,
        LOWEST = 1,
        BELOW_NORMAL = 2,
        NORMAL = 3,
        ABOVE_NORMAL = 4,
        HIGHEST = 5,
        TIME_CRITICAL = 6,
    }

    //动作状态
    public enum ACTION_STATE
    {
        UNKNOWN = -1,
        NORMAL = 0,
        EXCEPT = 1,   //例外处理
        CANCEL = 2,   //取消, 不停止线程
        ABORT = 3,   //终止，停止线程时发生
        DEAD = 4,   //终止后，线程退出前发生
    };

    public enum SAFETY_TYPE
    {
        ST_GENERIC = 0,
        ST_ACTION = 1,
    };

    public class KRobot
    {
        //消息
        public const int WM_USER_MSG = 0x0400;
        public const int ON_TRIGGER_OCCUR = (WM_USER_MSG + 1305);
        public const int ON_EMERGENCE_OCCUR = (WM_USER_MSG + 1306);
        public const int ON_ERROR_OCCUR = (WM_USER_MSG + 1307);
        public const int ON_SAFETY_INPUT = (WM_USER_MSG + 1308);
        public const int ON_EXCEPTION_OCCUR = (WM_USER_MSG + 1309);
        public const int ON_FUNCIN_OCCUR = (WM_USER_MSG + 1310);

        //机器场景(内置, 用户自定义的ID不能冲突)
        public const int ST_ANY = (-1);
        public const int ST_HOME = 0;
        public const int ST_EDIT = 1;
        public const int ST_JOG = 2;
        public const int ST_ESTOP = 3;    //急停
        public const int ST_MORE = 7;    //用户自定义

        //无限等待
        public const int WAIT_INFINITE = 0x7FFFFFFF;
        public const int INVALID_NUM = 0x7FFFFFFF;
        public const int INVALID_ID = 0x7FFFFFFF;
        public const ushort INVALID_WID = 0xFFFF;
        public const int INVALID_CODE = -0x7FFFFFFF;

        //机器状态
        public const int MS_UNKNOWN = (-1);
        public const int MS_STANDBY = 0;  //待机
        public const int MS_MOVING = 1;  //正在运行
        public const int MS_PAUSED = 2;  //暂停, 
        public const int MS_BROKEN = 3;  //故障发生 
        public const int MS_EMERGENCE = 4;  //急停

        //作业活动
        public const int RS_DUMMY = (0);      //仅进入启动状态
        public const int RS_ACTION = (1);     //动作
        public const int RS_EMERG = (1 << 1);     //急停
        public const int RS_SAFETY = (1 << 2);    //安全
        public const int RS_TRIGGER = (1 << 3);   //触发
        public const int RS_FUNCIN = (1 << 4);    //专用输入
        public const int RS_ALARM = (1 << 5);    //报警灯输出

        //报警灯 
        public const int AL_ALL_OFF = 0;
        public const int AL_GREEN = 1;
        public const int AL_YELLOW = (1 << 1);
        public const int AL_RED = (1 << 2);
        public const int AL_ALL_ON = (AL_GREEN | AL_YELLOW | AL_RED);

        //等待状标记选项
        public const int WS_PAUSE = 1;
        public const int WS_EMERG = (1 << 1);
        public const int WS_BROKEN = (1 << 2);

        //错误码
        public const int ROB_OK = 0;
        public const int ROB_FAILED = (-1);
        public const int ERR_INVALID_OBJ = (-10084);
        public const int ERR_INVALID_HANDLE = ERR_INVALID_OBJ;
        public const int ERR_CAMERA_FAILURE = (-10085);
        public const int ERR_BARCODE_NOT_FIND = (-10086);
        public const int ERR_BARCODE_READ = (-10087);
        public const int ERR_MARK_NOT_FIND = (-10088);
        public const int ERR_SHOT_TIMEOUT = (-10089);
        public const int ERR_IN_EMERGENCE = (-10090);
        public const int ERR_REENTRY_CONFLICT = (-10091);
        public const int ERR_SYSERR = (-10092);
        public const int ERR_NOT_ENOUGH = (-10093);
        public const int ERR_ILLEGAL_USE = (-10094);
        public const int ERR_INVALID_AXIS = (-10095);
        public const int ERR_UNSUPPORTED = (-10096);
        public const int ERR_OUT_OF_RANGE = (-10097);
        public const int ERR_NOT_READY = (-10098);
        public const int ERR_INPUT_TIME_OUT = (-10099);
        public const int ERR_TIME_OUT = ERR_INPUT_TIME_OUT;
        public const int ERR_INVALID_ARGS = (-10100);
        public const int ERR_OPERTAION_ABORT = (-10101);
        public const int ERR_NOT_IN_STATE = (-10102);
        public const int ERR_OPERTAION_CANCEL = (-10103);
        public const int ERR_NOT_RUNNING = (-10105);
        public const int ERR_NOT_ACTIVE = (-10106);

        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniCreateRobot();
        [DllImport("UniDevice.dll")]
        private static extern void uniDestroyRobot(IntPtr hRobot);
        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniRobotGetMotionName(IntPtr hRobot);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotStart(IntPtr hRobot, int activity, IntPtr hNotifyWnd, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern void uniRobotSetNotifyWnd(IntPtr hRobot, IntPtr hNotifyWnd);
        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniRobotGetNotifyWnd(IntPtr hRobot);

        [DllImport("UniDevice.dll")]
        private static extern void uniRobotStop(IntPtr hRobot);

        [DllImport("UniDevice.dll")]
        private static extern void uniRobotStopEx(IntPtr hRobot, bool bEmergStop);


        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotDelay(IntPtr hRobot, uint actId, int duration, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern int uniRobotGetCurState(IntPtr hRobot);

        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniRobotGetErrorDesc(IntPtr hRobot, int errCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGetCurPos(IntPtr hRobot, ref double pX, ref double pY, ref double pZ, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGetCurPosEx1(IntPtr hRobot, ref double pX, ref double pYorY1, ref double pZ, ref double pRorY2, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGetCurPosEx2(IntPtr hRobot, ushort axis, ref double pPos, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGetCurSpeed(IntPtr hRobot, ref double pX, ref double pY, ref double pZ, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGetCurSpeedEx1(IntPtr hRobot, ref double pX, ref double pYorY1, ref double pZ, ref double pRorY2, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGetCurSpeedEx2(IntPtr hRobot, ushort axis, ref double pSpeed, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotSetOutputLevel(IntPtr hRobot, int bit, int level, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGetOutputLevel(IntPtr hRobot, int bit, ref int level, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGetInputLevel(IntPtr hRobot, int bit, ref int level, ref int pErrCode);

        //FIO
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotAddFio(IntPtr hMotion, int id, int bit, int initLevel, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGetFioLevel(IntPtr hMotion, int id, ref int level, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotSetFioLevel(IntPtr hMotion, int id, int level, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern int uniRobotGetFioCount(IntPtr hMotion);
        [DllImport("UniDevice.dll")]
        private static extern void uniRobotRemoveAllFios(IntPtr hMotion);
        [DllImport("UniDevice.dll")]
        private static extern void uniRobotRemoveFio(IntPtr hMotion, int id);
        [DllImport("UniDevice.dll")]
        private static extern int uniRobotGetFioBit(IntPtr hMotion, int id);
        [DllImport("UniDevice.dll")]
        private static extern int uniRobotGetFioId(IntPtr hMotion, int bit);
        [DllImport("UniDevice.dll")]
        private static extern int uniRobotGetFioIdAt(IntPtr hMotion, int index);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotIsFioInputAt(IntPtr hMotion, int index);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotIsFioOutputAt(IntPtr hMotion, int index);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotIsFioInput(IntPtr hMotion, int id);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotIsFioOutput(IntPtr hMotion, int id);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotSetActionOption(IntPtr hRobot, uint actId, string strName, int nValue);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotSetActionOptionE1(IntPtr hRobot, uint actId, string strName, bool bValue);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotSetActionOptionE2(IntPtr hRobot, uint actId, string strName, double nValue);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotSetActionOptionE3(IntPtr hRobot, uint actId, string strName, string strValue);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGetActionOption(IntPtr hRobot, uint actId, string strName, out int nValue);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGetActionOptionE1(IntPtr hRobot, uint actId, string strName, out bool bValue);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGetActionOptionE2(IntPtr hRobot, uint actId, string strName, out double nValue);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGetActionOptionE3(IntPtr hRobot, uint actId, string strName, StringBuilder strValue, int size);



        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGoHome(IntPtr hRobot, uint actId, ushort axis, IntPtr hOptions, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotMoveTo(IntPtr hRobot, uint actId, ushort axis, double pos, bool bAbsolute, IntPtr hOptions, ref int pErrCode);
        [DllImport("UniDevice.dll")]//多轴
        private static extern bool uniRobotGoHomeMany(IntPtr hRobot, uint actId, IntPtr pAxisArray, int count, IntPtr hOptions, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotMoveManyTo(IntPtr hRobot, uint actId, IntPtr pAxisArray, IntPtr pPosArray, int count, bool bAbsolute, IntPtr hOptions, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotMoveManyToE1(IntPtr hRobot, uint actId, IntPtr pAxisArray, IntPtr pPosArray, IntPtr pOptArray, int count, bool bAbsolute, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotLineAnyTo(IntPtr hRobot, uint actId, uint csid, IntPtr pAxisArray, IntPtr pPosArray, int count, bool bAbsolute, IntPtr hOptions, ref int pErrCode); //插补
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotLineTo(IntPtr hRobot, uint actId, uint csid, ushort axisX, ushort axisY, double x, double y, bool bAbsolute, IntPtr hOptions, ref int pErrCode); //插补
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotArcTo(IntPtr hRobot, uint actId, uint csid, ushort axisX, ushort axisY, double cx, double cy, double ex, double ey, int dir, IntPtr hOptions, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGo(IntPtr hRobot, uint actId, IntPtr hQueue, IntPtr hOptions, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGoHomeEx(IntPtr hRobot, uint actId, ushort axis, IntPtr hOptions, SuppleTriple suppleTriple, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotMoveToEx(IntPtr hRobot, uint actId, ushort axis, double pos, bool bAbsolute, IntPtr hOptions, SuppleTriple suppleTriple, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGoHomeManyEx(IntPtr hRobot, uint actId, IntPtr pAxisArray, int count, IntPtr hOptions, SuppleTriple suppleTriple, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotMoveManyToEx(IntPtr hRobot, uint actId, IntPtr pAxisArray, IntPtr pPosArray, int count, bool bAbsolute, IntPtr hOptions, SuppleTriple suppleTriple, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotMoveManyToExE1(IntPtr hRobot, uint actId, IntPtr pAxisArray, IntPtr pPosArray, IntPtr hOptArray, int count, bool bAbsolute, SuppleTriple suppleTriple, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotLineAnyToEx(IntPtr hRobot, uint actId, uint csid, IntPtr pAxisArray, IntPtr pPosArray, int count, bool bAbsolute, IntPtr hOptions, SuppleTriple suppleTriple, ref int pErrCode); //插补
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotLineToEx(IntPtr hRobot, uint actId, uint csid, ushort axisX, ushort axisY, double x, double y, bool bAbsolute, IntPtr hOptions, SuppleTriple suppleTriple, ref int pErrCode);        //插补
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotArcToEx(IntPtr hRobot, uint actId, uint csid, ushort axisX, ushort axisY, RvPointF64 center, RvPointF64 end, int dir, IntPtr hOptions, SuppleTriple suppleTriple, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGoEx(IntPtr hRobot, uint actId, IntPtr hQueue, IntPtr hOptions, SuppleTriple suppleTriple, ref int pErrCode);


        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotWaitInput(IntPtr hRobot, IntPtr pIn, int nCheckFlags, int timeout, ref int pErrCode);

        //[DllImport("UniDevice.dll")]
        //private static extern bool uniRobotWaitAxis(IntPtr hRobot, uint actId, ushort axis, bool bGoHome, IntPtr hOptions, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotWaitMove(IntPtr hRobot, uint actId, ushort axis, int nCheckFlags, IntPtr hOptions, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotWaitHome(IntPtr hRobot, uint actId, ushort axis, int nCheckFlags, IntPtr hOptions, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotWaitLine(IntPtr hRobot, uint actId, uint csid, ushort axisX, ushort axisY, int nCheckFlags, IntPtr hOptions, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotWaitLine3d(IntPtr hRobot, uint actId, uint csid, ushort axisX, ushort axisY, ushort axisZ, int nCheckFlags, IntPtr hOptions, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotWaitLine4d(IntPtr hRobot, uint actId, uint csid, ushort axisX, ushort axisY, ushort axisZ, ushort axisR, int nCheckFlags, IntPtr hOptions, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotWaitLineXd(IntPtr hRobot, uint actId, uint csid, IntPtr hAxes, int nCheckFlags, IntPtr hOptions, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotWaitCrd(IntPtr hRobot, uint actId, uint csid, IntPtr hAxes, int nCheckFlags, IntPtr hOptions, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotWaitQueue(IntPtr hRobot, uint actId, IntPtr hQueue, int nCheckFlags, IntPtr hOptions, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotHaltMovingAxis(IntPtr hRobot, ushort axis, IntPtr option, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotHaltHomingAxis(IntPtr hRobot, ushort axis, IntPtr option, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotIsActionActive(IntPtr hRobot, uint actId);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotActivateAction(IntPtr hRobot, uint actId, int sceneId, bool bSynch, IntPtr hNotifyWnd, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotActivateActionE1(IntPtr hRobot, IntPtr hAction, IntPtr hOptions, bool bSynch, IntPtr hNotifyWnd, ref int intpErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotActivateActionE2(IntPtr hRobot, IntPtr hAction, IntPtr hOptions, bool bSynch, IntPtr hNotifyWnd, ref uint pActId, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotActivateActionE3(IntPtr hRobot, uint actId, IntPtr hOptions, bool bSynch, IntPtr hNotifyWnd, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotActivateActionMove(IntPtr hRobot, uint actId, int scene, double pos, bool bSynch, IntPtr hNotifyWnd, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotActivateActionMoveXY(IntPtr hRobot, uint actId, int scene, double x, double y, bool bSynch, IntPtr hNotifyWnd, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotActivateActionLineToXY(IntPtr hRobot, uint actId, int scene, double x, double y, bool bSynch, IntPtr hNotifyWnd, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern void uniRobotTerminateAction(IntPtr hRobot, uint actId);
        [DllImport("UniDevice.dll")]
        private static extern void uniRobotTerminateAllActions(IntPtr hRobot);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotIsActionAbort(IntPtr hRobot, uint actId);
        [DllImport("UniDevice.dll")]
        private static extern int uniRobotGetActivatedActionCount(IntPtr hRobot);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotIsAnyActionActivated(IntPtr hRobot);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotIsAnyActionActive(IntPtr hRobot);
        [DllImport("UniDevice.dll")]
        private static extern void uniRobotCancelAction(IntPtr hRobot, uint actId);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotIsActionCanceled(IntPtr hRobot, uint actId);
        [DllImport("UniDevice.dll")]
        private static extern void uniRobotSetActionNormal(IntPtr hRobot, uint actId);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotIsActionDead(IntPtr hRobot, uint actId);       //动作已经dead
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotIsActionActivated(IntPtr hRobot, uint actId);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotAddEmerg(IntPtr hRobot, int bit, bool bReverse, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern int uniRobotGetEmergCount(IntPtr hRobot);

        [DllImport("UniDevice.dll")]
        private static extern void uniRobotRemoveEmerg(IntPtr hRobot, int bit);

        [DllImport("UniDevice.dll")]
        private static extern void uniRobotRemoveAllEmergs(IntPtr hRobot);

        [DllImport("UniDevice.dll")]
        private static extern int uniRobotGetTriggerCount(IntPtr hRobot);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotAddTrigger(IntPtr hRobot, int bit, int type, int nInitLevel, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern void uniRobotRemoveTrigger(IntPtr hRobot, int bit);

        [DllImport("UniDevice.dll")]
        private static extern void uniRobotRemoveAllTriggers(IntPtr hRobot);

        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniRobotAddScene(IntPtr hRobot, int scene, double speed, double acc, double dec, bool bUpdateIfExist, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern int uniRobotGetSceneCount(IntPtr hRobot);

        [DllImport("UniDevice.dll")]
        private static extern void uniRobotRemoveScene(IntPtr hRobot, int sceneId);

        [DllImport("UniDevice.dll")]
        private static extern void uniRobotRemoveAllScenes(IntPtr hRobot);
        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniRobotGetOptionsAt(IntPtr hRobot, int index);
        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniRobotGetSceneOptions(IntPtr hRobot, int id);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotAddAction(IntPtr hRobot, IntPtr action, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern int uniRobotGetActionCount(IntPtr hRobot);

        [DllImport("UniDevice.dll")]
        private static extern void uniRobotRemoveAction(IntPtr hRobot, uint actId);
        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniRobotGetActionAt(IntPtr hRobot, int index);

        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniRobotFindAction(IntPtr hRobot, uint id);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotIsTriggerOn(IntPtr hRobot);


        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotSwitchTrigger(IntPtr hRobot, bool bOn, IntPtr hNotifyWnd);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotIsEmergenceOn(IntPtr hRobot);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotSwitchEmergence(IntPtr hRobot, bool bOn, IntPtr hNotifyWnd);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotIsFuncInputOn(IntPtr hRobot);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotSwitchFuncInput(IntPtr hRobot, bool bOn, IntPtr hNotifyWnd);
        [DllImport("UniDevice.dll")]
        private static extern void uniRobotAttachMotion(IntPtr hRobot, IntPtr pMotion);

        [DllImport("UniDevice.dll")]
        private static extern void uniRobotDetachMotion(IntPtr hRobot);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotEnterEmergState(IntPtr hRobot, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotClearEmergState(IntPtr hRobot, ref int pErrCode);



        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniRobotGetMotion(IntPtr hRobot);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotPause(IntPtr hRobot, ref int errCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotContinue(IntPtr hRobot, ref int errCode);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotIsPaused(IntPtr hRobot);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotIsRunning(IntPtr hRobot);


        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotReset(IntPtr hRobot, ref int errCode);

        [DllImport("UniDevice.dll")]
        private static extern void uniRobotRaise(IntPtr hRobot);

        [DllImport("UniDevice.dll")]
        private static extern void uniRobotFlat(IntPtr hRobot);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotIsRequested(IntPtr hRobot);

        [DllImport("UniDevice.dll")]
        private static extern bool uniActionHome(IntPtr hRobot, int scene, ushort axis, IntPtr hNotifyWnd);

        [DllImport("UniDevice.dll")]
        private static extern bool uniActionHomeXY(IntPtr hRobot, int scene, int axisX, int axisY, IntPtr hNotifyWnd);

        [DllImport("UniDevice.dll")]
        private static extern bool uniActionHomeZXY(IntPtr hRobot, int scene, int axisX, int axisY, int axisZ, IntPtr hNotifyWnd);


        [DllImport("UniDevice.dll")]
        private static extern bool uniActionMove(IntPtr hRobot, int scene, ushort axis, double pos, bool bAbsolute, IntPtr hNotifyWnd);

        [DllImport("UniDevice.dll")]
        private static extern bool uniActionMoveXY(IntPtr hRobot, int scene, int axisX, int axisY, double posX, double posY, bool bAbsolute, IntPtr hNotifyWnd);

        [DllImport("UniDevice.dll")]
        private static extern bool uniActionMoveZXY(IntPtr hRobot, int scene, int axisX, int axisY, int axisZ, double posX, double posY, double posZ, bool bAbsolute, IntPtr hNotifyWnd);
        [DllImport("UniDevice.dll")]
        private static extern bool uniActionMoveXYZ(IntPtr hRobot, int scene, int axisX, int axisY, int axisZ, double posX, double posY, double posZ, bool bAbsolute, double initZ, IntPtr hNotifyWnd);
        [DllImport("UniDevice.dll")]
        private static extern bool uniActionRepeatMove(IntPtr hRobot, int scene, int axisX, int axisY, double initX, double initY, double posX, double posY, bool bSingleMove, IntPtr hNotifyWnd);
        [DllImport("UniDevice.dll")]
        private static extern bool uniActionMoveInPulse(IntPtr hRobot, int scene, ushort axis, int pulses, IntPtr hNotifyWnd);

        [DllImport("UniDevice.dll")]
        private static extern bool uniActionSequence2(IntPtr hRobot, int scene, int axis1, int axis2, double pos1, double pos2, bool bAbsolute, IntPtr hNotifyWnd);

        [DllImport("UniDevice.dll")]
        private static extern bool uniActionSequence3(IntPtr hRobot, int scene, int axis1, int axis2, int axis3, double pos1, double pos2, double pos3, bool bAbsolute, IntPtr hNotifyWnd);


        [DllImport("UniDevice.dll")]
        private static extern void uniActionStop(IntPtr hRobot);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotIsCanceled(IntPtr hRobot);

        [DllImport("UniDevice.dll")]
        private static extern bool uniActionCustom(IntPtr hRobot, uint actId, CustomActionImplementDelegate caiDelegate, IntPtr pUserData, IntPtr hNotifyWnd);

        [DllImport("UniDevice.dll")]
        private static extern bool uniActionBeginMulti(IntPtr hRobot);

        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniAddAction(IntPtr hRobot, uint actId);

        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniAddActionE1(IntPtr hRobot, uint actId, CustomActionImplementDelegate pfnActionImplement, IntPtr pUserData);

        [DllImport("UniDevice.dll")]
        private static extern void uniActionSetImplementFunc(IntPtr hAction, CustomActionImplementDelegate pfnActionImplement, IntPtr pUserData);
        //control units: camera, motion ,screw drivers, barcode readers etc
        [DllImport("UniDevice.dll")]
        private static extern int uniActionSetControlUnits(IntPtr hAction, IntPtr pUnitsArray, int unitsCount);
        [DllImport("UniDevice.dll")]
        private static extern bool uniActionEndMulti(IntPtr hRobot, IntPtr hNotifyWnd);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotEnterStartState(IntPtr hRobot, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern void uniRobotLeaveStartState(IntPtr hRobot);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotRaiseException(IntPtr hRobot, uint actId, int exId, IntPtr pExceptData, int size, ref int pErrCode);
        [DllImport("UniDevice.dll")]

        private static extern void uniRobotTreatException(IntPtr hRobot, uint actId, int exId, bool bAbort, IntPtr pExceptData, int size);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotWaitExceptionConfirm(IntPtr hRobot, uint actId, int exId, int timeOut, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGetExceptionData(IntPtr hRobot, uint actId, int exId, IntPtr pExceptData, int size);


        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotAddSafety(IntPtr hRobot, int type, int bit, bool bReverse, ref int errCode);
        [DllImport("UniDevice.dll")]
        private static extern int uniRobotGetSafetyCount(IntPtr hRobot);
        [DllImport("UniDevice.dll")]
        private static extern void uniRobotRemoveSafety(IntPtr hRobot, int bit);
        [DllImport("UniDevice.dll")]
        private static extern void uniRobotRemoveAllSafeties(IntPtr hRobot);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGetEmergLevel(IntPtr hRobot, int bit, ref int pLevel, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGetTriggerLevel(IntPtr hRobot, int bit, ref int pLevel, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGetSafetyLevel(IntPtr hRobot, int bit, ref int pLevel, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotAddActionToSafety(IntPtr hRobot, int bit, uint actId, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern int uniRobotGetActionCountInSafety(IntPtr hRobot, int bit);
        [DllImport("UniDevice.dll")]
        private static extern void uniRobotRemoveActionFromSafety(IntPtr hRobot, int bit, uint actId);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotAddBan(IntPtr hRobot, uint actId, int bit, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern void uniRobotRemoveAllBans(IntPtr hRobot, uint actId);
        [DllImport("UniDevice.dll")]
        private static extern void uniRobotRemoveBan(IntPtr hRobot, uint actId, int bit);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotIsSafetyOn(IntPtr hRobot);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotSwitchSafety(IntPtr hRobot, bool bOn, IntPtr hNotifyWnd);

        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotSetOutputLevelE1(IntPtr hRobot, int slave, int offset, int bit, int level, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGetOutputLevelE1(IntPtr hRobot, int slave, int offset, int bit, ref int level, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGetInputLevelE1(IntPtr hRobot, int slave, int offset, int bit, ref int level, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern void uniRobotSetMotionLatence(IntPtr hRobot, int miniSeconds);
        [DllImport("UniDevice.dll")]
        private static extern int uniRobotGetMotionLatence(IntPtr hRobot);
        //New interfaces since 25-01-22 @25-01-25
        [DllImport("UniDevice.dll")]
        private static extern void uniRobotRemoveAllActions(IntPtr hRobot);
        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniRobotAddActionHome(IntPtr hRobot, uint actId, ushort axis, ref int errCode);
        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniRobotAddActionHomeXY(IntPtr hRobot, uint actId, ushort axisX, ushort axisY, ref int errCode);
        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniRobotAddActionHomeZXY(IntPtr hRobot, uint actId, ushort axisX, ushort axisY, ushort axisZ, bool bFreeMove, ref int errCode);
        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniRobotAddActionHomeMulti3(IntPtr hRobot, uint actId, ushort axisX, ushort axisY, ushort axisZ, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniRobotAddActionMove(IntPtr hRobot, uint actId, ushort axis, double pos, bool bAbsolute, ref int errCode);
        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniRobotAddActionMoveXY(IntPtr hRobot, uint actId, ushort axisX, ushort axisY, double posX, double posY, bool bAbsolute, ref int errCode);

        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniRobotAddActionLineToXY(IntPtr hRobot, uint actId, ushort axisX, ushort axisY, double posX, double posY, bool bAbsolute, ref int errCode);
        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniRobotAddActionArcToXY(IntPtr hRobot, uint actId, ushort axisX, ushort axisY, double cx, double cy, double ex, double ey, int direct, ref int pErrCode);

        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniRobotAddActionMoveZXY(IntPtr hRobot, uint actId, int axisX, int axisY, int axisZ, double posX, double posY, double posZ, bool bAbsolute, ref int errCode);
        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniRobotAddActionMoveXYZ(IntPtr hRobot, uint actId, int axisX, int axisY, int axisZ, double posX, double posY, double posZ, bool bAbsolute, double initZ, ref int errCode);
        [DllImport("UniDevice.dll")]
        //顺序执行2轴
        private static extern IntPtr uniRobotAddActionSequence2(IntPtr hRobot, uint actId, int axis1, int axis2, double pos1, double pos2, bool bAbsolute, ref int errCode);
        [DllImport("UniDevice.dll")]
        //顺序执行3轴
        private static extern IntPtr uniRobotAddActionSequence3(IntPtr hRobot, uint actId, int axis1, int axis2, int axis3, double pos1, double pos2, double pos3, bool bAbsolute, ref int errCode);
        [DllImport("UniDevice.dll")]
        //自定动作:单个
        private static extern IntPtr uniRobotAddActionCustom(IntPtr hRobot, uint actId, /*IntPtr pAxisArray, int axisCount,*/ CustomActionImplementDelegate pfnActionImplement, IntPtr pUserData, ref int errCode);
        // End of @25-01-25
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotSetAlarmLights(IntPtr hRobot, int lights, bool bFlicker, bool bBeepOn, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotSetBeepSilent(IntPtr hRobot, ref int pErrCode);
        [DllImport("UniDevice.dll")]
        private static extern void uniRobotSetFlickerDuration(IntPtr hRobot, int duration);
        [DllImport("UniDevice.dll")]
        private static extern int uniRobotGetFlickerDuration(IntPtr hRobot);
        [DllImport("UniDevice.dll")]
        private static extern bool uniRobotGenTransitLine(IntPtr hRobot, RvPointF64 startXY, double startZ, RvPointF64 endXY, double endZ, double transZ, double safeZ, double fillet, bool bCheckOnly, ref double pNewTransZ, IntPtr hQueue);
        [DllImport("UniDevice.dll")]
        private static extern void uniRobotSetActionCurPriority(IntPtr hRobot, uint actId, int level);
        [DllImport("UniDevice.dll")]
        private static extern int uniRobotGetActionCurPriority(IntPtr hRobot, uint actId);
        [DllImport("UniDevice.dll")]
        private static extern int uniRobotGetActionCurState(IntPtr hRobot, uint actId);
        [DllImport("UniDevice.dll")]
        private static extern IntPtr uniRobotGetActionCurOptions(IntPtr hRobot, uint actId);
        ~KRobot()
        {
            Destroy();
        }

        public KRobot()
        {

        }

        public KRobot(IntPtr hRobot, bool bDestroy = false)
        {
            m_hRobot = hRobot;

            if (hRobot != IntPtr.Zero)
            {
                m_bReferHandle = (!bDestroy);
            }
        }

        public IntPtr GetHandle() { return m_hRobot; }
        public bool IsValid() { return m_hRobot != IntPtr.Zero; }

        private IntPtr m_hRobot = IntPtr.Zero;
        private bool m_bReferHandle = false;

        //************************************************************************************************
        //创建KRobot
        public void FromHandle(IntPtr h)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                throw new Exception("KRobot instance exists already!");
            }

            m_hRobot = h;
            m_bReferHandle = true;
        }

        public bool Create()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return false;
                //throw new Exception("Object exist already!");
            }

            m_hRobot = uniCreateRobot();
            return (m_hRobot != IntPtr.Zero);
        }

        //删除KRobot
        public void Destroy()
        {
            if (m_bReferHandle)
            {
                m_hRobot = IntPtr.Zero;
                m_bReferHandle = false;
                return;
            }

            if (m_hRobot != IntPtr.Zero)
            {
                uniDestroyRobot(m_hRobot);
                m_hRobot = IntPtr.Zero;
            }
        }

        public string GetMotionName()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                IntPtr hName = uniRobotGetMotionName(m_hRobot);

                if (hName != IntPtr.Zero)
                {
                    return Marshal.PtrToStringAnsi(hName);
                }
            }
            return null;
        }

        public bool Start(int activity, IntPtr hNotifyWnd, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotStart(m_hRobot, activity, hNotifyWnd, ref pErrCode);
                return result;
            }
            return false;
        }

        public void Stop()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotStop(m_hRobot);
            }
        }

        public void SetNotifyWnd(IntPtr hNotifyWnd)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotSetNotifyWnd(m_hRobot, hNotifyWnd);
            }
        }

        public IntPtr GetNotifyWnd()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGetNotifyWnd(m_hRobot);
            }
            return IntPtr.Zero;
        }



        public void StopEx(bool bEmergStop)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotStopEx(m_hRobot, bEmergStop);
            }
        }

        public bool Delay(uint actId, int duration, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotDelay(m_hRobot, actId, duration, ref pErrCode);
                return result;
            }
            return false;
        }

        public int GetCurState()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                int state = uniRobotGetCurState(m_hRobot);
                return state;
            }
            return -1; // Or any appropriate error code
        }

        public string GetErrorDescription(int errorCode)
        {

            if (m_hRobot != IntPtr.Zero)
            {
                IntPtr hErrorDesc = uniRobotGetErrorDesc(m_hRobot, errorCode);

                if (hErrorDesc == IntPtr.Zero)
                {
                    return string.Empty;
                }

                return Marshal.PtrToStringAnsi(hErrorDesc);
            }

            return string.Empty; // null string
        }

        public static string GetErrorDescription(KMotion motion, int errorCode)
        {
            KRobot robot = new KRobot();

            if (!robot.Create())
            {
                return "system error, robot can not be created";
            }

            robot.Attach(motion);

            IntPtr hErrorDesc = uniRobotGetErrorDesc(robot.GetHandle(), errorCode);

            if (hErrorDesc != IntPtr.Zero)
            {
                string strErrorDesc = Marshal.PtrToStringAnsi(hErrorDesc);
                return strErrorDesc;
            }

            return null; // null string
        }


        public bool GetCurrentPosition(ref double pX, ref double pY, ref double pZ, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotGetCurPos(m_hRobot, ref pX, ref pY, ref pZ, ref pErrCode);
                return result;
            }
            return false;
        }

        public bool GetCurrentPosition(ref double pX, ref double pY, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                double z = 0;
                bool result = uniRobotGetCurPos(m_hRobot, ref pX, ref pY, ref z, ref pErrCode);
                return result;
            }
            return false;
        }

        public bool GetCurrentPosition(ref double pX, ref double pYorY1, ref double pZ, ref double pRorY2, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotGetCurPosEx1(m_hRobot, ref pX, ref pYorY1, ref pZ, ref pRorY2, ref pErrCode);
                return result;
            }
            return false;
        }

        public bool GetCurrentPosition(ushort axis, ref double pPos, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                IntPtr hMotion = uniRobotGetMotion(m_hRobot);
                if (IntPtr.Zero == hMotion)
                {
                    return false;
                }

                return uniRobotGetCurPosEx2(m_hRobot, axis, ref pPos, ref pErrCode);

            }
            return false;
        }

        public bool GetCurrentSpeed(ref double pX, ref double pY, ref double pZ, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotGetCurSpeed(m_hRobot, ref pX, ref pY, ref pZ, ref pErrCode);
                return result;
            }
            return false;
        }

        public bool GetCurrentSpeed(ref double pX, ref double pYorY1, ref double pZ, ref double pRorY2, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotGetCurSpeedEx1(m_hRobot, ref pX, ref pYorY1, ref pZ, ref pRorY2, ref pErrCode);
                return result;
            }
            return false;
        }

        public bool GetCurrentSpeed(ushort axis, ref double pSpeed, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotGetCurSpeedEx2(m_hRobot, axis, ref pSpeed, ref pErrCode);
                return result;
            }
            return false;
        }
        public bool SetOutputLevel(int bit, int level, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotSetOutputLevel(m_hRobot, bit, level, ref pErrCode);
                return result;
            }
            return false;
        }

        public bool GetOutputLevel(int bit, ref int level, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotGetOutputLevel(m_hRobot, bit, ref level, ref pErrCode);
                return result;
            }
            return false;
        }

        public bool GetInputLevel(int bit, ref int level, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotGetInputLevel(m_hRobot, bit, ref level, ref pErrCode);
                return result;
            }
            return false;
        }

        public bool SetOutputLevel(int slave, int offset, int bit, int level, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotSetOutputLevelE1(m_hRobot, slave, offset, bit, level, ref pErrCode);
                return result;
            }
            return false;
        }
        //slave 默认：0
        public bool GetOutputLevel(int slave, int offset, int bit, ref int level, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotGetOutputLevelE1(m_hRobot, slave, offset, bit, ref level, ref pErrCode);
                return result;
            }
            return false;
        }

        public bool GetInputLevel(int slave, int offset, int bit, ref int level, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotGetInputLevelE1(m_hRobot, slave, offset, bit, ref level, ref pErrCode);
                return result;
            }
            return false;
        }

        //public bool Move(ushort axis, double pos, bool bAbsolute, IntPtr pOptions, bool bSynchronous, ref int pErrCode)
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        bool result = uniRobotMove(m_hRobot, axis, pos, bAbsolute, pOptions, bSynchronous, ref pErrCode);
        //        return result;
        //    }
        //    return false;
        //}

        //public bool MoveAny2(int axisX, int axisY, double x, double y, bool bAbsolute, IntPtr pOptions, bool bSynchronous, ref int pErrCode)
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        bool result = uniRobotMoveAny2(m_hRobot, axisX, axisY, x, y, bAbsolute, pOptions, bSynchronous, ref pErrCode);
        //        return result;
        //    }
        //    return false;
        //}

        //public bool MoveAny3(int axisX, int axisY, int axisZ, double x, double y, double z, bool bAbsolute, IntPtr pOptions, bool bSynchronous, ref int pErrCode)
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        bool result = uniRobotMoveAny3(m_hRobot, axisX, axisY, axisZ, x, y, z, bAbsolute, pOptions, bSynchronous, ref pErrCode);
        //        return result;
        //    }
        //    return false;
        //}

        public bool GoHome(uint actId, ushort axis, KMoptions options, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGoHome(m_hRobot, actId, axis, options.GetHandle(), ref pErrCode); ;
            }
            return false;
        }

        public bool MoveTo(uint actId, ushort axis, double pos, bool bAbsolute, KMoptions options, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotMoveTo(m_hRobot, actId, axis, pos, bAbsolute, options.GetHandle(), ref pErrCode);
            }
            return false;
        }

        public bool GoHomeAny(uint actId, ushort[] axisArray, KMoptions options, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                int count = axisArray.Length;
                if (count <= 0) return false;

                short[] _axisArr = new short[count];
                for (int i = 0; i < count; i++)
                {
                    _axisArr[i] = (short)axisArray[i];
                }

                IntPtr pAxisArray = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(short)) * count);

                Marshal.Copy(_axisArr, 0, pAxisArray, count);

                bool ret = uniRobotGoHomeMany(m_hRobot, actId, pAxisArray, count, options.GetHandle(), ref pErrCode);

                Marshal.FreeHGlobal(pAxisArray);

                return ret;
            }
            return false;
        }

        public bool MoveManyTo(uint actId, ushort[] axisArray, double[] posArray, bool bAbsolute, KMoptions options, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                if (axisArray.Length != posArray.Length) return false;

                int count = axisArray.Length;
                if (count <= 0) return false;

                short[] _axisArr = new short[count];
                for (int i = 0; i < count; i++)
                {
                    _axisArr[i] = (short)axisArray[i];
                }

                IntPtr pAxisArray = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(short)) * count);
                IntPtr pPosArray = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * count);

                Marshal.Copy(_axisArr, 0, pAxisArray, count);
                Marshal.Copy(posArray, 0, pPosArray, count);

                bool ret = uniRobotMoveManyTo(m_hRobot, actId, pAxisArray, pPosArray, count, bAbsolute, options.GetHandle(), ref pErrCode);

                Marshal.FreeHGlobal(pAxisArray);
                Marshal.FreeHGlobal(pPosArray);

                return ret;


            }
            return false;
        }

        public bool MoveManyTo(uint actId, ushort[] axisArray, double[] posArray, IntPtr[] optArray, bool bAbsolute, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                if (axisArray.Length != posArray.Length) return false;

                int count = axisArray.Length;
                if (count <= 0) return false;

                short[] _axisArr = new short[count];
                for (int i = 0; i < count; i++)
                {
                    _axisArr[i] = (short)axisArray[i];
                }

                IntPtr pAxisArray = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(short)) * count);
                IntPtr pPosArray = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * count);
                IntPtr pOptArray = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * count);

                Marshal.Copy(_axisArr, 0, pAxisArray, count);
                Marshal.Copy(posArray, 0, pPosArray, count);
                Marshal.Copy(optArray, 0, pOptArray, count);

                bool ret = uniRobotMoveManyToE1(m_hRobot, actId, pAxisArray, pPosArray, pOptArray, count, bAbsolute, ref pErrCode);

                Marshal.FreeHGlobal(pAxisArray);
                Marshal.FreeHGlobal(pPosArray);
                Marshal.FreeHGlobal(pOptArray);

                return ret;


            }
            return false;
        }



        public bool LineAnyTo(uint actId, uint csid, ushort[] axisArray, double[] posArray, bool bAbsolute, KMoptions options, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                if (axisArray.Length != posArray.Length) return false;

                int count = axisArray.Length;
                if (count <= 0) return false;

                short[] _axisArr = new short[count];
                for (int i = 0; i < count; i++)
                {
                    _axisArr[i] = (short)axisArray[i];
                }

                IntPtr pAxisArray = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(short)) * count);
                IntPtr pPosArray = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * count);

                Marshal.Copy(_axisArr, 0, pAxisArray, count);
                Marshal.Copy(posArray, 0, pPosArray, count);

                bool ret = uniRobotLineAnyTo(m_hRobot, actId, csid, pAxisArray, pPosArray, count, bAbsolute, options.GetHandle(), ref pErrCode);

                Marshal.FreeHGlobal(pAxisArray);
                Marshal.FreeHGlobal(pPosArray);

                return ret;
            }

            return false;
        }

        public bool LineTo(uint actId, uint csid, ushort axisX, ushort axisY, double x, double y, bool bAbsolute, KMoptions options, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotLineTo(m_hRobot, actId, csid, axisX, axisY, x, y, bAbsolute, options.GetHandle(), ref pErrCode); //插补
            }
            return false;
        }

        public bool ArcTo(uint actId, uint csid, ushort axisX, ushort axisY, RvPointF64 center, RvPointF64 end, int dir, KMoptions options, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotArcTo(m_hRobot, actId, csid, axisX, axisY, center.x, center.y, end.x, end.y, dir, options.GetHandle(), ref pErrCode);
            }
            return false;
        }

        public bool Go(uint actId, KCmdQueue queue, KMoptions options, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGo(m_hRobot, actId, queue.GetHandle(), options.GetHandle(), ref pErrCode);
            }
            return false;
        }

        public bool GoHomeEx(uint actId, ushort axis, KMoptions options, SuppleTriple suppleTriple, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGoHomeEx(m_hRobot, actId, axis, options.GetHandle(), suppleTriple, ref pErrCode);
            }
            return false;
        }

        public bool MoveToEx(uint actId, ushort axis, double pos, bool bAbsolute, KMoptions options, SuppleTriple suppleTriple, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotMoveToEx(m_hRobot, actId, axis, pos, bAbsolute, options.GetHandle(), suppleTriple, ref pErrCode);
            }
            return false;
        }

        public bool GoHomeManyEx(uint actId, IntPtr pAxisArray, int count, KMoptions options, SuppleTriple suppleTriple, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGoHomeManyEx(m_hRobot, actId, pAxisArray, count, options.GetHandle(), suppleTriple, ref pErrCode);
            }
            return false;
        }

        public bool MoveManyToEx(uint actId, IntPtr pAxisArray, IntPtr pPosArray, int count, bool bAbsolute, KMoptions options, SuppleTriple suppleTriple, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotMoveManyToEx(m_hRobot, actId, pAxisArray, pPosArray, count, bAbsolute, options.GetHandle(), suppleTriple, ref pErrCode);
            }
            return false;
        }

        public bool MoveManyToEx(uint actId, IntPtr pAxisArray, IntPtr pPosArray, IntPtr hOptArray, int count, bool bAbsolute, SuppleTriple suppleTriple, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotMoveManyToExE1(m_hRobot, actId, pAxisArray, pPosArray, hOptArray, count, bAbsolute, suppleTriple, ref pErrCode);
            }
            return false;
        }

        public bool LineAnyToEx(uint actId, uint csid, IntPtr pAxisArray, IntPtr pPosArray, int count, bool bAbsolute, IntPtr hOptions, SuppleTriple suppleTriple, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotLineAnyToEx(m_hRobot, actId, csid, pAxisArray, pPosArray, count, bAbsolute, hOptions, suppleTriple, ref pErrCode);
            }
            return false;
        }

        public bool LineToEx(uint actId, uint csid, ushort axisX, ushort axisY, double x, double y, bool bAbsolute, KMoptions options, SuppleTriple suppleTriple, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotLineToEx(m_hRobot, actId, csid, axisX, axisY, x, y, bAbsolute, options.GetHandle(), suppleTriple, ref pErrCode); ;
            }
            return false;
        }

        public bool ArcToEx(uint actId, uint csid, ushort axisX, ushort axisY, RvPointF64 center, RvPointF64 end, int dir, KMoptions options, SuppleTriple suppleTriple, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotArcToEx(m_hRobot, actId, csid, axisX, axisY, center, end, dir, options.GetHandle(), suppleTriple, ref pErrCode);

            }
            return false;
        }

        public bool GoEx(uint actId, KCmdQueue queue, KMoptions options, SuppleTriple suppleTriple, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGoEx(m_hRobot, actId, queue.GetHandle(), options.GetHandle(), suppleTriple, ref pErrCode);

            }
            return false;
        }


        public bool WaitInput(IntPtr pIn, int nCheckFlags, int timeout, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotWaitInput(m_hRobot, pIn, nCheckFlags, timeout, ref pErrCode);
                return result;
            }
            return false;
        }



        public bool WaitMove(uint actId, ushort axis, int nCheckFlags, KMoptions options, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotWaitMove(m_hRobot, actId, axis, nCheckFlags, options.GetHandle(), ref pErrCode);
                return result;
            }
            return false;
        }
        public bool WaitHome(uint actId, ushort axis, int nCheckFlags, KMoptions options, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotWaitHome(m_hRobot, actId, axis, nCheckFlags, options.GetHandle(), ref pErrCode);
                return result;
            }
            return false;
        }

        public bool WaitLine4d(uint actId, uint csid, ushort axisX, ushort axisY, ushort axisZ, ushort axisR,
                             int nCheckFlags, KMoptions options, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotWaitLine4d(m_hRobot, actId, csid, axisX, axisY, axisZ, axisR, nCheckFlags, options.GetHandle(), ref pErrCode);
                return result;
            }
            return false;
        }

        public bool WaitLine(uint actId, uint csid, ushort axisX, ushort axisY, int nCheckFlags, KMoptions options, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotWaitLine(m_hRobot, actId, csid, axisX, axisY, nCheckFlags, options.GetHandle(), ref pErrCode);
                return result;
            }

            return false;
        }

        public bool WaitLine3d(uint actId, uint csid, ushort axisX, ushort axisY, ushort axisZ, int nCheckFlags, IntPtr hOptions, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotWaitLine3d(m_hRobot, actId, csid, axisX, axisY, axisZ, nCheckFlags, hOptions, ref pErrCode);

            }
            return false;
        }




        public bool WaitLineXd(uint actId, uint csid, IntPtr hAxes, int nCheckFlags, IntPtr hOptions, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotWaitLineXd(m_hRobot, actId, csid, hAxes, nCheckFlags, hOptions, ref pErrCode);
                //return result;
            }
            return false;
        }

        public bool WaitCrd(uint actId, uint csid, KMaxis axes, int nCheckFlags, KMoptions options, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotWaitCrd(m_hRobot, actId, csid, axes.GetHandle(), nCheckFlags, options.GetHandle(), ref pErrCode);
            }
            return false;
        }

        public bool WaitQueue(uint actId, IntPtr hQueue, int nCheckFlags, IntPtr hOptions, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotWaitQueue(m_hRobot, actId, hQueue, nCheckFlags, hOptions, ref pErrCode);
            }
            return false;
        }

        public bool HaltMovingAxis(ushort axis, IntPtr option, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotHaltMovingAxis(m_hRobot, axis, option, ref pErrCode);
                return result;
            }
            return false;
        }

        public bool HaltHomingAxis(ushort axis, IntPtr option, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotHaltHomingAxis(m_hRobot, axis, option, ref pErrCode);
                return result;
            }
            return false;
        }

        public bool IsActionActive(uint actId)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotIsActionActive(m_hRobot, actId);
                return result;
            }
            return false;
        }

        public bool ActivateAction(uint actId, int sceneId, bool bSynch, IntPtr hNotifyWnd, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotActivateAction(m_hRobot, actId, sceneId, bSynch, hNotifyWnd, ref pErrCode);

            }
            return false;
        }

        public bool ActivateAction(uint actId, KMoptions options, bool bSynch, IntPtr hNotifyWnd, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                IntPtr hOption = (options == null ? IntPtr.Zero : options.GetHandle());

                return uniRobotActivateActionE3(m_hRobot, actId, hOption, bSynch, hNotifyWnd, ref pErrCode);

            }
            return false;
        }


        public bool ActivateAction(KActeen action, KMoptions options, bool bSynch, IntPtr hNotifyWnd, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                IntPtr hOption = (options == null ? IntPtr.Zero : options.GetHandle());

                return uniRobotActivateActionE1(m_hRobot, hOption, options.GetHandle(), bSynch, hNotifyWnd, ref pErrCode);

            }
            return false;
        }

        public bool ActivateAction(KActeen action, KMoptions options, bool bSynch, IntPtr hNotifyWnd, ref uint pActId, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                IntPtr hOption = (options == null ? IntPtr.Zero : options.GetHandle());


                return uniRobotActivateActionE2(m_hRobot, hOption, options.GetHandle(), bSynch, hNotifyWnd, ref pActId, ref pErrCode);

            }
            return false;
        }



        public bool ActivateAction(uint actId, int scene, double pos, bool bSynch, IntPtr hNotifyWnd, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {

                return uniRobotActivateActionMove(m_hRobot, actId, scene, pos, bSynch, hNotifyWnd, ref pErrCode);

            }
            return false;
        }

        public bool ActivateAction(uint actId, int scene, double x, double y, bool bAlongLine, bool bSynch, IntPtr hNotifyWnd, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                if (bAlongLine)
                {
                    return uniRobotActivateActionLineToXY(m_hRobot, actId, scene, x, y, bSynch, hNotifyWnd, ref pErrCode);

                }
                else
                {
                    return uniRobotActivateActionMoveXY(m_hRobot, actId, scene, x, y, bSynch, hNotifyWnd, ref pErrCode);
                }

            }
            return false;
        }

        public bool ActivateAction(uint actId, IntPtr hOptions, bool bSynch, IntPtr hNotifyWnd, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotActivateActionE3(m_hRobot, actId, hOptions, bSynch, hNotifyWnd, ref pErrCode);

            }
            return false;
        }


        public void TerminateAction(uint actId)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotTerminateAction(m_hRobot, actId);
            }
        }

        public void TerminateAllActions()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotTerminateAllActions(m_hRobot);
            }
        }


        public bool IsActionAbort(uint actId)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotIsActionAbort(m_hRobot, actId);

            }

            return false;
        }

        public bool IsAnyActionActivated()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotIsAnyActionActivated(m_hRobot);
            }

            return false;
        }


        public int GetActivatedActionCount()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGetActivatedActionCount(m_hRobot);
            }

            return 0;
        }

        public bool IsAnyActionActive()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotIsAnyActionActive(m_hRobot);
            }

            return false;
        }

        public void CancelAction(uint actId)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotCancelAction(m_hRobot, actId);
            }
        }

        public bool IsActionCanceled(uint actId)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotIsActionCanceled(m_hRobot, actId);
            }

            return false;
        }

        public void SetActionNormal(uint actId)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotSetActionNormal(m_hRobot, actId);
            }
        }

        public bool IsActionDead(uint actId)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotIsActionDead(m_hRobot, actId);
            }

            return false;
        }
        public bool IsActionActivated(uint actId)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotIsActionActivated(m_hRobot, actId);
            }
            return false;
        }


        public bool AddEmerg(int bit, bool bReverse, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotAddEmerg(m_hRobot, bit, bReverse, ref pErrCode);
                return result;
            }
            return false;
        }

        public int GetEmergCount()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                int count = uniRobotGetEmergCount(m_hRobot);
                return count;
            }
            return -1; // Or any appropriate error code
        }

        public void RemoveEmerg(int bit)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotRemoveEmerg(m_hRobot, bit);
            }
        }

        public void RemoveAllEmergs()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotRemoveAllEmergs(m_hRobot);
            }
        }

        public int GetTriggerCount()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                int count = uniRobotGetTriggerCount(m_hRobot);
                return count;
            }
            return -1; // Or any appropriate error code
        }

        public bool AddTrigger(int bit, int type, int nInitLevel, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotAddTrigger(m_hRobot, bit, type, nInitLevel, ref pErrCode);
            }

            return false;
        }

        public void RemoveTrigger(int bit)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotRemoveTrigger(m_hRobot, bit);
            }
        }

        public void RemoveAllTriggers()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotRemoveAllTriggers(m_hRobot);
            }
        }

        public KMoptions AddScene(int scene, double speed, double acc, double dec, bool bUpdateIfExist, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                IntPtr h = uniRobotAddScene(m_hRobot, scene, speed, acc, dec, bUpdateIfExist, ref pErrCode);
                if (h != IntPtr.Zero)
                {
                    KMoptions op = new KMoptions();
                    op.Attach(h);
                    return op;
                }

            }
            return null;
        }

        public int GetSceneCount()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                int count = uniRobotGetSceneCount(m_hRobot);
                return count;
            }

            return 0; // Or any appropriate error code
        }

        public void RemoveScene(int sceneId)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotRemoveScene(m_hRobot, sceneId);
            }
        }

        public void RemoveAllScenes()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotRemoveAllScenes(m_hRobot);
            }
        }
        public bool AddAction(IntPtr action, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotAddAction(m_hRobot, action, ref pErrCode);
                return result;
            }
            return false;
        }

        public int GetActionCount()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                int count = uniRobotGetActionCount(m_hRobot);
                return count;
            }
            return -1; // Or any appropriate error code
        }

        public void RemoveAction(uint actId)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotRemoveAction(m_hRobot, actId);
            }
        }

        public KActeen GetActionAt(int index)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                IntPtr h = uniRobotGetActionAt(m_hRobot, index);
                if (h != IntPtr.Zero)
                {
                    return new KActeen(h);
                }
            }
            return null;
        }

        public KActeen FindAction(uint actId)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                IntPtr h = uniRobotFindAction(m_hRobot, actId);

                if (h != IntPtr.Zero)
                {
                    return new KActeen(h);
                }
            }
            return null;
        }


        public bool IsTriggerOn()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotIsTriggerOn(m_hRobot);
                return result;
            }
            return false;
        }

        public bool SwitchTrigger(bool bOn, IntPtr hNotifyWnd)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotSwitchTrigger(m_hRobot, bOn, hNotifyWnd);

            }
            return false;
        }
        //public bool ActivateTrigger()
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        bool result = uniRobotActivateTrigger(m_hRobot);
        //        return result;
        //    }
        //    return false;
        //}

        //public void DeactivateTrigger()
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        uniRobotDeactivateTrigger(m_hRobot);
        //    }
        //}


        public bool IsEmergenceOn()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotIsEmergenceOn(m_hRobot);
                return result;
            }
            return false;
        }

        public bool SwitchEmergence(bool bOn, IntPtr hNotifyWnd)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotSwitchEmergence(m_hRobot, bOn, hNotifyWnd);
                return result;
            }
            return false;
        }

        public bool IsFuncInputOn()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotIsFuncInputOn(m_hRobot);
                return result;
            }
            return false;
        }

        public bool SwitchFuncInput(bool bOn, IntPtr hNotifyWnd)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotSwitchFuncInput(m_hRobot, bOn, hNotifyWnd);
            }
            return false;
        }

        public bool AddSafety(int type, int bit, bool bReverse, ref int errCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotAddSafety(m_hRobot, type, bit, bReverse, ref errCode);
            }
            return false;
        }

        public int GetSafetyCount()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGetSafetyCount(m_hRobot);
            }
            return 0;
        }

        public void RemoveSafety(int bit)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotRemoveSafety(m_hRobot, bit);
            }

        }

        public void RemoveAllSafeties()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotRemoveAllSafeties(m_hRobot);
            }
        }

        public bool IsSafetyOn()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotIsSafetyOn(m_hRobot);
            }

            return false;
        }

        public bool GetEmergLevel(int bit, ref int pLevel, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGetEmergLevel(m_hRobot, bit, ref pLevel, ref pErrCode);
            }

            return false;
        }

        public bool GetTriggerLevel(int bit, ref int pLevel, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGetTriggerLevel(m_hRobot, bit, ref pLevel, ref pErrCode);
            }

            return false;
        }

        public bool GetSafetyLevel(int bit, ref int pLevel, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGetSafetyLevel(m_hRobot, bit, ref pLevel, ref pErrCode);
            }

            return false;
        }

        public bool AddActionToSafety(int bit, uint actId, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotAddActionToSafety(m_hRobot, bit, actId, ref pErrCode);
            }

            return false;
        }

        public int GetActionCountInSafety(int bit)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGetActionCountInSafety(m_hRobot, bit);
            }

            return 0;
        }

        public void RemoveActionFromSafety(int bit, uint actId)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotRemoveActionFromSafety(m_hRobot, bit, actId);
            }

        }

        public bool AddBan(uint actId, int bit, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotAddBan(m_hRobot, actId, bit, ref pErrCode);
            }
            return false;
        }
        public void RemoveAllBans(uint actId)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotRemoveAllBans(m_hRobot, actId);
            }
        }
        public void RemoveBan(uint actId, int bit)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotRemoveBan(m_hRobot, actId, bit);
            }
        }
        public bool SwitchSafety(bool bOn, IntPtr hNotifyWnd)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotSwitchSafety(m_hRobot, bOn, hNotifyWnd);
            }

            return false;
        }

        public void Attach(KMotion motion)
        {

            if (null == motion)
            {
                return;
            }

            if (m_hRobot != IntPtr.Zero)
            {

                if (IntPtr.Zero != motion.GetHandle())
                {
                    uniRobotAttachMotion(m_hRobot, motion.GetHandle());
                }
            }

        }

        public void Detach()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotDetachMotion(m_hRobot);
            }
        }




        //public bool AddSoftTrigger(int bit, int type, int nInitLevel, IntPtr hNotifyWnd)
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        return uniRobotAddTrigger(m_hRobot, bit, type, nInitLevel, hNotifyWnd);
        //    }

        //    return false;
        //}

        //public void RemoveSoftTrigger(int bit)
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        uniRobotRemoveSoftTrigger(m_hRobot, bit);
        //    }

        //}

        //public void RemoveAllSoftTriggers()
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        uniRobotRemoveAllSoftTriggers(m_hRobot);
        //    }

        //}

        public IntPtr GetMotionHandle()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                IntPtr result = uniRobotGetMotion(m_hRobot);
                return result;
            }
            return IntPtr.Zero;
        }

        public bool IsRunning()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotIsRunning(m_hRobot);
            }
            return false;
        }

        public bool Pause(ref int err)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotPause(m_hRobot, ref err);
            }
            return false;
        }

        public bool Continue(ref int err)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotContinue(m_hRobot, ref err);
            }
            return false;
        }

        public bool IsPaused()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotIsPaused(m_hRobot);
                return result;
            }
            return false;
        }

        public bool Reset(ref int errCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotReset(m_hRobot, ref errCode);
            }

            return false;
        }

        public void Raise()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotRaise(m_hRobot);
            }
        }

        public void Flat()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotFlat(m_hRobot);
            }
        }

        public bool IsRequested()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotIsRequested(m_hRobot);
                return result;
            }
            return false;
        }

        public KMoptions GetSceneOptions(int scene)
        {


            if (m_hRobot != IntPtr.Zero)
            {
                IntPtr h = uniRobotGetSceneOptions(m_hRobot, scene);
                if (h == IntPtr.Zero)
                {
                    return null;
                }

                KMoptions opt = new KMoptions();
                opt.Attach(h);

                return opt;
                // global.Assert(h != IntPtr.Zero, "无法获得运动场景参数");
            }

            return null;

        }

        //public bool ActionHome(int scene, ushort axis, IntPtr hNotifyWnd)
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        return uniActionHome(this.m_hRobot, scene, axis, hNotifyWnd);
        //    }
        //    return false;
        //}
        ////Action will revoke start automatically
        //public bool ActionHomeXY(int scene, int axisX, int axisY, IntPtr hNotifyWnd)
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        return uniActionHomeXY(this.m_hRobot, scene, axisX, axisY, hNotifyWnd);
        //    }
        //    return false;
        //}
        ////Action will revoke start automatically
        //public bool ActionHomeZXY(int scene, int axisX, int axisY, int axisZ, IntPtr hNotifyWnd)
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        return uniActionHomeZXY(this.m_hRobot, scene, axisX, axisY, axisZ, hNotifyWnd);
        //    }

        //    return false;
        //}
        ////Action will revoke start automatically
        //public bool ActionMove(int scene, ushort axis, double pos, bool bAbsolute, IntPtr hNotifyWnd)
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        return uniActionMove(m_hRobot, scene, axis, pos, bAbsolute, hNotifyWnd);
        //    }

        //    return false;
        //}
        ////Action will revoke start automatically
        //public bool ActionMove(int scene, int axisX, int axisY, double posX, double posY, bool bAbsolute, IntPtr hNotifyWnd)
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        return uniActionMoveXY(m_hRobot, scene, axisX, axisY, posX, posY, bAbsolute, hNotifyWnd);
        //        // return uniActionHome(this.m_hRobot, axis, hNotifyWnd);
        //    }
        //    return false;
        //}
        ////Action will revoke start automatically
        //public bool ActionMove(int scene, int axisX, int axisY, int axisZ, double posX, double posY, double posZ, bool bAbsolute, IntPtr hNotifyWnd)
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        return uniActionMoveZXY(m_hRobot, scene, axisX, axisY, axisZ, posX, posY, posZ, bAbsolute, hNotifyWnd);
        //    }
        //    return false;
        //}
        ////Action will revoke start automatically
        //public bool ActionMove(int scene, int axisX, int axisY, int axisZ, double posX, double posY, double posZ, bool bAbsolute, double initZ, IntPtr hNotifyWnd)
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        return uniActionMoveXYZ(m_hRobot, scene, axisX, axisY, axisZ, posX, posY, posZ, bAbsolute, initZ, hNotifyWnd);
        //    }

        //    return false;
        //}
        ////Action will revoke start automatically
        //public bool ActionRepeatMove(int scene, int axisX, int axisY, double initX, double initY, double posX, double posY, bool bSingleMove, IntPtr hNotifyWnd)
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        return uniActionRepeatMove(m_hRobot,   scene,   axisX,   axisY,   initX,   initY,   posX,   posY,   bSingleMove, hNotifyWnd);
        //    }

        //    return false;
        //}

        //public bool ActionMoveInPulse(int scene, ushort axis, int pulses, IntPtr hNotifyWnd)
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        return uniActionMoveInPulse(m_hRobot,   scene,   axis,   pulses,   hNotifyWnd);
        //    }

        //    return false;
        //}


        ////Action will revoke start automatically
        //public bool ActionSeq(int scene, int axis1, int axis2, double pos1, double pos2, bool bAbsolute, IntPtr hNotifyWnd)
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        return uniActionSequence2(m_hRobot, scene, axis1, axis2, pos1, pos2, bAbsolute, hNotifyWnd);
        //    }
        //    return false;
        //}
        ////Action will revoke start automatically
        //public bool ActionSeq(int scene, int axis1, int axis2, int axis3, double pos1, double pos2, double pos3, bool bAbsolute, IntPtr hNotifyWnd)
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        return uniActionSequence3(m_hRobot, scene, axis1, axis2, axis3, pos1, pos2, pos3, bAbsolute, hNotifyWnd);
        //    }
        //    return false;
        //}
        //Action will revoke start automatically
        //public bool Excute(uint actId, CustomActionImplementDelegate caiDelegate, IntPtr pUserData, IntPtr hNotifyWnd)
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        return uniActionCustom(m_hRobot, actId, caiDelegate, pUserData, hNotifyWnd);
        //    }

        //    return false;
        //}

        //public void StopAction()
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        uniActionStop(this.m_hRobot);
        //    }
        //}

        //public bool IsCanceled()
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        return uniRobotIsCanceled(this.m_hRobot);
        //    }

        //    return false;
        //}

        //public bool BeginActionQueue()
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        return uniActionBeginMulti(this.m_hRobot);
        //    }

        //    return false;
        //}

        //public IntPtr AddActionToQueue(uint actId)
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        return uniAddAction(this.m_hRobot, actId);
        //    }

        //    return IntPtr.Zero;
        //}
        //public IntPtr AddActionToQueue(uint actId, CustomActionImplementDelegate pfnActionImplement, IntPtr hUserData)
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        return uniAddActionE1(m_hRobot, actId, pfnActionImplement, hUserData);
        //    }

        //    return IntPtr.Zero;
        //}

        //public void SetActionImplementFunc(IntPtr hAction, CustomActionImplementDelegate pfnActionImplement, IntPtr pUserData)
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        uniActionSetImplementFunc(hAction, pfnActionImplement, pUserData);
        //    }
        //}
        public int SetActionControlUnits(IntPtr hAction, IntPtr pUnitsArray, int unitsCount)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniActionSetControlUnits(this.m_hRobot, pUnitsArray, unitsCount);
            }

            return 0;
        }
        //public bool EndActionQueue(IntPtr hNotifyWnd)
        //{
        //    if (m_hRobot != IntPtr.Zero)
        //    {
        //        return uniActionEndMulti(this.m_hRobot, hNotifyWnd);
        //    }

        //    return false;
        //}

        public bool EnterStartState(ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotEnterStartState(m_hRobot, ref pErrCode);
            }

            return false;
        }

        public void LeaveStartState()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotLeaveStartState(m_hRobot);
            }

        }

        public bool RaiseException(uint actId, int exId, IntPtr pExceptData, int size, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotRaiseException(m_hRobot, actId, exId, pExceptData, size, ref pErrCode);
            }

            return false;
        }

        public void TreatException(uint actId, int exId, bool bAbort, IntPtr pExceptData, int size)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotTreatException(m_hRobot, actId, exId, bAbort, pExceptData, size);
            }

        }

        public bool WaitExceptionConfirm(uint actId, int exId, int timeOut, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotWaitExceptionConfirm(m_hRobot, actId, exId, timeOut, ref pErrCode);
            }
            return false;
        }

        public bool GetExceptionData(uint actId, int exId, IntPtr pExceptData, int size)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGetExceptionData(m_hRobot, actId, exId, pExceptData, size);
            }
            return false;
        }


        public void EnterEmergState(out int pErrCode)
        {
            pErrCode = 0;
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotEnterEmergState(m_hRobot, ref pErrCode);
            }

        }

        public bool ClearEmergState(ref int pErrCode)
        {
            pErrCode = 0;
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotClearEmergState(m_hRobot, ref pErrCode);
            }

            return false;
        }

        public void SetMotionLatence(int miniSeconds)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotSetMotionLatence(m_hRobot, miniSeconds);
            }
        }

        public int GetMotionLatence()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGetMotionLatence(m_hRobot);
            }

            return 0;
        }

        public void RemoveAllActions()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotRemoveAllActions(m_hRobot);
            }
        }

        public IntPtr AddActionHome(uint actId, ushort axis, ref int errCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotAddActionHome(m_hRobot, actId, axis, ref errCode);
            }
            return IntPtr.Zero;
        }

        public IntPtr AddActionHomeXY(uint actId, ushort axisX, ushort axisY, ref int errCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotAddActionHomeXY(m_hRobot, actId, axisX, axisY, ref errCode);
            }
            return IntPtr.Zero;
        }

        public IntPtr AddActionHomeZXY(uint actId, ushort axisX, ushort axisY, ushort axisZ, bool bFreeMove, ref int errCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotAddActionHomeZXY(m_hRobot, actId, axisX, axisY, axisZ, bFreeMove, ref errCode);
            }
            return IntPtr.Zero;
        }

        public IntPtr AddActionHomeMulti3(uint actId, ushort axis1, ushort axis2, ushort axis3, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotAddActionHomeMulti3(m_hRobot, actId, axis1, axis2, axis3, ref pErrCode);
            }
            return IntPtr.Zero;
        }

        public IntPtr AddActionMove(uint actId, ushort axis, double pos, bool bAbsolute, ref int errCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotAddActionMove(m_hRobot, actId, axis, pos, bAbsolute, ref errCode);
            }
            return IntPtr.Zero;
        }

        public IntPtr AddActionMoveXY(uint actId, ushort axisX, ushort axisY, double posX, double posY, bool bAbsolute, ref int errCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotAddActionMoveXY(m_hRobot, actId, axisX, axisY, posX, posY, bAbsolute, ref errCode);
            }
            return IntPtr.Zero;
        }

        public IntPtr AddActionLineToXY(uint actId, ushort axisX, ushort axisY, double posX, double posY, bool bAbsolute, ref int errCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotAddActionLineToXY(m_hRobot, actId, axisX, axisY, posX, posY, bAbsolute, ref errCode);
            }
            return IntPtr.Zero;
        }

        public IntPtr AddActionArcToXY(uint actId, ushort axisX, ushort axisY, double cx, double cy, double ex, double ey, int direct, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotAddActionArcToXY(m_hRobot, actId, axisX, axisY, cx, cy, ex, ey, direct, ref pErrCode);
            }
            return IntPtr.Zero;
        }


        public IntPtr AddActionMoveZXY(uint actId, ushort axisX, ushort axisY, ushort axisZ, double posX, double posY, double posZ, bool bAbsolute, ref int errCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotAddActionMoveZXY(m_hRobot, actId, axisX, axisY, axisZ, posX, posY, posZ, bAbsolute, ref errCode);
            }
            return IntPtr.Zero;
        }
        public IntPtr AddActionMoveXYZ(uint actId, ushort axisX, ushort axisY, ushort axisZ, double posX, double posY, double posZ, bool bAbsolute, ref int errCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotAddActionMoveZXY(m_hRobot, actId, axisX, axisY, axisZ, posX, posY, posZ, bAbsolute, ref errCode);
            }
            return IntPtr.Zero;
        }
        public IntPtr AddActionSequence2(uint actId, ushort axis1, ushort axis2, double pos1, double pos2, bool bAbsolute, ref int errCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotAddActionSequence2(m_hRobot, actId, axis1, axis2, pos1, pos2, bAbsolute, ref errCode);
            }
            return IntPtr.Zero;
        }

        public IntPtr AddActionSequence3(uint actId, ushort axis1, ushort axis2, int axis3, double pos1, double pos2, double pos3, bool bAbsolute, ref int errCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotAddActionSequence3(m_hRobot, actId, axis1, axis2, axis3, pos1, pos2, pos3, bAbsolute, ref errCode);
            }
            return IntPtr.Zero;
        }

        public IntPtr AddActionCustom(uint actId, /*ushort[] axisArray,*/ CustomActionImplementDelegate pfnActionImplement, IntPtr pUserData, ref int errCode)
        {
            IntPtr h = IntPtr.Zero;

            if (m_hRobot != IntPtr.Zero)
            {
                //int cnt = axisArray.Length;



                h = uniRobotAddActionCustom(m_hRobot, actId, /*ptr, cnt,*/  pfnActionImplement, pUserData, ref errCode);

            }

            return h;
        }

        public KMoptions GetSceneAt(int index)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                IntPtr h = uniRobotGetOptionsAt(m_hRobot, index);
                if (h != IntPtr.Zero)
                {
                    return new KMoptions(h, false);
                }
            }
            return null;
        }
        public bool SetAlarmLights(int lights, bool bFlicker, bool bBeepOn, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotSetAlarmLights(m_hRobot, lights, bFlicker, bBeepOn, ref pErrCode);
            }

            return false;
        }

        public bool SetBeepSilent(ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotSetBeepSilent(m_hRobot, ref pErrCode);
            }

            return false;
        }

        public void SetFlickerDuration(int duration)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotSetFlickerDuration(m_hRobot, duration);
            }
        }

        public int GetFlickerDuration()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGetFlickerDuration(m_hRobot);
            }
            return 0;
        }

        public bool GenTransitLine(RvPointF64 startXY, double startZ, RvPointF64 endXY, double endZ, double transZ, double safeZ, double fillet, bool bCheckOnly, ref double pNewTransZ, IntPtr hQueue)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGenTransitLine(m_hRobot, startXY, startZ, endXY, endZ, transZ, safeZ, fillet, bCheckOnly, ref pNewTransZ, hQueue);
            }
            return false;
        }

        public void SetActionCurPriority(uint actId, ACTION_PRIORITY level)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotSetActionCurPriority(m_hRobot, actId, (int)level);
            }
        }
        public ACTION_PRIORITY GetActionCurPriority(uint actId)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return (ACTION_PRIORITY)uniRobotGetActionCurPriority(m_hRobot, actId);
            }

            return ACTION_PRIORITY.ERROR;
        }

        public ACTION_STATE GetActionCurState(uint actId)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return (ACTION_STATE)uniRobotGetActionCurState(m_hRobot, actId);
            }

            return ACTION_STATE.UNKNOWN;
        }

        public KMoptions GetActionCurOptions(uint actId)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                IntPtr h = uniRobotGetActionCurOptions(m_hRobot, actId);

                if (h != IntPtr.Zero)
                {
                    return new KMoptions(h, false);
                }
            }

            return null;
        }



        public bool AddFio(int id, int bit, int initLevel, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotAddFio(m_hRobot, id, bit, initLevel, ref pErrCode);
                return result;
            }
            return false;
        }

        public bool GetFioLevel(int id, ref int level, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotGetFioLevel(m_hRobot, id, ref level, ref pErrCode);
                return result;
            }
            return false;
        }

        public bool SetFioLevel(int id, int level, ref int pErrCode)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                bool result = uniRobotSetFioLevel(m_hRobot, id, level, ref pErrCode);
                return result;
            }
            return false;
        }
        public int GetFioCount()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGetFioCount(m_hRobot);

            }
            return 0;
        }
        public void RemoveAllFios()
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotRemoveAllFios(m_hRobot);

            }
        }
        public void RemoveFio(int id)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                uniRobotRemoveFio(m_hRobot, id);
            }
        }
        public int GetFioBit(int id)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGetFioBit(m_hRobot, id);
            }
            return 0;
        }
        public int GetFioId(int bit)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGetFioId(m_hRobot, bit);

            }
            return 0;
        }

        public int GetFioIdAt(int index)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGetFioIdAt(m_hRobot, index);

            }
            return 0;
        }

        public bool IsFioInputAt(int index)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotIsFioInputAt(m_hRobot, index);

            }
            return false;
        }

        public bool IsFioOutputAt(int index)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotIsFioOutputAt(m_hRobot, index);

            }
            return false;
        }

        public bool IsFioInput(int id)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotIsFioInput(m_hRobot, id);

            }
            return false;
        }

        public bool IsFioOutput(int id)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotIsFioOutput(m_hRobot, id);

            }
            return false;
        }
        public bool SetActionOption(uint actId, string strName, int nValue)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotSetActionOption(m_hRobot, actId, strName, nValue);

            }
            return false;
        }

        public bool SetActionOption(uint actId, string strName, bool bValue)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotSetActionOptionE1(m_hRobot, actId, strName, bValue);

            }
            return false;
        }
        public bool SetActionOption(uint actId, string strName, double nValue)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotSetActionOptionE2(m_hRobot, actId, strName, nValue);

            }
            return false;
        }
        public bool SetActionOption(uint actId, string strName, string strValue)
        {
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotSetActionOptionE3(m_hRobot, actId, strName, strValue);

            }
            return false;
        }


        public bool GetActionOption(uint actId, string strName, out int nValue)
        {
            nValue = 0;
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGetActionOption(m_hRobot, actId, strName, out nValue);

            }
            return false;
        }

        public bool GetActionOption(uint actId, string strName, out bool bValue)
        {
            bValue = false;
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGetActionOptionE1(m_hRobot, actId, strName, out bValue);

            }
            return false;
        }
        public bool GetActionOption(uint actId, string strName, out double nValue)
        {
            nValue = 0;
            if (m_hRobot != IntPtr.Zero)
            {
                return uniRobotGetActionOptionE2(m_hRobot, actId, strName, out nValue);

            }
            return false;
        }
        public bool GetActionOption(uint actId, string strName, out string strValue)
        {
            strValue = "";
            if (m_hRobot != IntPtr.Zero)
            {
                StringBuilder sb = new StringBuilder(1204);

                if (uniRobotGetActionOptionE3(m_hRobot, actId, strName, sb, 1204))
                {
                    strValue = sb.ToString();
                    return true;
                }
            }
            return false;
        }


    }
}
