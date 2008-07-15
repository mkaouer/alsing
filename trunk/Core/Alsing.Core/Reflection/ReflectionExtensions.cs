﻿using System;
using System.Linq;
using System.Reflection;

namespace Alsing
{
    public static class ReflectionExtensions
    {
        public static bool HasAttribute<T>(this MethodBase method)
        {
            return method
                .GetCustomAttributes(typeof (T), true)
                .HasContent();
        }

        public static bool HasAttribute<T>(this Type type)
        {
            return type
                .GetCustomAttributes(typeof (T), true)
                .HasContent();
        }

        public static FieldInfo GetAnyField(this Type type,string fieldName)
        {
            return GetFieldInfo(type, fieldName);
        }

        private static FieldInfo GetFieldInfo(Type type, string fieldName)
        {
            if (type == null)
                return null;

            var allFields = from f in type.GetFields(BindingFlags.Public |
                                                     BindingFlags.NonPublic |
                                                     BindingFlags.Instance)
                            where f.Name.ToLowerInvariant() == fieldName.ToLowerInvariant()
                            select f;

            FieldInfo field = allFields.FirstOrDefault();
            if (field != null)
                return field;

            return GetFieldInfo(type.BaseType, fieldName);
        }

        public static string GetTypeName(this Type type)
        {
            if (type.IsGenericType)
            {

                var argNames = (from argType in type.GetGenericArguments()
                                select GetTypeName(argType)).ToArray();

                string args = string.Join(",", argNames);

                string typeName = type.Name;
                int index = typeName.IndexOf("`");
                typeName = typeName.Substring(0, index);

                return string.Format("{0}[of {1}]", typeName, args);
            }
            return type.Name;
        }
    }
}