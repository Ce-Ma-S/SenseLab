using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CeMaS.Common
{
    /// <summary>
    /// Helps with expressions.
    /// </summary>
    public static class ExpressionHelper
    {
        /// <summary>
        /// Gets member info from <paramref name="member"/>.
        /// </summary>
        /// <param name="member">Member lambda with <see cref="MemberExpression"/>.</param>
        public static MemberInfo MemberInfo(this LambdaExpression member)
        {
            return MemberExpression(member.Body).Member;
        }

        #region Property

        /// <summary>
        /// Gets property info from <paramref name="property"/>.
        /// </summary>
        /// <param name="property">Property lambda with <see cref="MemberExpression"/>.</param>
        public static PropertyInfo PropertyInfo(this LambdaExpression property)
        {
            return (PropertyInfo)property.MemberInfo();
        }
        /// <summary>
        /// Gets property infos from <paramref name="property"/>.
        /// </summary>
        /// <param name="property">Property lambda with (multiple) <see cref="MemberExpression"/>(s).</param>
        public static PropertyInfo[] PropertyInfos(this LambdaExpression property)
        {
            var propertyInfos = new Stack<PropertyInfo>();
            var memberExpression = MemberExpression(property.Body);
            propertyInfos.Push((PropertyInfo)memberExpression.Member);
            while ((memberExpression = memberExpression.Expression as MemberExpression) != null)
                propertyInfos.Push((PropertyInfo)memberExpression.Member);
            return propertyInfos.ToArray();
        }
        /// <summary>
        /// Gets property name from <paramref name="property"/>.
        /// </summary>
        /// <param name="property">Property lambda with <see cref="MemberExpression"/>.</param>
        public static string PropertyName(this LambdaExpression property)
        {
            return property.PropertyInfo().Name;
        }
        /// <summary>
        /// Gets property path from <paramref name="property"/>.
        /// </summary>
        /// <param name="property">Property lambda with (multiple) <see cref="MemberExpression"/>(s).</param>
        /// <value>Dot '.' separated properties path.</value>
        public static string PropertyPath(this LambdaExpression property)
        {
            return string.Join(
                ".",
                property.PropertyInfos().
                Select(i => i.Name)
                );
        }

        #endregion

        #region Field

        /// <summary>
        /// Gets field info from <paramref name="field"/>.
        /// </summary>
        /// <param name="field">Field lambda with <see cref="MemberExpression"/>.</param>
        public static FieldInfo FieldInfo(this LambdaExpression field)
        {
            return (FieldInfo)field.MemberInfo();
        }
        /// <summary>
        /// Gets field name from <paramref name="field"/>.
        /// </summary>
        /// <param name="field">Field lambda with <see cref="MemberExpression"/>.</param>
        public static string FieldName(this LambdaExpression field)
        {
            return field.FieldInfo().Name;
        }

        #endregion

        public static ConstantExpression ConstantExpression<T>(this T value)
        {
            return Expression.Constant(value, typeof(T));
        }

        private static MemberExpression MemberExpression(Expression expression)
        {
            if (expression is UnaryExpression)
            {
                var ue = (UnaryExpression)expression;
                // support simple conversions like short to short? etc.
                if (ue.NodeType == ExpressionType.Convert)
                    expression = ue.Operand;
            }
            return (MemberExpression)expression;
        }
    }
}
