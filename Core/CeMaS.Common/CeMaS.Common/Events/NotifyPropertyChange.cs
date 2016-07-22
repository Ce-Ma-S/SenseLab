using CeMaS.Common.Collections;
using CeMaS.Common.Disposing;
using CeMaS.Common.Validation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        INotifyPropertyChanging,
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

        #region PropertyChanging

        /// <summary>
        /// Fired befire a property value change.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        public virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            PropertyChanging.RaiseEvent(this, propertyName);
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
            OnPropertyChanging(propertyName);
            field = value;
            T newValue = GetValue(field, getValue);
            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
                return false;
            onChanged?.Invoke(oldValue, newValue);
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
            OnPropertyChanging(propertyName);
            setValue();
            T newValue = getValue();
            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
                return false;
            onChanged?.Invoke(oldValue, newValue);
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

        #region Items

        #region INotifyEnumerable

        protected virtual void RegisterItemsChange<T>(INotifyEnumerable<T> source)
        {
            ValidateItems(source, source);
            RegisterItemsChange(source.OfType<IChangeable>(), true);
            AddDisposables(
                source.Adding.Subscribe(i => OnItemsAdding(source, i)),
                source.Added.Subscribe(i => OnItemsAdded(source, i)),
                source.Removed.Subscribe(i => OnItemsRemoved(source, i)),
                new DelegateDisposable(disposing =>
                {
                    if (disposing)
                        RegisterItemsChange(source.OfType<IChangeable>(), false);
                })
                );
        }

        protected void ValidateItems<T>(INotifyEnumerable<T> source, IEnumerable<T> items)
        {
            foreach (var item in items)
                ValidateItem(source, item);
        }
        protected virtual void ValidateItem<T>(INotifyEnumerable<T> source, T item)
        {
            Argument.NonNull(item, nameof(item));
        }

        protected virtual void OnItemsAdding<T>(INotifyEnumerable<T> source, IEnumerable<T> items)
        {
            ValidateItems(source, items);
        }
        protected virtual void OnItemsAdded<T>(INotifyEnumerable<T> source, IEnumerable<T> items)
        {
            RegisterItemsChange(source.OfType<IChangeable>(), true);
            OnChanged(new SubEventArgs(
                source,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items.ToArray())
                ));
        }
        protected virtual void OnItemsRemoved<T>(INotifyEnumerable<T> source, IEnumerable<T> items)
        {
            RegisterItemsChange(source.OfType<IChangeable>(), false);
            OnChanged(new SubEventArgs(
                source,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, items.ToArray())
                ));
        }

        #endregion

        protected void RegisterItemsChange(IEnumerable<IChangeable> items, bool register)
        {
            foreach (var item in items)
            {
                item.Changed -= OnItemChanged;
                if (register)
                    item.Changed += OnItemChanged;
            }
        }

        protected virtual void OnItemChanged(IChangeable item, EventArgs a)
        {
            OnChanged(new SubEventArgs(item, a));
        }

        private void OnItemChanged(object sender, EventArgs a)
        {
            OnItemChanged((IChangeable)sender, a);
        }

        #endregion

        protected override object DoClone()
        {
            var clone = (NotifyPropertyChange)base.DoClone();
            clone.ClearEventHandlers();
            return clone;
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
                ClearEventHandlers();
        }
        protected virtual void ClearEventHandlers()
        {
            Changed = null;
            PropertyChanging = null;
            PropertyChanged = null;
        }
    }
}
