using CeMaS.Common.Events;
using CeMaS.Common.Validation;
using System;

namespace CeMaS.Common.Identity
{
    /// <summary>
    /// <see cref="IIdentity{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Identity<T> :
        NotifyPropertyChange,
        IIdentity<T>
    {
        public Identity(T id, IdentityInfo info)
        {
            Id = id;
            Info = info;
        }

        public T Id { get; private set; }

        #region Info

        public IdentityInfo Info
        {
            get { return info; }
            set
            {
                value.ValidateNonNull();
                SetPropertyValue(ref info, value, OnInfoChanged);
            }
        }
        IIdentityInfo IIdentity<T>.Info
        {
            get { return Info; }
        }
        public event EventHandler InfoChanged;

        protected virtual void OnInfoChanged()
        {
            InfoChanged.RaiseEvent(this);
        }

        private IdentityInfo info;

        #endregion

        public override string ToString()
        {
            return $"{Info.Name} ({Id})";
        }

        protected override void ClearEventHandlers()
        {
            base.ClearEventHandlers();
            InfoChanged = null;
        }
    }
}
