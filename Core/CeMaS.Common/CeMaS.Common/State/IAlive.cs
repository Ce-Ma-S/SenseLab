using System;

namespace CeMaS.Common.State
{
    /// <summary>
    /// Specifies object availability.
    /// </summary>
    public interface IAlive
    {
        /// <summary>
        /// Whether this object is alive or available.
        /// </summary>
        bool IsAlive { get; }
        /// <summary>
        /// Raised when <see cref="IsAlive"/> changes.
        /// </summary>
        event EventHandler IsAliveChanged;
    }
}
