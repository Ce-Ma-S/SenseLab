using CeMaS.Common.Display;
using CeMaS.Common.Identity;
using CeMaS.Common.Validation;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CeMaS.Common.Properties
{
    /// <summary>
    /// Information about an object`s property.
    /// </summary>
    public class Property :
        IdentityWithInfo<string>,
        IProperty
    {
        #region Init

        public Property(
            string propertyName,
            Type valueType,
            bool isWritable,
            bool isReadable = true,
            IdentityInfo info = null
            ) :
            base(
                Argument.NonNullOrEmpty(propertyName, nameof(propertyName)),
                info ?? new IdentityInfo(propertyName)
                )
        {
            IsReadable = isReadable;
            IsWritable = isWritable;
            ValueType = valueType;
        }

        #endregion

        public string PropertyName
        {
            get { return Id; }
        }
        public bool IsReadable
        {
            get { return isReadable; }
            set
            {
                SetPropertyValue(ref isReadable, value);
            }
        }
        public bool IsWritable
        {
            get { return isWritable; }
            set
            {
                SetPropertyValue(ref isWritable, value);
            }
        }
        public Type ValueType
        {
            get { return valueType; }
            set
            {
                Argument.NonNull(value);
                ValidateValueType(value);
                SetPropertyValue(ref valueType, value);
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()} {ValueType.FullName}";
        }

        protected virtual void ValidateValueType(Type valueType)
        {
            Argument.IsTypeOf(valueType, ValueType, nameof(valueType));
        }

        private Type valueType;
        private bool isReadable;
        private bool isWritable;
    }


    /// <summary>
    /// Information about an object`s property.
    /// </summary>
    /// <typeparam name="T">Property value type.</typeparam>
    public class Property<T> :
        Property
    {
        #region Init

        public Property(
            string propertyName,
            bool isWritable,
            bool isReadable = true,
            IdentityInfo info = null
            ) :
            base(propertyName, typeof(T), isWritable, isReadable, info)
        { }

        public Property(
            Expression<Func<T>> property,
            IdentityInfo info = null
            ) :
            this(
                Argument.NonNull(property, i => i.PropertyInfo(), nameof(property)),
                info
            )
        { }

        public Property(
            PropertyInfo propertyInfo,
            IdentityInfo info = null
            ) :
            base(
                Argument.NonNull(propertyInfo, i => i.Name, nameof(propertyInfo)),
                typeof(T),
                propertyInfo.IsWritable(),
                propertyInfo.IsReadable(),
                propertyInfo.EnsureDisplayInfo(info)
            )
        { }

        #endregion

        protected override void ValidateValueType(Type valueType)
        {
            Argument.Is<T>(valueType, nameof(valueType));
        }
    }


    /// <summary>
    /// Information about an object`s property.
    /// </summary>
    /// <typeparam name="TOwner">Owner type.</typeparam>
    /// <typeparam name="TValue">Property value type.</typeparam>
    public partial class Property<TOwner, TValue> :
        Property<TValue>,
        IProperty<TOwner, TValue>
    {
        #region Init

        public Property(
            string propertyName,
            IdentityInfo info = null
            ) :
            this(
                TypeHelper.PropertyOf<TOwner>(propertyName),
                info
                )
        { }

        public Property(
            Expression<Func<TOwner, TValue>> property,
            IdentityInfo info = null
            ) :
            this(
                Argument.NonNull(property, i => i.PropertyInfo(), nameof(property)),
                info
                )
        { }

        protected Property(
            Property property,
            PropertyInfo propertyInfo
            ) :
            this(
                propertyInfo,
                property.Info.Copy()
                )
        { }

        internal Property(
            PropertyInfo propertyInfo,
            IdentityInfo info = null
            ) :
            base(
                propertyInfo,
                info
                )
        {
            PropertyInfo = propertyInfo;
        }

        protected virtual Property<TNewOwner, TNewValue> CreateProperty<TNewOwner, TNewValue>(
            PropertyInfo propertyInfo
            )
        {
            return new Property<TNewOwner, TNewValue>(this, propertyInfo);
        }

        #endregion

        public Type OwnerType { get; } = typeof(TOwner);
        public bool IsStatic
        {
            get { return PropertyInfo.GetMethod.IsStatic; }
        }

        public IPropertyWithOwner<TNewOwner> ChangeOwner<TNewOwner>()
        {
            IPropertyWithOwner<TNewOwner> result;
            if (typeof(TNewOwner) == typeof(TOwner))
            {
                result = (IPropertyWithOwner<TNewOwner>)this;
            }
            else
            {
                var propertyInfo = TypeHelper.PropertyOf<TNewOwner>(PropertyName);
                result = propertyInfo == null ?
                    null :
                    CreateProperty<TNewOwner, TValue>(propertyInfo);
            }
            return result;
        }
        public TValue GetValue(TOwner owner)
        {
            if (!IsReadable)
                throw new InvalidOperationException("Property is not readable.");
            return DoGetValue(owner);
        }
        object IPropertyWithOwner.GetValue(object owner)
        {
            ValidateOwner(owner);
            return GetValue((TOwner)owner);
        }
        public void SetValue(TOwner owner, TValue value)
        {
            if (!IsWritable)
                throw new InvalidOperationException("Property is not writable.");
            DoSetValue(owner, value);
        }
        void IPropertyWithOwner.SetValue(object owner, object value)
        {
            ValidateOwner(owner);
            Argument.Is<TValue>(value, nameof(value), true);
            SetValue((TOwner)owner, (TValue)value);
        }

        protected internal PropertyInfo PropertyInfo { get; }

        protected virtual TValue DoGetValue(TOwner owner)
        {
            return (TValue)PropertyInfo.GetValue(owner, null);
        }
        protected virtual void DoSetValue(TOwner owner, TValue value)
        {
            PropertyInfo.SetValue(owner, value, null);
        }

        private void ValidateOwner(object owner)
        {
            Argument.Is<TOwner>(owner, nameof(owner), IsStatic);
        }
    }
}
