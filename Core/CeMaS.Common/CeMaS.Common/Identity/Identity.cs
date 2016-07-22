using CeMaS.Common.Validation;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CeMaS.Common.Identity
{
    /// <summary>
    /// Writable <see cref="IIdentity{T}"/>.
    /// </summary>
    /// <typeparam name="T">Identifier type.</typeparam>
    [DataContract]
    public class Identity<T> :
        IdentityWithValues<T>,
        IIdentityInfoWritable
    {
        public Identity(
            T id,
            string name,
            string description = null,
            IDictionary<string, object> values = null
            ) :
            base(id, values)
        {
            Name = name;
            Description = description;
        }

        [DataMember(IsRequired = true)]
        public new string Name
        {
            get { return name; }
            set
            {
                Argument.NonNullOrEmpty(value);
                SetPropertyValue(ref name, value, () => OnNameChanged(false));
            }
        }
        [DataMember]
        public new string Description
        {
            get { return description; }
            set
            {
                SetPropertyValueWithEmptyAsNull(ref description, value, () => OnDescriptionChanged(false));
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

        private string name;
        private string description;
    }
}
