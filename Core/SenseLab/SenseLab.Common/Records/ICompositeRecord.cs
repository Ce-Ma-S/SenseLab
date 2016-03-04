using System.Collections.Generic;

namespace SenseLab.Common.Records
{
    public interface ICompositeRecord :
        IRecord
    {
        IEnumerable<IRecord> Items { get; }
    }
}
