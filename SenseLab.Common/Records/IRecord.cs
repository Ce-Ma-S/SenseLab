using System;

namespace SenseLab.Common.Records
{
    public interface IRecord
    {
        TimeSpan Start { get; set; }
        TimeSpan? Duration { get; }
        TimeSpan? End { get; set; }
    }
}