using System;

namespace SenseLab.Common.Records
{
    public interface IRecordItem
    {
        TimeSpan Start { get; set; }
        TimeSpan? Duration { get; }
        TimeSpan? End { get; set; }
    }
}