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
    public class KIndicater : KUcobj
    {

        public static string PICTURE = "Picture";
        public static string SHAPE = "Shape";
        public static string LABEL = "Label";
        public static string LINE = "Line";
        public static string MARK = "Mark";




        [DllImport("UniVision.dll")]
        private static extern IntPtr uniIndicaterCreate(string strClassName);
        [DllImport("UniVision.dll")]
        private static extern void uniIndicaterDestroy(IntPtr hIndicater);
        [DllImport("UniVision.dll")]
        private static extern bool uniIndicaterSetImage(IntPtr hIndicater, IntPtr image);
        [DllImport("UniVision.dll")]
        private static extern bool uniIndicaterSetImageE1(IntPtr hIndicater, string strFileName);

        // private IntPtr m_hWidget = IntPtr.Zero;

        public KIndicater(string strClassName)
        {
            Create(strClassName);
        }

        public KIndicater()
        {

        }

        ~KIndicater()
        {
            Destroy();
        }


        public override bool Create(string strClassName)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return false;
                // throw new Exception("Object exist already!");
            }

            m_hWidget = uniIndicaterCreate(strClassName);
            return (m_hWidget != IntPtr.Zero);
        }
        public override void Destroy()
        {
            if (m_bAttached) return;

            if (m_hWidget != IntPtr.Zero)
            {
                uniIndicaterDestroy(m_hWidget);
                m_hWidget = IntPtr.Zero;
            }
        }



        public bool SetImage(IntPtr image)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniIndicaterSetImage(m_hWidget, image);
            }

            return false;
        }

        public bool SetImage(string strFileName)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniIndicaterSetImageE1(m_hWidget, strFileName);
            }

            return false;
        }


    }

    public class Mark : KIndicater
    {
        //属性名称
        public const string PNAME_POS_CENTERED = "PosCentered";
        public const string PNAME_CUR_POS = "CurPos";
        public const string PNAME_OUTER_SHAPE = "OuterShape";
        public const string PNAME_INNER_SHAPE = "InnerShape";
        public const string PNAME_OUTER_SIZE = "OuterSize";
        public const string PNAME_INNER_SIZE = "InnerSize";

        //属性值
        //外侧形状
        public const string VNAME_NONE = "None";      //INNER形状 和outer形状
        public const string VNAME_FOCUS = "Focus";   //outer形状
        public const string VNAME_BOX = "Box";       //outer形状
        public const string VNAME_RULER = "Ruler";   //outer形状
        public const string VNAME_BOX_DISK = "BoxDisk";       //outer形状
        //public const string VNAME_DISK_LINES = "DiskLines";   //outer形状
        public const string VNAME_POLYGON = "Polygon";   //outer形状 

        //内侧形状
        public const string VNAME_SPOT = "Spot";   //inner形状
        public const string VNAME_CROSS = "Cross";   //inner形状
        public const string VNAME_CROSS_E2 = "CrossE2";   //inner形状
        public const string VNAME_CROSS_E1 = "CrossE1";   //inner形状
        public const string VNAME_HORIZON_LINE = "HorizonLine";   //inner形状
        public const string VNAME_VERTICAL_LINE = "VerticalLine";   //inner形状 
        public const string VNAME_HORIZON_RULER = "HorizonRuler";   //inner形状
        public const string VNAME_VERTICAL_RULER = "VerticalRuler";   //inner形状
        public const string VNAME_POLYLINE = "Polyline";   //inner形状

        public const string VNAME_ICON = "Icon";   //inner形状

        [DllImport("UniVision.dll")]
        private static extern void uniMarkSetVertexArray(IntPtr hIndicater, IntPtr pVertexArray, int count);

        public Mark()
        {
            Create(MARK);
        }

        public void SetVertexArray(RvPointF32[] vertexArray)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                int cnt = vertexArray.Length;

                if (cnt <= 0) return;

                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RvPointF32)) * cnt);
                for (int i = 0; i < cnt; i++)
                {
                    IntPtr ptrtmp = (IntPtr)(ptr + i * Marshal.SizeOf(typeof(RvPointF32)));
                    Marshal.StructureToPtr(vertexArray[i], ptrtmp, false);
                }

                uniMarkSetVertexArray(m_hWidget, ptr, cnt);

                Marshal.FreeHGlobal(ptr);

            }
        }


    }

    public class Polyline : KIndicater
    {
        public static string POLYLINE = "Polyline";
        [DllImport("UniVision.dll")]
        private static extern void uniPolylineSetVertexArray(IntPtr hPolyline, IntPtr pVertexArray, int count);
        [DllImport("UniVision.dll")]
        private static extern void uniPolylineSetSublineStyle(IntPtr hPolyline, int index, int type, int thick, uint color);
        [DllImport("UniVision.dll")]
        private static extern void uniPolylineSetClosed(IntPtr hPolyline, bool flag);

        public Polyline()
        {
            Create(POLYLINE);
        }

        public void SetVertexArray(RvPointF32[] vertexArray)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                int cnt = vertexArray.Length;

                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RvPointF32)) * cnt);
                for (int i = 0; i < cnt; i++)
                {
                    IntPtr ptrtmp = (IntPtr)(ptr + i * Marshal.SizeOf(typeof(RvPointF32)));
                    Marshal.StructureToPtr(vertexArray[i], ptrtmp, false);
                }

                uniPolylineSetVertexArray(m_hWidget, ptr, cnt);

                Marshal.FreeHGlobal(ptr);

            }
        }

        public void SetSublineStyle(int index, int type, int thick, uint color)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniPolylineSetSublineStyle(m_hWidget, index, type, thick, color);
            }
        }
        public void SetSublineStyle(int index, int type, int thick, RvRgba color)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uint icolor = Pool.RGBA(color.red, color.green, color.blue, color.alpha);
                uniPolylineSetSublineStyle(m_hWidget, index, type, thick, icolor);
            }
        }
        public void SetClose(bool flag)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniPolylineSetClosed(m_hWidget, flag);
            }
        }

    }

    public class Label : KIndicater
    {

    }
}