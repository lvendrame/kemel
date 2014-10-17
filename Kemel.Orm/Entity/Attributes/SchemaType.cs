using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Orm.Entity.Attributes
{
    public enum SchemaType
    {
        Table,
        View,
        Procedure,
        ScalarFunction,
        TableFunction,
        AggregateFunction,
        None
    }
}
