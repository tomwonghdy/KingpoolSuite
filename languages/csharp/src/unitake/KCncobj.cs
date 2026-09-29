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
using Kingpool;
using Kingpool.Xgui;

namespace Kingpool.AutoTab
{
    public class KCncobj : KUcobj
    {
        //类名称
        public const string DRILL_TEXT = ("CoDrill");
        public const string LINE_TEXT = ("CoLine");
        public const string CIRCLE_TEXT = ("CoCircle");
        public const string ARC_TEXT = ("CoArc");
        public const string SHAPE_L_TEXT = ("CoShapeL");
        public const string SHAPE_U_TEXT = ("CoShapeU");
        public const string POLYLINE_TEXT = ("CoPolyline");


        //cncojc id
        public const int SCID_CNCOBJ = (SCID_CALI_BASE + 1 + 2048 + 100);
        public const int SCID_CO_DRILL = (SCID_CNCOBJ + 1);
        public const int SCID_CO_LINE = (SCID_CNCOBJ + 2);
        public const int SCID_CO_ARC = (SCID_CNCOBJ + 3);
        public const int SCID_CO_CIRCLE = (SCID_CNCOBJ + 4);
        public const int SCID_CO_SHAPE_L = (SCID_CNCOBJ + 5);
        public const int SCID_CO_SHAPE_U = (SCID_CNCOBJ + 6);
        public const int SCID_CO_POLYLINE = (SCID_CNCOBJ + 7);

        //扩展风格

        public const int CO_EDIT_FINE = (1 << 20);   //精调
        public const int CO_SHOW_CUTTER = (1 << 21);   //显示刀具
        public const int CO_DONE = (1 << 24);  //已经检查或加工过了
        public const int CO_ANGLE = (1 << 25);   //显示角度
        public const int CO_MOTION = (1 << 26);   //路径类型：常规，空走，跳过
        public const int CO_CUTTER_UP = (1 << 27);   //提刀指示符号

        //运动类型
        public const int MT_DEFAULT = 0;//正常
        public const int MT_NOUP = 1;      //加工后不提刀
        public const int MT_TRANSIT = 2;   //空走
        public const int MT_SKIP = 3;      //跳过

        //坐标转换方式
        public const int SCTR_DEF = 0; // 比例标定方式
        public const int SCTR_RSM = 1;// 坐标计算， 用于固定视场
        public const int SCTR_RSM_E1 = 2;// 坐标计算，且考虑当前视场机械位置,这个用于移动视场

        //圆弧移动方向
        public const int CW = 0;    //顺时针
        public const int CCW = 1;     //move direction: 逆时针 

