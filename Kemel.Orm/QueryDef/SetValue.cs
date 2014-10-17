using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Schema;
using Kemel.Orm.Entity;

namespace Kemel.Orm.QueryDef
{
    public class SetValueCollection : List<SetValue>
    {
        new public SetValue Add(SetValue item)
        {
            base.Add(item);
            return item;
        }
    }


    public class SetValue: ColumnQuery
    {
        #region Properties
        public object Value { get; set; }
        public ColumnQuery ColumnValue { get; set; }
        public System.Data.ParameterDirection ParameterDirection { get; set; }
        #endregion

        #region Constructors
        public SetValue(ColumnSchema columnSchema, TableQuery parent)
            : base(columnSchema, parent)
        {
        }

        public SetValue(string columnName, TableQuery parent)
            : base(columnName, parent)
        {
        }
        #endregion

        #region Static Methods

        public static SetValue Set(string tableName, string columnName, Query query)
        {
            TableQuery tq = query.FindTableQuery(tableName);
            return new SetValue(columnName, tq);
        }

        public static SetValue Set(TableSchema table, string columnName, Query query)
        {
            TableQuery tq = query.FindTableQuery(table);
            return new SetValue(columnName, tq);
        }

        public static SetValue Set<TEtt>(string columnName, Query query)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = query.FindTableQuery(table);
            return new SetValue(columnName, tq);
        }

        public static SetValue Set(ColumnSchema column, Query query)
        {
            TableQuery tq = query.FindTableQuery(column.Parent);
            return new SetValue(column, tq);
        }

        #endregion

        #region Methods
        public Query Equal(object value)
        {
            this.Value = value;
            return this.Parent.Parent;
        }

        public Query Equal(ColumnSchema column)
        {
            this.ColumnValue = this.Parent.Parent.FindColumnQueryInTables(column);
            return this.Parent.Parent;
        }

        public Query Equal(TableSchema table, string columnName)
        {
            TableQuery tq = this.Parent.Parent.FindTableQuery(table);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            return this.Parent.Parent;
        }

        public Query Equal<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = this.Parent.Parent.FindTableQuery(table);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            return this.Parent.Parent;
        }

        public Query Equal(string tableName, string columnName)
        {
            TableQuery tq = this.Parent.Parent.FindTableQuery(tableName);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            return this.Parent.Parent;
        }

        public Query Null()
        {
            this.Value = null;
            return this.Parent.Parent;
        }

        #endregion

        public TableQuery Direction(System.Data.ParameterDirection parameterDirection)
        {
            this.ParameterDirection = parameterDirection;
            return this.Parent;
        }
    }
}
