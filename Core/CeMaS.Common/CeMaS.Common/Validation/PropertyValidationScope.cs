namespace CeMaS.Common.Validation
{
    public class PropertyValidationScope :
        ValidationScope
    {
        public PropertyValidationScope(string propertyName, string name = null) :
            base(name ?? propertyName)
        {
            propertyName.ValidateNonNullOrEmpty(nameof(propertyName));
            PropertyName = propertyName;
        }

        public string PropertyName { get; }
    }
}