using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.NQuery.Storage.Column;
using Kemel.Orm.NQuery.Storage;
using Kemel.Orm.NQuery.Storage.Table;

namespace Kemel.Orm.NQuery.Storage.StoredSelect
{

    public class StoredUnionCollection : List<StoredUnion>
    {

        public new StoredUnion Add(StoredUnion item)
        {
            base.Add(item);
            return item;
        }

    }

    public class StoredUnion : IGetQuery
    {

        public StoredUnion(Query query, bool isUnionAll, Query unionQuery)
        {
            this.Query = query;
            this.IsUnionAll = isUnionAll;
            this.UnionQuery = unionQuery;
            this.UnionQuery.Parent = query;
        }

        private Query objQuery = null;
        public Query Query
        {
            get { return this.objQuery; }
            set { this.objQuery = value; }
        }


        private bool objIsUnionAll = false;
        public bool IsUnionAll
        {
            get { return this.objIsUnionAll; }
            set { this.objIsUnionAll = value; }
        }

        private Query objUnionQuery = null;
        public Query UnionQuery
        {
            get { return this.objUnionQuery; }
            set { this.objUnionQuery = value; }
        }

        public Query EndUnion()
        {
            return this.Query;
        }

        public Storage.Query GetQuery()
        {
            return this.Query;
        }

        public void SetQuery(Storage.Query query)
        {
            this.Query = query;
        }
    }

}
