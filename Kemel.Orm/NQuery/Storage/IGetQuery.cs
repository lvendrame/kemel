using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Kemel.Orm.NQuery.Storage
{

    public interface IGetQuery
    {

        Query GetQuery();

        void SetQuery(Query query);
    }

}
