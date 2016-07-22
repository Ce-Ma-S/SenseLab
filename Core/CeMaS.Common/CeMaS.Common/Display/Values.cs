using CeMaS.Common.Identity;
using CeMaS.Common.Properties;

namespace CeMaS.Common.Display
{
    /// <summary>
    /// Common values.
    /// </summary>
    public static class Values
    {
        /// <summary>
        /// User prompt.
        /// Default value: <c>null</c>.
        /// </summary>
        public static readonly NamedValue<string> Prompt = new NamedValue<string>(nameof(Prompt));
        /// <summary>
        /// Short (column) name.
        /// Default value: <c>null</c>.
        /// </summary>
        public static readonly NamedValue<string> ShortName = new NamedValue<string>(nameof(ShortName));
        /// <summary>
        /// Group name.
        /// Default value: <c>null</c>.
        /// </summary>
        public static readonly NamedValue<string> GroupName = new NamedValue<string>(nameof(GroupName));
        /// <summary>
        /// Group info.
        /// Default value: <see cref="IdentityInfo"/> from <see cref="GroupName"/> if available otherwise <c>null</c>.
        /// </summary>
        public static readonly NamedValue<IIdentityInfo> GroupInfo = new NamedValue<IIdentityInfo>(nameof(GroupInfo),
            defaultValueCallback: m =>
            {
                string name = GroupName.GetValue(m);
                return name == null ?
                    null :
                    new IdentityInfo(name);
            });
        /// <summary>
        /// Image identifier.
        /// Default value: <c>null</c>.
        /// </summary>
        public static readonly NamedValue<string> ImageId = new NamedValue<string>(nameof(ImageId));
    }
}
