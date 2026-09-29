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

using Kingpool.Utility;
using Kingpool.Core;


namespace Kingpool.Xgui
{
    public enum LEAD_SHAPE
    {
        SQUARE = 0, //方形
        ROUND,       //圆形
        TRIANGLE,    //三角形
        DIAMOND,     //菱形
        ARROW,       //箭头
        CROSS,       //十字线
        CROSS_X,     //乘号线
    }

    //public enum LINE_TYPE
    //{
    //    SOLID = 0,
    //    HIDDEN = 1,
    //    CENTER = 2,
    //    INCLUSION = 3,
    //}

    public class KUcobj
    {
        public const int UCS_ENABLE = (1);
        public const int UCS_VISIBLE = (1 << 1);
        public const int UCS_TABSTOP = (1 << 2);
        public const int UCS_FIXPOS = (1 << 3);
        public const int UCS_FIXSIZE = (1 << 4);
        public const int UCS_ROTATABLE = (1 << 5);
        public const int UCS_ANIMATION = (1 << 6);
        public const int UCS_BORDER = (1 << 7);
        public const int UCS_CAPTION = (1 << 8);
        public const int UCS_FILLBKG = (1 << 9);
        public const int UCS_SYMMSIZING = (1 << 10);
        public const int UCS_URL = (1 << 11);

        public const int MIN_FONT_SIZE = 6;
        public const int MAX_FONT_NAME_LENGTH = 63;
        public const int MAX_CAPTION_LENGTH = 127;
        public const int MAX_NAME_LENGTH = 63;
        public const int MAX_PROPERTY_TEXT_LEN = 1203;

        //class id
        public const int SCID_INVALID = 0x7FFFFFFF;
        public const int SCID_INDT_BASE = (0);     // -4096
        public const int SCID_CALI_BASE = (5000);  //0 
        public const int SCID_RACT_BASE = (10000);   //4096


        public const string VNAME_TRUE = "true";
        public const string VNAME_FALSE = "false";

        [DllImport("UniVision.dll")]
        protected static extern void uniSetWidgetVisible(IntPtr hWidget, bool flag);
        [DllImport("UniVision.dll")]
        protected static extern bool uniIsWidgetVisible(IntPtr hWidget);


        [DllImport("UniVision.dll")]
        protected static extern void uniSetWidgetRect(IntPtr hWidget, int left, int top, uint width, uint height);
        [DllImport("UniVision.dll")]
        protected static extern void uniSetWidgetPos(IntPtr hWidget, int x, int y);
        [DllImport("UniVision.dll")]
        protected static extern RvPoint uniGetWidgetPos(IntPtr hWidget);
        [DllImport("UniVision.dll")]
        protected static extern RvRect uniGetWidgetRect(IntPtr hWidget);
        [DllImport("UniVision.dll")]
        protected static extern int uniGetWidgetStyle(IntPtr hWidget);
        [DllImport("UniVision.dll")]
        protected static extern void uniSetWidgetStyle(IntPtr hWidget, int style);

        [DllImport("UniVision.dll")]
        protected static extern bool uniWidgetSerialize(IntPtr hWidget, IntPtr hDisk, bool bRead);
        [DllImport("UniVision.dll")]
        protected static extern void uniSetBorderColor(IntPtr hWidget, RvRgba rgba);
        [DllImport("UniVision.dll")]
        protected static extern void uniSetBorderType(IntPtr hWidget, int type);
        [DllImport("UniVision.dll")]
        protected static extern void uniSetBorderThick(IntPtr hWidget, int width);
        [DllImport("UniVision.dll")]
        protected static extern RvRgba uniGetBorderColor(IntPtr hWidget);

