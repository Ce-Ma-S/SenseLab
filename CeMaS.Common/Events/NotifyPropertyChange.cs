using CeMaS.Common.Disposing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
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
            Changed.RaiseEvent(this, () => arguments ?? EventArgs.Empty);
        }

        #endregion

        #region PropertyChanged

        /// <summary>
        /// Fired when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged<T>(Expression<Func<T>> property)
        {
            OnPropertyChanged(property.PropertyName());
        }
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var a = PropertyChanged.RaiseEvent(this, propertyName);
            OnChanged(a ?? new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region SetValue

        protected bool SetValue<T>(
            Expression<Func<T>> property,
            ref T field,
            T value,
            Action<T, T> onChanged
            )
        {
            var propertyInfo = property.PropertyInfo();
            T oldValue = (T)propertyInfo.GetValue(this, null);
            field = value;
            T newValue = (T)propertyInfo.GetValue(this, null);
            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
                return false;
            if (onChanged != null)
                onChanged(oldValue, newValue);
            OnPropertyChanged(propertyInfo.Name);
            return true;
        }

        protected bool SetValue<T>(
            Expression<Func<T>> property,
            ref T field,
            T value,
            Action onChanged = null
            )
        {
            return SetValue(
                property,
                ref field,
                value,
                onChanged == null ?
                    (Action<T, T>)null :
                    (n, v) => onChanged()
                );
        }


        protected bool SetValue<T>(
            Expression<Func<T>> property,
            Action setValue,
            Action<T, T> onChanged
            )
        {
            var propertyInfo = property.PropertyInfo();
            T oldValue = (T)propertyInfo.GetValue(this, null);
            setValue();
            T newValue = (T)propertyInfo.GetValue(this, null);
            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
                return false;
            if (onChanged != null)
                onChanged(oldValue, newValue);
            OnPropertyChanged(propertyInfo.Name);
            return true;
        }

        protected bool SetValue<T>(
            Expression<Func<T>> property,
            Action setValue,
            Action onChanged = null
            )
        {
            return SetValue(
                property,
                setValue,
                onChanged == null ?
                    (Action<T, T>)null :
                    (n, v) => onChanged()
                );
        }


        protected bool SetValue(
            Expression<Func<string>> property,
            ref string field,
            string value,
            bool nullIsSameAsEmpty,
            Action onChanged = null
            )
        {
            if (nullIsSameAsEmpty && string.IsNullOrEmpty(value))
                value = null;
            return SetValue(property, ref field, value, onChanged);
        }

        protected bool SetValue<T>(
            Expression<Func<IEnumerable<T>>> property,
            ref IEnumerable<T> field,
            IEnumerable<T> value,
            bool nullIsSameAsEmpty,
            Action<IEnumerable<T>, IEnumerable<T>> onChanged = null
            )
        {
            if (nullIsSameAsEmpty && value != null && !value.Any())
                value = null;
            return SetValue(property, ref field, value, onChanged);
        }

        #endregion

        protected virtual object DoClone()
        {
            var clone = (NotifyPropertyChange)MemberwiseClone();
            clone.ClearEventHandlers();
            return clone;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                ClearEventHandlers();
        }
        protected virtual void ClearEventHandlers()
        {
            PropertyChanged = null;
            Changed = null;
        }
    }
}
