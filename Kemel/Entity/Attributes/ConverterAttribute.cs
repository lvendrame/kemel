using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Data;

namespace Kemel.Entity.Attributes
{
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ConverterAttribute : Attribute
    {
        readonly Type itcTypeConverter;

        public ConverterAttribute(Type typeConverter)
        {
            this.itcTypeConverter = typeConverter;
        }

        public ITypeConverter Converter
        {
            get
            {
                return (ITypeConverter)Activator.CreateInstance(itcTypeConverter);
            }
        }
    }
}
