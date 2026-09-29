//**************************************************************************************************
//Copyright (c) 2022-2026 
//Shenzhen SoftStorm Technology Co., Limited  
//All rights reserved.

//Licensed under the MIT license. See LICENCE file in the project root for full license information.
//***************************************************************************************************
using System;
using System.Runtime.InteropServices;
using System.Text;

using Kingpool.Utility;

namespace Kingpool.Core
{
    /// <summary>
    /// Represents a flexible data object (FDOX) that wraps an unmanaged handle.
    /// Inherits from KingsHandler for deterministic resource release.
    /// </summary>
    public class KFdox : KingsHandler
    {
        // ====================================================================
        // Type tag constants (RVF_TYPE_*)
        // ====================================================================

        public const int RVF_TYPE_NULL = 0;
        public const int RVF_TYPE_BOOL = 1;
        public const int RVF_TYPE_INT32 = 2;
        public const int RVF_TYPE_INT64 = 3;
        public const int RVF_TYPE_DOUBLE = 4;
        public const int RVF_TYPE_STRING = 5;
        public const int RVF_TYPE_TUPLE = 6;

        // ====================================================================
        // DLL import constants
        // ====================================================================

#if DEBUGGING_KINGPOOL_SUITE
        private const string LIB_NAME = "Fdox_d.dll";
#else
        private const string LIB_NAME = "Fdox.dll";
#endif

        // ====================================================================
        // String conversion helpers (based on CAP.CharSet)
        // ====================================================================

        private static byte[] StringToBytes(string str)
        {
            if (str == null) str = string.Empty;
            string withNull = str + "\0";
            if (CAP.CharSet == CharEncoding.Utf8)
                return Encoding.UTF8.GetBytes(withNull);
            else
                return Encoding.Default.GetBytes(withNull);
        }

        private static string PtrToString(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero) return null;

            if (CAP.CharSet == CharEncoding.Utf8)
            {
                int len = 0;
                while (Marshal.ReadByte(ptr, len) != 0) len++;
                if (len == 0) return string.Empty;
                byte[] bytes = new byte[len];
                Marshal.Copy(ptr, bytes, 0, len);
                return Encoding.UTF8.GetString(bytes);
            }
            else
            {
                return Marshal.PtrToStringAnsi(ptr);
            }
        }

        private static string BufferToString(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return null;
            int len = Array.IndexOf(buffer, (byte)0);
            if (len < 0) len = buffer.Length;
            if (len == 0) return string.Empty;
            if (CAP.CharSet == CharEncoding.Utf8)
                return Encoding.UTF8.GetString(buffer, 0, len);
            else
                return Encoding.Default.GetString(buffer, 0, len);
        }

