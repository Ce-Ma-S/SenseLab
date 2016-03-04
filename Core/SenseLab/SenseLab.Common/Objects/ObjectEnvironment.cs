using CeMaS.Common.Events;
using CeMaS.Common.Identity;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Objects
{
    public abstract class ObjectEnvironment :
        ObjectEnvironmentInfo,
        IObjectEnvironment
    {
        public ObjectEnvironment(
            Guid id,
            IdentityInfo info
            ) :
            base(id, info)
        { }

        #region IsAlive

        public abstract bool IsAlive { get; }
        public event EventHandler IsAliveChanged;

        protected virtual void OnIsAliveChanged()
        {
            IsAliveChanged.RaiseEvent(this);
        }

        #endregion

        public abstract IEnumerable<Object> Objects { get; }
        IEnumerable<IObject> IObjectEnvironment.Objects
        {
            get { return Objects; }
        }
    }
}
