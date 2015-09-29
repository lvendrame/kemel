using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.Kuery
{
    public enum ComparisonOperator
    {
        Equal,
        Different,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Between,
        Like,
        NotLike,
        In,
        NotIn,
        IsNull,
        IsNotNull,
        OpenParentheses,
        CloseParentheses,
        StartsWith, //Like '%_____'
        EndsWith,   //Like '_____%'
        Contains    //Like '%____%'
    }
}
