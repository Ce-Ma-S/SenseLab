using System.Collections.Generic;

namespace CeMaS.Common.State
{
    /// <summary>
    /// Object`s state.
    /// </summary>
    public interface IStateful
    {
        /// <summary>
        /// Gets object`s state.
        /// </summary>
        IReadOnlyDictionary<string, object> GetState();
    }
}
