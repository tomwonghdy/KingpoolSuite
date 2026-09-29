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

using Kingpool.Core;

//Smart Math
namespace Kingpool.Utility
{

    using RvColor = System.UInt32;

    public enum ColorSpace
    {
        Default = 0,
        HSV = 1,
        YCbCr = 2,
        YUV = 3,
    }

    /// <summary>
    /// Static math utility class providing geometry calculations.
    /// All public methods are wrappers around unmanaged functions from smath.dll.
    /// </summary>
    public static class Smath
    {
        // ====================================================================
        // Constants
        // ====================================================================

        public const int ORI_UNK = -1;
        public const int ORI_RIGHT = 1;
        public const int ORI_LEFT = 0;
        public const int ORI_INSIDE = 1;
        public const int ORI_OUTSIDE = 0;
        public const int ORI_CW = 1;
        public const int ORI_CCW = 0;


        // ====================================================================
        // Utility methods (inline helpers, not from DLL)
        // ====================================================================

        public static double NormalizeAngle360(double angle)
        {
            const double zero = 0.0;
            const double threeSixty = 360.0;
            while (angle >= threeSixty) angle -= threeSixty;
            while (angle < zero) angle += threeSixty;
            return angle;
        }

        public static double NormalizeAnglePi(double angle)
        {
            const double zero = 0.0;
            const double twoPi = 2.0 * Math.PI;
            while (angle >= twoPi) angle -= twoPi;
            while (angle < zero) angle += twoPi;
            return angle;
        }

        // ====================================================================
        // DLL import constants
        // ====================================================================

#if DEBUGGING_KINGPOOL_SUITE
        private const string LIB_NAME = "smath_d.dll";
#else
        private const string LIB_NAME = "smath.dll";
#endif

        // ====================================================================
        // Private unmanaged imports — Section 1: Arc / Ellipse
        // ====================================================================

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetArcCenter")]
        private static extern bool rvGetArcCenter(RvPoint p0, RvPoint p1, RvPoint p2, out RvPoint pCenter, out int pRadius);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetArcCenterE1")]
        private static extern bool rvGetArcCenterE1(RvPointF32 p0, RvPointF32 p1, RvPointF32 p2, out RvPointF32 pCenter, out float pRadius);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetArcCenterE2")]
        private static extern bool rvGetArcCenterE2(RvPointF64 p0, RvPointF64 p1, RvPointF64 p2, out RvPointF64 pCenter, out double pRadius);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsAdequateArc")]
        private static extern bool rvIsAdequateArc(RvPoint p1, RvPoint p2, RvPoint p3);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsAdequateArcE1")]
        private static extern bool rvIsAdequateArcE1(RvPointF32 p1, RvPointF32 p2, RvPointF32 p3);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsAdequateArcE2")]
        private static extern bool rvIsAdequateArcE2(RvPointF64 p1, RvPointF64 p2, RvPointF64 p3);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGenEllipse")]
        private static extern int rvGenEllipse(int cx, int cy, int radius0, int radius1, IntPtr pPointArray, int nArrayCount);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGenEllipseE1")]
        private static extern int rvGenEllipseE1(float cx, float cy, float radius0, float radius1, IntPtr pPointArray, int nArrayCount);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGenEllipseE2")]
        private static extern int rvGenEllipseE2(double cx, double cy, double radius0, double radius1, IntPtr pPointArray, int nArrayCount);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointInsideCircle")]
        private static extern RvBool rvIsPointInsideCircle(RvPointF64 p0, RvPointF64 p1, RvPointF64 p2, RvPointF64 outer);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointInsideCircleE1")]
        private static extern RvBool rvIsPointInsideCircleE1(RvPointF64 center, double radius, RvPointF64 outer);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointInsideCircleE2")]
        private static extern RvBool rvIsPointInsideCircleE2(RvPointF64 center, double rx, double ry, RvPointF64 outer);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvApproxEllipse")]
        private static extern bool rvApproxEllipse(IntPtr pEdgePoints, int count, out RvBox2D pBox2d, out double pAvgError, out double pMaxError, out double pMinError);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvApproxEllipseE1")]
        private static extern bool rvApproxEllipseE1(IntPtr pEdgePoints, int count, out RvBox2D pBox2d, out float pAvgError, out float pMaxError, out float pMinError);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvApproxEllipseE2")]
        private static extern bool rvApproxEllipseE2(IntPtr pEdgePoints, int count, out RvBox2D pBox2d, out double pAvgError, out double pMaxError, out double pMinError);

