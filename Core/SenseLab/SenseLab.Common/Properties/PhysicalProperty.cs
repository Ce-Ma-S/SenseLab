using CeMaS.Common.Identity;
using CeMaS.Common.Units;
using CeMaS.Common.Validation;
using SenseLab.Common.Objects;

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
            IdentityInfo info,
            Unit unit
            ) :
            base(@object, id, info)
        {
            Unit = unit;
        }

        public PhysicalProperty(
            Object @object,
            string id,
            IdentityInfo info,
            T value,
            Unit unit
            ) :
            base(@object, id, info, value)
        {
            Unit = unit;
        }

        #endregion

        public Unit Unit
        {
            get { return unit; }
            set
            {
                value.ValidateNonNull();
                SetPropertyValue(ref unit, value);
            }
        }

        private Unit unit;
    }
}
