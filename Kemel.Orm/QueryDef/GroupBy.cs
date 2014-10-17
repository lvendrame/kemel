using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Schema;

namespace Kemel.Orm.QueryDef
{
    public class GroupByCollection : List<GroupBy>
    {

        new public GroupBy Add(GroupBy item)
        {
            base.Add(item);
            return item;
        }
    }


    public class GroupBy: ColumnQuery
    {

        public GroupBy(ColumnSchema columnSchema, TableQuery parent)
            : base(columnSchema, parent)
        {
        }

        public GroupBy(string columnName, TableQuery parent)
            : base(columnName, parent)
        {
        }

        public GroupBy(string alias)
            : base(alias)
        {
        }

        new public Query As(string alias)
        {
            return base.As(alias).Parent;
        }

        new public Query End()
        {
            return base.End().Parent;
        }

    }
}
