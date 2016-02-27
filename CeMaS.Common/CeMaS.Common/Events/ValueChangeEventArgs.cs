using System;

namespace CeMaS.Common.Events
{
    public abstract class ValueChangeEventArgs :
        EventArgs
    {
        public Optional<object> OldValue
        {
            get { return GetOldValue(); }
        }
        public Optional<object> NewValue
        {
            get { return GetNewValue(); }
        }

        internal abstract Optional<object> GetOldValue();
        internal abstract Optional<object> GetNewValue();
    }


    /// <summary>
    /// Allows event notifications about a value change.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public class ValueChangeEventArgs<T> :
        ValueChangeEventArgs
    {
        public ValueChangeEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
        public ValueChangeEventArgs(T value, bool isNew)
        {
            if (isNew)
                NewValue = value;
            else
                OldValue = value;
        }

        public new Optional<T> OldValue { get; private set; }
        public new Optional<T> NewValue { get; private set; }

        internal override Optional<object> GetOldValue()
        {
            return OldValue;
        }
        internal override Optional<object> GetNewValue()
        {
            return OldValue;
        }
    }
}