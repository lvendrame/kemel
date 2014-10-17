using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Orm.Entity.Attributes
{

    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ColumnUseTypeAttribute : Attribute
    {
        public ColumnType Type { get; set; }
        public ColumnUseTypeAttribute(ColumnType type)
        {
            this.Type = type;
        }
    }
}
