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
using System.IO;

using Kingpool.Utility;
using Kingpool.Core;

namespace Kingpool.Core
{
    // 定义 size_t 别名（UIntPtr 平台自适应）
    using size_t = System.UIntPtr;

    /// <summary>
    /// Mask shape types (corresponds to the 'shape' parameter in the DLL).
    /// </summary>
    public enum MaskShape
    {

        None = 0,
        //area mask
        Rect = 1,
        FilledEllipse = 2,
        FullStrap = 3,
        Ring = 4,

        //line mask
        Line = 5,
        Arc = 6,
        Circle = 7,
        Ellipse = 8,

        HorizontalStrap = 31,
        VerticalStrap = 32,
        BananaQ1 = 41,
        BananaQ2 = 42,
        BananaQ3 = 43,
        BananaQ4 = 44,


    }


    /// <summary>
    /// 表示一个二值模板（Mask），封装了非托管 DLL 中的掩码句柄。
    /// 实现了 IDisposable，以便及时释放非托管资源。
    /// </summary>
    /// <summary>
    /// Represents a binary mask (template) that wraps an unmanaged mask handle.
    /// Inherits from KingsHandle for deterministic resource release.
    /// </summary>
    public class KMask : KingsHandler
    {
        /// <summary>
        /// Mask merge operation types.
        /// </summary>
        public enum MergeType
        {
            NotA = 0x11,
            NotB = 0x12,
            Both = 0x13,
            AnyOf = 0x14,
        }

        // ====================================================================
        // 2. DLL import constants (debug/release)
        // ====================================================================

#if DEBUGGING_KINGPOOL_SUITE
        private const string LIB_NAME = "msk_d.dll";
#else
        private const string LIB_NAME = "msk.dll";
#endif



