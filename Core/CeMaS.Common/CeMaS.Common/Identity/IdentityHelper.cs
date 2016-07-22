using CeMaS.Common.Collections;
using CeMaS.Common.Conditions;
using CeMaS.Common.Properties;
using CeMaS.Common.Validation;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;

namespace CeMaS.Common.Identity
{
    public static class IdentityHelper
    {
        #region Equals

        public static bool IdIsEqualTo<T>(this IId<T> value1, IId<T> value2)
        {
            return value1.IsEqualTo(value2, i => i.Id);
        }
        public static bool IdIsEqualTo<T>(this IId<T> value, T id)
        {
            Argument.NonNull(value, nameof(value));
            return value.Id.IsEqualTo(id);
        }
        public static int GetHashCode<T>(this IId<T> value)
        {
            if (value == null)
                return 0;
            T id = value.Id;
            return id == null ?
                0 :
                id.GetHashCode();
        }

        #endregion

        #region IdentityInfo

        public static IdentityInfo ToIdentityInfo(
            this string id,
            ResourceManager resources,
            IDictionary<string, object> values = null,
            CultureInfo culture = null
            )
        {
            Argument.NonNullOrEmpty(id, nameof(id));
            Argument.NonNull(resources, nameof(resources));
            string nameId = id.ResourceName(nameof(IdentityInfo.Name));
            string name = GetString(resources, nameId, culture);
            Argument.Validate(name, v => !string.IsNullOrEmpty(v), $"Missing Name resource '{nameId}'.", nameof(id));
            return new IdentityInfo(
                name,
                GetString(resources, id.ResourceName(nameof(IdentityInfo.Description)), culture),
                values
                );
        }

        public static IdentityInfo ToIdentityInfo(this IIdentityInfo source)
        {
            Argument.NonNull(source, nameof(source));
            return source is IdentityInfo ?
                (IdentityInfo)source :
                Copy(source, false);
        }

        private static string GetString(
            ResourceManager resources,
            string nameId,
            CultureInfo culture = null
            )
        {
            return culture == null ?
                resources.GetString(nameId) :
                resources.GetString(nameId, culture);
        }

        private static string ResourceName(
           this string id,
           string name
           )
        {
            return $"{id}_{name}";
        }

        #endregion

        #region Copy

        public static IdentityInfo Copy(this IIdentityInfo source)
        {
            Argument.NonNull(source, nameof(source));
            return Copy(source, true);
        }
        public static void EnsureImageId(this IdentityInfo info, string id)
        {
            Argument.NonNull(info, nameof(info));
            info.Values.SetValueIfMissing(Display.Values.ImageId, id);
        }

        private static IdentityInfo Copy(IIdentityInfo source, bool alwaysNewValues)
        {
            return new IdentityInfo(
                source.Name,
                source.Description,
                alwaysNewValues ?
                    source.Values.Copy() :
                    source.Values.ToDictionary()
                );
        }

        #endregion

        public static IDictionary<string, object> InitEnsureReadOnlyAsWell(IDictionary<string, object> values, string name = null)
        {
            if (values != null)
                Argument.Is<IReadOnlyDictionary<string, object>>(values, name ?? nameof(values));
            else
                values = new Dictionary<string, object>();
            return values;
        }
    }
}
