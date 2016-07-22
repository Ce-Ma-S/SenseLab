using CeMaS.Common.Collections;
using CeMaS.Common.Identity;
using CeMaS.Common.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CeMaS.Common.Validation
{
    public static class CompositeValidatorHelper
    {
        /// <summary>
        /// Validates <paramref name="value"/> with <paramref name="validators"/>.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="validators">Validators.</param>
        /// <param name="value">Value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="validators"/> is null.</exception>
        public static IEnumerable<ValidationError> Validate<T>(this IEnumerable<IValidator<T>> validators, T value)
        {
            Argument.NonNull(validators, nameof(validators));
            return validators.
                SelectMany(i => i.Validate(value));
        }

        /// <summary>
        /// Gets all validators from <paramref name="validator"/>.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="validator"><paramref name="value"/> validator.</param>
        /// <param name="value">Value.</param>
        public static IEnumerable<IValidator<T>> AllValidators<T>(
            this IValidator<T> validator,
            T value,
            bool includeThis = true
            )
        {
            Validate(validator);
            if (includeThis)
                yield return validator;
            if (validator is ICompositeValidator<T>)
            {
                foreach (var v in ((ICompositeValidator<T>)validator).GetChildren(value))
                {
                    if (v is ICompositeValidator<T>)
                    {
                        foreach (var vv in ((ICompositeValidator<T>)v).AllValidators(value))
                            yield return vv;
                    }
                    else
                    {
                        yield return v;
                    }
                }
            }
        }

        /// <summary>
        /// Whether <paramref name="validator"/>`s <see cref="IValidator{TValue}.Scope"/> is <paramref name="scope"/>.
        /// </summary>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <typeparam name="TScope">Scope type.</typeparam>
        /// <param name="validator">Validator.</param>
        /// <param name="scope">
        /// Required validation scope.
        /// If it is <see cref="ValidationScope.Whole"/>, <paramref name="validator"/> validates it as well regardless of its <see cref="IValidator{TValue}.Scope"/>.
        /// If null, <see cref="IValidator{TValue}.Scope"/>`s type must be <typeparamref name="TScope"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="validator"/> is null.</exception>
        public static bool ValidatesScope<TValue, TScope>(
            this IValidator<TValue> validator,
            TScope scope = null
            )
            where TScope : ValidationScope
        {
            Validate(validator);
            return
                validator != null &&
                (
                    scope == null && validator.Scope is TScope ||
                    scope != null &&
                    (
                        scope == ValidationScope.Whole ||
                        validator.Scope == scope
                    )
                );
        }

        /// <summary>
        /// Gets validators having true <see cref="ValidatesScope"/>.
        /// </summary>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <typeparam name="TScope">Scope type.</typeparam>
        /// <param name="validator"><paramref name="value"/> validator.</param>
        /// <param name="value">Value.</param>
        /// <param name="scope">
        /// Required validation scope.
        /// If it is <see cref="ValidationScope.Whole"/>, only <paramref name="validator"/> is returned.
        /// If null, <see cref="IValidator{TValue}.Scope"/>`s type must be <typeparamref name="TScope"/>.
        /// </param>
        public static IEnumerable<IValidator<TValue>> OfScope<TValue, TScope>(
            this IValidator<TValue> validator,
            TValue value,
            TScope scope = null
            )
            where TScope : ValidationScope
        {
            Validate(validator);
            if (scope == ValidationScope.Whole)
                return validator.ToEnumerable();
            var propertyValidators = validator.
                AllValidators(value, true).
                Where(i => i.ValidatesScope(scope));
            return propertyValidators;
        }

        /// <summary>
        /// Wraps <paramref name="validators"/> to <see cref="CompositeValidator{T}"/>.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="validators">Validators.</param>
        public static CompositeValidator<T> ToComposite<T>(this IEnumerable<IValidator<T>> validators)
        {
            return new CompositeValidator<T>(validators);
        }

        #region Add

        public static TValidator Add<TValidator, TValue>(this CompositeValidator<TValue> source, TValidator validator)
            where TValidator : IValidator<TValue>
        {
            Argument.NonNull(source, nameof(source));
            Validate(validator);
            source.Validators.Add(validator);
            return validator;
        }

        #region Property

        public static PropertyValidator<I, V> AddPropertyValidator<I, V>(
            this CompositeValidator<I> source,
            Property<I, V> property,
            Func<I, Func<V, IEnumerable<ValidationError>>> validation,
            bool cachePropertyValueValidators = true
            )
        {
            Argument.NonNull(validation, nameof(validation));
            return source.Add(new PropertyValidator<I, V>(
                property,
                validation,
                cachePropertyValueValidators
                ));
        }

        public static PropertyValidator<I, V> AddPropertyValidator<I, V>(
            this CompositeValidator<I> source,
            string propertyName,
            Func<I, Func<V, IEnumerable<ValidationError>>> validation,
            IdentityInfo info = null,
            bool cachePropertyValueValidators = true
            )
        {
            return source.AddPropertyValidator(
                new Property<I, V>(propertyName, info),
                validation,
                cachePropertyValueValidators
                );
        }

        public static PropertyValidator<I, V> AddPropertyValidator<I, V>(
            this CompositeValidator<I> source,
            Property<I, V> property,
            Func<V, IEnumerable<ValidationError>> validation
            )
        {
            Argument.NonNull(validation, nameof(validation));
            return source.Add(new PropertyValidator<I, V>(property, validation));
        }

        public static PropertyValidator<I, V> AddPropertyValidator<I, V>(
            this CompositeValidator<I> source,
            string propertyName,
            Func<V, IEnumerable<ValidationError>> validation,
            IdentityInfo info = null
            )
        {
            return source.AddPropertyValidator(
                new Property<I, V>(propertyName, info),
                validation
                );
        }

        #endregion

        #endregion

        private static void Validate<T>(IValidator<T> validator)
        {
            Argument.NonNull(validator, nameof(validator));
        }
    }
}
