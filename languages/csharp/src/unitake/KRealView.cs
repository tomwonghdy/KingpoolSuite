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

namespace Kingpool.Xgui
{
    //public enum VIEW_MODE
    //{
    //    DEFAULT = 0,
    //    CENTER = 1,
    //    STRETCH = 2,
    //    STRETCH_KR = 3,
    //    CUSTOM = 10,
    //}

    public enum HIT_POS
    {
        NOTHING = -100,
        CAPTION = -5,
        BORDER = -4,
        ROTATER = -3,
        GRABBER = -2,
        MIDDLE = -1,
        EXTENDER = 0,
    }

    ///**结构体**/
    //[StructLayout(LayoutKind.Sequential)]
    //public struct GSize
    //{
    //    public int sx;
    //    public int sy;
    //    public GSize(int sx, int sy)
    //    {
    //        this.sx = sx;
    //        this.sy = sy;
    //    }
    //}

    //[StructLayout(LayoutKind.Sequential)]
    //public struct GRgb
    //{
    //    public byte red;
    //    public byte green;
    //    public byte blue;
    //    public byte alpha;

    //    public GRgb(byte r, byte g, byte b)
    //    {
    //        red = r; green = g; blue = b;
    //        alpha = 255;
    //    }
    //}

    public class RvEventArgs : EventArgs
    {
        public IntPtr hObject;
        public IntPtr wParam;
        public IntPtr lParam;
    }

    public delegate bool EventProcDelegate(IntPtr hDelegate, IntPtr hObj, uint uEvent, IntPtr wParam, IntPtr lParam, IntPtr pOwnerData); //声明委托
    public delegate void RealViewEvent(object sender, RvEventArgs e);

    //public enum LAYER
    //{
    //    CANVAS = (0x02 << 16),
    //    VIRTUAL = (0x04 << 16),
    //    DISPLAY = (0x08 << 16),
    //    BACKGROUND = (0x01 << 16),
    //}

    public class KRealView
    {

        public static int BS_DEFAULT = 0;
        public static int BS_CHECK = 1;      //方格

        //REFREHS TYPE
        public const int RT_UPDATE = 1;
        public const int RT_REDRAW = 2;

        public static int DM_EDITION = 0;  //编辑状态
        public static int DM_RUNTIME = 1;     //运行态
        public static int DM_ADDING = 10;  //正在添加
        public static int DM_DIALOG = 11;   //对话框，运行态，编辑态
        public static int DM_EXTENT = 100;

        //游标
        public const int UC_IGNORE = -1;

        public const int UC_PENTIP = 0;    //笔尖
        public const int UC_TAR_S = 1;     //方形目标
        public const int UC_TAR_R = 2;      //圆形目标
        public const int UC_DIVIDER = 3;      //动态十字线  
        public const int UC_RULER_H = 4;    //水平线  
        public const int UC_RULER_V = 5;      //垂直线
        public const int UC_RULER_HV = 6;      //十字线
        public const int UC_IMAGE = 7;        //任意图象文件
        public const int UC_CROSS = 11;   //静态十字线
        public const int UC_CROSS_R = 12;   //十字线+圆
        public const int UC_CROSS_S = 13;     //十字线+方
        public const int UC_TRIANGLE = 21;  //动态三角形(等边)

        //rvb logo alignment
        public const int LP_LEFTTOP = 0;
        public const int LP_RIGHTTOP = 1;
        public const int LP_LEFTBOTTOM = 2;
        public const int LP_RIGHTBOTTOM = 3;

        //Events id
        //UC 事件
        public const int RV_EID_UC_LM_CLICK = 0x0020;
        public const int RV_EID_UC_LM_DBLCLK = 0x0021;
        public const int RV_EID_UC_RM_CLICK = 0x0022;
        public const int RV_EID_UC_RM_DBLCLK = 0x0023;
        public const int RV_EID_UC_MOUSE_WHEEL = 0x0024;

        public const int RV_EID_UC_POSCHANGED = 0x0025;
        public const int RV_EID_UC_SIZECHANGED = 0x0026;

        public const int RV_EID_UC_SELECTED = 0x0010;
        public const int RV_EID_UC_DESELETED = 0x0011;
        public const int RV_EID_UC_APPENDED = 0x0012;

        //public const int RV_EID_UC_BEFORE_DELETE = 0x0015;
        //public const int RV_EID_UC_BEFORE_MOVE   = 0x0016;
        //public const int RV_EID_UC_BEFORE_RESIZE = 0x0017;

        public const int RV_EID_SELECT_CHANGED = 0x0040;
        public const int RV_EID_LMOUSE_CLICK = 0x0041;
        public const int RV_EID_LMOUSE_DBLCLK = 0x0042;
        public const int RV_EID_RMOUSE_CLICK = 0x0043;
        public const int RV_EID_RMOUSE_DBLCLK = 0x0044;
        public const int RV_EID_MOUSE_WHEEL = 0x0045;//中间轮子滚动
        public const int RV_EID_UC_HIT = 0x0080;
        public const int RV_EID_UC_UNHIT = 0x0081;

        //画布或显示窗口大小改变
        //public const int RV_EID_CANVASSIZE_CHANGED = 0x0049;
        //public const int RV_EID_PIXELDEPTH_CHANGED = 0x0050;
        //public const int RV_EID_DISPLAYSIZE_CHANGED = 0x0051;
        //public const int RV_EID_DISPLAYPOS_CHANGED = 0x0052;


