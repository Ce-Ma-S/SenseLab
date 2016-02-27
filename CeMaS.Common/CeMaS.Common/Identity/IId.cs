namespace CeMaS.Common.Identity
{
    /// <summary>
    /// Identifiable object.
    /// </summary>
    /// <typeparam name="T">Identifier type.</typeparam>
    public interface IId<T>
    {
        /// <summary>
        /// Identifier of this object.
        /// </summary>
        T Id { get; }
    }
}
