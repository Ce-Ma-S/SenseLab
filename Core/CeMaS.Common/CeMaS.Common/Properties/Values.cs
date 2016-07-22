namespace CeMaS.Common.Properties
{
    /// <summary>
    /// Common values.
    /// </summary>
    public static class Values
    {
        /// <summary>
        /// Whether an object tagged with this value is required.
        /// Default value: <c>false</c>.
        /// </summary>
        public static readonly NamedValue<bool> IsRequired = new NamedValue<bool>(nameof(IsRequired));
        /// <summary>
        /// Order index.
        /// Default value: <c>false</c>.
        /// </summary>
        public static readonly NamedValue<int?> Order = new NamedValue<int?>(nameof(Order));
    }
}
