using CeMaS.Common.Disposing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace CeMaS.Common.Events
{
    /// <summary>
    /// Notifies about property changes.
    /// </summary>
    [DataContract]
    public abstract class NotifyPropertyChange :
        Disposable,
        INotifyPropertyChanged,
        IChangeable
    {
        #region Changed

        /// <summary>
        /// Fired on any change.
        /// </summary>
        public event EventHandler Changed;

        protected virtual void OnChanged(EventArgs arguments = null)
        {
            Changed.RaiseEvent(this, arguments);
        }

        #endregion

        #region PropertyChanged

        /// <summary>
        /// Fired when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var a = PropertyChanged.RaiseEvent(this, propertyName);
            OnChanged(a ?? new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region SetPropertyValue

        protected bool SetPropertyValue<T>(
            ref T field,
            T value,
            Action<T, T> onChanged,
            Func<T> getValue = null,
            [CallerMemberName] string propertyName = null
            )
        {
            T oldValue = GetValue(field, getValue);
            field = value;
            T newValue = GetValue(field, getValue);
            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
                return false;
            if (onChanged != null)
                onChanged(oldValue, newValue);
            OnPropertyChanged(propertyName);
            return true;
        }

        protected bool SetPropertyValue<T>(
            ref T field,
            T value,
            Action onChanged = null,
            Func<T> getValue = null,
            [CallerMemberName] string propertyName = null
            )
        {
            return SetPropertyValue(
                ref field,
                value,
                onChanged == null ?
                    (Action<T, T>)null :
                    (o, n) => onChanged(),
                getValue,
                propertyName
                );
        }


        protected bool SetPropertyValue<T>(
            Func<T> getValue,
            Action setValue,
            Action<T, T> onChanged,
            [CallerMemberName] string propertyName = null
            )
        {
            T oldValue = getValue();
            setValue();
            T newValue = getValue();
            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
                return false;
            if (onChanged != null)
                onChanged(oldValue, newValue);
            OnPropertyChanged(propertyName);
            return true;
        }

        protected bool SetPropertyValue<T>(
            Func<T> getValue,
            Action setValue,
            Action onChanged = null,
            [CallerMemberName] string propertyName = null
            )
        {
            return SetPropertyValue(
                getValue,
                setValue,
                onChanged == null ?
                    (Action<T, T>)null :
                    (o, n) => onChanged(),
                propertyName
                );
        }


        protected bool SetPropertyValueWithEmptyAsNull(
            ref string field,
            string value,
            Action onChanged = null,
            Func<string> getValue = null,
            [CallerMemberName] string propertyName = null
            )
        {
            if (string.IsNullOrEmpty(value))
                value = null;
            return SetPropertyValue(ref field, value, onChanged, getValue, propertyName);
        }

        protected bool SetPropertyValueWithEmptyAsNull<T>(
            ref IEnumerable<T> field,
            IEnumerable<T> value,
            Action<IEnumerable<T>, IEnumerable<T>> onChanged = null,
            Func<IEnumerable<T>> getValue = null,
            [CallerMemberName] string propertyName = null
            )
        {
            if (value != null && !value.Any())
                value = null;
            return SetPropertyValue(ref field, value, onChanged, getValue, propertyName);
        }

        private static T GetValue<T>(T field, Func<T> getValue)
        {
            return getValue == null ?
                field :
                getValue();
        }

        #endregion

        //protected virtual object DoClone()
        //{
        //    var clone = (NotifyPropertyChange)MemberwiseClone();
        //    clone.ClearEventHandlers();
        //    return clone;
        //}
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                ClearEventHandlers();
        }
        protected virtual void ClearEventHandlers()
        {
            Changed = null;
            PropertyChanged = null;
        }
    }
}
