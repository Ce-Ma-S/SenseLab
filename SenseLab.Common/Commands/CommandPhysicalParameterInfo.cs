using CeMaS.Common.Units;
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
            Unit unit,
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

        public Unit Unit
        {
            get { return unit; }
            set
            {
                value.ValidateNonNull();
                SetValue(() => Unit, ref unit, value);
            }
        }

        private Unit unit;
    }
}