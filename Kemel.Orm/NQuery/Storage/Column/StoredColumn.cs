using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.NQuery.Storage.Table;
using Kemel.Orm.Base;

namespace Kemel.Orm.NQuery.Storage.Column
{

    public class StoredColumnCollectoin : List<StoredColumn>
    {
        public new StoredColumn Add(StoredColumn item)
        {
            base.Add(item);
            return item;
        }

        public StoredColumn Find(string name)
        {
            name = name.ToUpper();

            foreach (StoredColumn column in this)
            {
                if ((column.Compare(name)))
                {
                    return column;
                }
            }
            return null;
        }

        public StoredColumn Find(IColumnDefinition columnDef)
        {
            return this.Find(columnDef.Name);
        }

    }

    public class StoredColumn : IGetQuery, IGenParameter
    {

        public StoredColumn(IColumnDefinition columnDef, StoredTypes type, Query query)
        {
            this.icdColumnDefinition = columnDef;
            this.estType = type;
            this.objQuery = query;
        }

        public StoredColumn(IColumnDefinition columnDef, StoredTypes type, StoredTable table, Query query)
            : this(columnDef, type, query)
        {
            this.objTable = table;
        }

        private IColumnDefinition icdColumnDefinition;
        public IColumnDefinition ColumnDefinition
        {
            get { return icdColumnDefinition; }
        }

        private string objAlias = null;
        public string Alias
        {
            get { return this.objAlias; }
            set { this.objAlias = value; }
        }

        public bool HasAlias
        {
            get { return !string.IsNullOrEmpty(this.Alias) || !string.IsNullOrEmpty(this.ColumnDefinition.Alias); }
        }

        private StoredTable objTable = null;
        public StoredTable Table
        {
            get { return this.objTable; }
            set { this.objTable = value; }
        }

        private Query objQuery = null;
        public Query Query
        {
            get { return this.objQuery; }
        }

        private string strParameterName;
        public string ParameterName
        {
            get { return strParameterName; }
            set { strParameterName = value; }
        }

        public StoredTable As(string p_alias)
        {
            this.Alias = p_alias;
            return this.Table;
        }

        public StoredTable EndColumn()
        {
            return this.Table;
        }

        public Query EndColumnUnknowTable()
        {
            return this.Query;
        }

        public bool Compare(string name)
        {
            if ((!string.IsNullOrEmpty(icdColumnDefinition.Name)))
            {
                if ((icdColumnDefinition.Name.ToUpper().Equals(name)))
                {
                    return true;
                }
            }
            else if ((!string.IsNullOrEmpty(icdColumnDefinition.Alias)))
            {
                if ((icdColumnDefinition.Alias.ToUpper().Equals(name)))
                {
                    return true;
                }
            }

            if ((!string.IsNullOrEmpty(this.Alias)))
            {
                if ((this.Alias.ToUpper().Equals(name)))
                {
                    return true;
                }
            }

            return false;
        }

        public T To<T>()
            where T : class, IColumnDefinition
        {
            return this.ColumnDefinition as T;
        }

        private StoredTypes estType;
        public StoredTypes Type
        {
            get { return estType; }
        }

        public enum StoredTypes
        {
            Schema,
            Name,
            OnlyAlias,
            UnknowTable,
            StoredFunction,
            StringConstant,
            Concat
        }

        public Query GetQuery()
        {
            return this.Query;
        }

        public void SetQuery(Query query)
        {
            this.objQuery = query;
        }
    }

}
