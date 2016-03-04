namespace CeMaS.Common.Validation
{
    public class ValidationScope :
        IValidationScope
    {
        public ValidationScope(string name)
        {
            name.ValidateNonNullOrEmpty(nameof(name));
            Name = name;
        }

        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}