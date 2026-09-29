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

namespace Kingpool.Core
{
    // 定义 size_t 别名（平台自适应）
    using size_t = System.UIntPtr;
    using RvColor = System.UInt32;

    public enum PixelFormat
    {
        Unknown = -1,
        Bin = 1,
        Gray = 8,
        BGR = 24,
        RGB,
        BGRA = 32,
        RGBA,
        HSV = 40,
        HSV_H,
        HSV_S,
        HSV_V,
        YUV = 45,
        YUV_Y,
        YUV_U,
        YUV_V,
        YCBCR = 50,
        YCBCR_Y,
        YCBCR_CB,
        YCBCR_CR,
    }

    public enum RgbToGray
    {
        Default = 0, // Gray = R*0.299 + G*0.587 + B*0.114
        ValueOfHSV = 1,     // Gray = value of HSV
        MaxOfRgb = 2,     // MAX(R,G,B)
        MinOfRgb = 3,     // MIN(R,G,B)
        AvgOfRgb = 4,     // Gray =( R + G + B )/3
    }

    public struct RvHsv
    {
        public float Hue;        //0-360
        public float Saturation; //0-1
        public float Value;      //0-1
    }

    public struct RvYCbCr
    {
        public float Y;   //0-255
        public float Cb;  //0-255
        public float Cr;  //0-255
    }

    public struct RvYuv
    {
        public float Y;
        public float U;
        public float V;
    }

    /// <summary>
    /// 表示一个图像对象，封装了非托管 DLL 中的图像句柄。
    /// 实现了 IDisposable 以便及时释放非托管资源。
    /// </summary>
    public class KImage : KingsHandler
    {
        // ====================================================================
        // 常量定义（全部保留）
        // ====================================================================
        public const int BIN_ONE = 0;
        public const int BIN_ZERO = 255;

        public const string IFF_DEFAULT = "bmp";
        public const string IFF_ICO = "ico";
        public const string IFF_JPG = "jpg";
        public const string IFF_JPEG = "jpeg";
        public const string IFF_JNG = "jng";
        public const string IFF_KOALA = "koa";
        public const string IFF_LBM = "lbm";
        public const string IFF_IFF = "iff";
        public const string IFF_MNG = "mng";
        public const string IFF_PBM = "pbm";
        public const string IFF_PBMRAW = "pbm";
        public const string IFF_PCD = "pcd";
        public const string IFF_PCX = "pcx";
        public const string IFF_PGM = "pgm";
        public const string IFF_PGMRAW = "pgm";
        public const string IFF_PNG = "png";
        public const string IFF_PPM = "ppm";
        public const string IFF_PPMRAW = "ppm";
        public const string IFF_RAS = "ras";
        public const string IFF_TARGA = "targa";
        public const string IFF_TGA = "tga";
        public const string IFF_TIFF = "tiff";
        public const string IFF_TIF = "tif";
        public const string IFF_WBMP = "wbmp";
        public const string IFF_PSD = "psd";
        public const string IFF_CUT = "cut";
        public const string IFF_XBM = "xbm";
        public const string IFF_XPM = "xpm";
        public const string IFF_DDS = "dds";
        public const string IFF_GIF = "gif";
        public const string IFF_HDR = "hdr";
        public const string IFF_FAXG3 = "faxg3";
        public const string IFF_SGI = "sgi";
        public const string IFF_EXR = "exr";
        public const string IFF_J2K = "j2k";
        public const string IFF_JP2 = "jp2";
        public const string IFF_PFM = "pfm";
        public const string IFF_PICT = "pict";
        public const string IFF_RAW = "raw";

