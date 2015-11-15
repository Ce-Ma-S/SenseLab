using CeMaS.Common.Validation;
using SenseLab.Common.Properties;
using SenseLab.Common.Values;

namespace SenseLab.Common.Commands
{
    public class CommandPhysicalParameterInfo<T> :
        CommandParameterInfo<T>,
        IPhysicalValueInfoWritable
    {
        #region Init

        public CommandPhysicalParameterInfo(
            string id,
            string name,
            string unit,
            string description = null
            ) :
            base(id, name, description)
        {
            Unit = unit;
        }

        public CommandPhysicalParameterInfo(
            IPhysicalProperty<T> property
            ) :
            base(property.Id, property.Name, property.Description)
        {
            Unit = property.Unit;
        }

        #endregion

        public string Unit
        {
            get { return unit; }
            set
            {
                value.ValidateNonNull();
                SetValue(() => Unit, ref unit, value);
            }
        }

        private string unit;
    }
}