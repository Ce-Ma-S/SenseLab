using CeMaS.Common.Events;
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
            string name,
            string description = null
            ) :
            base(@object, id, name, description)
        {
        }

        public Property(
            Object @object,
            string id,
            string name,
            T value,
            string description = null
            ) :
            base(@object, id, name, description)
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
                SetValue(() => Value, ref this.value, value, OnValueChanged);
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
                var a = ValueChanged.RaiseEvent(this, () => new ValueChangeEventArgs<T>(oldValue, newValue));
                valueChangedUntyped.RaiseEvent(this, () => a ?? new ValueChangeEventArgs<T>(oldValue, newValue));
            }
            else
            {
                var a = ValueChanged.RaiseEvent(this, () => new ValueChangeEventArgs<T>(newValue));
                valueChangedUntyped.RaiseEvent(this, () => a ?? new ValueChangeEventArgs<T>(newValue));
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