        public const int IMF_DEFAULT = -1;
        public const int IMF_BMP = 0;
        public const int IMF_ICO = 1;
        public const int IMF_JPEG = 2;
        public const int IMF_JPG = IMF_JPEG;
        public const int IMF_JNG = 3;
        public const int IMF_KOALA = 4;
        public const int IMF_LBM = 5;
        public const int IMF_IFF = IMF_LBM;
        public const int IMF_MNG = 6;
        public const int IMF_PBM = 7;
        public const int IMF_PBMRAW = 8;
        public const int IMF_PCD = 9;
        public const int IMF_PCX = 10;
        public const int IMF_PGM = 11;
        public const int IMF_PGMRAW = 12;
        public const int IMF_PNG = 13;
        public const int IMF_PPM = 14;
        public const int IMF_PPMRAW = 15;
        public const int IMF_RAS = 16;
        public const int IMF_TARGA = 17;
        public const int IMF_TGA = IMF_TARGA;
        public const int IMF_TIFF = 18;
        public const int IMF_TIF = IMF_TIFF;
        public const int IMF_WBMP = 19;
        public const int IMF_PSD = 20;
        public const int IMF_CUT = 21;
        public const int IMF_XBM = 22;
        public const int IMF_XPM = 23;
        public const int IMF_DDS = 24;
        public const int IMF_GIF = 25;
        public const int IMF_HDR = 26;
        public const int IMF_FAXG3 = 27;
        public const int IMF_SGI = 28;
        public const int IMF_EXR = 29;
        public const int IMF_J2K = 30;
        public const int IMF_JP2 = 31;
        public const int IMF_PFM = 32;
        public const int IMF_PICT = 33;
        public const int IMF_RAW = 34;

        //floodE2, FloodE3支持的数据类型
        public const int RDT_BYTE = 1;
        public const int RDT_INT = 4;
        public const int RDT_FLOAT = 5;
        public const int RDT_DOUBLE = 6;

        // ====================================================================
        // DLL 导入（统一 CharSet.Ansi）
        // ====================================================================

#if DEBUGGING_KINGPOOL_SUITE
        private const string IMG_LIB_NAME = "img_d.dll";
        private const string IMIO_LIB_NAME = "imio_d.dll";
#else
        private const string IMG_LIB_NAME = "img.dll";
        private const string IMIO_LIB_NAME = "imio.dll";
#endif

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        public static extern IntPtr rvCreateImage(int type, int width, int height);

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rvCreateImageEx(PixelFormat pixelFormat, int width, int height, bool bDummy, IntPtr pDataSource, size_t nDataSize);

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        public static extern void rvDestroyImage(IntPtr image);

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern void rviFlood(IntPtr image, byte level);
        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        public static extern void rviFloodE1(IntPtr image, RvColor color);
        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern void rviFloodE2(IntPtr image, int dataType, IntPtr pAnyData, size_t nDataSize);
        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern void rviFloodE3(IntPtr image, int dataType, IntPtr pAnyData, size_t nDataSize, double minValue, double maxValue);

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern void rviFloodEx(IntPtr image, IntPtr pImageData, int nDataSize);

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool rviCast(IntPtr image, int type);

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rviClone(IntPtr src, IntPtr dest);

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        public static extern bool rviSetSize(IntPtr image, int width, int height);

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern size_t rviGetSize(IntPtr image);

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern PixelFormat rviGetPixelFormat(IntPtr image);

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rviGetData(IntPtr image);

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern int rviGetPitch(IntPtr image);

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern int rviGetWidth(IntPtr image);

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern int rviGetHeight(IntPtr image);

