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


    public enum BlobPart
    {
        Whole = 0xFF,
        North = 1,
        East = (1 << 1),
        South = (1 << 2),
        West = (1 << 3),
        NorthWest = (North | West),
        NorthEast = (North | East),
        SouthWest = (South | West),
        SouthEast = (South | East),
    }

    public enum BinarizationType
    {
        Gaussian = 0,
        Poinson = 1,
        Entropy = 2,
        Majority = 3,
        Otsu = 4,
    }

    /// <summary>
    /// Static utility class for image analysis.
    /// Wraps anlz_d.dll / anlz.dll.
    /// </summary>
    public static class Dia
    {
        // ====================================================================
        // Constants (RV_ prefix removed)
        // ====================================================================

        // rvGetBoundBoxE1 order
        public const int GBB_AREA = 1;
        public const int GBB_PERIM = (1 << 1);

        // Blob density part
        public const int BSP_WHOLE = 0xFF;
        public const int BSP_NORTH = 1;
        public const int BSP_EAST = (1 << 1);
        public const int BSP_SOUTH = (1 << 2);
        public const int BSP_WEST = (1 << 3);
        public const int BSP_NW = (BSP_NORTH | BSP_WEST);
        public const int BSP_NE = (BSP_NORTH | BSP_EAST);
        public const int BSP_SW = (BSP_SOUTH | BSP_WEST);
        public const int BSP_SE = (BSP_SOUTH | BSP_EAST);

        // Auto-binarizing methods
        public const int AB_GAUSSIAN = 0;
        public const int AB_POINSON = 1;
        public const int AB_ENTROPY = 2;
        public const int AB_MAJORITY = 3;
        public const int AB_OTSU = 4;

        // Canny contrast methods
        public const int ECC_MEDINA = 0;
        public const int ECC_ADAPTIVE = 1;
        public const int ECC_HALIKE = 2;

        // ====================================================================
        // DLL import constants
        // ====================================================================

#if DEBUGGING_KINGPOOL_SUITE
        private const string LIB_NAME = "anlz_d.dll";
#else
        private const string LIB_NAME = "anlz.dll";
#endif

        // ====================================================================
        // Unmanaged function imports
        // ====================================================================

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCountPixels")]
        public static extern long CountPixels(IntPtr hImage, IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCountPixelsEx")]
        public static extern IntPtr CountPixelsEx(IntPtr hImage, IntPtr hMask, IntPtr reading);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvHistogram")]
        public static extern int rvHistogram(IntPtr hImage, IntPtr pReadingArray, int nArraySize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvHistogramEx")]
        public static extern IntPtr rvHistogramEx(IntPtr hImage, IntPtr reading);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetClearness")]
        public static extern double GetClearness(IntPtr hImage, IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetBoundRect")]
        public static extern RvRect GetBoundRect(IntPtr hImage, IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetBoundBox")]
        public static extern RvBox2D GetBoundBox(IntPtr hImage, IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetBoundBoxE1")]
        public static extern RvBox2D GetBoundBoxE1(IntPtr hImage, int order, bool bIncludeBorder);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcGrayStatsEx")]
        public static extern IntPtr CalcGrayStatsEx(IntPtr hImage, IntPtr hMask, IntPtr reading);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCalcGrayStats")]
        public static extern int CalcGrayStats(IntPtr hImage, IntPtr hMask, IntPtr pReadingArray, int nArraySize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvRetrievePosArray")]
        public static extern int RetrievePosArray(IntPtr hImage, IntPtr pPosArray, int arrSize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetPixelSumEx")]
        public static extern double GetPixelSumEx(IntPtr hImage, int x, int y, int radius, IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFindPolar")]
        public static extern int FindPolar(IntPtr hImage, IntPtr hMask, IntPtr pPosOut, bool bFindMinimum);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFindPolarEx")]
        public static extern IntPtr FindPolarEx(IntPtr hImage, IntPtr hMask, IntPtr reading);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvSummary")]
        public static extern double Summary(IntPtr hImage, IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvSummaryE1")]
        public static extern double SummaryE1(IntPtr hImage, RvRect rect);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvSummaryEx")]
        public static extern IntPtr SummaryEx(IntPtr hImage, IntPtr hMask, IntPtr reading);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvVariance")]
        public static extern double Variance(IntPtr hImage, IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvVarianceEx")]
        public static extern IntPtr VarianceEx(IntPtr hImage, IntPtr hMask, IntPtr reading);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvAverage")]
        public static extern double Average(IntPtr hImage, IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvAverageEx")]
        public static extern IntPtr AverageEx(IntPtr hImage, IntPtr hMask, IntPtr reading);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvArea")]
        public static extern uint Area(IntPtr hImage, IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCircular")]
        public static extern double Circular(IntPtr hImage, IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvSlope")]
        public static extern double Slope(IntPtr hImage, IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCentroid")]
        public static extern RvPointF32 Centroid(IntPtr hImage, IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFindBoundRect")]
        public static extern bool FindBoundRect(IntPtr hImage, ref RvRect seed, bool bInward);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvPerimeter")]
        public static extern uint Perimeter(IntPtr hImage, IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDensity")]
        public static extern double Density(IntPtr hImage, int subRegion);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvProjectEx")]
        public static extern IntPtr ProjectEx(IntPtr hImage, int dir, IntPtr reading);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvProject")]
        public static extern int Project(IntPtr hImage, int dir, IntPtr pReadingArray, int nArraySize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvBoxVertex")]
        private static extern void rvBoxVertex(ref RvBox2D pBox, IntPtr arrPts4);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetPixelSum")]
        public static extern uint GetPixelSum(IntPtr hImage, int x, int y, int radius);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetLum")]
        public static extern double GetLum(IntPtr hImage, IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetBinarizationOptimum")]
        public static extern byte GetBinarizationOptimum(IntPtr hImage, IntPtr hMask, int method);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvEstimateCannyContrast")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EstimateCannyContrast(IntPtr hImage, IntPtr hMask, int method, float extra,
                                                       ref float minContrast, ref float maxContrast);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvEstimateImmerkaerNoise")]
        public static extern float EstimateImmerkaerNoise(IntPtr hImage, IntPtr hMask);

        // ====================================================================
        // Private helper: wraps a returned handle into a KFdox instance
        // ====================================================================

        private static KFdox WrapResult(IntPtr returnedHandle, KFdox provided)
        {
            if (provided != null) return provided;
            if (returnedHandle == IntPtr.Zero) return null;
            return new KFdox(returnedHandle, false);
        }

        // ====================================================================
        // Public wrapper: BoxVertex
        // ====================================================================

        public static void BoxVertex(ref RvBox2D pBox, RvPointF32[] arrPts4)
        {
            if (arrPts4 == null || arrPts4.Length != 4)
                throw new ArgumentException("arrPts4 must contain exactly 4 points.", nameof(arrPts4));

            int elemSize = Marshal.SizeOf(typeof(RvPointF32));
            IntPtr ptr = Marshal.AllocHGlobal(4 * elemSize);
            try
            {
                for (int i = 0; i < 4; i++)
                    Marshal.StructureToPtr(arrPts4[i], IntPtr.Add(ptr, i * elemSize), false);
                rvBoxVertex(ref pBox, ptr);
                for (int i = 0; i < 4; i++)
                    arrPts4[i] = Marshal.PtrToStructure<RvPointF32>(IntPtr.Add(ptr, i * elemSize));
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        // ====================================================================
        // Convenience wrappers using KImage / KMask / KFdox
        // ====================================================================

        public static long CountPixels(KImage image, KMask mask = null)
        {
            if (image == null || !image.IsValid) return 0;
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            return CountPixels(image.Handle, hMask);
        }

        public static KFdox CountPixelsEx(KImage image, KMask mask = null, KFdox reading = null)
        {
            if (image == null || !image.IsValid) return null;
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            IntPtr readingHandle = reading?.Handle ?? IntPtr.Zero;
            IntPtr h = CountPixelsEx(image.Handle, hMask, readingHandle);
            return WrapResult(h, reading);
        }

        public static int Histogram(KImage image, long[] readingArray)
        {
            if (image == null || !image.IsValid) return 0;
            if (readingArray == null || readingArray.Length == 0) return 0;


            int nArraySize = readingArray.Length;
            int elemSize = sizeof(long);
            IntPtr ptr = Marshal.AllocHGlobal(nArraySize * elemSize);
            try
            {
                int written = rvHistogram(image.Handle, ptr, nArraySize);
                if (written > 0)
                {
                    int count = Math.Min(written, nArraySize);
                    Marshal.Copy(ptr, readingArray, 0, count);
                }
                return written;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }



        public static KFdox HistogramEx(KImage image, KFdox reading = null)
        {
            if (image == null || !image.IsValid) return null;

            IntPtr readingHandle = reading?.Handle ?? IntPtr.Zero;
            IntPtr h = rvHistogramEx(image.Handle, readingHandle);
            return WrapResult(h, reading);
        }

        public static double GetClearness(KImage image, KMask mask = null)
        {
            if (image == null || !image.IsValid) return 0;
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            return GetClearness(image.Handle, hMask);
        }

        public static RvRect GetBoundRect(KImage image, KMask mask = null)
        {
            if (image == null || !image.IsValid) return new RvRect();
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            return GetBoundRect(image.Handle, hMask);
        }

        public static RvBox2D GetBoundBox(KImage image, KMask mask = null)
        {
            if (image == null || !image.IsValid) return new RvBox2D();
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            return GetBoundBox(image.Handle, hMask);
        }

        public static RvBox2D GetBoundBoxE1(KImage image, int order, bool bIncludeBorder)
        {
            if (image == null || !image.IsValid) return new RvBox2D();
            return GetBoundBoxE1(image.Handle, order, bIncludeBorder);
        }

        public static KFdox CalcGrayStatsEx(KImage image, KMask mask = null, KFdox reading = null)
        {
            if (image == null || !image.IsValid) return null;
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            IntPtr readingHandle = reading?.Handle ?? IntPtr.Zero;
            IntPtr h = CalcGrayStatsEx(image.Handle, hMask, readingHandle);
            return WrapResult(h, reading);
        }

        public static int CalcGrayStats(KImage image, KMask mask, double[] readingArray)
        {
            if (image == null || !image.IsValid) return 0;
            if (readingArray == null || readingArray.Length == 0) return 0;

            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            int nArraySize = readingArray.Length;
            int elemSize = sizeof(double);
            IntPtr ptr = Marshal.AllocHGlobal(nArraySize * elemSize);
            try
            {
                int written = CalcGrayStats(image.Handle, hMask, ptr, nArraySize);
                if (written > 0)
                {
                    int count = Math.Min(written, nArraySize);
                    Marshal.Copy(ptr, readingArray, 0, count);
                }
                return written;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public static int CalcGrayStats(KImage image, KMask mask, IntPtr pReadingArray, int nArraySize)
        {
            if (image == null || !image.IsValid) return 0;
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            return CalcGrayStats(image.Handle, hMask, pReadingArray, nArraySize);
        }

        public static double GetPixelSumEx(KImage image, int x, int y, int radius, KMask mask = null)
        {
            if (image == null || !image.IsValid) return 0;
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            return GetPixelSumEx(image.Handle, x, y, radius, hMask);
        }

        public static int FindPolar(KImage image, KMask mask, out RvPoint pos, bool bFindMinimum)
        {
            pos = new RvPoint();
            if (image == null || !image.IsValid) return 0;

            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(elemSize);
            try
            {
                int result = FindPolar(image.Handle, hMask, ptr, bFindMinimum);
                if (result > 0)
                    pos = Marshal.PtrToStructure<RvPoint>(ptr);
                return result;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public static int FindPolar(KImage image, KMask mask, IntPtr pPosOut, bool bFindMinimum)
        {
            if (image == null || !image.IsValid) return 0;
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            return FindPolar(image.Handle, hMask, pPosOut, bFindMinimum);
        }

        public static KFdox FindPolarEx(KImage image, KMask mask = null, KFdox reading = null)
        {
            if (image == null || !image.IsValid) return null;
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            IntPtr readingHandle = reading?.Handle ?? IntPtr.Zero;
            IntPtr h = FindPolarEx(image.Handle, hMask, readingHandle);
            return WrapResult(h, reading);
        }

        public static double Summary(KImage image, KMask mask = null)
        {
            if (image == null || !image.IsValid) return 0;
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            return Summary(image.Handle, hMask);
        }

        public static double SummaryE1(KImage image, RvRect rect)
        {
            if (image == null || !image.IsValid) return 0;
            return SummaryE1(image.Handle, rect);
        }

        public static KFdox SummaryEx(KImage image, KMask mask = null, KFdox reading = null)
        {
            if (image == null || !image.IsValid) return null;
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            IntPtr readingHandle = reading?.Handle ?? IntPtr.Zero;
            IntPtr h = SummaryEx(image.Handle, hMask, readingHandle);
            return WrapResult(h, reading);
        }

        public static double Variance(KImage image, KMask mask = null)
        {
            if (image == null || !image.IsValid) return 0;
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            return Variance(image.Handle, hMask);
        }

        public static KFdox VarianceEx(KImage image, KMask mask = null, KFdox reading = null)
        {
            if (image == null || !image.IsValid) return null;
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            IntPtr readingHandle = reading?.Handle ?? IntPtr.Zero;
            IntPtr h = VarianceEx(image.Handle, hMask, readingHandle);
            return WrapResult(h, reading);
        }

        public static double Average(KImage image, KMask mask = null)
        {
            if (image == null || !image.IsValid) return 0;
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            return Average(image.Handle, hMask);
        }

        public static KFdox AverageEx(KImage image, KMask mask = null, KFdox reading = null)
        {
            if (image == null || !image.IsValid) return null;
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            IntPtr readingHandle = reading?.Handle ?? IntPtr.Zero;
            IntPtr h = AverageEx(image.Handle, hMask, readingHandle);
            return WrapResult(h, reading);
        }

        public static uint Area(KImage image, KMask mask = null)
        {
            if (image == null || !image.IsValid) return 0;
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            return Area(image.Handle, hMask);
        }

        public static double Circular(KImage image, KMask mask = null)
        {
            if (image == null || !image.IsValid) return 0;
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            return Circular(image.Handle, hMask);
        }

        public static double Slope(KImage image, KMask mask = null)
        {
            if (image == null || !image.IsValid) return 0;
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            return Slope(image.Handle, hMask);
        }

        public static RvPointF32 Centroid(KImage image, KMask mask = null)
        {
            if (image == null || !image.IsValid) return new RvPointF32();
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            return Centroid(image.Handle, hMask);
        }

        public static bool FindBoundRect(KImage image, ref RvRect seed, bool bInward)
        {
            if (image == null || !image.IsValid) return false;
            return FindBoundRect(image.Handle, ref seed, bInward);
        }

        public static uint Perimeter(KImage image, KMask mask = null)
        {
            if (image == null || !image.IsValid) return 0;
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            return Perimeter(image.Handle, hMask);
        }

        public static double Density(KImage image, BlobPart subRegion)
        {
            if (image == null || !image.IsValid) return 0;
            return Density(image.Handle, (int)subRegion);
        }

        public static KFdox ProjectEx(KImage image, int dir, KFdox reading = null)
        {
            if (image == null || !image.IsValid) return null;

            IntPtr readingHandle = reading?.Handle ?? IntPtr.Zero;
            IntPtr h = ProjectEx(image.Handle, dir, readingHandle);
            return WrapResult(h, reading);
        }

        public static int Project(KImage image, int dir, long[] readingArray)
        {
            if (image == null || !image.IsValid) return 0;
            if (readingArray == null || readingArray.Length == 0) return 0;


            int nArraySize = readingArray.Length;
            int elemSize = sizeof(long);
            IntPtr ptr = Marshal.AllocHGlobal(nArraySize * elemSize);
            try
            {
                int written = Project(image.Handle, dir, ptr, nArraySize);
                if (written > 0)
                {
                    int count = Math.Min(written, nArraySize);
                    Marshal.Copy(ptr, readingArray, 0, count);
                }
                return written;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public static int Project(KImage image, int dir, IntPtr pReadingArray, int nArraySize)
        {
            if (image == null || !image.IsValid) return 0;

            return Project(image.Handle, dir, pReadingArray, nArraySize);
        }

        public static uint GetPixelSum(KImage image, int x, int y, int radius)
        {
            if (image == null || !image.IsValid) return 0;
            return GetPixelSum(image.Handle, x, y, radius);
        }

        public static double GetLum(KImage image, KMask mask = null)
        {
            if (image == null || !image.IsValid) return 0;
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            return GetLum(image.Handle, hMask);
        }

        public static byte GetBinarizationOptimum(KImage image, KMask mask, int method)
        {
            if (image == null || !image.IsValid) return 0;
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            return GetBinarizationOptimum(image.Handle, hMask, method);
        }

        public static bool EstimateCannyContrast(KImage image, KMask mask, int method, float extra,
                                                 ref float minContrast, ref float maxContrast)
        {
            if (image == null || !image.IsValid) return false;
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            return EstimateCannyContrast(image.Handle, hMask, method, extra, ref minContrast, ref maxContrast);
        }

        public static float EstimateImmerkaerNoise(KImage image, KMask mask = null)
        {
            if (image == null || !image.IsValid) return 0;
            IntPtr hMask = mask?.Handle ?? IntPtr.Zero;
            return EstimateImmerkaerNoise(image.Handle, hMask);
        }
    }

}
