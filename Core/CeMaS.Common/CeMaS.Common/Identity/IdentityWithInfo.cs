using CeMaS.Common.Validation;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CeMaS.Common.Identity
{
    /// <summary>
    /// <see cref="IIdentity{T}"/> with <see cref="IdentityInfo"/>.
    /// </summary>
    /// <typeparam name="T">Identifier type.</typeparam>
    [DataContract]
    public class IdentityWithInfo<T> :
        IdentityBase<T>,
        IIdentityInfoWritable
    {
        public IdentityWithInfo(
            T id,
            IdentityInfo info = null
            ) :
            base(id)
        {
            Info = info ??
                new IdentityInfo(id.ToString());
        }

        #region Info

        [DataMember(IsRequired = true)]
        public IdentityInfo Info
        {
            get { return info; }
            set
            {
                Argument.NonNull(value);
                SetPropertyValue(ref info, value, (Action<IdentityInfo, IdentityInfo>)OnInfoChanged);
            }
        }
        public new string Name
        {
            get { return Info.Name; }
            set
            {
                SetPropertyValue(() => Name, () => Info.Name = value, () => OnNameChanged(false));
            }
        }
        public new string Description
        {
            get { return Info.Description; }
            set
            {
                SetPropertyValue(() => Description, () => Info.Description = value, () => OnDescriptionChanged(false));
            }
        }

        protected override string GetName()
        {
            return Name;
        }
        protected override string GetDescription()
        {
            return Description;
        }
        protected override IDictionary<string, object> GetValues()
        {
            return Info.Values;
        }

        public virtual void OnInfoChanged()
        {
            OnNameChanged();
            OnDescriptionChanged();
            OnValuesChanged();
        }

        protected virtual void OnInfoChanged(IdentityInfo oldValue, IdentityInfo newValue)
        {
            OnInfoChanged();
        }

        private IdentityInfo info;

        #endregion
    }
}
