using CeMaS.Common.Validation;
using SenseLab.Common.Objects;
using SenseLab.Common.Values;

namespace SenseLab.Common.Properties
{
    public class PhysicalProperty<T> :
        Property<T>,
        IPhysicalProperty<T>,
        IPhysicalValueInfoWritable
    {
        #region Init

        public PhysicalProperty(
            IObject @object,
            string id,
            string name,
            string unit,
            string description = null
            ) :
            base(@object, id, name, description)
        {
            Unit = unit;
        }

        public PhysicalProperty(
            IObject @object,
            string id,
            string name,
            T value,
            string unit,
            string description = null
            ) :
            base(@object, id, name, value, description)
        {
            Unit = unit;
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
