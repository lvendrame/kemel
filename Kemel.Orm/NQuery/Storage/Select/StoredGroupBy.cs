using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.NQuery.Storage.Column;
using Kemel.Orm.NQuery.Storage;
using Kemel.Orm.NQuery.Storage.Table;
using Kemel.Orm.Base;

namespace Kemel.Orm.NQuery.Storage.StoredSelect
{

    public class StoredGroupByCollection : List<StoredGroupBy>
    {

        public new StoredGroupBy Add(StoredGroupBy item)
        {
            base.Add(item);
            return item;
        }

        public StoredGroupBy Find(string name)
        {
            name = name.ToUpper();

            foreach (StoredGroupBy table in this)
            {
                if ((table.Compare(name)))
                {
                    return table;
                }
            }
            return null;
        }

        public StoredGroupBy Find(ITableDefinition tableDef)
        {
            return this.Find(tableDef.Name);
        }

    }

    public class StoredGroupBy : StoredColumn
    {

        public StoredGroupBy(IColumnDefinition columnDef, StoredTypes type, Query query)
            : base(columnDef, type, query)
        {
        }

        public StoredGroupBy(IColumnDefinition columnDef, StoredTypes type, StoredTable table, Query query)
            : base(columnDef, type, table, query)
        {
        }

        private new Query As(string p_alias)
        {
            return base.As(p_alias).Parent;
        }

        public Query EndGroupBy()
        {
            return base.Query;
        }

    }

}
