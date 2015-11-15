using CeMaS.Common.Validation;
using SenseLab.Common.Commands;
using SenseLab.Common.Events;
using SenseLab.Common.Properties;

namespace SenseLab.Common.Objects
{
    public static class ObjectHelper
    {
        public static IProperty<T> Property<T>(this IObject @object, string id)
        {
            @object.ValidateNonNull(nameof(@object));
            return @object[id] as IProperty<T>;
        }

        public static ICommand Command(this IObject @object, string id)
        {
            @object.ValidateNonNull(nameof(@object));
            return @object[id] as ICommand;
        }

        public static IEvent Event(this IObject @object, string id)
        {
            @object.ValidateNonNull(nameof(@object));
            return @object[id] as IEvent;
        }
    }
}
