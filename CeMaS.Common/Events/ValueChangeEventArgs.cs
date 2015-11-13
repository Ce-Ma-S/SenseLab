using System;

namespace CeMaS.Common.Events
{
    public abstract class ValueChangeEventArgs :
        EventArgs
    {
        public ValueChangeEventArgs(bool hasOldValue)
        {
            HasOldValue = hasOldValue;
        }

        public bool HasOldValue { get; }
        public object OldValue
        {
            get
            {
                if (!HasOldValue)
                    throw new InvalidOperationException("Old value is not available.");
                return GetOldValue();
            }
        }
        public object NewValue
        {
            get { return GetNewValue(); }
        }

        internal abstract object GetOldValue();
        internal abstract object GetNewValue();
    }

    public class ValueChangeEventArgs<T> :
        ValueChangeEventArgs
    {
        public ValueChangeEventArgs(T oldValue, T newValue) :
            base(true)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
        public ValueChangeEventArgs(T newValue) :
            base(false)
        {
            NewValue = newValue;
        }

        public new T OldValue { get; }
        public new T NewValue { get; }

        internal override object GetOldValue()
        {
            return OldValue;
        }
        internal override object GetNewValue()
        {
            return NewValue;
        }
    }
}
