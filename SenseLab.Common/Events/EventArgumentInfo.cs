using SenseLab.Common.Values;

namespace SenseLab.Common.Events
{
    public class EventArgumentInfo :
        ValueInfo<string>,
        IEventArgumentInfoWritable
    {
        public EventArgumentInfo(
            string id,
            string name,
            string description = null
            ) :
            base(id, name, description)
        {
        }
    }
}