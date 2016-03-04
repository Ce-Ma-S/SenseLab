namespace CeMaS.Common.Identity
{
    /// <summary>
    /// Identifiable object with changeable identifier.
    /// </summary>
    /// <typeparam name="T">Identifier type.</typeparam>
    public interface IIdWritable<T> :
        IId<T>
    {
        /// <summary>
        /// Identifier of this object.
        /// </summary>
        new T Id { get; set; }
    }
}
