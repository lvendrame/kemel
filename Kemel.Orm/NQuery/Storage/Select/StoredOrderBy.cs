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

    public class StoredOrderByCollection : List<StoredOrderBy>
    {

        public new StoredOrderBy Add(StoredOrderBy item)
        {
            base.Add(item);
            return item;
        }

        public StoredOrderBy Find(string name)
        {
            name = name.ToUpper();

            foreach (StoredOrderBy table in this)
            {
                if ((table.Compare(name)))
                {
                    return table;
                }
            }
            return null;
        }

        public StoredOrderBy Find(ITableDefinition tableDef)
        {
            return this.Find(tableDef.Name);
        }

    }

    public class StoredOrderBy : StoredColumn
    {

        private Order objSortOrder = Order.Asc;
        public Order SortOrder
        {
            get { return this.objSortOrder; }
            set { this.objSortOrder = value; }
        }

        public StoredOrderBy(IColumnDefinition columnDef, StoredTypes type, Query query)
            : base(columnDef, type, query)
        {
        }

        public StoredOrderBy(IColumnDefinition columnDef, StoredTypes type, StoredTable table, Query query)
            : base(columnDef, type, table, query)
        {
        }

        private new Query As(string p_alias)
        {
            return base.As(p_alias).Parent;
        }

        public Query Asc()
        {
            this.SortOrder = Order.Asc;
            return this.Query;
        }

        public Query Desc()
        {
            this.SortOrder = Order.Desc;
            return this.Query;
        }

        public Query EndOrderBy()
        {
            return base.Query;
        }

        public enum Order
        {
            Asc,
            Desc,
            DefaultOrder
        }

    }

}
