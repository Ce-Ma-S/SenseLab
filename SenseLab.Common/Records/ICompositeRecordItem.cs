using System.Collections.Generic;

namespace SenseLab.Common.Records
{
    public interface ICompositeRecordItem :
        IRecordItem
    {
        IEnumerable<IRecordItem> Items { get; }
    }
}
