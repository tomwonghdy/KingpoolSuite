using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingpool.Core
{
    /// <summary>
    /// Base class for types that wrap an unmanaged handle.
    /// Provides IDisposable and finalizer, relying on handle-zero check (no disposed flag).
    /// </summary>
    public abstract class KingsHandler : IDisposable
    {
        // The unmanaged handle (accessible to derived classes)
        protected IntPtr _handle = IntPtr.Zero;
        // Indicates whether the handle is owned by this instance (default: false = owned)
        protected bool _attached = false;

        /// <summary>
        /// Releases the unmanaged handle (implemented by derived class).
        /// Must be idempotent and handle null checks internally.
        /// </summary>
        protected abstract void ReleaseHandle();

        /// <summary>
        /// Explicitly destroy the handle (idempotent).
        /// </summary>
        public void Destroy()
        {
            if (_handle != IntPtr.Zero)
            {
                ReleaseHandle();          // polymorphic call
                _handle = IntPtr.Zero;
            }
            _attached = false;
        }

        // IDisposable implementation
        public void Dispose()
        {
            Destroy();
            GC.SuppressFinalize(this);
        }

        // Finalizer (safety net)
        ~KingsHandler()
        {
            Destroy();   // direct call, no Dispose
        }

        // Helper to safely set a new handle (releases old one first)
        protected void SetHandle(IntPtr newHandle, bool attached = false)
        {
            Destroy();   // release old handle
            _handle = newHandle;
            _attached = attached;
        }

        //obsolete, used Handle instead
        public IntPtr GetHandle()
        {
            return _handle;
        }

        // Public property to get the handle (read-only for external)
        public IntPtr Handle => _handle;
        public bool IsValid => _handle != IntPtr.Zero;
    }
}
