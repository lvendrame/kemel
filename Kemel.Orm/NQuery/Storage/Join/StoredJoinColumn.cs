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

namespace Kemel.Orm.NQuery.Storage.Join
{

    public class StorageJoinColumnCollection : List<StoredJoinColumn>
    {

        public new StoredJoinColumn Add(StoredJoinColumn item)
        {
            base.Add(item);
            return item;
        }

        public StoredJoinColumn Find(string name)
        {
            name = name.ToUpper();

            foreach (StoredJoinColumn column in this)
            {
                if ((column.Compare(name)))
                {
                    return column;
                }
            }
            return null;
        }

        public StoredJoinColumn Find(IColumnDefinition columnDef)
        {
            return this.Find(columnDef.Name);
        }
    }


    public class StoredJoinColumn : StoredColumn
    {

        public StoredJoinColumn(IColumnDefinition columnDef, StoredTypes type, Query query)
            : base(columnDef, type, query)
        {
        }

        public StoredJoinColumn(IColumnDefinition columnDef, StoredTypes type, StoredTable table, Query query)
            : base(columnDef, type, table, query)
        {
        }

        public new StoredJoin As(string p_alias)
        {
            base.As(p_alias);
            return this.Table as StoredJoin;
        }

        public new StoredJoin EndColumn()
        {
            return this.Table as StoredJoin;
        }
    }

}