        //消息FLAGS
        public const int MF_LMOUSE_CLICK = 1;
        public const int MF_LMOUSE_DBLCLK = 1 << 1;
        public const int MF_RMOUSE_CLICK = 1 << 2;
        public const int MF_RMOUSE_DBLCLK = 1 << 3;

        //消息定义
        public const int WM_USER = 0x0400;  //系统定义，不用处理
        public const int WM_LMOUSE_CLICK = (WM_USER + 1401);
        public const int WM_LMOUSE_DBLCLK = (WM_USER + 1402);
        public const int WM_RMOUSE_CLICK = (WM_USER + 1403);
        public const int WM_RMOUSE_DBLCLK = (WM_USER + 1404);

        //CUSTOM VIEW , 支持滚轮缩放
        //放大和缩小倍率参照画笔软件设置
        //每次缩放按照2倍进行
        private float MIN_CUSTOM_SCALE = 0.125f;
        private float MAX_CUSTOM_SCALE = 8.0f;
        //private float m_nCurScale   = 1.0f;
        //private float m_nScaleDelta = 2;




        [DllImport("UniVision.dll")]
        private static extern IntPtr uniCreateView(IntPtr hParent, string strName, int left, int top, int width, int height);

        [DllImport("UniVision.dll")]
        private static extern IntPtr uniCreateViewEx(IntPtr hParent, string strName, int canvasWidth, int canvasHeight, RvRect rect, int flags);

