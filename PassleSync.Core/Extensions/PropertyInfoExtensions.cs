using System;
using System.Reflection;

namespace PassleSync.Core.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static bool HasAttribute(this PropertyInfo propertyInfo, Type attribute)
        {
            return propertyInfo.IsDefined(attribute);
        }
    }
}
