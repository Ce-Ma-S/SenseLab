using CeMaS.Common.Identity;

namespace CeMaS.Common.Units
{
    /// <summary>
    /// Writable <see cref="IUnit{T}"/>.
    /// </summary>
    /// <typeparam name="T">Identifier type.</typeparam>
    public interface IUnitWritable<T> :
        IUnit<T>,
        IIdentityInfoWritable
    {
        new string Symbol { get; set; }
    }
}