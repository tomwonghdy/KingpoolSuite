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
using Kingpool.Xgui;

namespace Kingpool.AutoTab
{
    //局部视场对象
    public enum MV_MODE
    {
        DEFAULT = 0,   //默认, 可以添加对象
        ADD_CNCOBJ = 1,   //添加CNC对象
        MOD_CNCOBJ = 2,    //修改CNC对象，位置和删除
        LEARN_MARK = 3,    //mark校正
        MULTI_COPY = 4,      //矩阵复制	
    }
    //全局视场对象
    public enum GV_MODE
    {
        DEFAULT = 0,     //默认,导航
        ADDOBJ = 1,      //添加cncobj
        SELROI = 2,  //选择ROI 
    }

    public class KCncView : KRealView
    {
        public const int CVT_GLOBAL = 2170;
        public const int CVT_MOBILE = 2171;

        //显示类型
        public const int VM_DEFAULT = 0;
        public const int VM_FILLWIN = 1;
        public const int VM_ZOOMIN = 2;
        public const int VM_ZOOMOUT = 3;
        public const int VM_CUSTOM = 4;


        //添加类型
        public const int AT_PICK = 0;
        public const int AT_POINT = 1;
        public const int AT_LINE = 2;
        public const int AT_ARC = 3;
        public const int AT_CIRCLE = 4;
        public const int AT_SHAPE_L = 5;
        public const int AT_SHAPE_U = 6;
        public const int AT_POLYLINE = 7;
        public const int AT_RECT = 8;
        public const int AT_CIRCLE_2P = 9;
        public const int AT_RECT_EX = 10;
        public const int AT_POLYGON = 11;
        public const int AT_CONCEN = 12;
        public const int AT_RAKE = 13;


        //消息
        //public const int WM_UC_SUB_FINISH = (WM_USER + 1501);
        public const int WM_UC_SUB_POPMENU = (WM_USER + 1502);
        public const int WM_UC_SUB_REFRASH = (WM_USER + 1503);
        public const int WM_MV_SIGHT_DBLCLK = (WM_USER + 1509);
        public const int WM_MV_LMOUSE_CLICK = (WM_USER + 1505); // 
        public const int WM_MV_RMOUSE_CLICK = (WM_USER + 1506); // 
        public const int WM_MV_SIGHT_MOUSE_MOVE = (WM_USER + 1507);
        public const int WM_UC_OBJ_ADD = (WM_USER + 1510);
        public const int WM_UC_OBJ_DEL = (WM_USER + 1511);
        public const int WM_UC_OBJ_MOD = (WM_USER + 1512);
        public const int WM_UC_CTRLPOINT_HIT = (WM_USER + 1501);
        public const int WM_UC_CTRLPOINT_UNHIT = (WM_USER + 1508);

        //CNC OBJ添加事件
        public const int EID_CNCOBJ_ADD = 0x0060;
        public const int EID_SUB_POPMENU = 0x0061;
        public const int EID_SUB_REFRESH = 0x0062;
        public const int EID_CTRLPT_ADD = 0x0063;
        public const int EID_CTRLPT_REVOKE = 0x0064;

        //全局视场矩形选择事件
        public const int EID_RECT_SELECT_START = 0x0070;
        public const int EID_RECT_SELECT_END = 0x0071;


        //选项开关
        public const int OF_IGNORE_KEY_CONTROL = 1;
        public const int OF_TRACE_ON_CANVAS = (1 << 1);