        [DllImport(IMIO_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rvLoadImage(byte[] strFileName, byte[] strFormat, int flag = 0);

        [DllImport(IMIO_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool rvSaveImage(IntPtr image, byte[] strFileName, byte[] strFormat, int flag = 0);

        [DllImport(IMIO_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern uint rvExportImageToMemory(IntPtr image, int nFormat, IntPtr pBuffer, uint size, int flag = 0);

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rviGetRedChannel(IntPtr src, IntPtr dest);

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rviGetGreenChannel(IntPtr src, IntPtr dest);

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rviGetBlueChannel(IntPtr src, IntPtr dest);

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rviGetAlphaChannel(IntPtr src, IntPtr dest);

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr rviMergeRGB(IntPtr red, IntPtr green, IntPtr blue, IntPtr dest);

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool rviCvt24To8(IntPtr image, RgbToGray method);

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern int rviBytesPerPixel(IntPtr image);

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern int rviGetDepth(IntPtr image);

        [DllImport(IMG_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern int rviGetChannels(IntPtr image);

        [DllImport(IMIO_LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool rvLoadImageInMemoryEx(IntPtr image, IntPtr pData, uint size, int nFormat, int flag);

        // ====================================================================
        // 构造函数
        // ====================================================================

        public KImage()
        {
            Create(PixelFormat.BGR, 120, 80);
        }

        public KImage(PixelFormat pixfmt, int width, int height)
        {
            Create(pixfmt, width, height);
        }

        public KImage(PixelFormat pixfmt)
        {
            Create(pixfmt, 120, 80);
        }

        public KImage(IntPtr hImage, bool bAttach = true)
        {
            _handle = hImage;
            _attached = bAttach;
        }

        public KImage(string strFilePath)
        {
            Create(PixelFormat.BGR, 120, 80);
            if (_handle != IntPtr.Zero)
            {
                Load(strFilePath);
            }
        }

        // ====================================================================
        // 实现 ReleaseHandle（密封，禁止子类篡改）
        // ====================================================================

        protected sealed override void ReleaseHandle()
        {
            if (!_attached && _handle != IntPtr.Zero)
            {
                rvDestroyImage(_handle);
            }
            // 如果 _attached == true，则不释放（外部拥有）
        }

        // ====================================================================
        // 公共方法（全部保留，仅将 m_hImage 替换为 _handle）
        // ====================================================================


        public bool Create(PixelFormat pixfmt, int width, int height)
        {
            Destroy(); // 释放旧句柄（基类方法）
            _handle = rvCreateImage((int)pixfmt, width, height);
            _attached = false;
            return _handle != IntPtr.Zero;
        }

        public void Flood(byte level)
        {
            if (_handle != IntPtr.Zero)
                rviFlood(_handle, level);
        }

        public void Flood(RvColor color)
        {
            if (_handle != IntPtr.Zero)
                rviFloodE1(_handle, color);
        }

        // ====================================================================
        // Public Flood wrappers for rviFloodE2 (no min/max range)
        // ====================================================================

        /// <summary>
        /// Floods the image with the given byte array using default range.
        /// </summary>
        public void Flood(byte[] data)
        {
            if (data == null || data.Length == 0 || _handle == IntPtr.Zero) return;

            int size = data.Length * sizeof(byte);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(data, 0, ptr, data.Length);
                rviFloodE2(_handle, RDT_BYTE, ptr, (size_t)(uint)size);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        /// <summary>
        /// Floods the image with the given int array using default range.
        /// </summary>
        public void Flood(int[] data)
        {
            if (data == null || data.Length == 0 || _handle == IntPtr.Zero) return;

            int size = data.Length * sizeof(int);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(data, 0, ptr, data.Length);
                rviFloodE2(_handle, RDT_INT, ptr, (size_t)(uint)size);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        /// <summary>
        /// Floods the image with the given float array using default range.
        /// </summary>
        public void Flood(float[] data)
        {
            if (data == null || data.Length == 0 || _handle == IntPtr.Zero) return;

            int size = data.Length * sizeof(float);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(data, 0, ptr, data.Length);
                rviFloodE2(_handle, RDT_FLOAT, ptr, (size_t)(uint)size);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        /// <summary>
        /// Floods the image with the given double array using default range.
        /// </summary>
        public void Flood(double[] data)
        {
            if (data == null || data.Length == 0 || _handle == IntPtr.Zero) return;

            int size = data.Length * sizeof(double);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(data, 0, ptr, data.Length);
                rviFloodE2(_handle, RDT_DOUBLE, ptr, (size_t)(uint)size);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        // ====================================================================
        // Public Flood wrappers for rviFloodE3 (with min/max range)
        // ====================================================================

        /// <summary>
        /// Floods the image with the given byte array within the range [minValue, maxValue].
        /// </summary>
        public void Flood(byte[] data, double minValue, double maxValue)
        {
            if (data == null || data.Length == 0 || _handle == IntPtr.Zero) return;

            int size = data.Length * sizeof(byte);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(data, 0, ptr, data.Length);
                rviFloodE3(_handle, RDT_BYTE, ptr, (size_t)(uint)size, minValue, maxValue);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        /// <summary>
        /// Floods the image with the given int array within the range [minValue, maxValue].
        /// </summary>
        public void Flood(int[] data, double minValue, double maxValue)
        {
            if (data == null || data.Length == 0 || _handle == IntPtr.Zero) return;

            int size = data.Length * sizeof(int);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(data, 0, ptr, data.Length);
                rviFloodE3(_handle, RDT_INT, ptr, (size_t)(uint)size, minValue, maxValue);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        /// <summary>
        /// Floods the image with the given float array within the range [minValue, maxValue].
        /// </summary>
        public void Flood(float[] data, double minValue, double maxValue)
        {
            if (data == null || data.Length == 0 || _handle == IntPtr.Zero) return;

            int size = data.Length * sizeof(float);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(data, 0, ptr, data.Length);
                rviFloodE3(_handle, RDT_FLOAT, ptr, (size_t)(uint)size, minValue, maxValue);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        /// <summary>
        /// Floods the image with the given double array within the range [minValue, maxValue].
        /// </summary>
        public void Flood(double[] data, double minValue, double maxValue)
        {
            if (data == null || data.Length == 0 || _handle == IntPtr.Zero) return;

            int size = data.Length * sizeof(double);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(data, 0, ptr, data.Length);
                rviFloodE3(_handle, RDT_DOUBLE, ptr, (size_t)(uint)size, minValue, maxValue);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
        //public void FloodEx(IntPtr data, int size)
        //{
        //    if (_handle != IntPtr.Zero)
        //        rviFloodEx(_handle, data, size);
        //}

        public void FloodEx(byte[] data)
        {
            if (data == null || data.Length == 0 || _handle == IntPtr.Zero)
                return;
            IntPtr ptr = Marshal.AllocHGlobal(data.Length);
            try
            {
                Marshal.Copy(data, 0, ptr, data.Length);
                rviFloodEx(_handle, ptr, data.Length);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public void SetSize(int width, int height)
        {
            if (_handle != IntPtr.Zero)
                rviSetSize(_handle, width, height);
        }

        public size_t GetSize()
        {
            return _handle != IntPtr.Zero ? rviGetSize(_handle) : size_t.Zero;
        }

        public PixelFormat GetPixelFormat()
        {
            return _handle != IntPtr.Zero ? rviGetPixelFormat(_handle) : 0;
        }

        public IntPtr GetData()
        {
            return _handle != IntPtr.Zero ? rviGetData(_handle) : IntPtr.Zero;
        }

        public int GetPitch()
        {
            return _handle != IntPtr.Zero ? rviGetPitch(_handle) : 0;
        }

        public int GetWidth()
        {
            return _handle != IntPtr.Zero ? rviGetWidth(_handle) : 0;
        }

        public int GetHeight()
        {
            return _handle != IntPtr.Zero ? rviGetHeight(_handle) : 0;
        }

        public uint ExportImageToMemory(int nFormat, int flag, ref byte[] data)
        {
            if (_handle == IntPtr.Zero || data == null || data.Length == 0)
                return 0;
            IntPtr h = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);
            return rvExportImageToMemory(_handle, nFormat, h, (uint)data.Length, flag);
        }

        public bool LoadEx(string strFileName, string strFormat, int flag = 0)
        {
            Destroy();
            byte[] utf8Path, utf8Format;
            if (CAP.CharSet == CharEncoding.Utf8)
            {
                utf8Path = Encoding.UTF8.GetBytes(strFileName + "\0");
                utf8Format = Encoding.UTF8.GetBytes(strFormat + "\0");
            }
            else
            {
                utf8Path = Encoding.Default.GetBytes(strFileName + "\0");
                utf8Format = Encoding.Default.GetBytes(strFormat + "\0");
            }
            _handle = rvLoadImage(utf8Path, utf8Format, flag);
            _attached = false;
            return _handle != IntPtr.Zero;
        }

        public bool Load(string strFileName)
        {
            Destroy();
            int pos = strFileName.LastIndexOf(".");
            string strExt = pos >= 0 ? strFileName.Substring(pos + 1) : "";
            byte[] utf8Path = Pool.StringToCharBytes(strFileName);
            byte[] utf8Format = Pool.StringToCharBytes(strExt);
            _handle = rvLoadImage(utf8Path, utf8Format, 0);
            _attached = false;
            return _handle != IntPtr.Zero;
        }

        public bool Save(string strFileName, string strFormat = IFF_DEFAULT, int flag = 0)
        {
            if (string.IsNullOrEmpty(strFileName) || _handle == IntPtr.Zero)
                return false;
            byte[] utf8Path, utf8Format;
            if (CAP.CharSet == CharEncoding.Utf8)
            {
                utf8Path = Encoding.UTF8.GetBytes(strFileName + "\0");
                utf8Format = Encoding.UTF8.GetBytes(strFormat + "\0");
            }
            else
            {
                utf8Path = Encoding.Default.GetBytes(strFileName + "\0");
                utf8Format = Encoding.Default.GetBytes(strFormat + "\0");
            }
            return rvSaveImage(_handle, utf8Path, utf8Format, flag);
        }

        public KImage Clone(bool bDummy)
        {
            if (_handle == IntPtr.Zero) return null;
            IntPtr p = rvCreateImageEx((PixelFormat)rviGetPixelFormat(_handle),
                                       rviGetWidth(_handle), rviGetHeight(_handle),
                                       true, rviGetData(_handle), rviGetSize(_handle));
            return p != IntPtr.Zero ? new KImage(p, false) : null;
        }

        public KImage Clone()
        {
            if (_handle == IntPtr.Zero) return null;
            IntPtr p = rviClone(_handle, IntPtr.Zero);
            return p != IntPtr.Zero ? new KImage(p, false) : null;
        }

        public bool Clone(out IntPtr hImage)
        {
            hImage = IntPtr.Zero;
            if (_handle == IntPtr.Zero) return false;
            IntPtr p = rviClone(_handle, IntPtr.Zero);
            if (p != IntPtr.Zero)
            {
                hImage = p;
                return true;
            }
            return false;
        }

        public static void Copy(IntPtr src, IntPtr dest)
        {
            if (src != IntPtr.Zero && dest != IntPtr.Zero)
                rviClone(src, dest);
        }

        public void CopyFrom(IntPtr hImage)
        {
            if (_handle != IntPtr.Zero && hImage != IntPtr.Zero)
                rviClone(hImage, _handle);
        }

        public void Cast(PixelFormat pixelFormat)
        {
            if (_handle != IntPtr.Zero)
                rviCast(_handle, (int)pixelFormat);
        }

        public static void Destroy(ref IntPtr hImage)
        {
            if (hImage != IntPtr.Zero)
            {
                rvDestroyImage(hImage);
                hImage = IntPtr.Zero;
            }
        }

        public void Convert24To8(RgbToGray method)
        {
            if (_handle != IntPtr.Zero)
                rviCvt24To8(_handle, method);
        }

        public void Split(KImage red, KImage green, KImage blue)
        {
            if (_handle == IntPtr.Zero) return;
            PixelFormat type = rviGetPixelFormat(_handle);
            if (!(type == PixelFormat.BGR || type == PixelFormat.BGRA)) return;

            if (red != null && red._handle != IntPtr.Zero)
                rviGetRedChannel(_handle, red._handle);
            if (green != null && green._handle != IntPtr.Zero)
                rviGetGreenChannel(_handle, green._handle);
            if (blue != null && blue._handle != IntPtr.Zero)
                rviGetBlueChannel(_handle, blue._handle);
        }

        public void Split(KImage red, KImage green, KImage blue, KImage alpha)
        {
            if (_handle == IntPtr.Zero) return;
            PixelFormat type = rviGetPixelFormat(_handle);
            if (type != PixelFormat.BGRA) return;

            if (red != null && red._handle != IntPtr.Zero)
                rviGetRedChannel(_handle, red._handle);
            if (green != null && green._handle != IntPtr.Zero)
                rviGetGreenChannel(_handle, green._handle);
            if (blue != null && blue._handle != IntPtr.Zero)
                rviGetBlueChannel(_handle, blue._handle);
            if (alpha != null && alpha._handle != IntPtr.Zero)
                rviGetAlphaChannel(_handle, alpha._handle);
        }

        public void Merge(KImage red, KImage green, KImage blue)
        {
            if (_handle != IntPtr.Zero && red != null && green != null && blue != null)
                rviMergeRGB(red._handle, green._handle, blue._handle, _handle);
        }

        public int GetBytesPerPixel()
        {
            return _handle != IntPtr.Zero ? rviBytesPerPixel(_handle) : 1;
        }

        public int GetDepth()
        {
            return _handle != IntPtr.Zero ? rviGetDepth(_handle) : 0;
        }

        public int GetChannels()
        {
            return _handle != IntPtr.Zero ? rviGetChannels(_handle) : 1;
        }

        public bool LoadImageInMemory(byte[] data)
        {
            if (_handle == IntPtr.Zero || data == null || data.Length == 0)
                return false;
            IntPtr ptr = Marshal.AllocHGlobal(data.Length);
            try
            {
                Marshal.Copy(data, 0, ptr, data.Length);
                return rvLoadImageInMemoryEx(_handle, ptr, (uint)data.Length, IMF_DEFAULT, 0);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
