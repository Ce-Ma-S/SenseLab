using CeMaS.Common.Identity;
using CeMaS.Common.Validation;

namespace CeMaS.Common.Units
{
    public abstract class SourceTargetUnitConverter<T, TValue, TUnit> :
        UnitConverter<T, TValue, TUnit>
        where TUnit : class, IUnit<T>
    {
        public SourceTargetUnitConverter(
            TUnit source,
            TUnit target
            )
        {
            Source = source;
            Target = target;
        }

        public TUnit Source
        {
            get { return source; }
            set
            {
                Argument.NonNull(value);
                SetPropertyValue(ref source, value);
            }
        }
        public TUnit Target
        {
            get { return target; }
            set
            {
                Argument.NonNull(value);
                SetPropertyValue(ref target, value);
            }
        }

        public virtual bool IsForwardConversion(TUnit source, TUnit target)
        {
            return
                source.IdIsEqualTo(Source) &&
                target.IdIsEqualTo(Target);
        }
        public virtual bool IsBackwardConversion(TUnit source, TUnit target)
        {
            return
                target.IdIsEqualTo(Source) &&
                source.IdIsEqualTo(Target);
        }

        protected override bool DoCanConvert(TUnit source, TUnit target)
        {
            return
                IsForwardConversion(source, target) ||
                IsBackwardConversion(source, target);
        }

        private TUnit source;
        private TUnit target;
    }
}