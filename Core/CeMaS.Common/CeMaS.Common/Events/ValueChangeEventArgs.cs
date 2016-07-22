using System;

namespace CeMaS.Common.Events
{
    /// <summary>
    /// Allows event notifications about a value change.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public class ValueChangeEventArgs<T> :
        EventArgs
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

        public Optional<T> OldValue { get; private set; }
        public Optional<T> NewValue { get; private set; }
    }
}