        [DllImport("UniVision.dll")]
        protected static extern int uniGetBorderType(IntPtr hWidget);
        [DllImport("UniVision.dll")]
        protected static extern int uniGetBorderThick(IntPtr hWidget);
        [DllImport("UniVision.dll")]
        protected static extern RvRgba uniGetBodyColor(IntPtr hWidget);
        [DllImport("UniVision.dll")]
        protected static extern void uniSetBodyColor(IntPtr hWidget, RvRgba rgba);
        [DllImport("UniVision.dll")]
        protected static extern void uniSetTextColor(IntPtr hWidget, RvRgba rgba);
        [DllImport("UniVision.dll")]
        protected static extern void uniSetFontSize(IntPtr hWidget, int size);
        [DllImport("UniVision.dll")]
        protected static extern void uniSetFontName(IntPtr hWidget, string strName);
        [DllImport("UniVision.dll")]
        protected static extern RvRgba uniGetTextColor(IntPtr hWidget);
        [DllImport("UniVision.dll")]
        protected static extern int uniGetFontSize(IntPtr hWidget);
        [DllImport("UniVision.dll")]
        protected static extern int uniGetFontName(IntPtr hWidget, StringBuilder strOut, int maxLength);
        [DllImport("UniVision.dll")]
        protected static extern int uniWidgetGetClassId(IntPtr hWidget);
        [DllImport("UniVision.dll")]
        protected static extern void uniSetWidgetCaption(IntPtr hWidget, string strCaption);
        [DllImport("UniVision.dll")]
        protected static extern int uniGetWidgetCaption(IntPtr hWidget, StringBuilder strOut, int maxLength);
        [DllImport("UniVision.dll")]
        protected static extern void uniSetWidgetName(IntPtr hWidget, string strName);
        [DllImport("UniVision.dll")]
        protected static extern int uniGetWidgetName(IntPtr hWidget, StringBuilder strOut, int maxLength);
        [DllImport("UniVision.dll")]
        protected static extern void uniSetWidgetId(IntPtr hWidget, uint id);
        [DllImport("UniVision.dll")]
        protected static extern uint uniGetWidgetId(IntPtr hWidget);
        [DllImport("UniVision.dll")]
        protected static extern IntPtr uniWidgetClone(IntPtr hWidget);
        [DllImport("UniVision.dll")]
        protected static extern void uniWidgetCopyFrom(IntPtr hWidget, IntPtr hTwin);

        [DllImport("UniVision.dll")]
        protected static extern bool uniSetPropertyValue(IntPtr hWidget, string strName, string strValue);
        [DllImport("UniVision.dll")]
        protected static extern bool uniGetPropertyValue(IntPtr hWidget, string strName, StringBuilder sb, int maxLength);
        [DllImport("UniVision.dll")]
        protected static extern IntPtr uniWidgetCreate(string strClassName);
        [DllImport("UniVision.dll")]
        protected static extern void uniWidgetDestroy(IntPtr hWidget);
        [DllImport("UniVision.dll")]
        protected static extern void uniSetSelectState(IntPtr hWidget, bool flag);
        [DllImport("UniVision.dll")]
        protected static extern int uniHitTest(IntPtr hWidget, IntPtr hDelegate, RvPoint point);



        protected IntPtr m_hWidget = IntPtr.Zero;
        protected bool m_bAttached = false;

        ~KUcobj()
        {
            Destroy();
        }

        public virtual bool Create(string strClassName)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return false;
            }

