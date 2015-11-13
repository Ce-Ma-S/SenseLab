using System;
using System.Collections.Generic;

namespace SenseLab.Common.Objects
{
    public interface IObject :
        IItem<Guid>
    {
        IObjectType Type { get; }
        string Path { get; }

        bool IsAlive { get; }
        event EventHandler IsAliveChanged;

        IEnumerable<IObjectItem> Items { get; }
        IObjectItem this[string id] { get; }

        #region Hierarchy

        IObject Parent { get; }
        IEnumerable<IObject> Children { get; }

        #endregion
    }
}
