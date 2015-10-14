using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Entity;
using System.Data;
using Kemel.Orm.Schema;
using Kemel.Orm.Constants;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml;

namespace Kemel
{

    public static class TypeExtensions
    {
        public static bool IsNullable(this Type self)
        {
            if (!self.IsGenericType)
                return false;

            return self.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

        public static Type GetFinalValueType(this Type self)
        {
            return Nullable.GetUnderlyingType(self) ?? self;
        }

        public static bool IsEntity(this Type self)
        {
            return self.IsSubclassOf(typeof(EntityBase));
        }

        public static DbType GetDbType(this Type self)
        {
            if (self == typeof(string))
            {
                return DbType.AnsiString;
            }
            else if (self == typeof(int))
            {
                return DbType.Int32;
            }
            else if (self == typeof(double))
            {
                return DbType.Decimal;
            }
            else if (self == typeof(decimal))
            {
                return DbType.Decimal;
            }
            else if (self == typeof(bool))
            {
                return DbType.Boolean;
            }
            else if (self == typeof(DateTime))
            {
                return DbType.DateTime;
            }
            else if (self == typeof(char))
            {
                return DbType.AnsiString;
            }
            else if (self == typeof(short))
            {
                return DbType.Int16;
            }
            else if (self == typeof(long))
            {
                return DbType.Int64;
            }
            else if (self == typeof(float))
            {
                return DbType.Double;
            }
            else if (self == typeof(Guid))
            {
                return DbType.Guid;
            }
            else if (self == typeof(byte))
            {
                return DbType.Byte;
            }
            else if (self == typeof(object))
            {
                return DbType.Object;
            }
            else if (self == typeof(byte[]))
            {
                return DbType.Binary;
            }
            else if (self == typeof(TimeSpan))
            {
                return DbType.Time;
            }
            else if (self == typeof(DBNull))
            {
                return DbType.AnsiString;
            }
            else
            {
                return DbType.Object;
            }
        }

        public static PropertyInfo[] GetPublicDeclaredProperties(this Type self)
        {
            return self.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

    }
}
