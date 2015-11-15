using System;
using System.Collections.Generic;
using CeMaS.Common.Events;
using CeMaS.Common.Validation;
using System.Linq;
using System.Collections.ObjectModel;

namespace SenseLab.Common.Objects
{
    public abstract class Object :
        Item<Guid>,
        IObject
    {
        public Object(
            Guid id,
            string name,
            ObjectType type,
            string description = null,
            IObject parent = null
            ) :
            base(id, name, description)
        {
            type.ValidateNonNull(nameof(type));
            Type = type;
            Parent = parent;
        }

        #region Identification

        public ObjectType Type { get; }
        IObjectType IObject.Type
        {
            get { return Type; }
        }

        #endregion

        #region IsAlive

        public abstract bool IsAlive { get; }
        public event EventHandler IsAliveChanged;

        protected virtual void OnIsAliveChanged()
        {
            IsAliveChanged.RaiseEvent(this);
        }

        #endregion

        #region Items

        public ObservableCollection<IObjectItemWritable> Items { get; } =
            new ObservableCollection<IObjectItemWritable>();
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

        public IObject Parent
        {
            get;
        }
        public ObservableCollection<IObject> Children { get; } =
            new ObservableCollection<IObject>();
        IReadOnlyList<IObject> IObject.Children
        {
            get { return Children; }
        }

        #endregion
    }
}
