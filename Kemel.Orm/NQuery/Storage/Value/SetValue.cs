using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.Schema;
using Kemel.Orm.Entity;

namespace Kemel.Orm.NQuery.Storage.Value
{
    public class SetValueCollection : List<SetValue>
    {
        public new SetValue Add(SetValue item)
        {
            base.Add(item);
            return item;
        }
    }

    public class SetValue
    {
        #region "Properties"

        private Query sqyQuery;
        public Query Query
        {
            get { return sqyQuery; }
        }

        private StoredValue stvSetObject = null;
        public StoredValue SetObject
        {
            get { return this.stvSetObject; }
            set { this.stvSetObject = value; }
        }

        private StoredValue stvValue = null;
        public StoredValue Value
        {
            get { return this.stvValue; }
            set { this.stvValue = value; }
        }

        private System.Data.ParameterDirection objParameterDirection = System.Data.ParameterDirection.Input;
        public System.Data.ParameterDirection ParameterDirection
        {
            get { return this.objParameterDirection; }
            set { this.objParameterDirection = value; }
        }

        #endregion

        #region "Constructors"
        public SetValue(StoredValue setObject, Query parent)
        {
            this.stvSetObject = setObject;
            this.sqyQuery = parent;
        }
        #endregion

        #region "Methods"

        public Query Equal(object value)
        {
            this.Value = StorageFactory.Value.Create(value);
            return this.Query;
        }

        public Query Equal(ColumnSchema column)
        {
            this.Value = StorageFactory.Value.Create(column, this.Query);
            return this.Query;
        }

        public Query Equal(string columnName, TableSchema table)
        {
            this.Value = StorageFactory.Value.Create(columnName, StorageFactory.Table.Create(table, this.Query), this.Query);
            return this.Query;
        }

        public Query Equal<TEtt>(string columnName) where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            this.Value = StorageFactory.Value.Create(columnName, StorageFactory.Table.Create(table, this.Query), this.Query);
            return this.Query;
        }

        public Query Equal(string tableName, string columnName)
        {
            this.Value = StorageFactory.Value.Create(columnName, StorageFactory.Table.Create(tableName, this.Query), this.Query);
            return this.Query;
        }

        public Query NullValue()
        {
            this.Value = null;
            return this.Query;
        }

        #endregion

        public Query Direction(System.Data.ParameterDirection parameterDirection)
        {
            this.ParameterDirection = parameterDirection;
            return this.Query;
        }
    }
}
