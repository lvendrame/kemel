using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Orm.QueryDef
{
    public class UnionCollection : List<Union>
    {
        new public Union Add(Union item)
        {
            base.Add(item);
            return item;
        }
    }

    public class Union
    {
        public Union(Query parent, bool isUnionAll, Query unionQuery)
        {
            this.Parent = parent;
            this.IsUnionAll = isUnionAll;
            this.UnionQuery = unionQuery;
            this.UnionQuery.Parent = parent;
        }


        public Query Parent { get; set; }

        public bool IsUnionAll { get; set; }

        public Query UnionQuery { get; set; }

    }
}
