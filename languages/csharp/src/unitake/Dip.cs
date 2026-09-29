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

namespace Kingpool.Vision
{
    using RvColor = System.UInt32;

    //filter methods
    public enum PixelFilterType
    {
        Nearest = 1,
        Bilinear = 2,
        Box = 3,
        Gaussian = 4,
        Hamming = 5,
        Blackman = 6,
    }

    public enum AutoBinarize
    {
        MinError = 1,
        MaxEntropy = 2,
        Otsu = 3,
        Gaussian = 4,
    }

    public enum OptimalType
    {

    }
    //public const int AB_GAUSS = 0;
    //public const int AB_POINSON = 1;
    //public const int AB_ENTROPY = 2;
    //public const int AB_MAJORITY = 3;
    //public const int AB_OTSU = 4;

    public enum AdaptiveBinarize
    {
        Mean = 0,
        Gaussian = 1,
        LocalInteger = 2,
        IsoData = 3,
    }


    //minerror auto-binarize
    public enum MinErrorMode
    {
        Gaussian = 0,
        Poisson = 1,
    }

    public enum FillTextStyle
    {
        Italic = 1,
        Underline = (1 << 1),
        Bold = (1 << 2),
        StrikeOut = (1 << 3),
        Transparent = (1 << 4),
    }

    public enum PixelOperator
    {
        Add = 0, //加
        Sub = 1, //减
        Abs = 2, //差的绝对值
        Mul = 3, //乘
        Div = 4, //除 
        And = 5, //与 
        Or = 6,  //或
        Xor = 7, //异或 
        Large = 8, //大值 
        Small = 9, //小值 
    }

    public enum GradientDirection
    {
        Horizontal = 0,        // 从左到右
        Vertical,          // 从上到下
        Diagonal,          // 左上到右下
        DiagonalReverse, // 右下到左上
    }

    public enum MorphologyType
    {
        Erode = 0,
        Dilate = 1,
        Open = 2,
        Close = 3,
        Gradient = 4,
        TopHat = 5,
        BlackHat = 6,
    }

    public enum ColorFusionType
    {
        Salience = 0,
        Voting = 1,
        Direction = 2,
    }

    public enum DetectEdgeType
    {
        Sobel = 0,
        ZeroCross = 1,
    }

    /// <summary>
    /// Static utility class for image processing operations (thresholding, filtering, geometric transforms, etc.)
    /// All methods are static wrappers around unmanaged functions from pprs.dll.
    /// </summary>
    public static class Dip
    {
        // ====================================================================
        // Constants (unchanged)
        // ====================================================================

        public const Byte BIN_ONE = 255;
        public const Byte BIN_ZERO = 0;

        public const int FT_MAX_TEXT_LEN = 4096;



        public const int RTA_SIMPLE = 0;
        public const int RTA_OBJ_SIZE = 1;
        public const int RTA_BACKGND = 2;
        public const int RTA_FOREGND = 3;
        public const int RTA_RANGE = 4;
        public const int RTA_RANGE_EX = 5;
        public const int RTA_LOCAL = 6;
        public const int RTA_GLOBAL = 7;

        public const int BT_SIMPLE = 0x0;
        public const int BT_DARK = 0x1;
        public const int BT_LIGHT = 0x2;
        public const int BT_INNER = 0x3;
        public const int BT_OUTER = 0x4;
        //public const int BT_LOCAL_INTEGRAL = 0x10;
        //public const int BT_MULTI_LEVEL = 0x11;
        public const int BT_MAX_ENTROPY = 0x12;
        //public const int BT_ISO_DATA = 0x13;
        public const int BT_MIN_ERROR = 0x14;
        public const int BT_OTSU = 0x15;



        public const int COLOR_CH1 = 1;
        public const int COLOR_CH2 = (1 << 1);
        public const int COLOR_CH3 = (1 << 2);
        public const int COLOR_CH4 = (1 << 3);

        public const int FF_DEFAULT_RANGE = 0;
        public const int FF_FIXED_RANGE = (1 << 16);
        public const int FF_CONN4 = 4;
        public const int FF_CONN8 = 8;

