using System;
using System.Collections.Generic;
using System.Text;

namespace Kemel.Orm.Entity.Attributes
{
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TableSchemaTypeAttribute : Attribute
    {
        public SchemaType SchemaType { get; private set; }

        // This is a positional argument
        public TableSchemaTypeAttribute(SchemaType schemaType)
        {
            this.SchemaType = schemaType;
        }
    }
}
