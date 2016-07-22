using CeMaS.Common.Properties;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CeMaS.Common.Identity
{
    /// <summary>
    /// <see cref="IIdentity{T}"/> with <see cref="IHaveNamedValuesWritable.Values"/>.
    /// </summary>
    /// <typeparam name="T">Identifier type.</typeparam>
    [DataContract]
    public abstract class IdentityWithValues<T> :
        IdentityBase<T>,
        IHaveNamedValuesWritable
    {
        public IdentityWithValues(
            T id,
            IDictionary<string, object> values = null
            ) :
            base(id)
        {
            Values = IdentityHelper.InitEnsureReadOnlyAsWell(values);
        }

        [DataMember(IsRequired = true)]
        public new IDictionary<string, object> Values { get; private set; }

        protected override IDictionary<string, object> GetValues()
        {
            return Values;
        }
    }
}
