using CeMaS.Common.Validation;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CeMaS.Common.Units
{
    [DataContract]
    public class SIPrefix :
        Unit<string>,
        ISIPrefix
    {
        public SIPrefix(
            string id,
            string symbol,
            double multiplier,
            string name = null,
            string description = null,
            IDictionary<string, object> values = null
            ) :
            base(SI.GetId(id), symbol, name ?? id, description, values)
        {
            Argument.GreaterThan(multiplier, 0, nameof(multiplier));
            Multiplier = multiplier;
        }

        public static readonly SIPrefix Atto = new SIPrefix(nameof(Atto), "a", 1e-18);
        public static readonly SIPrefix Femto = new SIPrefix(nameof(Femto), "f", 1e-15);
        public static readonly SIPrefix Pico = new SIPrefix(nameof(Pico), "p", 1e-12);
        public static readonly SIPrefix Nano = new SIPrefix(nameof(Nano), "n", 1e-9);
        public static readonly SIPrefix Micro = new SIPrefix(nameof(Micro), "μ", 1e-6);
        public static readonly SIPrefix Milli = new SIPrefix(nameof(Milli), "m", 1e-3);
        public static readonly SIPrefix Centi = new SIPrefix(nameof(Centi), "c", 1e-2);
        public static readonly SIPrefix Deci = new SIPrefix(nameof(Deci), "d", 1e-1);

        public static readonly SIPrefix Deca = new SIPrefix(nameof(Deca), "da", 1e1);
        public static readonly SIPrefix Hecto = new SIPrefix(nameof(Hecto), "h", 1e2);
        public static readonly SIPrefix Kilo = new SIPrefix(nameof(Kilo), "k", 1e3);
        public static readonly SIPrefix Mega = new SIPrefix(nameof(Mega), "M", 1e6);
        public static readonly SIPrefix Giga = new SIPrefix(nameof(Giga), "G", 1e9);
        public static readonly SIPrefix Tera = new SIPrefix(nameof(Tera), "T", 1e12);
        public static readonly SIPrefix Peta = new SIPrefix(nameof(Peta), "P", 1e15);
        public static readonly SIPrefix Exa = new SIPrefix(nameof(Exa), "E", 1e18);

        [DataMember]
        public double Multiplier { get; private set; }
    }
}