        // ====================================================================
        // Private unmanaged imports — Section 2: Point / Line
        // ====================================================================

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetNormalAngle")]
        private static extern double rvGetNormalAngle(double x0, double y0, double x1, double y1, bool bDegree);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetNormalAngleE1")]
        private static extern double rvGetNormalAngleE1(double y, double x, bool bDegree);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetNormalAngleE2")]
        private static extern float rvGetNormalAngleE2(float y, float x, bool bDegree);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRotateLine")]
        private static extern RvLine rvRotateLine(RvLine line, RvPoint center, float angle);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRotateLineE1")]
        private static extern RvLineF32 rvRotateLineE1(RvLineF32 line, RvPointF32 center, float angle);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRotateLineE2")]
        private static extern RvLineF64 rvRotateLineE2(RvLineF64 line, RvPointF64 center, double angle);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRotatePoint")]
        private static extern RvPoint rvRotatePoint(RvPoint point, int x, int y, float angle);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRotatePointE1")]
        private static extern RvPointF32 rvRotatePointE1(RvPointF32 point, float x, float y, float angle);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRotatePointE2")]
        private static extern RvPointF64 rvRotatePointE2(RvPointF64 point, double x, double y, double angle);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRotateVertex")]
        private static extern void rvRotateVertex(IntPtr pVertex, int count, int x, int y, float angle);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRotateVertexE1")]
        private static extern void rvRotateVertexE1(IntPtr pVertex, int count, float x, float y, float angle);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRotateVertexE2")]
        private static extern void rvRotateVertexE2(IntPtr pVertex, int count, double x, double y, double angle);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvScaleVertex")]
        private static extern void rvScaleVertex(IntPtr pVertex, int count, float scaleX, float scaleY);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvScaleVertexE1")]
        private static extern void rvScaleVertexE1(IntPtr pVertex, int count, float scaleX, float scaleY);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvScaleVertexE2")]
        private static extern void rvScaleVertexE2(IntPtr pVertex, int count, double scaleX, double scaleY);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPointAlongLine")]
        private static extern RvPoint rvCalcPointAlongLine(RvPoint start, RvPoint end, int dist);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPointAlongLineE1")]
        private static extern RvPointF32 rvCalcPointAlongLineE1(RvPointF32 start, RvPointF32 end, float dist);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPointAlongLineE2")]
        private static extern RvPointF64 rvCalcPointAlongLineE2(RvPointF64 start, RvPointF64 end, double dist);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPerpendicularPoint")]
        private static extern bool rvCalcPerpendicularPoint(RvLine line, RvPoint outer, out RvPoint pResult);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPerpendicularPointE1")]
        private static extern bool rvCalcPerpendicularPointE1(RvLineF32 line, RvPointF32 outer, out RvPointF32 pResult);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPerpendicularPointE2")]
        private static extern bool rvCalcPerpendicularPointE2(RvLineF64 line, RvPointF64 outer, out RvPointF64 pResult);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcLineAngle")]
        private static extern double rvCalcLineAngle(RvLine line0, RvLine line1, bool bDegree);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcLineAngleE1")]
        private static extern double rvCalcLineAngleE1(RvLineF32 line0, RvLineF32 line1, bool bDegree);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcLineAngleE2")]
        private static extern double rvCalcLineAngleE2(RvLineF64 line0, RvLineF64 line1, bool bDegree);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalc3PAngle")]
        private static extern double rvCalc3PAngle(RvPoint oripos, RvPoint start, RvPoint end, bool bDegree);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalc3PAngleE1")]
        private static extern double rvCalc3PAngleE1(RvPointF32 oripos, RvPointF32 start, RvPointF32 end, bool bDegree);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalc3PAngleE2")]
        private static extern double rvCalc3PAngleE2(RvPointF64 oripos, RvPointF64 start, RvPointF64 end, bool bDegree);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcIntersection")]
        private static extern bool rvCalcIntersection(RvLine line0, RvLine line1, out RvPoint pResult);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcIntersectionE1")]
        private static extern bool rvCalcIntersectionE1(RvLineF32 line0, RvLineF32 line1, out RvPointF32 pResult);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcIntersectionE2")]
        private static extern bool rvCalcIntersectionE2(RvLineF64 line0, RvLineF64 line1, out RvPointF64 pResult);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDeriveParallel")]
        private static extern RvLineF64 rvDeriveParallel(RvLineF64 line, double distance);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDeriveParallelE1")]
        private static extern RvLineF32 rvDeriveParallelE1(RvLineF32 line, float distance);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDeriveParallelE2")]
        private static extern RvLine rvDeriveParallelE2(RvLine line, int distance);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDeriveParallPoint")]
        private static extern RvPointF32 rvDeriveParallPoint(RvPointF32 start, RvPointF32 end, RvPointF32 outer);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDeriveParallPointE1")]
        private static extern RvPointF64 rvDeriveParallPointE1(RvPointF64 start, RvPointF64 end, RvPointF64 outer);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDeriveParallPointE2")]
        private static extern RvPoint rvDeriveParallPointE2(RvPoint start, RvPoint end, RvPoint outer);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDeriveMidPoint")]
        private static extern RvPoint rvDeriveMidPoint(RvPoint p0, RvPoint p1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDeriveMidPointE1")]
        private static extern RvPointF32 rvDeriveMidPointE1(RvPointF32 p0, RvPointF32 p1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDeriveMidPointE2")]
        private static extern RvPointF64 rvDeriveMidPointE2(RvPointF64 p0, RvPointF64 p1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDerivePerpend")]
        private static extern RvLine rvDerivePerpend(RvLine line, int length, bool bFullSize, bool bStartPos);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDerivePerpendE1")]
        private static extern RvLineF32 rvDerivePerpendE1(RvLineF32 line, float length, bool bFullSize, bool bStartPos);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDerivePerpendE2")]
        private static extern RvLineF64 rvDerivePerpendE2(RvLineF64 line, double length, bool bFullSize, bool bStartPos);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvExtendLine")]
        private static extern RvLine rvExtendLine(RvLine line, int length);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvExtendLineE1")]
        private static extern RvLineF32 rvExtendLineE1(RvLineF32 line, float length);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvExtendLineE2")]
        private static extern RvLineF64 rvExtendLineE2(RvLineF64 line, double length);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvTrimLine")]
        private static extern RvLine rvTrimLine(RvLine line, RvLine @base, int index);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvTrimLineE1")]
        private static extern RvLineF32 rvTrimLineE1(RvLineF32 line, RvLineF32 @base, int index);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvTrimLineE2")]
        private static extern RvLineF64 rvTrimLineE2(RvLineF64 line, RvLineF64 @base, int index);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPointToLineDist")]
        private static extern int rvCalcPointToLineDist(RvPoint p0, RvPoint p1, RvPoint outer, bool bUprightDist);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPointToLineDistE1")]
        private static extern float rvCalcPointToLineDistE1(RvPointF32 p0, RvPointF32 p1, RvPointF32 outer, bool bUprightDist);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPointToLineDistE2")]
        private static extern double rvCalcPointToLineDistE2(RvPointF64 p0, RvPointF64 p1, RvPointF64 outer, bool bUprightDist);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDistPointToLine")]
        private static extern double rvDistPointToLine(RvPoint point, RvLine line);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDistPointToLineE1")]
        private static extern double rvDistPointToLineE1(RvPointF32 point, RvLineF32 line);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDistPointToLineE2")]
        private static extern double rvDistPointToLineE2(RvPointF64 point, RvLineF64 line);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDistPointToPoint")]
        private static extern double rvDistPointToPoint(RvPoint point0, RvPoint point1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDistPointToPointE1")]
        private static extern double rvDistPointToPointE1(RvPointF32 point0, RvPointF32 point1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDistPointToPointE2")]
        private static extern double rvDistPointToPointE2(RvPointF64 point0, RvPointF64 point1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDistLineToLine")]
        private static extern double rvDistLineToLine(RvLine line1, RvLine line2);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDistLineToLineE1")]
        private static extern double rvDistLineToLineE1(RvLineF32 line1, RvLineF32 line2);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDistLineToLineE2")]
        private static extern double rvDistLineToLineE2(RvLineF64 line1, RvLineF64 line2);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsRightOfLine")]
        private static extern RvBool rvIsRightOfLine(RvLine line, RvPoint outer);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsRightOfLineE1")]
        private static extern RvBool rvIsRightOfLineE1(RvLineF32 line, RvPointF32 outer);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsRightOfLineE2")]
        private static extern RvBool rvIsRightOfLineE2(RvLineF64 line, RvPointF64 outer);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsClockWise")]
        private static extern RvBool rvIsClockWise(RvPoint start, RvPoint mid, RvPoint end);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsClockWiseE1")]
        private static extern RvBool rvIsClockWiseE1(RvPointF32 start, RvPointF32 mid, RvPointF32 end);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsClockWiseE2")]
        private static extern RvBool rvIsClockWiseE2(RvPointF64 start, RvPointF64 mid, RvPointF64 end);

        // ====================================================================
        // Private unmanaged imports — Section 3: Polyline / Polygon
        // ====================================================================

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvApproxLine")]
        private static extern int rvApproxLine(IntPtr pSrcSet, int sourceCount, IntPtr pDestSet, int destCount);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvApproxLineE1")]
        private static extern int rvApproxLineE1(IntPtr pSrcSet, int sourceCount, IntPtr pDestSet, int destCount);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvApproxLineE2")]
        private static extern int rvApproxLineE2(IntPtr pSrcSet, int sourceCount, IntPtr pDestSet, int destCount);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcBoundRect")]
        private static extern RvRect rvCalcBoundRect(IntPtr pVertex, int count);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcBoundRectE1")]
        private static extern RvRectF32 rvCalcBoundRectE1(IntPtr pVertex, int count);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcBoundRectE2")]
        private static extern RvRectF64 rvCalcBoundRectE2(IntPtr pVertex, int count);
        // ---- Point in polygon ----

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointInPolygon")]
        private static extern bool rvIsPointInPolygon(RvPoint point, IntPtr pPolygon, int nCount);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointInPolygonE1")]
        private static extern bool rvIsPointInPolygonE1(RvPointF32 point, IntPtr pPolygon, int nCount);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointInPolygonE2")]
        private static extern bool rvIsPointInPolygonE2(RvPointF64 point, IntPtr pPolygon, int nCount);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointInPolygonRing")]
        private static extern bool rvIsPointInPolygonRing(RvPoint point, IntPtr pInnerArray, int nInnerCount, IntPtr pOuterArray, int nOuterCount);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointInPolygonRingE1")]
        private static extern bool rvIsPointInPolygonRingE1(RvPointF32 point, IntPtr pInnerArray, int nInnerCount, IntPtr pOuterArray, int nOuterCount);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointInPolygonRingE2")]
        private static extern bool rvIsPointInPolygonRingE2(RvPointF64 point, IntPtr pInnerArray, int nInnerCount, IntPtr pOuterArray, int nOuterCount);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointOnPolygonBorder")]
        private static extern bool rvIsPointOnPolygonBorder(RvPoint point, IntPtr pVertexArray, int nVertexCount);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointOnPolygonBorderE1")]
        private static extern bool rvIsPointOnPolygonBorderE1(RvPointF32 point, IntPtr pVertexArray, int nVertexCount);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointOnPolygonBorderE2")]
        private static extern bool rvIsPointOnPolygonBorderE2(RvPointF64 point, IntPtr pVertexArray, int nVertexCount);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointOnPolyline")]
        private static extern bool rvIsPointOnPolyline(RvPoint point, IntPtr pVertexArray, int nVertexCount);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointOnPolylineE1")]
        private static extern bool rvIsPointOnPolylineE1(RvPointF32 point, IntPtr pVertexArray, int nVertexCount);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointOnPolylineE2")]
        private static extern bool rvIsPointOnPolylineE2(RvPointF64 point, IntPtr pVertexArray, int nVertexCount);

        //[DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointInPolygon")]
        //private static extern bool rvIsPointInPolygon(RvPoint point, IntPtr pPolygon, int nCount);

        //[DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointInPolygonE1")]
        //private static extern bool rvIsPointInPolygonE1(int x, int y, IntPtr pPolygon, int nCount);

        //[DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointInPolygonE2")]
        //private static extern bool rvIsPointInPolygonE2(RvPointF32 point, IntPtr pPolygon, int nCount);

        //[DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointInPolygonE3")]
        //private static extern bool rvIsPointInPolygonE3(float x, float y, IntPtr pPolygon, int nCount);

        //[DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointInPolygonE4")]
        //private static extern bool rvIsPointInPolygonE4(int x, int y, IntPtr pInnerArray, int nInnerCount, IntPtr pOuterArray, int nOuterCount);

        //[DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointInPolygonE5")]
        //private static extern bool rvIsPointInPolygonE5(int x, int y, IntPtr pVertexArray, int nVertexCount, out bool pOnBorder);

        //[DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointInPolygonE6")]
        //private static extern bool rvIsPointInPolygonE6(double x, double y, IntPtr pPolygon, int nCount);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPolygonOffset")]
        private static extern int rvPolygonOffset(IntPtr pVertexArray, int nArraySize, int distance, IntPtr pDestArray, int nDestSize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPolygonOffsetE1")]
        private static extern int rvPolygonOffsetE1(IntPtr pVertexArray, int nArraySize, float distance, IntPtr pDestArray, int nDestSize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPolygonOffsetE2")]
        private static extern int rvPolygonOffsetE2(IntPtr pVertexArray, int nArraySize, double distance, IntPtr pDestArray, int nDestSize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPolygonArea")]
        private static extern double rvCalcPolygonArea(IntPtr pVertexArray, int nArraySize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPolygonAreaE1")]
        private static extern double rvCalcPolygonAreaE1(IntPtr pVertexArray, int nArraySize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPolygonAreaE2")]
        private static extern double rvCalcPolygonAreaE2(IntPtr pVertexArray, int nArraySize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPolylineLength")]
        private static extern double rvCalcPolylineLength(IntPtr pVertexArray, int nArraySize, bool bClosed);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPolylineLengthE1")]
        private static extern double rvCalcPolylineLengthE1(IntPtr pVertexArray, int nArraySize, bool bClosed);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPolylineLengthE2")]
        private static extern double rvCalcPolylineLengthE2(IntPtr pVertexArray, int nArraySize, bool bClosed);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPolygonCenter")]
        private static extern RvPoint rvCalcPolygonCenter(IntPtr pVertexArray, int nArraySize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPolygonCenterE1")]
        private static extern RvPointF32 rvCalcPolygonCenterE1(IntPtr pVertexArray, int nArraySize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPolygonCenterE2")]
        private static extern RvPointF64 rvCalcPolygonCenterE2(IntPtr pVertexArray, int nArraySize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPointToPolylineDistance")]
        private static extern int rvCalcPointToPolylineDistance(RvPoint point, IntPtr pVertexArray, int nArraySize, bool bClosed);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPointToPolylineDistanceE1")]
        private static extern int rvCalcPointToPolylineDistanceE1(RvPointF32 point, IntPtr pVertexArray, int nArraySize, bool bClosed);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPointToPolylineDistanceE2")]
        private static extern int rvCalcPointToPolylineDistanceE2(RvPointF64 point, IntPtr pVertexArray, int nArraySize, bool bClosed);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPolylineMove")]
        private static extern void rvPolylineMove(IntPtr pVertexArray, int nArraySize, int dx, int dy);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPolylineMoveE1")]
        private static extern void rvPolylineMoveE1(IntPtr pVertexArray, int nArraySize, float dx, float dy);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPolylineMoveE2")]
        private static extern void rvPolylineMoveE2(IntPtr pVertexArray, int nArraySize, double dx, double dy);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetSubPolygonVertex")]
        private static extern int rvGetSubPolygonVertex(IntPtr pArrIn, int inCount, int startIndex, int endIndex, IntPtr pArrOut, int outCount);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetSubPolygonVertexE1")]
        private static extern int rvGetSubPolygonVertexE1(IntPtr pArrIn, int inCount, int startIndex, int endIndex, IntPtr pArrOut, int outCount);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetSubPolygonVertexE2")]
        private static extern int rvGetSubPolygonVertexE2(IntPtr pArrIn, int inCount, int startIndex, int endIndex, IntPtr pArrOut, int outCount);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetSubPolygonPerimeter")]
        private static extern double rvGetSubPolygonPerimeter(IntPtr pArrIn, int inCount, int startIndex, int endIndex);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetSubPolygonPerimeterE1")]
        private static extern double rvGetSubPolygonPerimeterE1(IntPtr pArrIn, int inCount, int startIndex, int endIndex);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetSubPolygonPerimeterE2")]
        private static extern double rvGetSubPolygonPerimeterE2(IntPtr pArrIn, int inCount, int startIndex, int endIndex);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsSubPolygon")]
        private static extern bool rvIsSubPolygon(IntPtr pArrBig, int bigCount, IntPtr pArrSmall, int smallCount, bool bIncludeBorder);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsSubPolygonE1")]
        private static extern bool rvIsSubPolygonE1(IntPtr pArrBig, int bigCount, IntPtr pArrSmall, int smallCount, bool bIncludeBorder);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsSubPolygonE2")]
        private static extern bool rvIsSubPolygonE2(IntPtr pArrBig, int bigCount, IntPtr pArrSmall, int smallCount, bool bIncludeBorder);

        // ====================================================================
        // Private unmanaged imports — Section 4: Pixel / Image
        // ====================================================================

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPitch")]
        public static extern int CalcPitch(int pixbits, int width);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcDepth")]
        public static extern int CalcDepth(PixelFormat type);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetBrighterPixel")]
        public static extern RvRgb GetBrighterPixel(RvRgb color, float percent);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetDarkerPixel")]
        public static extern RvRgb GetDarkerPixel(RvRgb color, float percent);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcPixelStrength")]
        public static extern int CalcPixelStrength(RvRgb color, ColorSpace method);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcOtsu")]
        private static extern int rvCalcOtsu(int maxValue, IntPtr pMagArr, int arrSize);

        // ====================================================================
        // Private unmanaged imports — Section 5: Rect / Box / Misc
        // ====================================================================

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvAdaptRect")]
        private static extern RvRect rvAdaptRect(RvRect container, int width, int height, ref double ratio);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvAdaptRectE1")]
        private static extern RvRect rvAdaptRectE1(RvRect container, int width, int height, bool bNoAlignCenter, ref double ratio);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvAdaptRectE2")]
        private static extern RvRect rvAdaptRectE2(RvRect container, int width, int height, ref int fitness, ref double ratio);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPower")]
        private static extern int rvPower(int @base, int index);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPowerE1")]
        private static extern float rvPowerE1(float @base, int index);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPowerE2")]
        private static extern double rvPowerE2(double @base, int index);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvBox2dToVertex")]
        private static extern int rvBox2dToVertex(RvBox2D box, IntPtr pPointArray, int size);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvBox2dToVertexE1")]
        private static extern int rvBox2dToVertexE1(RvBox2D box, IntPtr pPointArray, int size);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvBox2dToVertexE2")]
        private static extern int rvBox2dToVertexE2(RvBox2D box, IntPtr pPointArray, int size);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRectToVertex")]
        private static extern int rvRectToVertex(RvRect rect, IntPtr pPointArray, int size);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRectToVertexE1")]
        private static extern int rvRectToVertexE1(RvRect rect, IntPtr pPointArray, int size);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRectToVertexE2")]
        private static extern int rvRectToVertexE2(RvRect rect, IntPtr pPointArray, int size);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvBox2dToRect")]
        private static extern RvRect rvBox2dToRect(RvBox2D box);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvBox2dToRectE1")]
        private static extern RvRectF32 rvBox2dToRectE1(RvBox2D box);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvBox2dToRectE2")]
        private static extern RvRectF64 rvBox2dToRectE2(RvBox2D box);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsBox2dInRect")]
        private static extern bool rvIsBox2dInRect(RvBox2D box, RvRect rect);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsBox2dInRectE1")]
        private static extern bool rvIsBox2dInRectE1(RvBox2D box, RvRectF32 rect);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsBox2dInRectE2")]
        private static extern bool rvIsBox2dInRectE2(RvBox2D box, RvRectF64 rect);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetRectCenter")]
        private static extern RvPoint rvGetRectCenter(RvRect rect);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetRectCenterE1")]
        private static extern RvPointF32 rvGetRectCenterE1(RvRectF32 rect);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetRectCenterE2")]
        private static extern RvPointF64 rvGetRectCenterE2(RvRectF64 rect);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetRectSize")]
        private static extern RvSize rvGetRectSize(RvRect rect, bool bIncludeBorder);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetRectSizeE1")]
        private static extern RvSizeF32 rvGetRectSizeE1(RvRectF32 rect, bool bIncludeBorder);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetRectSizeE2")]
        private static extern RvSizeF64 rvGetRectSizeE2(RvRectF64 rect, bool bIncludeBorder);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetRectWidth")]
        private static extern int rvGetRectWidth(RvRect rect, bool bIncludeBorder);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetRectWidthE1")]
        private static extern int rvGetRectWidthE1(RvRectF32 rect, bool bIncludeBorder);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetRectWidthE2")]
        private static extern int rvGetRectWidthE2(RvRectF64 rect, bool bIncludeBorder);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetRectHeight")]
        private static extern int rvGetRectHeight(RvRect rect, bool bIncludeBorder);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetRectHeightE1")]
        private static extern int rvGetRectHeightE1(RvRectF32 rect, bool bIncludeBorder);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetRectHeightE2")]
        private static extern int rvGetRectHeightE2(RvRectF64 rect, bool bIncludeBorder);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRotateRect")]
        private static extern RvRect rvRotateRect(RvRect rect, double angle);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRotateRectE1")]
        private static extern RvRectF32 rvRotateRectE1(RvRectF32 rect, double angle);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRotateRectE2")]
        private static extern RvRectF64 rvRotateRectE2(RvRectF64 rect, double angle);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvMoveRect")]
        private static extern RvRect rvMoveRect(RvRect rect, int dx, int dy);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvMoveRectE1")]
        private static extern RvRectF32 rvMoveRectE1(RvRectF32 rect, int dx, int dy);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvMoveRectE2")]
        private static extern RvRectF64 rvMoveRectE2(RvRectF64 rect, int dx, int dy);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvScaleRect")]
        private static extern RvRect rvScaleRect(RvRect rect, double scale);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvScaleRectE1")]
        private static extern RvRect rvScaleRectE1(RvRect rect, double scaleX, double scaleY);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvScaleRectE2")]
        private static extern RvRect rvScaleRectE2(RvRect rect, double scale);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRectIntersect")]
        private static extern RvRect rvRectIntersect(RvRect rect, RvRect other);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRectIntersectE1")]
        private static extern RvRectF32 rvRectIntersectE1(RvRectF32 rect, RvRectF32 other);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRectIntersectE2")]
        private static extern RvRectF64 rvRectIntersectE2(RvRectF64 rect, RvRectF64 other);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRectUnion")]
        private static extern RvRect rvRectUnion(RvRect rect, RvRect other);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRectUnionE1")]
        private static extern RvRectF32 rvRectUnionE1(RvRectF32 rect, RvRectF32 other);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRectUnionE2")]
        private static extern RvRectF64 rvRectUnionE2(RvRectF64 rect, RvRectF64 other);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsRectEmpty")]
        private static extern bool rvIsRectEmpty(RvRect rect);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsRectEmptyE1")]
        private static extern bool rvIsRectEmptyE1(RvRectF32 rect);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsRectEmptyE2")]
        private static extern bool rvIsRectEmptyE2(RvRectF64 rect);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetMipUpScale")]
        private static extern int rvGetMipUpScale(int level);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetMipUpScaleE1")]
        private static extern float rvGetMipUpScaleE1(int level);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetMipUpScaleE2")]
        private static extern double rvGetMipUpScaleE2(int level);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetMipDownScale")]
        private static extern float rvGetMipDownScale(int level);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetMipDownScaleE1")]
        private static extern float rvGetMipDownScaleE1(int level);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetMipDownScaleE2")]
        private static extern double rvGetMipDownScaleE2(int level);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetMipScale")]
        private static extern float rvGetMipScale(int level, bool bOpposite);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetMipScaleE1")]
        private static extern float rvGetMipScaleE1(int level, bool bOpposite);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetMipScaleE2")]
        private static extern double rvGetMipScaleE2(int level, bool bOpposite);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsNullRect")]
        private static extern bool rvIsNullRect(RvRect rect);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsNullRectE1")]
        private static extern bool rvIsNullRectE1(RvRectF32 rect);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsNullRectE2")]
        private static extern bool rvIsNullRectE2(RvRectF64 rect);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsRectEqual")]
        private static extern bool rvIsRectEqual(RvRect rect0, RvRect rect1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsRectEqualE1")]
        private static extern bool rvIsRectEqualE1(RvRectF32 rect0, RvRectF32 rect1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsRectEqualE2")]
        private static extern bool rvIsRectEqualE2(RvRectF64 rect0, RvRectF64 rect1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvNormalizeRect")]
        private static extern void rvNormalizeRect(ref RvRect pRect);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvNormalizeRectE1")]
        private static extern void rvNormalizeRectE1(ref RvRectF32 pRect);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvNormalizeRectE2")]
        private static extern void rvNormalizeRectE2(ref RvRectF64 pRect);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRectShrink")]
        private static extern void rvRectShrink(ref RvRect pRect, int dx, int dy);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRectShrinkE1")]
        private static extern void rvRectShrinkE1(ref RvRectF32 pRect, int dx, int dy);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRectShrinkE2")]
        private static extern void rvRectShrinkE2(ref RvRectF64 pRect, int dx, int dy);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRectDeflate")]
        private static extern void rvRectDeflate(ref RvRect pRect, int dx, int dy);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRectDeflateE1")]
        private static extern void rvRectDeflateE1(ref RvRectF32 pRect, int dx, int dy);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRectDeflateE2")]
        private static extern void rvRectDeflateE2(ref RvRectF64 pRect, int dx, int dy);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCompareRect")]
        private static extern int rvCompareRect(RvRect rect0, RvRect rect1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCompareRectE1")]
        private static extern int rvCompareRectE1(RvRectF32 rect0, RvRectF32 rect1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCompareRectE2")]
        private static extern int rvCompareRectE2(RvRectF64 rect0, RvRectF64 rect1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointInsideRect")]
        private static extern RvBool rvIsPointInsideRect(RvRect rect, RvPoint pos, bool bOnBorderOnly);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointInsideRectE1")]
        private static extern RvBool rvIsPointInsideRectE1(RvRectF32 rect, RvPointF32 pos, bool bOnBorderOnly);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsPointInsideRectE2")]
        private static extern RvBool rvIsPointInsideRectE2(RvRectF64 rect, RvPointF64 pos, bool bOnBorderOnly);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsRectSurround")]
        private static extern RvBool rvIsRectSurround(RvRect rect0, RvRect rect1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsRectSurroundE1")]
        private static extern RvBool rvIsRectSurroundE1(RvRectF32 rect0, RvRectF32 rect1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIsRectSurroundE2")]
        private static extern RvBool rvIsRectSurroundE2(RvRectF64 rect0, RvRectF64 rect1);

        // ====================================================================
        // Public wrappers — Section 1: Arc / Ellipse
        // ====================================================================

        public static bool GetArcCenter(RvPoint p0, RvPoint p1, RvPoint p2, out RvPoint pCenter, out int pRadius)
            => rvGetArcCenter(p0, p1, p2, out pCenter, out pRadius);

        public static bool GetArcCenter(RvPointF32 p0, RvPointF32 p1, RvPointF32 p2, out RvPointF32 pCenter, out float pRadius)
            => rvGetArcCenterE1(p0, p1, p2, out pCenter, out pRadius);

        public static bool GetArcCenter(RvPointF64 p0, RvPointF64 p1, RvPointF64 p2, out RvPointF64 pCenter, out double pRadius)
            => rvGetArcCenterE2(p0, p1, p2, out pCenter, out pRadius);

        public static bool IsAdequateArc(RvPoint p1, RvPoint p2, RvPoint p3) => rvIsAdequateArc(p1, p2, p3);
        public static bool IsAdequateArc(RvPointF32 p1, RvPointF32 p2, RvPointF32 p3) => rvIsAdequateArcE1(p1, p2, p3);
        public static bool IsAdequateArc(RvPointF64 p1, RvPointF64 p2, RvPointF64 p3) => rvIsAdequateArcE2(p1, p2, p3);

        public static RvBool IsPointInsideCircle(RvPointF64 p0, RvPointF64 p1, RvPointF64 p2, RvPointF64 outer)
            => rvIsPointInsideCircle(p0, p1, p2, outer);

        public static RvBool IsPointInsideCircle(RvPointF64 center, double radius, RvPointF64 outer)
            => rvIsPointInsideCircleE1(center, radius, outer);

        public static RvBool IsPointInsideCircle(RvPointF64 center, double rx, double ry, RvPointF64 outer)
            => rvIsPointInsideCircleE2(center, rx, ry, outer);

        public static int GenEllipse(int cx, int cy, int radius0, int radius1, RvPoint[] pointArray)
        {
            if (pointArray == null || pointArray.Length == 0) return 0;
            int nArrayCount = pointArray.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(nArrayCount * elemSize);
            try
            {
                int result = rvGenEllipse(cx, cy, radius0, radius1, ptr, nArrayCount);
                if (result > 0)
                    for (int i = 0; i < result; i++)
                        pointArray[i] = Marshal.PtrToStructure<RvPoint>(IntPtr.Add(ptr, i * elemSize));
                return result;
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static int GenEllipse(float cx, float cy, float radius0, float radius1, RvPointF32[] pointArray)
        {
            if (pointArray == null || pointArray.Length == 0) return 0;
            int nArrayCount = pointArray.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(nArrayCount * elemSize);
            try
            {
                int result = rvGenEllipseE1(cx, cy, radius0, radius1, ptr, nArrayCount);
                if (result > 0)
                    for (int i = 0; i < result; i++)
                        pointArray[i] = Marshal.PtrToStructure<RvPointF32>(IntPtr.Add(ptr, i * elemSize));
                return result;
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static int GenEllipse(double cx, double cy, double radius0, double radius1, RvPointF64[] pointArray)
        {
            if (pointArray == null || pointArray.Length == 0) return 0;
            int nArrayCount = pointArray.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF64));
            IntPtr ptr = Marshal.AllocHGlobal(nArrayCount * elemSize);
            try
            {
                int result = rvGenEllipseE2(cx, cy, radius0, radius1, ptr, nArrayCount);
                if (result > 0)
                    for (int i = 0; i < result; i++)
                        pointArray[i] = Marshal.PtrToStructure<RvPointF64>(IntPtr.Add(ptr, i * elemSize));
                return result;
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static bool ApproxEllipse(RvPoint[] edgePoints, out RvBox2D box2d, out double avgError, out double maxError, out double minError)
        {
            box2d = new RvBox2D();
            avgError = maxError = minError = 0;
            if (edgePoints == null || edgePoints.Length == 0) return false;
            int count = edgePoints.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
            try
            {
                for (int i = 0; i < count; i++)
                    Marshal.StructureToPtr(edgePoints[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvApproxEllipse(ptr, count, out box2d, out avgError, out maxError, out minError);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static bool ApproxEllipse(RvPointF32[] edgePoints, out RvBox2D box2d, out float avgError, out float maxError, out float minError)
        {
            box2d = new RvBox2D();
            avgError = maxError = minError = 0;
            if (edgePoints == null || edgePoints.Length == 0) return false;
            int count = edgePoints.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
            try
            {
                for (int i = 0; i < count; i++)
                    Marshal.StructureToPtr(edgePoints[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvApproxEllipseE1(ptr, count, out box2d, out avgError, out maxError, out minError);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static bool ApproxEllipse(RvPointF64[] edgePoints, out RvBox2D box2d, out double avgError, out double maxError, out double minError)
        {
            box2d = new RvBox2D();
            avgError = maxError = minError = 0;
            if (edgePoints == null || edgePoints.Length == 0) return false;
            int count = edgePoints.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF64));
            IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
            try
            {
                for (int i = 0; i < count; i++)
                    Marshal.StructureToPtr(edgePoints[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvApproxEllipseE2(ptr, count, out box2d, out avgError, out maxError, out minError);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        // ====================================================================
        // Public wrappers — Section 2: Point / Line
        // ====================================================================

        public static double GetNormalAngle(double x0, double y0, double x1, double y1, bool bDegree = false)
            => rvGetNormalAngle(x0, y0, x1, y1, bDegree);

        public static double GetNormalAngleE1(double y, double x, bool bDegree = false)
            => rvGetNormalAngleE1(y, x, bDegree);

        public static float GetNormalAngleE2(float y, float x, bool bDegree = false)
            => rvGetNormalAngleE2(y, x, bDegree);

        public static RvLine RotateLine(RvLine line, RvPoint center, float angle) => rvRotateLine(line, center, angle);
        public static RvLineF32 RotateLine(RvLineF32 line, RvPointF32 center, float angle) => rvRotateLineE1(line, center, angle);
        public static RvLineF64 RotateLine(RvLineF64 line, RvPointF64 center, double angle) => rvRotateLineE2(line, center, angle);

        public static RvPoint RotatePoint(RvPoint point, int x, int y, float angle) => rvRotatePoint(point, x, y, angle);
        public static RvPointF32 RotatePoint(RvPointF32 point, float x, float y, float angle) => rvRotatePointE1(point, x, y, angle);
        public static RvPointF64 RotatePoint(RvPointF64 point, double x, double y, double angle) => rvRotatePointE2(point, x, y, angle);

        public static RvPoint CalcPointAlongLine(RvPoint start, RvPoint end, int dist) => rvCalcPointAlongLine(start, end, dist);
        public static RvPointF32 CalcPointAlongLine(RvPointF32 start, RvPointF32 end, float dist) => rvCalcPointAlongLineE1(start, end, dist);
        public static RvPointF64 CalcPointAlongLine(RvPointF64 start, RvPointF64 end, double dist) => rvCalcPointAlongLineE2(start, end, dist);

        public static bool CalcPerpendicularPoint(RvLine line, RvPoint outer, out RvPoint pResult)
            => rvCalcPerpendicularPoint(line, outer, out pResult);

        public static bool CalcPerpendicularPoint(RvLineF32 line, RvPointF32 outer, out RvPointF32 pResult)
            => rvCalcPerpendicularPointE1(line, outer, out pResult);

        public static bool CalcPerpendicularPoint(RvLineF64 line, RvPointF64 outer, out RvPointF64 pResult)
            => rvCalcPerpendicularPointE2(line, outer, out pResult);

        public static double CalcLineAngle(RvLine line0, RvLine line1, bool bDegree) => rvCalcLineAngle(line0, line1, bDegree);
        public static double CalcLineAngle(RvLineF32 line0, RvLineF32 line1, bool bDegree) => rvCalcLineAngleE1(line0, line1, bDegree);
        public static double CalcLineAngle(RvLineF64 line0, RvLineF64 line1, bool bDegree) => rvCalcLineAngleE2(line0, line1, bDegree);

        public static double Calc3PAngle(RvPoint oripos, RvPoint start, RvPoint end, bool bDegree) => rvCalc3PAngle(oripos, start, end, bDegree);
        public static double Calc3PAngle(RvPointF32 oripos, RvPointF32 start, RvPointF32 end, bool bDegree) => rvCalc3PAngleE1(oripos, start, end, bDegree);
        public static double Calc3PAngle(RvPointF64 oripos, RvPointF64 start, RvPointF64 end, bool bDegree) => rvCalc3PAngleE2(oripos, start, end, bDegree);

        public static bool CalcIntersection(RvLine line0, RvLine line1, out RvPoint pResult) => rvCalcIntersection(line0, line1, out pResult);
        public static bool CalcIntersection(RvLineF32 line0, RvLineF32 line1, out RvPointF32 pResult) => rvCalcIntersectionE1(line0, line1, out pResult);
        public static bool CalcIntersection(RvLineF64 line0, RvLineF64 line1, out RvPointF64 pResult) => rvCalcIntersectionE2(line0, line1, out pResult);

        public static RvLineF64 DeriveParallel(RvLineF64 line, double distance) => rvDeriveParallel(line, distance);
        public static RvLineF32 DeriveParallel(RvLineF32 line, float distance) => rvDeriveParallelE1(line, distance);
        public static RvLine DeriveParallel(RvLine line, int distance) => rvDeriveParallelE2(line, distance);

        public static RvPointF32 DeriveParallPoint(RvPointF32 start, RvPointF32 end, RvPointF32 outer) => rvDeriveParallPoint(start, end, outer);
        public static RvPointF64 DeriveParallPoint(RvPointF64 start, RvPointF64 end, RvPointF64 outer) => rvDeriveParallPointE1(start, end, outer);
        public static RvPoint DeriveParallPoint(RvPoint start, RvPoint end, RvPoint outer) => rvDeriveParallPointE2(start, end, outer);

        public static RvPoint DeriveMidPoint(RvPoint p0, RvPoint p1) => rvDeriveMidPoint(p0, p1);
        public static RvPointF32 DeriveMidPoint(RvPointF32 p0, RvPointF32 p1) => rvDeriveMidPointE1(p0, p1);
        public static RvPointF64 DeriveMidPoint(RvPointF64 p0, RvPointF64 p1) => rvDeriveMidPointE2(p0, p1);




        public static RvLine DerivePerpend(RvLine line, int length, bool bFullSize, bool bStartPos) => rvDerivePerpend(line, length, bFullSize, bStartPos);
        public static RvLineF32 DerivePerpend(RvLineF32 line, float length, bool bFullSize, bool bStartPos) => rvDerivePerpendE1(line, length, bFullSize, bStartPos);
        public static RvLineF64 DerivePerpend(RvLineF64 line, double length, bool bFullSize, bool bStartPos) => rvDerivePerpendE2(line, length, bFullSize, bStartPos);

        public static RvLine ExtendLine(RvLine line, int length) => rvExtendLine(line, length);
        public static RvLineF32 ExtendLine(RvLineF32 line, float length) => rvExtendLineE1(line, length);
        public static RvLineF64 ExtendLine(RvLineF64 line, double length) => rvExtendLineE2(line, length);

        public static RvLine TrimLine(RvLine line, RvLine @base, int index) => rvTrimLine(line, @base, index);
        public static RvLineF32 TrimLine(RvLineF32 line, RvLineF32 @base, int index) => rvTrimLineE1(line, @base, index);
        public static RvLineF64 TrimLine(RvLineF64 line, RvLineF64 @base, int index) => rvTrimLineE2(line, @base, index);

        public static int CalcPointToLineDist(RvPoint p0, RvPoint p1, RvPoint outer, bool bUprightDist) => rvCalcPointToLineDist(p0, p1, outer, bUprightDist);
        public static float CalcPointToLineDist(RvPointF32 p0, RvPointF32 p1, RvPointF32 outer, bool bUprightDist) => rvCalcPointToLineDistE1(p0, p1, outer, bUprightDist);
        public static double CalcPointToLineDist(RvPointF64 p0, RvPointF64 p1, RvPointF64 outer, bool bUprightDist) => rvCalcPointToLineDistE2(p0, p1, outer, bUprightDist);

        public static double DistPointToLine(RvPoint point, RvLine line) => rvDistPointToLine(point, line);
        public static double DistPointToLine(RvPointF32 point, RvLineF32 line) => rvDistPointToLineE1(point, line);
        public static double DistPointToLine(RvPointF64 point, RvLineF64 line) => rvDistPointToLineE2(point, line);

        public static double DistPointToPoint(RvPoint point0, RvPoint point1) => rvDistPointToPoint(point0, point1);
        public static double DistPointToPoint(RvPointF32 point0, RvPointF32 point1) => rvDistPointToPointE1(point0, point1);
        public static double DistPointToPoint(RvPointF64 point0, RvPointF64 point1) => rvDistPointToPointE2(point0, point1);

        public static double DistLineToLine(RvLine line1, RvLine line2) => rvDistLineToLine(line1, line2);
        public static double DistLineToLine(RvLineF32 line1, RvLineF32 line2) => rvDistLineToLineE1(line1, line2);
        public static double DistLineToLine(RvLineF64 line1, RvLineF64 line2) => rvDistLineToLineE2(line1, line2);

        public static RvBool IsRightOfLine(RvLine line, RvPoint outer) => rvIsRightOfLine(line, outer);
        public static RvBool IsRightOfLine(RvLineF32 line, RvPointF32 outer) => rvIsRightOfLineE1(line, outer);
        public static RvBool IsRightOfLine(RvLineF64 line, RvPointF64 outer) => rvIsRightOfLineE2(line, outer);

        public static RvBool IsClockWise(RvPoint start, RvPoint mid, RvPoint end) => rvIsClockWise(start, mid, end);
        public static RvBool IsClockWise(RvPointF32 start, RvPointF32 mid, RvPointF32 end) => rvIsClockWiseE1(start, mid, end);
        public static RvBool IsClockWise(RvPointF64 start, RvPointF64 mid, RvPointF64 end) => rvIsClockWiseE2(start, mid, end);

        // ====================================================================
        // Public wrappers — Section 3: Polyline / Polygon (array marshaling)
        // ====================================================================

        public static void RotateVertex(RvPoint[] vertices, int x, int y, float angle)
        {
            if (vertices == null || vertices.Length == 0) return;
            int count = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
            try
            {
                for (int i = 0; i < count; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                rvRotateVertex(ptr, count, x, y, angle);
                for (int i = 0; i < count; i++)
                    vertices[i] = Marshal.PtrToStructure<RvPoint>(IntPtr.Add(ptr, i * elemSize));
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static void RotateVertex(RvPointF32[] vertices, float x, float y, float angle)
        {
            if (vertices == null || vertices.Length == 0) return;
            int count = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
            try
            {
                for (int i = 0; i < count; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                rvRotateVertexE1(ptr, count, x, y, angle);
                for (int i = 0; i < count; i++)
                    vertices[i] = Marshal.PtrToStructure<RvPointF32>(IntPtr.Add(ptr, i * elemSize));
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static void RotateVertex(RvPointF64[] vertices, double x, double y, double angle)
        {
            if (vertices == null || vertices.Length == 0) return;
            int count = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF64));
            IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
            try
            {
                for (int i = 0; i < count; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                rvRotateVertexE2(ptr, count, x, y, angle);
                for (int i = 0; i < count; i++)
                    vertices[i] = Marshal.PtrToStructure<RvPointF64>(IntPtr.Add(ptr, i * elemSize));
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static void ScaleVertex(RvPoint[] vertices, float scaleX, float scaleY)
        {
            if (vertices == null || vertices.Length == 0) return;
            int count = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
            try
            {
                for (int i = 0; i < count; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                rvScaleVertex(ptr, count, scaleX, scaleY);
                for (int i = 0; i < count; i++)
                    vertices[i] = Marshal.PtrToStructure<RvPoint>(IntPtr.Add(ptr, i * elemSize));
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static void ScaleVertex(RvPointF32[] vertices, float scaleX, float scaleY)
        {
            if (vertices == null || vertices.Length == 0) return;
            int count = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
            try
            {
                for (int i = 0; i < count; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                rvScaleVertexE1(ptr, count, scaleX, scaleY);
                for (int i = 0; i < count; i++)
                    vertices[i] = Marshal.PtrToStructure<RvPointF32>(IntPtr.Add(ptr, i * elemSize));
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static void ScaleVertex(RvPointF64[] vertices, double scaleX, double scaleY)
        {
            if (vertices == null || vertices.Length == 0) return;
            int count = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF64));
            IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
            try
            {
                for (int i = 0; i < count; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                rvScaleVertexE2(ptr, count, scaleX, scaleY);
                for (int i = 0; i < count; i++)
                    vertices[i] = Marshal.PtrToStructure<RvPointF64>(IntPtr.Add(ptr, i * elemSize));
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static int ApproxLine(RvPoint[] srcSet, RvPoint[] destSet)
        {
            if (srcSet == null || destSet == null || srcSet.Length == 0 || destSet.Length == 0) return 0;
            int srcCount = srcSet.Length;
            int dstCount = destSet.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptrSrc = Marshal.AllocHGlobal(srcCount * elemSize);
            IntPtr ptrDst = Marshal.AllocHGlobal(dstCount * elemSize);
            try
            {
                for (int i = 0; i < srcCount; i++)
                    Marshal.StructureToPtr(srcSet[i], IntPtr.Add(ptrSrc, i * elemSize), false);
                int result = rvApproxLine(ptrSrc, srcCount, ptrDst, dstCount);
                for (int i = 0; i < Math.Min(result, dstCount); i++)
                    destSet[i] = Marshal.PtrToStructure<RvPoint>(IntPtr.Add(ptrDst, i * elemSize));
                return result;
            }
            finally
            {
                Marshal.FreeHGlobal(ptrSrc);
                Marshal.FreeHGlobal(ptrDst);
            }
        }

        public static int ApproxLine(RvPointF32[] srcSet, RvPointF32[] destSet)
        {
            if (srcSet == null || destSet == null || srcSet.Length == 0 || destSet.Length == 0) return 0;
            int srcCount = srcSet.Length;
            int dstCount = destSet.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptrSrc = Marshal.AllocHGlobal(srcCount * elemSize);
            IntPtr ptrDst = Marshal.AllocHGlobal(dstCount * elemSize);
            try
            {
                for (int i = 0; i < srcCount; i++)
                    Marshal.StructureToPtr(srcSet[i], IntPtr.Add(ptrSrc, i * elemSize), false);
                int result = rvApproxLineE1(ptrSrc, srcCount, ptrDst, dstCount);
                for (int i = 0; i < Math.Min(result, dstCount); i++)
                    destSet[i] = Marshal.PtrToStructure<RvPointF32>(IntPtr.Add(ptrDst, i * elemSize));
                return result;
            }
            finally
            {
                Marshal.FreeHGlobal(ptrSrc);
                Marshal.FreeHGlobal(ptrDst);
            }
        }

        public static int ApproxLine(RvPointF64[] srcSet, RvPointF64[] destSet)
        {
            if (srcSet == null || destSet == null || srcSet.Length == 0 || destSet.Length == 0) return 0;
            int srcCount = srcSet.Length;
            int dstCount = destSet.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF64));
            IntPtr ptrSrc = Marshal.AllocHGlobal(srcCount * elemSize);
            IntPtr ptrDst = Marshal.AllocHGlobal(dstCount * elemSize);
            try
            {
                for (int i = 0; i < srcCount; i++)
                    Marshal.StructureToPtr(srcSet[i], IntPtr.Add(ptrSrc, i * elemSize), false);
                int result = rvApproxLineE2(ptrSrc, srcCount, ptrDst, dstCount);
                for (int i = 0; i < Math.Min(result, dstCount); i++)
                    destSet[i] = Marshal.PtrToStructure<RvPointF64>(IntPtr.Add(ptrDst, i * elemSize));
                return result;
            }
            finally
            {
                Marshal.FreeHGlobal(ptrSrc);
                Marshal.FreeHGlobal(ptrDst);
            }
        }

        public static RvRect CalcBoundRect(RvPoint[] vertices)
        {
            if (vertices == null || vertices.Length == 0) return new RvRect();
            int count = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
            try
            {
                for (int i = 0; i < count; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvCalcBoundRect(ptr, count);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static RvRectF32 CalcBoundRect(RvPointF32[] vertices)
        {
            if (vertices == null || vertices.Length == 0) return new RvRectF32();
            int count = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
            try
            {
                for (int i = 0; i < count; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvCalcBoundRectE1(ptr, count);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static RvRectF64 CalcBoundRect(RvPointF64[] vertices)
        {
            if (vertices == null || vertices.Length == 0) return new RvRectF64();
            int count = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF64));
            IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
            try
            {
                for (int i = 0; i < count; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvCalcBoundRectE2(ptr, count);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        // ---- IsPointInPolygon ----
        // ---- Point in polygon ----

        public static bool IsPointInPolygon(RvPoint point, RvPoint[] polygon)
        {
            if (polygon == null || polygon.Length == 0) return false;
            int count = polygon.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
            try
            {
                for (int i = 0; i < count; i++)
                    Marshal.StructureToPtr(polygon[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvIsPointInPolygon(point, ptr, count);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static bool IsPointInPolygon(RvPointF32 point, RvPointF32[] polygon)
        {
            if (polygon == null || polygon.Length == 0) return false;
            int count = polygon.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
            try
            {
                for (int i = 0; i < count; i++)
                    Marshal.StructureToPtr(polygon[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvIsPointInPolygonE1(point, ptr, count);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static bool IsPointInPolygon(RvPointF64 point, RvPointF64[] polygon)
        {
            if (polygon == null || polygon.Length == 0) return false;
            int count = polygon.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF64));
            IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
            try
            {
                for (int i = 0; i < count; i++)
                    Marshal.StructureToPtr(polygon[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvIsPointInPolygonE2(point, ptr, count);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        // ---- Point in polygon ring (with hole) ----

        public static bool IsPointInPolygonRing(RvPoint point, RvPoint[] innerArray, RvPoint[] outerArray)
        {
            IntPtr innerPtr = IntPtr.Zero, outerPtr = IntPtr.Zero;
            int innerCount = innerArray?.Length ?? 0;
            int outerCount = outerArray?.Length ?? 0;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            try
            {
                if (innerCount > 0)
                {
                    innerPtr = Marshal.AllocHGlobal(innerCount * elemSize);
                    for (int i = 0; i < innerCount; i++)
                        Marshal.StructureToPtr(innerArray[i], IntPtr.Add(innerPtr, i * elemSize), false);
                }
                if (outerCount > 0)
                {
                    outerPtr = Marshal.AllocHGlobal(outerCount * elemSize);
                    for (int i = 0; i < outerCount; i++)
                        Marshal.StructureToPtr(outerArray[i], IntPtr.Add(outerPtr, i * elemSize), false);
                }
                return rvIsPointInPolygonRing(point, innerPtr, innerCount, outerPtr, outerCount);
            }
            finally
            {
                if (innerPtr != IntPtr.Zero) Marshal.FreeHGlobal(innerPtr);
                if (outerPtr != IntPtr.Zero) Marshal.FreeHGlobal(outerPtr);
            }
        }

        public static bool IsPointInPolygonRing(RvPointF32 point, RvPointF32[] innerArray, RvPointF32[] outerArray)
        {
            IntPtr innerPtr = IntPtr.Zero, outerPtr = IntPtr.Zero;
            int innerCount = innerArray?.Length ?? 0;
            int outerCount = outerArray?.Length ?? 0;
            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            try
            {
                if (innerCount > 0)
                {
                    innerPtr = Marshal.AllocHGlobal(innerCount * elemSize);
                    for (int i = 0; i < innerCount; i++)
                        Marshal.StructureToPtr(innerArray[i], IntPtr.Add(innerPtr, i * elemSize), false);
                }
                if (outerCount > 0)
                {
                    outerPtr = Marshal.AllocHGlobal(outerCount * elemSize);
                    for (int i = 0; i < outerCount; i++)
                        Marshal.StructureToPtr(outerArray[i], IntPtr.Add(outerPtr, i * elemSize), false);
                }
                return rvIsPointInPolygonRingE1(point, innerPtr, innerCount, outerPtr, outerCount);
            }
            finally
            {
                if (innerPtr != IntPtr.Zero) Marshal.FreeHGlobal(innerPtr);
                if (outerPtr != IntPtr.Zero) Marshal.FreeHGlobal(outerPtr);
            }
        }

        public static bool IsPointInPolygonRing(RvPointF64 point, RvPointF64[] innerArray, RvPointF64[] outerArray)
        {
            IntPtr innerPtr = IntPtr.Zero, outerPtr = IntPtr.Zero;
            int innerCount = innerArray?.Length ?? 0;
            int outerCount = outerArray?.Length ?? 0;
            int elemSize = Marshal.SizeOf(typeof(RvPointF64));
            try
            {
                if (innerCount > 0)
                {
                    innerPtr = Marshal.AllocHGlobal(innerCount * elemSize);
                    for (int i = 0; i < innerCount; i++)
                        Marshal.StructureToPtr(innerArray[i], IntPtr.Add(innerPtr, i * elemSize), false);
                }
                if (outerCount > 0)
                {
                    outerPtr = Marshal.AllocHGlobal(outerCount * elemSize);
                    for (int i = 0; i < outerCount; i++)
                        Marshal.StructureToPtr(outerArray[i], IntPtr.Add(outerPtr, i * elemSize), false);
                }
                return rvIsPointInPolygonRingE2(point, innerPtr, innerCount, outerPtr, outerCount);
            }
            finally
            {
                if (innerPtr != IntPtr.Zero) Marshal.FreeHGlobal(innerPtr);
                if (outerPtr != IntPtr.Zero) Marshal.FreeHGlobal(outerPtr);
            }
        }

        // ---- Point on polygon border ----

        public static bool IsPointOnPolygonBorder(RvPoint point, RvPoint[] vertexArray)
        {
            if (vertexArray == null || vertexArray.Length == 0) return false;
            int count = vertexArray.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
            try
            {
                for (int i = 0; i < count; i++)
                    Marshal.StructureToPtr(vertexArray[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvIsPointOnPolygonBorder(point, ptr, count);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static bool IsPointOnPolygonBorder(RvPointF32 point, RvPointF32[] vertexArray)
        {
            if (vertexArray == null || vertexArray.Length == 0) return false;
            int count = vertexArray.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
            try
            {
                for (int i = 0; i < count; i++)
                    Marshal.StructureToPtr(vertexArray[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvIsPointOnPolygonBorderE1(point, ptr, count);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static bool IsPointOnPolygonBorder(RvPointF64 point, RvPointF64[] vertexArray)
        {
            if (vertexArray == null || vertexArray.Length == 0) return false;
            int count = vertexArray.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF64));
            IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
            try
            {
                for (int i = 0; i < count; i++)
                    Marshal.StructureToPtr(vertexArray[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvIsPointOnPolygonBorderE2(point, ptr, count);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        // ---- Point on polyline ----
        public static bool IsPointOnPolyline(RvPoint point, RvPoint[] vertexArray)
        {
            if (vertexArray == null || vertexArray.Length == 0) return false;
            int count = vertexArray.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
            try
            {
                for (int i = 0; i < count; i++)
                    Marshal.StructureToPtr(vertexArray[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvIsPointOnPolyline(point, ptr, count);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static bool IsPointOnPolyline(RvPointF32 point, RvPointF32[] vertexArray)
        {
            if (vertexArray == null || vertexArray.Length == 0) return false;
            int count = vertexArray.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
            try
            {
                for (int i = 0; i < count; i++)
                    Marshal.StructureToPtr(vertexArray[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvIsPointOnPolylineE1(point, ptr, count);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static bool IsPointOnPolyline(RvPointF64 point, RvPointF64[] vertexArray)
        {
            if (vertexArray == null || vertexArray.Length == 0) return false;
            int count = vertexArray.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF64));
            IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
            try
            {
                for (int i = 0; i < count; i++)
                    Marshal.StructureToPtr(vertexArray[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvIsPointOnPolylineE2(point, ptr, count);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }
        //public static bool IsPointInPolygon(RvPoint point, RvPoint[] polygon)
        //{
        //    if (polygon == null || polygon.Length == 0) return false;
        //    int count = polygon.Length;
        //    int elemSize = Marshal.SizeOf(typeof(RvPoint));
        //    IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
        //    try
        //    {
        //        for (int i = 0; i < count; i++)
        //            Marshal.StructureToPtr(polygon[i], IntPtr.Add(ptr, i * elemSize), false);
        //        return rvIsPointInPolygon(point, ptr, count);
        //    }
        //    finally { Marshal.FreeHGlobal(ptr); }
        //}

        //public static bool IsPointInPolygon(int x, int y, RvPoint[] polygon)
        //{
        //    if (polygon == null || polygon.Length == 0) return false;
        //    int count = polygon.Length;
        //    int elemSize = Marshal.SizeOf(typeof(RvPoint));
        //    IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
        //    try
        //    {
        //        for (int i = 0; i < count; i++)
        //            Marshal.StructureToPtr(polygon[i], IntPtr.Add(ptr, i * elemSize), false);
        //        return rvIsPointInPolygonE1(x, y, ptr, count);
        //    }
        //    finally { Marshal.FreeHGlobal(ptr); }
        //}

        //public static bool IsPointInPolygon(RvPointF32 point, RvPointF32[] polygon)
        //{
        //    if (polygon == null || polygon.Length == 0) return false;
        //    int count = polygon.Length;
        //    int elemSize = Marshal.SizeOf(typeof(RvPointF32));
        //    IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
        //    try
        //    {
        //        for (int i = 0; i < count; i++)
        //            Marshal.StructureToPtr(polygon[i], IntPtr.Add(ptr, i * elemSize), false);
        //        return rvIsPointInPolygonE2(point, ptr, count);
        //    }
        //    finally { Marshal.FreeHGlobal(ptr); }
        //}

        //public static bool IsPointInPolygon(float x, float y, RvPointF32[] polygon)
        //{
        //    if (polygon == null || polygon.Length == 0) return false;
        //    int count = polygon.Length;
        //    int elemSize = Marshal.SizeOf(typeof(RvPointF32));
        //    IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
        //    try
        //    {
        //        for (int i = 0; i < count; i++)
        //            Marshal.StructureToPtr(polygon[i], IntPtr.Add(ptr, i * elemSize), false);
        //        return rvIsPointInPolygonE3(x, y, ptr, count);
        //    }
        //    finally { Marshal.FreeHGlobal(ptr); }
        //}

        //public static bool IsPointInPolygon(int x, int y, RvPoint[] innerArray, RvPoint[] outerArray)
        //{
        //    IntPtr innerPtr = IntPtr.Zero, outerPtr = IntPtr.Zero;
        //    int innerCount = innerArray?.Length ?? 0;
        //    int outerCount = outerArray?.Length ?? 0;
        //    try
        //    {
        //        if (innerCount > 0)
        //        {
        //            int elemSize = Marshal.SizeOf(typeof(RvPoint));
        //            innerPtr = Marshal.AllocHGlobal(innerCount * elemSize);
        //            for (int i = 0; i < innerCount; i++)
        //                Marshal.StructureToPtr(innerArray[i], IntPtr.Add(innerPtr, i * elemSize), false);
        //        }
        //        if (outerCount > 0)
        //        {
        //            int elemSize = Marshal.SizeOf(typeof(RvPoint));
        //            outerPtr = Marshal.AllocHGlobal(outerCount * elemSize);
        //            for (int i = 0; i < outerCount; i++)
        //                Marshal.StructureToPtr(outerArray[i], IntPtr.Add(outerPtr, i * elemSize), false);
        //        }
        //        return rvIsPointInPolygonE4(x, y, innerPtr, innerCount, outerPtr, outerCount);
        //    }
        //    finally
        //    {
        //        if (innerPtr != IntPtr.Zero) Marshal.FreeHGlobal(innerPtr);
        //        if (outerPtr != IntPtr.Zero) Marshal.FreeHGlobal(outerPtr);
        //    }
        //}

        //public static bool IsPointInPolygon(int x, int y, RvPoint[] vertexArray, out bool onBorder)
        //{
        //    onBorder = false;
        //    if (vertexArray == null || vertexArray.Length == 0) return false;
        //    int count = vertexArray.Length;
        //    int elemSize = Marshal.SizeOf(typeof(RvPoint));
        //    IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
        //    try
        //    {
        //        for (int i = 0; i < count; i++)
        //            Marshal.StructureToPtr(vertexArray[i], IntPtr.Add(ptr, i * elemSize), false);
        //        return rvIsPointInPolygonE5(x, y, ptr, count, out onBorder);
        //    }
        //    finally { Marshal.FreeHGlobal(ptr); }
        //}

        //public static bool IsPointInPolygon(double x, double y, RvPointF64[] polygon)
        //{
        //    if (polygon == null || polygon.Length == 0) return false;
        //    int count = polygon.Length;
        //    int elemSize = Marshal.SizeOf(typeof(RvPointF64));
        //    IntPtr ptr = Marshal.AllocHGlobal(count * elemSize);
        //    try
        //    {
        //        for (int i = 0; i < count; i++)
        //            Marshal.StructureToPtr(polygon[i], IntPtr.Add(ptr, i * elemSize), false);
        //        return rvIsPointInPolygonE6(x, y, ptr, count);
        //    }
        //    finally { Marshal.FreeHGlobal(ptr); }
        //}

        // ---- Polygon offset ----

        /// <summary>
        /// Offsets a polygon with int coordinates. Returns the offset polygon vertices.
        /// </summary>
        public static RvPoint[] PolygonOffset(RvPoint[] vertexArray, int distance)
        {
            if (vertexArray == null || vertexArray.Length == 0)
                return new RvPoint[0];

            int nArraySize = vertexArray.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptrIn = Marshal.AllocHGlobal(nArraySize * elemSize);
            IntPtr ptrOut = IntPtr.Zero;
            try
            {
                // 1. Copy input vertices to unmanaged memory
                for (int i = 0; i < nArraySize; i++)
                    Marshal.StructureToPtr(vertexArray[i], IntPtr.Add(ptrIn, i * elemSize), false);

                // 2. Initial output buffer: same size as input
                int nDestSize = nArraySize;
                ptrOut = Marshal.AllocHGlobal(nDestSize * elemSize);

                // 3. First attempt
                int ret = rvPolygonOffset(ptrIn, nArraySize, distance, ptrOut, nDestSize);

                if (ret > 0)
                {
                    // Success: copy the first `ret` vertices
                    int count = Math.Min(ret, nDestSize);
                    RvPoint[] result = new RvPoint[count];
                    for (int i = 0; i < count; i++)
                        result[i] = Marshal.PtrToStructure<RvPoint>(IntPtr.Add(ptrOut, i * elemSize));
                    return result;
                }
                else if (ret < 0)
                {
                    // Buffer too small; required size is -ret
                    int requiredSize = -ret;
                    Marshal.FreeHGlobal(ptrOut);
                    ptrOut = Marshal.AllocHGlobal(requiredSize * elemSize);

                    int ret2 = rvPolygonOffset(ptrIn, nArraySize, distance, ptrOut, requiredSize);
                    if (ret2 > 0)
                    {
                        int count = Math.Min(ret2, requiredSize);
                        RvPoint[] result = new RvPoint[count];
                        for (int i = 0; i < count; i++)
                            result[i] = Marshal.PtrToStructure<RvPoint>(IntPtr.Add(ptrOut, i * elemSize));
                        return result;
                    }
                    else if (ret2 == 0)
                    {
                        return new RvPoint[0];
                    }
                    else
                    {
                        // Should not happen, but return null on repeated failure
                        return null;
                    }
                }
                else // ret == 0
                {
                    return new RvPoint[0];
                }
            }
            finally
            {
                Marshal.FreeHGlobal(ptrIn);
                if (ptrOut != IntPtr.Zero) Marshal.FreeHGlobal(ptrOut);
            }
        }

        /// <summary>
        /// Offsets a polygon with float coordinates. Returns the offset polygon vertices.
        /// </summary>
        public static RvPointF32[] PolygonOffset(RvPointF32[] vertexArray, float distance)
        {
            if (vertexArray == null || vertexArray.Length == 0)
                return new RvPointF32[0];

            int nArraySize = vertexArray.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptrIn = Marshal.AllocHGlobal(nArraySize * elemSize);
            IntPtr ptrOut = IntPtr.Zero;
            try
            {
                for (int i = 0; i < nArraySize; i++)
                    Marshal.StructureToPtr(vertexArray[i], IntPtr.Add(ptrIn, i * elemSize), false);

                int nDestSize = nArraySize;
                ptrOut = Marshal.AllocHGlobal(nDestSize * elemSize);

                int ret = rvPolygonOffsetE1(ptrIn, nArraySize, distance, ptrOut, nDestSize);

                if (ret > 0)
                {
                    int count = Math.Min(ret, nDestSize);
                    RvPointF32[] result = new RvPointF32[count];
                    for (int i = 0; i < count; i++)
                        result[i] = Marshal.PtrToStructure<RvPointF32>(IntPtr.Add(ptrOut, i * elemSize));
                    return result;
                }
                else if (ret < 0)
                {
                    int requiredSize = -ret;
                    Marshal.FreeHGlobal(ptrOut);
                    ptrOut = Marshal.AllocHGlobal(requiredSize * elemSize);

                    int ret2 = rvPolygonOffsetE1(ptrIn, nArraySize, distance, ptrOut, requiredSize);
                    if (ret2 > 0)
                    {
                        int count = Math.Min(ret2, requiredSize);
                        RvPointF32[] result = new RvPointF32[count];
                        for (int i = 0; i < count; i++)
                            result[i] = Marshal.PtrToStructure<RvPointF32>(IntPtr.Add(ptrOut, i * elemSize));
                        return result;
                    }
                    else if (ret2 == 0)
                    {
                        return new RvPointF32[0];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return new RvPointF32[0];
                }
            }
            finally
            {
                Marshal.FreeHGlobal(ptrIn);
                if (ptrOut != IntPtr.Zero) Marshal.FreeHGlobal(ptrOut);
            }
        }

        /// <summary>
        /// Offsets a polygon with double coordinates. Returns the offset polygon vertices.
        /// </summary>
        public static RvPointF64[] PolygonOffset(RvPointF64[] vertexArray, double distance)
        {
            if (vertexArray == null || vertexArray.Length == 0)
                return new RvPointF64[0];

            int nArraySize = vertexArray.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF64));
            IntPtr ptrIn = Marshal.AllocHGlobal(nArraySize * elemSize);
            IntPtr ptrOut = IntPtr.Zero;
            try
            {
                for (int i = 0; i < nArraySize; i++)
                    Marshal.StructureToPtr(vertexArray[i], IntPtr.Add(ptrIn, i * elemSize), false);

                int nDestSize = nArraySize;
                ptrOut = Marshal.AllocHGlobal(nDestSize * elemSize);

                int ret = rvPolygonOffsetE2(ptrIn, nArraySize, distance, ptrOut, nDestSize);

                if (ret > 0)
                {
                    int count = Math.Min(ret, nDestSize);
                    RvPointF64[] result = new RvPointF64[count];
                    for (int i = 0; i < count; i++)
                        result[i] = Marshal.PtrToStructure<RvPointF64>(IntPtr.Add(ptrOut, i * elemSize));
                    return result;
                }
                else if (ret < 0)
                {
                    int requiredSize = -ret;
                    Marshal.FreeHGlobal(ptrOut);
                    ptrOut = Marshal.AllocHGlobal(requiredSize * elemSize);

                    int ret2 = rvPolygonOffsetE2(ptrIn, nArraySize, distance, ptrOut, requiredSize);
                    if (ret2 > 0)
                    {
                        int count = Math.Min(ret2, requiredSize);
                        RvPointF64[] result = new RvPointF64[count];
                        for (int i = 0; i < count; i++)
                            result[i] = Marshal.PtrToStructure<RvPointF64>(IntPtr.Add(ptrOut, i * elemSize));
                        return result;
                    }
                    else if (ret2 == 0)
                    {
                        return new RvPointF64[0];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return new RvPointF64[0];
                }
            }
            finally
            {
                Marshal.FreeHGlobal(ptrIn);
                if (ptrOut != IntPtr.Zero) Marshal.FreeHGlobal(ptrOut);
            }
        }
        // ---- Polygon area ----

        public static double CalcPolygonArea(RvPoint[] vertices)
        {
            if (vertices == null || vertices.Length == 0) return 0;
            int nArraySize = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(nArraySize * elemSize);
            try
            {
                for (int i = 0; i < nArraySize; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvCalcPolygonArea(ptr, nArraySize);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static double CalcPolygonArea(RvPointF32[] vertices)
        {
            if (vertices == null || vertices.Length == 0) return 0;
            int nArraySize = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(nArraySize * elemSize);
            try
            {
                for (int i = 0; i < nArraySize; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvCalcPolygonAreaE1(ptr, nArraySize);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static double CalcPolygonArea(RvPointF64[] vertices)
        {
            if (vertices == null || vertices.Length == 0) return 0;
            int nArraySize = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF64));
            IntPtr ptr = Marshal.AllocHGlobal(nArraySize * elemSize);
            try
            {
                for (int i = 0; i < nArraySize; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvCalcPolygonAreaE2(ptr, nArraySize);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        // ---- Polyline length ----

        public static double CalcPolylineLength(RvPoint[] vertices, bool bClosed)
        {
            if (vertices == null || vertices.Length == 0) return 0;
            int nArraySize = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(nArraySize * elemSize);
            try
            {
                for (int i = 0; i < nArraySize; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvCalcPolylineLength(ptr, nArraySize, bClosed);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static double CalcPolylineLength(RvPointF32[] vertices, bool bClosed)
        {
            if (vertices == null || vertices.Length == 0) return 0;
            int nArraySize = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(nArraySize * elemSize);
            try
            {
                for (int i = 0; i < nArraySize; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvCalcPolylineLengthE1(ptr, nArraySize, bClosed);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static double CalcPolylineLength(RvPointF64[] vertices, bool bClosed)
        {
            if (vertices == null || vertices.Length == 0) return 0;
            int nArraySize = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF64));
            IntPtr ptr = Marshal.AllocHGlobal(nArraySize * elemSize);
            try
            {
                for (int i = 0; i < nArraySize; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvCalcPolylineLengthE2(ptr, nArraySize, bClosed);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        // ---- Polygon center ----

        public static RvPoint CalcPolygonCenter(RvPoint[] vertices)
        {
            if (vertices == null || vertices.Length == 0) return new RvPoint();
            int nArraySize = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(nArraySize * elemSize);
            try
            {
                for (int i = 0; i < nArraySize; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvCalcPolygonCenter(ptr, nArraySize);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static RvPointF32 CalcPolygonCenter(RvPointF32[] vertices)
        {
            if (vertices == null || vertices.Length == 0) return new RvPointF32();
            int nArraySize = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(nArraySize * elemSize);
            try
            {
                for (int i = 0; i < nArraySize; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvCalcPolygonCenterE1(ptr, nArraySize);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static RvPointF64 CalcPolygonCenter(RvPointF64[] vertices)
        {
            if (vertices == null || vertices.Length == 0) return new RvPointF64();
            int nArraySize = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF64));
            IntPtr ptr = Marshal.AllocHGlobal(nArraySize * elemSize);
            try
            {
                for (int i = 0; i < nArraySize; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvCalcPolygonCenterE2(ptr, nArraySize);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        // ---- Point to polyline distance ----

        public static int CalcPointToPolylineDistance(RvPoint point, RvPoint[] vertices, bool bClosed)
        {
            if (vertices == null || vertices.Length == 0) return -1;
            int nArraySize = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(nArraySize * elemSize);
            try
            {
                for (int i = 0; i < nArraySize; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvCalcPointToPolylineDistance(point, ptr, nArraySize, bClosed);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static int CalcPointToPolylineDistance(RvPointF32 point, RvPointF32[] vertices, bool bClosed)
        {
            if (vertices == null || vertices.Length == 0) return -1;
            int nArraySize = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(nArraySize * elemSize);
            try
            {
                for (int i = 0; i < nArraySize; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvCalcPointToPolylineDistanceE1(point, ptr, nArraySize, bClosed);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static int CalcPointToPolylineDistance(RvPointF64 point, RvPointF64[] vertices, bool bClosed)
        {
            if (vertices == null || vertices.Length == 0) return -1;
            int nArraySize = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF64));
            IntPtr ptr = Marshal.AllocHGlobal(nArraySize * elemSize);
            try
            {
                for (int i = 0; i < nArraySize; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvCalcPointToPolylineDistanceE2(point, ptr, nArraySize, bClosed);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        // ---- Polyline move ----

        public static void PolylineMove(RvPoint[] vertices, int dx, int dy)
        {
            if (vertices == null || vertices.Length == 0) return;
            int nArraySize = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(nArraySize * elemSize);
            try
            {
                for (int i = 0; i < nArraySize; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                rvPolylineMove(ptr, nArraySize, dx, dy);
                for (int i = 0; i < nArraySize; i++)
                    vertices[i] = Marshal.PtrToStructure<RvPoint>(IntPtr.Add(ptr, i * elemSize));
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static void PolylineMove(RvPointF32[] vertices, float dx, float dy)
        {
            if (vertices == null || vertices.Length == 0) return;
            int nArraySize = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(nArraySize * elemSize);
            try
            {
                for (int i = 0; i < nArraySize; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                rvPolylineMoveE1(ptr, nArraySize, dx, dy);
                for (int i = 0; i < nArraySize; i++)
                    vertices[i] = Marshal.PtrToStructure<RvPointF32>(IntPtr.Add(ptr, i * elemSize));
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static void PolylineMove(RvPointF64[] vertices, double dx, double dy)
        {
            if (vertices == null || vertices.Length == 0) return;
            int nArraySize = vertices.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF64));
            IntPtr ptr = Marshal.AllocHGlobal(nArraySize * elemSize);
            try
            {
                for (int i = 0; i < nArraySize; i++)
                    Marshal.StructureToPtr(vertices[i], IntPtr.Add(ptr, i * elemSize), false);
                rvPolylineMoveE2(ptr, nArraySize, dx, dy);
                for (int i = 0; i < nArraySize; i++)
                    vertices[i] = Marshal.PtrToStructure<RvPointF64>(IntPtr.Add(ptr, i * elemSize));
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        // ---- Sub polygon ----

        public static int GetSubPolygonVertex(RvPoint[] arrIn, int startIndex, int endIndex, RvPoint[] arrOut)
        {
            if (arrIn == null || arrOut == null || arrIn.Length == 0 || arrOut.Length == 0) return 0;
            int inCount = arrIn.Length;
            int outCount = arrOut.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptrIn = Marshal.AllocHGlobal(inCount * elemSize);
            IntPtr ptrOut = Marshal.AllocHGlobal(outCount * elemSize);
            try
            {
                for (int i = 0; i < inCount; i++)
                    Marshal.StructureToPtr(arrIn[i], IntPtr.Add(ptrIn, i * elemSize), false);
                int result = rvGetSubPolygonVertex(ptrIn, inCount, startIndex, endIndex, ptrOut, outCount);
                for (int i = 0; i < Math.Min(result, outCount); i++)
                    arrOut[i] = Marshal.PtrToStructure<RvPoint>(IntPtr.Add(ptrOut, i * elemSize));
                return result;
            }
            finally
            {
                Marshal.FreeHGlobal(ptrIn);
                Marshal.FreeHGlobal(ptrOut);
            }
        }

        public static int GetSubPolygonVertex(RvPointF32[] arrIn, int startIndex, int endIndex, RvPointF32[] arrOut)
        {
            if (arrIn == null || arrOut == null || arrIn.Length == 0 || arrOut.Length == 0) return 0;
            int inCount = arrIn.Length;
            int outCount = arrOut.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptrIn = Marshal.AllocHGlobal(inCount * elemSize);
            IntPtr ptrOut = Marshal.AllocHGlobal(outCount * elemSize);
            try
            {
                for (int i = 0; i < inCount; i++)
                    Marshal.StructureToPtr(arrIn[i], IntPtr.Add(ptrIn, i * elemSize), false);
                int result = rvGetSubPolygonVertexE1(ptrIn, inCount, startIndex, endIndex, ptrOut, outCount);
                for (int i = 0; i < Math.Min(result, outCount); i++)
                    arrOut[i] = Marshal.PtrToStructure<RvPointF32>(IntPtr.Add(ptrOut, i * elemSize));
                return result;
            }
            finally
            {
                Marshal.FreeHGlobal(ptrIn);
                Marshal.FreeHGlobal(ptrOut);
            }
        }

        public static int GetSubPolygonVertex(RvPointF64[] arrIn, int startIndex, int endIndex, RvPointF64[] arrOut)
        {
            if (arrIn == null || arrOut == null || arrIn.Length == 0 || arrOut.Length == 0) return 0;
            int inCount = arrIn.Length;
            int outCount = arrOut.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF64));
            IntPtr ptrIn = Marshal.AllocHGlobal(inCount * elemSize);
            IntPtr ptrOut = Marshal.AllocHGlobal(outCount * elemSize);
            try
            {
                for (int i = 0; i < inCount; i++)
                    Marshal.StructureToPtr(arrIn[i], IntPtr.Add(ptrIn, i * elemSize), false);
                int result = rvGetSubPolygonVertexE2(ptrIn, inCount, startIndex, endIndex, ptrOut, outCount);
                for (int i = 0; i < Math.Min(result, outCount); i++)
                    arrOut[i] = Marshal.PtrToStructure<RvPointF64>(IntPtr.Add(ptrOut, i * elemSize));
                return result;
            }
            finally
            {
                Marshal.FreeHGlobal(ptrIn);
                Marshal.FreeHGlobal(ptrOut);
            }
        }

        public static double GetSubPolygonPerimeter(RvPoint[] arrIn, int startIndex, int endIndex)
        {
            if (arrIn == null || arrIn.Length == 0) return 0;
            int inCount = arrIn.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(inCount * elemSize);
            try
            {
                for (int i = 0; i < inCount; i++)
                    Marshal.StructureToPtr(arrIn[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvGetSubPolygonPerimeter(ptr, inCount, startIndex, endIndex);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static double GetSubPolygonPerimeter(RvPointF32[] arrIn, int startIndex, int endIndex)
        {
            if (arrIn == null || arrIn.Length == 0) return 0;
            int inCount = arrIn.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(inCount * elemSize);
            try
            {
                for (int i = 0; i < inCount; i++)
                    Marshal.StructureToPtr(arrIn[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvGetSubPolygonPerimeterE1(ptr, inCount, startIndex, endIndex);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static double GetSubPolygonPerimeter(RvPointF64[] arrIn, int startIndex, int endIndex)
        {
            if (arrIn == null || arrIn.Length == 0) return 0;
            int inCount = arrIn.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF64));
            IntPtr ptr = Marshal.AllocHGlobal(inCount * elemSize);
            try
            {
                for (int i = 0; i < inCount; i++)
                    Marshal.StructureToPtr(arrIn[i], IntPtr.Add(ptr, i * elemSize), false);
                return rvGetSubPolygonPerimeterE2(ptr, inCount, startIndex, endIndex);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static bool IsSubPolygon(RvPoint[] bigArr, RvPoint[] smallArr, bool bIncludeBorder)
        {
            if (bigArr == null || smallArr == null || bigArr.Length == 0 || smallArr.Length == 0) return false;
            int bigCount = bigArr.Length;
            int smallCount = smallArr.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptrBig = Marshal.AllocHGlobal(bigCount * elemSize);
            IntPtr ptrSmall = Marshal.AllocHGlobal(smallCount * elemSize);
            try
            {
                for (int i = 0; i < bigCount; i++)
                    Marshal.StructureToPtr(bigArr[i], IntPtr.Add(ptrBig, i * elemSize), false);
                for (int i = 0; i < smallCount; i++)
                    Marshal.StructureToPtr(smallArr[i], IntPtr.Add(ptrSmall, i * elemSize), false);
                return rvIsSubPolygon(ptrBig, bigCount, ptrSmall, smallCount, bIncludeBorder);
            }
            finally
            {
                Marshal.FreeHGlobal(ptrBig);
                Marshal.FreeHGlobal(ptrSmall);
            }
        }

        public static bool IsSubPolygon(RvPointF32[] bigArr, RvPointF32[] smallArr, bool bIncludeBorder)
        {
            if (bigArr == null || smallArr == null || bigArr.Length == 0 || smallArr.Length == 0) return false;
            int bigCount = bigArr.Length;
            int smallCount = smallArr.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptrBig = Marshal.AllocHGlobal(bigCount * elemSize);
            IntPtr ptrSmall = Marshal.AllocHGlobal(smallCount * elemSize);
            try
            {
                for (int i = 0; i < bigCount; i++)
                    Marshal.StructureToPtr(bigArr[i], IntPtr.Add(ptrBig, i * elemSize), false);
                for (int i = 0; i < smallCount; i++)
                    Marshal.StructureToPtr(smallArr[i], IntPtr.Add(ptrSmall, i * elemSize), false);
                return rvIsSubPolygonE1(ptrBig, bigCount, ptrSmall, smallCount, bIncludeBorder);
            }
            finally
            {
                Marshal.FreeHGlobal(ptrBig);
                Marshal.FreeHGlobal(ptrSmall);
            }
        }

        public static bool IsSubPolygon(RvPointF64[] bigArr, RvPointF64[] smallArr, bool bIncludeBorder)
        {
            if (bigArr == null || smallArr == null || bigArr.Length == 0 || smallArr.Length == 0) return false;
            int bigCount = bigArr.Length;
            int smallCount = smallArr.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF64));
            IntPtr ptrBig = Marshal.AllocHGlobal(bigCount * elemSize);
            IntPtr ptrSmall = Marshal.AllocHGlobal(smallCount * elemSize);
            try
            {
                for (int i = 0; i < bigCount; i++)
                    Marshal.StructureToPtr(bigArr[i], IntPtr.Add(ptrBig, i * elemSize), false);
                for (int i = 0; i < smallCount; i++)
                    Marshal.StructureToPtr(smallArr[i], IntPtr.Add(ptrSmall, i * elemSize), false);
                return rvIsSubPolygonE2(ptrBig, bigCount, ptrSmall, smallCount, bIncludeBorder);
            }
            finally
            {
                Marshal.FreeHGlobal(ptrBig);
                Marshal.FreeHGlobal(ptrSmall);
            }
        }

        // ====================================================================
        // Public wrappers — Section 4: Pixel / Image (direct calls)
        // ====================================================================


        public static int CalcOtsu(int maxValue, int[] magArr)
        {
            if (magArr == null || magArr.Length == 0) return 0;
            int arrSize = magArr.Length;
            int elemSize = sizeof(int);
            IntPtr ptr = Marshal.AllocHGlobal(arrSize * elemSize);
            try
            {
                Marshal.Copy(magArr, 0, ptr, arrSize);
                return rvCalcOtsu(maxValue, ptr, arrSize);
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        // ====================================================================
        // Public wrappers — Section 5: Rect / Box / Misc
        // ====================================================================

        public static RvRect AdaptRect(RvRect container, int width, int height, ref double ratio)
            => rvAdaptRect(container, width, height, ref ratio);

        public static RvRect AdaptRect(RvRect container, int width, int height, bool bNoAlignCenter, ref double ratio)
            => rvAdaptRectE1(container, width, height, bNoAlignCenter, ref ratio);

        public static RvRect AdaptRect(RvRect container, int width, int height, ref int fitness, ref double ratio)
            => rvAdaptRectE2(container, width, height, ref fitness, ref ratio);

        public static int Power(int @base, int index) => rvPower(@base, index);
        public static float Power(float @base, int index) => rvPowerE1(@base, index);
        public static double Power(double @base, int index) => rvPowerE2(@base, index);

        public static RvRect Box2dToRect(RvBox2D box) => rvBox2dToRect(box);
        public static RvRectF32 Box2dToRectF32(RvBox2D box) => rvBox2dToRectE1(box);
        public static RvRectF64 Box2dToRectF64(RvBox2D box) => rvBox2dToRectE2(box);

        public static bool IsBox2dInRect(RvBox2D box, RvRect rect) => rvIsBox2dInRect(box, rect);
        public static bool IsBox2dInRect(RvBox2D box, RvRectF32 rect) => rvIsBox2dInRectE1(box, rect);
        public static bool IsBox2dInRect(RvBox2D box, RvRectF64 rect) => rvIsBox2dInRectE2(box, rect);

        public static RvPoint GetRectCenter(RvRect rect) => rvGetRectCenter(rect);
        public static RvPointF32 GetRectCenter(RvRectF32 rect) => rvGetRectCenterE1(rect);
        public static RvPointF64 GetRectCenter(RvRectF64 rect) => rvGetRectCenterE2(rect);

        public static RvSize GetRectSize(RvRect rect, bool bIncludeBorder = false) => rvGetRectSize(rect, bIncludeBorder);
        public static RvSizeF32 GetRectSize(RvRectF32 rect, bool bIncludeBorder = false) => rvGetRectSizeE1(rect, bIncludeBorder);
        public static RvSizeF64 GetRectSize(RvRectF64 rect, bool bIncludeBorder = false) => rvGetRectSizeE2(rect, bIncludeBorder);

        public static int GetRectWidth(RvRect rect, bool bIncludeBorder = false) => rvGetRectWidth(rect, bIncludeBorder);
        public static int GetRectWidth(RvRectF32 rect, bool bIncludeBorder = false) => rvGetRectWidthE1(rect, bIncludeBorder);
        public static int GetRectWidth(RvRectF64 rect, bool bIncludeBorder = false) => rvGetRectWidthE2(rect, bIncludeBorder);

        public static int GetRectHeight(RvRect rect, bool bIncludeBorder = false) => rvGetRectHeight(rect, bIncludeBorder);
        public static int GetRectHeight(RvRectF32 rect, bool bIncludeBorder = false) => rvGetRectHeightE1(rect, bIncludeBorder);
        public static int GetRectHeight(RvRectF64 rect, bool bIncludeBorder = false) => rvGetRectHeightE2(rect, bIncludeBorder);

        public static RvRect RotateRect(RvRect rect, double angle) => rvRotateRect(rect, angle);
        public static RvRectF32 RotateRect(RvRectF32 rect, double angle) => rvRotateRectE1(rect, angle);
        public static RvRectF64 RotateRect(RvRectF64 rect, double angle) => rvRotateRectE2(rect, angle);

        public static RvRect MoveRect(RvRect rect, int dx, int dy) => rvMoveRect(rect, dx, dy);
        public static RvRectF32 MoveRect(RvRectF32 rect, int dx, int dy) => rvMoveRectE1(rect, dx, dy);
        public static RvRectF64 MoveRect(RvRectF64 rect, int dx, int dy) => rvMoveRectE2(rect, dx, dy);

        public static RvRect ScaleRect(RvRect rect, double scale) => rvScaleRect(rect, scale);
        public static RvRect ScaleRect(RvRect rect, double scaleX, double scaleY) => rvScaleRectE1(rect, scaleX, scaleY);
        public static RvRect ScaleRectE2(RvRect rect, double scale) => rvScaleRectE2(rect, scale);

        public static RvRect RectIntersect(RvRect rect, RvRect other) => rvRectIntersect(rect, other);
        public static RvRectF32 RectIntersect(RvRectF32 rect, RvRectF32 other) => rvRectIntersectE1(rect, other);
        public static RvRectF64 RectIntersect(RvRectF64 rect, RvRectF64 other) => rvRectIntersectE2(rect, other);

        public static RvRect RectUnion(RvRect rect, RvRect other) => rvRectUnion(rect, other);
        public static RvRectF32 RectUnion(RvRectF32 rect, RvRectF32 other) => rvRectUnionE1(rect, other);
        public static RvRectF64 RectUnion(RvRectF64 rect, RvRectF64 other) => rvRectUnionE2(rect, other);

        public static bool IsRectEmpty(RvRect rect) => rvIsRectEmpty(rect);
        public static bool IsRectEmpty(RvRectF32 rect) => rvIsRectEmptyE1(rect);
        public static bool IsRectEmpty(RvRectF64 rect) => rvIsRectEmptyE2(rect);

        public static int GetMipUpScale(int level) => rvGetMipUpScale(level);
        public static float GetMipUpScaleF(int level) => rvGetMipUpScaleE1(level);
        public static double GetMipUpScaleD(int level) => rvGetMipUpScaleE2(level);

        public static float GetMipDownScale(int level) => rvGetMipDownScale(level);
        public static float GetMipDownScaleF(int level) => rvGetMipDownScaleE1(level);
        public static double GetMipDownScaleD(int level) => rvGetMipDownScaleE2(level);

        public static float GetMipScale(int level, bool bOpposite = false) => rvGetMipScale(level, bOpposite);
        public static float GetMipScaleF(int level, bool bOpposite = false) => rvGetMipScaleE1(level, bOpposite);
        public static double GetMipScaleD(int level, bool bOpposite = false) => rvGetMipScaleE2(level, bOpposite);

        public static bool IsNullRect(RvRect rect) => rvIsNullRect(rect);
        public static bool IsNullRect(RvRectF32 rect) => rvIsNullRectE1(rect);
        public static bool IsNullRect(RvRectF64 rect) => rvIsNullRectE2(rect);

        public static bool IsRectEqual(RvRect rect0, RvRect rect1) => rvIsRectEqual(rect0, rect1);
        public static bool IsRectEqual(RvRectF32 rect0, RvRectF32 rect1) => rvIsRectEqualE1(rect0, rect1);
        public static bool IsRectEqual(RvRectF64 rect0, RvRectF64 rect1) => rvIsRectEqualE2(rect0, rect1);

        public static void NormalizeRect(ref RvRect pRect) => rvNormalizeRect(ref pRect);
        public static void NormalizeRect(ref RvRectF32 pRect) => rvNormalizeRectE1(ref pRect);
        public static void NormalizeRect(ref RvRectF64 pRect) => rvNormalizeRectE2(ref pRect);

        public static void RectShrink(ref RvRect pRect, int dx, int dy) => rvRectShrink(ref pRect, dx, dy);
        public static void RectShrink(ref RvRectF32 pRect, int dx, int dy) => rvRectShrinkE1(ref pRect, dx, dy);
        public static void RectShrink(ref RvRectF64 pRect, int dx, int dy) => rvRectShrinkE2(ref pRect, dx, dy);

        public static void RectDeflate(ref RvRect pRect, int dx, int dy) => rvRectDeflate(ref pRect, dx, dy);
        public static void RectDeflate(ref RvRectF32 pRect, int dx, int dy) => rvRectDeflateE1(ref pRect, dx, dy);
        public static void RectDeflate(ref RvRectF64 pRect, int dx, int dy) => rvRectDeflateE2(ref pRect, dx, dy);

        public static int CompareRect(RvRect rect0, RvRect rect1) => rvCompareRect(rect0, rect1);
        public static int CompareRect(RvRectF32 rect0, RvRectF32 rect1) => rvCompareRectE1(rect0, rect1);
        public static int CompareRect(RvRectF64 rect0, RvRectF64 rect1) => rvCompareRectE2(rect0, rect1);

        public static RvBool IsPointInsideRect(RvRect rect, RvPoint pos, bool bOnBorderOnly = false)
            => rvIsPointInsideRect(rect, pos, bOnBorderOnly);

        public static RvBool IsPointInsideRect(RvRectF32 rect, RvPointF32 pos, bool bOnBorderOnly = false)
            => rvIsPointInsideRectE1(rect, pos, bOnBorderOnly);

        public static RvBool IsPointInsideRect(RvRectF64 rect, RvPointF64 pos, bool bOnBorderOnly = false)
            => rvIsPointInsideRectE2(rect, pos, bOnBorderOnly);

        public static RvBool IsRectSurround(RvRect rect0, RvRect rect1) => rvIsRectSurround(rect0, rect1);
        public static RvBool IsRectSurround(RvRectF32 rect0, RvRectF32 rect1) => rvIsRectSurroundE1(rect0, rect1);
        public static RvBool IsRectSurround(RvRectF64 rect0, RvRectF64 rect1) => rvIsRectSurroundE2(rect0, rect1);

        // ---- Box2dToVertex / RectToVertex (array) ----

        public static int Box2dToVertex(RvBox2D box, RvPoint[] pointArray)
        {
            if (pointArray == null || pointArray.Length < 4) return 0;
            int size = pointArray.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(size * elemSize);
            try
            {
                int result = rvBox2dToVertex(box, ptr, size);
                for (int i = 0; i < Math.Min(result, size); i++)
                    pointArray[i] = Marshal.PtrToStructure<RvPoint>(IntPtr.Add(ptr, i * elemSize));
                return result;
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static int Box2dToVertex(RvBox2D box, RvPointF32[] pointArray)
        {
            if (pointArray == null || pointArray.Length < 4) return 0;
            int size = pointArray.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(size * elemSize);
            try
            {
                int result = rvBox2dToVertexE1(box, ptr, size);
                for (int i = 0; i < Math.Min(result, size); i++)
                    pointArray[i] = Marshal.PtrToStructure<RvPointF32>(IntPtr.Add(ptr, i * elemSize));
                return result;
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static int Box2dToVertex(RvBox2D box, RvPointF64[] pointArray)
        {
            if (pointArray == null || pointArray.Length < 4) return 0;
            int size = pointArray.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF64));
            IntPtr ptr = Marshal.AllocHGlobal(size * elemSize);
            try
            {
                int result = rvBox2dToVertexE2(box, ptr, size);
                for (int i = 0; i < Math.Min(result, size); i++)
                    pointArray[i] = Marshal.PtrToStructure<RvPointF64>(IntPtr.Add(ptr, i * elemSize));
                return result;
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static int RectToVertex(RvRect rect, RvPoint[] pointArray)
        {
            if (pointArray == null || pointArray.Length < 4) return 0;
            int size = pointArray.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(size * elemSize);
            try
            {
                int result = rvRectToVertex(rect, ptr, size);
                for (int i = 0; i < Math.Min(result, size); i++)
                    pointArray[i] = Marshal.PtrToStructure<RvPoint>(IntPtr.Add(ptr, i * elemSize));
                return result;
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static int RectToVertex(RvRect rect, RvPointF32[] pointArray)
        {
            if (pointArray == null || pointArray.Length < 4) return 0;
            int size = pointArray.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(size * elemSize);
            try
            {
                int result = rvRectToVertexE1(rect, ptr, size);
                for (int i = 0; i < Math.Min(result, size); i++)
                    pointArray[i] = Marshal.PtrToStructure<RvPointF32>(IntPtr.Add(ptr, i * elemSize));
                return result;
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        public static int RectToVertex(RvRect rect, RvPointF64[] pointArray)
        {
            if (pointArray == null || pointArray.Length < 4) return 0;
            int size = pointArray.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPointF64));
            IntPtr ptr = Marshal.AllocHGlobal(size * elemSize);
            try
            {
                int result = rvRectToVertexE2(rect, ptr, size);
                for (int i = 0; i < Math.Min(result, size); i++)
                    pointArray[i] = Marshal.PtrToStructure<RvPointF64>(IntPtr.Add(ptr, i * elemSize));
                return result;
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }
    }
}
