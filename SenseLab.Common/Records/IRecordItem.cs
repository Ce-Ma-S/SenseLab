using System;

namespace SenseLab.Common.Records
{
    public interface IRecordItem
    {
        TimeSpan Start { get; set; }
        TimeSpan Duration { get; set; }
        TimeSpan End { get; }
        object Data { get; }
    }
}