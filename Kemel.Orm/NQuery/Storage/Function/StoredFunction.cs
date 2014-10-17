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

namespace Kemel.Orm.NQuery.Storage.Function
{

    public class StoredFunction : IGetQuery
    {

        public StoredFunction(string name)
            : this(name, null, null)
        {
        }

        public StoredFunction(string name, Query query)
            : this(name, null, query)
        {
        }

        public StoredFunction(string name, string owner)
            : this(name, owner, null)
        {
        }

        public StoredFunction(string name, string owner, Query query)
        {
            this.strName = name;
            this.strOwner = owner;
            this.qryQuery = query;
        }

        private string strName;
        public string Name
        {
            get { return strName; }
            set { strName = value; }
        }

        private string strOwner;
        public string Owner
        {
            get { return strOwner; }
            set { strOwner = value; }
        }


        private string strAlias;
        public string Alias
        {
            get { return strAlias; }
            set { strAlias = value; }
        }


        private SetColumnValueCollection lstSetValues;
        public SetColumnValueCollection SetValues
        {
            get
            {
                if ((lstSetValues == null))
                {
                    lstSetValues = new SetColumnValueCollection(SetColumnValue.SetValueType.Parameter);
                }
                return lstSetValues;
            }
        }

        private Query qryQuery;
        public Query Query
        {
            get { return qryQuery; }
            set { qryQuery = value; }
        }

        public SetColumnValueRet<StoredFunction> SetParameter(ColumnSchema column)
        {
            SetColumnValueRet<StoredFunction> item = StorageFactory.SetColumnValue.Create<StoredFunction>(column, qryQuery, this);
            this.SetValues.Add(item);
            return item;
        }

        public SetColumnValueRet<StoredFunction> SetParameter(Schema.ColumnSchema column, StoredTable table)
        {
            SetColumnValueRet<StoredFunction> item = StorageFactory.SetColumnValue.Create<StoredFunction>(column, table, qryQuery, this);
            this.SetValues.Add(item);
            return item;
        }

        public SetColumnValueRet<StoredFunction> SetParameter(string columnName, StoredTable table)
        {
            SetColumnValueRet<StoredFunction> item = StorageFactory.SetColumnValue.Create<StoredFunction>(columnName, table, qryQuery, this);
            this.SetValues.Add(item);
            return item;
        }

        public SetColumnValueRet<StoredFunction> SetParameter(string columnName)
        {
            SetColumnValueRet<StoredFunction> item = StorageFactory.SetColumnValue.Create<StoredFunction>(columnName, qryQuery, this);
            this.SetValues.Add(item);
            return item;
        }

        public SetColumnValueRet<StoredFunction> SetParameter(Schema.ColumnSchema column, string tableName)
        {
            SetColumnValueRet<StoredFunction> item = StorageFactory.SetColumnValue.Create<StoredFunction>(column, tableName, qryQuery, this);
            this.SetValues.Add(item);
            return item;
        }

        public SetColumnValueRet<StoredFunction> SetParameter(string columnName, string tableName)
        {
            SetColumnValueRet<StoredFunction> item = StorageFactory.SetColumnValue.Create<StoredFunction>(columnName, tableName, qryQuery, this);
            this.SetValues.Add(item);
            return item;
        }

        public SetColumnValueRet<StoredFunction> SetParameter(string columnName, TableSchema tableSchema)
        {
            SetColumnValueRet<StoredFunction> item = StorageFactory.SetColumnValue.Create<StoredFunction>(columnName, tableSchema, qryQuery, this);
            this.SetValues.Add(item);
            return item;
        }

        public SetColumnValueRet<StoredFunction> SetParameter(string columnName, EntityBase entity)
        {
            SetColumnValueRet<StoredFunction> item = StorageFactory.SetColumnValue.Create<StoredFunction>(columnName, entity, qryQuery, this);
            this.SetValues.Add(item);
            return item;
        }

        public SetColumnValueRet<StoredFunction> SetParameter<TEtt>(string columnName) where TEtt : EntityBase
        {
            SetColumnValueRet<StoredFunction> item = StorageFactory.SetColumnValue.Create<TEtt, StoredFunction>(columnName, qryQuery, this);
            this.SetValues.Add(item);
            return item;
        }

        public SetColumnValueRet<StoredFunction> SetParameter(StoredFunction stFunction, StoredTable table)
        {
            SetColumnValueRet<StoredFunction> item = StorageFactory.SetColumnValue.Create<StoredFunction>(stFunction, table, qryQuery, this);
            this.SetValues.Add(item);
            return item;
        }

        public SetColumnValueRet<StoredFunction> SetParameter(StoredFunction stFunction)
        {
            SetColumnValueRet<StoredFunction> item = StorageFactory.SetColumnValue.Create<StoredFunction>(stFunction, qryQuery, this);
            this.SetValues.Add(item);
            return item;
        }

        public Query EndFunction()
        {
            return this.Query;
        }

        public StoredFunction SetOwner(string owner)
        {
            this.strOwner = owner;
            return this;
        }

        public Query As(string pAlias)
        {
            this.strAlias = pAlias;
            return this.Query;
        }

        public Query GetQuery()
        {
            return this.qryQuery;
        }

        public void SetQuery(Query query)
        {
            this.qryQuery = query;
        }
    }

}
