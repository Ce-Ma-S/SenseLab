using CeMaS.Common.Validation;
using System.Resources;

namespace CeMaS.Common.Identity
{
    public static class IdentityHelper
    {
        public static IdentityInfo ToIdentityInfo(this string id, ResourceManager resources)
        {
            resources.ValidateNonNull(nameof(resources));
            return new IdentityInfo(
                resources.GetString($"{id}_Name"),
                resources.GetString($"{id}_Description")
                );
        }
    }
}
