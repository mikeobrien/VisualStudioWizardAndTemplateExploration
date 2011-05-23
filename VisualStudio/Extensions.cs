using System;
using System.Collections.Generic;
using System.IO;

namespace VisualStudio
{
    public static class Extensions
    {
        public static string FormatWith(this string value, params object[] args)
        {
            return string.Format(value, args);
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static Maybe<Exception> MaybeException<T>(this T value, Action<T> action)
        {
            try
            {
                action(value);
                return Maybe.Nothing<Exception>();
            }
            catch (Exception e)
            {
                return Maybe.Just(e);
            }
        }

        public static IDictionary<TKey, TValue> Concat<TKey, TValue>(this IDictionary<TKey, TValue> a, IDictionary<TKey, TValue> b)
        {
            if (a == null && b == null) return null;
            if (a != null && b == null) return a;
            if (a == null) return b;
            foreach (var pair in b) a.Add(pair.Key, pair.Value);
            return a;
        }

        public static string ReadAllText(this Stream stream)
        {
            using (var reader = new StreamReader(stream)) return reader.ReadToEnd();
        }

        public static string AsString(this object value)
        {
            return value == null || value == DBNull.Value ? null : value.ToString();
        }

        public static string CombinePath(this string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        public static string AppendExtension(this string path, string extension)
        {
            return "{0}.{1}".FormatWith(path, extension);
        }
    }
}
