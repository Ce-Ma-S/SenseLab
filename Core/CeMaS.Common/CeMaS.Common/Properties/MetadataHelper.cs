using CeMaS.Common.Validation;

namespace CeMaS.Common.Properties
{
    /// <summary>
    /// <see cref="IMetadata"/>/<see cref="IMetadataWritable"/> helper.
    /// </summary>
    public static class MetadataHelper
    {
        public static void CopyTo(this IMetadata source, IMetadataWritable target, bool overwrite)
        {
            source.ValidateNonNull(nameof(source));
            target.ValidateNonNull(nameof(target));
            foreach (var value in source)
            {
                if (overwrite || !target.Contains(value.Key))
                    target[value.Key] = value.Value;
            }
        }

        public static Metadata Copy(this IMetadata source)
        {
            return new Metadata(source);
        }
    }
}
