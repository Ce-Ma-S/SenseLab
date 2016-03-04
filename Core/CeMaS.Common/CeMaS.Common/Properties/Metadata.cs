using CeMaS.Common.Validation;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Collections;
using CeMaS.Common.Collections;

namespace CeMaS.Common.Properties
{
    /// <summary>
    /// <see cref="IMetadata"/> base.
    /// </summary>
    [DataContract]
    public class Metadata :
        IMetadataWritable
    {
        public Metadata(params KeyValuePair<string, object>[] values) :
            this((IEnumerable<KeyValuePair<string, object>>)values)
        {
        }
        public Metadata(IEnumerable<KeyValuePair<string, object>> values)
        {
            foreach (var value in values)
                this.values.Add(value.Key, value.Value);
        }

        public IEnumerable<string> Ids
        {
            get { return values.Keys; }
        }
        public int Count
        {
            get { return values.Count; }
        }
        public object this[string id]
        {
            get
            {
                ValidateId(id);
                return TryGetItem(id).ValueOrDefault();
            }
            set
            {
                ValidateId(id);
                values[id] = value;
            }
        }

        public bool Contains(string id)
        {
            return
                !string.IsNullOrEmpty(id) &&
                values.ContainsKey(id);
        }
        public object GetItem(string id)
        {
            var value = TryGetItem(id);
            if (!value.HasValue)
                throw new KeyNotFoundException();
            return value.Value;
        }
        public Optional<object> TryGetItem(string id)
        {
            return
                string.IsNullOrEmpty(id) ?
                Optional<object>.None :
                values.GetValue(id);
        }
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static void ValidateId(string id)
        {
            id.ValidateNonNullOrEmpty(nameof(id));
        }

        [DataMember(Name = "Values", IsRequired = true)]
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();
    }
}
