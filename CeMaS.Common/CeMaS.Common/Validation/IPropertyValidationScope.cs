namespace CeMaS.Common.Validation
{
    /// <summary>
    /// Specifies property validation scope.
    /// </summary>
    public interface IPropertyValidationScope :
        IValidationScope
    {
        /// <summary>
        /// Property name.
        /// </summary>
        string PropertyName { get; }
    }
}