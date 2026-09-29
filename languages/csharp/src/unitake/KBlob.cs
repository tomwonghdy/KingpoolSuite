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
    // 定义 size_t 别名（如果需要）
    using size_t = System.UIntPtr;

    /// <summary>
    /// Blob encoder types.
    /// </summary>
    public enum BlobEncoder
    {
        Unknown = -1,
        Raw = 0,  //原图
        RLE = 1,  //跑马编码,目前不支持
        Contour = 2,  //轮廓线
        Cluster = 3,  //点簇
    }

    /// <summary>
    /// Blob distance measurement types.
    /// </summary>
    public enum BlobDistance
    {
        Default = 0,    // 质心距离
        Rect = 1,       // 外接矩形
        MinBox = 2,     // 最小矩形
        Contour = 3,    // 轮廓（暂不支持）
    }

    /// <summary>
    /// Represents a binary object (Blob) that wraps an unmanaged blob handle.
    /// Inherits from KingsHandler for deterministic resource release.
    /// </summary>
    public class KBlob : KingsHandler
    {


        // ====================================================================
        // DLL import constants (debug/release)
        // ====================================================================

#if DEBUGGING_KINGPOOL_SUITE
        private const string BLOB_LIB_NAME = "blob_d.dll";
#else
        private const string BLOB_LIB_NAME = "blob.dll";
#endif

        // ====================================================================
        // Unmanaged function imports (CharSet.Ansi)
        // ====================================================================

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rvCreateBlob(IntPtr baseImage = default(IntPtr));

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rvCreateBlobEx(IntPtr image, int encoder);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern void rvDestroyBlob(IntPtr blob);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern void rvbClear(IntPtr blob);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rvbIsEmpty(IntPtr blob);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rvbIsNormal(IntPtr blob);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern BlobEncoder rvbGetEncoder(IntPtr blob);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rvbClone(IntPtr blob, IntPtr dest = default(IntPtr));

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rvbHitTest(IntPtr blob, int x, int y);

        // Conversion functions
        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rvTransToRawBlob(IntPtr imBin, IntPtr destBlob = default(IntPtr));

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern int rvTransToRawBlobE1(IntPtr imBin, int maxCount, IntPtr seqOut = default(IntPtr));

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rvTransToClusterBlob(IntPtr imBin, IntPtr destBlob = default(IntPtr));

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern int rvTransToClusterBlobE1(IntPtr imBin, int maxCount, IntPtr seqOut = default(IntPtr));

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rvTransToContourBlob(IntPtr imBin, int chainCoder = 0, IntPtr destBlob = default(IntPtr));

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rvTransToClusterList(IntPtr imBin, [MarshalAs(UnmanagedType.Bool)] bool bConn8, int minSize);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rvTransToRawList(IntPtr imBin, [MarshalAs(UnmanagedType.Bool)] bool bConn8, int minSize);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rvTransToContourList(IntPtr imBin, int chainCoder, int filter, int minSize);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rvTransToContourListE1(IntPtr imBin, int chainCoder, int minSize, int maxCount);

        // Transform to image/mask
        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rvTransformToImage(IntPtr blob, IntPtr dest = default(IntPtr));

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rvTransformToMask(IntPtr blob, IntPtr dest = default(IntPtr));

        // Cluster data
        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rvbGetClusterData(IntPtr blob, out int pSize);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern int rvbGetClusterSize(IntPtr blob);

        // Size
        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern int rvbGetWidth(IntPtr blob);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern int rvbGetHeight(IntPtr blob);

        // Tag
        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern void rvbSetTag(IntPtr blob, int tag);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern int rvbGetTag(IntPtr blob);

        // Geometry
        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern RvPointF32 rvbGetCentroid(IntPtr blob);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern uint rvbGetArea(IntPtr blob);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern RvBox2D rvbGetBoundBox(IntPtr blob);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern RvPoint rvbGetOffset(IntPtr blob);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern RvRect rvbGetRect(IntPtr blob);

        // Statistics
        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern uint rvbGetSummary(IntPtr blob, IntPtr image, out double pAvg, out double pVar);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern double rvbGetSummaryF64(IntPtr blob, IntPtr image, out double pAvg, out double pVar);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern double rvbGetAverage(IntPtr blob, IntPtr image, out double pVar);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern double rvbGetVariance(IntPtr blob, IntPtr image);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern double rvbGetStrength(IntPtr blob, IntPtr image);

        // Edges
        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern int rvbGetEdges(IntPtr blob, IntPtr pPointArray, int nArraySize);

        // Perimeter
        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern double rvbGetPerimeter(IntPtr blob);

        // Circularity and density
        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern double rvbGetCircular(IntPtr blob);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern double rvbGetDensity(IntPtr blob, BlobPart part);

        // Orientation
        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern double rvbGetSlope(IntPtr blob);

        // Distance
        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern float rvbDistance(IntPtr blob0, IntPtr blob1, BlobDistance type);

        // Merge
        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rvbMerge(IntPtr blob, IntPtr dest);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rvbMergeTree(IntPtr blob, int relation, IntPtr dest);

        // Release list
        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern void rvbReleaseBlobList(IntPtr seq);

        [DllImport(BLOB_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern void rvReleaseBlobList(ref IntPtr seq);

        // ====================================================================
        // Constructors
        // ====================================================================

        public KBlob()
        {
            _handle = rvCreateBlob(IntPtr.Zero);
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create KBlob.");
            _attached = false;
        }

        public KBlob(BlobEncoder encoder, IntPtr baseImage = default(IntPtr))
        {
            _handle = rvCreateBlobEx(baseImage, (int)encoder);
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create KBlob with encoder.");
            _attached = false;
        }

        public KBlob(IntPtr existingBlob, bool attach)
        {
            if (existingBlob == IntPtr.Zero)
                throw new ArgumentException("Invalid blob handle", nameof(existingBlob));
            _handle = existingBlob;
            _attached = attach;
        }

        // ====================================================================
        // Sealed ReleaseHandle implementation
        // ====================================================================

        protected sealed override void ReleaseHandle()
        {
            if (_handle != IntPtr.Zero && !_attached)
            {
                rvDestroyBlob(_handle);
            }
        }

        // ====================================================================
        // Public properties and methods
        // ====================================================================

        public void Clear()
        {
            if (_handle != IntPtr.Zero)
                rvbClear(_handle);
        }

        public bool IsEmpty() => _handle != IntPtr.Zero && rvbIsEmpty(_handle);

        public bool IsNormal() => _handle != IntPtr.Zero && rvbIsNormal(_handle);

        public BlobEncoder GetEncoder()
        {
            if (_handle == IntPtr.Zero) return BlobEncoder.Unknown;
            return rvbGetEncoder(_handle);
        }

        public KBlob Clone()
        {
            if (_handle == IntPtr.Zero) return null;
            IntPtr newHandle = rvbClone(_handle, IntPtr.Zero);
            return newHandle != IntPtr.Zero ? new KBlob(newHandle, false) : null;
        }

        public bool HitTest(int x, int y)
        {
            return _handle != IntPtr.Zero && rvbHitTest(_handle, x, y);
        }

        // Static factory methods – accept KImage
        public static KBlob FromBinaryImage(KImage imBin, BlobEncoder encoder = BlobEncoder.Raw)
        {
            if (imBin == null) return null;
            IntPtr handle;
            if (encoder == BlobEncoder.Raw)
                handle = rvTransToRawBlob(imBin.Handle, IntPtr.Zero);
            else if (encoder == BlobEncoder.Cluster)
                handle = rvTransToClusterBlob(imBin.Handle, IntPtr.Zero);
            else if (encoder == BlobEncoder.Contour)
                handle = rvTransToContourBlob(imBin.Handle, 0, IntPtr.Zero);
            else
                throw new NotSupportedException("Encoder not supported for FromBinaryImage.");

            return handle != IntPtr.Zero ? new KBlob(handle, false) : null;
        }

        public static KBlob FromBinaryImageRaw(KImage imBin)
            => FromBinaryImage(imBin, BlobEncoder.Raw);

        public static KBlob FromBinaryImageCluster(KImage imBin)
            => FromBinaryImage(imBin, BlobEncoder.Cluster);

        public static KBlob FromBinaryImageContour(KImage imBin)
            => FromBinaryImage(imBin, BlobEncoder.Contour);

        // Convert blob to image
        public KImage ToImage()
        {
            if (_handle == IntPtr.Zero) return null;
            IntPtr imgHandle = rvTransformToImage(_handle, IntPtr.Zero);
            return imgHandle != IntPtr.Zero ? new KImage(imgHandle, false) : null;
        }

        // Convert blob to mask
        public KMask ToMask()
        {
            if (_handle == IntPtr.Zero) return null;
            IntPtr maskHandle = rvTransformToMask(_handle, IntPtr.Zero);
            return maskHandle != IntPtr.Zero ? new KMask(maskHandle, false) : null;
        }

        // Cluster data
        public RvPoint[] GetClusterPoints()
        {
            if (_handle == IntPtr.Zero) return null;
            int size;
            IntPtr pData = rvbGetClusterData(_handle, out size);
            if (pData == IntPtr.Zero || size <= 0) return null;
            RvPoint[] points = new RvPoint[size];
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            for (int i = 0; i < size; i++)
            {
                IntPtr ptr = IntPtr.Add(pData, i * elemSize);
                points[i] = Marshal.PtrToStructure<RvPoint>(ptr);
            }
            return points;
        }

        public int GetClusterSize() => _handle != IntPtr.Zero ? rvbGetClusterSize(_handle) : 0;

        public int GetWidth() => _handle != IntPtr.Zero ? rvbGetWidth(_handle) : 0;
        public int GetHeight() => _handle != IntPtr.Zero ? rvbGetHeight(_handle) : 0;

        public void SetTag(int tag) { if (_handle != IntPtr.Zero) rvbSetTag(_handle, tag); }
        public int GetTag() => _handle != IntPtr.Zero ? rvbGetTag(_handle) : 0;

        public RvPointF32 GetCentroid() => _handle != IntPtr.Zero ? rvbGetCentroid(_handle) : new RvPointF32();
        public uint GetArea() => _handle != IntPtr.Zero ? rvbGetArea(_handle) : 0;
        public RvBox2D GetBoundBox() => _handle != IntPtr.Zero ? rvbGetBoundBox(_handle) : new RvBox2D();
        public RvPoint GetOffset() => _handle != IntPtr.Zero ? rvbGetOffset(_handle) : new RvPoint();
        public RvRect GetRect() => _handle != IntPtr.Zero ? rvbGetRect(_handle) : new RvRect();

        // Statistics – accept KImage
        public bool GetSummary(KImage image, out double avg, out double var)
        {
            if (_handle == IntPtr.Zero || image == null || image.Handle == IntPtr.Zero)
            {
                avg = var = 0;
                return false;
            }
            rvbGetSummary(_handle, image.Handle, out avg, out var);
            return true;
        }

        public double GetAverage(KImage image)
        {
            if (_handle == IntPtr.Zero || image == null || image.Handle == IntPtr.Zero)
            {
                return 0;
            }
            double n;
            return rvbGetAverage(_handle, image.Handle, out n);
        }

        public double GetVariance(KImage image)
        {
            if (_handle == IntPtr.Zero || image == null || image.Handle == IntPtr.Zero)
                return 0;
            return rvbGetVariance(_handle, image.Handle);
        }

        public double GetStrength(KImage image)
        {
            if (_handle == IntPtr.Zero || image == null || image.Handle == IntPtr.Zero)
                return 0;
            return rvbGetStrength(_handle, image.Handle);
        }

        // Edges
        public RvPoint[] GetEdges()
        {
            if (_handle == IntPtr.Zero) return null;
            int count = rvbGetEdges(_handle, IntPtr.Zero, 0);
            if (count <= 0) return null;
            RvPoint[] points = new RvPoint[count];
            int elemSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr buffer = Marshal.AllocHGlobal(count * elemSize);
            try
            {
                int realCount = rvbGetEdges(_handle, buffer, count);
                if (realCount <= 0) return null;
                for (int i = 0; i < realCount; i++)
                {
                    IntPtr ptr = IntPtr.Add(buffer, i * elemSize);
                    points[i] = Marshal.PtrToStructure<RvPoint>(ptr);
                }
                Array.Resize(ref points, realCount);
                return points;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        public double GetPerimeter() => _handle != IntPtr.Zero ? rvbGetPerimeter(_handle) : 0;
        public double GetCircular() => _handle != IntPtr.Zero ? rvbGetCircular(_handle) : 0;

        // GetDensity accepts KMask (default null)
        public double GetDensity(BlobPart part)
        {
            if (_handle == IntPtr.Zero) return 0;
            return rvbGetDensity(_handle, part);
        }

        public double GetSlope() => _handle != IntPtr.Zero ? rvbGetSlope(_handle) : 0;

        public static float Distance(KBlob blob0, KBlob blob1, BlobDistance type = BlobDistance.Default)
        {
            if (blob0 == null || blob1 == null || blob0._handle == IntPtr.Zero || blob1._handle == IntPtr.Zero)
                return float.MaxValue;
            return rvbDistance(blob0._handle, blob1._handle, type);
        }

        public KBlob Merge(KBlob other)
        {
            if (other == null || other._handle == IntPtr.Zero || _handle == IntPtr.Zero)
                return null;
            IntPtr merged = rvbMerge(other._handle, _handle);
            if (merged == IntPtr.Zero) return null;
            return new KBlob(merged, false);
        }

        // Release blob list – accepts KSequence
        public static void ReleaseBlobList(KSequence seq, bool alsoDestroySequence = true)
        {
            if (seq == null || seq.Handle == IntPtr.Zero) return;
            rvbReleaseBlobList(seq.Handle);
            if (alsoDestroySequence)
                seq.Dispose();
        }

        // Extract lists – return KSequence, accept KImage
        public static KSequence ExtractRawList(KImage imBin, bool bConn8 = false, int minSize = 4)
        {
            if (imBin == null || imBin.Handle == IntPtr.Zero) return null;
            IntPtr hSeq = rvTransToRawList(imBin.Handle, bConn8, minSize);
            return hSeq != IntPtr.Zero ? new KSequence(hSeq, false) : null;
        }

        public static KSequence ExtractClusterList(KImage imBin, bool bConn8, int minSize)
        {
            if (imBin == null || imBin.Handle == IntPtr.Zero) return null;
            IntPtr hSeq = rvTransToClusterList(imBin.Handle, bConn8, minSize);
            return hSeq != IntPtr.Zero ? new KSequence(hSeq, false) : null;
        }

        public static KSequence ExtractContourList(KImage imBin, int chainCoder = 0, int filter = 0, int minSize = 4)
        {
            if (imBin == null || imBin.Handle == IntPtr.Zero) return null;
            IntPtr hSeq = rvTransToContourList(imBin.Handle, chainCoder, filter, minSize);
            return hSeq != IntPtr.Zero ? new KSequence(hSeq, false) : null;
        }
    }


}
