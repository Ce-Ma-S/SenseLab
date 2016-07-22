using CeMaS.Common.Collections;
using CeMaS.Common.Properties;
using CeMaS.Common.Validation;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CeMaS.Common.Identity
{
    [DataContract]
    public class IdentityInfo :
        IIdentityInfo,
        IHaveNamedValuesWritable
    {
        public IdentityInfo(
            string name,
            string description = null,
            IDictionary<string, object> values = null
            )
        {
            Name = name;
            Description = description;
            Values = IdentityHelper.InitEnsureReadOnlyAsWell(values);
        }

        [DataMember(IsRequired = true)]
        public string Name
        {
            get { return name; }
            set
            {
                Argument.NonNullOrEmpty(value);
                name = value;
            }
        }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public IDictionary<string, object> Values { get; }
        IReadOnlyDictionary<string, object> IHaveNamedValues.Values
        {
            get { return Values.ReadOnly(); }
        }

        public override string ToString()
        {
            return Name;
        }

        private string name;
    }
}
