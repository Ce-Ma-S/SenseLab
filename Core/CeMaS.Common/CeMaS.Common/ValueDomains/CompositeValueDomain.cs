using CeMaS.Common.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CeMaS.Common.ValueDomains
{
    /// <summary>
    /// <see cref="ICompositeValueDomain{T}"/> implementation.
    /// </summary>
    /// <typeparam name="TValue">Value type.</typeparam>
    /// <typeparam name="TDomain">Domain type.</typeparam>
    public class CompositeValueDomain<TValue, TDomain> :
        ValueDomain<TValue>,
        ICompositeValueDomain<TValue>
        where TDomain : IValueDomain<TValue>
    {
        public CompositeValueDomain(
            IEnumerable<TDomain> included = null,
            IEnumerable<TDomain> excluded = null
            )
        {
            Included = new ObservableCollection<TDomain>(included);
            RegisterItemsChange(Included);
            Excluded = new ObservableCollection<TDomain>(excluded);
            RegisterItemsChange(Excluded);
        }

        /// <summary>
        /// Included domains.
        /// </summary>
        /// <value>non-null</value>
        INotifyList<TDomain> Included { get; }
        IEnumerable<IValueDomain<TValue>> ICompositeValueDomain<TValue>.Included
        {
            get { return Included.Cast<IValueDomain<TValue>>(); }
        }
        /// <summary>
        /// Excluded domains.
        /// </summary>
        /// <value>non-null</value>
        INotifyList<TDomain> Excluded { get; }
        IEnumerable<IValueDomain<TValue>> ICompositeValueDomain<TValue>.Excluded
        {
            get { return Excluded.Cast<IValueDomain<TValue>>(); }
        }

        public override bool Contains(TValue value)
        {
            return
                Included.Any(i => i.Contains(value)) &&
                Excluded.All(i => !i.Contains(value));
        }
    }
}
