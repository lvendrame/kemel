using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.NQuery.Storage.Column;
using Kemel.Orm.NQuery.Storage;
using Kemel.Orm.NQuery.Storage.Table;
using Kemel.Orm.NQuery.Storage.Value;
using Kemel.Orm.Schema;
using Kemel.Orm.Entity;
using Kemel.Orm.NQuery.Storage.Function;

namespace Kemel.Orm.NQuery.Storage.StoredSelect
{

    public class StoredConcat : IGetQuery
    {

        public StoredConcat(Query query)
        {
            this.qryQuery = query;
        }

        private Query qryQuery;
        public Query Query
        {
            get { return qryQuery; }
            set { qryQuery = value; }
        }

        private string strAlias;
        public string Alias
        {
            get { return strAlias; }
            set { strAlias = value; }
        }

        private StoredValueCollection lstValues;
        public StoredValueCollection Values
        {
            get
            {
                if (lstValues == null)
                {
                    lstValues = new StoredValueCollection();
                }
                return lstValues;
            }
        }

        public Query EndConcat()
        {
            return this.Query;
        }

        public Query AsAlias(string p_alias)
        {
            this.Alias = p_alias;
            return this.Query;
        }

        #region "Methods"

        public StoredConcat AndValue(object objValue)
        {
            this.Values.Add(StorageFactory.Value.Create(objValue));
            return this;
        }

        public StoredConcat AndValue(StoredFunction stFunction)
        {
            this.Values.Add(StorageFactory.Value.Create(stFunction));
            return this;
        }

        public StoredConcat AndValue(ColumnSchema column)
        {
            this.Values.Add(StorageFactory.Value.Create(column, this.Query));
            return this;
        }

        public StoredConcat AndValue(string columnName)
        {
            this.Values.Add(StorageFactory.Value.Create(columnName, this.Query));
            return this;
        }

        public StoredConcat AndValue(string columnName, TableSchema table)
        {
            this.Values.Add(StorageFactory.Value.Create(columnName, StorageFactory.Table.Create(table, this.Query), this.Query));
            return this;
        }

        public StoredConcat AndValue<TEtt>(string columnName) where TEtt : EntityBase
        {
            this.Values.Add(StorageFactory.Value.CreateValue<TEtt>(columnName, this.Query));
            return this;
        }

        public StoredConcat AndValue(string tableName, string columnName)
        {
            this.Values.Add(StorageFactory.Value.CreateValue(tableName, columnName, this.Query));
            return this;
        }

        #endregion

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
