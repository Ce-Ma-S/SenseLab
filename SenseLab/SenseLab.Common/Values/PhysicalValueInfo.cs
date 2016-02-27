using CeMaS.Common.Identity;
using CeMaS.Common.Units;
using CeMaS.Common.Validation;
using SenseLab.Common.Values;
using System;

namespace SenseLab.Common.Commands
{
    public class PhysicalValueInfo :
        ValueInfo,
        IPhysicalValueInfo
    {
        #region Init

        public PhysicalValueInfo(
            string id,
            IdentityInfo info,
            Type type,
            Unit unit
            ) :
            base(id, info, type)
        {
            Unit = unit;
        }

        public PhysicalValueInfo(
            IPhysicalValueInfo value
            ) :
            base(value)
        {
            Unit = value.Unit;
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