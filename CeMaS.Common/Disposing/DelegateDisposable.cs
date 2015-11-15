using CeMaS.Common.Validation;
using System;

namespace CeMaS.Common.Disposing
{
    /// <summary>
    /// Disposable with delegate dispose action.
    /// </summary>
    public class DelegateDisposable :
        Disposable
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dispose">Action implementing <see cref="Dispose"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dispose"/> is null.</exception>
        public DelegateDisposable(Action<bool> dispose)
        {
            dispose.ValidateNonNull(nameof(dispose));
            this.dispose = dispose;
        }

        protected override void Dispose(bool disposing)
        {
            dispose(disposing);
        }

        private readonly Action<bool> dispose;
    }
}
