using CeMaS.Common.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SenseLab.Common.Objects
{
    public abstract class ObjectEnvironment :
        ObjectEnvironmentInfo,
        IObjectEnvironment
    {
        public ObjectEnvironment(
            Guid id,
            string name,
            string description = null
            ) :
            base(id, name, description)
        {
        }

        #region IsAlive

        public abstract bool IsAlive { get; }
        public event EventHandler IsAliveChanged;

        protected virtual void OnIsAliveChanged()
        {
            IsAliveChanged.RaiseEvent(this);
        }

        #endregion

        public ObservableCollection<Object> Objects { get; } =
            new ObservableCollection<Object>();
        IReadOnlyList<IObject> IObjectEnvironment.Objects
        {
            get { return Objects; }
        }
    }
}
