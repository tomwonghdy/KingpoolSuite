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
using System.Diagnostics;
using System.Threading;

using Kingpool.Utility;

namespace Kingpool.Core
{

    using size_t = System.UInt64;

    public enum CharEncoding
    {
        Utf8 = 0,
        local = 1,
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct RvSizeF32
    {
        public float sx, sy;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct RvSizeF64
    {
        public double sx, sy;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RvOffset
    {
        public int dx, dy;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct RvOffsetF32
    {
        public float dx, dy;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct RvOffsetF64
    {
        public double dx, dy;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct RvRect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
        public RvRect(int left, int top, int right, int bottom)
        {
            this.left = left; this.top = top; this.right = right; this.bottom = bottom;
        }
        public RvRect(RvPoint leftTop, RvPoint rightBottom)
        {
            this.left = leftTop.x; this.top = leftTop.y;
            this.right = rightBottom.x; this.bottom = rightBottom.y;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", left, top, right, bottom);
        }
        public int Width()
        {
            return right - left;
        }
        public int Height()
        {
            return bottom - top;
        }
        public void Offset(int dx, int dy)
        {
            left += dx; right += dx;
            top += dy; bottom += dy;
        }
        public void Offset(double dx, double dy)
        {
            left += (int)dx; right += (int)dx;
            top += (int)dy; bottom += (int)dy;
        }
        public bool IsInside(int x, int y)
        {
            return (x >= left && x <= right && y >= top && y <= bottom);
        }
        public RvPoint Center()
        {
            return new RvPoint((left + right) / 2, (top + bottom) / 2);
        }
        //按照图像中心计算方法计算
        public void Center(out RvPointF32 pt)
        {
            pt.x = left + (Width() - 1) * 0.5f;
            pt.y = top + (Height() - 1) * 0.5f;
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RvRectF32
    {
        public float left;
        public float top;
        public float right;
        public float bottom;
        public RvRectF32(float l, float t, float r, float b)
        {
            left = l; top = t; right = r; bottom = b;
        }
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", left, top, right, bottom);
        }
        public float Width()
        {
            return right - left;
        }
        public float Height()
        {
            return bottom - top;
        }
        public void Offset(float dx, float dy)
        {
            left += dx; right += dx;
            top += dy; bottom += dy;
        }
        public void Offset(double dx, double dy)
        {
            left += (float)dx; right += (float)dx;
            top += (float)dy; bottom += (float)dy;
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RvRectF64
    {
        public double left;
        public double top;
        public double right;
        public double bottom;
        public RvRectF64(float l, float t, float r, float b)
        {
            left = l; top = t; right = r; bottom = b;
        }
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", left, top, right, bottom);
        }
        public double Width()
        {
            return right - left;
        }
        public double Height()
        {
            return bottom - top;
        }
        public void Offset(double dx, double dy)
        {
            left += dx; right += dx;
            top += dy; bottom += dy;
        }
        public void Offset(float dx, float dy)
        {
            left += (double)dx; right += (double)dx;
            top += (double)dy; bottom += (double)dy;
        }

    }
    [StructLayout(LayoutKind.Sequential)]
    public struct RvBox2D
    {
        public double cx, cy;            /* Center of the box.                          */
        public double width, height;     /* Box width and length.                       */
        public double angle;             /* Angle between the horizontal axis and the first side (i.e. length) in degrees */

        public RvBox2D(double cx, double cy, double width, double height, double angle)
        {
            this.cx = cx;
            this.cy = cy;
            this.width = width;
            this.height = height;
            this.angle = angle;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RvRgba
    {
        public byte red;
        public byte green;
        public byte blue;
        public byte alpha;

        public RvRgba Invert()
        {
            return new RvRgba((byte)(255 - red), (byte)(255 - green), (byte)(255 - blue), alpha);
        }
        public RvRgba(byte r, byte g, byte b, byte a)
        {
            red = r;
            green = g;
            blue = b;
            alpha = a;
        }
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", red, green, blue, alpha);
        }

        public UInt32 ToColor()
        {
            return (UInt32)((red << 16) | (green << 8) | blue | (alpha << 24));
        }
        public byte ToGray()
        {
            return (byte)((red) * 0.299f + (green) * 0.587f + (blue) * 0.114f);
        }
    }

    public struct RvRgb
    {
        public byte red;
        public byte green;
        public byte blue;
        public RvRgb(Byte r, byte g, byte b)
        {
            red = r;
            green = g;
            blue = b;
        }

        public RvRgb Invert()
        {
            return new RvRgb((byte)(255 - red), (byte)(255 - green), (byte)(255 - blue));
        }
        public override string ToString()
        {
            return string.Format("{0},{1},{2}", red, green, blue);
        }
        public UInt32 ToColor()
        {
            return (UInt32)((red << 16) | (green << 8) | blue | (255 << 24));
        }
        public byte ToGray()
        {
            return (byte)((red) * 0.299f + (green) * 0.587f + (blue) * 0.114f);
        }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct RvPoint
    {
        public int x;
        public int y;

        public RvPoint(int _x, int _y)
        {
            x = _x; y = _y;
        }

        public RvPoint(string s)
        {
            x = 0; y = 0;
            if (s != null)
            {
                int pos = s.IndexOf(',');
                if (pos > 0)
                {
                    x = int.Parse(s.Substring(0, pos));
                    y = int.Parse(s.Substring(pos + 1, s.Length - (pos + 1)));
                }
            }
        }
        public double Dist(int _x, int _y)
        {
            return Math.Sqrt((x - _x) * (x - _x) + (y - _y) * (y - _y));
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", x, y);
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct RvPointF32
    {
        public float x, y;

        public RvPointF32(float _x, float _y)
        {
            x = _x; y = _y;
        }
        public RvPointF32(string s)
        {
            x = 0; y = 0;
            if (s != null)
            {
                int pos = s.IndexOf(',');
                if (pos > 0)
                {
                    x = float.Parse(s.Substring(0, pos));
                    y = float.Parse(s.Substring(pos + 1, s.Length - (pos + 1)));
                }
            }
        }
        public double Dist(float _x, float _y)
        {
            return Math.Sqrt((x - _x) * (x - _x) + (y - _y) * (y - _y));
        }
        public override string ToString()
        {
            return string.Format("{0},{1}", x, y);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RvPointF64
    {
        public double x, y;

        public RvPointF64(double _x, double _y)
        {
            x = _x; y = _y;
        }
        public RvPointF64(string s)
        {
            x = 0; y = 0;
            if (s != null)
            {
                int pos = s.IndexOf(',');
                if (pos > 0)
                {
                    x = double.Parse(s.Substring(0, pos));
                    y = double.Parse(s.Substring(pos + 1, s.Length - (pos + 1)));
                }
            }
        }

        public double Dist(double _x, double _y)
        {
            return Math.Sqrt((x - _x) * (x - _x) + (y - _y) * (y - _y));
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", x, y);
        }
    }



    [StructLayout(LayoutKind.Sequential)]
    public struct RvSize
    {
        public int sx;
        public int sy;

        public RvSize(int _sx, int _sy)
        {
            sx = _sx; sy = _sy;
        }
        public RvSize(string s)
        {
            sx = 0; sy = 0;
            if (s != null)
            {
                int pos = s.IndexOf(',');
                if (pos > 0)
                {
                    sx = int.Parse(s.Substring(0, pos));
                    sy = int.Parse(s.Substring(pos + 1, s.Length - (pos + 1)));
                }
            }
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", sx, sy);
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RvLine
    {
        public RvPoint p0, p1;

        public RvLine(RvPoint _p0, RvPoint _p1)
        {
            p0 = _p0; p1 = _p1;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", p0.x, p0.y, p1.x, p1.y);
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RvLineF32
    {
        public RvPointF32 p0, p1;
        public RvLineF32(RvPointF32 _p0, RvPointF32 _p1)
        {
            p0 = _p0; p1 = _p1;
        }

        public RvLineF32(RvPoint _p0, RvPoint _p1)
        {
            p0.x = _p0.x; p0.y = _p0.y;
            p1.x = _p1.x; p1.y = _p1.y;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", p0.x, p0.y, p1.x, p1.y);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RvLineF64
    {
        public RvPointF64 p0, p1;
        public RvLineF64(RvPointF64 _p0, RvPointF64 _p1)
        {
            p0 = _p0; p1 = _p1;
        }

        public RvLineF64(RvPointF32 _p0, RvPointF32 _p1)
        {
            p0.x = _p0.x; p0.y = _p0.y;
            p1.x = _p1.x; p1.y = _p1.y;
        }

        public RvLineF64(RvPoint _p0, RvPoint _p1)
        {
            p0.x = _p0.x; p0.y = _p0.y;
            p1.x = _p1.x; p1.y = _p1.y;
        }

        public void Move(double dx, double dy)
        {
            p0.x += dx; p0.y += dy;
            p1.x += dx; p1.y += dy;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", p0.x, p0.y, p1.x, p1.y);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RvScalarF64
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] val;
        public RvScalarF64(double n)
        {
            val = new double[4];
            val[0] = n;
        }
        public RvScalarF64(double n1, double n2)
        {
            val = new double[4];
            val[0] = n1; val[1] = n2;
        }
        public RvScalarF64(double n1, double n2, double n3)
        {
            val = new double[4];
            val[0] = n1; val[1] = n2; val[2] = n3;
        }
        public RvScalarF64(double n1, double n2, double n3, double n4)
        {
            val = new double[4];
            val[0] = n1; val[1] = n2; val[2] = n3; val[3] = n4;
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RvScalarF32
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] val;
        public RvScalarF32(float n)
        {
            val = new float[4];
            val[0] = n;
        }
        public RvScalarF32(float n1, float n2)
        {
            val = new float[4];
            val[0] = n1; val[1] = n2;
        }
        public RvScalarF32(float n1, float n2, float n3)
        {
            val = new float[4];
            val[0] = n1; val[1] = n2; val[2] = n3;
        }
        public RvScalarF32(float n1, float n2, float n3, float n4)
        {
            val = new float[4];
            val[0] = n1; val[1] = n2; val[2] = n3; val[3] = n4;
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RvScalar
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public int[] val;
        public RvScalar(int n)
        {
            val = new int[4];
            val[0] = n;
        }
        public RvScalar(int n1, int n2)
        {
            val = new int[4];
            val[0] = n1; val[1] = n2;
        }
        public RvScalar(int n1, int n2, int n3)
        {
            val = new int[4];
            val[0] = n1; val[1] = n2; val[2] = n3;
        }
        public RvScalar(int n1, int n2, int n3, int n4)
        {
            val = new int[4];
            val[0] = n1; val[1] = n2; val[2] = n3; val[3] = n4;
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct RvEllipticArcF32
    {
        public RvPointF32 center;
        public float majorAxis;
        public float minorAxis;
        public float startAngle;
        public float endAngle;
    }
    public struct RvCircularArcF32
    {
        public RvPointF32 center;
        public float radius;
        public float startAngle;
        public float endAngle;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct RvEllipticArcF64
    {
        public RvPointF64 center;
        public double majorAxis;
        public double minorAxis;
        public double startAngle;
        public double endAngle;
    }
    public struct RvCircularArcF64
    {
        public RvPointF64 center;
        public double radius;
        public double startAngle;
        public double endAngle;
    }

    //三态布尔
    public enum RvBool
    {
        False = 0,
        True = 1,
        Fuzzy = -1,
    };

    public enum RvDirection
    {
        Unknown = 0,
        Horizontal = 1,
        Vertical = (1 << 1),
        Both = 1 | (1 << 1),
    };


    public static class CAP
    {
        //软件版本
        public const int MAIN_VERSION = 2;
        public const int SUB_VERSION = 5;
        public const int MINOR_VERSION = 904;

        public const int MAX_DESC_TEXT_LEN = 63 + 1;

        //镜头畸变校准
        public const int LR_RVEC_SIZE = (3 * 1);
        public const int LR_TVEC_SIZE = (3 * 1);

#if DEBUGGING_KINGPOOL_SUITE
        private const string LIB_NAME = "eeh_d.dll";
#else
        private const string LIB_NAME = "eeh.dll";
#endif

        [DllImport(LIB_NAME)]
        private static extern void rvSetStringEncoding(int encoding);


        public static CharEncoding CharSet
        {
            get { return _charSet; }
            set
            {
                rvSetStringEncoding((int)value);
                _charSet = value;
            }
        }

        //character set
        private static CharEncoding _charSet = CharEncoding.Utf8;
    }
    /// <summary>
    /// Wraps various C# data types into an unmanaged buffer and exposes the
    /// first address as a <see cref="UIntPtr"/> for native calls.
    /// All internally allocated unmanaged memory is automatically released
    /// on <see cref="IDisposable.Dispose"/> or in the finalizer.
    /// </summary>
    public sealed class NativeArg : KingsHandler
    {
        private bool _ownsBuffer;

        // ====================================================================
        // ANSI conversion (no NuGet, no RuntimeInformation dependency)
        // ====================================================================

#if !NETFRAMEWORK
        private const uint CP_ACP = 0;

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int WideCharToMultiByte(
            uint CodePage,
            uint dwFlags,
            string lpWideCharStr,
            int cchWideChar,
            byte[] lpMultiByteStr,
            int cbMultiByte,
            IntPtr lpDefaultChar,
            IntPtr lpUsedDefaultChar);

        // Cached result: true = Windows and WideCharToMultiByte is available,
        // false = non-Windows or failed.
        private static bool? _ansiAvailable = null;
#endif

        /// <summary>
        /// Converts a managed string to a byte array using the local ANSI code page
        /// on Windows, or UTF-8 on non-Windows platforms.
        /// </summary>
        public static byte[] StringToAnsiBytes(string value)
        {
            if (value == null) value = string.Empty;

#if NETFRAMEWORK
            // .NET Framework: Encoding.Default is the real ANSI code page.
            return Encoding.Default.GetBytes(value);
#else
            // On .NET Core / .NET 5+, Encoding.Default is UTF-8,
            // so we must use Win32 WideCharToMultiByte(CP_ACP) on Windows.
            if (_ansiAvailable == true)
                return AnsiConvert(value);

            if (_ansiAvailable == false)
                return Encoding.UTF8.GetBytes(value);   // non-Windows fallback

            // First call: probe by invoking the native function.
            try
            {
                byte[] result = AnsiConvert(value);
                _ansiAvailable = true;
                return result;
            }
            catch (DllNotFoundException)
            {
                _ansiAvailable = false;
                return Encoding.UTF8.GetBytes(value);
            }
            catch (EntryPointNotFoundException)
            {
                _ansiAvailable = false;
                return Encoding.UTF8.GetBytes(value);
            }
#endif
        }

#if !NETFRAMEWORK
        /// <summary>
        /// Calls WideCharToMultiByte(CP_ACP) to convert UTF-16 to ANSI bytes.
        /// The result is NOT null-terminated (caller should append if needed).
        /// </summary>
        private static byte[] AnsiConvert(string value)
        {
            if (value.Length == 0) return new byte[0];

            // First call: measure required buffer size in bytes.
            int size = WideCharToMultiByte(
                CP_ACP, 0, value, value.Length,
                null, 0, IntPtr.Zero, IntPtr.Zero);

            if (size <= 0)
                throw new ExternalException("WideCharToMultiByte returned 0.");

            byte[] bytes = new byte[size];
            int written = WideCharToMultiByte(
                CP_ACP, 0, value, value.Length,
                bytes, size, IntPtr.Zero, IntPtr.Zero);

            if (written <= 0)
                throw new ExternalException("WideCharToMultiByte failed.");

            if (written < size)
                Array.Resize(ref bytes, written);

            return bytes;
        }
#endif

        /// <summary>
        /// Converts a managed string to a null-terminated byte array using the
        /// encoding selected by <c>CAP.CharSet</c>:
        ///   - UTF-8 when <c>CAP.CharSet == CharEncoding.Utf8</c>
        ///   - local ANSI otherwise
        /// The returned array always ends with a '\0' byte.
        /// </summary>
        public static byte[] StringToNativeBytes(string value)
        {
            if (value == null) value = string.Empty;

            if (CAP.CharSet == CharEncoding.Utf8)
            {
                return Encoding.UTF8.GetBytes(value + "\0");
            }
            else
            {
                byte[] ansi = StringToAnsiBytes(value);
                byte[] result = new byte[ansi.Length + 1];
                Array.Copy(ansi, result, ansi.Length);
                result[ansi.Length] = 0;    // null terminator
                return result;
            }
        }

        // ====================================================================
        // Private helpers
        // ====================================================================

        private void AllocateAndCopy(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                _handle = IntPtr.Zero;
                _ownsBuffer = false;
                return;
            }

            _handle = Marshal.AllocHGlobal(data.Length);
            if (_handle == IntPtr.Zero)
                throw new OutOfMemoryException("Failed to allocate unmanaged memory.");

            Marshal.Copy(data, 0, _handle, data.Length);
            _ownsBuffer = true;
        }

        // ====================================================================
        // Constructors — strings
        // ====================================================================

        /// <summary>
        /// Wraps a string as a null-terminated byte sequence.
        /// Encoding is selected automatically based on <c>CAP.CharSet</c>.
        /// </summary>
        public NativeArg(string value)
        {
            byte[] bytes = StringToNativeBytes(value);
            AllocateAndCopy(bytes);
        }

        /// <summary>
        /// Wraps a string as a null-terminated byte sequence with an explicit encoding.
        /// </summary>
        public NativeArg(string value, Encoding encoding)
        {
            if (value == null) value = string.Empty;
            if (encoding == null) encoding = Encoding.UTF8;

            byte[] bytes = encoding.GetBytes(value + "\0");
            AllocateAndCopy(bytes);
        }

        /// <summary>
        /// Wraps a string as a null-terminated byte sequence, forcing
        /// UTF-8 or local ANSI regardless of <c>CAP.CharSet</c>.
        /// </summary>
        public NativeArg(string value, bool utf8)
        {
            if (value == null) value = string.Empty;

            if (utf8)
            {
                AllocateAndCopy(Encoding.UTF8.GetBytes(value + "\0"));
            }
            else
            {
                byte[] ansi = StringToAnsiBytes(value);
                byte[] result = new byte[ansi.Length + 1];
                Array.Copy(ansi, result, ansi.Length);
                result[ansi.Length] = 0;
                AllocateAndCopy(result);
            }
        }

        // ====================================================================
        // Constructors — scalars
        // ====================================================================

        public NativeArg(float value) => AllocateAndCopy(BitConverter.GetBytes(value));
        public NativeArg(double value) => AllocateAndCopy(BitConverter.GetBytes(value));
        public NativeArg(int value) => AllocateAndCopy(BitConverter.GetBytes(value));
        public NativeArg(long value) => AllocateAndCopy(BitConverter.GetBytes(value));
        public NativeArg(short value) => AllocateAndCopy(BitConverter.GetBytes(value));
        public NativeArg(byte value) => AllocateAndCopy(new byte[] { value });
        public NativeArg(bool value) => AllocateAndCopy(BitConverter.GetBytes(value ? 1 : 0));

        // ====================================================================
        // Constructors — arrays
        // ====================================================================

        public NativeArg(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                _handle = IntPtr.Zero;
                _ownsBuffer = false;
                return;
            }
            AllocateAndCopy(data);
        }

        public NativeArg(float[] data)
        {
            if (data == null || data.Length == 0)
            {
                _handle = IntPtr.Zero;
                _ownsBuffer = false;
                return;
            }

            int size = data.Length * sizeof(float);
            _handle = Marshal.AllocHGlobal(size);
            if (_handle == IntPtr.Zero)
                throw new OutOfMemoryException("Failed to allocate unmanaged memory.");

            Marshal.Copy(data, 0, _handle, data.Length);
            _ownsBuffer = true;
        }

        public NativeArg(double[] data)
        {
            if (data == null || data.Length == 0)
            {
                _handle = IntPtr.Zero;
                _ownsBuffer = false;
                return;
            }

            int size = data.Length * sizeof(double);
            _handle = Marshal.AllocHGlobal(size);
            if (_handle == IntPtr.Zero)
                throw new OutOfMemoryException("Failed to allocate unmanaged memory.");

            Marshal.Copy(data, 0, _handle, data.Length);
            _ownsBuffer = true;
        }

        public NativeArg(int[] data)
        {
            if (data == null || data.Length == 0)
            {
                _handle = IntPtr.Zero;
                _ownsBuffer = false;
                return;
            }

            int size = data.Length * sizeof(int);
            _handle = Marshal.AllocHGlobal(size);
            if (_handle == IntPtr.Zero)
                throw new OutOfMemoryException("Failed to allocate unmanaged memory.");

            Marshal.Copy(data, 0, _handle, data.Length);
            _ownsBuffer = true;
        }

        public NativeArg(short[] data)
        {
            if (data == null || data.Length == 0)
            {
                _handle = IntPtr.Zero;
                _ownsBuffer = false;
                return;
            }

            int size = data.Length * sizeof(short);
            _handle = Marshal.AllocHGlobal(size);
            if (_handle == IntPtr.Zero)
                throw new OutOfMemoryException("Failed to allocate unmanaged memory.");

            Marshal.Copy(data, 0, _handle, data.Length);
            _ownsBuffer = true;
        }

        // ====================================================================
        // Constructor — raw pointer (borrowed)
        // ====================================================================

        /// <summary>
        /// Wraps a raw pointer directly without copying.
        /// The caller retains ownership of the underlying memory;
        /// this instance will NOT free it on Dispose.
        /// </summary>
        public NativeArg(IntPtr pointer)
        {
            _handle = pointer;
            _ownsBuffer = false;
        }

        // ====================================================================
        // Public API
        // ====================================================================

        /// <summary>
        /// Gets the first address of the wrapped data as <see cref="UIntPtr"/>.
        /// Returns <see cref="UIntPtr.Zero"/> if the buffer is empty.
        /// </summary>
        public UIntPtr Ptr
        {
            get
            {
                if (_handle == IntPtr.Zero) return UIntPtr.Zero;
                return new UIntPtr(unchecked((ulong)_handle.ToInt64()));
            }
        }



        /// <summary>
        /// Returns true if this instance owns the unmanaged buffer.
        /// </summary>
        public bool OwnsBuffer => _ownsBuffer;

        // ====================================================================
        // ReleaseHandle
        // ====================================================================

        protected sealed override void ReleaseHandle()
        {
            if (_ownsBuffer && _handle != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_handle);
            }
        }
    }


    public static class Pool
    {
        //对话框返回值
        //public const int DLG_OK = 1;
        //public const int DLG_CANCEL = 0;
        //public const int DLG_ABORT = -1;

        //public const int LANG_CHINESE = 0;
        //public const int LANG_ENGLISH = 1;


        public const int MAX_DESC_TEXT_LEN = 63 + 1;

        //布尔
        public const int TRUE = 1;
        public const int FALSE = 0;



        public const uint INVALID_ID = 0x7FFFFFFF;

        //无限等待
        public const int WAIT_INFINITE = 0x7FFFFFFF;
        public const int WAIT_NONE = 0;

#if DEBUGGING_KINGPOOL_SUITE

        private const string SUPPORT_LIB_NAME = "UniSupport_d.dll";
#else
          
       private const string SUPPORT_LIB_NAME = "UniSupport.dll";
#endif
        ////轴移动方向
        //public const int MOVE_POS = 1;
        //public const int MOVE_NEG = (-1);

        ////圆弧移动方向
        //public const int CW = 0;     //顺时针
        //public const int CCW = 1;     //move direction: 逆时针 

        ////输入输出电平:LOW 低,  HI 高
        //public const int TTL_LOW = 0;
        //public const int TTL_HI = 1;

        ////窗口z顺序
        //public const int ZW_TOP = 1;
        //public const int ZW_TOPMOST = 2;
        //public const int ZW_BOTTOM = 3;
        //public const int ZW_BOTTOMMOST = 4;


        public static void Assert(bool b, string s = null)
        {
            if (s == null)
            {
                Debug.Assert(b);
            }
            else
            {
                Debug.Assert(b, s);
            }
        }

        public static int BOOL(bool b)
        {
            if (b) return TRUE;
            return FALSE;
        }
        public static bool BOOL(int b)
        {
            if (b == TRUE) return true;
            return false;
        }

        public static UInt32 RGB(byte red, byte green, byte blue)
        {
            return (UInt32)(((red) << 16) | ((green) << 8) | (blue) | (255 << 24));
        }

        public static UInt32 RGBA(byte red, byte green, byte blue, byte alpha)
        {
            return (UInt32)(((red) << 16) | ((green) << 8) | (blue) | ((alpha) << 24));
        }

        public static byte GET_COLOR_RED(UInt32 color)
        {
            return (byte)(((color) >> 16) & 0xFF);
        }
        public static byte GET_COLOR_GREEN(UInt32 color)
        {
            return (byte)(((color) >> 8) & 0xFF);
        }
        public static byte GET_COLOR_BLUE(UInt32 color)
        {
            return (byte)(((color)) & 0xFF);
        }
        public static byte GET_COLOR_ALPHA(UInt32 color)
        {
            return (byte)(((color) >> 24) & 0xFF);
        }

        public static RvRgb COLOR_TO_RGB(UInt32 color)
        {
            return new RvRgb(GET_COLOR_RED(color), GET_COLOR_GREEN(color), GET_COLOR_BLUE(color));
        }

        public static RvRgba COLOR_TO_RGBA(UInt32 color)
        {
            return new RvRgba(GET_COLOR_RED(color), GET_COLOR_GREEN(color), GET_COLOR_BLUE(color), GET_COLOR_ALPHA(color));
        }

        public static uint RGB_TO_COLOR(RvRgb rgb)
        {
            return (uint)((rgb.red << 16) | (rgb.green << 8) | rgb.blue | (255 << 24));

        }
        public static uint RGBA_TO_COLOR(RvRgba rgba)
        {
            return (uint)((rgba.red << 16) | (rgba.green << 8) | rgba.blue | (rgba.alpha << 24));
        }


        public static void SetCharEncoding(CharEncoding encoding)
        {
            CAP.CharSet = encoding;
        }



        public static byte[] StringToCharBytes(string s)
        {

            byte[] utf8;
            if (CAP.CharSet == CharEncoding.Utf8)
            {
                utf8 = Encoding.UTF8.GetBytes(s + "\0");
            }
            else
            {
                utf8 = Encoding.Default.GetBytes(s + "\0");
            }

            return utf8;
        }

        public static int Limit(int lower, int upper, int pos)
        {
            if (pos < lower) return lower;
            if (pos > upper) return upper;
            return pos;
        }
        public static string GetFileName(string strFilePath)
        {
            int n = strFilePath.LastIndexOf('\\');

            if (n < 0)
            {
                n = strFilePath.LastIndexOf('.');
                if (n < 0)
                {
                    return strFilePath;
                }

                return strFilePath.Substring(0, n);
            }
            else
            {
                string s = strFilePath.Substring(n + 1);
                n = s.LastIndexOf('.');
                if (n < 0)
                {
                    return s;
                }
                else
                {
                    return s.Substring(0, n);
                }
            }
        }



        //最多sleep 1000
        public static void Sleep(int minisecs)
        {
            if (minisecs < 0) minisecs = 0;
            else if (minisecs > 1000) minisecs = 1000;
            Thread.Sleep(minisecs);
        }

        public static string GetFullFileName(string strFilePath)
        {
            int n = strFilePath.LastIndexOf('\\');

            if (n < 0)
            {
                return strFilePath;
            }
            else
            {
                return strFilePath.Substring(n + 1);
            }
        }

        public static double Max(double n1, double n2)
        {
            return Math.Max(n1, n2);
        }
        public static double Min(double n1, double n2)
        {
            return Math.Min(n1, n2);
        }

        public static float Max(float n1, float n2)
        {
            return Math.Max(n1, n2);
        }
        public static float Min(float n1, float n2)
        {
            return Math.Min(n1, n2);
        }
        public static int Max(int n1, int n2)
        {
            return Math.Max(n1, n2);
        }
        public static int Min(int n1, int n2)
        {
            return Math.Min(n1, n2);
        }
        public static short Max(short n1, short n2)
        {
            return Math.Max(n1, n2);
        }
        public static short Min(short n1, short n2)
        {
            return Math.Min(n1, n2);
        }


        public static string GetPath(string strFilePath)
        {
            int n = strFilePath.LastIndexOf('\\');

            if (n < 0)
            {
                return strFilePath;
            }
            else
            {
                return strFilePath.Substring(0, n);
            }
        }

        public static double Dist(RvPointF64 p0, RvPointF64 p1)
        {
            return Math.Sqrt((p0.x - p1.x) * (p0.x - p1.x) + (p0.y - p1.y) * (p0.y - p1.y));
        }
        public static double Dist(RvPointF32 p0, RvPointF32 p1)
        {
            return Math.Sqrt((p0.x - p1.x) * (p0.x - p1.x) + (p0.y - p1.y) * (p0.y - p1.y));
        }

        public static double Dist(float x0, float y0, float x1, float y1)
        {
            return Math.Sqrt((x0 - x1) * (x0 - x1) + (y0 - y1) * (y0 - y1));
        }


        public static KImage LoadImage(string strFilePath)
        {
            if (string.IsNullOrEmpty(strFilePath)) return null;

            KImage image = new KImage();
            if (image.Load(strFilePath))
            {
                return image;
            }

            return null;
        }



        public static bool SaveImage(KImage image, string strFilePath)
        {
            if (null == image || string.IsNullOrEmpty(strFilePath)) return false;

            string strFormat = "bmp";// IFF_DEFAULT, int flag = 0
            int idx = strFilePath.LastIndexOf('.');

            if (idx > 0)
            {
                strFormat = strFilePath.Substring(idx + 1);
            }

            return image.Save(strFilePath, strFormat);
        }

        public static string GetVersionDescription()
        {

#if DEBUGGING_KINGPOOL_SUITE
            string s = $"Version: {CAP.MAIN_VERSION}.{CAP.SUB_VERSION}.{CAP.MINOR_VERSION}, Debug: true";

#else
            string s = $"Version: {CAP.MAIN_VERSION}.{CAP.SUB_VERSION}.{CAP.MINOR_VERSION}, Debug: false";
            
#endif
            return s;
        }

        //[DllImport("UniSupport.dll", CharSet = CharSet.Ansi, EntryPoint = "uniInitialize")]
        //public static extern bool Initialize(string strLicenceCode, string strAppName, string strLicenceFile);

        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi, EntryPoint = "uniInitTrial")]
        public static extern bool InitTrial(string strLicenceFile, int verifyCode);

        //注册订阅版：不超过1年
        [DllImport("UniSupport.dll", CharSet = CharSet.Ansi, EntryPoint = "uniInitSubscribe")]
        public static extern bool InitSubscribe(string strLicenceFile, int verifyCode);

        //注册永久版
        [DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniInitLifetime")]
        public static extern bool InitLifetime(string strFrontGateFile, int verifyCode);


        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniInitializeE1")]
        //public static extern bool InitializeE1(int pin, string strLicencePath, ref int userData);

        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniInitializeE2")]
        //public static extern bool InitializeE2(string strAppName, string strLicenceCode, string strAppPath, string strLicenceFile);

        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniInitializeE3")]
        //public static extern bool InitializeE3(int appId, bool bEnableTrial, string strDeviceSnum, string strLicencePath, ref int userData);
        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniInitializeE4")]
        //public static extern bool InitializeE4(string strAppPath, string strAccount, string strPassword, bool bVipUser, int timeout);

        [DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniUninitialize")]

        public static extern void Uninitialize();

        [DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniGetErrorText")]
        public static extern IntPtr uniGetErrorText(int errCode);

        [DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniGetLicenceType")]
        public static extern int GetLicenceType(string strFilePath);
        [DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniGetComputerId")]
        public static extern int GetComputerId(StringBuilder strOut, int size);

        [DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniGetDiskCode")]
        public static extern int GetDiskCode(StringBuilder strOut, int size);


        [DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniGetFrontUserData")]
        public static extern bool GetFrontUserData(string strFilePath, StringBuilder strOut, int size);

        [DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniGetRemainedDays")]
        public static extern int GetRemainedDays(string strFilePath);


        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniShowWindow")]
        //public static extern void ShowWindow(IntPtr hWnd, bool bShow);

        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniZorderWindow")]
        //public static extern void ZorderWindow(IntPtr hWnd, int order);

        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniEnableWindow")]
        //public static extern void EnableWindow(IntPtr hWnd, bool bEnable);

        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniMoveWindow")]
        //public static extern void MoveWindow(IntPtr hThis, int left, int top, int width, int height);
        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniSetWindowCaption")]
        //public static extern void SetWindowCaption(IntPtr hThis, string strCaption);

        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniIsWindowVisible")]
        //public static extern bool IsWindowVisible(IntPtr hWnd);
        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniIsWindowEnable")]
        //public static extern bool IsWindowEnable(IntPtr hWnd);


        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniAuthorizeViaCloud")]
        //public static extern bool AuthorizeViaCloud(string strAccount, string strPassword);
        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniAuthorizeViaFirmware")]
        //public static extern bool AuthorizeViaFirmware(string strFirmwInfo, int nFirmwCount);


        //视场扫描
        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniShowScanWndE1")]
        //public static extern int ShowSightScanWindow(IntPtr hRobot, IntPtr hCamera, IntPtr hRealView, double pixelsPerMm, ref UniParamScan extra);

        //live camera window, pop
        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniCreateLiveCameraWnd")]
        //public static extern IntPtr CreateLiveCameraWnd(IntPtr hParent, IntPtr hCamera);
        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniReleaseLiveCameraWndView")]
        //public static extern void ReleaseLiveCameraWndView(IntPtr hLiveCameraWin);
        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniDestroyLiveCameraWnd")]
        //public static extern void DestroyLiveCameraWnd(IntPtr hLiveCameraWin);


        ////calib fov window
        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniShowCaliFovE2Wnd")]
        //public static extern int ShowCaliFovE2Wnd(string strTitle, IntPtr hCamera, IntPtr hRobot, string strParamFile, ref UniParamCf2 extra);
        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniReadCaliFovE2Params")]
        //public static extern bool ReadCaliFovE2Params(string strParamFile, ref uint pId, ref UniFovInfo pFovInfo);
        ////calib w2c window
        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniShowCaliMobileW2cWin")]
        //public static extern int ShowCaliMobileW2cWin(string strTitle, IntPtr hCamera, IntPtr hRobot, string strParamFile, UniParamMobileW2c extra);
        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniShowAutoCaliW2cWnd")]
        //public static extern int ShowAutoCaliW2cWnd(string strTitle, IntPtr hCamera, IntPtr hRobot, UniAw2cParams extra, IntPtr hRsm);

        //single mover
        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniShowSingleMoverWnd")]
        //public static extern int ShowSingleMoverWnd(IntPtr hRobot, IntPtr pAxisArray, int axisCount);

        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniShowGate3MoverWnd")]
        //public static extern int ShowGate3MoverWnd(IntPtr hRobot, int axisX, int axisY, int axisZ);

        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniShowGate3MoverWndE1")]
        //public static extern int ShowGate3MoverWnd(IntPtr hRobot, int axisX, int axisY, int axisZ, int sceneId);


        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniShowSingleMoverWndE2")]
        //public static extern int ShowSingleMoverWndE2(IntPtr hRobot, IntPtr pAxisArray, int axisCount);
        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniCaliberShowParamWin")]
        //public static extern int ShowCaliberParamWin(IntPtr hCaliber, IntPtr image);

        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniSetLanguage")]
        //public static extern void SetLanguage(int lang);
        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniGetLanguage")]
        //public static extern int GetLanguage();

        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniShowFindShotPosWnd")]
        //public static extern int ShowFindShotPosWnd(IntPtr hRobot, IntPtr hCamera, UniFspParams param, ref double pResult);

        //

        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniSetNotifyWnd")]
        //public static extern IntPtr SetNotifyWnd(IntPtr hRobot, IntPtr hNotifyWnd);
        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniGetNotifyWnd")]
        //public static extern IntPtr GetNotifyWnd(IntPtr hRobot);

        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniShowCaliCamToToolDistWnd")]
        //public static extern int ShowCaliCamToToolDistWnd(string strTitle, IntPtr hRobot, IntPtr hCamera, UniCaliT2lDistParams param, ref RvOffsetF64 result);

        ////licence utilities
        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniWebcomRequestAuthorizeFileAdv")]
        //public static extern bool RequestAuthorizeAppFile(IntPtr hWebcom, int appId, string strMachineCode, string strDeviceSnum, int userData, string strDestPath);

        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniWebcomRequestAuthorizeFileModu")]
        //public static extern bool RequestAuthorizeModuleFile(IntPtr hWebcom, int accFlags, int pin, int userData, string strDestPath);
        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniWebcomRequestAuthorizeFileTrial")]
        //public static extern bool RequestAuthorizeTrialFile(IntPtr hWebcom, int csysId, int trialDays, int userData, string strDestPath);

        //[DllImport(SUPPORT_LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniWebcomGetLatestVersion")]
        //public static extern bool GetAppLatestVersion(IntPtr hWebcom, int appId, string strLicenceCode, ref int pMain, ref int pSub, ref int pMini);


        ////single axis movement
        //public static int ShowSingleMoverWndE2(IntPtr hRobot, ref UniSingleMoverParamsE1[] pAxisArray, int length)
        //{
        //    int cnt = length;
        //    IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(UniSingleMoverParamsE1)) * cnt);

        //    for (int i = 0; i < cnt; i++)
        //    {
        //        IntPtr ptrtmp = (IntPtr)(ptr + i * Marshal.SizeOf(typeof(UniSingleMoverParamsE1)));
        //        Marshal.StructureToPtr(pAxisArray[i], ptrtmp, false);
        //    }

        //    int ret = ShowSingleMoverWndE2(hRobot, ptr, cnt);

        //    //不需要传出来
        //    //for (int i = 0; i < cnt; i++)
        //    //{
        //    //    IntPtr ptrtmp = (IntPtr)( ptr + i * Marshal.SizeOf(typeof(UniSingleMoverParamsE1)));
        //    //    pAxisArray[i] = (UniSingleMoverParamsE1)Marshal.PtrToStructure(ptrtmp, typeof(UniSingleMoverParamsE1));
        //    //}

        //    Marshal.FreeHGlobal(ptr);

        //    return ret;
        //}

        public static string GetErrorText(int errCode)
        {
            IntPtr ptr = uniGetErrorText(errCode);
            if (ptr != IntPtr.Zero)
            {
                return Marshal.PtrToStringAnsi(ptr);
            }

            return "";
        }

        public static int ParseInt(string str)
        {
            if (str == null) return 0;

            if (string.IsNullOrEmpty(str)) return 0;

            int n;
            if (int.TryParse(str, out n))
            {
                return n;
            }
            return 0;
        }

        public static ushort ParseUshort(string str)
        {
            if (str == null) return 0;

            if (string.IsNullOrEmpty(str)) return 0;

            ushort n;
            if (ushort.TryParse(str, out n))
            {
                return n;
            }

            return 0;
        }

        public static bool ParseBool(string str)
        {
            if (str == null) return false;

            if (string.IsNullOrEmpty(str)) return false;

            return (string.Compare(str.ToLower(), "true") == 0);
        }

        public static double ParseDouble(string str)
        {
            if (str == null) return 0;
            if (string.IsNullOrEmpty(str)) return 0;

            double n;
            if (double.TryParse(str, out n))
            {
                return n;
            }
            return 0;
        }

        public static short HIWORD(IntPtr p)
        {
            int n = 0;
            if (IntPtr.Size == 8)
            {
                n = (int)((long)p.ToInt64() << 32 >> 32);
            }
            else
            {
                n = (int)p.ToInt32();
            }
            return ((short)(n >> 16));
        }
        public static short LOWORD(IntPtr p)
        {
            int n = 0;
            if (IntPtr.Size == 8)
            {
                n = (int)((long)p.ToInt64() << 32 >> 32);
            }
            else
            {
                n = (int)p.ToInt32();
            }

            return ((short)(n & 0xFFFF));
        }

        //LPARAM, WPARAM都可以这么make
        public static IntPtr MAKE_PARAM(int low, int high)
        {
            return (IntPtr)((high << 16) | (low & 0xFFFF));
        }

        public static short GET_PARAM_X(IntPtr lParam)
        {
            return LOWORD(lParam);
            //long n = (long)lParam.ToInt64();
            //return (ushort)(n & 0xFFFF);
        }
        public static short GET_PARAM_Y(IntPtr lParam)
        {
            return HIWORD(lParam);
            //long n = (long)lParam.ToInt64();
            //return (ushort)((n >> 16) & 0xFFFF);
        }

        public static string GetComputerId()
        {
            StringBuilder sb = new StringBuilder(1204);

            GetComputerId(sb, 120);

            return sb.ToString();
        }

        public static string GetDiskCode()
        {
            StringBuilder sb = new StringBuilder(1204);

            GetDiskCode(sb, 120);

            return sb.ToString();
        }

        public static string GetFrontUserData(string strFilePath)
        {
            StringBuilder sb = new StringBuilder(1204);

            bool ret = GetFrontUserData(strFilePath, sb, 1204);

            if (ret)
            {
                return sb.ToString();
            }
            return null;
        }


    }


    public class Clock
    {
        DateTime m_start = DateTime.Now;
        bool m_bStarted = false;
        double m_timeElapsed = 0;

        public void Start()
        {
            m_start = DateTime.Now;
            m_timeElapsed = 0;
            m_bStarted = true;
        }

        public double Stop(bool bStartAgain)
        {
            if (!m_bStarted) return 0;

            m_timeElapsed = GetElapsedSeconds();

            m_bStarted = false;

            double n = m_timeElapsed;
            if (bStartAgain)
            {
                Start();
            }

            return n;
        }

        public double GetElapsedSeconds()
        {
            if (!m_bStarted)
            {
                return m_timeElapsed;
            }


            DateTime end = DateTime.Now;
            TimeSpan ts = end - m_start;
            return ts.TotalMilliseconds / 1000;
        }


    }

    public class FileOption
    {
        private string m_strFilePath = "";

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(string strSection, string strField, string strDefault, StringBuilder sbReturnString, int len, string strFilePath);
        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(string strSection, string strField, string strValue, string strFilePath);

        public int GetInt(string strSection, string strField, int defval = 0)
        {
            string strFilePath = m_strFilePath;
            return GetPrivateProfileInt(strSection, strField, defval, strFilePath);
        }

        public FileOption(string strFilePath)
        {
            m_strFilePath = strFilePath;
        }

        public void Redir(string strFilePath)
        {
            m_strFilePath = strFilePath;
        }

        public double GetFloat(string strSection, string strField, double defval = 0)
        {
            string strFilePath = m_strFilePath;

            StringBuilder sb = new StringBuilder(123);
            if (GetPrivateProfileString(strSection, strField, "0", sb, 123, strFilePath) > 0)
            {
                return double.Parse(sb.ToString());
            }

            return 0;
        }

        public bool GetBool(string strSection, string strField, bool defval = false)
        {
            string strFilePath = m_strFilePath;
            int n = GetPrivateProfileInt(strSection, strField, (defval ? 1 : 0), strFilePath);
            return (n != 0);
        }


        public string GetText(string strSection, string strField)
        {
            StringBuilder sb = new StringBuilder(123);

            if (GetPrivateProfileString(strSection, strField, "", sb, 123, m_strFilePath) > 0)
            {
                return sb.ToString();
            }

            return "";

        }


        public void SetInt(string strSection, string strField, int val)
        {
            int defval = val;
            string str = string.Format("{0}", defval);
            WritePrivateProfileString(strSection, strField, str, m_strFilePath);

        }

        public void SetFloat(string strSection, string strField, double val)
        {
            double defval = val;
            string str = string.Format("{0}", defval);

            SetText(strSection, strField, str);
        }
        public void SetBool(string strSection, string strField, bool val)
        {
            SetInt(strSection, strField, (val ? 1 : 0));
        }
        //void SetBool(const TCHAR* strSection, const TCHAR* strField, bool val);

        public void SetText(string strSection, string strField, string strVal)
        {
            WritePrivateProfileString(strSection, strField, strVal, m_strFilePath);
        }


        //public uint Read(string strSection, string strField, ref int val)
        //{
        //    val = GetPrivateProfileInt(strSection, strField, 0, m_strFilePath);
        //    return (uint)val;
        //}
        //uint Read(string strSection, string strField, ref double val)
        //{
        //    string strFilePath = m_strFilePath;
        //    char[] buff = new char[64];
        //    char[] str = new char[64];
        //    double defval = 0.0;
        //    string str1 = string.Format("{0}", defval);
        //    double ret = GetPrivateProfileString(strSection, strField, str, buff, buff.Length, strFilePath);
        //    return (uint)ret;
        //}
        //public uint Read(string strSection, string strField, char[] pVal, int size)
        //{
        //    char[] str = new char[1];
        //    double result = GetPrivateProfileString(strSection, strField, str, pVal, size, m_strFilePath);
        //    return (uint)result;
        //}

        //public uint Read(string strSection, string strField, ref bool val)
        //{
        //    string strFilePath = m_strFilePath;
        //    int n = GetPrivateProfileInt(strSection, strField, 0, strFilePath);
        //    return (uint)n;
        //}



        //public bool Write(string strSection, string strField, int val)
        //{
        //    //char[] str = new char[64];
        //    string str1 = string.Format("{0}", val);
        //    return WritePrivateProfileString(strSection, strField, str1, m_strFilePath);

        //}
        //public bool Write(string strSection, string strField, bool val)
        //{
        //    string str1 = string.Format("{0}", val);
        //    return WritePrivateProfileString(strSection, strField, str1, m_strFilePath);
        //}
        //public bool Write(string strSection, string strField, double val)
        //{
        //    int result = (int)(val != 0 ? 1 : 0);
        //    string str1 = string.Format("{0}", result);
        //    return WritePrivateProfileString(strSection, strField, str1, m_strFilePath);
        //}
        //public bool Write(string strSection, string strField, string strVal)
        //{
        //    if (null == strVal) return false;
        //    return WritePrivateProfileString(strSection, strField, strVal, m_strFilePath);
        //}

    }
}
