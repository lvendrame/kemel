using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Data;

namespace Kemel.Entity.Attributes
{
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ConverterAttribute : Attribute
    {
        readonly Type _typeConverter;

        public ConverterAttribute(Type typeConverter)
        {
            this._typeConverter = typeConverter;
        }

        public ITypeConverter Converter
        {
            get
            {
                return (ITypeConverter)Activator.CreateInstance(_typeConverter);
            }
        }
    }
}
