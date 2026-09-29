//**************************************************************************************************
//Copyright (c) 2022-2026 
//Shenzhen SoftStorm Technology Co., Limited  
//All rights reserved.

//Licensed under the MIT license. See LICENCE file in the project root for full license information.
//***************************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Reflection;


using Kingpool.Core;
using Kingpool.Vision;

namespace Kingpool.Utility
{

    /// <summary>
    /// Provides disk I/O operations for Kingpool Suite objects.
    /// Inherits from <see cref="KingsHandler"/> for deterministic resource release.
    /// </summary>
    public class DiskHelper : KingsHandler
    {
        // ====================================================================
        // DLL import constants
        // ====================================================================
#if DEBUGGING_KINGPOOL_SUITE
        private const string LIB_NAME = "UniSupport_d.dll";
#else
        private const string LIB_NAME = "UniSupport.dll";
#endif

        // ====================================================================
        // Private unmanaged imports
        // ====================================================================

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr uniDiskCreateFile(string strFilePath);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern void uniDiskDestroyFile(ref IntPtr hFile);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr uniDiskOpenFile(string strFilePath);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr uniDiskOpenFileInMemory(IntPtr pFileData, uint nSize, ref int pErrCode);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern void uniDiskCloseFile(IntPtr hFile);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskLoadBitmap(IntPtr image, string strFileName);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskSaveBitmap(IntPtr image, string strFileName);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskSaveMask(IntPtr mask, string strFileName);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskSaveBlob(IntPtr blob, string strFileName);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskBeginFileOut1(IntPtr hFile, int version, string strTag);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskBeginFileOut2(IntPtr hFile);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskEndFileOut(IntPtr hFile);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskBeginFileIn(IntPtr hFile, out int pVersion, StringBuilder strFileTag, int tagSize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskBeginFileInE1(IntPtr hFile);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskEndFileIn(IntPtr hFile);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskBeginObjOutE1(IntPtr hFile, uint tag, int version, out uint pCurPos);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskEndObjOutE1(IntPtr hFile, uint tag, uint nPrevPos);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskBeginObjInE1(IntPtr hFile, uint tag, out int pVersion, out uint pObjSize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskEndObjInE1(IntPtr hFile, uint tag);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskPickObjTagE1(IntPtr hFile, ref uint pTag, ref int pVersion, ref uint pObjSize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskEncodeImage(IntPtr hFile, IntPtr image, int nFormat);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskDecodeImage1(IntPtr hFile, IntPtr image);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern IntPtr uniDiskDecodeImage2(IntPtr hFile);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskEncodeMask(IntPtr hFile, IntPtr mask);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskDecodeMask(IntPtr hFile, IntPtr mask, bool bRestrictSize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskEncodeBlob(IntPtr hFile, IntPtr blob);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskDecodeBlob(IntPtr hFile, IntPtr blob);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskWriteString(IntPtr hFile, string strVal);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskWriteChar(IntPtr hFile, char val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskWriteShort(IntPtr hFile, short val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskWriteUShort(IntPtr hFile, ushort val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskWriteBool(IntPtr hFile, bool val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskWriteInt(IntPtr hFile, int val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskWriteUInt(IntPtr hFile, uint val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskWriteDWORD(IntPtr hFile, uint val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskWriteFloat(IntPtr hFile, float val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskWriteDouble(IntPtr hFile, double val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskWritePoint(IntPtr hFile, RvPoint val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskWritePointF(IntPtr hFile, RvPointF32 val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskWritePoint64F(IntPtr hFile, RvPointF64 val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskWriteInt64(IntPtr hFile, long val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskWriteUInt64(IntPtr hFile, ulong val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskWriteData(IntPtr hFile, IntPtr pData, int size);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskReadInt(IntPtr hFile, ref int val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskReadChar(IntPtr hFile, ref char val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskReadShort(IntPtr hFile, ref short val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskReadUShort(IntPtr hFile, ref ushort val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskReadBool(IntPtr hFile, ref bool val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskReadString(IntPtr hFile, StringBuilder val, int size);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskReadFloat(IntPtr hFile, ref float val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskReadDouble(IntPtr hFile, ref double val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskReadUInt(IntPtr hFile, ref uint val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskReadDWORD(IntPtr hFile, ref uint val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskReadInt64(IntPtr hFile, ref long val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskReadUInt64(IntPtr hFile, ref ulong val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskReadPoint(IntPtr hFile, ref RvPoint val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskReadPointF(IntPtr hFile, ref RvPointF32 val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskReadPoint64F(IntPtr hFile, ref RvPointF64 val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskReadData(IntPtr hFile, IntPtr pData, uint size);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi)]
        private static extern bool uniDiskReadDataE1(IntPtr hFile, IntPtr pData, ref uint size);

        // ====================================================================
        // Constructors
        // ====================================================================

        /// <summary>
        /// Creates a new empty DiskHelper with no associated file handle.
        /// Use <see cref="CreateFile"/> or <see cref="OpenFile"/> to attach a file.
        /// </summary>
        public DiskHelper()
        {
            _handle = IntPtr.Zero;
            _attached = false;
        }

        /// <summary>
        /// Wraps an existing disk file handle.
        /// </summary>
        /// <param name="existingHandle">Existing file handle.</param>
        /// <param name="attach">
        /// True if the handle is owned externally (this instance will not close it
        /// on dispose); false if this instance takes ownership and will close it.
        /// </param>
        public DiskHelper(IntPtr existingHandle, bool attach)
        {
            if (existingHandle == IntPtr.Zero)
                throw new ArgumentException("Invalid disk file handle", nameof(existingHandle));
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
                uniDiskCloseFile(_handle);
            }
        }

        // ====================================================================
        // File lifecycle
        // ====================================================================

        /// <summary>
        /// Creates a new disk file at the given path and attaches it to this instance.
        /// Releases any previously attached file first.
        /// </summary>
        public bool CreateFile(string strFilePath)
        {
            Destroy(); // release any existing handle
            _handle = uniDiskCreateFile(strFilePath);
            _attached = false;
            return _handle != IntPtr.Zero;
        }

        /// <summary>
        /// Opens an existing disk file at the given path and attaches it to this instance.
        /// Releases any previously attached file first.
        /// </summary>
        public bool OpenFile(string strFilePath)
        {
            Destroy();
            _handle = uniDiskOpenFile(strFilePath);
            _attached = false;
            return _handle != IntPtr.Zero;
        }

        /// <summary>
        /// Opens a disk file from an in-memory buffer.
        /// </summary>
        /// <param name="pFileData">Pointer to the file data.</param>
        /// <param name="nSize">Size of the data in bytes.</param>
        /// <param name="pErrCode">Receives an error code on failure.</param>
        public bool OpenFileInMemory(IntPtr pFileData, uint nSize, ref int errCode)
        {
            Destroy();

            _handle = uniDiskOpenFileInMemory(pFileData, nSize, ref errCode);

            _attached = false;
            return _handle != IntPtr.Zero;
        }

        /// <summary>
        /// Closes the current file and releases its handle.
        /// Equivalent to <see cref="KingsHandler.Destroy"/>.
        /// </summary>
        public void CloseFile()
        {
            Destroy();
        }

        // ====================================================================
        // Static bitmap / mask / blob I/O
        // ====================================================================

        public static bool LoadBitmap(IntPtr image, string strFileName)
            => uniDiskLoadBitmap(image, strFileName);

        public static bool SaveBitmap(IntPtr image, string strFileName)
            => uniDiskSaveBitmap(image, strFileName);

        public static bool SaveMask(IntPtr mask, string strFileName)
            => uniDiskSaveMask(mask, strFileName);

        public static bool SaveBlob(IntPtr blob, string strFileName)
            => uniDiskSaveBlob(blob, strFileName);

        // ====================================================================
        // File-level framing
        // ====================================================================

        public bool BeginFileOut(int version, string strTag)
        {
            if (_handle == IntPtr.Zero) return false;
            return uniDiskBeginFileOut1(_handle, version, strTag);
        }

        public bool BeginFileOut()
        {
            if (_handle == IntPtr.Zero) return false;
            return uniDiskBeginFileOut2(_handle);
        }

        public bool EndFileOut()
        {
            if (_handle == IntPtr.Zero) return false;
            return uniDiskEndFileOut(_handle);
        }

        public bool BeginFileIn(out int pVersion, ref string strFileTag)
        {
            pVersion = 0;
            if (_handle == IntPtr.Zero) return false;

            StringBuilder sb = new StringBuilder(512);
            if (uniDiskBeginFileIn(_handle, out pVersion, sb, 512))
            {
                strFileTag = sb.ToString();
                return true;
            }
            return false;
        }

        public bool BeginFileIn()
        {
            if (_handle == IntPtr.Zero) return false;
            return uniDiskBeginFileInE1(_handle);
        }

        public bool EndFileIn()
        {
            if (_handle == IntPtr.Zero) return false;
            return uniDiskEndFileIn(_handle);
        }

        // ====================================================================
        // Section-level framing
        // ====================================================================

        public bool BeginSectionOut(uint tag, int version, out uint pCurPos)
        {
            pCurPos = 0;
            if (_handle == IntPtr.Zero) return false;
            return uniDiskBeginObjOutE1(_handle, tag, version, out pCurPos);
        }

        public bool EndSectionOut(uint tag, uint nPrevPos)
        {
            if (_handle == IntPtr.Zero) return false;
            return uniDiskEndObjOutE1(_handle, tag, nPrevPos);
        }

        public bool BeginSectionIn(uint tag, out int pVersion, out uint pObjSize)
        {
            pVersion = 0;
            pObjSize = 0;
            if (_handle == IntPtr.Zero) return false;
            return uniDiskBeginObjInE1(_handle, tag, out pVersion, out pObjSize);
        }

        public bool EndSectionIn(uint tag)
        {
            if (_handle == IntPtr.Zero) return false;
            return uniDiskEndObjInE1(_handle, tag);
        }

        public bool PickSectionTag(ref uint pTag, ref int pVersion, ref uint pObjSize)
        {
            if (_handle == IntPtr.Zero) return false;
            return uniDiskPickObjTagE1(_handle, ref pTag, ref pVersion, ref pObjSize);
        }

        // ====================================================================
        // Image / mask / blob encoding
        // ====================================================================

        public bool EncodeImage(IntPtr image, int nFormat)
        {
            if (_handle == IntPtr.Zero) return false;
            return uniDiskEncodeImage(_handle, image, nFormat);
        }

        public bool EncodeImage(KImage image, int nFormat)
        {
            if (image == null) return false;
            return EncodeImage(image.Handle, nFormat);
        }

        public bool DecodeImage(IntPtr image)
        {
            if (_handle == IntPtr.Zero) return false;
            return uniDiskDecodeImage1(_handle, image);
        }

        public bool DecodeImage(KImage image)
        {
            if (image == null) return false;
            return DecodeImage(image.Handle);
        }

        public KImage DecodeImage()
        {
            if (_handle == IntPtr.Zero) return null;
            IntPtr h = uniDiskDecodeImage2(_handle);
            return h != IntPtr.Zero ? new KImage(h, false) : null;
        }

        public bool EncodeMask(IntPtr mask)
        {
            if (_handle == IntPtr.Zero) return false;
            return uniDiskEncodeMask(_handle, mask);
        }

        public bool EncodeMask(KMask mask)
        {
            if (mask == null) return false;
            return EncodeMask(mask.Handle);
        }

        public bool DecodeMask(IntPtr mask, bool bRestrictSize)
        {
            if (_handle == IntPtr.Zero) return false;
            return uniDiskDecodeMask(_handle, mask, bRestrictSize);
        }

        public bool DecodeMask(KMask mask, bool bRestrictSize)
        {
            if (mask == null) return false;
            return DecodeMask(mask.Handle, bRestrictSize);
        }

        public bool EncodeBlob(IntPtr blob)
        {
            if (_handle == IntPtr.Zero) return false;
            return uniDiskEncodeBlob(_handle, blob);
        }

        public bool EncodeBlob(KBlob blob)
        {
            if (blob == null) return false;
            return EncodeBlob(blob.Handle);
        }

        public bool DecodeBlob(IntPtr blob)
        {
            if (_handle == IntPtr.Zero) return false;
            return uniDiskDecodeBlob(_handle, blob);
        }

        public bool DecodeBlob(KBlob blob)
        {
            if (blob == null) return false;
            return DecodeBlob(blob.Handle);
        }

        // ====================================================================
        // Write helpers
        // ====================================================================

        public bool Write(string strVal)
        {
            if (_handle == IntPtr.Zero) return false;
            return uniDiskWriteString(_handle, strVal ?? "");
        }

        public bool WriteE1(string strVal)
        {
            if (_handle == IntPtr.Zero) return false;
            int len = strVal?.Length ?? 0;
            if (!uniDiskWriteInt(_handle, len)) return false;
            if (len > 0) return uniDiskWriteString(_handle, strVal);
            return true;
        }

        public bool Write(char val) => _handle != IntPtr.Zero && uniDiskWriteChar(_handle, val);
        public bool Write(short val) => _handle != IntPtr.Zero && uniDiskWriteShort(_handle, val);
        public bool Write(ushort val) => _handle != IntPtr.Zero && uniDiskWriteUShort(_handle, val);
        public bool Write(bool val) => _handle != IntPtr.Zero && uniDiskWriteBool(_handle, val);
        public bool Write(int val) => _handle != IntPtr.Zero && uniDiskWriteInt(_handle, val);
        public bool Write(uint val) => _handle != IntPtr.Zero && uniDiskWriteUInt(_handle, val);
        public bool Write(float val) => _handle != IntPtr.Zero && uniDiskWriteFloat(_handle, val);
        public bool Write(double val) => _handle != IntPtr.Zero && uniDiskWriteDouble(_handle, val);
        public bool Write(RvPoint val) => _handle != IntPtr.Zero && uniDiskWritePoint(_handle, val);
        public bool Write(RvPointF32 val) => _handle != IntPtr.Zero && uniDiskWritePointF(_handle, val);
        public bool Write(RvPointF64 val) => _handle != IntPtr.Zero && uniDiskWritePoint64F(_handle, val);
        public bool Write(long val) => _handle != IntPtr.Zero && uniDiskWriteInt64(_handle, val);
        public bool Write(ulong val) => _handle != IntPtr.Zero && uniDiskWriteUInt64(_handle, val);

        public bool Write(RvRect rect)
        {
            if (_handle == IntPtr.Zero) return false;
            return uniDiskWriteInt(_handle, rect.left)
                && uniDiskWriteInt(_handle, rect.top)
                && uniDiskWriteInt(_handle, rect.right)
                && uniDiskWriteInt(_handle, rect.bottom);
        }

        public bool Write(RvRectF32 rect)
        {
            if (_handle == IntPtr.Zero) return false;
            return uniDiskWriteFloat(_handle, rect.left)
                && uniDiskWriteFloat(_handle, rect.top)
                && uniDiskWriteFloat(_handle, rect.right)
                && uniDiskWriteFloat(_handle, rect.bottom);
        }

        public bool Write(RvRectF64 rect)
        {
            if (_handle == IntPtr.Zero) return false;
            return uniDiskWriteDouble(_handle, rect.left)
                && uniDiskWriteDouble(_handle, rect.top)
                && uniDiskWriteDouble(_handle, rect.right)
                && uniDiskWriteDouble(_handle, rect.bottom);
        }

        public bool Write(int left, int top, int right, int bottom)
        {
            if (_handle == IntPtr.Zero) return false;
            return uniDiskWriteInt(_handle, left)
                && uniDiskWriteInt(_handle, top)
                && uniDiskWriteInt(_handle, right)
                && uniDiskWriteInt(_handle, bottom);
        }

        public bool Write(IntPtr pData, int size)
            => _handle != IntPtr.Zero && uniDiskWriteData(_handle, pData, size);

        // ====================================================================
        // Read helpers
        // ====================================================================

        public bool Read(ref int left, ref int top, ref int right, ref int bottom)
        {
            if (_handle == IntPtr.Zero) return false;
            return uniDiskReadInt(_handle, ref left)
                && uniDiskReadInt(_handle, ref top)
                && uniDiskReadInt(_handle, ref right)
                && uniDiskReadInt(_handle, ref bottom);
        }

        public bool Read(ref int val) => _handle != IntPtr.Zero && uniDiskReadInt(_handle, ref val);
        public bool Read(ref char val) => _handle != IntPtr.Zero && uniDiskReadChar(_handle, ref val);
        public bool Read(ref short val) => _handle != IntPtr.Zero && uniDiskReadShort(_handle, ref val);
        public bool Read(ref ushort val) => _handle != IntPtr.Zero && uniDiskReadUShort(_handle, ref val);
        public bool Read(ref bool val) => _handle != IntPtr.Zero && uniDiskReadBool(_handle, ref val);

        public bool Read(ref string val)
        {
            if (_handle == IntPtr.Zero) return false;
            const int maxlen = 2408;
            StringBuilder sb = new StringBuilder(maxlen);
            if (uniDiskReadString(_handle, sb, maxlen))
            {
                val = sb.ToString();
                return true;
            }
            return false;
        }

        public bool ReadE1(ref string val)
        {
            if (_handle == IntPtr.Zero) return false;
            int len = 0;
            if (!uniDiskReadInt(_handle, ref len)) return false;
            if (len == 0) { val = null; return true; }

            StringBuilder sb = new StringBuilder(len + 1);
            if (uniDiskReadString(_handle, sb, len + 1))
            {
                val = sb.ToString();
                return true;
            }
            return false;
        }

        public bool Read(ref float val) => _handle != IntPtr.Zero && uniDiskReadFloat(_handle, ref val);
        public bool Read(ref double val) => _handle != IntPtr.Zero && uniDiskReadDouble(_handle, ref val);
        public bool Read(ref uint val) => _handle != IntPtr.Zero && uniDiskReadUInt(_handle, ref val);
        public bool Read(ref long val) => _handle != IntPtr.Zero && uniDiskReadInt64(_handle, ref val);
        public bool Read(ref ulong val) => _handle != IntPtr.Zero && uniDiskReadUInt64(_handle, ref val);
        public bool Read(ref RvPoint val) => _handle != IntPtr.Zero && uniDiskReadPoint(_handle, ref val);
        public bool Read(ref RvPointF32 val) => _handle != IntPtr.Zero && uniDiskReadPointF(_handle, ref val);
        public bool Read(ref RvPointF64 val) => _handle != IntPtr.Zero && uniDiskReadPoint64F(_handle, ref val);

        public bool Read(ref RvRect rect)
        {
            if (_handle == IntPtr.Zero) return false;
            int le = 0, to = 0, ri = 0, bo = 0;
            if (!uniDiskReadInt(_handle, ref le)) return false;
            if (!uniDiskReadInt(_handle, ref to)) return false;
            if (!uniDiskReadInt(_handle, ref ri)) return false;
            if (!uniDiskReadInt(_handle, ref bo)) return false;
            rect.left = le; rect.top = to; rect.right = ri; rect.bottom = bo;
            return true;
        }

        public bool Read(ref RvRectF32 rect)
        {
            if (_handle == IntPtr.Zero) return false;
            float le = 0, to = 0, ri = 0, bo = 0;
            if (!uniDiskReadFloat(_handle, ref le)) return false;
            if (!uniDiskReadFloat(_handle, ref to)) return false;
            if (!uniDiskReadFloat(_handle, ref ri)) return false;
            if (!uniDiskReadFloat(_handle, ref bo)) return false;
            rect.left = le; rect.top = to; rect.right = ri; rect.bottom = bo;
            return true;
        }

        public bool Read(ref RvRectF64 rect)
        {
            if (_handle == IntPtr.Zero) return false;
            double le = 0, to = 0, ri = 0, bo = 0;
            if (!uniDiskReadDouble(_handle, ref le)) return false;
            if (!uniDiskReadDouble(_handle, ref to)) return false;
            if (!uniDiskReadDouble(_handle, ref ri)) return false;
            if (!uniDiskReadDouble(_handle, ref bo)) return false;
            rect.left = le; rect.top = to; rect.right = ri; rect.bottom = bo;
            return true;
        }

        public bool Read(IntPtr pData, uint size)
            => _handle != IntPtr.Zero && uniDiskReadData(_handle, pData, size);

        public bool Read(IntPtr pData, ref uint size)
            => _handle != IntPtr.Zero && uniDiskReadDataE1(_handle, pData, ref size);

        // ====================================================================
        // Struct-based serialization (via reflection)
        // ====================================================================

        /// <summary>
        /// Reads the public fields of a value-type object from the file.
        /// Supported field types: enum, bool, char, int, uint, short, ulong,
        /// float, double, string, RvPoint, RvPointF32, RvPointF64, RvRect,
        /// RvRectF32, RvRectF64.
        /// </summary>
        public bool Read(ref object obj)
        {
            if (obj == null || _handle == IntPtr.Zero) return false;

            Type structType = obj.GetType();
            if (!structType.IsValueType) return false;

            // Skip unsupported special types
            if (structType == typeof(DateTime) ||
                structType == typeof(DateTimeOffset) ||
                structType == typeof(TimeSpan) ||
                structType == typeof(Guid) ||
                structType == typeof(decimal))
                return false;

            var fields = structType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                Type type = field.FieldType;

                if (type.IsEnum)
                {
                    int n = 0;
                    if (!Read(ref n)) return false;
                    field.SetValueDirect(__makeref(obj), Enum.ToObject(type, n));
                }
                else if (type == typeof(bool)) { bool v = false; if (!Read(ref v)) return false; field.SetValueDirect(__makeref(obj), v); }
                else if (type == typeof(char)) { char v = ' '; if (!Read(ref v)) return false; field.SetValueDirect(__makeref(obj), v); }
                else if (type == typeof(int)) { int v = 0; if (!Read(ref v)) return false; field.SetValueDirect(__makeref(obj), v); }
                else if (type == typeof(uint)) { uint v = 0; if (!Read(ref v)) return false; field.SetValueDirect(__makeref(obj), v); }
                else if (type == typeof(short)) { short v = 0; if (!Read(ref v)) return false; field.SetValueDirect(__makeref(obj), v); }
                else if (type == typeof(ulong)) { ulong v = 0; if (!Read(ref v)) return false; field.SetValueDirect(__makeref(obj), v); }
                else if (type == typeof(float)) { float v = 0; if (!Read(ref v)) return false; field.SetValueDirect(__makeref(obj), v); }
                else if (type == typeof(double)) { double v = 0; if (!Read(ref v)) return false; field.SetValueDirect(__makeref(obj), v); }
                else if (type == typeof(string)) { string v = ""; if (!Read(ref v)) return false; field.SetValueDirect(__makeref(obj), v); }
                else if (type == typeof(RvPoint)) { RvPoint v = new RvPoint(); if (!Read(ref v)) return false; field.SetValueDirect(__makeref(obj), v); }
                else if (type == typeof(RvPointF32)) { RvPointF32 v = new RvPointF32(); if (!Read(ref v)) return false; field.SetValueDirect(__makeref(obj), v); }
                else if (type == typeof(RvPointF64)) { RvPointF64 v = new RvPointF64(); if (!Read(ref v)) return false; field.SetValueDirect(__makeref(obj), v); }
                else if (type == typeof(RvRect)) { RvRect v = new RvRect(); if (!Read(ref v)) return false; field.SetValueDirect(__makeref(obj), v); }
                else if (type == typeof(RvRectF32)) { RvRectF32 v = new RvRectF32(); if (!Read(ref v)) return false; field.SetValueDirect(__makeref(obj), v); }
                else if (type == typeof(RvRectF64)) { RvRectF64 v = new RvRectF64(); if (!Read(ref v)) return false; field.SetValueDirect(__makeref(obj), v); }
            }
            return true;
        }

        /// <summary>
        /// Writes the public fields of a value-type object to the file.
        /// See <see cref="Read(ref object)"/> for the list of supported field types.
        /// </summary>
        public bool Write(object obj)
        {
            if (obj == null || _handle == IntPtr.Zero) return false;

            Type structType = obj.GetType();
            if (!structType.IsValueType) return false;

            if (structType == typeof(DateTime) ||
                structType == typeof(DateTimeOffset) ||
                structType == typeof(TimeSpan) ||
                structType == typeof(Guid) ||
                structType == typeof(decimal))
                return false;

            var fields = structType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                Type type = field.FieldType;

                if (type.IsEnum) { if (!Write((int)field.GetValue(obj))) return false; }
                else if (type == typeof(bool)) { if (!Write((bool)field.GetValue(obj))) return false; }
                else if (type == typeof(char)) { if (!Write((char)field.GetValue(obj))) return false; }
                else if (type == typeof(int)) { if (!Write((int)field.GetValue(obj))) return false; }
                else if (type == typeof(uint)) { if (!Write((uint)field.GetValue(obj))) return false; }
                else if (type == typeof(short)) { if (!Write((short)field.GetValue(obj))) return false; }
                else if (type == typeof(ulong)) { if (!Write((ulong)field.GetValue(obj))) return false; }
                else if (type == typeof(float)) { if (!Write((float)field.GetValue(obj))) return false; }
                else if (type == typeof(double)) { if (!Write((double)field.GetValue(obj))) return false; }
                else if (type == typeof(string)) { string s = (string)field.GetValue(obj) ?? ""; if (!Write(s)) return false; }
                else if (type == typeof(RvPoint)) { if (!Write((RvPoint)field.GetValue(obj))) return false; }
                else if (type == typeof(RvPointF32)) { if (!Write((RvPointF32)field.GetValue(obj))) return false; }
                else if (type == typeof(RvPointF64)) { if (!Write((RvPointF64)field.GetValue(obj))) return false; }
                else if (type == typeof(RvRect)) { if (!Write((RvRect)field.GetValue(obj))) return false; }
                else if (type == typeof(RvRectF32)) { if (!Write((RvRectF32)field.GetValue(obj))) return false; }
                else if (type == typeof(RvRectF64)) { if (!Write((RvRectF64)field.GetValue(obj))) return false; }
            }
            return true;
        }
    }
}
