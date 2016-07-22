using CeMaS.Common.Units;
using CeMaS.Common.Validation;
using SenseLab.Common.Objects;
using System.Collections.Generic;

namespace SenseLab.Common.Properties
{
    public class PhysicalProperty<T> :
        Property<T>,
        IPhysicalProperty<T>
    {
        #region Init

        public PhysicalProperty(
            Object @object,
            string id,
            string name,
            Unit<string> unit,
            string description = null,
            IDictionary<string, object> values = null
            ) :
            base(@object, id, name, description, values)
        {
            Unit = unit;
        }

        public PhysicalProperty(
            Object @object,
            string id,
            string name,
            T value,
            Unit<string> unit,
            string description = null,
            IDictionary<string, object> values = null
            ) :
            base(@object, id, name, value, description, values)
        {
            Unit = unit;
        }

        #endregion

        public Unit<string> Unit
        {
            get { return unit; }
            set
            {
                Argument.NonNull(value);
                SetPropertyValue(ref unit, value);
            }
        }

        private Unit<string> unit;
    }
}
