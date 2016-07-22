using CeMaS.Common.Identity;

namespace CeMaS.Common.Units
{
    /// <summary>
    /// Unit of values.
    /// </summary>
    /// <typeparam name="T">Identifier type.</typeparam>
    public interface IUnit<T> :
        IIdentity<T>
    {
        /// <summary>
        /// Symbol.
        /// </summary>
        /// <value>non-empty</value>
        string Symbol { get; }
    }
}