        public const int SM_DIST_TRANS = 0;
        public const int SM_MULT_FILTER = 1;



        public const int BD_DARK = 0;
        public const int BD_LIGHT = 1;
        public const int BD_EQUAL = 2;
        public const int BD_NOTEQUAL = 3;

        // ====================================================================
        // DLL import constants
        // ====================================================================

#if DEBUGGING_KINGPOOL_SUITE
        private const string LIB_NAME = "pprs_d.dll";
#else
        private const string LIB_NAME = "pprs.dll";
#endif

        // ====================================================================
        // Unmanaged imports (all public static extern, no 'rv' prefix in C# name)
        // ====================================================================

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvbSimple")]
        public static extern void Simple(IntPtr hImage, Byte level, bool bReverse);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvbSimpleE1")]
        public static extern void SimpleE1(IntPtr hImage, Byte level, bool bReverse);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvbDark")]
        public static extern void Dark(IntPtr hImage, Byte level, Byte newVal = BIN_ONE, bool bBin = false);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvbLight")]
        public static extern void Light(IntPtr hImage, Byte level, Byte newVal = BIN_ONE, bool bBin = false);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvbInner")]
        public static extern void Inner(IntPtr hImage, Byte low, Byte high, Byte newVal = BIN_ONE, bool bBin = false);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvbOuter")]
        public static extern void Outer(IntPtr hImage, Byte low, Byte high, Byte newVal = BIN_ONE, bool bBin = false);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvbMaxEntropy")]
        public static extern void MaxEntropy(IntPtr hImage);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvbMinError")]
        public static extern void MinError(IntPtr hImage, MinErrorMode mode);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvbMinErrorE1")]
        public static extern void MinErrorE1(IntPtr hImage, IntPtr hMask, MinErrorMode mode);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvbOtsu")]
        public static extern void Otsu(IntPtr hImage, IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvbMajority")]
        public static extern void Majority(IntPtr hImage, IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvbHysteresis")]
        public static extern void Hysteresis(IntPtr hImage, int low, int high, int maxLength);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvbGetOptimum")]
        public static extern Byte GetOptimum(IntPtr hImage, int method);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvbGetOptimumE1")]
        public static extern Byte GetOptimumE1(IntPtr hImage, IntPtr hMask, int method);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPixelAdd")]
        public static extern IntPtr PixelAdd(IntPtr hImage0, IntPtr hImage1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPixelSubtract")]
        public static extern IntPtr PixelSubtract(IntPtr hImage0, IntPtr hImage1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPixelDiff")]
        public static extern IntPtr PixelDiff(IntPtr hImage0, IntPtr hImage1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPixelLarge")]
        public static extern IntPtr PixelLarge(IntPtr hImage0, IntPtr hImage1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPixelSmall")]
        public static extern IntPtr PixelSmall(IntPtr hImage0, IntPtr hImage1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPixelXor")]
        public static extern IntPtr PixelXor(IntPtr hImage0, IntPtr hImage1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPixelAnd")]
        public static extern IntPtr PixelAnd(IntPtr hImage0, IntPtr hImage1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPixelOr")]
        public static extern IntPtr PixelOr(IntPtr hImage0, IntPtr hImage1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPixelCopy")]
        public static extern IntPtr PixelCopy(IntPtr hImage0, IntPtr hImage1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPixelMerge")]
        public static extern IntPtr PixelMerge(IntPtr hImage0, IntPtr hImage1, PixelOperator type);

        //[DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPixelMergeE1")]
        //public static extern IntPtr PixelMergeE1(IntPtr hImage0, IntPtr hImage1, PixelOperator type, float alpha, float beta);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvSetPixel")]
        public static extern void SetPixel(IntPtr hImage, int x, int y, uint color);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvSetPixelE1")]
        private static extern void rvSetPixelE1(IntPtr hImage, IntPtr pPtArr, int arrSize, uint color);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvSetPixelE2")]
        private static extern void rvSetPixelE2(IntPtr hImage, int dx, int dy, IntPtr pPtArr, int arrSize, uint color);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvSetPixelEx")]
        public static extern void SetPixelEx(IntPtr hImage, uint color, IntPtr hMask, bool bOutside);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetPixel")]
        public static extern UInt32 GetPixel(IntPtr hImage, int x, int y);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetPixelE1")]
        public static extern UInt32 GetPixelE1(IntPtr hImage, int x, int y, int aperture);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCopyPixels")]
        public static extern void CopyPixels(IntPtr src, int sx, int sy, int sw, int sh, IntPtr dest,
                                                  int dx = 0, int dy = 0, int dw = -1, int dh = -1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCopyPixelsE2")]
        public static extern void CopyPixelsE2(IntPtr src, IntPtr dest, IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCopyPixelsE3")]
        public static extern void CopyPixelsE3(IntPtr src, IntPtr dest, RvPointF32[] vertex /*4*/);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvReplacePixels")]
        public static extern void ReplacePixels(IntPtr hImage, uint target, uint newColor, IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvReplaceChannel")]
        public static extern IntPtr ReplaceChannel(IntPtr hImage, IntPtr sub, int channel = COLOR_CH1);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFill")]
        public static extern IntPtr Fill(IntPtr hImage, int x, int y, uint nNewValue);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFillE1")]
        public static extern IntPtr FillE1(IntPtr hImage, int x, int y, uint targetColor, uint newColor);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvClip")]
        private static extern IntPtr rvClip(IntPtr hImage, int left, int top, int right, int bottom);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvbDiffer")]
        public static extern void Differ(IntPtr hImage, IntPtr background, int offset, int mode);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFloodMask")]
        public static extern IntPtr FloodMask(IntPtr hImage, RvPoint pos, RvScalarF64 lower, RvScalarF64 upper, int connectivity, int flags, IntPtr imMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFloodFill")]
        public static extern void FloodFill(IntPtr hImage, RvPoint pos, RvRgb newColor, RvScalarF64 lodiff, RvScalarF64 updiff, int connectivity, int flags = 0);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFillEx")]
        public static extern void FillEx(IntPtr hImage, int channel, Byte nNewValue);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFillRect")]
        public static extern void FillRect(IntPtr hImage, RvRect rect, uint nNewValue, bool bBorderOnly);

        // [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFillRectE1")]
        // public static extern void FillRectE1(IntPtr hImage, RvRect rect, uint nNewValue);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFillGradient")]
        public static extern void FillGradient(IntPtr hImage, RvRgb colorStart, RvRgb colorEnd, GradientDirection direction);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFillBorder")]
        public static extern void FillBorder(IntPtr hImage, uint nNewValue);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFillEllipse")]
        public static extern void FillEllipse(IntPtr hImage, int left, int top, int right, int bottom, uint nNewValue);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFillEllipseE1")]
        public static extern void FillEllipseE1(IntPtr hImage, RvBox2D box, uint nNewValue);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFillPolygon")]
        public static extern void FillPolygon(IntPtr hImage, IntPtr pVertexArray, int nArraySize, uint nNewValue);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFillPolygonE1")]
        public static extern void FillPolygonE1(IntPtr hImage, IntPtr pVertexArray, int nArraySize, IntPtr pHoleArray, int nHoleSize, uint nNewValue);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFillPolygonE2")]
        public static extern void FillPolygonE2(IntPtr hImage, IntPtr pVertexArray, int nArraySize, uint nNewValue, bool bFill);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvSetPolyline")]
        private static extern void SetPolyline(IntPtr hImage, IntPtr pVertexArray, int nArraySize, uint nNewValue);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvSetLine")]
        public static extern void SetLine(IntPtr hImage, RvPoint p0, RvPoint p1, uint nNewValue);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvSetEllipse")]
        public static extern void SetEllipse(IntPtr hImage, int cx, int cy, int rx, int ry, float angle, uint nNewValue);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvMakeSquarePixel")]
        public static extern bool MakeSquarePixel(IntPtr hImage, int pixelSizeX, int pixelSizeY, int filterType, bool bShrink);



        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvScale")]
        public static extern bool Scale(IntPtr hImage, float scale, PixelFilterType flag = PixelFilterType.Nearest);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvScaleE1")]
        public static extern void ScaleE1(IntPtr hImage, float scale, RvColor fillColor, PixelFilterType flag = PixelFilterType.Nearest, bool bReserveDim = false);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvScaleEx")]
        public static extern bool ScaleEx(IntPtr imgOri, IntPtr imgNew, PixelFilterType flag = PixelFilterType.Nearest);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvResize")]
        public static extern void Resize(IntPtr hImage, int width, int height, int fillColor);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvResizeE1")]
        public static extern void ResizeE1(IntPtr hImage, int width, int height, PixelFilterType flag = PixelFilterType.Bilinear);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRotate")]
        public static extern void Rotate(IntPtr hImage, int times);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRotateEx")]
        public static extern void RotateEx(IntPtr hImage, double angle, bool bNearest, bool bReserveDim);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRotateE2")]
        public static extern void RotateE2(IntPtr hImage, double angle, RvColor fillColor, bool bNearest, bool bReserveDim);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRotateE3")]
        public static extern void RotateE3(IntPtr hImage, double cx, double cy, double angle, RvColor fillColor);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFuseGradients")]
        public static extern void FuseGradients(IntPtr hImage, ColorFusionType mode, float threshold);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFlip")]
        public static extern void Flip(IntPtr hImage, RvDirection direction);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvInvert")]
        public static extern IntPtr Invert(IntPtr hImage);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvTranslate")]
        public static extern void Translate(IntPtr hImage, float dx, float dy, bool bNearest);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvTranslateEx")]
        public static extern void TranslateEx(IntPtr hImage, float dx, float dy, bool bNearest, RvColor fillColor);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvNormalize")]
        public static extern void Normalize(IntPtr hImage, float avg, float var);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvEqualize")]
        public static extern void Equalize(IntPtr hImage);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvContrast")]
        public static extern void Contrast(IntPtr hImage, float lPercent, float hPercent);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvLinear")]
        public static extern void Linear(IntPtr hImage, float gain, float offset);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvExpand")]
        public static extern void Expand(IntPtr hImage);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvLinearEx")]
        public static extern void LinearEx(IntPtr hImage, int channel, float gain, float offset);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGradient")]
        public static extern void Gradient(IntPtr hImage, RvDirection direction, int gap);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDiffFirst")]
        public static extern void DiffFirst(IntPtr hImage, RvDirection direction);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDiffSecond")]
        public static extern void DiffSecond(IntPtr hImage, RvDirection direction);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPrewitt")]
        public static extern void Prewitt(IntPtr hImage, RvDirection direction);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvSobel")]
        public static extern void Sobel(IntPtr hImage, RvDirection direction);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvScharr")]
        public static extern void Scharr(IntPtr hImage, RvDirection direction);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvKirsch")]
        public static extern void Kirsch(IntPtr hImage, RvDirection direction);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvLaPlacian")]
        public static extern void Laplacian(IntPtr hImage);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRoberts")]
        public static extern void Roberts(IntPtr hImage);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvSharp1541")]
        public static extern IntPtr Sharp1541(IntPtr hImage);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCanny")]
        public static extern void Canny(IntPtr hImage, double lowThresh, double highThresh, bool bForceToBin, int aperture = 3);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFilter")]
        public static extern void Filter(IntPtr hImage, IntPtr kernel);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvMedian")]
        public static extern void Median(IntPtr hImage, int kernelSize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDetectEdge")]
        public static extern IntPtr DetectEdge(IntPtr hImage, DetectEdgeType type, float thresVal);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvSkeleton")]
        public static extern void Skeleton(IntPtr hImage, int method = SM_DIST_TRANS);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvThinning")]
        public static extern void Thinning(IntPtr hImage);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDilate")]
        public static extern void Dilate(IntPtr hImage, IntPtr shape);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDilateE1")]
        public static extern void DilateE1(IntPtr hImage, int radius);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvErode")]
        public static extern void Erode(IntPtr hImage, IntPtr shape);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRemoveBorder")]
        public static extern void RemoveBorder(IntPtr hImage);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRemoveBorderEx")]
        public static extern void RemoveBorderEx(IntPtr hImage, IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFillHole")]
        public static extern void FillHole(IntPtr hImage);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFillHoleEx")]
        public static extern void FillHoleEx(IntPtr hImage, IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRetrieveEdge")]
        public static extern IntPtr RetrieveEdge(IntPtr hImage, bool bDark2Light, IntPtr dest);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRetrieveEdgeE1")]
        public static extern IntPtr RetrieveEdgeE1(IntPtr hImage, bool n8, IntPtr dest);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPyramid")]
        public static extern IntPtr Pyramid(IntPtr hImage, bool bUp, IntPtr dest);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvWarp")]
        public static extern IntPtr Warp(IntPtr hImage, IntPtr pSrcControls, IntPtr pDestControls, IntPtr dest);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvWarpEx")]
        public static extern IntPtr WarpEx(IntPtr hImage, IntPtr pSrcControls, int nSrcControls, IntPtr pDestControls, int nDestControls, bool bKeepOutlier, IntPtr dest);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvUnwarp")]
        private static extern IntPtr Unwarp(IntPtr hImage, IntPtr pSrcCtrls, IntPtr dest);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCutMargin")]
        public static extern void CutMargin(IntPtr hImage, int thick, RvColor color);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvBlur")]
        public static extern IntPtr Blur(IntPtr hImage, int kernWidth, int kernHeight);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvSmoothGaussian")]
        public static extern IntPtr SmoothGaussian(IntPtr hImage, int kernWidth, int kernHeight);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvSmoothMedian")]
        public static extern IntPtr SmoothMedian(IntPtr hImage, int kernelSize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvSmoothAvg")]
        public static extern IntPtr SmoothAvg(IntPtr hImage, int kernelSize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvMorphology")]
        public static extern IntPtr Morphology(IntPtr hImage, MorphologyType type, int kernelSize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvMorpClose")]
        public static extern IntPtr MorpClose(IntPtr hImage);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvMorpOpen")]
        public static extern IntPtr MorpOpen(IntPtr hImage);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvAdaptive")]
        public static extern void Adaptive(IntPtr hImage, AdaptiveBinarize method, int blockSize, double extra1, double extra2);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvEclose")]
        public static extern void Eclose(IntPtr hImage, int radius);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDistTrans")]
        public static extern uint DistTrans(IntPtr hImage, IntPtr pArrResult, uint size);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDistTransE1")]
        public static extern IntPtr DistTransE1(IntPtr hImage, int thres);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvIncreaseLum")]
        public static extern IntPtr IncreaseLum(IntPtr hImage);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFillText")]
        public static extern void FillText(IntPtr hImage, string strText, int x, int y, int fontSize, uint textColor);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFillTextEx")]
        public static extern void FillTextEx(IntPtr hImage, string strText, int x, int y, string strFaceName, int fontSize, uint textColor, uint backColor, int flags);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvErodeBin")]
        public static extern void ErodeBin(IntPtr hImage);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvOutlineE1")]
        public static extern void OutlineE1(IntPtr hImage, IntPtr hMask, int dist, bool b8);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvOutline")]
        public static extern void Outline(IntPtr hImage, IntPtr hMask, bool b8);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvAdjustBright")]
        public static extern void AdjustBright(IntPtr hImage, bool bDarker, float percent);

        // ====================================================================
        // Array wrapper overloads (same name, array parameters)
        // ====================================================================

        /// <summary>
        /// Sets pixels at specified positions with offset.
        /// </summary>
        public static void SetPixelE1(IntPtr hImage, RvPoint[] pPtArr, uint color)
        {
            if (pPtArr == null || pPtArr.Length == 0) return;
            int arrSize = pPtArr.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(arrSize * elemSize);
            try
            {
                for (int i = 0; i < arrSize; i++)
                    Marshal.StructureToPtr(pPtArr[i], IntPtr.Add(ptr, i * elemSize), false);
                rvSetPixelE1(hImage, ptr, arrSize, color);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public static void SetPixelE2(IntPtr hImage, int dx, int dy, RvPoint[] pPtArr, uint color)
        {
            if (pPtArr == null || pPtArr.Length == 0) return;
            int arrSize = pPtArr.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(arrSize * elemSize);
            try
            {
                for (int i = 0; i < arrSize; i++)
                    Marshal.StructureToPtr(pPtArr[i], IntPtr.Add(ptr, i * elemSize), false);
                rvSetPixelE2(hImage, dx, dy, ptr, arrSize, color);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        /// <summary>
        /// Fills a polygon defined by an array of vertices.
        /// </summary>
        public static void FillPolygon(IntPtr hImage, RvPoint[] vertexArray, uint nNewValue)
        {
            if (vertexArray == null || vertexArray.Length < 3) return;
            int nArraySize = vertexArray.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(nArraySize * elemSize);
            try
            {
                for (int i = 0; i < nArraySize; i++)
                    Marshal.StructureToPtr(vertexArray[i], IntPtr.Add(ptr, i * elemSize), false);
                FillPolygon(hImage, ptr, nArraySize, nNewValue);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        /// <summary>
        /// Fills a polygon with holes defined by vertex arrays.
        /// </summary>
        public static void FillPolygonE1(IntPtr hImage, RvPoint[] vertexArray, RvPoint[] holeArray, uint nNewValue)
        {
            if (vertexArray == null || vertexArray.Length < 3) return;
            int nArraySize = vertexArray.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptrVerts = Marshal.AllocHGlobal(nArraySize * elemSize);
            IntPtr ptrHoles = IntPtr.Zero;
            int nHoleSize = 0;
            try
            {
                for (int i = 0; i < nArraySize; i++)
                    Marshal.StructureToPtr(vertexArray[i], IntPtr.Add(ptrVerts, i * elemSize), false);

                if (holeArray != null && holeArray.Length >= 3)
                {
                    nHoleSize = holeArray.Length;
                    ptrHoles = Marshal.AllocHGlobal(nHoleSize * elemSize);
                    for (int i = 0; i < nHoleSize; i++)
                        Marshal.StructureToPtr(holeArray[i], IntPtr.Add(ptrHoles, i * elemSize), false);
                }

                FillPolygonE1(hImage, ptrVerts, nArraySize, ptrHoles, nHoleSize, nNewValue);
            }
            finally
            {
                Marshal.FreeHGlobal(ptrVerts);
                if (ptrHoles != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptrHoles);
            }
        }

        /// <summary>
        /// Fills a polygon with an option to fill or outline.
        /// </summary>
        public static void FillPolygonE2(IntPtr hImage, RvPoint[] vertexArray, uint nNewValue, bool bFill)
        {
            if (vertexArray == null || vertexArray.Length < 3) return;
            int nArraySize = vertexArray.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(nArraySize * elemSize);
            try
            {
                for (int i = 0; i < nArraySize; i++)
                    Marshal.StructureToPtr(vertexArray[i], IntPtr.Add(ptr, i * elemSize), false);
                FillPolygonE2(hImage, ptr, nArraySize, nNewValue, bFill);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        /// <summary>
        /// Draws a polyline defined by an array of vertices.
        /// </summary>
        public static void SetPolyline(IntPtr hImage, RvPoint[] vertexArray, uint nNewValue)
        {
            if (vertexArray == null || vertexArray.Length < 2) return;
            int nArraySize = vertexArray.Length;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(nArraySize * elemSize);
            try
            {
                for (int i = 0; i < nArraySize; i++)
                    Marshal.StructureToPtr(vertexArray[i], IntPtr.Add(ptr, i * elemSize), false);
                SetPolyline(hImage, ptr, nArraySize, nNewValue);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        /// <summary>
        /// Unwarps an image using 4 source control points (array of 4 points).
        /// </summary>
        public static void Unwarp(KImage src, RvPointF32[] srcCtrls, KImage dest)
        {

            if (srcCtrls == null || srcCtrls.Length != 4)
                throw new ArgumentException("Need exactly 4 control points for Unwarp.");
            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(4 * elemSize);
            try
            {
                for (int i = 0; i < 4; i++)
                    Marshal.StructureToPtr(srcCtrls[i], IntPtr.Add(ptr, i * elemSize), false);
                Unwarp(src.Handle, ptr, dest.Handle);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        // ====================================================================
        // Existing wrapper methods (unchanged)
        // ====================================================================

        public static void CopyImage(KImage src, int sx, int sy, int sw, int sh, KImage dest, int dx = 0, int dy = 0, int dw = -1, int dh = -1)
        {
            if (null == src || dest == null) return;
            CopyPixels(src.Handle, sx, sy, sw, sh, dest.Handle, dx, dy, dw, dh);
        }

        public static void Invert(KImage image)
        {
            if (null == image) return;
            Invert(image.Handle);
        }

        public static void Differ(KImage image, KImage background, int offset, int mode)
        {
            if (null == image || background == null) return;
            Differ(image.Handle, background.Handle, offset, mode);
        }

        public static void Warp(KImage src, RvPointF32[] pSrcControls, KImage dest)
        {
            if (null == src || dest == null) return;
            if (pSrcControls == null || pSrcControls.Length < 4) return;
            if (dest.Handle == IntPtr.Zero) return;

            int cnt = 4;
            IntPtr hSrcCtrls = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RvPointF32)) * cnt);
            try
            {
                for (int i = 0; i < cnt; i++)
                    Marshal.StructureToPtr(pSrcControls[i], IntPtr.Add(hSrcCtrls, i * Marshal.SizeOf(typeof(RvPointF32))), false);

                IntPtr hDstCtrls = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RvPointF32)) * cnt);
                try
                {
                    RvPointF32[] destCtrls = new RvPointF32[cnt];
                    destCtrls[0].x = 0; destCtrls[0].y = 0;
                    destCtrls[1].x = dest.GetWidth() - 1; destCtrls[1].y = 0;
                    destCtrls[2].x = dest.GetWidth() - 1; destCtrls[2].y = dest.GetHeight() - 1;
                    destCtrls[3].x = 0; destCtrls[3].y = dest.GetHeight() - 1;

                    for (int i = 0; i < cnt; i++)
                        Marshal.StructureToPtr(destCtrls[i], IntPtr.Add(hDstCtrls, i * Marshal.SizeOf(typeof(RvPointF32))), false);

                    Warp(src.Handle, hSrcCtrls, hDstCtrls, dest.Handle);
                }
                finally
                {
                    Marshal.FreeHGlobal(hDstCtrls);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(hSrcCtrls);
            }
        }

        public static void Warp(KImage src, RvPointF32[] pSrcControls, KImage dest, RvPointF32[] pDestControls, bool bKeepOutlier)
        {
            if (null == src || dest == null) return;
            if (pSrcControls == null || pSrcControls.Length < 4) return;
            if (pDestControls == null || pDestControls.Length < 4) return;
            if (dest.Handle == IntPtr.Zero) return;

            int cnt = 4;
            IntPtr hSrcCtrls = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RvPointF32)) * cnt);
            IntPtr hDstCtrls = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RvPointF32)) * cnt);
            try
            {
                for (int i = 0; i < cnt; i++)
                {
                    Marshal.StructureToPtr(pSrcControls[i], IntPtr.Add(hSrcCtrls, i * Marshal.SizeOf(typeof(RvPointF32))), false);
                    Marshal.StructureToPtr(pDestControls[i], IntPtr.Add(hDstCtrls, i * Marshal.SizeOf(typeof(RvPointF32))), false);
                }
                WarpEx(src.Handle, hSrcCtrls, cnt, hDstCtrls, cnt, bKeepOutlier, dest.Handle);
            }
            finally
            {
                Marshal.FreeHGlobal(hSrcCtrls);
                Marshal.FreeHGlobal(hDstCtrls);
            }
        }

        public static KImage Clip(KImage image, int x, int y, uint width, uint height)
        {
            if (image != null)
            {
                IntPtr h = rvClip(image.Handle, x, y, (int)(x + width), (int)(y + height));

                if (h != IntPtr.Zero)
                {
                    return new KImage(h, false);
                }
            }

            return null;

        }
    }

}
