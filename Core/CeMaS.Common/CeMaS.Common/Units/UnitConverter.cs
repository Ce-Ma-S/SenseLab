using CeMaS.Common.Events;
using CeMaS.Common.Validation;

namespace CeMaS.Common.Units
{
    /// <summary>
    /// <see cref="IUnitConverter{T, TValue, TUnit}"/> base.
    /// </summary>
    /// <typeparam name="T">Unit identifier type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
    /// <typeparam name="TUnit">Unit type.</typeparam>
    public abstract class UnitConverter<T, TValue, TUnit> :
        NotifyPropertyChange,
        IUnitConverter<T, TValue, TUnit>
        where TUnit : class, IUnit<T>
    {
        public bool CanConvert(TUnit source, TUnit target)
        {
            Validate(source, target);
            return DoCanConvert(source, target);
        }
        public TValue Convert(TValue value, TUnit source, TUnit target)
        {
            Validate(source, target);
            return DoConvert(value, source, target);
        }

        protected abstract bool DoCanConvert(TUnit source, TUnit target);
        protected abstract TValue DoConvert(TValue value, TUnit source, TUnit target);

        private static void Validate(TUnit source, TUnit target)
        {
            Argument.NonNull(source, nameof(source));
            Argument.NonNull(target, nameof(target));
        }
    }
}