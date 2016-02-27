using CeMaS.Common.Identity;
using CeMaS.Common.Validation;

namespace CeMaS.Common.Units
{
    public class Unit :
        IId<string>
    {
        public Unit(
            string id,
            string symbol,
            string name,
            string description = null
            )
        {
            id.ValidateNonNullOrEmpty(nameof(id));
            symbol.ValidateNonNullOrEmpty(nameof(symbol));
            name.ValidateNonNullOrEmpty(nameof(name));
            Id = id;
            Symbol = symbol;
            Name = name;
            Description = description;
        }

        public string Id { get; }
        public string Symbol { get; }
        public string Name { get; }
        public string Description { get; }
    }
}
