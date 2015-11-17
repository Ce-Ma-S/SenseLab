using CeMaS.Common;
using System.Collections.Generic;

namespace SenseLab.Common.Objects
{
    /// <summary>
    /// Environment with objects.
    /// </summary>
    public interface IObjectEnvironment :
        IObjectEnvironmentInfo,
        IAlive
    {
        /// <summary>
        /// Root objects of this environment.
        /// </summary>
        IReadOnlyList<IObject> Objects { get; }
    }
}