        // ====================================================================
        // 3. Unmanaged function imports (CharSet.Ansi)
        // ====================================================================

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rvCreateMask(int shape, int width, int height);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rvCreateMaskEx(int shape, int width, int height, int stride);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rvCreateMaskE1(IntPtr pArray, int count);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern void rvDestroyMask(IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern int rmkGetWidth(IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern int rmkGetHeight(IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern int rmkGetPitch(IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern size_t rmkGetSize(IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern void rmkSetData(IntPtr mask, IntPtr pData, size_t size);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rmkGetData(IntPtr mask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern void rmkToggle(IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern void rmkSetZero(IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern void rmkSetOne(IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool rmkResize(IntPtr hMask, int width, int height);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool rmkResizeE1(IntPtr hMask, int width, int height, bool bKeepShape);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool rmkIsEmpty(IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern uint rmkGetArea(IntPtr hMask);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern RvPoint rmkGetAncor(IntPtr hMask);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern void rmkSetAncor(IntPtr hMask, int x, int y);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern RvPoint rmkGetOrigin(IntPtr hMask);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern void rmkSetOrigin(IntPtr hMask, int x, int y);


        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rmkClone(IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rmkMerge(IntPtr mask1, IntPtr mask2, int opType);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool rmkScale(IntPtr hMask, float scale);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern void rmkSetPolygon(IntPtr mask, IntPtr pArray, int count);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rmkDeriveImage(IntPtr hMask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rmkDeriveMask(IntPtr hImage);

        // ====================================================================
        // 4. Constructors
        // ====================================================================

        public KMask(int width, int height)
        {
            if (!Create(MaskShape.Rect, width, height))
                throw new InvalidOperationException("Failed to create KMask.");
        }

        public KMask()
        {
            Create(MaskShape.Rect, 120, 80);
        }

        public KMask(IntPtr hMask, bool bAttached)
        {
            if (hMask == IntPtr.Zero)
                throw new ArgumentException("Invalid handle", nameof(hMask));

            _handle = hMask;
            _attached = bAttached; // external ownership
        }

        public KMask(MaskShape shape, int width, int height)
        {
            if (!Create(shape, width, height))
                throw new InvalidOperationException("Failed to create KMask.");
        }

        public KMask(MaskShape shape, int para1, int para2, int para3)
        {
            if (!Create(shape, para1, para2, para3))
                throw new InvalidOperationException("Failed to create KMask.");
        }

        public KMask(KImage imBin)
        {
            if (imBin == null || imBin.Handle == IntPtr.Zero || imBin.GetPixelFormat() != PixelFormat.Bin)
            {
                if (!Create(MaskShape.Rect, 120, 80))
                    throw new InvalidOperationException("Failed to create default KMask.");
            }
            else
            {
                _handle = rmkDeriveMask(imBin.Handle);
                if (_handle == IntPtr.Zero)
                    throw new InvalidOperationException("Failed to derive mask from image.");
                _attached = false;
            }
        }

        // ====================================================================
        // 5. Sealed ReleaseHandle implementation
        // ====================================================================

        protected sealed override void ReleaseHandle()
        {
            if (_handle != IntPtr.Zero && !_attached)
            {
                rvDestroyMask(_handle);
            }
            // If _attached == true, the handle is owned externally; we do not destroy it.
        }

        // ====================================================================
        // 6. Public methods (all use _handle, and call Destroy() when needed)
        // ====================================================================

        public bool Create(MaskShape shape, int width, int height)
        {
            Destroy(); // release old handle (base method)
            _handle = rvCreateMask((int)shape, width, height);
            _attached = false;
            return _handle != IntPtr.Zero;
        }

        public bool Create(MaskShape shape, int width, int height, int stride)
        {
            Destroy();
            _handle = rvCreateMaskEx((int)shape, width, height, stride);
            _attached = false;
            return _handle != IntPtr.Zero;
        }

        public bool Create(RvPoint[] polygon)
        {
            Destroy();
            if (polygon == null || polygon.Length < 3)
                return false;

            int elementSize = Marshal.SizeOf(typeof(RvPoint));
            IntPtr ptr = Marshal.AllocHGlobal(polygon.Length * elementSize);
            try
            {
                for (int i = 0; i < polygon.Length; i++)
                {
                    IntPtr offset = IntPtr.Add(ptr, i * elementSize);
                    Marshal.StructureToPtr(polygon[i], offset, false);
                }
                _handle = rvCreateMaskE1(ptr, polygon.Length);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            _attached = false;
            return _handle != IntPtr.Zero;
        }

        public bool CreateBanana(MaskShape shape, int radius, int angle, int stride)
        {
            if (shape < MaskShape.BananaQ1 || shape > MaskShape.BananaQ4)
                return false;
            return Create(shape, radius, angle, stride);
        }

        public KMask Clone()
        {
            if (_handle == IntPtr.Zero) return null;
            IntPtr h = rmkClone(_handle);
            return h != IntPtr.Zero ? new KMask(h, false) : null;
        }

        public void CopyTo(KMask twin)
        {
            if (_handle == IntPtr.Zero) return;
            if (twin == null) throw new ArgumentNullException(nameof(twin));
            twin.Destroy(); // release twin's old handle
            twin._handle = rmkClone(_handle);
            twin._attached = false;
        }

        public size_t GetSize()
        {
            return _handle != IntPtr.Zero ? rmkGetSize(_handle) : size_t.Zero;
        }

        public bool Resize(int width, int height)
        {
            return _handle != IntPtr.Zero && rmkResize(_handle, width, height);
        }

        public int GetWidth() => _handle != IntPtr.Zero ? rmkGetWidth(_handle) : 0;
        public int GetHeight() => _handle != IntPtr.Zero ? rmkGetHeight(_handle) : 0;
        public int GetPitch() => _handle != IntPtr.Zero ? rmkGetPitch(_handle) : 0;

        public KImage Derive()
        {
            if (_handle == IntPtr.Zero) return null;
            IntPtr p = rmkDeriveImage(_handle);
            return p != IntPtr.Zero ? new KImage(p, false) : null;
        }

        public void Reshape(KImage image)
        {
            if (image == null || image.Handle == IntPtr.Zero || image.GetPixelFormat() != PixelFormat.Bin)
                throw new ArgumentException("Invalid binary image.");
            Destroy();
            _handle = rmkDeriveMask(image.Handle);
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to derive mask.");
            _attached = false;
        }

        public void Reshape(RvPoint[] vertexArray)
        {
            if (vertexArray == null || vertexArray.Length < 3)
                throw new ArgumentException("Need at least 3 vertices.");
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("Mask not created.");

            int size = Marshal.SizeOf(typeof(RvPoint));
            IntPtr pArray = Marshal.AllocHGlobal(size * vertexArray.Length);
            try
            {
                for (int i = 0; i < vertexArray.Length; i++)
                {
                    IntPtr pEle = IntPtr.Add(pArray, i * size);
                    Marshal.StructureToPtr(vertexArray[i], pEle, false);
                }
                rmkSetPolygon(_handle, pArray, vertexArray.Length);
            }
            finally
            {
                Marshal.FreeHGlobal(pArray);
            }
        }

        public void Reshape(RvPointF32[] vertexArray)
        {
            if (vertexArray == null || vertexArray.Length < 3)
                throw new ArgumentException("Need at least 3 vertices.");
            var pts = new RvPoint[vertexArray.Length];
            for (int i = 0; i < vertexArray.Length; i++)
            {
                pts[i].x = (int)vertexArray[i].x;
                pts[i].y = (int)vertexArray[i].y;
            }
            Reshape(pts);
        }

        public void Reshape(RvPointF64[] vertexArray)
        {
            if (vertexArray == null || vertexArray.Length < 3)
                throw new ArgumentException("Need at least 3 vertices.");
            var pts = new RvPoint[vertexArray.Length];
            for (int i = 0; i < vertexArray.Length; i++)
            {
                pts[i].x = (int)vertexArray[i].x;
                pts[i].y = (int)vertexArray[i].y;
            }
            Reshape(pts);
        }

        public size_t GetArea() => _handle != IntPtr.Zero ? (size_t)rmkGetArea(_handle) : size_t.Zero;

        public void SetData(byte[] data)
        {
            if (data == null || data.Length == 0) return;
            if (_handle == IntPtr.Zero) throw new InvalidOperationException("Mask not created.");
            IntPtr ptr = Marshal.AllocHGlobal(data.Length);
            try
            {
                Marshal.Copy(data, 0, ptr, data.Length);
                rmkSetData(_handle, ptr, (size_t)data.Length);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public byte[] GetData()
        {
            if (_handle == IntPtr.Zero) return null;
            IntPtr p = rmkGetData(_handle);
            if (p == IntPtr.Zero) return null;
            size_t sz = rmkGetSize(_handle);
            uint len = sz.ToUInt32();
            byte[] arr = new byte[len];
            Marshal.Copy(p, arr, 0, (int)len);
            return arr;
        }

        public void Toggle() { if (_handle != IntPtr.Zero) rmkToggle(_handle); }
        public void SetZero() { if (_handle != IntPtr.Zero) rmkSetZero(_handle); }
        public void SetOne() { if (_handle != IntPtr.Zero) rmkSetOne(_handle); }

        public void Resize(int width, int height, bool bKeepShape)
        {
            if (_handle != IntPtr.Zero)
                rmkResizeE1(_handle, width, height, bKeepShape);
        }

        public bool IsEmpty() => _handle != IntPtr.Zero && rmkIsEmpty(_handle);

        public RvPoint GetAnchor()
        {
            return _handle != IntPtr.Zero ? rmkGetAncor(_handle) : new RvPoint();
        }

        public void SetAncor(int x, int y)
        {
            if (_handle != IntPtr.Zero)
                rmkSetAncor(_handle, x, y);
        }

        public RvPoint GetOrigin()
        {
            return _handle != IntPtr.Zero ? rmkGetOrigin(_handle) : new RvPoint();
        }

        public void SetOrigin(int x, int y)
        {
            if (_handle != IntPtr.Zero)
                rmkSetOrigin(_handle, x, y);
        }

        public static KMask Merge(KMask mask1, KMask mask2, int opType)
        {
            if (mask1 == null || mask2 == null) return null;
            IntPtr p = rmkMerge(mask1.Handle, mask2.Handle, opType);
            return p != IntPtr.Zero ? new KMask(p, false) : null;
        }

        public bool Scale(float scale)
        {
            return _handle != IntPtr.Zero && rmkScale(_handle, scale);
        }
    }


}