        [DllImport("UniVision.dll")]
        private static extern IntPtr uniCncviewCreate(IntPtr hParent, int type, string strName, int sightWidth, int sightHeight, RvRect rect);
        [DllImport("UniVision.dll")]
        private static extern void uniCncviewDestroy(IntPtr hCncview);
        [DllImport("UniVision.dll")]
        private static extern void uniCncviewSetSpotVisible(IntPtr hCncview, bool bVisible, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern bool uniCncviewIsSpotVisible(IntPtr hCncview);
        [DllImport("UniVision.dll")]
        private static extern void uniCncviewSetSpotAppearance(IntPtr hCncview, RvRgba color, int thick, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern void uniCncviewSetSpotSize(IntPtr hCncview, int cx, int cy, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern GSize uniCncviewGetSpotSize(IntPtr hCncview);
        [DllImport("UniVision.dll")]
        private static extern void uniCncviewSetSpotPos(IntPtr hCncview, int x, int y, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern void uniCncviewSetViewMode(IntPtr hCncview, int mode, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern int uniCncviewGetViewMode(IntPtr hCncview);
        [DllImport("UniVision.dll")]
        private static extern bool uniCncviewSetAddinType(IntPtr hCncview, int type, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern int uniCncviewGetAddinType(IntPtr hCncview);
        [DllImport("UniVision.dll")]
        private static extern void uniCncviewSetSparkleVisible(IntPtr hCncview, bool flag, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern bool uniCncviewIsSparkleVisible(IntPtr hCncview);
        [DllImport("UniVision.dll")]
        private static extern void uniCncviewSetSparkleSize(IntPtr hCncview, int min, int max, int steps, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern void uniCncviewBeatSparkle(IntPtr hCncview, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern void uniCncviewSetSparklePos(IntPtr hCncview, int x, int y, bool bRefresh);

        [DllImport("UniVision.dll")]
        private static extern void uniCncviewEnableResizeMark(IntPtr hCncview, bool flag, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern bool uniCncviewIsMarkResizable(IntPtr hCncview);
        [DllImport("UniVision.dll")]
        private static extern void uniCncviewSetPosMark(IntPtr hCncview, int index, IntPtr hIndicater, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern void uniCncviewClearPosMark(IntPtr hCncview, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern void uniCncviewSetPosMarkVisible(IntPtr hCncview, bool bVisible, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern bool uniCncviewIsPosMarkVisible(IntPtr hCncview);
        [DllImport("UniVision.dll")]
        private static extern IntPtr uniCncviewGetHwnd(IntPtr hCncview);

        [DllImport("UniVision.dll")]
        private static extern void uniCncviewSetSubMode(IntPtr hCncview, int mode, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern int uniCncviewGetSubMode(IntPtr hCncview);

        [DllImport("UniVision.dll")]
        private static extern void uniCncviewSetNotifyWnd(IntPtr hCncview, IntPtr hNotifyWnd);
        [DllImport("UniVision.dll")]
        private static extern IntPtr uniCncviewGetNotifyWnd(IntPtr hCncview);
        [DllImport("UniVision.dll")]
        private static extern void uniCncviewEnableCncobjCreation(IntPtr hCncview, bool flag);
        [DllImport("UniVision.dll")]
        private static extern bool uniCncviewIsCncobjCreationEnbled(IntPtr hCncview);
        [DllImport("UniVision.dll")]
        //至少4个点
        private static extern bool uniCncviewGetRectVertex(IntPtr hCncview, IntPtr/*RvPoint_f**/ pVertexArray, int count);
        //至少3个点，点越多，将分的越细
        [DllImport("UniVision.dll")]
        private static extern bool uniCncviewGetCircleVertex(IntPtr hCncview, IntPtr/*RvPoint_f**/ pVertexArray, int count);
        //至少4个点
        [DllImport("UniVision.dll")]
        private static extern bool uniCncviewGetRakeVertex(IntPtr hCncview, IntPtr/*RvPoint_f**/ pVertexArray, int count);
        //至少3个点，点越多，将分的越细
        [DllImport("UniVision.dll")]
        private static extern bool uniCncviewGetArcVertex(IntPtr hCncview, IntPtr/*RvPoint_f**/ pVertexArray, int count);
        [DllImport("UniVision.dll")]
        private static extern void uniCncviewAddTracePos(IntPtr hCncview, float x, float y, bool bCanvasPos, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern void uniCncviewRemoveLastPos(IntPtr hCncview, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern void uniCncviewClearAllTracePos(IntPtr hCncview, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern void uniCncviewCommitTracePos(IntPtr hCncview, bool bRefresh);



        //事件
        public event RealViewEvent OnSightRectSelectStart;
        public event RealViewEvent OnSightRectSelectEnd;
        public event RealViewEvent OnCncobjAdditionOccur;

        public event RealViewEvent OnSightPopMenu;
        public event RealViewEvent OnSightRefresh;

        public event RealViewEvent OnControlPointAdd;
        public event RealViewEvent OnControlPointRevoke;


        public bool Create(IntPtr hParent, int type, string strName, int sightWidth, int sightHeight, RvRect rect)
        {
            m_hSight = uniCncviewCreate(hParent, type, strName, sightWidth, sightHeight, rect);

            if (m_hSight != IntPtr.Zero)
            {
                SetEventProcCallback(m_eventProcDelegate);
            }

            return (m_hSight != IntPtr.Zero);
        }

        public override bool Create(IntPtr hParent, string strName, int left, int top, int width, int height)
        {
            RvRect re = new RvRect(left, top, left + width, top + height);
            return this.Create(hParent, CVT_GLOBAL, strName, 0, 0, re);
        }

        public override bool CreateEx(IntPtr hParent, string strName, int canvasWidth, int canvasHeight, RvRect rect, int flags)
        {
            return this.Create(hParent, CVT_GLOBAL, strName, canvasWidth, canvasHeight, rect);
        }

        public override void Destroy()
        {
            if (m_hSight != IntPtr.Zero && !m_bAttached)
            {
                uniCncviewDestroy(m_hSight);
                m_hSight = IntPtr.Zero;
            }
        }

        public override bool RaiseViewEvents(IntPtr hObj, uint uEvent, IntPtr wParam, IntPtr lParam)
        {


            object sender = this;
            RvEventArgs args = new RvEventArgs();
            args.hObject = hObj;
            args.wParam = wParam;
            args.lParam = lParam;

            switch (uEvent)
            {
                case EID_RECT_SELECT_START:
                    {
                        if (OnSightRectSelectStart != null)
                        {
                            OnSightRectSelectStart.Invoke(sender, args);
                            return true;
                        }
                    }
                    break;
                case EID_RECT_SELECT_END:
                    {
                        if (null != OnSightRectSelectEnd)
                        {
                            OnSightRectSelectEnd.Invoke(sender, args);
                            return true;
                        }
                    }
                    break;
                case EID_CNCOBJ_ADD:
                    {
                        if (null != OnCncobjAdditionOccur)
                        {
                            OnCncobjAdditionOccur.Invoke(sender, args);
                            return true;
                        }
                    }
                    break;
                case EID_SUB_POPMENU:
                    {
                        if (null != OnSightPopMenu)
                        {
                            OnSightPopMenu.Invoke(sender, args);
                            return true;
                        }
                    }
                    break;
                case EID_SUB_REFRESH:
                    {
                        if (null != OnSightRefresh)
                        {
                            OnSightRefresh.Invoke(sender, args);
                            return true;
                        }
                    }
                    break;
                case EID_CTRLPT_ADD:
                    {
                        if (null != OnControlPointAdd)
                        {
                            OnControlPointAdd.Invoke(sender, args);
                            return true;
                        }
                    }
                    break;
                case EID_CTRLPT_REVOKE:
                    {
                        if (null != OnControlPointRevoke)
                        {
                            OnControlPointRevoke.Invoke(sender, args);
                            return true;
                        }
                    }
                    break;
            }

            return base.RaiseViewEvents(hObj, uEvent, wParam, lParam);
        }
        public void SetSpotVisible(bool bVisible, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniCncviewSetSpotVisible(m_hSight, bVisible, bRefresh);
            }
        }

        public bool IsSpotVisible()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniCncviewIsSpotVisible(m_hSight);
            }
            return false;
        }

        public void SetSpotAppearance(RvRgba color, int thick, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniCncviewSetSpotAppearance(m_hSight, color, thick, bRefresh);
            }
        }

        public void SetSpotSize(int cx, int cy, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniCncviewSetSpotSize(m_hSight, cx, cy, bRefresh);
            }
        }

        public GSize GetSpotSize()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniCncviewGetSpotSize(m_hSight);
            }
            return new GSize();
        }

        public void SetSpotPos(int x, int y, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniCncviewSetSpotPos(m_hSight, x, y, bRefresh);
            }
        }

        public void SetViewMode(int mode, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniCncviewSetViewMode(m_hSight, mode, bRefresh);
            }
        }
        public new int GetViewMode()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniCncviewGetViewMode(m_hSight);
            }
            return 0;
        }

        public bool SetAddinType(int type, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniCncviewSetAddinType(m_hSight, type, bRefresh);
            }
            return false;
        }

        public int GetAddinType()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniCncviewGetAddinType(m_hSight);
            }
            return 0;
        }

        public void SetSparkleVisible(bool flag, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniCncviewSetSparkleVisible(m_hSight, flag, bRefresh);
            }
        }
        public bool IsSparkleVisible()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniCncviewIsSparkleVisible(m_hSight);
            }
            return false;
        }

