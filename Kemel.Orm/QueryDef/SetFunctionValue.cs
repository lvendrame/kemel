using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Schema;
using Kemel.Orm.Entity;

namespace Kemel.Orm.QueryDef
{
    public class SetFunctionValueCollection : List<SetFunctionValue>
    {
        new public SetFunctionValue Add(SetFunctionValue item)
        {
            base.Add(item);
            return item;
        }
    }


    public class SetFunctionValue: ColumnQuery
    {
        #region Properties
        public object Value { get; set; }
        public ColumnQuery ColumnValue { get; set; }
        public System.Data.ParameterDirection ParameterDirection { get; set; }
        #endregion

        #region Constructors
        public SetFunctionValue(ColumnSchema columnSchema, TableQuery parent)
            : base(columnSchema, parent)
        {
        }

        public SetFunctionValue(string columnName, TableQuery parent)
            : base(columnName, parent)
        {
        }
        #endregion

        #region Static Methods

        public static SetFunctionValue Set(string tableName, string columnName, Query query)
        {
            TableQuery tq = query.FindTableQuery(tableName);
            return new SetFunctionValue(columnName, tq);
        }

        public static SetFunctionValue Set(TableSchema table, string columnName, Query query)
        {
            TableQuery tq = query.FindTableQuery(table);
            return new SetFunctionValue(columnName, tq);
        }

        public static SetFunctionValue Set<TEtt>(string columnName, Query query)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = query.FindTableQuery(table);
            return new SetFunctionValue(columnName, tq);
        }

        public static SetFunctionValue Set(ColumnSchema column, Query query)
        {
            TableQuery tq = query.FindTableQuery(column.Parent);
            return new SetFunctionValue(column, tq);
        }

        #endregion

        #region Methods
        public Function Equal(object value)
        {
            this.Value = value;
            return this.Parent as Function;
        }

        public Function Equal(ColumnSchema column)
        {
            this.ColumnValue = this.Parent.Parent.FindColumnQueryInTables(column);
            return this.Parent as Function;
        }

        public Function Equal(TableSchema table, string columnName)
        {
            TableQuery tq = this.Parent.Parent.FindTableQuery(table);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            return this.Parent as Function;
        }

        public Function Equal<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = this.Parent.Parent.FindTableQuery(table);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            return this.Parent as Function;
        }

        public Function Equal(string tableName, string columnName)
        {
            TableQuery tq = this.Parent.Parent.FindTableQuery(tableName);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            return this.Parent as Function;
        }

        #endregion
    }
}
