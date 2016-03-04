using CeMaS.Common.Validation;
using System;

namespace SenseLab.Common.Objects
{
    public static class ObjectPath
    {
        public const string Separator = "/";

        public static string Join(params string[] paths)
        {
            paths.ValidateNonNull(nameof(paths));
            for (int i = 0; i < paths.Length; i++)
                paths[i].ValidateNonNullOrEmpty(string.Format($"{nameof(paths)}[{i}]"));
            return string.Join(Separator, paths);
        }
        public static string[] Split(string path)
        {
            path.ValidateNonNull(nameof(path));
            return path.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        private static readonly string[] separators = new[] { Separator };
    }
}
