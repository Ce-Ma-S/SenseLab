using CeMaS.Common.Collections;
using CeMaS.Common.Events;
using CeMaS.Common.Properties;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CeMaS.Common.Identity
{
    /// <summary>
    /// <see cref="IIdentity{T}"/> base.
    /// </summary>
    /// <typeparam name="T">Identifier type.</typeparam>
    [DataContract]
    public abstract class IdentityBase<T> :
        NotifyPropertyChange,
        IIdentity<T>,
        IHaveNamedValuesWritable
    {
        public IdentityBase(T id)
        {
            Id = id;
        }

        #region Id

        [DataMember(IsRequired = true)]
        public T Id { get; protected set; }

        #endregion

        #region Info

        public string Name
        {
            get { return GetName(); }
        }
        public string Description
        {
            get { return GetDescription(); }
        }
        public IDictionary<string, object> Values
        {
            get { return GetValues(); }
        }
        IReadOnlyDictionary<string, object> IHaveNamedValues.Values
        {
            get { return Values.ReadOnly(); }
        }

        protected virtual string GetName()
        {
            return Id.ToString();
        }
        protected virtual string GetDescription()
        {
            return null;
        }
        protected abstract IDictionary<string, object> GetValues();

        protected virtual void OnNameChanged(bool raisePropertyChanged = true)
        {
            if (raisePropertyChanged)
                OnPropertyChanged(nameof(Name));
        }
        protected virtual void OnDescriptionChanged(bool raisePropertyChanged = true)
        {
            if (raisePropertyChanged)
                OnPropertyChanged(nameof(Description));
        }
        protected virtual void OnValuesChanged()
        {
            OnPropertyChanged(nameof(Values));
        }

        #endregion

        public override string ToString()
        {
            return $"{Name} ({Id})";
        }
    }
}
