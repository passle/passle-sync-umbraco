using System;
using System.ComponentModel;
using System.Linq;

namespace PassleSync.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum input)
        {
            var fieldInfo = input.GetType().GetField(input.ToString());
            if (fieldInfo == null)
            {
                return null;
            }

            var attributes = (DescriptionAttribute[]) fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes.First().Description ?? input.ToString();
            }

            return input.ToString();
        }
    }
}
