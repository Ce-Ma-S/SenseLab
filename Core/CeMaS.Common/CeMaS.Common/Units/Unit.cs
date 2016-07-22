using CeMaS.Common.Identity;
using CeMaS.Common.Validation;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CeMaS.Common.Units
{
    /// <summary>
    /// <see cref="IUnitWritable{T}"/> base.
    /// </summary>
    /// <typeparam name="T">Identifier type.</typeparam>
    [DataContract]
    [KnownType(typeof(SIPrefix))]
    public class Unit<T> :
        Identity<T>,
        IUnitWritable<T>
    {
        public Unit(
            T id,
            string name,
            string symbol,
            string description = null,
            IDictionary<string, object> values = null
            ) :
            base(id, name, description, values)
        {
            Symbol = symbol;
        }

        [DataMember(IsRequired = true)]
        public string Symbol
        {
            get { return symbol; }
            set
            {
                Argument.NonNullOrEmpty(value);
                SetPropertyValue(ref symbol, value);
            }
        }

        private string symbol;
    }
}