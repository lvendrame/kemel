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

namespace Kemel.Orm.NQuery.Storage.Function.Scalar
{

    public class FunctionConvert : SequenciaParamStoredFunction
    {

        public FunctionConvert()
            : base(Functions.CONVERT)
        {
        }

        public FunctionConvert(Query query)
            : base(Functions.CONVERT, query)
        {
        }

    }

}
