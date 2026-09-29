using System;
using System.Runtime.InteropServices;

namespace Kingpool.Core
{
    /// <summary>
    /// Callback function prototype for releasing custom data associated with an element.
    /// Matches: typedef void (WINAPI* RvDestroyFunc) (RV_HANDLE);
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    public delegate void CustomDataReleaseDelegate(IntPtr handle);

    /// <summary>
    /// Represents a dynamic sequence (list) that stores unmanaged handles or pointers.
    /// Inherits from KingsHandler for deterministic resource release.
    /// </summary>
    public class KSequence : KingsHandler
    {
        // ====================================================================
        // Constants (mirroring C header)
        // ====================================================================

        public const int RV_SQ_PREV = 1;
        public const int RV_SQ_NEXT = 2;

        // ====================================================================
        // DLL import constants
        // ====================================================================

#if DEBUG
        private const string LIB_NAME = "dsm_d.dll";
#else
        private const string LIB_NAME = "dsm.dll";
#endif

        // ====================================================================
        // Native functions imports
        // ====================================================================

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCreateSequence")]
        private static extern IntPtr CreateSequence(IntPtr fnDestroyElem);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCreateSequenceEx")]
        private static extern IntPtr CreateSequenceEx(int flag, IntPtr fnDestroyElem);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDestroySequence")]
        private static extern void DestroySequence(IntPtr sequence);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqGetSliceLength")]
        private static extern int GetSliceLength(IntPtr sequence, IntPtr slice);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqAddLast_")]
        private static extern IntPtr AddLast(IntPtr sequence, IntPtr pVal);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqAppend_")]
        private static extern IntPtr Append(IntPtr sequence, IntPtr pVal);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqAddFirst_")]
        private static extern IntPtr AddFirst(IntPtr sequence, IntPtr pVal);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqRemoveLast_")]
        private static extern IntPtr RemoveLast(IntPtr sequence);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqRemoveFirst_")]
        private static extern IntPtr RemoveFirst(IntPtr sequence);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqFindIndex_")]
        private static extern int FindIndex(IntPtr sequence, IntPtr pVal);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqFindIndexEx_")]
        private static extern IntPtr FindIndexEx(IntPtr sequence, IntPtr pVal);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqRemoveMulti")]
        private static extern int RemoveMulti(IntPtr sequence, IntPtr slice, IntPtr[] pElemArray, int nElemCount, bool bReverse);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqExtractToArray")]
        private static extern int ExtractToArray(IntPtr sequence, IntPtr slice, IntPtr[] pElemArray, int nArraySize, bool bReverse);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqInsertAt_")]
        private static extern IntPtr InsertAt(IntPtr sequence, int index, IntPtr pVal);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqInsertBefore_")]
        private static extern IntPtr InsertBefore(IntPtr sequence, int index, IntPtr pVal);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqInsertAfter_")]
        private static extern IntPtr InsertAfter(IntPtr sequence, int index, IntPtr pVal);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqInsertEx_")]
        private static extern IntPtr InsertEx(IntPtr sequence, IntPtr hCurItem, IntPtr pVal, bool bAfter);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqRemoveAt_")]
        private static extern IntPtr RemoveAt(IntPtr sequence, int index);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqRemoveElem_")]
        private static extern void RemoveElem(IntPtr sequence, IntPtr pVal);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqRemoveItem_")]
        private static extern IntPtr RemoveItem(IntPtr sequence, IntPtr hItem);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqGetFirst_")]
        private static extern IntPtr GetFirst(IntPtr sequence);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqClear")]
        private static extern void Clear(IntPtr sequence);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqIsEmpty")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rsqIsEmpty(IntPtr sequence);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqGetCount")]
        private static extern int GetCount(IntPtr sequence);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqGetItemValue_")]
        private static extern IntPtr GetItemValue(IntPtr sequence, int index);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqSetItemValue_")]
        private static extern IntPtr SetItemValue(IntPtr sequence, int index, IntPtr pVal);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqGetItemValueE1_")]
        private static extern IntPtr GetItemValueE1(IntPtr sequence, IntPtr item);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqSetItemValueE1_")]
        private static extern void SetItemValueE1(IntPtr sequence, IntPtr item, IntPtr pVal);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqGetHead")]
        private static extern IntPtr GetHead(IntPtr sequence);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqGetTail")]
        private static extern IntPtr GetTail(IntPtr sequence);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqGetItemAt")]
        private static extern IntPtr GetItemAt(IntPtr sequence, int index);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqGetNextItem")]
        private static extern IntPtr GetNextItem(IntPtr sequence, IntPtr item, int nCode);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rsqMerge")]
        private static extern int Merge(IntPtr sequence, IntPtr income);

        // ====================================================================
        // Private fields
        // ====================================================================

        // Hold reference to destroy delegate to prevent GC collection
        private CustomDataReleaseDelegate _destroyFunc = null;

        // ====================================================================
        // Constructors
        // ====================================================================

        /// <summary>
        /// Creates a new empty sequence with no destroy callback.
        /// </summary>
        public KSequence()
        {
            _handle = CreateSequence(IntPtr.Zero);
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create KSequence.");
            _attached = false;
            _destroyFunc = null;
        }

        /// <summary>
        /// Creates a new empty sequence with a custom destroy function for elements.
        /// </summary>
        /// <param name="fnDestroyElem">Destroy callback (can be null).</param>
        public KSequence(CustomDataReleaseDelegate customDataReleaseDelegate)
        {
            // Store delegate reference to prevent GC
            _destroyFunc = customDataReleaseDelegate;

            IntPtr pfn = customDataReleaseDelegate != null ? Marshal.GetFunctionPointerForDelegate(customDataReleaseDelegate) : IntPtr.Zero;
            _handle = CreateSequence(pfn);
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create KSequence with destroy function.");
            _attached = false;
        }

        /// <summary>
        /// Creates a new empty sequence with flags and optional destroy function.
        /// </summary>
        /// <param name="flag">Creation flags.</param>
        /// <param name="fnDestroyElem">Destroy callback (can be null).</param>
        public KSequence(int flag, CustomDataReleaseDelegate destroyElementDelegate = null)
        {
            _destroyFunc = destroyElementDelegate;
            IntPtr pfn = destroyElementDelegate != null ? Marshal.GetFunctionPointerForDelegate(destroyElementDelegate) : IntPtr.Zero;
            _handle = CreateSequenceEx(flag, pfn);
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create KSequence with flags.");
            _attached = false;
        }

        /// <summary>
        /// Wraps an existing sequence handle (ownership determined by attach flag).
        /// </summary>
        /// <param name="existingHandle">Existing sequence handle.</param>
        /// <param name="attach">True if handle is owned externally (no destruction on dispose).</param>
        public KSequence(IntPtr existingHandle, bool attach)
        {
            if (existingHandle == IntPtr.Zero)
                throw new ArgumentException("Invalid sequence handle", nameof(existingHandle));
            _handle = existingHandle;
            _attached = attach;
            _destroyFunc = null; // External handle, no destroy callback.
        }



        // ====================================================================
        // Sealed ReleaseHandle implementation
        // ====================================================================

        protected sealed override void ReleaseHandle()
        {
            if (_handle != IntPtr.Zero && !_attached)
            {
                DestroySequence(_handle);
            }
        }

        // ====================================================================
        // Public properties and methods
        // ====================================================================

        /// <summary>Gets the number of elements in the sequence.</summary>
        public int Count => _handle != IntPtr.Zero ? GetCount(_handle) : 0;

        /// <summary>Checks if the sequence is empty.</summary>
        public bool IsEmpty => _handle != IntPtr.Zero && rsqIsEmpty(_handle);

        /// <summary>Clears all elements from the sequence.</summary>
        public void Clear()
        {
            if (_handle != IntPtr.Zero)
                Clear(_handle);
        }

        public IntPtr GetAt(int index)
        {
            if (_handle != IntPtr.Zero)
                return GetItemValue(_handle, index);

            return IntPtr.Zero;
        }

        public void SetAt(int index, IntPtr hValue)
        {
            if (_handle != IntPtr.Zero)
                SetItemValue(_handle, index, hValue);

        }
        /// <summary>Adds an element to the end of the sequence.</summary>
        public IntPtr AddLast(IntPtr value)
        {
            if (_handle == IntPtr.Zero) return IntPtr.Zero;
            return AddLast(_handle, value);
        }

        /// <summary>Appends an element to the end (same as AddLast).</summary>
        public IntPtr Append(IntPtr value) => AddLast(value);

        /// <summary>Adds an element to the beginning of the sequence.</summary>
        public IntPtr AddFirst(IntPtr value)
        {
            if (_handle == IntPtr.Zero) return IntPtr.Zero;
            return AddFirst(_handle, value);
        }

        /// <summary>Removes the last element and returns its value.</summary>
        public IntPtr RemoveLast()
        {
            if (_handle == IntPtr.Zero) return IntPtr.Zero;
            return RemoveLast(_handle);
        }

        /// <summary>Removes the first element and returns its value.</summary>
        public IntPtr RemoveFirst()
        {
            if (_handle == IntPtr.Zero) return IntPtr.Zero;
            return RemoveFirst(_handle);
        }

        /// <summary>Finds the index of an element by value.</summary>
        public int FindIndex(IntPtr value)
        {
            if (_handle == IntPtr.Zero) return -1;
            return FindIndex(_handle, value);
        }

        /// <summary>Finds an element by value and returns its item handle.</summary>
        public IntPtr FindItem(IntPtr value)
        {
            if (_handle == IntPtr.Zero) return IntPtr.Zero;
            return FindIndexEx(_handle, value);
        }

        /// <summary>Inserts an element after the specified index.</summary>
        public IntPtr InsertAt(int index, IntPtr value)
        {
            if (_handle == IntPtr.Zero) return IntPtr.Zero;
            return InsertAt(_handle, index, value);
        }

        /// <summary>Inserts an element before the specified index.</summary>
        public IntPtr InsertBefore(int index, IntPtr value)
        {
            if (_handle == IntPtr.Zero) return IntPtr.Zero;
            return InsertBefore(_handle, index, value);
        }

        /// <summary>Inserts an element after the specified index.</summary>
        public IntPtr InsertAfter(int index, IntPtr value)
        {
            if (_handle == IntPtr.Zero) return IntPtr.Zero;
            return InsertAfter(_handle, index, value);
        }

        /// <summary>Inserts an element relative to an existing item.</summary>
        public IntPtr InsertEx(IntPtr hCurItem, IntPtr value, bool after = false)
        {
            if (_handle == IntPtr.Zero) return IntPtr.Zero;
            return InsertEx(_handle, hCurItem, value, after);
        }

        /// <summary>Removes the element at the specified index and returns the next item.</summary>
        public IntPtr RemoveAt(int index)
        {
            if (_handle == IntPtr.Zero) return IntPtr.Zero;
            return RemoveAt(_handle, index);
        }

        /// <summary>Removes the first occurrence of an element by its value.</summary>
        public void Remove(IntPtr value)
        {
            if (_handle != IntPtr.Zero)
                RemoveElem(_handle, value);
        }

        /// <summary>Removes an element by its item handle.</summary>
        //public IntPtr RemoveItem(IntPtr hItem)
        //{
        //    if (_handle == IntPtr.Zero) return IntPtr.Zero;
        //    return RemoveItem(_handle, hItem);
        //}

        /// <summary>Gets the value of the element at the specified index.</summary>
        //public IntPtr GetItemValue(int index)
        //{
        //    if (_handle == IntPtr.Zero) return IntPtr.Zero;
        //    return GetItemValue(_handle, index);
        //}

        /// <summary>Sets the value of the element at the specified index.</summary>
        //public IntPtr SetItemValue(int index, IntPtr value)
        //{
        //    if (_handle == IntPtr.Zero) return IntPtr.Zero;
        //    return SetItemValue(_handle, index, value);
        //}

        /// <summary>Gets the value of an element by its item handle.</summary>
        //public IntPtr GetItemValue(IntPtr hItem)
        //{
        //    if (_handle == IntPtr.Zero) return IntPtr.Zero;
        //    return GetItemValueE1(_handle, hItem);
        //}

        ///// <summary>Sets the value of an element by its item handle.</summary>
        //public void SetItemValue(IntPtr hItem, IntPtr value)
        //{
        //    if (_handle != IntPtr.Zero)
        //        SetItemValueE1(_handle, hItem, value);
        //}

        /// <summary>Gets the head (first) item handle.</summary>
        public IntPtr Head => _handle != IntPtr.Zero ? GetHead(_handle) : IntPtr.Zero;

        /// <summary>Gets the tail (last) item handle.</summary>
        public IntPtr Tail => _handle != IntPtr.Zero ? GetTail(_handle) : IntPtr.Zero;

        /// <summary>Gets the item handle at the specified index.</summary>
        public IntPtr GetItemAt(int index)
        {
            if (_handle == IntPtr.Zero) return IntPtr.Zero;
            return GetItemAt(_handle, index);
        }

        /// <summary>Gets the next item relative to a given item.</summary>
        public IntPtr GetNextItem(IntPtr hItem, int code)
        {
            if (_handle == IntPtr.Zero) return IntPtr.Zero;
            return GetNextItem(_handle, hItem, code);
        }

        /// <summary>Merges another sequence into this one.</summary>
        public int Merge(KSequence income)
        {
            if (_handle == IntPtr.Zero || income == null || income._handle == IntPtr.Zero)
                return 0;
            return Merge(_handle, income._handle);
        }

        // ====================================================================
        // Advanced operations with slices
        // ====================================================================

        /// <summary>Gets the length of a slice.</summary>
        //public int GetSliceLength(IntPtr slice)
        //{
        //    if (_handle == IntPtr.Zero) return 0;
        //    return GetSliceLength(_handle, slice);
        //}

        /// <summary>Removes multiple elements defined by a slice.</summary>
        //public int RemoveMulti(IntPtr slice, IntPtr[] pElemArray, bool bReverse = false)
        //{
        //    if (_handle == IntPtr.Zero) return 0;
        //    return RemoveMulti(_handle, slice, pElemArray, pElemArray?.Length ?? 0, bReverse);
        //}

        /// <summary>Extracts elements defined by a slice into an array.</summary>
        //public int ExtractToArray(IntPtr slice, IntPtr[] pElemArray, bool bReverse = false)
        //{
        //    if (_handle == IntPtr.Zero) return 0;
        //    return ExtractToArray(_handle, slice, pElemArray, pElemArray?.Length ?? 0, bReverse);
        //}
    }

    /// <summary>
    /// Represents a thread-safe doubly linked list that stores unmanaged handles or pointers.
    /// Inherits from KingsHandler for deterministic resource release.
    /// </summary>
    public class KList : KingsHandler
    {
        // ====================================================================
        // DLL import constants
        // ====================================================================

#if DEBUG
        private const string LIB_NAME = "dsm_d.dll";
#else
        private const string LIB_NAME = "dsm.dll";
#endif

        // ====================================================================
        // Native functions imports
        // ====================================================================

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCreateList")]
        private static extern IntPtr CreateList(IntPtr fnDestroyElem);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDestroyList")]
        private static extern void DestroyList(IntPtr list);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rlsGetHead")]
        private static extern IntPtr GetHead(IntPtr list);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rlsGetTail")]
        private static extern IntPtr GetTail(IntPtr list);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rlsGetCur")]
        private static extern IntPtr GetCur(IntPtr list);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rlsGetAt")]
        private static extern IntPtr GetAt(IntPtr list, int index);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rlsAddHead")]
        private static extern void AddHead(IntPtr list, IntPtr val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rlsAddTail")]
        private static extern void AddTail(IntPtr list, IntPtr val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rlsAdd")]
        private static extern void Add(IntPtr list, IntPtr val);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rlsRemoveAll")]
        private static extern void RemoveAll(IntPtr list);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rlsRemoveHead")]
        private static extern IntPtr RemoveHead(IntPtr list);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rlsRemoveTail")]
        private static extern IntPtr RemoveTail(IntPtr list);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rlsRemoveCur")]
        private static extern IntPtr RemoveCur(IntPtr list);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rlsRemove")]
        private static extern void Remove(IntPtr list, IntPtr pVal);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rlsRemoveAt")]
        private static extern IntPtr RemoveAt(IntPtr list, int index);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rlsGoHead")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GoHead(IntPtr list);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rlsGoTail")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GoTail(IntPtr list);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rlsStep")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Step(IntPtr list, bool bBackward);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rlsGetCount")]
        private static extern int GetCount(IntPtr list);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rlsIsEmpty")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rlsIsEmpty(IntPtr list);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rlsIsBegin")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rlsIsBegin(IntPtr list);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rlsIsEnd")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rlsIsEnd(IntPtr list);

        // ====================================================================
        // Constructors
        // ====================================================================

        /// <summary>
        /// Creates a new empty list with default destroy function (NULL).
        /// </summary>
        public KList()
        {
            _handle = CreateList(IntPtr.Zero);
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create KList.");
            _attached = false;
        }

        /// <summary>
        /// Creates a new empty list with a custom destroy function for elements.
        /// </summary>
        /// <param name="fnDestroyElem">Pointer to a destroy callback (can be IntPtr.Zero).</param>
        public KList(IntPtr fnDestroyElem)
        {
            _handle = CreateList(fnDestroyElem);
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create KList with custom destroy function.");
            _attached = false;
        }

        /// <summary>
        /// Wraps an existing list handle (ownership determined by attach flag).
        /// </summary>
        /// <param name="existingHandle">Existing list handle.</param>
        /// <param name="attach">True if handle is owned externally (no destruction on dispose).</param>
        public KList(IntPtr existingHandle, bool attach)
        {
            if (existingHandle == IntPtr.Zero)
                throw new ArgumentException("Invalid list handle", nameof(existingHandle));
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
                DestroyList(_handle);
            }
        }

        // ====================================================================
        // Public properties and methods
        // ====================================================================

        /// <summary>
        /// Gets the number of elements in the list.
        /// </summary>
        public int Count => _handle != IntPtr.Zero ? GetCount(_handle) : 0;

        /// <summary>
        /// Checks if the list is empty.
        /// </summary>
        public bool IsEmpty => _handle != IntPtr.Zero && rlsIsEmpty(_handle);

        /// <summary>
        /// Gets the head (first) element's value.
        /// </summary>
        public IntPtr Head => _handle != IntPtr.Zero ? GetHead(_handle) : IntPtr.Zero;

        /// <summary>
        /// Gets the tail (last) element's value.
        /// </summary>
        public IntPtr Tail => _handle != IntPtr.Zero ? GetTail(_handle) : IntPtr.Zero;

        /// <summary>
        /// Gets the current element's value.
        /// </summary>
        public IntPtr Current => _handle != IntPtr.Zero ? GetCur(_handle) : IntPtr.Zero;

        /// <summary>
        /// Gets the value at the specified index.
        /// </summary>
        public IntPtr GetAt(int index)
        {
            if (_handle == IntPtr.Zero) return IntPtr.Zero;
            return GetAt(_handle, index);
        }

        /// <summary>
        /// Adds an element to the head of the list.
        /// </summary>
        public void AddHead(IntPtr value)
        {
            if (_handle != IntPtr.Zero)
                AddHead(_handle, value);
        }

        /// <summary>
        /// Adds an element to the tail of the list.
        /// </summary>
        public void AddTail(IntPtr value)
        {
            if (_handle != IntPtr.Zero)
                AddTail(_handle, value);
        }

        /// <summary>
        /// Adds an element (default to tail). Alias of AddTail.
        /// </summary>
        public void Add(IntPtr value) => AddTail(value);

        /// <summary>
        /// Removes all elements from the list.
        /// </summary>
        public void Clear()
        {
            if (_handle != IntPtr.Zero)
                RemoveAll(_handle);
        }

        /// <summary>
        /// Removes and returns the head element.
        /// </summary>
        public IntPtr RemoveHead()
        {
            if (_handle == IntPtr.Zero) return IntPtr.Zero;
            return RemoveHead(_handle);
        }

        /// <summary>
        /// Removes and returns the tail element.
        /// </summary>
        public IntPtr RemoveTail()
        {
            if (_handle == IntPtr.Zero) return IntPtr.Zero;
            return RemoveTail(_handle);
        }

        /// <summary>
        /// Removes and returns the current element.
        /// </summary>
        public IntPtr RemoveCurrent()
        {
            if (_handle == IntPtr.Zero) return IntPtr.Zero;
            return RemoveCur(_handle);
        }

        /// <summary>
        /// Removes the first occurrence of the specified value.
        /// </summary>
        public void Remove(IntPtr value)
        {
            if (_handle != IntPtr.Zero)
                Remove(_handle, value);
        }

        /// <summary>
        /// Removes the element at the specified index and returns its value.
        /// </summary>
        public IntPtr RemoveAt(int index)
        {
            if (_handle == IntPtr.Zero) return IntPtr.Zero;
            return RemoveAt(_handle, index);
        }

        /// <summary>
        /// Moves the internal cursor to the head.
        /// </summary>
        /// <returns>True if successful.</returns>
        public bool GoHead()
        {
            if (_handle == IntPtr.Zero) return false;
            return GoHead(_handle);
        }

        /// <summary>
        /// Moves the internal cursor to the tail.
        /// </summary>
        public bool GoTail()
        {
            if (_handle == IntPtr.Zero) return false;
            return GoTail(_handle);
        }

        /// <summary>
        /// Steps the internal cursor forward or backward.
        /// </summary>
        /// <param name="backward">True to move backward, false to move forward.</param>
        public bool Step(bool backward = false)
        {
            if (_handle == IntPtr.Zero) return false;
            return Step(_handle, backward);
        }

        /// <summary>
        /// Checks if the internal cursor is at the beginning.
        /// </summary>
        public bool IsBegin => _handle != IntPtr.Zero && rlsIsBegin(_handle);

        /// <summary>
        /// Checks if the internal cursor is at the end.
        /// </summary>
        public bool IsEnd => _handle != IntPtr.Zero && rlsIsEnd(_handle);
    }

    /// <summary>
    /// Represents a generic tree structure that stores unmanaged handles or pointers.
    /// Inherits from KingsHandler for deterministic resource release.
    /// </summary>
    public class KTree : KingsHandler
    {
        // ====================================================================
        // DLL import constants
        // ====================================================================

#if DEBUG
        private const string LIB_NAME = "dsm_d.dll";
#else
        private const string LIB_NAME = "dsm.dll";
#endif

        // ====================================================================
        // Constants (mirroring C header)
        // ====================================================================

        public const int RTR_ROOT = 0;     // Root (for insertion)
        public const int RTR_PARENT = 1;   // Parent relation
        public const int RTR_CHILD = 2;    // First child
        public const int RTR_SIBLING = 3;  // Next sibling (same as RTR_NEXT)
        public const int RTR_NEXT = 3;
        public const int RTR_PREV = 4;     // Previous sibling
        public const int RTR_RIGHT = 3;
        public const int RTR_LEFT = 4;
        public const int RTR_LAST = 5;
        public const int RTR_FIRST = 6;

        // ====================================================================
        // Native functions imports
        // ====================================================================

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvCreateTree")]
        private static extern IntPtr CreateTree(IntPtr fnDestroyElem);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rvDestroyTree")]
        private static extern void DestroyTree(IntPtr tree);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rtrClear")]
        private static extern void Clear(IntPtr tree);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rtrGetCount")]
        private static extern int GetCount(IntPtr tree);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rtrGetRoot")]
        private static extern IntPtr GetRoot(IntPtr tree);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rtrGetChildItem")]
        private static extern IntPtr GetChildItem(IntPtr hItem);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rtrGetNextItem")]
        private static extern IntPtr GetNextItem(IntPtr hItem);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rtrGetParentItem")]
        private static extern IntPtr GetParentItem(IntPtr hItem);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rtrRemove")]
        private static extern void Remove(IntPtr tree, IntPtr hItem);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rtrInsert")]
        private static extern IntPtr Insert(IntPtr tree, IntPtr hItem, uint nCode, IntPtr pVal);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rtrGetItemValue")]
        private static extern IntPtr GetItemValue(IntPtr hItem);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rtrSetItemValue")]
        private static extern void SetItemValue(IntPtr hItem, IntPtr pVal);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rtrIsEmpty")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool rtrIsEmpty(IntPtr tree);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rtrSetFlags")]
        private static extern void SetFlags(IntPtr hItem, int flags);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rtrGetFlags")]
        private static extern int GetFlags(IntPtr hItem);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "rtrSplit")]
        private static extern IntPtr Split(IntPtr tree, IntPtr hItem, IntPtr sub);

        // ====================================================================
        // Constructors
        // ====================================================================

        /// <summary>
        /// Creates a new empty tree with default destroy function (NULL).
        /// </summary>
        public KTree()
        {
            _handle = CreateTree(IntPtr.Zero);
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create KTree.");
            _attached = false;
        }

        /// <summary>
        /// Creates a new empty tree with a custom destroy function for elements.
        /// </summary>
        /// <param name="fnDestroyElem">Pointer to a destroy callback (can be IntPtr.Zero).</param>
        public KTree(IntPtr fnDestroyElem)
        {
            _handle = CreateTree(fnDestroyElem);
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create KTree with custom destroy function.");
            _attached = false;
        }

        /// <summary>
        /// Wraps an existing tree handle (ownership determined by attach flag).
        /// </summary>
        /// <param name="existingHandle">Existing tree handle.</param>
        /// <param name="attach">True if handle is owned externally (no destruction on dispose).</param>
        public KTree(IntPtr existingHandle, bool attach)
        {
            if (existingHandle == IntPtr.Zero)
                throw new ArgumentException("Invalid tree handle", nameof(existingHandle));
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
                DestroyTree(_handle);
            }
        }

        // ====================================================================
        // Public properties and methods
        // ====================================================================

        /// <summary>
        /// Gets the number of nodes in the tree.
        /// </summary>
        public int Count => _handle != IntPtr.Zero ? GetCount(_handle) : 0;

        /// <summary>
        /// Checks if the tree is empty.
        /// </summary>
        public bool IsEmpty => _handle != IntPtr.Zero && rtrIsEmpty(_handle);

        /// <summary>
        /// Gets the root node handle.
        /// </summary>
        public IntPtr Root => _handle != IntPtr.Zero ? GetRoot(_handle) : IntPtr.Zero;

        /// <summary>
        /// Clears all nodes from the tree.
        /// </summary>
        public void Clear()
        {
            if (_handle != IntPtr.Zero)
                Clear(_handle);
        }

        /// <summary>
        /// Gets the first child of the specified node.
        /// </summary>
        public IntPtr GetChild(IntPtr hItem)
        {
            if (hItem == IntPtr.Zero) return IntPtr.Zero;
            return GetChildItem(hItem);
        }

        /// <summary>
        /// Gets the next sibling of the specified node.
        /// </summary>
        public IntPtr GetNextSibling(IntPtr hItem)
        {
            if (hItem == IntPtr.Zero) return IntPtr.Zero;
            return GetNextItem(hItem);
        }

        /// <summary>
        /// Gets the parent of the specified node.
        /// </summary>
        public IntPtr GetParent(IntPtr hItem)
        {
            if (hItem == IntPtr.Zero) return IntPtr.Zero;
            return GetParentItem(hItem);
        }

        /// <summary>
        /// Removes the specified node and all its children.
        /// </summary>
        public void RemoveNode(IntPtr hItem)
        {
            if (_handle != IntPtr.Zero && hItem != IntPtr.Zero)
                Remove(_handle, hItem);
        }

        /// <summary>
        /// Inserts a new node relative to a specified node.
        /// </summary>
        /// <param name="hItem">Reference node handle (can be IntPtr.Zero for root insertion).</param>
        /// <param name="relation">Relation code (RTR_ROOT, RTR_PARENT, RTR_CHILD, RTR_SIBLING, etc.).</param>
        /// <param name="value">Value to store.</param>
        /// <returns>Handle of the inserted node, or IntPtr.Zero on failure.</returns>
        public IntPtr InsertNode(IntPtr hItem, int relation, IntPtr value)
        {
            if (_handle == IntPtr.Zero) return IntPtr.Zero;
            return Insert(_handle, hItem, (uint)relation, value);
        }

        /// <summary>
        /// Gets the value stored in a node.
        /// </summary>
        public IntPtr GetNodeValue(IntPtr hItem)
        {
            if (hItem == IntPtr.Zero) return IntPtr.Zero;
            return GetItemValue(hItem);
        }

        /// <summary>
        /// Sets the value of a node.
        /// </summary>
        public void SetNodeValue(IntPtr hItem, IntPtr value)
        {
            if (hItem != IntPtr.Zero)
                SetItemValue(hItem, value);
        }

        /// <summary>
        /// Gets the flags of a node.
        /// </summary>
        public int GetNodeFlags(IntPtr hItem)
        {
            if (hItem == IntPtr.Zero) return 0;
            return GetFlags(hItem);
        }

        /// <summary>
        /// Sets the flags of a node.
        /// </summary>
        public void SetNodeFlags(IntPtr hItem, int flags)
        {
            if (hItem != IntPtr.Zero)
                SetFlags(hItem, flags);
        }

        /// <summary>
        /// Splits the tree by detaching the subtree rooted at hItem into a new tree.
        /// </summary>
        /// <param name="hItem">Node to split from.</param>
        /// <param name="newTree">Optional existing KTree to receive the split part (if null, a new one is created).</param>
        /// <returns>The new KTree containing the split subtree, or null on failure.</returns>
        public KTree Split(IntPtr hItem, KTree newTree = null)
        {
            if (_handle == IntPtr.Zero || hItem == IntPtr.Zero)
                return null;

            IntPtr subHandle = (newTree != null) ? newTree._handle : IntPtr.Zero;
            IntPtr result = Split(_handle, hItem, subHandle);
            if (result == IntPtr.Zero)
                return null;

            // If subHandle was zero, Split created a new tree; we need to wrap it.
            if (subHandle == IntPtr.Zero)
            {
                // The returned handle is the new tree; we own it.
                return new KTree(result, false);
            }
            else
            {
                // The tree was inserted into the existing newTree, so we return that instance.
                return newTree;
            }
        }

        // ====================================================================
        // Helper methods for common insert patterns
        // ====================================================================

        /// <summary>
        /// Inserts a node as the first child of a parent.
        /// </summary>
        public IntPtr AddChildFirst(IntPtr hParent, IntPtr value)
        {
            return InsertNode(hParent, RTR_FIRST, value);
        }

        /// <summary>
        /// Inserts a node as the last child of a parent.
        /// </summary>
        public IntPtr AddChildLast(IntPtr hParent, IntPtr value)
        {
            return InsertNode(hParent, RTR_LAST, value);
        }

        /// <summary>
        /// Inserts a node as a child (appends at end) of a parent.
        /// </summary>
        public IntPtr AddChild(IntPtr hParent, IntPtr value)
        {
            // Typically, RTR_CHILD inserts as first child? The header says "RTR_CHILD retrieves the first child", but for insertion, 
            // the code may treat it differently. The insertion flag RTR_ROOT, RTR_PARENT, etc. seem to indicate relation.
            // From the documentation: "if ncode == ROOT, hItem maybe NULL". So RTR_ROOT inserts as root?
            // Actually, we need to understand the semantics. Based on typical tree APIs, RTR_CHILD likely inserts as a child of hItem,
            // but which position? The header is ambiguous. We'll provide a generic InsertNode and let caller specify RTR_CHILD, RTR_FIRST, RTR_LAST.
            // We'll add convenience methods for common cases:
            // - Insert as first child: use RTR_FIRST with hItem as parent.
            // - Insert as last child: use RTR_LAST with hItem as parent.
            // - Insert as next sibling: use RTR_SIBLING (or RTR_NEXT) with hItem as previous node.
            // - Insert as previous sibling: use RTR_PREV.
            // - Insert as root: use RTR_ROOT with hItem = IntPtr.Zero.
            // We'll document this.
            return InsertNode(hParent, RTR_LAST, value);
        }
    }
}