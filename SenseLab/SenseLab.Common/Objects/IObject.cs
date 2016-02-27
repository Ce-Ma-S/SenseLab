using CeMaS.Common.State;
using System.Collections.Generic;

namespace SenseLab.Common.Objects
{
    public interface IObject :
        IObjectInfo,
        IAlive
    {
        #region Items

        IReadOnlyList<IObjectItem> Items { get; }
        IObjectItem this[string id] { get; }

        #endregion

        #region Hierarchy

        new IObjectEnvironment Environment { get; }
        new IObject Parent { get; }
        IReadOnlyList<IObject> Children { get; }

        #endregion
    }
}