        [DllImport("UniVision.dll")]
        private static extern void uniDestroyView(IntPtr hRealView);
        [DllImport("UniVision.dll")]
        private static extern void uniViewRefresh(IntPtr hRealView, int type);
        [DllImport("UniVision.dll")]
        private static extern bool uniViewSetSightFormat(IntPtr hRealView, int imageType, int width, int height, bool bRedraw);
        [DllImport("UniVision.dll")]
        private static extern int uniViewGetImageType(IntPtr hRealView);
        [DllImport("UniVision.dll")]
        private static extern int uniViewGetImageWidth(IntPtr hRealView);
        [DllImport("UniVision.dll")]
        private static extern int uniViewGetImageHeight(IntPtr hRealView);
        [DllImport("UniVision.dll")]
        private static extern void uniViewSetDisplayRect(IntPtr hRealView, int left, int top, int width, int height, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern void uniViewSetDisplaySize(IntPtr hRealView, int width, int height, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern GSize uniViewGetDisplaySize(IntPtr hRealView);
        [DllImport("UniVision.dll")]
        private static extern GSize uniViewGetViewSize(IntPtr hRealView);

        [DllImport("UniVision.dll")]
        private static extern void uniViewSetUserCursorType(IntPtr hRealView, int cursorType, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern int uniViewGetUserCursorType(IntPtr hRealView);
        [DllImport("UniVision.dll")]
        private static extern void uniViewSetUserCursorSize(IntPtr hRealView, GSize size, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern GSize uniViewGetUserCursorSize(IntPtr hRealView);
        [DllImport("UniVision.dll")]
        private static extern void uniViewSetUserCursorTransparence(IntPtr hRealView, int ratio, bool bRefresh);

        [DllImport("UniVision.dll")]
        private static extern int uniViewGetUserCursorTransparence(IntPtr hRealView);
        [DllImport("UniVision.dll")]
        private static extern void uniViewSetUserCursorColor(IntPtr hRealView, GRgb color, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern GRgb uniViewGetUserCursorColor(IntPtr hRealView);
        [DllImport("UniVision.dll")]
        private static extern int uniViewGetUserCursorLineType(IntPtr hRealView);
        [DllImport("UniVision.dll")]
        private static extern void uniViewSetUserCursorThickness(IntPtr hRealView, int thickness, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern int uniViewGetUserCursorThickness(IntPtr hRealView);
        [DllImport("UniVision.dll")]
        private static extern void uniViewSetUserCursorImage(IntPtr hRealView, IntPtr image, bool bRefresh);
        [DllImport("UniVision.dll")]

        private static extern void uniViewSetPixelBarFontSize(IntPtr hRealView, int size, bool bRefresh);

        // 获取像素条标签字体大小
        [DllImport("UniVision.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int uniViewGetPixelBarLabelFontSize(IntPtr hRealView);

        // 设置像素条透明度
        [DllImport("UniVision.dll")]
        private static extern void uniViewSetPixelBarTransparence(IntPtr hRealView, int ratio, bool bRefresh);

        // 获取像素条透明度
        [DllImport("UniVision.dll")]
        private static extern int uniViewGetPixelBarTransparence(IntPtr hRealView);

        // 设置像素条颜色
        [DllImport("UniVision.dll")]
        private static extern void uniViewSetPixelBarColor(IntPtr hRealView, GRgb color, bool bRefresh);

        // 获取像素条颜色
        [DllImport("UniVision.dll")]
        private static extern GRgb uniViewGetPixelBarColor(IntPtr hRealView);

        // 设置像素条可见性
        [DllImport("UniVision.dll")]
        private static extern void uniViewSetPixelBarVisible(IntPtr hRealView, bool flag, bool bRefresh);

        // 查询像素条可见性
        [DllImport("UniVision.dll")]

        private static extern bool uniViewIsPixelBarVisible(IntPtr hRealView);

        [DllImport("UniVision.dll")]
        private static extern void uniViewSetPixelBarText(IntPtr hRealView, string strText, bool bRefresh);

        // 获取像素条对象指针
        [DllImport("UniVision.dll")]
        private static extern IntPtr uniViewGetPixelBarUc(IntPtr hRealView);

        // 获取用户光标对象指针
        [DllImport("UniVision.dll")]
        private static extern IntPtr uniViewGetUserCursorUc(IntPtr hRealView);

        // 获取重绘类型
        [DllImport("UniVision.dll")]
        private static extern int uniViewGetRedrawType(IntPtr hRealView);
        // 设置重绘类型
        [DllImport("UniVision.dll")]
        private static extern void uniViewSetRedrawType(IntPtr hRealView, int type, int rate);
        [DllImport("UniVision.dll")]
        private static extern void uniViewClearFrame(IntPtr hRealView, bool bRedraw);
        // 喂入图像
        [DllImport("UniVision.dll")]
        private static extern bool uniViewFeedImage(IntPtr hRealView, IntPtr hImage);

        [DllImport("UniVision.dll")]
        private static extern bool uniViewFeedData(IntPtr hRealView, IntPtr pImageData, UInt64 size);
        [DllImport("UniVision.dll")]
        private static extern bool uniViewForceFrame(IntPtr hRealView, IntPtr hImage);
        [DllImport("UniVision.dll")]
        private static extern bool uniViewForceBuffer(IntPtr hRealView, IntPtr pImageData, uint size);
        [DllImport("UniVision.dll")]
        private static extern void uniViewSetFrameMip(IntPtr hRealView, int mip);

        // 加载标定
        [DllImport("UniVision.dll")]
        private static extern bool uniViewLoadCaliber(IntPtr hRealView, string strFilePath, bool bRedraw);
        // 保存标定
        [DllImport("UniVision.dll")]
        private static extern bool uniViewSaveCaliber(IntPtr hRealView, string strFilePath);

        [DllImport("UniVision.dll")]
        private static extern void uniViewAddWidget(IntPtr hRealView, IntPtr hWidget, bool bRedraw);
        [DllImport("UniVision.dll")]
        private static extern void uniViewRemoveWidget(IntPtr hRealView, IntPtr hWidget, bool bRedraw);
        [DllImport("UniVision.dll")]
        private static extern void uniViewRemoveWidgetE1(IntPtr hRealView, uint id, bool bRedraw);
        [DllImport("UniVision.dll")]
        private static extern void uniViewClearWidget(IntPtr hRealView, bool bRedraw);
        [DllImport("UniVision.dll")]
        private static extern int uniViewGetWidgetCount(IntPtr hRealView);
        [DllImport("UniVision.dll")]
        private static extern int uniViewGetSelectCount(IntPtr hRealView);

        [DllImport("UniVision.dll")]
        private static extern IntPtr uniViewGetWidgetAt(IntPtr hRealView, int index, bool bSelOnly);
        [DllImport("UniVision.dll")]
        private static extern IntPtr uniViewGetSelectAt(IntPtr hRealView, int index);

        [DllImport("UniVision.dll")]
        private static extern void uniViewSetBackgroundStyle(IntPtr hRealView, int style, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern UInt32 uniViewGetBackgroundStyle(IntPtr hRealView);
        [DllImport("UniVision.dll")]
        private static extern void uniViewSetBackgroundColor(IntPtr hRealView, GRgb color, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern GRgb uniViewGetBackgroundColor(IntPtr hRealView);
        [DllImport("UniVision.dll")]
        private static extern void uniViewSetBlankColor(IntPtr hRealView, GRgb color, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern GRgb uniViewGetBlankColor(IntPtr hRealView);
        [DllImport("UniVision.dll")]
        private static extern void uniViewSetDesignMode(IntPtr hRealView, int mode, bool bRefresh);

        [DllImport("UniVision.dll")]
        private static extern int uniViewGetDesignMode(IntPtr hRealView);
        [DllImport("UniVision.dll")]
        private static extern bool uniViewIsEditionLocked(IntPtr hRealView);
        [DllImport("UniVision.dll")]
        private static extern void uniViewSetEditionState(IntPtr hRealView, bool bLock, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern void uniViewSetViewMode(IntPtr hRealView, ViewMode mode, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern ViewMode uniViewGetViewMode(IntPtr hRealView);
        [DllImport("UniVision.dll")]
        private static extern void uniViewGetViewScale(IntPtr hRealView, ref float scaleX, ref float scaleY);
        [DllImport("UniVision.dll")]
        private static extern void uniViewSetViewScale(IntPtr hRealView, float scaleX, float scaleY, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern void uniViewSetViewLeftTop(IntPtr hRealView, int left, int top, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern void uniViewGetViewLeftTop(IntPtr hRealView, ref int left, ref int top);


        [DllImport("UniVision.dll")]
        private static extern bool uniViewIsImageFeed(IntPtr hRealView);
        [DllImport("UniVision.dll")]
        private static extern bool uniViewCopyImage(IntPtr hRealView, IntPtr hImage);
        [DllImport("UniVision.dll")]
        private static extern void uniViewSetRvbLogoPos(IntPtr hRealView, int location, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern int uniViewGetRvbLogoPos(IntPtr hRealView);
        [DllImport("UniVision.dll")]
        private static extern void uniViewSetRvbLogoVisible(IntPtr hRealView, bool flag, bool bRefresh);
        [DllImport("UniVision.dll")]
        private static extern bool uniViewIsRvbLogoVisible(IntPtr hRealView);
        [DllImport("UniVision.dll")]
        private static extern RvPoint uniViewConvertPointInCanvas(IntPtr hRealView, RvPoint point);
        [DllImport("UniVision.dll")]
        private static extern void uniViewShow(IntPtr hRealView, bool flag);
        [DllImport("UniVision.dll")]
        private static extern bool uniViewIsVisible(IntPtr hRealView);
        [DllImport("UniVision.dll")]
        private static extern double uniViewConvertXInDisplay(IntPtr hRealView, double x);
        [DllImport("UniVision.dll")]
        private static extern double uniViewConvertYInDisplay(IntPtr hRealView, double y);
        [DllImport("UniVision.dll")]
        private static extern double uniViewConvertXInCanvas(IntPtr hRealView, double x);
        [DllImport("UniVision.dll")]
        private static extern double uniViewConvertYInCanvas(IntPtr hRealView, double y);

        [DllImport("UniVision.dll")]
        private static extern void uniViewSetEventProcCallback(IntPtr hRealView, EventProcDelegate eventProcDelegate, IntPtr pOwnerData);
        [DllImport("UniVision.dll")]
        private static extern bool uniViewTranslateMessage(IntPtr hRealView, uint msgId, IntPtr wParam, IntPtr lParam);
        [DllImport("UniVision.dll")]
        private static extern bool uniViewIsMouseInside(IntPtr hRealView, RvPoint point);
        [DllImport("UniVision.dll")]
        private static extern bool uniViewScreenToClient(IntPtr hRealView, RvPoint point, bool bReverse, ref RvPoint pResult);
        [DllImport("UniVision.dll")]
        private static extern void uniViewClearCanvas(IntPtr hRealView, bool bRedraw);
        [DllImport("UniVision.dll")]
        private static extern void uniViewSetOptionFlags(IntPtr hRealView, int flags);
        [DllImport("UniVision.dll")]
        private static extern int uniViewGetOptionFlags(IntPtr hRealView);
        //[DllImport("UniVision.dll")]
        //private static extern  void   uniCncviewSetTraceLayer(IntPtr hCncview, int layer);
        //[DllImport("UniVision.dll")] 
        //private static extern  int    uniCncviewGetTraceLayer(IntPtr hCncview);


        protected IntPtr m_hSight = IntPtr.Zero;
        protected GCHandle? m_handle = null;
        protected bool m_bAttached = false;

        protected EventProcDelegate m_eventProcDelegate = new EventProcDelegate(EventProcCallback);

        //public events         
        public event RealViewEvent OnUcobjSelectionChange;
        public event RealViewEvent OnSightRmouseClick;
        public event RealViewEvent OnSightLmouseClick;
        public event RealViewEvent OnSightLmouseDblclk;  //左鼠标双击
        public event RealViewEvent OnSightMouseWheel;  //左鼠标双击

        public event RealViewEvent OnWidgetHit;    //控件点击测试
        public event RealViewEvent OnWidgetUnhit;  //无点击测试

        // public event RealViewEvent OnSightRmouseDblclk;  //右鼠标双击



        //************************************************************************************************

        ~KRealView()
        {
            Destroy();

            m_handle?.Free();
        }

        public KRealView()
        {
            m_hSight = IntPtr.Zero;
        }
        public KRealView(IntPtr hView)
        {
            m_hSight = hView;
            if (m_hSight != IntPtr.Zero)
            {
                m_bAttached = true;
            }
        }
        public virtual bool Create(IntPtr hParent, string strName, int left, int top, int width, int height)
        {
            m_hSight = uniCreateView(hParent, strName, left, top, width, height);
            if (m_hSight != IntPtr.Zero)
            {
                SetEventProcCallback(m_eventProcDelegate);
            }

            return (m_hSight != IntPtr.Zero);
        }

        public virtual bool CreateEx(IntPtr hParent, string strName, int canvasWidth, int canvasHeight, RvRect rect, int flags)
        {
            m_hSight = uniCreateViewEx(hParent, strName, canvasWidth, canvasHeight, rect, flags);
            return (m_hSight != IntPtr.Zero);
        }

        public virtual void Destroy()
        {
            if (m_hSight != IntPtr.Zero && !m_bAttached)
            {
                uniDestroyView(m_hSight);
                m_hSight = IntPtr.Zero;
            }
        }





        public IntPtr GetHandle()
        {
            return m_hSight;
        }

        public void FromHandle(IntPtr h)
        {
            if (m_hSight != IntPtr.Zero)
            {
                throw new Exception("KRobot instance exists already!");
            }
            //Global.Assert(m_hSight == IntPtr.Zero, "KRealView句柄已经存在");

            m_hSight = h;
            m_bAttached = true;
        }

        public virtual bool RaiseViewEvents(IntPtr hObj, uint uEvent, IntPtr wParam, IntPtr lParam)
        {
            object sender = this;
            RvEventArgs args = new RvEventArgs();
            args.hObject = hObj;
            args.wParam = wParam;
            args.lParam = lParam;

            switch (uEvent)
            {
                case RV_EID_SELECT_CHANGED:
                    {
                        if (null != OnUcobjSelectionChange)
                        {
                            OnUcobjSelectionChange.Invoke(sender, args);
                            return true;
                        }
                    }
                    break;
                case RV_EID_RMOUSE_CLICK:
                    {
                        if (null != OnSightRmouseClick)
                        {
                            OnSightRmouseClick.Invoke(sender, args);
                            return true;
                        }
                    }
                    break;
                case RV_EID_LMOUSE_CLICK:
                    {
                        if (null != OnSightLmouseClick)
                        {
                            OnSightLmouseClick.Invoke(sender, args);
                            return true;
                        }
                    }
                    break;

                case RV_EID_LMOUSE_DBLCLK:
                    {
                        if (null != OnSightLmouseDblclk)
                        {
                            OnSightLmouseDblclk.Invoke(sender, args);
                            return true;
                        }
                    }
                    break;
                case RV_EID_MOUSE_WHEEL:
                    {
                        if (null != OnSightMouseWheel)
                        {
                            OnSightMouseWheel.Invoke(sender, args);
                            return true;
                        }
                    }
                    break;
                case RV_EID_UC_HIT:
                    {
                        if (null != OnWidgetHit)
                        {
                            OnWidgetHit.Invoke(sender, args);
                            return true;
                        }
                    }
                    break;
                case RV_EID_UC_UNHIT:
                    {
                        if (null != OnWidgetUnhit)
                        {
                            OnWidgetUnhit.Invoke(sender, args);
                            return true;
                        }
                    }
                    break;

            }


            return false;
        }

        private static bool EventProcCallback(IntPtr hDelegate, IntPtr hObj, uint uEvent, IntPtr wParam, IntPtr lParam, IntPtr pOwnerData)
        {
            if (pOwnerData == IntPtr.Zero)
            {
                return false;
            }

            GCHandle gch = GCHandle.FromIntPtr(pOwnerData);

            KRealView rv = (KRealView)gch.Target;

            return rv.RaiseViewEvents(hObj, uEvent, wParam, lParam);
        }


        public void Refresh(int type)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewRefresh(m_hSight, type);
            }

        }

        //public void SetTraceLayer(LAYER layer)
        //{
        //    if (m_hSight != IntPtr.Zero)
        //    {
        //        uniCncviewSetTraceLayer(m_hSight, (int)layer);
        //    }
        //}

        //public int GetTraceLayer(LAYER layer)
        //{
        //    if (m_hSight != IntPtr.Zero)
        //    {
        //        return uniCncviewGetTraceLayer(m_hSight );
        //    }

        //    return -1;
        //}


        public void SetOptionFlags(int flags)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetOptionFlags(m_hSight, flags);
            }
        }

        public int GetOptionFlags()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetOptionFlags(m_hSight);
            }
            return 0;
        }


        public bool SetSightFormat(int imageType, int width, int height, bool bRedraw)
        {
            if (m_hSight != IntPtr.Zero)
            {
                //这个函数必须刷新
                return uniViewSetSightFormat(m_hSight, imageType, width, height, true/*bRedraw*/);

            }
            return false;
        }


        public bool SetImage(KImage image, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                if (image.GetPixelFormat() != this.GetImageType() ||
                    image.GetWidth() != this.GetImageWidth() ||
                    image.GetHeight() != this.GetImageHeight())
                {
                    bool ret = uniViewSetSightFormat(m_hSight, (int)image.GetPixelFormat(), image.GetWidth(), image.GetHeight(), false);

                    if (!ret)
                    {
                        return false;
                    }
                }

                uniViewFeedImage(m_hSight, image.GetHandle());

                if (bRefresh)
                {
                    Refresh(KRealView.RT_REDRAW);
                }
                return true;
            }
            return false;
        }

        public void ClearFrame(bool bRedraw)
        {
            if (m_hSight != IntPtr.Zero)
            {
                //必须刷新
                uniViewClearFrame(m_hSight, true);
            }
        }

        public void ClearCanvas(bool bRedraw)
        {
            if (m_hSight != IntPtr.Zero)
            {
                //必须刷新
                uniViewClearCanvas(m_hSight, true  /*bRedraw*/);
            }
        }

        public PixelFormat GetImageType()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return (PixelFormat)uniViewGetImageType(m_hSight);
            }
            return PixelFormat.Unknown;
        }
        public int GetImageWidth()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetImageWidth(m_hSight);
            }
            return 0;
        }

        public int GetImageHeight()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetImageHeight(m_hSight);
            }
            return 0;
        }

        public void SetDisplayRect(int left, int top, int width, int height, bool refresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetDisplayRect(m_hSight, left, top, width, height, refresh);
            }
        }

        public void SetDisplaySize(int width, int height, bool refresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetDisplaySize(m_hSight, width, height, refresh);
            }
        }

        public GSize GetDisplaySize()
        {
            GSize sz = new GSize();
            if (m_hSight != IntPtr.Zero)
            {
                sz = uniViewGetDisplaySize(m_hSight);
            }
            return sz;
        }

        public GSize GetViewSize()
        {
            GSize sz = new GSize();
            if (m_hSight != IntPtr.Zero)
            {
                sz = uniViewGetViewSize(m_hSight);
            }
            return sz;
        }

        public void SetDesignMode(int mode, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetDesignMode(m_hSight, mode, bRefresh);
            }
        }

