using SenseLab.Common.Values;

namespace SenseLab.Common.Commands
{
    public class CommandParameterInfo<T> :
        ValueInfo<T>,
        ICommandParameterInfoWritable
    {
        public CommandParameterInfo(
            string id,
            string name,
            string description = null
            ) :
            base(id, name, description)
        {
        }
    }
}