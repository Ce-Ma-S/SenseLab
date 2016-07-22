namespace CeMaS.Common.Units
{
    /// <summary>
    /// Converts values between units.
    /// </summary>
    /// <typeparam name="T">Unit identifier type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
    /// <typeparam name="TUnit">Unit type.</typeparam>
    public interface IUnitConverter<T, TValue, TUnit>
        where TUnit : class, IUnit<T>
    {
        /// <summary>
        /// Whether conversion from <paramref name="source"/> to <paramref name="target"/> is supported.
        /// </summary>
        /// <param name="source">Source unit.</param>
        /// <param name="target">Target unit.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="target"/> is null.</exception>
        bool CanConvert(TUnit source, TUnit target);
        /// <summary>
        /// Converts <paramref name="value"/> from <paramref name="source"/> to <paramref name="target"/> unit.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="source">Source unit.</param>
        /// <param name="target">Target unit.</param>
        /// <exception cref="System.ArgumentException"><paramref name="value"/> is invalid.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="target"/> is null.</exception>
        TValue Convert(TValue value, TUnit source, TUnit target);
    }


    //public interface IUnitConverter<TValue> :
    //    IUnitConverter<TValue, IUnit>
    //{ }
}