using CeMaS.Common.Events;
using CeMaS.Common.Identity;
using SenseLab.Common.Objects;

namespace SenseLab.Common.Properties
{
    public class Property<T> :
        ObjectItem,
        IProperty<T>
    {
        #region Init

        public Property(
            Object @object,
            string id,
            IdentityInfo info
            ) :
            base(@object, id, info)
        { }

        public Property(
            Object @object,
            string id,
            IdentityInfo info,
            T value
            ) :
            base(@object, id, info)
        {
            Value = value;
        }

        #endregion

        public System.Type Type
        {
            get
            {
                return typeof(T);
            }
        }

        public bool HasValue { get; set; }

        public T Value
        {
            get
            {
                return HasValue ?
                    value :
                    default(T);
            }
            set
            {
                SetPropertyValue(ref this.value, value, OnValueChanged);
            }
        }
        object IProperty.Value
        {
            get
            {
                return Value;
            }
        }

        public event System.EventHandler<ValueChangeEventArgs<T>> ValueChanged;
        event System.EventHandler<ValueChangeEventArgs> IProperty.ValueChanged
        {
            add
            {
                valueChangedUntyped += value;
            }
            remove
            {
                valueChangedUntyped -= value;
            }
        }

        protected virtual void OnValueChanged(bool hasOldValue, T oldValue, T newValue)
        {
            if (hasOldValue)
            {
                var a = new ValueChangeEventArgs<T>(oldValue, newValue);
                ValueChanged.RaiseEvent(this, a);
                valueChangedUntyped.RaiseEvent(this, a);
            }
            else
            {
                var a = new ValueChangeEventArgs<T>(newValue, true);
                ValueChanged.RaiseEvent(this, a);
                valueChangedUntyped.RaiseEvent(this, a);
            }
        }

        private void OnValueChanged(T oldValue, T newValue)
        {
            var hasOldValue = HasValue;
            HasValue = true;
            OnValueChanged(hasOldValue, oldValue, newValue);
        }

        private T value;
        private event System.EventHandler<ValueChangeEventArgs> valueChangedUntyped;
    }
}
