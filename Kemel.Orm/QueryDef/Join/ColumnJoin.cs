using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Schema;

namespace Kemel.Orm.QueryDef
{
    public class ColumnJoinCollection : List<ColumnJoin>
    {

        new public ColumnJoin Add(ColumnJoin item)
        {
            base.Add(item);
            return item;
        }
    }


    public class ColumnJoin: ColumnQuery
    {
        public ColumnJoin(ColumnSchema column, QueryJoin parent)
            : base(column, parent)
        {
        }

        public ColumnJoin(string columnName, QueryJoin parent)
            : base(columnName, parent)
        {
        }

        new public QueryJoin As(string alias)
        {
            base.As(alias);
            return this.Parent as QueryJoin;
        }

        new public QueryJoin End()
        {
            return this.Parent as QueryJoin;
        }
    }
}
