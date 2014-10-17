using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Kemel.Orm.NQuery.Storage.Constraint
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
        StartsWith,
        //Like '_____%'
        EndsWith,
        //Like '%_____'
        Contains
        //Like '%____%'
    }

}
