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

    using size_t = System.UIntPtr;

    // ====================================================================
    // 枚举定义
    // ====================================================================

    public enum ContourCoder
    {
        Unknown = -1,
        Default = 0,   // No fitting, raw point coordinates
        Simple,        // Simple line segments, contour may be discontinuous
        Approx,        // Reserved
    }

    public enum ContourHierarchy
    {
        Unknown = -1,
        Default = 0,   // No hierarchy
        Compound,      // Two-level BLOB
        Tree,          // Tree structure
    }

    public enum ContourType
    {
        Unknown = -1,
        //NotClose = 0,
        Hole = 1,              // Hole
        Normal = (1 << 1),     // Normal contour (outermost)
    }

    [Flags]
    public enum ContourFilter
    {
        External = ContourType.Normal,                    // Outermost contours
        HoleOnly = ContourType.Hole,                      // Only holes
        Any = (ContourType.Normal | ContourType.Hole),   // All closed contours
        Max = (1 << 16),        // Maximum contour
        //Compound = (1 << 17),   // BLOB
        //Tree = (1 << 18),       // Tree
    }

    /// <summary>
    /// Ordering field constants for sorting contours.
    /// </summary>
    public static class ContourOrder
    {
        public const int Area = 1;          // Area
        public const int Perim = (1 << 1);  // Perimeter
        public const int Angle = (1 << 2);  // Angle
        public const int Circular = (1 << 3); // Circularity
        public const int KeyPos = (1 << 4); // Key point count
        public const int RawPos = (1 << 5); // Raw point count
    }

    /// <summary>
    /// Contour comparison methods.
    /// </summary>
    public static class ContourMatch
    {
        public const int Match1 = 1;
        public const int Match2 = 2;
        public const int Match3 = 3;
    }

    // ====================================================================
    // KContour class
    // ====================================================================

    /// <summary>
    /// Represents a contour (polyline) that wraps an unmanaged contour handle.
    /// Inherits from KingsHandle for deterministic resource release.
    /// </summary>
    public class KContour : KingsHandler
    {
        // ====================================================================
        // DLL import constants (debug/release)
        // ====================================================================

#if DEBUGGING_KINGPOOL_SUITE
        private const string LIB_NAME = "anlz_d.dll";
#else
        private const string LIB_NAME = "anlz.dll";
#endif

        // ====================================================================
        // Unmanaged function imports (CharSet.Ansi, EntryPoint specified)
        // ====================================================================

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCreateContour")]
        private static extern IntPtr CreateContour(ContourCoder code);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDestroyContour")]
        private static extern void DestroyContour(IntPtr hContour);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvSetContourDataE1")]
        private static extern void SetContourDataE1(IntPtr contour, ContourCoder type, IntPtr pArray, int count);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetCntrPointCount")]
        private static extern uint GetCntrPointCount(IntPtr hContour);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetContourDataE1")]
        private static extern int GetContourDataE1(IntPtr hContour, IntPtr pArray, int count);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvAppendContourData")]
        private static extern int AppendContourData(IntPtr hContour, int parent, ContourCoder type, IntPtr pArray, uint count);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetContourDataAt")]
        private static extern IntPtr GetContourDataAt(IntPtr hContour, int index);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetContourCountAt")]
        private static extern int GetContourCountAt(IntPtr hContour, int index);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetTotalContours")]
        private static extern int GetTotalContours(IntPtr hContour);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetContourBaseData")]
        private static extern IntPtr GetContourBaseData(IntPtr hContour);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetContourBaseCount")]
        private static extern int GetContourBaseCount(IntPtr hContour);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetContourHierarchy")]
        private static extern ContourHierarchy GetContourHierarchy(IntPtr hContour);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvSetContourHierarchy")]
        private static extern void SetContourHierarchy(IntPtr hContour, ContourHierarchy hierarchy);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetContourRect")]
        private static extern RvRect GetContourRect(IntPtr hContour);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetContourBoundBox")]
        private static extern RvBox2D GetContourBoundBox(IntPtr hContour);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetContourType")]
        private static extern int GetContourType(IntPtr hContour, int index);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetContourCoder")]
        private static extern int GetContourCoder(IntPtr hContour); // obsolete

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetContourCode")]
        private static extern ContourCoder GetContourCode(IntPtr hContour);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvSetContourCode")]
        private static extern void SetContourCode(IntPtr hContour, ContourCoder code);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetContourCircular")]
        private static extern double GetContourCircular(IntPtr hContour);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetContourArea")]
        private static extern double GetContourArea(IntPtr hContour);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetContourPerim")]
        private static extern double GetContourPerim(IntPtr hContour);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetContourAngle")]
        private static extern double GetContourAngle(IntPtr hContour);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetContourCentroid")]
        private static extern RvPointF32 GetContourCentroid(IntPtr hContour);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetContourEccentricity")]
        private static extern double GetContourEccentricity(IntPtr hContour);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvGetContourAngleE1")]
        private static extern double GetContourAngleE1(IntPtr hContour);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFindContours")]
        private static extern int rvgFindContours(IntPtr hImage, ContourCoder code, ContourFilter filter, int minLength, IntPtr seqOut);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFindContoursE1")]
        private static extern int FindContoursE1(IntPtr hImage, ContourCoder code, int maxCount, int order, IntPtr seqOut);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvFindContoursE2")]
        private static extern int rvFindContoursE2(IntPtr image, ContourCoder code, IntPtr treeOut);


        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCloneContour")]
        private static extern IntPtr CloneContour(IntPtr hContour);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvMoveContour")]
        private static extern void MoveContour(IntPtr hContour, int dx, int dy);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCompareContours")]
        private static extern double CompareContours(IntPtr contour1, IntPtr contour2, int method);

        // ====================================================================
        // Constructors
        // ====================================================================

        public KContour()
        {
            _handle = CreateContour(ContourCoder.Default);
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create KContour.");
            _attached = false;
        }

        public KContour(ContourCoder coder)
        {
            _handle = CreateContour(coder);
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create KContour with specified coder.");
            _attached = false;
        }

        public KContour(IntPtr existingHandle, bool attach = true)
        {
            if (existingHandle == IntPtr.Zero)
                throw new ArgumentException("Invalid contour handle", nameof(existingHandle));
            _handle = existingHandle;
            _attached = attach;
        }

        // ====================================================================
        // Sealed ReleaseHandle implementation
        // ====================================================================

        protected sealed override void ReleaseHandle()
        {
            if (_handle != IntPtr.Zero && !_attached)
            {
                DestroyContour(_handle);
            }
            // If _attached == true, we do not destroy it.
        }

        // ====================================================================
        // Public instance methods
        // ====================================================================

        // GetHandle() and IsValid() are removed – use base class properties Handle and IsValid.

        public void SetData(ContourCoder coder, RvPoint[] points)
        {
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("Contour handle is invalid.");
            if (points == null || points.Length == 0)
                throw new ArgumentException("Points cannot be null or empty.", nameof(points));

            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(points.Length * elemSize);
            try
            {
                for (int i = 0; i < points.Length; i++)
                {
                    IntPtr offset = IntPtr.Add(ptr, i * elemSize);
                    Marshal.StructureToPtr(points[i], offset, false);
                }
                SetContourDataE1(_handle, coder, ptr, points.Length);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public uint PointCount => _handle != IntPtr.Zero ? GetCntrPointCount(_handle) : 0;

        public RvPoint[] GetPoints()
        {
            if (_handle == IntPtr.Zero) return null;
            uint count = PointCount;
            if (count == 0) return null;

            RvPoint[] points = new RvPoint[count];
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr buffer = Marshal.AllocHGlobal((int)count * elemSize);
            try
            {
                int realCount = GetContourDataE1(_handle, buffer, (int)count);
                if (realCount <= 0) return null;
                for (int i = 0; i < realCount; i++)
                {
                    IntPtr ptr = IntPtr.Add(buffer, i * elemSize);
                    points[i] = Marshal.PtrToStructure<RvPoint>(ptr);
                }
                if (realCount < count)
                    Array.Resize(ref points, realCount);
                return points;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        public int AppendData(int parent, ContourCoder coder, RvPoint[] points)
        {
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("Contour handle is invalid.");
            if (points == null || points.Length == 0)
                return -1;

            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(points.Length * elemSize);
            try
            {
                for (int i = 0; i < points.Length; i++)
                {
                    IntPtr offset = IntPtr.Add(ptr, i * elemSize);
                    Marshal.StructureToPtr(points[i], offset, false);
                }
                return AppendContourData(_handle, parent, coder, ptr, (uint)points.Length);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public IntPtr GetDataAt(int index)
        {
            return _handle != IntPtr.Zero ? GetContourDataAt(_handle, index) : IntPtr.Zero;
        }

        public int GetCountAt(int index)
        {
            return _handle != IntPtr.Zero ? GetContourCountAt(_handle, index) : 0;
        }

        public int TotalContours => _handle != IntPtr.Zero ? GetTotalContours(_handle) : 0;

        public IntPtr GetBaseData()
        {
            return _handle != IntPtr.Zero ? GetContourBaseData(_handle) : IntPtr.Zero;
        }

        public int BaseCount => _handle != IntPtr.Zero ? GetContourBaseCount(_handle) : 0;

        public ContourHierarchy Hierarchy
        {
            get => _handle != IntPtr.Zero ? GetContourHierarchy(_handle) : ContourHierarchy.Unknown;
            set { if (_handle != IntPtr.Zero) SetContourHierarchy(_handle, value); }
        }

        public RvRect Rect => _handle != IntPtr.Zero ? GetContourRect(_handle) : new RvRect();
        public RvBox2D BoundBox => _handle != IntPtr.Zero ? GetContourBoundBox(_handle) : new RvBox2D();

        public int GetTypeAt(int index)
        {
            return _handle != IntPtr.Zero ? GetContourType(_handle, index) : -1;
        }

        public int GetCoder() // obsolete
        {
            return _handle != IntPtr.Zero ? GetContourCoder(_handle) : -1;
        }

        public ContourCoder Code
        {
            get => _handle != IntPtr.Zero ? GetContourCode(_handle) : ContourCoder.Unknown;
            set { if (_handle != IntPtr.Zero) SetContourCode(_handle, value); }
        }

        public double Circularity => _handle != IntPtr.Zero ? GetContourCircular(_handle) : 0;
        public double Area => _handle != IntPtr.Zero ? GetContourArea(_handle) : 0;
        public double Perimeter => _handle != IntPtr.Zero ? GetContourPerim(_handle) : 0;
        public double Angle => _handle != IntPtr.Zero ? GetContourAngle(_handle) : 0;
        public double AngleNormalized => _handle != IntPtr.Zero ? GetContourAngleE1(_handle) : 0;
        public RvPointF32 Centroid => _handle != IntPtr.Zero ? GetContourCentroid(_handle) : new RvPointF32();
        public double Eccentricity => _handle != IntPtr.Zero ? GetContourEccentricity(_handle) : 0;

        public KContour Clone()
        {
            if (_handle == IntPtr.Zero) return null;
            IntPtr newHandle = CloneContour(_handle);
            return newHandle != IntPtr.Zero ? new KContour(newHandle, false) : null;
        }

        public void Move(int dx, int dy)
        {
            if (_handle != IntPtr.Zero)
                MoveContour(_handle, dx, dy);
        }

        public double CompareTo(KContour other, int method)
        {
            if (_handle == IntPtr.Zero || other == null || other._handle == IntPtr.Zero)
                return 0;
            return CompareContours(_handle, other._handle, method);
        }

        // ====================================================================
        // Static methods for contour extraction from images
        // ====================================================================

        public static int FindContours(KImage image, ContourCoder coder, ContourFilter filter, int minLength, KSequence seqOut)
        {
            if (image == null || seqOut == null) return 0;

            if (image.Handle == IntPtr.Zero || seqOut.Handle == IntPtr.Zero) return 0;
            return rvgFindContours(image.Handle, coder, filter, minLength, seqOut.Handle);
        }

        public static int FindContours(KImage image, ContourCoder coder, int maxCount, int order, KSequence seqOut)
        {
            if (image == null || seqOut == null) return 0;

            if (image.Handle == IntPtr.Zero || seqOut.Handle == IntPtr.Zero) return 0;
            return FindContoursE1(image.Handle, coder, maxCount, order, seqOut.Handle);
        }

        private static int FindContours(KImage image, ContourCoder coder, KTree treeOut)
        {
            if (image == null || treeOut == null) return 0;

            if (image.Handle == IntPtr.Zero || treeOut.Handle == IntPtr.Zero) return 0;
            return rvFindContoursE2(image.Handle, coder, treeOut.Handle);

        }

        public static void ReleaseContour(IntPtr hContour)
        {
            if (hContour != IntPtr.Zero)
            {
                DestroyContour(hContour);
            }
        }
        public static void ReleaseContours(KSequence seqContour)
        {
            if (seqContour != null)
            {
                for (int i = 0; i < seqContour.Count; i++)
                {
                    DestroyContour(seqContour.GetAt(i));
                }

                seqContour.Clear();
            }
        }
    }
}
