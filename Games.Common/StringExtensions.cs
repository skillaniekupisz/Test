using System;
using System.Collections.Generic;
using System.Text;

namespace Games.Common
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string value)
        {
            return string.IsNullOrWhiteSpace(value) || value.Length < 2
                ? value
                : char.ToLowerInvariant(value[0]) + value.Substring(1);
        }
    }
}
