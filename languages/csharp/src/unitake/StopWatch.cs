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

using Kingpool.Core;

namespace Kingpool.Utility
{

    /// <summary>
    /// High-resolution stopwatch backed by an unmanaged stopwatch handle.
    /// Inherits from <see cref="KingsHandler"/> for deterministic resource release.
    /// </summary>
    public class StopWatch : KingsHandler
    {
        // ====================================================================
        // DLL import constants
        // ====================================================================

        private const string LIB_NAME = "UniSupport.dll";

        // ====================================================================
        // Private unmanaged imports
        // ====================================================================

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniCreateStopWatch")]
        private static extern IntPtr uniCreateStopWatch(string strName);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniDestroyStopWatch")]
        private static extern void uniDestroyStopWatch(IntPtr hStopWatch);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniStopWatchStart")]
        private static extern void uniStopWatchStart(IntPtr hStopWatch);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniStopWatchStop")]
        private static extern double uniStopWatchStop(IntPtr hStopWatch, bool bStartAgain);

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniStopWatchElapsedTime")]
        private static extern double uniStopWatchElapsedTime(IntPtr hStopWatch);  // unit: seconds

        [DllImport(LIB_NAME, CharSet = CharSet.Ansi, EntryPoint = "uniStopWatchIsStarted")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool uniStopWatchIsStarted(IntPtr hStopWatch);

        // ====================================================================
        // Constructors
        // ====================================================================

        /// <summary>
        /// Creates a new StopWatch with an internally allocated handle.
        /// </summary>
        public StopWatch()
        {
            _handle = uniCreateStopWatch(null);
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create StopWatch.");
            _attached = false;
        }

        /// <summary>
        /// Creates a new StopWatch with the given name.
        /// </summary>
        public StopWatch(string strName)
        {
            _handle = uniCreateStopWatch(strName);
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create StopWatch.");
            _attached = false;
        }

        /// <summary>
        /// Wraps an existing stopwatch handle.
        /// </summary>
        /// <param name="existingHandle">Existing stopwatch handle.</param>
        /// <param name="attach">
        /// True if the handle is owned externally (this instance will not destroy it);
        /// false if this instance takes ownership and will destroy it on dispose.
        /// </param>
        public StopWatch(IntPtr existingHandle, bool attach)
        {
            if (existingHandle == IntPtr.Zero)
                throw new ArgumentException("Invalid stopwatch handle", nameof(existingHandle));
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
                uniDestroyStopWatch(_handle);
            }
        }

        // ====================================================================
        // Public API
        // ====================================================================

        /// <summary>
        /// Recreates the underlying stopwatch handle with an optional name.
        /// Releases any previously attached handle first.
        /// </summary>
        public bool Create(string strName)
        {
            Destroy();  // release old handle (base method)
            _handle = uniCreateStopWatch(strName);
            _attached = false;
            return _handle != IntPtr.Zero;
        }

        /// <summary>
        /// Starts (or restarts) the stopwatch.
        /// </summary>
        public void Start()
        {
            if (_handle != IntPtr.Zero)
                uniStopWatchStart(_handle);
        }

        /// <summary>
        /// Stops the stopwatch.
        /// </summary>
        /// <param name="bStartAgain">
        /// If true, the stopwatch restarts immediately after stopping.
        /// </param>
        public void Stop(bool bStartAgain)
        {
            if (_handle != IntPtr.Zero)
                uniStopWatchStop(_handle, bStartAgain);
        }

        /// <summary>
        /// Stops the stopwatch without restarting.
        /// </summary>
        public void Stop() => Stop(false);

        /// <summary>
        /// Gets the elapsed time in seconds.
        /// </summary>
        public double GetElapsedTime()
        {
            return _handle != IntPtr.Zero ? uniStopWatchElapsedTime(_handle) : 0.0;
        }

        /// <summary>
        /// Returns true if the stopwatch is currently running.
        /// </summary>
        public bool IsStarted()
        {
            return _handle != IntPtr.Zero && uniStopWatchIsStarted(_handle);
        }

        /// <summary>
        /// Gets the elapsed time in seconds (property-style accessor).
        /// </summary>
        public double ElapsedTime => GetElapsedTime();

        /// <summary>
        /// Gets whether the stopwatch is running (property-style accessor).
        /// </summary>
        public bool Running => IsStarted();
    }
}