        // ====================================================================
        // Private unmanaged imports
        // ====================================================================

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxCreate", CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr rdxCreate();

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxDestroy", CallingConvention = CallingConvention.Winapi)]
        private static extern void rdxDestroy(IntPtr h);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxClear", CallingConvention = CallingConvention.Winapi)]
        private static extern void rdxClear(IntPtr h);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxClone", CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr rdxClone(IntPtr h);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxCopy", CallingConvention = CallingConvention.Winapi)]
        private static extern void rdxCopy(IntPtr src, IntPtr dst);
        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxGetType", CallingConvention = CallingConvention.Winapi)]
        private static extern int rdxGetType(IntPtr h);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxIsNull", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxIsNull(IntPtr h);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxIsBool", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxIsBool(IntPtr h);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxIsInt32", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxIsInt32(IntPtr h);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxIsInt64", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxIsInt64(IntPtr h);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxIsDouble", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxIsDouble(IntPtr h);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxIsString", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxIsString(IntPtr h);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxIsNumber", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxIsNumber(IntPtr h);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxIsTuple", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxIsTuple(IntPtr h);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxSetNull", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxSetNull(IntPtr h);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxSetBool", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxSetBool(IntPtr h, [MarshalAs(UnmanagedType.Bool)] bool v);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxSetInt32", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxSetInt32(IntPtr h, int v);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxSetInt64", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxSetInt64(IntPtr h, long v);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxSetDouble", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxSetDouble(IntPtr h, double v);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxSetString", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxSetString(IntPtr h, byte[] v);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxGetBool", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxGetBool(IntPtr h, [MarshalAs(UnmanagedType.Bool)] out bool v);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxGetInt32", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxGetInt32(IntPtr h, out int v);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxGetInt64", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxGetInt64(IntPtr h, out long v);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxGetDouble", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxGetDouble(IntPtr h, out double v);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxGetString", CallingConvention = CallingConvention.Winapi)]
        private static extern int rdxGetString(IntPtr h, byte[] buf, int bufSize);

        // ---- Index-based getters ----

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxGetBoolAt", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxGetBoolAt(IntPtr h, int index, [MarshalAs(UnmanagedType.Bool)] out bool v);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxGetInt32At", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxGetInt32At(IntPtr h, int index, out int v);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxGetInt64At", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxGetInt64At(IntPtr h, int index, out long v);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxGetDoubleAt", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxGetDoubleAt(IntPtr h, int index, out double v);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxGetStringAt", CallingConvention = CallingConvention.Winapi)]
        private static extern int rdxGetStringAt(IntPtr h, int index, byte[] buf, int bufSize);

        // ---- Index-based setters ----

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxSetNullAt", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxSetNullAt(IntPtr h, int index);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxSetBoolAt", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxSetBoolAt(IntPtr h, int index, [MarshalAs(UnmanagedType.Bool)] bool v);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxSetInt32At", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxSetInt32At(IntPtr h, int index, int v);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxSetInt64At", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxSetInt64At(IntPtr h, int index, long v);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxSetDoubleAt", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxSetDoubleAt(IntPtr h, int index, double v);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxSetStringAt", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxSetStringAt(IntPtr h, int index, byte[] v);

        // ---- Tuple metadata ----

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxGetCount", CallingConvention = CallingConvention.Winapi)]
        private static extern int rdxGetCount(IntPtr h);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxGetName", CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr rdxGetName(IntPtr h, int index);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxSetName", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxSetName(IntPtr h, int index, byte[] name);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxFindIndex", CallingConvention = CallingConvention.Winapi)]
        private static extern int rdxFindIndex(IntPtr h, byte[] name);

        // ---- Append helpers (return borrowed child handle) ----

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxAppendNull", CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr rdxAppendNull(IntPtr h, byte[] name);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxAppendBool", CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr rdxAppendBool(IntPtr h, byte[] name, [MarshalAs(UnmanagedType.Bool)] bool v);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxAppendInt32", CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr rdxAppendInt32(IntPtr h, byte[] name, int v);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxAppendInt64", CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr rdxAppendInt64(IntPtr h, byte[] name, long v);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxAppendDouble", CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr rdxAppendDouble(IntPtr h, byte[] name, double v);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxAppendString", CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr rdxAppendString(IntPtr h, byte[] name, byte[] v);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxAppendTuple", CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr rdxAppendTuple(IntPtr h, byte[] name);

        // ---- Reverse lookup ----

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxGetIndex", CallingConvention = CallingConvention.Winapi)]
        private static extern int rdxGetIndex(IntPtr parent, IntPtr child);

        // ---- Child access ----

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxGetChild", CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr rdxGetChild(IntPtr h, int index);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxFindChild", CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr rdxFindChild(IntPtr h, byte[] name);

        // ---- Iterators ----

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxIterFirst", CallingConvention = CallingConvention.Winapi)]
        private static extern int rdxIterFirst(IntPtr h);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxIterNext", CallingConvention = CallingConvention.Winapi)]
        private static extern int rdxIterNext(IntPtr h, int currentIndex);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxIterLast", CallingConvention = CallingConvention.Winapi)]
        private static extern int rdxIterLast(IntPtr h);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxIterPrev", CallingConvention = CallingConvention.Winapi)]
        private static extern int rdxIterPrev(IntPtr h, int currentIndex);

        // ---- Path query ----

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxQuery", CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr rdxQuery(IntPtr root, byte[] path);

        // ---- JSON / Binary serialization ----

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxToJson", CallingConvention = CallingConvention.Winapi)]
        private static extern int rdxToJson(IntPtr h, byte[] buf, int bufSize, [MarshalAs(UnmanagedType.Bool)] bool pretty);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxFromJson", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxFromJson(IntPtr h, byte[] text);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxToBinary", CallingConvention = CallingConvention.Winapi)]
        private static extern int rdxToBinary(IntPtr h, IntPtr buf, int bufSize);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rdxFromBinary", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rdxFromBinary(IntPtr h, IntPtr data, int dataSize);

        // ====================================================================
        // Constructors
        // ====================================================================

        public KFdox()
        {
            _handle = rdxCreate();
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create KFdox.");
            _attached = false;
        }

        public KFdox(IntPtr existingHandle, bool attach)
        {
            if (existingHandle == IntPtr.Zero)
                throw new ArgumentException("Invalid Fdox handle", nameof(existingHandle));
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
                rdxDestroy(_handle);
            }
        }

        // ====================================================================
        // Public methods
        // ====================================================================

        public void Clear() => rdxClear(_handle);

        public KFdox Clone()
        {
            IntPtr newHandle = rdxClone(_handle);
            return newHandle != IntPtr.Zero ? new KFdox(newHandle, false) : null;
        }

        public void CopyTo(KFdox other)
        {
            if (null == other) return;
            rdxCopy(_handle, other.Handle);
        }

        public int GetFdoxType() => rdxGetType(_handle);
        public bool IsNull() => rdxIsNull(_handle);
        public bool IsBool() => rdxIsBool(_handle);
        public bool IsInt32() => rdxIsInt32(_handle);
        public bool IsInt64() => rdxIsInt64(_handle);
        public bool IsDouble() => rdxIsDouble(_handle);
        public bool IsString() => rdxIsString(_handle);
        public bool IsNumber() => rdxIsNumber(_handle);
        public bool IsTuple() => rdxIsTuple(_handle);

        // ---- Scalar setters (self) ----

        public bool SetNull() => rdxSetNull(_handle);
        public bool SetBool(bool v) => rdxSetBool(_handle, v);
        public bool SetInt32(int v) => rdxSetInt32(_handle, v);
        public bool SetInt64(long v) => rdxSetInt64(_handle, v);
        public bool SetDouble(double v) => rdxSetDouble(_handle, v);

        public bool SetString(string v)
        {
            byte[] bytes = StringToBytes(v);
            return rdxSetString(_handle, bytes);
        }

        // ---- Scalar getters (self) ----

        public bool GetBool(out bool v) => rdxGetBool(_handle, out v);
        public bool GetInt32(out int v) => rdxGetInt32(_handle, out v);
        public bool GetInt64(out long v) => rdxGetInt64(_handle, out v);
        public bool GetDouble(out double v) => rdxGetDouble(_handle, out v);

        public string GetString()
        {
            int required = rdxGetString(_handle, null, 0);
            if (required <= 0) return null;
            byte[] buf = new byte[required + 1];
            int written = rdxGetString(_handle, buf, buf.Length);
            if (written <= 0) return null;
            return BufferToString(buf);
        }

        // ---- Scalar getters by index ----

        public bool GetBoolAt(int index, out bool v) => rdxGetBoolAt(_handle, index, out v);
        public bool GetInt32At(int index, out int v) => rdxGetInt32At(_handle, index, out v);
        public bool GetInt64At(int index, out long v) => rdxGetInt64At(_handle, index, out v);
        public bool GetDoubleAt(int index, out double v) => rdxGetDoubleAt(_handle, index, out v);

        public string GetStringAt(int index)
        {
            int required = rdxGetStringAt(_handle, index, null, 0);
            if (required <= 0) return null;
            byte[] buf = new byte[required + 1];
            int written = rdxGetStringAt(_handle, index, buf, buf.Length);
            if (written <= 0) return null;
            return BufferToString(buf);
        }

        // ---- Scalar setters by index ----

        public bool SetNullAt(int index) => rdxSetNullAt(_handle, index);
        public bool SetBoolAt(int index, bool v) => rdxSetBoolAt(_handle, index, v);
        public bool SetInt32At(int index, int v) => rdxSetInt32At(_handle, index, v);
        public bool SetInt64At(int index, long v) => rdxSetInt64At(_handle, index, v);
        public bool SetDoubleAt(int index, double v) => rdxSetDoubleAt(_handle, index, v);

        public bool SetStringAt(int index, string v)
        {
            byte[] bytes = StringToBytes(v);
            return rdxSetStringAt(_handle, index, bytes);
        }

        // ---- Tuple metadata ----

        public int GetCount() => rdxGetCount(_handle);

        public string GetName(int index)
        {
            IntPtr ptr = rdxGetName(_handle, index);
            return PtrToString(ptr);
        }

        public bool SetName(int index, string name)
        {
            byte[] bytes = StringToBytes(name);
            return rdxSetName(_handle, index, bytes);
        }

        public int FindIndex(string name)
        {
            byte[] bytes = StringToBytes(name);
            return rdxFindIndex(_handle, bytes);
        }

        // ---- Append helpers (return borrowed child handle) ----

        public KFdox AppendNull(string name)
        {
            byte[] bytes = StringToBytes(name);
            IntPtr h = rdxAppendNull(_handle, bytes);
            return h != IntPtr.Zero ? new KFdox(h, true) : null;
        }

        public KFdox AppendBool(string name, bool v)
        {
            byte[] bytes = StringToBytes(name);
            IntPtr h = rdxAppendBool(_handle, bytes, v);
            return h != IntPtr.Zero ? new KFdox(h, true) : null;
        }

        public KFdox AppendInt32(string name, int v)
        {
            byte[] bytes = StringToBytes(name);
            IntPtr h = rdxAppendInt32(_handle, bytes, v);
            return h != IntPtr.Zero ? new KFdox(h, true) : null;
        }

        public KFdox AppendInt64(string name, long v)
        {
            byte[] bytes = StringToBytes(name);
            IntPtr h = rdxAppendInt64(_handle, bytes, v);
            return h != IntPtr.Zero ? new KFdox(h, true) : null;
        }

        public KFdox AppendDouble(string name, double v)
        {
            byte[] bytes = StringToBytes(name);
            IntPtr h = rdxAppendDouble(_handle, bytes, v);
            return h != IntPtr.Zero ? new KFdox(h, true) : null;
        }

        public KFdox AppendString(string name, string v)
        {
            byte[] nameBytes = StringToBytes(name);
            byte[] valueBytes = StringToBytes(v);
            IntPtr h = rdxAppendString(_handle, nameBytes, valueBytes);
            return h != IntPtr.Zero ? new KFdox(h, true) : null;
        }

        public KFdox AppendTuple(string name)
        {
            byte[] bytes = StringToBytes(name);
            IntPtr h = rdxAppendTuple(_handle, bytes);
            return h != IntPtr.Zero ? new KFdox(h, true) : null;
        }

        // ---- Reverse lookup ----

        /// <summary>
        /// Returns the index of a child within this parent, or -1 if not found.
        /// </summary>
        public int GetIndex(KFdox child)
        {
            if (child == null || child.Handle == IntPtr.Zero) return -1;
            return rdxGetIndex(_handle, child.Handle);
        }

        // ---- Child access (borrowed handles) ----

        public KFdox GetChild(int index)
        {
            IntPtr childHandle = rdxGetChild(_handle, index);
            if (childHandle == IntPtr.Zero) return null;
            return new KFdox(childHandle, true);
        }

        public KFdox FindChild(string name)
        {
            byte[] bytes = StringToBytes(name);
            IntPtr childHandle = rdxFindChild(_handle, bytes);
            if (childHandle == IntPtr.Zero) return null;
            return new KFdox(childHandle, true);
        }

        // ---- Iterators ----

        public int IterFirst() => rdxIterFirst(_handle);
        public int IterNext(int currentIndex) => rdxIterNext(_handle, currentIndex);
        public int IterLast() => rdxIterLast(_handle);
        public int IterPrev(int currentIndex) => rdxIterPrev(_handle, currentIndex);

        /// <summary>
        /// Enumerates all children of a tuple. The yielded KFdox instances are
        /// borrowed handles (owned by this instance); do not dispose them.
        /// </summary>
        public System.Collections.Generic.IEnumerable<KFdox> GetChildren()
        {
            if (!IsTuple()) yield break;
            int count = GetCount();
            for (int i = 0; i < count; i++)
            {
                yield return GetChild(i);
            }
        }

        // ---- Path query ----

        public KFdox Query(string path)
        {
            byte[] bytes = StringToBytes(path);
            IntPtr childHandle = rdxQuery(_handle, bytes);
            if (childHandle == IntPtr.Zero) return null;
            return new KFdox(childHandle, true);
        }

        // ---- JSON ----

        public string ToJson(bool pretty = false)
        {
            int required = rdxToJson(_handle, null, 0, pretty);
            if (required < 0) return null;
            byte[] buf = new byte[required + 1];
            int written = rdxToJson(_handle, buf, buf.Length, pretty);
            if (written < 0) return null;
            return Encoding.UTF8.GetString(buf, 0, written);
        }

        public bool FromJson(string json)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(json + "\0");
            return rdxFromJson(_handle, bytes);
        }

        // ---- Binary ----

        public byte[] ToBinary()
        {
            int required = rdxToBinary(_handle, IntPtr.Zero, 0);
            if (required < 0) return null;
            byte[] buf = new byte[required];
            IntPtr ptr = Marshal.AllocHGlobal(required);
            try
            {
                int written = rdxToBinary(_handle, ptr, required);
                if (written < 0) return null;
                Marshal.Copy(ptr, buf, 0, written);
                return buf;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public bool FromBinary(byte[] data)
        {
            if (data == null || data.Length == 0) return false;
            IntPtr ptr = Marshal.AllocHGlobal(data.Length);
            try
            {
                Marshal.Copy(data, 0, ptr, data.Length);
                return rdxFromBinary(_handle, ptr, data.Length);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
