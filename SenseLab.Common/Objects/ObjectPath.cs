using CeMaS.Common.Validation;
using System;

namespace SenseLab.Common.Objects
{
    public static class ObjectPath
    {
        public const string Delimiter = "/";

        public static string Join(params string[] paths)
        {
            paths.ValidateNonNull(nameof(paths));
            for (int i = 0; i < paths.Length; i++)
                paths[i].ValidateNonEmpty(string.Format($"{nameof(paths)}[{i}]"));
            return string.Join(Delimiter, paths);
        }
        public static string[] Split(string path)
        {
            path.ValidateNonNull(nameof(path));
            return path.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }

        private static readonly string[] delimiters = new[] { Delimiter };
    }
}
