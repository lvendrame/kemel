using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Schema
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
