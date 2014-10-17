using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Orm.QueryDef
{
    public enum AggregateFunction
    {
        Count,
        Sum,
        Avg,
        Min,
        Max,
        StDev,
        StDevP,
        Var,
        VarP,
        Cast,
        Convert
    }
}
