using SenseLab.Common.Objects;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Records
{
    public interface IRecordLine
    {
        IObjectItemInfo ObjectItem { get; }
        IEnumerable<IRecord> Records { get; }

        DateTimeOffset Start { get; }
        TimeSpan? Duration { get; }
        DateTimeOffset? End { get; }

        IRecord RecordAt(DateTimeOffset time);
    }
}
