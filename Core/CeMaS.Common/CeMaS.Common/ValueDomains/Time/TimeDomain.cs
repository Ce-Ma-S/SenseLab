using System;

namespace CeMaS.Common.ValueDomains.Time
{
    /// <summary>
    /// Defines a time domain.
    /// </summary>
    public abstract class TimeDomain :
        OrderedValueDomain<DateTimeOffset>,
        ITimeDomain
    {
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
