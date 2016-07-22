using System;

namespace CeMaS.Common.ValueDomains.Time
{
    /// <summary>
    /// Defines a time domain.
    /// </summary>
    public interface ITimeDomain :
        IOrderedValueDomain<DateTimeOffset>
    {
        /// <summary>
        /// Optional length of this domain if it can be obtained.
        /// </summary>
        Optional<TimeSpan> Length { get; }
    }
}
