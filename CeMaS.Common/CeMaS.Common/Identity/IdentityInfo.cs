using CeMaS.Common.Properties;
using CeMaS.Common.Validation;
using System.Runtime.Serialization;

namespace CeMaS.Common.Identity
{
    [DataContract]
    public class IdentityInfo :
        IIdentityInfo
    {
        public IdentityInfo(
            string name,
            string description = null,
            Metadata metadata = null
            )
        {
            Name = name;
            Description = description;
            Metadata = metadata ?? new Metadata();
        }

        [DataMember(IsRequired = true)]
        public string Name
        {
            get { return name; }
            set
            {
                value.ValidateNonNullOrEmpty();
                name = value;
            }
        }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public Metadata Metadata { get; }
        IMetadata IHaveMetadata.Metadata
        {
            get { return Metadata; }
        }

        private string name;
    }
}
