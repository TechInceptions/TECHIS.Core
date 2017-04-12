
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace TECHIS.Core
{ 
    internal static class TypeExtensions
    {
        public const string DbNullValue = "5896F2751B614DFA886F86C2DDCE4FA3";
        public const TypeCode TypeCodeDbNull = (TypeCode)2;

        public static TypeCode GetTypeCode(this Type type)
        {
            if (!type.IsValueType())
            {
                return TypeCode.Object;
            }
            return GetTypeCode(Activator.CreateInstance(type));
        }
        public static TypeCode GetTypeCode(object request)
        {
            var convertible = request as IConvertible;
            if (convertible == null)
            {
                return TypeCode.Object;
            }

            return convertible.GetTypeCode();            
        }

        public static bool IsValueType(this Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }
    }
}