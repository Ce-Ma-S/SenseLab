using CeMaS.Common.Identity;
using CeMaS.Common.Properties;

namespace CeMaS.Common.Validation
{
    /// <summary>
    /// Specifies property validation scope.
    /// </summary>
    public class PropertyValidationScope :
        ValidationScope
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="propertyName"><see cref="PropertyName"/></param>
        /// <param name="name"><see cref="ValidationScope.Name"/>. If null, <paramref name="propertyName"/> is used.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="propertyName"/> is null.</exception>
        public PropertyValidationScope(
            string propertyName,
            IdentityInfo info = null
            ) :
            base(propertyName, info)
        { }

        /// <summary>
        /// Property (system) name.
        /// </summary>
        public string PropertyName
        {
            get {return Id; }
        }
    }

    public class PropertyValidationScope<I, V> :
        PropertyValidationScope
    {
        public PropertyValidationScope(Property<I, V> property) :
            base(
                Argument.NonNull(property, i => i.PropertyName, nameof(property)),
                property.Info
                )
        {
            Property = property;
        }

        public Property<I, V> Property { get; }
    }
}