            m_hWidget = uniWidgetCreate(strClassName);
            return (m_hWidget != IntPtr.Zero);

        }
        public virtual void Destroy()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                if (!m_bAttached)
                {
                    uniWidgetDestroy(m_hWidget);
                }
                m_hWidget = IntPtr.Zero;
            }

        }

        public bool Attach(IntPtr hWidget)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return false;
            }
            if (IntPtr.Zero == hWidget)
            {
                return false;
            }

            m_bAttached = true;
            m_hWidget = hWidget;

            return true;
        }

        public void Detach()
        {
            if (m_bAttached)
            {
                m_hWidget = IntPtr.Zero;
                m_bAttached = false;
            }
        }

        public IntPtr GetHandle()
        {
            return m_hWidget;
        }

        public bool IsValid()
        {
            return (m_hWidget != IntPtr.Zero);
        }

        public virtual void SetVisible(bool flag)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniSetWidgetVisible(m_hWidget, flag);
            }
        }

        public bool IsVisible()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniIsWidgetVisible(m_hWidget);
            }
            return false;
        }

        public uint GetClassId()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return (uint)uniWidgetGetClassId(m_hWidget);
            }
            return Pool.INVALID_ID;
        }


        public virtual void SetStyle(int style)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniSetWidgetStyle(m_hWidget, style);
            }
        }
        public int GetStyle()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniGetWidgetStyle(m_hWidget);
            }
            return 0;
        }

        public void SetRect(int left, int top, uint width, uint height)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniSetWidgetRect(m_hWidget, left, top, width, height);
            }
        }

        public RvRect GetRect()
        {
            RvRect r = new RvRect(0, 0, 0, 0);

            if (m_hWidget != IntPtr.Zero)
            {
                return uniGetWidgetRect(m_hWidget);
            }

            //r.left = 0;
            //r.top = 0;
            //r.right = 0;
            //r.bottom = 0;

            return r;
        }
        public void SetPos(int x, int y)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniSetWidgetPos(m_hWidget, x, y);
            }
        }

        public RvPoint GetPos(bool bCenter)
        {
            RvPoint pt;
            pt.x = 0;
            pt.y = 0;

            if (bCenter)
            {
                RvRect re = GetRect();
                pt.x = (re.left + re.right) / 2;
                pt.y = (re.top + re.bottom) / 2;
            }
            else
            {
                if (m_hWidget != IntPtr.Zero)
                {
                    return uniGetWidgetPos(m_hWidget);
                }
            }

            return pt;
        }

        public bool Serialize(IntPtr hDisk, bool bRead)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniWidgetSerialize(m_hWidget, hDisk, bRead);
            }

            return false;
        }

        public virtual bool Serialize(DiskHelper diskHelper, bool bRead)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniWidgetSerialize(m_hWidget, diskHelper.GetHandle(), bRead);
            }

            return false;
        }


        public void SetBorderColor(RvRgba rgba)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniSetBorderColor(m_hWidget, rgba);
            }
        }

        public void SetBorderType(int type)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniSetBorderType(m_hWidget, type);
            }
        }
        public void SetBorderThick(int width)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniSetBorderThick(m_hWidget, width);
            }
        }

        public RvRgba GetBorderColor(int width)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniGetBorderColor(m_hWidget);
            }

            return new RvRgba();
        }

        public LinePattern GetBorderType()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return (LinePattern)uniGetBorderType(m_hWidget);
            }

            return LinePattern.Solid;
        }

        public int GetBorderThick()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniGetBorderThick(m_hWidget);
            }

            return 1;
        }

        public void SetTextColor(RvRgba rgba)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniSetTextColor(m_hWidget, rgba);
            }
        }

        public void SetFontSize(int size)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniSetFontSize(m_hWidget, size);
            }
        }

        public void SetFontName(string strName)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniSetFontName(m_hWidget, strName);
            }
        }

        public RvRgba GetTextColor(string strName)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniGetTextColor(m_hWidget);
            }
            return new RvRgba();
        }

        public int GetFontSize()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniGetFontSize(m_hWidget);
            }
            return MIN_FONT_SIZE;
        }

        public string GetFontName()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                StringBuilder sb = new StringBuilder(MAX_FONT_NAME_LENGTH + 1);

                uniGetFontName(m_hWidget, sb, MAX_FONT_NAME_LENGTH + 1);

                return sb.ToString();
            }
            return null;
        }

        public RvRgba GetBodyColor()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniGetBodyColor(m_hWidget);
            }
            return new RvRgba(0, 0, 0, 255);
        }

        public void SetBodyColor(RvRgba rgba)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniSetBodyColor(m_hWidget, rgba);
            }
        }

        public int GetSubclassId()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniWidgetGetClassId(m_hWidget);
            }

            return SCID_INVALID;
        }

        public void SetCaption(string strCaption)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniSetWidgetCaption(m_hWidget, strCaption);
            }

        }
        public string GetCaption()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                StringBuilder sb = new StringBuilder(MAX_CAPTION_LENGTH + 1);
                uniGetWidgetCaption(m_hWidget, sb, MAX_CAPTION_LENGTH + 1);

                return sb.ToString();
            }

            return null;

        }

        public void SetName(string strName)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniSetWidgetName(m_hWidget, strName);
            }

        }
        public string GetName()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                StringBuilder sb = new StringBuilder(MAX_NAME_LENGTH + 1);
                uniGetWidgetName(m_hWidget, sb, MAX_NAME_LENGTH + 1);
                return sb.ToString();
            }

            return null;
        }

        public void SetId(uint id)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniSetWidgetId(m_hWidget, id);
            }

        }
        public uint GetId()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniGetWidgetId(m_hWidget);
            }

            return 0;

        }
        public void SelectState(bool flag)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniSetSelectState(m_hWidget, flag);
            }
        }

        public int HitTest(KRealView realView, RvPoint point)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniHitTest(m_hWidget, realView.GetHandle(), point);
            }

            return (int)HIT_POS.NOTHING;
        }



        public virtual IntPtr Clone()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniWidgetClone(m_hWidget);
            }

            return IntPtr.Zero;
        }

        public virtual void CopyFrom(IntPtr hTwin)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniWidgetCopyFrom(m_hWidget, hTwin);
            }
        }

        public bool SetPropertyValue(string strName, string strValue)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniSetPropertyValue(m_hWidget, strName, strValue);
            }

            return false;
        }

        public bool GetPropertyValue(string strName, out string strReturn)
        {
            strReturn = "";
            if (m_hWidget != IntPtr.Zero)
            {
                int n = MAX_PROPERTY_TEXT_LEN + 1;
                StringBuilder sb = new StringBuilder(n);

                bool ret = uniGetPropertyValue(m_hWidget, strName, sb, n);

                if (ret)
                {
                    strReturn = sb.ToString();
                }

                return ret;
            }

            return false;
        }


    }
}
