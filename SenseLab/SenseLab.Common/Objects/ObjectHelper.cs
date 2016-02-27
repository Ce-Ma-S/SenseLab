using CeMaS.Common.Identity;
using CeMaS.Common.Units;
using CeMaS.Common.Validation;
using SenseLab.Common.Commands;
using SenseLab.Common.Events;
using SenseLab.Common.Properties;
using SenseLab.Common.Values;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

namespace SenseLab.Common.Objects
{
    public static class ObjectHelper
    {
        public static IEnumerable<IProperty> Properties<T>(this IObject @object)
        {
            @object.ValidateNonNull(nameof(@object));
            return @object.Items.
                OfType<IProperty>();
        }
        public static IProperty<T> Property<T>(this IObject @object, string id)
        {
            @object.ValidateNonNull(nameof(@object));
            return @object[id] as IProperty<T>;
        }

        public static IEnumerable<ICommand> Commands<T>(this IObject @object)
        {
            @object.ValidateNonNull(nameof(@object));
            return @object.Items.
                OfType<ICommand>();
        }
        public static ICommand Command(this IObject @object, string id)
        {
            @object.ValidateNonNull(nameof(@object));
            return @object[id] as ICommand;
        }

        public static IEnumerable<IEvent> Events<T>(this IObject @object)
        {
            @object.ValidateNonNull(nameof(@object));
            return @object.Items.
                OfType<IEvent>();
        }
        public static IEvent Event(this IObject @object, string id)
        {
            @object.ValidateNonNull(nameof(@object));
            return @object[id] as IEvent;
        }

        #region Convert

        public static ObjectEnvironmentInfo ToInfo(this IObjectEnvironment value)
        {
            value.ValidateNonNull(nameof(value));
            return new ObjectEnvironmentInfo(value);
        }
        public static ObjectInfo ToInfo(this IObject value)
        {
            value.ValidateNonNull(nameof(value));
            return new ObjectInfo(value);
        }
        public static ObjectItemInfo ToInfo(this IObjectItem value)
        {
            value.ValidateNonNull(nameof(value));
            return new ObjectItemInfo(value);
        }
        public static ValueInfo ToValueInfo<T>(this string id, IdentityInfo info)
        {
            return new ValueInfo(id, info, typeof(T));
        }
        public static PhysicalValueInfo ToValueInfo<T>(this string id, IdentityInfo info, Unit unit)
        {
            return new PhysicalValueInfo(id, info, typeof(T), unit);
        }

        #endregion
    }
}
