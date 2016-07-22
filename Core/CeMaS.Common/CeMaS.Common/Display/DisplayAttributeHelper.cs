using CeMaS.Common.Identity;
using CeMaS.Common.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CeMaS.Common.Display
{
    /// <summary>
    /// <see cref="DisplayAttribute"/> helper.
    /// </summary>
    public static class DisplayAttributeHelper
    {
        public static string Name(this PropertyInfo propertyInfo)
        {
            return DisplayValue(propertyInfo, i => i.GetName());
        }
        public static string Description(this PropertyInfo propertyInfo)
        {
            return DisplayValue(propertyInfo, i => i.GetDescription());
        }
        public static string ShortName(this PropertyInfo propertyInfo)
        {
            return DisplayValue(propertyInfo, i => i.GetShortName());
        }
        public static string GroupName(this PropertyInfo propertyInfo)
        {
            return DisplayValue(propertyInfo, i => i.GetGroupName());
        }
        public static string Prompt(this PropertyInfo propertyInfo)
        {
            return DisplayValue(propertyInfo, i => i.GetPrompt());
        }
        public static int? Order(this PropertyInfo propertyInfo)
        {
            return DisplayValue(propertyInfo, i => i.GetOrder());
        }

        public static IdentityInfo EnsureDisplayInfo(this PropertyInfo propertyInfo, IdentityInfo info = null)
        {
            if (info == null)
            {
                info = new IdentityInfo(
                    propertyInfo.Name() ?? propertyInfo.Name,
                    propertyInfo.Description()
                    );
            }
            else
            {
                if (info.Description == null)
                    info.Description = propertyInfo.Description();
            }
            info.Values.
                SetValueIfMissing(Values.ShortName, propertyInfo.ShortName()).
                SetValueIfMissing(Values.GroupName, propertyInfo.GroupName()).
                SetValueIfMissing(Values.Prompt, propertyInfo.Prompt()).
                SetValueIfMissing(Properties.Values.Order, propertyInfo.Order());
            return info;
        }

        private static T DisplayValue<T>(
            PropertyInfo propertyInfo,
            Func<DisplayAttribute, T> getValue,
            T defaultValue = default(T)
            )
        {
            T result = defaultValue;
            var display = propertyInfo.Attribute<DisplayAttribute>();
            if (display != null)
                result = getValue(display);
            return result;
        }
    }
}