        [DllImport("UniVision.dll")]
        private static extern IntPtr uniCncobjCreate(string strClassName);
        [DllImport("UniVision.dll")]
        public static extern void uniCncobjDestroy(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern bool uniCncobjSerialize(IntPtr hCncobj, IntPtr hDisk, bool bIn);
        [DllImport("UniVision.dll")]
        private static extern IntPtr uniCncobjCloneNew(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjCloneFrom(IntPtr hCncobj, IntPtr hTwin);

        [DllImport("UniVision.dll")]
        private static extern bool uniCncobjIsFocused(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjSetFocus(IntPtr hCncobj, bool flag);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjSetExclude(IntPtr hCncobj, bool flag);
        [DllImport("UniVision.dll")]
        private static extern bool uniCncobjIsExcluded(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern double uniCncobjGetMacLength(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern RvPointF64 uniCncobjGetStartMacPos(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern RvPointF64 uniCncobjGetEndMacPos(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern RvPointF64 uniCncobjGetOrientedMacPosAt(IntPtr hCncobj, int index);
        [DllImport("UniVision.dll")]
        private static extern RvPointF64 uniCncobjGetReferMacPos(IntPtr hCncobj);
        // [DllImport("UniVision.dll")]
        // private static extern RvPoint uniCncobjGetCounterCamPos(IntPtr hCncobj, int index);
        [DllImport("UniVision.dll")]
        private static extern int uniCncobjGetMacPosCount(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern RvPointF64 uniCncobjGetMacPosAt(IntPtr hCncobj, int index);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjSetMacPosAt(IntPtr hCncobj, int index, RvPointF64 pos);
        [DllImport("UniVision.dll")]
        //返回RvPointF64*指针
        private static extern IntPtr uniCncobjGetMacPosArray(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjUpdateCamPos(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjUpdateCamPosE1(IntPtr hCncobj, double curMachX, double curMachY);

        [DllImport("UniVision.dll")]
        private static extern void uniCncobjUpdateMacPos(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern RvOffsetF64 uniCncobjCalcAlignCoefs(RvPointF64 referPos1, RvPointF64 referPos2,
                                           RvPointF64 targetPos1, RvPointF64 targetPos2, ref RvPointF64 pCenter, ref double pAngle);
        [DllImport("UniVision.dll")]
        private static extern RvOffsetF64 uniCncobjCalcAlignCoefsE1(RvPointF64 referPos, RvPointF64 targetPos);
        [DllImport("UniVision.dll")]
        private static extern RvPointF64 uniCncobjCalcMirrorPos(RvPointF64 referPos1, RvPointF64 referPos2, RvPointF64 sourcePos);
        [DllImport("UniVision.dll")]
        private static extern int uniCncobjCalcMatrixPos(RvPointF64 originPos, RvPointF64 horiPos, RvPointF64 vertPos,
                                                               int rows, int cols, IntPtr pArrOut, int arrSize);
        [DllImport("UniVision.dll")]
        private static extern RvPointF64 uniCalcAlignPosE1(RvPointF64 src, double offsetX, double offsetY, double cx, double cy, double angle);
        [DllImport("UniVision.dll")]
        private static extern RvPointF64 uniCalcAlignPos(RvPointF64 src, double offsetX, double offsetY);

        [DllImport("UniVision.dll")]
        private static extern bool uniCalcArcCenter(RvPointF64 start, RvPointF64 mid, RvPointF64 end, ref double cx, ref double cy, ref double radius);


        [DllImport("UniVision.dll")]
        private static extern int uniCncobjGetOrientation(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjToppleOrientation(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern int uniCncobjGetMotionType(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjSetMotionType(IntPtr hCncobj, int type);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjSetCoordTransParams(IntPtr hCncobj, int type, RvPointF32 cameraPos, RvPointF64 machPos, IntPtr hRsm);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjSetCoordTransParamsE1(IntPtr hCncobj, RvScalarF64 scalar, RvPointF32 cameraPos, RvPointF64 machPos);


        [DllImport("UniVision.dll")]
        private static extern RvPointF64 uniCncobjConvertPix2Mac(RvPointF32 camAnchor, RvPointF64 macAnchor, RvScalarF64 ratio, float x, float y);
        [DllImport("UniVision.dll")]
        private static extern RvPointF32 uniCncobjConvertMac2Pix(RvPointF64 macAnchor, RvPointF32 camAnchor, RvScalarF64 ratio, double x, double y);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjSetToolSize(IntPtr hCncobj, double diameter);
        [DllImport("UniVision.dll")]
        private static extern double uniCncobjGetToolSize(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjMoveMacPos(IntPtr hCncobj, double dx, double dy, bool bUpdateCam);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjRotateMacPos(IntPtr hCncobj, double cx, double cy, double angle, bool bUpdateCam);
        [DllImport("UniVision.dll")]
        private static extern int uniCncobjGetCamPosCount(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern RvPointF32 uniCncobjGetCamPosAt(IntPtr hCncobj, int index);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjSetCamPosAt(IntPtr hCncobj, int index, RvPointF32 pos);
        [DllImport("UniVision.dll")]
        private static extern bool uniIsArcClockWise(RvPointF64 start, RvPointF64 mid, RvPointF64 end);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjSetMacPosCount(IntPtr hCncobj, int count);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjMove(IntPtr hCncobj, double dx, double dy);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjScale(IntPtr hCncobj, double scale);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjScaleE1(IntPtr hCncobj, double scaleX, double scaleY);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjRotate(IntPtr hCncobj, double cx, double cy, double angle);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjRotateE1(IntPtr hCncobj, double angle);

        [DllImport("UniVision.dll")]
        private static extern RvPointF32 uniCncobjGetCentroid(IntPtr hCncobj);


        public override bool Create(string strClassName)
        {
            if (this.m_hWidget != IntPtr.Zero) return false;
            m_hWidget = uniCncobjCreate(strClassName);

            return (m_hWidget != IntPtr.Zero);
        }

        public bool CreateIndirect(IntPtr hCncObj)
        {
            if (this.m_hWidget != IntPtr.Zero)
            {
                if (m_bAttached == false)
                {
                    uniCncobjDestroy(m_hWidget);
                }
                m_bAttached = false;
            }

            m_hWidget = hCncObj;

            return (m_hWidget != IntPtr.Zero);
        }

        public bool Create(int scid)
        {
            switch (scid)
            {
                case SCID_CO_DRILL: return Create(DRILL_TEXT);
                case SCID_CO_LINE: return Create(LINE_TEXT);
                case SCID_CO_CIRCLE: return Create(CIRCLE_TEXT);
                case SCID_CO_ARC: return Create(ARC_TEXT);
                case SCID_CO_SHAPE_L: return Create(SHAPE_L_TEXT);
                case SCID_CO_SHAPE_U: return Create(SHAPE_U_TEXT);
                case SCID_CO_POLYLINE: return Create(POLYLINE_TEXT);

            }
            return false;
        }

        public void CopyFrom(KCncobj twin)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniCncobjCloneFrom(m_hWidget, twin.GetHandle());
            }
        }

        public override IntPtr Clone()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniCncobjCloneNew(m_hWidget);
            }

            return IntPtr.Zero;

        }


        public override void Destroy()
        {
            if (this.m_hWidget == IntPtr.Zero) return;

            if (m_bAttached == false)
            {
                uniCncobjDestroy(m_hWidget);
            }
            m_hWidget = IntPtr.Zero;
        }

        public bool IsFocused()
        {
            if (this.m_hWidget == IntPtr.Zero) return false;

            return uniCncobjIsFocused(m_hWidget);
        }

        public void SetFocus(bool flag)
        {
            if (this.m_hWidget == IntPtr.Zero) return;

            uniCncobjSetFocus(m_hWidget, flag);
        }
        public void SetExclude(bool flag)
        {
            if (this.m_hWidget == IntPtr.Zero) return;

            uniCncobjSetExclude(m_hWidget, flag);
        }

        public bool IsExcluded()
        {
            if (this.m_hWidget == IntPtr.Zero) return false;

            return uniCncobjIsExcluded(m_hWidget);
        }

        public double GetMacLength()
        {
            if (this.m_hWidget == IntPtr.Zero) return 0;

            return uniCncobjGetMacLength(m_hWidget);
        }


        public RvPointF64 GetStartMacPos()
        {
            if (this.m_hWidget == IntPtr.Zero) return new RvPointF64();

            return uniCncobjGetStartMacPos(m_hWidget);
        }

        public RvPointF64 GetEndMacPos()
        {
            if (this.m_hWidget == IntPtr.Zero) return new RvPointF64();

            return uniCncobjGetEndMacPos(m_hWidget);
        }

        public RvPointF64 GetOrientedMacPosAt(int index)
        {
            if (this.m_hWidget == IntPtr.Zero) return new RvPointF64();

            return uniCncobjGetOrientedMacPosAt(m_hWidget, index);
        }

        public RvPointF64 GetReferMacPos()
        {
            if (this.m_hWidget == IntPtr.Zero) return new RvPointF64();

            return uniCncobjGetReferMacPos(m_hWidget);
        }
        //public RvPoint GetCounterCamPos(int index)
        //{
        //    if (this.m_hWidget == IntPtr.Zero) return new RvPoint();

        //    return uniCncobjGetCounterCamPos(m_hWidget, index);
        //}

        public int GetMacPosCount()
        {
            if (this.m_hWidget == IntPtr.Zero) return 0;

            return uniCncobjGetMacPosCount(m_hWidget);
        }
        public RvPointF64 GetMacPosAt(int index)
        {
            if (this.m_hWidget == IntPtr.Zero) return new RvPointF64();

            return uniCncobjGetMacPosAt(m_hWidget, index);
        }

        public void SetMacPosAt(int index, RvPointF64 pos)
        {
            if (this.m_hWidget == IntPtr.Zero) return;

            uniCncobjSetMacPosAt(m_hWidget, index, pos);
        }

        public IntPtr uniCncobjGetMacPosArray()
        {
            if (this.m_hWidget == IntPtr.Zero) return IntPtr.Zero;

            return uniCncobjGetMacPosArray(m_hWidget);
        }

        public void UpdateCamPos()
        {
            if (this.m_hWidget == IntPtr.Zero) return;

            uniCncobjUpdateCamPos(m_hWidget);

        }

        public void UpdateCamPos(double curMachX, double curMachY)
        {
            if (this.m_hWidget == IntPtr.Zero) return;

            uniCncobjUpdateCamPosE1(m_hWidget, curMachX, curMachY);
        }

        public void UpdateMacPos()
        {
            if (this.m_hWidget == IntPtr.Zero) return;

            uniCncobjUpdateMacPos(m_hWidget);
        }

        public static RvOffsetF64 CalcAlignCoefs(RvPointF64 referPos1, RvPointF64 referPos2,
                                          RvPointF64 targetPos1, RvPointF64 targetPos2,
                                          ref RvPointF64 pCenter, ref double pAngle)
        {
            //   if (this.m_hWidget == IntPtr.Zero) return new RvOffsetF64();

            return uniCncobjCalcAlignCoefs(/*m_hWidget,*/ referPos1, referPos2,
                                           targetPos1, targetPos2, ref pCenter, ref pAngle);
        }

        public static RvOffsetF64 CalcAlignCoefs(RvPointF64 referPos, RvPointF64 targetPos)
        {
            // if (this.m_hWidget == IntPtr.Zero) return new RvOffsetF64();

            return uniCncobjCalcAlignCoefsE1(/*m_hWidget,*/ referPos, targetPos);
        }

        public static RvPointF64 CalcMirrorPos(RvPointF64 referPos1, RvPointF64 referPos2, RvPointF64 sourcePos)
        {
            //  if (this.m_hWidget == IntPtr.Zero) return new RvPointF64();

            return uniCncobjCalcMirrorPos(/*m_hWidget,*/ referPos1, referPos2, sourcePos);
        }

        public static int CalcMatrixPos(RvPointF64 originPos, RvPointF64 horiPos, RvPointF64 vertPos,
                                 int rows, int cols, RvPointF64[] pArrOut)
        {
            //   if (this.m_hWidget == IntPtr.Zero) return 0;

            int cnt = pArrOut.Length;

            if (cnt < rows * cols) return 0;

            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RvPointF64)) * cnt);

            if (ptr == IntPtr.Zero) return 0;

            int n = uniCncobjCalcMatrixPos(/*m_hWidget,*/ originPos, horiPos, vertPos,
                                   rows, cols, ptr, cnt);

            for (int i = 0; i < n; i++)
            {
                IntPtr ptrtmp = (IntPtr)(ptr + i * Marshal.SizeOf(typeof(RvPointF64)));
                pArrOut[i] = (RvPointF64)Marshal.PtrToStructure(ptrtmp, typeof(RvPointF64));
            }


            Marshal.FreeHGlobal(ptr);

            return n;
        }

        public static RvPointF64 CalcAlignPos(RvPointF64 src, double offsetX, double offsetY, double cx, double cy, double angle)
        {
            return uniCalcAlignPosE1(src, offsetX, offsetY, cx, cy, angle);
        }
        public static RvPointF64 CalcAlignPos(RvPointF64 src, double offsetX, double offsetY)
        {
            return uniCalcAlignPos(src, offsetX, offsetY);
        }

        public static bool CalcArcCenter(RvPointF64 start, RvPointF64 mid, RvPointF64 end, ref double cx, ref double cy, ref double radius)
        {
            return uniCalcArcCenter(start, mid, end, ref cx, ref cy, ref radius);
        }


        public int GetOrientation()
        {
            if (this.m_hWidget == IntPtr.Zero) return 0;

            return uniCncobjGetOrientation(m_hWidget);
        }

        public void ToppleOrientation()
        {
            if (this.m_hWidget == IntPtr.Zero) return;

            uniCncobjToppleOrientation(m_hWidget);
        }

        public int GetMotionType()
        {
            if (this.m_hWidget == IntPtr.Zero) return 0;

            return uniCncobjGetMotionType(m_hWidget);
        }
        public void SetMotionType(int type)
        {
            if (this.m_hWidget == IntPtr.Zero) return;

            uniCncobjSetMotionType(m_hWidget, type);
        }

        public string GetCncName()
        {
            if (this.m_hWidget == IntPtr.Zero) return "";

            int id = GetSubclassId();

            return GetCncName(id);
        }
        public static string GetCncName(int id)
        {
            if (id == KCncobj.SCID_CO_DRILL)
            {
                return "Drill";
            }
            else if (id == KCncobj.SCID_CO_LINE)
            {
                return "Line";
            }
            else if (id == KCncobj.SCID_CO_ARC)
            {
                return "Arc";
            }
            else if (id == KCncobj.SCID_CO_CIRCLE)
            {
                return "Circle";
            }
            else if (id == KCncobj.SCID_CO_SHAPE_L)
            {
                return "Shape L";
            }
            else if (id == KCncobj.SCID_CO_SHAPE_U)
            {
                return "Shape U";
            }
            else if (id == KCncobj.SCID_CO_POLYLINE)
            {
                return "Polyline";
            }
            //else if (id == KCncobj.SCID_CO_CHECK_POINT)
            //{
            //    return "Check Point";
            //}
            //else if (id == KCncobj.SCID_CO_MARK_LOCATER)
            //{
            //    return "Mark Locater";
            //}
            //else if (id == KCncobj.SCID_CO_SHAPE_LOCATER)
            //{
            //    return "Shape Locater";
            //}
            //else if (id == KCncobj.SCID_CO_BLOB_LOCATER)
            //{
            //    return "Blob Locater";
            //}
            //else if (id == KCncobj.SCID_CO_TEMPLATE_LOCATER)
            //{
            //    return "Templet Locater";
            //}

            return "Unknown";
        }

        public void SetCoordTransParams(int type, RvPointF32 cameraPos, RvPointF64 machPos, IntPtr hRsm)
        {
            if (this.m_hWidget == IntPtr.Zero) return;

            uniCncobjSetCoordTransParams(m_hWidget, type, cameraPos, machPos, hRsm);
        }

        public void SetCoordTransParams(RvScalarF64 scalar, RvPointF32 cameraPos, RvPointF64 machPos)
        {
            if (this.m_hWidget == IntPtr.Zero) return;

            uniCncobjSetCoordTransParamsE1(m_hWidget, scalar, cameraPos, machPos);
        }


        public static RvPointF64 ConvertPix2Mac(RvPointF32 camAnchor, RvPointF64 macAnchor, RvScalarF64 ratio, float x, float y)
        {
            return uniCncobjConvertPix2Mac(camAnchor, macAnchor, ratio, x, y);
        }
        public static RvPointF32 ConvertMac2Pix(RvPointF64 macAnchor, RvPointF32 camAnchor, RvScalarF64 ratio, double x, double y)
        {
            return uniCncobjConvertMac2Pix(macAnchor, camAnchor, ratio, x, y);
        }

        public void SetToolSize(double diameter)
        {
            if (this.m_hWidget == IntPtr.Zero) return;
            uniCncobjSetToolSize(m_hWidget, diameter);
        }

        public double GetToolSize()
        {
            if (this.m_hWidget == IntPtr.Zero) return 0;
            return uniCncobjGetToolSize(m_hWidget);
        }

        public void MoveMacPos(double dx, double dy, bool bUpdateCam)
        {
            if (this.m_hWidget == IntPtr.Zero) return;

            uniCncobjMoveMacPos(m_hWidget, dx, dy, bUpdateCam);
        }
        public void RotateMacPos(double cx, double cy, double angle, bool bUpdateCam)
        {
            if (this.m_hWidget == IntPtr.Zero) return;

            uniCncobjRotateMacPos(m_hWidget, cx, cy, angle, bUpdateCam);
        }

        public int GetCamPosCount()
        {
            if (this.m_hWidget == IntPtr.Zero) return 0;

            return uniCncobjGetCamPosCount(m_hWidget);
        }

        public RvPointF32 GetCamPosAt(int index)
        {
            if (this.m_hWidget == IntPtr.Zero) return new RvPointF32();

            return uniCncobjGetCamPosAt(m_hWidget, index);
        }

        public void SetCamPosAt(int index, RvPointF32 pos)
        {
            if (this.m_hWidget == IntPtr.Zero) return;

            uniCncobjSetCamPosAt(m_hWidget, index, pos);
        }

        public void SetMacPosCount(int count)
        {
            if (this.m_hWidget == IntPtr.Zero) return;

            uniCncobjSetMacPosCount(m_hWidget, count);
        }
        public static bool IsArcClockWise(RvPointF64 start, RvPointF64 mid, RvPointF64 end)
        {
            return uniIsArcClockWise(start, mid, end);
        }

        public void Move(double dx, double dy)
        {
            if (this.m_hWidget == IntPtr.Zero) return;

            uniCncobjMove(m_hWidget, dx, dy);
        }
        public void Scale(double scale)
        {
            if (this.m_hWidget == IntPtr.Zero) return;

            uniCncobjScale(m_hWidget, scale);
        }
        public void Scale(double scaleX, double scaleY)
        {
            if (this.m_hWidget == IntPtr.Zero) return;

            uniCncobjScaleE1(m_hWidget, scaleX, scaleY);
        }
        public void Rotate(double cx, double cy, double angle)
        {
            if (this.m_hWidget == IntPtr.Zero) return;

            uniCncobjRotate(m_hWidget, cx, cy, angle);
        }

        public void Rotate(double angle)
        {
            if (this.m_hWidget == IntPtr.Zero) return;

            uniCncobjRotateE1(m_hWidget, angle);
        }


        //获得图像坐标的质心
        //带圆弧的图元可能有些误差
        public RvPointF32 GetCentroid()
        {
            if (this.m_hWidget == IntPtr.Zero) return new RvPointF32();

            return uniCncobjGetCentroid(m_hWidget);
        }


    }


    public class CncMarkLocator : KCncobj
    {
        public const string TEMPLET_LOCATER_TEXT = ("CoTempletLocater");
        public const string BLOB_LOCATER_TEXT = ("CoBlobLocater");
        public const string SHAPE_LOCATER_TEXT = ("CoShapeLocater");

        public const int SCID_CO_SHAPE_LOCATER = (SCID_CNCOBJ + 41);
        public const int SCID_CO_BLOB_LOCATER = (SCID_CNCOBJ + 42);
        public const int SCID_CO_TEMPLATE_LOCATER = (SCID_CNCOBJ + 43);

        [DllImport("UniVision.dll")]
        private static extern IntPtr uniCncmarkGetOperator(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern bool uniCncmarkLearn(IntPtr hCncobj, IntPtr image, double machPosX, double machPosY, ref int pErrCode);
        [DllImport("UniVision.dll")]
        private static extern bool uniCncmarkLocate(IntPtr hCncobj, IntPtr image, ref RvPointF32 pResult, ref int pErrCode);
        [DllImport("UniVision.dll")]
        private static extern bool uniCncmarkGetResult(IntPtr hCncobj, ref RvPointF32 pResult);
        [DllImport("UniVision.dll")]
        private static extern void uniCncmarkSetStyle(IntPtr hCncobj, int style);
        [DllImport("UniVision.dll")]
        private static extern int uniCncmarkGetStyle(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern void uniCncmarkClearPatterns(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern void uniCncmarkClearResult(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern IntPtr uniCncmarkGetLearnedSceneImage(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern IntPtr uniCncmarkGetLearnedPatternImage(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern void uniCncmarkDestroyLearnedImages(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern bool uniCncmarkIsPatternLearnt(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern RvPointF64 uniCncmarkGetLearntMachPos(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern RvPointF32 uniCncmarkGetLearntImagePos(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern void uniCncmarkSetConfidence(IntPtr hCncobj, double score);
        [DllImport("UniVision.dll")]
        private static extern double uniCncmarkGetConfidence(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern void uniCncmarkSetCrossSize(IntPtr hCncobj, int size);
        [DllImport("UniVision.dll")]
        private static extern int uniCncmarkGetCrossSize(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern RvSize uniCncmarkGetPatternZoneSize(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern RvSize uniCncmarkGetSearchZoneSize(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern void uniCncmarkSetPatternRect(IntPtr hCncobj, RvRect rect);
        [DllImport("UniVision.dll")]
        private static extern void uniCncmarkSetSearchRect(IntPtr hCncobj, RvRect rect);
        [DllImport("UniVision.dll")]
        private static extern RvRect uniCncmarkGetPatternRect(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern RvRect uniCncmarkGetSearchRect(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern IntPtr uniCncmarkSnapshot(IntPtr hCncobj, IntPtr hImage, bool bSearchArea);

        public CncMarkLocator()
        {
            Create(TEMPLET_LOCATER_TEXT);
        }


        public IntPtr GetOperator()
        {
            if (this.m_hWidget == IntPtr.Zero) return IntPtr.Zero;

            return uniCncmarkGetOperator(m_hWidget);
        }

        public bool Learn(IntPtr hImage, double machPosX, double machPosY, ref int pErrCode)
        {
            if (this.m_hWidget == IntPtr.Zero) return false;

            return uniCncmarkLearn(m_hWidget, hImage, machPosX, machPosY, ref pErrCode);
        }
        public bool Learn(KImage image, double machPosX, double machPosY, ref int pErrCode)
        {
            if (null == image) return false;

            return Learn(image.GetHandle(), machPosX, machPosY, ref pErrCode);
        }


        public bool Locate(IntPtr hImage, ref RvPointF32 pResult, ref int pErrCode)
        {
            if (this.m_hWidget == IntPtr.Zero) return false;

            return uniCncmarkLocate(m_hWidget, hImage, ref pResult, ref pErrCode);
        }
        public bool Locate(KImage image, ref RvPointF32 pResult, ref int pErrCode)
        {
            if (null == image) return false;

            return Locate(image.GetHandle(), ref pResult, ref pErrCode);
        }

        public bool GetResult(ref RvPointF32 pResult)
        {
            if (this.m_hWidget == IntPtr.Zero) return false;

            return uniCncmarkGetResult(m_hWidget, ref pResult);
        }
        //public override void SetStyle(int style)
        //{
        //    if (this.m_hWidget == IntPtr.Zero) return;

        //    uniCncmarkSetStyle(m_hWidget, style);
        //}


        public void ClearPatterns()
        {
            if (this.m_hWidget == IntPtr.Zero) return;
            uniCncmarkClearPatterns(m_hWidget);
        }
        public void ClearResult()
        {
            if (this.m_hWidget == IntPtr.Zero) return;
            uniCncmarkClearResult(m_hWidget);
        }
        public IntPtr GetLearnedSceneImage()
        {
            if (this.m_hWidget == IntPtr.Zero) return IntPtr.Zero;
            return uniCncmarkGetLearnedSceneImage(m_hWidget);
        }
        public IntPtr GetLearnedPatternImage()
        {
            if (this.m_hWidget == IntPtr.Zero) return IntPtr.Zero;
            return uniCncmarkGetLearnedPatternImage(m_hWidget);
        }

        public void DestroyLearnedImages()
        {
            if (this.m_hWidget == IntPtr.Zero) return;
            uniCncmarkDestroyLearnedImages(m_hWidget);
        }
        public bool IsPatternLearnt()
        {
            if (this.m_hWidget == IntPtr.Zero) return false;
            return uniCncmarkIsPatternLearnt(m_hWidget);
        }
        public RvPointF64 GetLearntMachPos()
        {
            if (this.m_hWidget == IntPtr.Zero) return new RvPointF64();
            return uniCncmarkGetLearntMachPos(m_hWidget);
        }

        public RvPointF32 GetLearntImagePos()
        {
            if (this.m_hWidget == IntPtr.Zero) return new RvPointF32();
            return uniCncmarkGetLearntImagePos(m_hWidget);
        }
        //score范围： 0-1.0
        public void SetConfidence(double score)
        {
            if (this.m_hWidget == IntPtr.Zero) return;
            uniCncmarkSetConfidence(m_hWidget, score);
        }
        public double GetConfidence()
        {
            if (this.m_hWidget == IntPtr.Zero) return 0;
            return uniCncmarkGetConfidence(m_hWidget);
        }

        public void SetCrossSize(int size)
        {
            if (this.m_hWidget == IntPtr.Zero) return;
            uniCncmarkSetCrossSize(m_hWidget, size);
        }
        public int GetCrossSize()
        {
            if (this.m_hWidget == IntPtr.Zero) return 0;
            return uniCncmarkGetCrossSize(m_hWidget);
        }

        public RvSize GetPatternZoneSize()
        {
            if (this.m_hWidget == IntPtr.Zero) return new RvSize();

            return uniCncmarkGetPatternZoneSize(m_hWidget);
        }
        public RvSize GetSearchZoneSize()
        {
            if (this.m_hWidget == IntPtr.Zero) return new RvSize();

            return uniCncmarkGetSearchZoneSize(m_hWidget);
        }

        public RvRect GetPatternRect()
        {
            if (this.m_hWidget == IntPtr.Zero) return new RvRect();

            return uniCncmarkGetPatternRect(m_hWidget);
        }

        public void SetPatternRect(RvRect rect)
        {
            if (this.m_hWidget == IntPtr.Zero) return;

            uniCncmarkSetPatternRect(m_hWidget, rect);
        }

        public RvRect GetSearchRect()
        {
            if (this.m_hWidget == IntPtr.Zero) return new RvRect();

            return uniCncmarkGetSearchRect(m_hWidget);
        }

        public void SetSearchRect(RvRect rect)
        {
            if (this.m_hWidget == IntPtr.Zero) return;

            uniCncmarkSetSearchRect(m_hWidget, rect);
        }

        public KImage Snapshot(KImage image, bool bSearchArea)
        {
            if (this.m_hWidget == IntPtr.Zero) return null;

            IntPtr h = uniCncmarkSnapshot(m_hWidget, image.GetHandle(), bSearchArea);

            if (h == IntPtr.Zero) return null;

            KImage im = new KImage(h);

            return im.Clone();
        }



    }

    public class CncPolyline : KCncobj
    {
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjSetSublineStyle(IntPtr hCncobj, int index, int type, int thick, RvRgba color);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjSetSublineType(IntPtr hCncobj, int index, int type);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjSetSublineThick(IntPtr hCncobj, int index, int thick);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjSetSublineColor(IntPtr hCncobj, int index, RvRgba color);
        [DllImport("UniVision.dll")]
        private static extern int uniCncobjGetSublineType(IntPtr hCncobj, int index);
        [DllImport("UniVision.dll")]
        private static extern RvRgba uniCncobjGetSublineColor(IntPtr hCncobj, int index);
        [DllImport("UniVision.dll")]
        private static extern int uniCncobjGetSublineThick(IntPtr hCncobj, int index);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjSetPolylineClose(IntPtr hCncobj, bool flag);
        [DllImport("UniVision.dll")]
        private static extern bool uniCncobjIsPolylineClosed(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjSetVertexCount(IntPtr hCncobj, int count);
        [DllImport("UniVision.dll")]
        private static extern int uniCncobjGetVertexCount(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjSetVertexPos(IntPtr hCncobj, int index, RvPointF32 pos);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjSetVertexArray(IntPtr hCncobj, IntPtr pVertexArray, int count);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjAppendVertexArray(IntPtr hCncobj, IntPtr pVertexArray, int count);
        [DllImport("UniVision.dll")]
        private static extern RvPointF32 uniCncobjGetVertexPos(IntPtr hCncobj, int index);
        [DllImport("UniVision.dll")]
        private static extern float uniCncobjGetTotalLength(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern IntPtr uniCncobjGetVertexArray(IntPtr hCncobj);
        [DllImport("UniVision.dll")]
        private static extern int uniCncobjGetExtenderSize(IntPtr hCncobj, int index);
        [DllImport("UniVision.dll")]
        private static extern int uniCncobjGetExtenderShape(IntPtr hCncobj, int index);
        [DllImport("UniVision.dll")]
        private static extern RvRgba uniCncobjGetExtenderColor(IntPtr hCncobj, int index);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjSetExtenderSize(IntPtr hCncobj, int index, int size);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjSetExtenderShape(IntPtr hCncobj, int index, int shape);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjSetExtenderColor(IntPtr hCncobj, int index, RvRgba color);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjInsertVertex(IntPtr hCncobj, int index, float x, float y);
        [DllImport("UniVision.dll")]
        private static extern void uniCncobjRemoveSubline(IntPtr hCncobj, int index);

        public void SetSublineStyle(int index, int type, int thick, RvRgba color)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniCncobjSetSublineStyle(m_hWidget, index, type, thick, color);
            }
        }
        public void SetSublineType(int index, int type)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniCncobjSetSublineType(m_hWidget, index, type);
            }
        }
        public void SetSublineThick(int index, int thick)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniCncobjSetSublineThick(m_hWidget, index, thick);
            }
        }
        public void SetSublineColor(int index, RvRgba color)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniCncobjSetSublineColor(m_hWidget, index, color);
            }
        }

        public LinePattern GetSublineType(int index)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return (LinePattern)uniCncobjGetSublineType(m_hWidget, index);
            }
            return LinePattern.Solid;
        }
        public RvRgba GetSublineColor(int index)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniCncobjGetSublineColor(m_hWidget, index);
            }
            return new RvRgba();
        }
        public int GetSublineThick(int index)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniCncobjGetSublineThick(m_hWidget, index);
            }
            return 0;
        }

        public void SetPolylineClose(bool flag)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniCncobjSetPolylineClose(m_hWidget, flag);
            }
        }

        public bool IsPolylineClosed()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniCncobjIsPolylineClosed(m_hWidget);
            }
            return false;
        }

        public void SetVertexCount(int count)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniCncobjSetVertexCount(m_hWidget, count);
            }
        }

        public int GetVertexCount()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniCncobjGetVertexCount(m_hWidget);
            }
            return 0;
        }

        public void SetVertexPos(int index, RvPointF32 pos)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniCncobjSetVertexPos(m_hWidget, index, pos);
            }

        }

        public void SetVertexArray(RvPointF32[] vertexArray)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                //  IntPtr pVertexArray, int count;
                int cnt = vertexArray.Length;

                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RvPointF32)) * cnt);

                for (int i = 0; i < cnt; i++)
                {
                    IntPtr ptrtmp = (IntPtr)(ptr + i * Marshal.SizeOf(typeof(RvPointF32)));
                    Marshal.StructureToPtr(vertexArray[i], ptrtmp, false);
                }

                uniCncobjSetVertexArray(m_hWidget, ptr, cnt);

                Marshal.FreeHGlobal(ptr);
            }

        }

