using System;
using System.Collections.Generic;

namespace SenseLab.Common.Objects
{
    public interface IObject :
        IItem<Guid>
    {
        #region Identification

        IObjectType Type { get; }
        //string Path { get; }

        #endregion

        #region IsAlive

        bool IsAlive { get; }
        event EventHandler IsAliveChanged;

        #endregion

        #region Items

        IReadOnlyList<IObjectItem> Items { get; }
        IObjectItem this[string id] { get; }

        #endregion

        #region Hierarchy

        IObject Parent { get; }
        IReadOnlyList<IObject> Children { get; }

        #endregion
    }
}
