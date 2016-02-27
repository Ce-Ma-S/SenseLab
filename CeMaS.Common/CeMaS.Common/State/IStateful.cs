using CeMaS.Common.Properties;
using System.Threading.Tasks;

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
        Task<IMetadata> GetState();
    }
}
