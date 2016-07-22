using CeMaS.Common.Events;
using SenseLab.Common.Objects;
using System.Collections.Generic;

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
            string description = null,
            IDictionary<string, object> values = null
            ) :
            base(@object, id, name, description, values)
        { }

        public Property(
            Object @object,
            string id,
            string name,
            T value,
            string description = null,
            IDictionary<string, object> values = null
            ) :
            base(@object, id, name, description, values)
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
        event System.EventHandler<ValueChangeEventArgs<object>> IProperty.ValueChanged
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
                ValueChanged.RaiseEvent(this, new ValueChangeEventArgs<T>(oldValue, newValue));
                valueChangedUntyped.RaiseEvent(this, new ValueChangeEventArgs<object>(oldValue, newValue));
            }
            else
            {
                ValueChanged.RaiseEvent(this, new ValueChangeEventArgs<T>(newValue, true));
                valueChangedUntyped.RaiseEvent(this, new ValueChangeEventArgs<object>(newValue, true));
            }
        }

        private void OnValueChanged(T oldValue, T newValue)
        {
            var hasOldValue = HasValue;
            HasValue = true;
            OnValueChanged(hasOldValue, oldValue, newValue);
        }

        private T value;
        private event System.EventHandler<ValueChangeEventArgs<object>> valueChangedUntyped;
    }
}
