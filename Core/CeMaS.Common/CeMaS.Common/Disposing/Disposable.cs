using System;
using System.Reactive.Disposables;
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
        public bool Disposed { get; private set; }

        /// <summary>
        /// Disposes this object directly.
        /// </summary>
        public void Dispose()
        {
            if (Disposed)
                return;
            Dispose(true);
            Disposed = true;
            GC.SuppressFinalize(this);
        }

        protected CompositeDisposable Disposables
        {
            get
            {
                if (disposables == null)
                    disposables = new CompositeDisposable();
                return disposables;
            }
        }

        protected void AddDisposables(params IDisposable[] disposables)
        {
            foreach (var disposable in disposables)
                Disposables.Add(disposable);
        }

        protected virtual object DoClone()
        {
            var clone = (Disposable)MemberwiseClone();
            clone.disposables = null;
            return clone;
        }

        /// <summary>
        /// Disposes this object.
        /// </summary>
        /// <param name="disposing">Whether this method is called from <see cref="Dispose()"/>, otherwise from destructor.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (
                disposing &&
                disposables != null
                )
            {
                foreach (var disposable in disposables)
                    disposable.Dispose();
                disposables = null;
            }
        }

        private CompositeDisposable disposables;
    }
}
