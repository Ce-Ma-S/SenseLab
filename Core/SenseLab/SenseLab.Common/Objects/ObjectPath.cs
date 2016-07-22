using CeMaS.Common.Validation;
using System;

namespace SenseLab.Common.Objects
{
    public static class ObjectPath
    {
        public const string Separator = "/";

        public static string Join(params string[] paths)
        {
            Argument.NonNull(paths, nameof(paths));
            for (int i = 0; i < paths.Length; i++)
                Argument.NonNullOrEmpty(paths[i], string.Format($"{nameof(paths)}[{i}]"));
            return string.Join(Separator, paths);
        }
        public static string[] Split(string path)
        {
            Argument.NonNull(path, nameof(path));
            return path.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        private static readonly string[] separators = new[] { Separator };
    }
}
