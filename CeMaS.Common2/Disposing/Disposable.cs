using System;
using System.Runtime.Serialization;

namespace CeMaS.Common.Disposing
{
    /// <summary>
    /// Disposable.
    /// </summary>
    [DataContract]
    public abstract class Disposable :
        IDisposable
    {
        ~Disposable()
        {
            Dispose(false);
        }

        /// <summary>
        /// Whether this object is disposed already.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Disposes this object directly.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
                return;
            Dispose(true);
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes this object.
        /// </summary>
        /// <param name="disposing">Whether this method is called from <see cref="Dispose()"/>, otherwise from destructor.</param>
        protected abstract void Dispose(bool disposing);
    }
}
