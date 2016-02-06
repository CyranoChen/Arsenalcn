using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataReaderMapper.Internal;

namespace DataReaderMapper.Mappers
{
    public static class TypeHelper
    {
        public static Type GetElementType(Type enumerableType)
        {
            return GetElementType(enumerableType, null);
        }

        public static Type GetElementType(Type enumerableType, IEnumerable enumerable)
        {
            if (enumerableType.HasElementType)
            {
                return enumerableType.GetElementType();
            }

            if (TypeExtensions.IsGenericType(enumerableType) &&
                enumerableType.GetGenericTypeDefinition() == typeof (IEnumerable<>))
            {
                return enumerableType.GetGenericArguments()[0];
            }

            var ienumerableType = GetIEnumerableType(enumerableType);
            if (ienumerableType != null)
            {
                return ienumerableType.GetGenericArguments()[0];
            }

            if (typeof (IEnumerable).IsAssignableFrom(enumerableType))
            {
                var first = enumerable?.Cast<object>().FirstOrDefault();

                return first?.GetType() ?? typeof (object);
            }

            throw new ArgumentException($"Unable to find the element type for type '{enumerableType}'.",
                nameof(enumerableType));
        }

        public static Type GetEnumerationType(Type enumType)
        {
            if (PrimitiveExtensions.IsNullableType(enumType))
            {
                enumType = enumType.GetGenericArguments()[0];
            }

            if (!TypeExtensions.IsEnum(enumType))
                return null;

            return enumType;
        }

        private static Type GetIEnumerableType(Type enumerableType)
        {
            try
            {
                return enumerableType.GetInterfaces().FirstOrDefault(t => t.Name == "IEnumerable`1");
            }
            catch (AmbiguousMatchException)
            {
                if (TypeExtensions.BaseType(enumerableType) != typeof (object))
                    return GetIEnumerableType(TypeExtensions.BaseType(enumerableType));

                return null;
            }
        }
    }
}