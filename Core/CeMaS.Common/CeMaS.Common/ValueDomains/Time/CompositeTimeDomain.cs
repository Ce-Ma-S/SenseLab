using System;
using System.Collections.Generic;

namespace CeMaS.Common.ValueDomains.Time
{
    /// <summary>
    /// Defines a time domain.
    /// </summary>
    public class CompositeTimeDomain :
        CompositeOrderedValueDomain<DateTimeOffset, ITimeDomain>,
        ITimeDomain
    {
        public CompositeTimeDomain(
            IEnumerable<ITimeDomain> included = null,
            IEnumerable<ITimeDomain> excluded = null
            ) :
            base(included, excluded)
        { }

        /// <summary>
        /// Optional length of this domain if it can be obtained.
        /// </summary>
        public virtual Optional<TimeSpan> Length
        {
            get
            {
                var min = Min;
                var max = Max;
                return min.HasValue && max.HasValue ?
                    max.Value - min.Value :
                    Optional<TimeSpan>.None;
            }
        }
    }
}