        public int GetDesignMode()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetDesignMode(m_hSight);
            }
            return 0;
        }

        public bool IsEditionLocked()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewIsEditionLocked(m_hSight);
            }
            return false;
        }

        public void SetEditionState(bool bLock, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetEditionState(m_hSight, bLock, bRefresh);
            }

        }


        public void SetUserCursorType(int cursorType, bool refresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetUserCursorType(m_hSight, cursorType, refresh);
            }
        }
        public int GetUserCursorType()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetUserCursorType(m_hSight);
            }
            return -1;
        }

        public void SetUserCursorSize(GSize size, bool refresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetUserCursorSize(m_hSight, size, refresh);
            }
        }

        public GSize GetUserCursorSize()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetUserCursorSize(m_hSight);
            }
            return new GSize();
        }

        public void SetUserCursorTransparence(int ratio, bool refresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetUserCursorTransparence(m_hSight, ratio, refresh);
            }
        }

        public int GetUserCursorTransparence()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetUserCursorTransparence(m_hSight);
            }
            return -1;
        }

        public void SetUserCursorColor(GRgb color, bool refresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetUserCursorColor(m_hSight, color, refresh);
            }
        }

        public GRgb GetUserCursorColor()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetUserCursorColor(m_hSight);
            }
            return new GRgb();
        }


        public int GetUserCursorLineType()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetUserCursorLineType(m_hSight);
            }
            return -1;
        }

        public void SetUserCursorThickness(int thickness, bool refresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetUserCursorThickness(m_hSight, thickness, refresh);
            }
        }

        public int GetUserCursorThickness()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetUserCursorThickness(m_hSight);
            }
            return -1;
        }

        public void SetUserCursorImage(IntPtr image, bool refresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetUserCursorImage(m_hSight, image, refresh);
            }
        }

        public void SetPixelBarFontSize(int size, bool refresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetPixelBarFontSize(m_hSight, size, refresh);
            }
        }

        public int GetPixelBarLabelFontSize()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetPixelBarLabelFontSize(m_hSight);
            }
            return -1;
        }

        public void SetPixelBarTransparence(int ratio, bool refresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetPixelBarTransparence(m_hSight, ratio, refresh);
            }
        }

        public int GetPixelBarTransparence()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetPixelBarTransparence(m_hSight);
            }
            return -1;
        }

        public void SetPixelBarColor(GRgb color, bool refresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetPixelBarColor(m_hSight, color, refresh);
            }
        }

        public GRgb GetPixelBarColor()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetPixelBarColor(m_hSight);
            }
            return new GRgb();
        }

        public void SetPixelBarVisible(bool flag, bool refresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetPixelBarVisible(m_hSight, flag, refresh);
            }
        }

        public bool IsPixelBarVisible()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewIsPixelBarVisible(m_hSight);
            }
            return false;
        }

        public void SetPixelBarText(string strText, bool refresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetPixelBarText(m_hSight, strText, refresh);
            }
        }

        public IntPtr GetPixelBarUc()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetPixelBarUc(m_hSight);
            }
            return IntPtr.Zero;
        }

        public IntPtr GetUserCursorUc()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetUserCursorUc(m_hSight);
            }
            return IntPtr.Zero;
        }

        public int GetRedrawType()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetRedrawType(m_hSight);
            }
            return -1;
        }

        public void SetRedrawType(int type, int rate)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetRedrawType(m_hSight, type, rate);
            }
        }



        public bool LoadCaliber(string strFilePath, bool bRedraw)
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewLoadCaliber(m_hSight, strFilePath, bRedraw);
            }
            return false;
        }

        public bool SaveCaliber(string strFilePath)
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewSaveCaliber(m_hSight, strFilePath);
            }
            return false;
        }

        public bool FeedData(IntPtr pImageData, UInt64 size)
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewFeedData(m_hSight, pImageData, size);
            }
            return false;
        }

        public bool FeedImage(IntPtr hImage)
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewFeedImage(m_hSight, hImage);
            }
            return false;
        }
        public bool FeedImage(KImage image)
        {
            if (null == image) return false;
            return FeedImage(image.GetHandle());
        }


        public bool ForceData(IntPtr pImageData, UInt64 size)
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewForceBuffer(m_hSight, pImageData, (uint)size);
            }
            return false;
        }

        public void SetFrameMip(int mip)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetFrameMip(m_hSight, mip);
            }
        }

        public bool ForceFrame(KImage image)
        {
            if (null == image) return false;

            if (m_hSight != IntPtr.Zero)
            {
                return uniViewForceFrame(m_hSight, image.GetHandle());
            }
            return false;

        }
        public bool ForceFrame(IntPtr hImage)
        {
            if (IntPtr.Zero == hImage) return false;

            if (m_hSight != IntPtr.Zero)
            {
                return uniViewForceFrame(m_hSight, hImage);
            }
            return false;

        }

        public void AddWidget(IntPtr hWidget, bool bRedraw)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewAddWidget(m_hSight, hWidget, bRedraw);
            }
        }

        public void AddWidget(KUcobj widget, bool bRedraw)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewAddWidget(m_hSight, widget.GetHandle(), bRedraw);
            }
        }

        public void RemoveWidget(IntPtr hWidget, bool bRedraw)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewRemoveWidget(m_hSight, hWidget, bRedraw);
            }
        }
        public void RemoveWidget(KUcobj widget, bool bRedraw)
        {
            if (null == widget) return;

            if (m_hSight != IntPtr.Zero)
            {
                uniViewRemoveWidget(m_hSight, widget.GetHandle(), bRedraw);
            }
        }
        public void RemoveWidget(uint id, bool bRedraw)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewRemoveWidgetE1(m_hSight, id, bRedraw);
            }
        }
        public void ClearWidget(bool bRedraw)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewClearWidget(m_hSight, bRedraw);
            }
        }

        public int GetWidgetCount()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetWidgetCount(m_hSight);
            }
            return 0;
        }

        public int GetSelectCount()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetSelectCount(m_hSight);
            }
            return 0;
        }

        public IntPtr GetWidgetAt(int index, bool bSelOnly)
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetWidgetAt(m_hSight, index, bSelOnly);
            }
            return IntPtr.Zero;
        }

        public IntPtr GetSelectAt(int index)
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetSelectAt(m_hSight, index);
            }
            return IntPtr.Zero;
        }

        public void SetBackgroundStyle(int style, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetBackgroundStyle(m_hSight, style, bRefresh);
            }
        }
        public UInt32 GetBackgroundStyle()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetBackgroundStyle(m_hSight);
            }
            return 0;
        }
        public void SetBackgroundColor(GRgb color, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetBackgroundColor(m_hSight, color, bRefresh);
            }
        }
        public GRgb GetBackgroundColor()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetBackgroundColor(m_hSight);
            }

            return new GRgb();
        }
        public void SetBlankColor(GRgb color, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetBlankColor(m_hSight, color, bRefresh);
            }

        }
        public GRgb GetBlankColor()
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewGetBlankColor(m_hSight);
            }
            return new GRgb();
        }

        public void SetViewMode(ViewMode mode, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetViewMode(m_hSight, mode, bRefresh);
            }
        }

        public ViewMode GetViewMode()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetViewMode(m_hSight);
            }

            return ViewMode.Default;
        }

        public int GetPosInCanvas(int pos, bool bVertical = false)
        {
            if (m_hSight != IntPtr.Zero)
            {
                if (bVertical)
                {
                    return (int)uniViewConvertYInCanvas(m_hSight, pos);
                }
                else
                {
                    return (int)uniViewConvertXInCanvas(m_hSight, pos);
                }
            }
            return pos;
        }

        public float GetPosInCanvas(float pos, bool bVertical = false)
        {
            if (m_hSight != IntPtr.Zero)
            {
                if (bVertical)
                {
                    return (float)uniViewConvertYInCanvas(m_hSight, pos);
                }
                else
                {
                    return (float)uniViewConvertXInCanvas(m_hSight, pos);
                }
            }
            return pos;
        }

        public int GetPosInDisplay(int pos, bool bVertical = false)
        {
            if (m_hSight != IntPtr.Zero)
            {
                if (bVertical)
                {
                    return (int)uniViewConvertYInDisplay(m_hSight, pos);
                }
                else
                {
                    return (int)uniViewConvertXInDisplay(m_hSight, pos);
                }
            }
            return pos;
        }

        public int GetSizeInCanvas(int size, bool bVertical = false)
        {
            if (m_hSight != IntPtr.Zero)
            {
                float scaleX = 1;
                float scaleY = 1;

                uniViewGetViewScale(m_hSight, ref scaleX, ref scaleY);

                if (!bVertical)
                {
                    size = (int)(size / scaleX);
                }
                else
                {
                    size = (int)(size / scaleY);
                }
            }

            return size;
        }

        public int GetSizeInDisplay(int size, bool bVertical = false)
        {
            if (m_hSight != IntPtr.Zero)
            {
                float scaleX = 1;
                float scaleY = 1;

                uniViewGetViewScale(m_hSight, ref scaleX, ref scaleY);

                if (!bVertical)
                {
                    size = (int)(size * scaleX);
                }
                else
                {
                    size = (int)(size * scaleY);
                }
            }

            return size;
        }


        public void GetViewScale(ref float scaleX, ref float scaleY)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewGetViewScale(m_hSight, ref scaleX, ref scaleY);
            }
        }

        public void SetViewScale(float scaleX, float scaleY, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetViewScale(m_hSight, scaleX, scaleY, bRefresh);
            }
        }

        public void SetViewLeftTop(int left, int top, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetViewLeftTop(m_hSight, left, top, bRefresh);
            }
        }

        public void GetViewLeftTop(ref int left, ref int top)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewGetViewLeftTop(m_hSight, ref left, ref top);
            }
        }

        public bool IsImageFeed()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewIsImageFeed(m_hSight);
            }

            return false;
        }

        public bool CopyImageTo(IntPtr hImage)
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewCopyImage(m_hSight, hImage);
            }

            return false;
        }
        public bool CopyImageTo(KImage image)
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewCopyImage(m_hSight, image.GetHandle());
            }

            return false;
        }

        public void SetRvbLogoPos(int location, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetRvbLogoPos(m_hSight, location, bRefresh);
            }

        }

        public int GetRvbLogoPos()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewGetRvbLogoPos(m_hSight);
            }

            return 0;

        }

        public void SetRvbLogoVisible(bool flag, bool bRefresh)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewSetRvbLogoVisible(m_hSight, flag, bRefresh);
            }

        }

        public bool IsRvbLogoVisible()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewIsRvbLogoVisible(m_hSight);
            }

            return false;
        }

        public RvPoint ConvertPointInCanvas(RvPoint point)
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewConvertPointInCanvas(m_hSight, point);
            }

            return new RvPoint();
        }
        public void Show(bool flag)
        {
            if (m_hSight != IntPtr.Zero)
            {
                uniViewShow(m_hSight, flag);
            }
        }
        public bool IsVisible()
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewIsVisible(m_hSight);
            }
            return false;
        }

        public double ConvertXInDisplay(double x)
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewConvertXInDisplay(m_hSight, x);
            }
            return 0;
        }

        public double ConvertYInDisplay(double y)
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewConvertYInDisplay(m_hSight, y);
            }
            return 0;
        }

        public double ConvertXInCanvas(double x)
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewConvertXInCanvas(m_hSight, x);
            }
            return 0;
        }

        public double ConvertYInCanvas(double y)
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewConvertYInCanvas(m_hSight, y);
            }
            return 0;
        }

        public void SetEventProcCallback(EventProcDelegate eventProcDelegate)
        {
            if (m_hSight != IntPtr.Zero)
            {
                if (m_handle == null)
                {
                    m_handle = GCHandle.Alloc(this);
                }

                GCHandle gc = (GCHandle)m_handle;

                uniViewSetEventProcCallback(m_hSight, eventProcDelegate, GCHandle.ToIntPtr(gc));
            }
        }

        public void ScaleCustomView(float scale, int x, int y, bool bPosInMid, bool bRefresh)
        {
            Pool.Assert(m_hSight != IntPtr.Zero);
            Pool.Assert(this.GetViewMode() == ViewMode.Custom);

            //保持鼠标位置
            if (bPosInMid)
            {
                //    int left = 0, top = 0;
                double prev_canv_x = 0;
                double prev_canv_y = 0;

                double new_disp_x = 0;
                double new_disp_y = 0;

                //   GetViewLeftTop(ref left, ref top);

                prev_canv_x = ConvertXInCanvas(x);
                prev_canv_y = ConvertYInCanvas(y);

                this.SetViewScale(scale, scale, false);

                new_disp_x = this.ConvertXInDisplay(prev_canv_x);
                new_disp_y = this.ConvertYInDisplay(prev_canv_y);

                SetViewLeftTop((int)((x - new_disp_x)), (int)((y - new_disp_y)), false);
            }
            else
            {
                this.SetViewScale(scale, scale, false);
                SetViewLeftTop(x, y, false);
            }


            if (bRefresh)
            {
                Refresh(RT_REDRAW);
            }
        }

        //当前当前自定义比例
        private float GetCurCustomScale(ref int x, ref int y)
        {
            if (GetViewMode() != ViewMode.Custom)
            {
                //如果当前视图模式不在自定义模式，需要调整当前的
                //x,y值
                //重新计算窗口坐标对应在自定义模式下的值窗口坐标值
                //1. 转成画布坐标
                double x_ = ConvertXInCanvas(x);
                double y_ = ConvertYInCanvas(y);

                //2. 切换到自定义模式
                float scale = 1.0f;
                this.GetViewScale(ref scale, ref scale);

                SetViewMode(ViewMode.Custom, false);
                SetViewLeftTop(0, 0, false);
                SetViewScale(scale, scale, false);

                //3. 重新计算窗口坐标
                x = (int)ConvertXInDisplay(x_);
                y = (int)ConvertYInDisplay(y_);
            }

            float n = 1.0f;

            GetViewScale(ref n, ref n);

            return n;

        }

        //缩小
        public void CustomZoomIn(int x, int y, bool bRefresh)
        {
            float n = GetCurCustomScale(ref x, ref y);
            if (n > MIN_CUSTOM_SCALE)
            {
                n *= 0.75f;  //n /2 
                ScaleCustomView(n, x, y, true, bRefresh);
            }

        }
        //放大
        public void CustomZoomOut(int x, int y, bool bRefresh)
        {
            float n = GetCurCustomScale(ref x, ref y);
            if (n < MAX_CUSTOM_SCALE)
            {
                n *= 1.25f; //2->1.25
                ScaleCustomView(n, x, y, true, bRefresh);
            }
        }

        public bool TranslateMessage(uint msgId, IntPtr wParam, IntPtr lParam)
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewTranslateMessage(m_hSight, msgId, wParam, lParam);
            }
            return false;
        }

        public bool IsMouseInside(RvPoint point)
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewIsMouseInside(m_hSight, point);
            }
            return false;
        }

        public bool ScreenToClient(RvPoint point, bool bReverse, ref RvPoint pResult)
        {
            if (m_hSight != IntPtr.Zero)
            {
                return uniViewScreenToClient(m_hSight, point, bReverse, ref pResult);
            }
            return false;
        }

        public void RelayoutCustomView(bool bRefresh)
        {
            if (m_hSight == IntPtr.Zero)
            {
                return;
            }

            GSize dispSize = this.GetDisplaySize();

            RvRect rect = new RvRect(0, 0, dispSize.sx - 1, dispSize.sy - 1);

            double scale = 1;

            int w = GetImageWidth();
            int h = GetImageHeight();

            RvRect bound = Smath.AdaptRect(rect, w, h, ref scale);

            if (scale <= 0) return;

            //if (w <= dispSize.sx && h <= dispSize.sy)
            //{
            //    m_nMinScale = 1;
            //    m_nMaxScale = 2;
            //    m_nCurScale = (float)Math.Min(m_nMaxScale, scale);
            //}
            //else
            //{
            //    m_nMinScale = (float)scale;
            //    m_nMaxScale = (float)(scale + m_nScaleDelta * 5);
            //    m_nCurScale = (float)m_nMinScale;
            //}

            this.SetViewScale((float)scale, (float)scale/*m_nCurScale, m_nCurScale*/, false);

            GSize viewsze = GetViewSize();

            //这里需要再仔细测试
            //默认居中对齐
            int left = (dispSize.sx - viewsze.sx) / 2;
            int top = (dispSize.sy - viewsze.sy) / 2;

            SetViewLeftTop(left, top, false);

            if (bRefresh)
            {
                Refresh(RT_REDRAW);
            }
        }
    }
}