        public void AppendVertexArray(RvPointF32[] vertexArray)
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

                uniCncobjAppendVertexArray(m_hWidget, ptr, cnt);

                Marshal.FreeHGlobal(ptr);
            }

        }

        public RvPointF32 GetVertexPos(int index)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniCncobjGetVertexPos(m_hWidget, index);
            }

            return new RvPointF32();
        }

        public RvPointF32[] GetVertexArray()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                IntPtr h = uniCncobjGetVertexArray(m_hWidget);
                int cnt = uniCncobjGetVertexCount(m_hWidget);
                if (h != IntPtr.Zero)
                {
                    Pool.Assert(cnt > 0);

                    RvPointF32[] arr = new RvPointF32[cnt];
                    for (int i = 0; i < cnt; i++)
                    {
                        IntPtr ptr = new IntPtr(h.ToInt64() + Marshal.SizeOf(typeof(RvPointF32)) * i);
                        arr[i] = (RvPointF32)Marshal.PtrToStructure(ptr, typeof(RvPointF32));
                    }

                    return arr;
                }

            }

            return null;
        }

        public float GetTotalLength()
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniCncobjGetTotalLength(m_hWidget);
            }

            return 0;
        }

        public int GetExtenderSize(int index)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniCncobjGetExtenderSize(m_hWidget, index);
            }

            return 0;
        }

        public int GetExtenderShape(int index)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniCncobjGetExtenderShape(m_hWidget, index);
            }

            return 0;
        }
        public RvRgba GetExtenderColor(int index)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                return uniCncobjGetExtenderColor(m_hWidget, index);
            }

            return new RvRgba();
        }

        public void SetExtenderSize(int index, int size)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniCncobjSetExtenderSize(m_hWidget, index, size);
            }
        }

        public void SetExtenderShape(int index, int shape)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniCncobjSetExtenderShape(m_hWidget, index, shape);
            }
        }

        public void SetExtenderColor(int index, RvRgba color)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniCncobjSetExtenderColor(m_hWidget, index, color);
            }
        }

        //index表示顶点索引
        public void InsertVertex(int index, float x, float y)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniCncobjInsertVertex(m_hWidget, index, x, y);
            }
        }

        //index 表示线段
        public void RemoveSubline(int index)
        {
            if (m_hWidget != IntPtr.Zero)
            {
                uniCncobjRemoveSubline(m_hWidget, index);
            }
        }


    }
}
