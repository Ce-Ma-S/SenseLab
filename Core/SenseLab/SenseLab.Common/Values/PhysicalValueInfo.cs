using CeMaS.Common.Units;
using CeMaS.Common.Validation;
using SenseLab.Common.Values;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Commands
{
    public class PhysicalValueInfo :
        ValueInfo,
        IPhysicalValueInfo
    {
        #region Init

        public PhysicalValueInfo(
            string id,
            string name,
            Type type,
            Unit<string> unit,
            string description = null,
            IDictionary<string, object> values = null
            ) :
            base(id, name, type, description, values)
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