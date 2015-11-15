using SenseLab.Common.Objects;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Records
{
    public interface IRecord
    {
        IObjectItem ObjectItem { get; }
        IEnumerable<IRecordItem> Items { get; }
        DateTimeOffset Start { get; }
        TimeSpan Duration { get; }
        DateTimeOffset End { get; }
    }
}
