using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.NQuery.Storage.Value;
using Kemel.Orm.Schema;
using Kemel.Orm.NQuery.Storage.Column;
using Kemel.Orm.Entity;
using Kemel.Orm.NQuery.Storage.Table;
using Kemel.Orm.Constants;

namespace Kemel.Orm.NQuery.Storage.Function.Aggregated
{

    public class FunctionMin : SequenciaParamStoredFunction
    {

        public FunctionMin()
            : base(Functions.MIN)
        {
        }

        public FunctionMin(Query query)
            : base(Functions.MIN, query)
        {
        }

    }

}
