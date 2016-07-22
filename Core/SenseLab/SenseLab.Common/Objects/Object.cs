using System;
using System.Collections.Generic;
using CeMaS.Common.Events;
using CeMaS.Common.Validation;
using System.Linq;
using System.Collections.ObjectModel;
using CeMaS.Common.Identity;

namespace SenseLab.Common.Objects
{
    public abstract class Object :
        ObjectInfo,
        IObject
    {
        public Object(
            ObjectEnvironment environment,
            string id,
            string name,
            ObjectType type,
            string description = null,
            IDictionary<string, object> values = null,
            Object parent = null
            ) :
            base(environment, id, name, type, description, values, parent)
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

        #region Items

        public ObservableCollection<ObjectItem> Items { get; } =
            new ObservableCollection<ObjectItem>();
        IReadOnlyList<IObjectItem> IObject.Items
        {
            get { return Items; }
        }
        public virtual IObjectItem this[string id]
        {
            get
            {
                return Items.SingleOrDefault(i => i.Id.Equals(id));
            }
        }

        #endregion

        #region Hierarchy

        public new ObjectEnvironment Environment
        {
            get { return (ObjectEnvironment)base.Environment; }
        }
        IObjectEnvironment IObject.Environment
        {
            get { return Environment; }
        }

        public new Object Parent
        {
            get { return (Object)base.Parent; }
        }
        IObject IObject.Parent
        {
            get { return Parent; }
        }

        public ObservableCollection<Object> Children { get; } =
            new ObservableCollection<Object>();
        IReadOnlyList<IObject> IObject.Children
        {
            get { return Children; }
        }

        #endregion
    }
}
