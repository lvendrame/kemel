using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.Schema;
using Kemel.Orm.Entity;
using Kemel.Orm.NQuery.Storage.Column;
using Kemel.Orm.NQuery.Storage.Table;
using Kemel.Orm.Base;

namespace Kemel.Orm.NQuery.Storage.Value
{
    public class SetColumnValueCollection : List<SetColumnValue>
    {
        public SetColumnValueCollection(SetColumnValue.SetValueType type)
        {
            this.enmSetType = type;
        }

        private SetColumnValue.SetValueType enmSetType;
        public SetColumnValue.SetValueType Type
        {
            get { return enmSetType; }
            set { enmSetType = value; }
        }

        public new SetColumnValue Add(SetColumnValue item)
        {
            base.Add(item);
            return item;
        }

        public SetColumnValue Find(string name)
        {
            name = name.ToUpper();

            foreach (SetColumnValue column in this)
            {
                if ((column.Compare(name)))
                {
                    return column;
                }
            }
            return null;
        }

        public SetColumnValue Find(IColumnDefinition columnDef)
        {
            return this.Find(columnDef.Name);
        }
    }

    public class SetColumnValue : StoredColumn
    {

        #region "Properties"

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

        public SetColumnValue(IColumnDefinition columnDef, StoredTypes type, Query query)
            : base(columnDef, type, query)
        {
        }

        public SetColumnValue(IColumnDefinition columnDef, StoredTypes type, StoredTable table, Query query)
            : base(columnDef, type, table, query)
        {
        }

        #endregion

        #region "Methods"

        public Query EqualC(string columnName)
        {
            this.Value = StorageFactory.Value.CreateValue(columnName, this.Query);
            return this.Query;
        }

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

        public enum SetValueType
        {
            Parameter,
            Define,
            Parts
        }

    }

    public class SetColumnValueRet<T> : SetColumnValue
    {


        private T tParent;
        public T Parent
        {
            get { return tParent; }
        }

        public SetColumnValueRet(IColumnDefinition columnDef, StoredTypes type, Query query, T parent)
            : base(columnDef, type, query)
        {
            this.tParent = parent;
        }

        public SetColumnValueRet(IColumnDefinition columnDef, StoredTypes type, StoredTable table, Query query, T parent)
            : base(columnDef, type, table, query)
        {
            this.tParent = parent;
        }

        #region "Methods"

        public new T EqualC(string columnName)
        {
            this.Value = StorageFactory.Value.CreateValue(columnName, this.Query);
            return this.Parent;
        }

        public new T Equal(object value)
        {
            this.Value = StorageFactory.Value.Create(value);
            return this.Parent;
        }

        public new T Equal(ColumnSchema column)
        {
            this.Value = StorageFactory.Value.Create(column, this.Query);
            return this.Parent;
        }

        public new T Equal(string columnName, TableSchema table)
        {
            this.Value = StorageFactory.Value.Create(columnName, StorageFactory.Table.Create(table, this.Query), this.Query);
            return this.Parent;
        }

        public new T Equal<TEtt>(string columnName) where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            this.Value = StorageFactory.Value.Create(columnName, StorageFactory.Table.Create(table, this.Query), this.Query);
            return this.Parent;
        }

        public new T Equal(string tableName, string columnName)
        {
            this.Value = StorageFactory.Value.Create(columnName, StorageFactory.Table.Create(tableName, this.Query), this.Query);
            return this.Parent;
        }

        public new T NullValue()
        {
            this.Value = null;
            return this.Parent;
        }

        #endregion
    }
}