        public void SetSparkleSize(int min, int max, int steps, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniCncviewSetSparkleSize(m_hSight, min, max, steps, bRefresh);
            }
        }

        public void BeatSparkle(bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniCncviewBeatSparkle(m_hSight, bRefresh);
            }
        }

        public void SetSparklePos(int x, int y, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniCncviewSetSparklePos(m_hSight, x, y, bRefresh);
            }
        }

        public void EnableResizeMark(bool flag, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniCncviewEnableResizeMark(m_hSight, flag, bRefresh);
            }
        }
        public bool IsMarkResizable()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniCncviewIsMarkResizable(m_hSight);
            }
            return false;
        }

        public void SetPosMark(int index, IntPtr hIndicater, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniCncviewSetPosMark(m_hSight, index, hIndicater, bRefresh);
            }
        }

        public void ClearPosMark(bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniCncviewClearPosMark(m_hSight, bRefresh);
            }
        }
        public void SetPosMarkVisible(bool bVisible, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniCncviewSetPosMarkVisible(m_hSight, bVisible, bRefresh);
            }
        }

        public bool IsPosMarkVisible()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniCncviewIsPosMarkVisible(m_hSight);
            }
            return true;
        }

        public IntPtr GetHwnd()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniCncviewGetHwnd(m_hSight);
            }
            return IntPtr.Zero;
        }

        public virtual void SetSubMode(int mode, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniCncviewSetSubMode(m_hSight, mode, bRefresh);
            }
        }

        public virtual int GetSubMode()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniCncviewGetSubMode(m_hSight);
            }
            return -1;
        }

        public void SetNotifyWnd(IntPtr hNotifyWnd)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniCncviewSetNotifyWnd(m_hSight, hNotifyWnd);
            }
        }

        public IntPtr GetNotifyWnd()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniCncviewGetNotifyWnd(m_hSight);
            }
            return IntPtr.Zero;
        }

        public void EnableCncobjCreation(bool flag)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniCncviewEnableCncobjCreation(m_hSight, flag);
            }
        }

        public bool IsCncobjCreationEnbled()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniCncviewIsCncobjCreationEnbled(m_hSight);
            }
            return false;
        }
        public bool GetRectVertex(ref RvPointF32[] vertexArray)
        {
            if (m_hSight != IntPtr.Zero)
            {
                int cnt = vertexArray.Length;
                if (cnt < 4) return false;

                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RvPointF32)) * cnt);

                bool ret = uniCncviewGetRectVertex(m_hSight, ptr, vertexArray.Length);

                if (ret)
                {
                    for (int i = 0; i < cnt; i++)
                    {
                        IntPtr ptrtmp = (IntPtr)(ptr + i * Marshal.SizeOf(typeof(RvPointF32)));
                        vertexArray[i] = (RvPointF32)Marshal.PtrToStructure(ptrtmp, typeof(RvPointF32));
                    }
                }


                Marshal.FreeHGlobal(ptr);

                return ret;
            }

            return false;
        }

        public bool GetCircleVertex(ref RvPointF32[] vertexArray)
        {
            if (m_hSight != IntPtr.Zero)
            {
                int cnt = vertexArray.Length;
                if (cnt < 3) return false;

                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RvPointF32)) * cnt);

                bool ret = uniCncviewGetCircleVertex(m_hSight, ptr, vertexArray.Length);

                for (int i = 0; i < cnt; i++)
                {
                    IntPtr ptrtmp = (IntPtr)(ptr + i * Marshal.SizeOf(typeof(RvPointF32)));
                    vertexArray[i] = (RvPointF32)Marshal.PtrToStructure(ptrtmp, typeof(RvPointF32));
                }

                Marshal.FreeHGlobal(ptr);

                return ret;
            }

            return false;
        }

        public bool GetRakeVertex(ref RvPointF32[] vertexArray)
        {
            if (m_hSight != IntPtr.Zero)
            {
                int cnt = vertexArray.Length;
                if (cnt < 4) return false;

                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RvPointF32)) * cnt);

                bool ret = uniCncviewGetRakeVertex(m_hSight, ptr, vertexArray.Length);

                for (int i = 0; i < cnt; i++)
                {
                    IntPtr ptrtmp = (IntPtr)(ptr + i * Marshal.SizeOf(typeof(RvPointF32)));
                    vertexArray[i] = (RvPointF32)Marshal.PtrToStructure(ptrtmp, typeof(RvPointF32));
                }

                Marshal.FreeHGlobal(ptr);

                return ret;
            }

            return false;
        }

        public bool GetArcVertex(ref RvPointF32[] vertexArray)
        {
            if (m_hSight != IntPtr.Zero)
            {
                int cnt = vertexArray.Length;
                if (cnt < 3) return false;

                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RvPointF32)) * cnt);

                bool ret = uniCncviewGetArcVertex(m_hSight, ptr, vertexArray.Length);

                for (int i = 0; i < cnt; i++)
                {
                    IntPtr ptrtmp = (IntPtr)(ptr + i * Marshal.SizeOf(typeof(RvPointF32)));
                    vertexArray[i] = (RvPointF32)Marshal.PtrToStructure(ptrtmp, typeof(RvPointF32));
                }

                Marshal.FreeHGlobal(ptr);

                return ret;
            }

            return false;
        }

        public void AddTracePos(float x, float y, bool bCanvasPos, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniCncviewAddTracePos(m_hSight, x, y, bCanvasPos, bRefresh);
            }
        }

        public void RemoveLastPos(bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniCncviewRemoveLastPos(m_hSight, bRefresh);
            }
        }

        public void ClearAllTracePos(bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniCncviewClearAllTracePos(m_hSight, bRefresh);
            }
        }

        public void CommitTracePos(bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniCncviewCommitTracePos(m_hSight, bRefresh);
            }
        }

    }


}
