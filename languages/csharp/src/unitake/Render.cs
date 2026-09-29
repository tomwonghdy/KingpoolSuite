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
    using HSTROKE = IntPtr;
    using HPEN = IntPtr;
    using HBRUSH = IntPtr;
    using HFONT = IntPtr;
    using HMOLD = IntPtr;
    using HIMAGE = IntPtr;
    using HGDC = IntPtr;
    using HMASK = IntPtr;
    using HBLOB = IntPtr;
    using GColor = UInt32;
    using HDOCK = IntPtr;

    public enum ContextType
    {
        Generic = 0,  //无画布对象,用于实时播放。
        Enhanced,     //有画布对象， 也可以用于实时播放。
    };



    public enum ViewMode
    {
        Default = 0,
        Cender = 1,
        Stretch = 2,
        Zoom = 3,
        Custom = 10,
    }


    public enum CanvasLayer
    {
        Canvas = (0x02 << 16),
        Virtual = (0x04 << 16),
        Display = (0x08 << 16),
        Background = (0x01 << 16),
        Foreground = (Canvas | Virtual | Display),
        All = (Background | Foreground),
    }

    public enum CanvasBackStyle
    {
        Default = 0,
        Check = 1,
    }

    public enum LogoPlacement
    {
        LeftTop = 0,
        RightTop = 1,
        LeftBottom = 2,
        RightBottom = 3,
    }

    public enum LinePattern
    {
        Solid = 0,  // .....................
        Hidden = 1,    // ..  ..  ..  ..  ..  .
        Center = 2,   // .......  ..  ........
        Inclusion = 3,   // .....  ..  ..  ......
    }

    public enum FontFlags
    {
        // Font style flags
        Default = 0x0000,
        Bold = 0x0001,
        Italic = 0x0002,
        Underline = 0x0004,
        Strikeout = 0x0008,
        Outline = 0x0010,//空心字
        //Transparent = 0x0020,
    }
    //draw text align flags
    public enum TextAlign
    {
        Default = (Top | Left),
        Top = 0x01,
        Vcenter = 0x02,
        Bottom = 0x04,
        Left = (Top << 8),
        Hcenter = (Vcenter << 8),
        Right = (Bottom << 8),
        Middle = (Vcenter | Hcenter),
    }


    public enum BrushPattern
    {
        Solid = 0,
        BackwardDiagonal = 1, //Downward hatch   at 45 degrees(like '\')
        Cross = 2, //Horizontal and vertical crosshatch
        DiagonalCross = 3, //  Crosshatch at 45 degrees
        ForwardDiagonal = 4,// Upward hatch   at 45 degrees (like '/')
        HorizontalLine = 5, // Horizontal hatch
        VerticalLine = 6,// Vertical hatch
        Dod = 7,   // dot hatch
    }


    //矩形框里面绘制
    //用于rvgDrawImage 
    //mold type
    public enum MoldType
    {
        Default = 0,
        Align = 1,
        Rotate = 2,
        Stretch = 3, //拉伸
        Scale = 4,
        Skew = 5,
        Tile = 6,                    //平铺
        Flip = 7,                    //翻转
    }
    public enum TileType
    {
        None = 0,
        Horizontal = 2,
        Vertical = 1,
        Both = 3,
    }

    public enum FlipType
    {
        Never = 0,
        Horizontal = 2,
        Vertical = 1,
        Both = 3,
    }


    //mold alignment  type
    public enum MoldAlignType
    {
        Top = 0x01,
        Vcenter = 0x02,
        Bottom = 0x03,
        Left = 0x01,
        Hcenter = 0x02,
        Right = 0x03,
        Center = 0x02,
    }

    /**结构体**/
    [StructLayout(LayoutKind.Sequential)]
    public struct GSize
    {
        public int sx;
        public int sy;
        public GSize(int sx, int sy)
        {
            this.sx = sx;
            this.sy = sy;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GRgb
    {
        public static GRgb White = new GRgb(0xFF, 0xFF, 0xFF);
        public static GRgb Black = new GRgb(0x00, 0x00, 0x00);
        public static GRgb Grey = new GRgb(0x80, 0x80, 0x80);
        public static GRgb Blue = new GRgb(0x00, 0x00, 0xFF);
        public static GRgb Green = new GRgb(0x00, 0xFF, 0x00);
        public static GRgb Red = new GRgb(0xFF, 0x00, 0x00);

        public static GRgb Magenta = new GRgb(0x8B, 0x00, 0x8B);
        public static GRgb Brown = new GRgb(0xA5, 0x2A, 0x2A);
        public static GRgb Yellow = new GRgb(0xFF, 0xFF, 0x00);
        public static GRgb Cyan = new GRgb(0x00, 0xFF, 0xFF);

        public static GRgb DarkGrey = new GRgb(0x40, 0x40, 0x40);
        public static GRgb LightGrey = new GRgb(0xD3, 0xD3, 0xD3);
        public static GRgb LightBlue = new GRgb(0x80, 0x80, 0xFF);
        public static GRgb LightGreen = new GRgb(0x80, 0xFF, 0x80);

        public static GRgb LightCyan = new GRgb(0xFF, 0xFF, 0x80);
        public static GRgb LightRed = new GRgb(0xFF, 0x80, 0x80);
        public static GRgb LightMagenta = new GRgb(0xFF, 0x80, 0xFF);

        public byte red;
        public byte green;
        public byte blue;
        public byte alpha;

        public GRgb(byte a = 255)
        {
            red = 255; green = 255; blue = 255;
            alpha = a;
        }

        public GRgb(byte r, byte g, byte b)
        {
            red = r; green = g; blue = b;
            alpha = 255;
        }
        public GRgb(byte r, byte g, byte b, byte alpha)
        {
            red = r; green = g; blue = b;
            this.alpha = alpha;
        }

        public GRgb Invert()
        {
            GRgb clr;

            clr.alpha = this.alpha;
            clr.blue = (byte)(255 - this.blue);
            clr.green = (byte)(255 - this.green);
            clr.red = (byte)(255 - this.red);

            return clr;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GPoint
    {
        public int x;
        public int y;

        public GPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public GPoint(GPoint pt)
        {
            this.x = pt.x;
            this.y = pt.y;
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct GRect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public GRect(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }
        public GRect(int left, int top, uint width, uint height)
        {
            this.left = left;
            this.top = top;
            this.right = (int)(left + width);
            this.bottom = (int)(top + height);
        }

        public GRect(GPoint leftTop, GPoint rightBottom)
        {
            this.left = leftTop.x;
            this.top = leftTop.y;
            this.right = rightBottom.x;
            this.bottom = rightBottom.y;
        }
        public GRect(GPoint p1, GPoint p2, GPoint p3)
        {
            GPoint lt, rb;

            lt.x = Math.Min(Math.Min(p1.x, p2.x), p3.x);
            lt.y = Math.Min(Math.Min(p1.y, p2.y), p3.y);

            rb.x = Math.Max(Math.Max(p1.x, p2.x), p3.x);
            rb.y = Math.Max(Math.Max(p1.y, p2.y), p3.y);

            this.left = lt.x;
            this.top = lt.y;
            this.right = rb.x;
            this.bottom = rb.y;
        }


    }

    //[StructLayout(LayoutKind.Sequential)]
    //public struct GPen
    //{
    //    public int size;
    //    public GRgb color;
    //    public int style;
    //}
    //;
    //[StructLayout(LayoutKind.Sequential)]
    //public struct GBrush
    //{
    //    public int size; // default 9
    //    public int type;
    //    public GRgb color;
    //}

    [StructLayout(LayoutKind.Sequential)]
    public struct GDock
    {
        public GSize srcSize;  //原来的画布大小
        public GRect dstRect;  //目标显示区域
    }
    //[StructLayout(LayoutKind.Sequential)]
    //public struct GFont
    //{
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = (Render.MAX_FONT_NAME_LEN + 1))]
    //    string strFaceName/*[RV_MAX_FONT_NAME_LEN + 1]*/;  //字体名称
    //    int fontSize;   //字体大小
    //    int style;  //字体风格：加粗，正常，斜体，等.

    //    int refcnt; //引用计数 
    //    IntPtr hFontManager;
    //}


    //erase type
    public enum EraseType
    {
        Hide = 0,
        Delete = -1,
        Restore = 1,
    }

    //modify stroke type
    public enum ModifyType
    {
        Transparence = 0x0001,
        Color = 0x0002,
        Image = 0x0020,
        Text = 0x0030,
    }


    public class Render
    {
        //Graphical Device Context(GDC) Style
        public const int GS_ANTIALIAS = 0x0001;
        public const int GS_LINE_QUALITY = 0x0002; //最好质量
        public const int GS_TRANSPARENT = 0x0004;
        public const int GS_TEXTURE_QULITY = 0x0008;  //纹理质量
        public const int GS_USE_MIP = 0x0010; //MIP for texture
        public const int GS_INVISIBLE_CHAR = 0x0020;//show  a space for an invisible charater(空格)   



        private const string DEFAULT_FONT_NAME = "Arial";
        private const int DEFAULT_FONT_SIZE = 11;

        //Multiline flags
        //在多行情况下，所有的换行符都会换行。
        //当一行长度大于指定区域长度的时候，
        //可以进行字符或单词换行，当同时指定两种换行方式的时候，
        //单词换行优先
        public const int DT_MULTILINE = (0x01 << 16); //default linefeed break
        public const int DT_WORDBREAK = (0x02 << 16); //
        //public const int DT_CHARBREAK = (0x04 << 16);
        public const int DT_TRANSPARENT = (0x08 << 16);
        //public const int DT_NOCLIP = (0x10 << 16); //不剪裁

        //忽略颜色，图像绘制
        public static GRgb IGNORE_COLOR = new GRgb(255, 255, 255, 255);
        public const int MAX_FONT_NAME_LEN = 63;

        //DrawTriangles: 绘制多个非连续的填充三角形,每条三角形需要使用三个点（GPoint），
        //不同类型的三角形，需要的点数量不一样
        public const int DT_SEPARATE = 0;
        public const int DT_FAN = 1;
        public const int DT_STRIP = 2;

#if DEBUGGING_KINGPOOL_SUITE
        private const string LIB_NAME = "xgui_d.dll";
#else
        private const string LIB_NAME  = "xgui.dll"; 
#endif



        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgCreateBrush")]
        public static extern HBRUSH CreateBrush(BrushPattern type /*= RV_BT_SOLID*/, int size, GRgb color);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDestroyBrush")]
        public static extern void DestroyBrush(HBRUSH hBrush);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgCreatePen")]
        public static extern HPEN CreatePen(GRgb color/* = GDC_BLACK*/, int size /*= 1*/, LinePattern style /*= RV_PS_SOLID*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDestroyPen")]
        public static extern void DestroyPen(HPEN hPen);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetPenColor")]
        public static extern void SetPenColor(HPEN hPen, GRgb color);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetPenColor")]
        public static extern GRgb GetPenColor(HPEN hPen);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetPenSize")]
        public static extern void SetPenSize(HPEN hPen, int size);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetPenSize")]
        public static extern int GetPenSize(HPEN hPen);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetPenPattern")]
        public static extern void SetPenPattern(HPEN hPen, int pattern);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetPenPattern")]
        public static extern int GetPenPattern(HPEN hPen);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetBrushColor")]
        public static extern void SetBrushColor(HBRUSH hBrush, GRgb color);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetBrushColor")]
        public static extern GRgb GetBrushColor(HBRUSH hBrush);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetBrushSize")]
        public static extern void SetBrushSize(HBRUSH hBrush, int size);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetBrushSize")]
        public static extern int GetBrushSize(HBRUSH hBrush);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetBrushPattern")]
        public static extern void SetBrushPattern(HBRUSH hBrush, int pattern);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetBrushPattern")]
        public static extern int GetBrushPattern(HBRUSH hBrush);

        //创建GDC可能失败的原因:
        //1  没有licence.dvl文件，或许可文件过期，失效
        //2  opengl(directx9)初始化失败
        //3  创建画布失败
        //4  传入无效的hwnd
        //5  创建DSM等对象失败
        //
        // opengl 1.1需要画布大小为2的n次方
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgCreateContext")]
        public static extern IntPtr CreateContext(ContextType type, IntPtr hBindWnd, int canvasWidth /*= -1*/, int canvasHeight/* = -1*/);
        //上层程序在应在窗口销毁之前，调用rvgDestroyContext。
        //否则会陷入死循环,造成死锁。虽然不是所有操作系统都如此，
        //但还是强烈建议在窗口销毁之前，释放IntPtr。
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDestroyContext")]
        public static extern void DestroyContext(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetDrawStyle")]
        public static extern void SetDrawStyle(HGDC dc, int flag);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetDrawStyle")]
        public static extern int GetDrawStyle(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgClear")]
        public static extern void Clear(HGDC dc, CanvasLayer/*int*/ layers/* = RV_ALL_LAYER*/, bool bRefresh = false);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgErase")]
        public static extern void Erase(HGDC dc, HSTROKE hStroke, EraseType type, bool bRefresh = false);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgRealize")]
        public static extern void Realize(HGDC dc, CanvasLayer /*int*/ layers /*= RV_ALL_LAYER*/, bool bRefresh = false);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgFlush")]
        public static extern bool Flush(HGDC dc/*, bool bRefresh=false*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgArrangeText")]

        public static extern void ArrangeText(HGDC dc, int charSpace, int lineGap);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetGraphAccuracy")]
        //0, 0.1, 0.01, 0.001, etc
        public static extern void SetGraphAccuracy(HGDC dc, float accuracy);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetGraphAccuracy")]
        public static extern float GetGraphAccuracy(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgModify")]

        public static extern void Modify(HGDC dc, HSTROKE hStroke, ModifyType type, UIntPtr value, bool bRefresh = false);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgOffset")]
        //移动画笔
        public static extern void Offset(HGDC dc, HSTROKE hStroke, float dx, float dy, bool bRefresh = false);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgHide")]
        //隐藏画笔
        public static extern void Hide(HGDC dc, HSTROKE hStroke, bool flag, bool bRefresh = false);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetRvbLogoVisible")]
        //免费版不能隐藏RVB图标 since v5.1
        public static extern void SetRvbLogoVisible(HGDC dc, bool flag);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgIsRvbLogoVisible")]
        public static extern bool IsRvbLogoVisible(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetRvbLogoPlacement")]
        public static extern void SetRvbLogoPlacement(HGDC dc, LogoPlacement placement);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetRvbLogoPlacement")]
        public static extern LogoPlacement GetRvbLogoPlacement(HGDC dc);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetContextType")]
        public static extern ContextType GetContextType(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgIsFullScreen")]
        public static extern bool IsFullScreen(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetDisplayRect")]
        public static extern GRect GetDisplayRect(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetClientRect")]
        //画布在窗口中的有效区域（不是实际画布大小，是窗口中画布可见的大小），可能小于窗口区域
        public static extern GRect GetClientRect(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgResizeDisplayRect")]
        public static extern void ResizeDisplayRect(HGDC dc, int width, int height, bool bRefresh);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetCanvasSizeEx")]
        // opengl 1.1需要画布大小为2的n次方
        public static extern bool SetCanvasSizeEx(HGDC dc, RvSize size, bool bRefresh = false);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetCanvasSize")]
        public static extern bool SetCanvasSize(HGDC dc, int width, int height, bool bRefresh = false);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetCanvasDepth")]
        //深度目前仅支持8,24,32
        public static extern bool SetCanvasDepth(HGDC dc, int depth, bool bRefresh = false);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetCanvasDepth")]
        public static extern int GetCanvasDepth(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetCanvasWidth")]
        public static extern int GetCanvasWidth(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetCanvasHeight")]
        public static extern int GetCanvasHeight(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetCanvasSize")]
        public static extern RvSize GetCanvasSize(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetBlankColor")]
        public static extern void SetBlankColor(HGDC dc, GRgb color);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetBlankColor")]
        public static extern GRgb GetBlankColor(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetBlankImage")]
        //有空白背景图优先空白背景图，否则使用空白颜色填充
        public static extern void SetBlankImage(HGDC dc, HIMAGE image);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetBlankImage")]
        //外部不可释放返回的RvImage
        public static extern HIMAGE/*RvImage*/  GetBlankImage(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetViewPos")]
        //放大后视图位置
        private static extern void rvgSetViewPos(HGDC dc, int pos, bool bVertical);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetViewPosE1")]
        private static extern void rvgSetViewPosE1(HGDC dc, int x, int y);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetViewPos")]
        public static extern GPoint GetViewPos(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetViewPosE1")]
        public static extern int GetViewPosE1(HGDC dc, bool bVertical);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetDisplayLeftTop")]
        //显示层的左上角点
        public static extern void SetDisplayLeftTop(HGDC dc, int x, int y);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetDisplayLeftTop")]
        public static extern GPoint GetDisplayLeftTop(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetViewDelta")]
        private static extern int rvgGetViewDelta(HGDC dc, bool bVertical);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetViewDeltaE1")]
        private static extern void rvgGetViewDeltaE1(HGDC dc, ref int dx, ref int dy);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetViewDeltaEx")]
        public static extern GSize GetViewDeltaEx(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetViewScale")]

        public static extern void SetViewScale(HGDC dc, float scale, bool bScaleY = false);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetViewScaleEx")]
        public static extern void SetViewScaleEx(HGDC dc, float scaleX, float scaleY);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetViewScale")]
        public static extern float GetViewScale(HGDC dc, bool bScaleY = false);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetViewScaleEx")]
        public static extern void GetViewScaleEx(HGDC dc, ref float pScaleX, ref float pScaleY);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetViewRect")]
        public static extern void SetViewRect(HGDC dc, GRect rect);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetViewRect")]
        //可以看到的画布区域（不是实际的可见画布大小，可能放大或缩小）
        public static extern GRect GetViewRect(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetViewSize")]
        public static extern GSize GetViewSize(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetViewMode")]
        public static extern void SetViewMode(HGDC dc, ViewMode mode);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetViewModeEx")]
        public static extern void SetViewModeEx(HGDC dc, ViewMode mode, IntPtr pParam /*= IntPtr.Zero*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetViewMode")]
        public static extern ViewMode GetViewMode(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgMoveTo")]
        public static extern void MoveTo(HGDC dc, int x, int y);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgMoveToEx")]
        public static extern void MoveToEx(HGDC dc, GPoint pos);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgLineTo")]
        public static extern void LineTo(HGDC dc, int x, int y);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgLineToEx")]
        public static extern void LineToEx(HGDC dc, GPoint pos);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawRectEx")]
        //graphical shapes//
        public static extern HSTROKE DrawRectEx(HGDC dc, float left, float top, float right, float bottom, float rotate, bool bFill /*= false*/, HSTROKE hStroke/* = NULL*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawRectE1")]
        public static extern HSTROKE DrawRectE1(HGDC dc, int left, int top, int right, int bottom, float rotate, bool bFill/* = false*/, HSTROKE hStroke/* = NULL*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawRect")]
        public static extern HSTROKE DrawRect(HGDC dc, int left, int top, int right, int bottom, bool bFill /*= false*/, HSTROKE hStroke/* = NULL*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawLine")]
        public static extern HSTROKE DrawLine(HGDC dc, int x0, int y0, int x1, int y1, HSTROKE hStroke /*= IntPtr.Zero*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawLineEx")]
        public static extern HSTROKE DrawLineEx(HGDC dc, GPoint p0, GPoint p1, HSTROKE hStroke /*= IntPtr.Zero*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawPoint")]
        public static extern HSTROKE DrawPoint(HGDC dc, int x, int y, HSTROKE hStroke /*= IntPtr.Zero*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawPointEx")]
        public static extern HSTROKE DrawPointEx(HGDC dc, GPoint pos, HSTROKE hStroke /*= IntPtr.Zero*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawCircle")]
        public static extern HSTROKE DrawCircle(HGDC dc, int x0, int y0, int radius, bool bFill, HSTROKE hStroke /*= IntPtr.Zero*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawCircleEx")]
        public static extern HSTROKE DrawCircleEx(HGDC dc, GPoint center, int radius, bool bFill, HSTROKE hStroke /*= IntPtr.Zero*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawEllipse")]
        public static extern HSTROKE DrawEllipse(HGDC dc, int x0, int y0, int rx, int ry, float angle /*= 0.0f*/, bool bFill /*= false*/, HSTROKE hStroke /*= IntPtr.Zero*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawEllipseEx")]
        public static extern HSTROKE DrawEllipseEx(HGDC dc, GPoint center, int rx, int ry, float angle /*= 0.0f*/, bool bFill /*= false*/, HSTROKE hStroke /*= IntPtr.Zero*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawCross")]
        //画十字线
        public static extern HSTROKE DrawCross(HGDC dc, int cx, int cy, int sx, int sy, float angle /*= 0.0f*/, HSTROKE hStroke /*= IntPtr.Zero*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawTriangle")]
        //画等边三角形
        public static extern HSTROKE DrawTriangle(HGDC dc, int cx, int cy, int size, float angle /*= 0.0f*/, bool bFill /*= false*/, HSTROKE hStroke /*= IntPtr.Zero*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawArcEx")]
        public static extern HSTROKE DrawArcEx(HGDC dc, GPoint center, int radius, float startAngle, float endAngle, bool bSector /*= false*/, HSTROKE hStroke /*= IntPtr.Zero*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawArc")]
        public static extern HSTROKE DrawArc(HGDC dc, int x0, int y0, int radius, float startAngle, float endAngle, HSTROKE hStroke/*= NULL*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawPolygon")]
        private static extern HSTROKE rvgDrawPolygon(HGDC dc, IntPtr/*const GPoint**/ pVertexSet, int nVertexCount, bool bFill, HSTROKE hStroke/* = NULL*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawPolyline")]
        public static extern HSTROKE DrawPolyline(HGDC dc, IntPtr/*const GPoint**/ pVertexSet, int nVertexCount, HSTROKE hStroke/* = NULL*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawPolylineE1")]
        //float type 
        public static extern HSTROKE DrawPolylineE1(HGDC dc, IntPtr/*= const RvPoint_f**/ pVertexSet, int nVertexCount, HSTROKE hStroke);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawPolygonE1")]
        private static extern HSTROKE rvgDrawPolygonE1(HGDC dc, IntPtr/*= const RvPoint_f**/ pVertexSet, int nVertexCount, bool bFill, HSTROKE hStroke);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawLineE1")]
        public static extern HSTROKE DrawLineE1(HGDC dc, float x0, float y0, float x1, float y1, HSTROKE hStroke);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawArcE1")]
        public static extern HSTROKE DrawArcE1(HGDC dc, float cx, float cy, float radius, float startAngle, float endAngle, HSTROKE hStroke);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawCircleE1")]
        public static extern HSTROKE DrawCircleE1(HGDC dc, float cx, float cy, float radius, bool bFill, HSTROKE hStroke);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawDotsE1")]
        private static extern HSTROKE DrawDotsE1(HGDC dc, IntPtr/*= const RvPoint_f**/ pPointArray, int nPointCount, HSTROKE hStroke);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawSegmentsE1")]
        public static extern HSTROKE DrawSegmentsE1(HGDC dc, IntPtr /*= const RvPoint_f**/ pPointArray, int nPointCount, HSTROKE hStroke);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawCrossE1")]
        public static extern HSTROKE DrawCrossE1(HGDC dc, float cx, float cy, float sx, float sy, float angle, HSTROKE hStroke);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawPointE1")]
        public static extern HSTROKE DrawPointE1(HGDC dc, float x, float y, HSTROKE hStroke);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawEllipseE1")]
        public static extern HSTROKE DrawEllipseE1(HGDC dc, float cx, float cy, float rx, float ry, float angle, bool bFill, HSTROKE hStroke);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawTextEx")]
        //如pRect为NULL, nFormat将自动忽略
        private static extern HSTROKE DrawTextEx(HGDC dc, byte[] strText, GRect rect, int nFormat /*= RV_DT_DEFAULT*/, int lineSpace /*= 0*/, HSTROKE hStroke/* = NULL*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawText")]
        private static extern HSTROKE DrawText(HGDC dc, byte[] strText, int x, int y, HSTROKE hStroke/* = NULL*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawTextE1")]
        private static extern HSTROKE DrawTextE1(HGDC dc, byte[] strText, GRect rect, HSTROKE hStroke/* = NULL*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetTextSize")]
        private static extern GSize GetTextSize(HGDC dc, byte[] strText, int count = -1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgTextOut")]
        private static extern void rvgTextOut(HGDC dc, byte[] strText);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgTextOutEx")]
        private static extern void rvgTextOutEx(HGDC dc, byte[] strText, int fontSize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawImage")]
        public static extern HSTROKE DrawImage(HGDC dc, HIMAGE image, HSTROKE hStroke/* = NULL*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawImageEx")]
        public static extern HSTROKE DrawImageEx(HGDC dc, HIMAGE image, int x, int y, int width /*= -1*/, int height/* = -1*/, HSTROKE hStroke/* = NULL*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawImageE1")]
        public static extern HSTROKE DrawImageE1(HGDC dc, HIMAGE image, IntPtr vertexArray/*RvPoint_f vertexArray[4]*/, HSTROKE hStroke/* = NULL*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawImageE2")]
        public static extern HSTROKE DrawImageE2(HGDC dc, HIMAGE image, /*int x, int y,*/ GRect rect, HSTROKE hStroke/* = NULL*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawMask")]
        public static extern HSTROKE DrawMask(HGDC dc, HMASK /*RvMask*/ mask, int dx, int dy, HSTROKE hStroke/* = NULL*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawMaskEx")]
        //public static extern HSTROKE DrawMaskEx(HGDC dc, HMASK/*RvMask*/ mask, float offsetx, float offsety, float angle, HSTROKE hStroke/* = NULL*/);
        //[DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawMatrix")]
        public static extern HSTROKE DrawMatrix(HGDC dc, IntPtr/*const RvMatrix*/ matrix, HSTROKE hStroke/* = NULL*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawBlob")]
        public static extern HSTROKE DrawBlob(HGDC dc, HBLOB /*const RvBlob*/ blob, int dx /*= 0*/, int dy /*= 0*/ , HSTROKE hStroke/* = NULL*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawContour")]
        public static extern HSTROKE DrawContour(HGDC dc, IntPtr/*const RvContour*/ contour, int dx /*= 0*/, int dy /*= 0*/, HSTROKE hStroke/* = NULL*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawDots")]
        //绘制多个的点
        private static extern HSTROKE DrawDots(HGDC dc, IntPtr/*GPoint**/ pPointArray, int nPointCount, HSTROKE hStroke/* = NULL*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawSegments")]
        //绘制多个非连续的线段,每条线段使用两个点（GPoint）
        public static extern HSTROKE DrawSegments(HGDC dc, IntPtr/*GPoint**/ pPointArray, int nPointCount, HSTROKE hStroke/* = NULL*/);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawTriangles")]
        private static extern HSTROKE DrawTriangles(HGDC dc, IntPtr/*GPoint**/ pPointArray, int nPointCount, int type/* = RV_DT_SEPARATE*/, HSTROKE hStroke/* = NULL*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawTrianglesE1")]
        private static extern HSTROKE DrawTrianglesE1(HGDC dc, IntPtr/*RvPoint_f**/ pPointArray, int nPointCount, int type/* = RV_DT_SEPARATE*/, HSTROKE hStroke/* = NULL*/);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawBox2D")]
        public static extern HSTROKE DrawBox2D(HGDC dc, RvBox2D box, HSTROKE hStroke/* = NULL*/);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDrawDummy")]
        //绘制哑元对象，有笔画对象产生，但无实际绘制。
        public static extern HSTROKE DrawDummy(HGDC dc, HSTROKE hStroke/* = NULL*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgBitBlt")]
        //以下函数没有更新，慎用@240130
        public static extern void BitBlt(HGDC dc, HIMAGE image, int canvasLeft /*= 0*/, int canvasTop /*= 0*/, int canvasWidth /*= -1*/, int canvasHeight /*= -1*/,
                                        int imageLeft /*= 0*/, int imageTop /*= 0*/, int imageWidth /*= -1*/, int imageHeight /*= -1*/, GColor ignore /*= -1*/);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgPaintImage")]
        public static extern void PaintImage(HGDC dc, HIMAGE image, int left /*= 0*/, int top /*= 0*/);
        //end of @240130
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgFeedFrame")]
        //该函数主要用于实时图像显示,多线程可以调用，但需要加锁才安全。
        public static extern bool FeedFrame(HGDC dc, HIMAGE image);
        //[DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDirectFeed")]
        ////速度更快,仅限于在界面线程下调用,非UI线程会产生其它意象不到的异常。
        //public static extern bool DirectFeed(HGDC dc, HIMAGE image);
        //[DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgIsFrameFed")]
        //public static extern bool IsFrameFed(HGDC dc);
        //[DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgHasNewFrame")]
        //public static extern bool HasNewFrame(HGDC dc);
        //[DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetFramePixel")]
        ////仅支持RV_CT_ENHANCED画布@240130
        //public static extern bool GetFramePixel(HGDC dc, int x, int y, ref GRgb pColor);
        //[DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetCurFrame")]
        //public static extern bool GetCurFrame(HGDC dc, HIMAGE /*RvImage*/ image);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgCreateFont")]
        //"Arial" , 12, RV_RGB(0,0,0), RV_RGB(255,255,255), RV_FS_DEFAULT
        private static extern HFONT/*GFont**/ CreateFont(/*HGDC dc,*/ byte[] strFamilyName, int size, /*GColor textColor, GColor backColor, */ int flags);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDestroyFont")]
        public static extern void DestroyFont(HFONT/*GFont* */pFont);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgCreateFontEx")]
        private static extern HFONT rvgCreateFontEx(byte[] strFamilyName, int fontSize, GRgb textColor, GRgb backColor, int flags);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetFontTextColor")]
        public static extern void rvgSetFontTextColor(HFONT hFont, GRgb color);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetFontBackColor")]
        public static extern void rvgSetFontBackColor(HFONT hFont, GRgb color);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetFontTextColor")]
        public static extern GRgb rvgGetFontTextColor(HFONT hFont);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetFontBackColor")]
        public static extern GRgb rvgGetFontBackColor(HFONT hFont);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetCurPen")]
        public static extern HPEN/*GPen**/  GetCurPen(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetCurBrush")]
        public static extern HBRUSH /*GBrush**/  GetCurBrush(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetCurFont")]
        public static extern HFONT/*GFont**/  GetCurFont(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetCurMold")]
        public static extern HMOLD/*GMold**/  GetCurMold(HGDC dc);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSelectPen")]
        //drawing objects
        public static extern HPEN/*GPen**/  SelectPen(HGDC dc, HPEN/*GPen**/ hPen);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSelectBrush")]
        public static extern HBRUSH/*GBrush**/ SelectBrush(HGDC dc, HBRUSH/*GBrush**/ hBrush);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSelectFont")]
        public static extern HFONT/*GFont* */ SelectFont(HGDC dc, HFONT/*GFont**/ pFont);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSelectMold")]
        public static extern HMOLD/*GMold**/  SelectMold(HGDC dc, HMOLD/*GMold**/ pModel);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetCurrentLayer")]
        public static extern int SetCurrentLayer(HGDC dc, int nLayer);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetCurrentLayer")]
        public static extern int GetCurrentLayer(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetTrasparence")]
        //透明度在0~1之间, 0表示不透明， 1表示全透明
        public static extern float SetTrasparence(IntPtr self, float rate);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetTrasparence")]
        public static extern float GetTrasparence(IntPtr self);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetDock")]
        public static extern HDOCK/*GDock**/  SetDock(IntPtr self, HDOCK /*GDock* */pDock);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetDock")]
        public static extern HDOCK/*GDock**/ GetDock(IntPtr self);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgResetCanvas")]
        public static extern void ResetCanvas(HGDC dc, bool bRefresh = false);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetCanvasBackColor")]
        public static extern void SetCanvasBackColor(HGDC dc, GColor color);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetCanvasBackColor")]
        public static extern GColor GetCanvasBackColor(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetBackgroundColor")]
        public static extern void SetBackgroundColor(HGDC dc, GRgb color);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetBackgroundColor")]
        public static extern GRgb GetBackgroundColor(HGDC dc);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetCanvasBackStyle")]
        public static extern void SetCanvasBackStyle(HGDC dc, int style);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetCanvasBackStyle")]
        public static extern int GetCanvasBackStyle(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetCanvasGridSize")]
        public static extern void SetCanvasGridSize(HGDC dc, int sx, int sy);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetCanvasGridSize")]
        public static extern RvSize GetCanvasGridSize(HGDC dc);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgExportCanvas")]
        public static extern HIMAGE/*RvImage*/ ExportCanvas(HGDC dc, HIMAGE/*RvImage*/ image);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetDriverVersion")]
        //获得GL版本，版本由主版本和次版本组成，格式为 (main_version << 16) | sub_version
        //即前两字节为主版本，后两字节为次版本。
        public static extern int GetDriverVersion(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgSetSurfaceSize")]
        //表面相关
        public static extern void SetSurfaceSize(HGDC dc, int size);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetSurfaceSize")]
        public static extern int GetSurfaceSize(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgGetMaxSurfaceSize")]
        public static extern int GetMaxSurfaceSize(HGDC dc);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgCreateMold")]
        private static extern HMOLD/*GMold**/  CreateMold(MoldType type, IntPtr dwPrime, IntPtr dwMinor);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgDestroyMold")]
        public static extern void DestroyMold(HMOLD/*GMold**/ pMold);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgConvertXToDisplay")]
        public static extern int ConvertXToDisplay(HGDC dc, int value);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgConvertYToDisplay")]
        public static extern int ConvertYToDisplay(HGDC dc, int value);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgConvertXToDisplayE1")]
        //保证精度
        public static extern double ConvertXToDisplayE1(HGDC dc, double value);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgConvertYToDisplayE1")]
        public static extern double ConvertYToDisplayE1(HGDC dc, double value);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgConvertPointToDisplay")]
        public static extern GPoint ConvertPointToDisplay(HGDC dc, int x, int y);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgConvertPointToDisplayEx")]
        public static extern GPoint ConvertPointToDisplayEx(HGDC dc, ref GPoint pPoint);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgConvertRectToDisplay")]
        public static extern GRect ConvertRectToDisplay(HGDC dc, int left, int top, int right, int bottom);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgConvertRectToDisplayEx")]
        public static extern GRect ConvertRectToDisplayEx(HGDC dc, ref GRect pRect);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgConvertPointToDisplayE1")]
        public static extern RvPointF32/*RvPoint_f*/  ConvertPointToDisplayE1(HGDC dc, float x, float y);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgConvertXToCanvas")]
        //layer为原值所在层
        public static extern int ConvertXToCanvas(HGDC dc, int value);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgConvertYToCanvas")]
        public static extern int ConvertYToCanvas(HGDC dc, int value);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgConvertXToCanvasE1")]
        //高精度转换
        public static extern double ConvertXToCanvasE1(HGDC dc, double value);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgConvertYToCanvasE1")]
        public static extern double ConvertYToCanvasE1(HGDC dc, double value);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgConvertPointToCanvas")]
        public static extern GPoint ConvertPointToCanvas(HGDC dc, int x, int y);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgConvertPointToCanvasE1")]
        public static extern RvPointF32/*RvPoint_f*/  ConvertPointToCanvasE1(HGDC dc, float x, float y);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgConvertPointToCanvasEx")]
        public static extern GPoint ConvertPointToCanvasEx(HGDC dc, ref GPoint pPoint);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgConvertRectToCanvas")]
        public static extern GRect ConvertRectToCanvas(HGDC dc, int left, int top, int right, int bottom);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgConvertRectToCanvasEx")]
        public static extern GRect ConvertRectToCanvasEx(HGDC dc, ref GRect pRect);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgCombine")]
        //combined strokes, 同时删除原来数组中单个笔画对象。
        private static extern HSTROKE Combine(HGDC dc, IntPtr/*HSTROKE* */hStrokeArray, int nStrokeCount/*, HSTROKE hResult*/);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvgUncombine")]
        //拆分联合的 strokes成独立单个的stroke（输出到hResultArray）,
        //如果成功，删除原来的stroke
        private static extern int Uncombine(HGDC dc, HSTROKE hStroke, IntPtr/*HSTROKE**/ hResultArray, int nArraySize/*,bool bCreateDummy=false, bool bEraseOriginal = false*/);

        //generic mold
        public static HMOLD/*GMold**/  CreateMold(MoldType type, int prime, int minor)
        {
            IntPtr ptr = new IntPtr(prime);
            IntPtr ptr2 = new IntPtr(minor);

            return CreateMold(type, ptr, ptr2);
        }

        //flip mold
        public static HMOLD/*GMold**/  CreateFlipMold(int type)
        {
            IntPtr ptr = new IntPtr(type);
            IntPtr ptr2 = new IntPtr(0);

            return CreateMold(MoldType.Flip, ptr, ptr2);
        }

        //align mold
        public static HMOLD/*GMold**/  CreateAlignMold(int horizonAlign, int verticalAlign)
        {
            IntPtr ptr = new IntPtr(horizonAlign);
            IntPtr ptr2 = new IntPtr(verticalAlign);

            return CreateMold(MoldType.Align, ptr, ptr2);
        }

        //rotate mold
        public static HMOLD/*GMold**/  CreateRotateMold(float angle, bool bKeepSize)
        {
            IntPtr ptr = Marshal.AllocHGlobal(sizeof(float));
            Marshal.StructureToPtr(angle, ptr, false);

            IntPtr ptr2 = new IntPtr(bKeepSize ? 1 : 0);

            HMOLD h = CreateMold(MoldType.Rotate, ptr, ptr2);

            Marshal.FreeHGlobal(ptr);

            return h;
        }
        //strech mold
        public static HMOLD/*GMold**/  CreateStretchMold(bool bKeepRatio, bool bKeepSize)
        {
            IntPtr ptr = new IntPtr(bKeepRatio ? 1 : 0);
            IntPtr ptr2 = new IntPtr(bKeepSize ? 1 : 0);

            return CreateMold(MoldType.Stretch, ptr, ptr2);
        }
        //scale mold
        public static HMOLD/*GMold**/  CreateScaleMold(float scaleX, float scaleY, bool bCentered)
        {
            RvScalarF32 scalar = new RvScalarF32(scaleX, scaleY);

            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RvScalarF32)));
            Marshal.StructureToPtr(scalar, ptr, false);

            IntPtr ptr2 = new IntPtr(bCentered ? 1 : 0);

            HMOLD h = CreateMold(MoldType.Scale, ptr, ptr2);

            Marshal.FreeHGlobal(ptr);

            return h;
        }
        //skew mold
        public static HMOLD/*GMold**/  CreateSkewMold(RvPoint[] vertex, bool bKeepSize)
        {
            if (vertex == null) return IntPtr.Zero;
            if (vertex.Length != 4) return IntPtr.Zero;

            int size = Marshal.SizeOf(typeof(RvPoint)) * vertex.Length;

            IntPtr ptr = Marshal.AllocHGlobal(size);

            for (int i = 0; i < vertex.Length; i++)
            {
                IntPtr offset = IntPtr.Add(ptr, i * Marshal.SizeOf(typeof(RvPoint)));
                Marshal.StructureToPtr(vertex[i], offset, false);
            }

            IntPtr ptr2 = new IntPtr(bKeepSize ? 1 : 0);

            HMOLD h = CreateMold(MoldType.Skew, ptr, ptr2);

            Marshal.FreeHGlobal(ptr);

            return h;
        }

        public static HMOLD/*GMold**/  CreateTileMold(int rows, int cols, int mirrorType)
        {
            RvScalar scalar = new RvScalar(rows, cols);

            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RvScalar)));
            Marshal.StructureToPtr(scalar, ptr, false);

            IntPtr ptr2 = new IntPtr(mirrorType);

            HMOLD h = CreateMold(MoldType.Tile, ptr, ptr2);

            Marshal.FreeHGlobal(ptr);

            return h;

        }

        public static HSTROKE DrawText(HGDC dc, string strText, int x, int y, HSTROKE hStroke)
        {
            if (string.IsNullOrEmpty(strText))
            {
                return DrawDummy(dc, hStroke);
            }

            byte[] strBuff = Pool.StringToCharBytes(strText);
            return DrawText(dc, strBuff, x, y, hStroke);
        }

        public static void TextOut(HGDC dc, string strText)
        {
            if (string.IsNullOrEmpty(strText))
            {
                return;
            }

            byte[] strBuff = Pool.StringToCharBytes(strText);
            rvgTextOut(dc, strBuff);
        }

        public static void TextOut(HGDC dc, string strText, int fontSize)
        {
            if (string.IsNullOrEmpty(strText))
            {
                return;
            }

            byte[] strBuff = Pool.StringToCharBytes(strText);
            rvgTextOutEx(dc, strBuff, fontSize);
        }

        public static GSize GetTextSize(HGDC dc, string strText, int count)
        {
            if (string.IsNullOrEmpty(strText))
            {
                return new GSize();
            }

            byte[] strBuff = Pool.StringToCharBytes(strText);

            return GetTextSize(dc, strBuff, count);
        }

        public static HSTROKE DrawText(HGDC dc, string strText, GRect rect, HSTROKE hStroke)
        {
            if (string.IsNullOrEmpty(strText))
            {
                return DrawDummy(dc, hStroke);
            }

            byte[] strBuff = Pool.StringToCharBytes(strText);

            return DrawTextE1(dc, strBuff, rect, hStroke/* = NULL*/);
        }

        public static HSTROKE DrawTextEx(HGDC dc, string strText, GRect rect, int nFormat, int lineSpace, HSTROKE hStroke)
        {
            if (string.IsNullOrEmpty(strText))
            {
                return DrawDummy(dc, hStroke);
            }

            byte[] strBuff = Pool.StringToCharBytes(strText);

            return DrawTextEx(dc, strBuff, rect, nFormat, lineSpace, hStroke);
        }

        public static HFONT CreateFont(string strFamilyName, int size, FontFlags flags)
        {
            if (string.IsNullOrEmpty(strFamilyName))
            {
                strFamilyName = "Arial";
            }

            byte[] strBuff = Pool.StringToCharBytes(strFamilyName);

            return CreateFont(strBuff, size, (int)flags);
        }

        public static HFONT CreateFontEx(string strFamilyName, int fontSize, GRgb textColor, GRgb backColor, FontFlags flags)
        {
            if (string.IsNullOrEmpty(strFamilyName))
            {
                strFamilyName = "Arial";
            }

            byte[] strBuff = Pool.StringToCharBytes(strFamilyName);

            return rvgCreateFontEx(strBuff, fontSize, textColor, backColor, (int)flags);
        }

        //public static HSTROKE DrawDots(HGDC dc, GPoint[] arr, HSTROKE hStroke)
        //{
        //    if (arr == null)
        //    {
        //        return DrawDummy(dc, hStroke);
        //    }

        //    int unisze = Marshal.SizeOf(typeof(GPoint));
        //    IntPtr ptr = Marshal.AllocHGlobal(unisze * arr.Length);

        //    for (int i = 0; i < arr.Length; i++)
        //    {
        //        IntPtr offset = IntPtr.Add(ptr, i * unisze);
        //        Marshal.StructureToPtr(arr[i], offset, false);
        //    }
        //    HSTROKE h = DrawDots(dc, ptr, arr.Length, hStroke);

        //    Marshal.FreeHGlobal(ptr);

        //    return h;
        //}

        public static HSTROKE DrawDots(HGDC dc, RvPoint[] arr, HSTROKE hStroke)
        {
            if (arr == null)
            {
                return DrawDummy(dc, hStroke);
            }

            int unisze = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(unisze * arr.Length);

            for (int i = 0; i < arr.Length; i++)
            {
                IntPtr offset = IntPtr.Add(ptr, i * unisze);
                Marshal.StructureToPtr(arr[i], offset, false);
            }
            HSTROKE h = DrawDots(dc, ptr, arr.Length, hStroke);

            Marshal.FreeHGlobal(ptr);

            return h;
        }
        public static HSTROKE DrawDots(HGDC dc, RvPointF32[] arr, HSTROKE hStroke)
        {
            if (arr == null)
            {
                return DrawDummy(dc, hStroke);
            }

            int unisze = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(unisze * arr.Length);

            for (int i = 0; i < arr.Length; i++)
            {
                IntPtr offset = IntPtr.Add(ptr, i * unisze);
                Marshal.StructureToPtr(arr[i], offset, false);
            }
            HSTROKE h = DrawDotsE1(dc, ptr, arr.Length, hStroke);

            Marshal.FreeHGlobal(ptr);

            return h;
        }

        public static HSTROKE DrawTriangles(HGDC dc, RvPoint[] arr, int type, HSTROKE hStroke)
        {
            if (arr == null)
            {
                return DrawDummy(dc, hStroke);
            }

            int unisze = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(unisze * arr.Length);

            for (int i = 0; i < arr.Length; i++)
            {
                IntPtr offset = IntPtr.Add(ptr, i * unisze);
                Marshal.StructureToPtr(arr[i], offset, false);
            }
            HSTROKE h = DrawTriangles(dc, ptr, arr.Length, type, hStroke);

            Marshal.FreeHGlobal(ptr);

            return h;
        }

        public static HSTROKE DrawTriangles(HGDC dc, RvPointF32[] arr, int type, HSTROKE hStroke)
        {
            if (arr == null)
            {
                return DrawDummy(dc, hStroke);
            }

            int unisze = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(unisze * arr.Length);

            for (int i = 0; i < arr.Length; i++)
            {
                IntPtr offset = IntPtr.Add(ptr, i * unisze);
                Marshal.StructureToPtr(arr[i], offset, false);
            }
            HSTROKE h = DrawTrianglesE1(dc, ptr, arr.Length, type, hStroke);

            Marshal.FreeHGlobal(ptr);

            return h;
        }

        public static HSTROKE DrawPolygon(HGDC dc, RvPoint[] arr, bool bFill, HSTROKE hStroke)
        {
            if (arr == null)
            {
                return DrawDummy(dc, hStroke);
            }

            int unisze = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(unisze * arr.Length);

            for (int i = 0; i < arr.Length; i++)
            {
                IntPtr offset = IntPtr.Add(ptr, i * unisze);
                Marshal.StructureToPtr(arr[i], offset, false);
            }
            HSTROKE h = rvgDrawPolygon(dc, ptr, arr.Length, bFill, hStroke);

            Marshal.FreeHGlobal(ptr);

            return h;
        }

        public static HSTROKE DrawPolygon(HGDC dc, RvPointF64[] arr64, bool bFill, HSTROKE hStroke)
        {
            if (arr64 == null)
            {
                return DrawDummy(dc, hStroke);
            }

            RvPointF32[] arr = new RvPointF32[arr64.Length];
            for (int i = 0; i < arr64.Length; i++)
            {
                arr[i].x = (float)arr64[i].x;
                arr[i].y = (float)arr64[i].y;
            }

            int unisze = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(unisze * arr.Length);

            for (int i = 0; i < arr.Length; i++)
            {
                IntPtr offset = IntPtr.Add(ptr, i * unisze);
                Marshal.StructureToPtr(arr[i], offset, false);
            }
            HSTROKE h = rvgDrawPolygonE1(dc, ptr, arr.Length, bFill, hStroke);

            Marshal.FreeHGlobal(ptr);

            return h;
        }

        public static HSTROKE DrawPolygon(HGDC dc, RvPointF32[] arr, bool bFill, HSTROKE hStroke)
        {
            if (arr == null)
            {
                return DrawDummy(dc, hStroke);
            }

            int unisze = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(unisze * arr.Length);

            for (int i = 0; i < arr.Length; i++)
            {
                IntPtr offset = IntPtr.Add(ptr, i * unisze);
                Marshal.StructureToPtr(arr[i], offset, false);
            }
            HSTROKE h = rvgDrawPolygonE1(dc, ptr, arr.Length, bFill, hStroke);

            Marshal.FreeHGlobal(ptr);

            return h;
        }

        public static HSTROKE DrawPolyline(HGDC dc, RvPoint[] arr, HSTROKE hStroke)
        {
            if (arr == null)
            {
                return DrawDummy(dc, hStroke);
            }

            int unisze = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(unisze * arr.Length);

            for (int i = 0; i < arr.Length; i++)
            {
                IntPtr offset = IntPtr.Add(ptr, i * unisze);
                Marshal.StructureToPtr(arr[i], offset, false);
            }
            HSTROKE h = DrawPolyline(dc, ptr, arr.Length, hStroke);

            Marshal.FreeHGlobal(ptr);

            return h;
        }

        public static HSTROKE DrawPolyline(HGDC dc, RvPointF32[] arr, HSTROKE hStroke)
        {
            if (arr == null)
            {
                return DrawDummy(dc, hStroke);
            }

            int unisze = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(unisze * arr.Length);

            for (int i = 0; i < arr.Length; i++)
            {
                IntPtr offset = IntPtr.Add(ptr, i * unisze);
                Marshal.StructureToPtr(arr[i], offset, false);
            }
            HSTROKE h = DrawPolylineE1(dc, ptr, arr.Length, hStroke);

            Marshal.FreeHGlobal(ptr);

            return h;
        }

        public static HSTROKE DrawSegments(HGDC dc, RvPoint[] arr, HSTROKE hStroke)
        {
            if (arr == null)
            {
                return DrawDummy(dc, hStroke);
            }
            //should be even
            int sze = arr.Length - (arr.Length % 2);
            if (sze < 2)
            {
                return DrawDummy(dc, hStroke);
            }

            int unisze = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(unisze * arr.Length);

            for (int i = 0; i < arr.Length; i++)
            {
                IntPtr offset = IntPtr.Add(ptr, i * unisze);
                Marshal.StructureToPtr(arr[i], offset, false);
            }
            HSTROKE h = DrawSegments(dc, ptr, arr.Length, hStroke);

            Marshal.FreeHGlobal(ptr);

            return h;
        }

        public static HSTROKE DrawSegments(HGDC dc, RvPointF32[] arr, HSTROKE hStroke)
        {
            if (arr == null)
            {
                return DrawDummy(dc, hStroke);
            }
            //should be even
            int sze = arr.Length - (arr.Length % 2);
            if (sze < 2)
            {
                return DrawDummy(dc, hStroke);
            }

            int unisze = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(unisze * arr.Length);

            for (int i = 0; i < arr.Length; i++)
            {
                IntPtr offset = IntPtr.Add(ptr, i * unisze);
                Marshal.StructureToPtr(arr[i], offset, false);
            }
            HSTROKE h = DrawSegmentsE1(dc, ptr, arr.Length, hStroke);

            Marshal.FreeHGlobal(ptr);

            return h;
        }

        public static void SetViewPos(HGDC dc, int pos, bool bVertical)
        {
            rvgSetViewPos(dc, pos, bVertical);
        }
        public static void SetViewPos(HGDC dc, int x, int y)
        {
            rvgSetViewPosE1(dc, x, y);
        }

        public static int GetViewDelta(HGDC dc, bool bVertical)
        {
            return rvgGetViewDelta(dc, bVertical);
        }

        public static void GetViewDelta(HGDC dc, ref int dx, ref int dy)
        {
            rvgGetViewDeltaE1(dc, ref dx, ref dy);
        }

        public static HSTROKE Combine(HGDC dc, IntPtr[] strokeArray)
        {
            if (strokeArray == null) return IntPtr.Zero;

            int unisze = Marshal.SizeOf(typeof(IntPtr));
            IntPtr ptr = Marshal.AllocHGlobal(unisze * strokeArray.Length);

            for (int i = 0; i < strokeArray.Length; i++)
            {
                IntPtr offset = IntPtr.Add(ptr, i * unisze);
                Marshal.StructureToPtr(strokeArray[i], offset, false);
            }

            HSTROKE h = Combine(dc, ptr, strokeArray.Length);

            Marshal.FreeHGlobal(ptr);

            return h;

        }

        public static IntPtr[] Uncombine(HGDC dc, HSTROKE hMergedStroke, int subStrokeCount)
        {
            if (hMergedStroke == IntPtr.Zero || subStrokeCount <= 0) return null;
            if (subStrokeCount == 1) return new IntPtr[1] { hMergedStroke };

            int unisze = Marshal.SizeOf(typeof(IntPtr));
            IntPtr ptr = Marshal.AllocHGlobal(unisze * subStrokeCount);

            int n = Uncombine(dc, hMergedStroke, ptr, subStrokeCount);

            IntPtr[] subArr = null;

            if (n > 0)
            {
                subArr = new IntPtr[n];
                Marshal.Copy(ptr, subArr, 0, n);
            }

            Marshal.FreeHGlobal(ptr);

            return subArr;

        }


    }
}
