using CeMaS.Common.Properties;
using System.Threading.Tasks;

namespace CeMaS.Common.State
{
    /// <summary>
    /// Object`s writable state.
    /// </summary>
    public interface IStatefulWritable :
        IStateful
    {
        /// <summary>
        /// Applies object`s state.
        /// </summary>
        /// <param name="state">State to be applied.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="state"/> is null.</exception>
        /// <exception cref="System.ArgumentException"><paramref name="state"/> is invalid.</exception>
        /// <value>Whether object`s state has been changed.</value>
        Task<bool> SetState(IMetadata state);
    }
}
