using SenseLab.Common.Properties;
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

        public CommandParameterInfo(
            IProperty<T> property
            ) :
            base(property.Id, property.Name, property.Description)
        {
        }
    }
}