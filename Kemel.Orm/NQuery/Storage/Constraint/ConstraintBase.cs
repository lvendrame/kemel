using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.NQuery.Storage.Column;
using Kemel.Orm.NQuery.Storage.Value;
using Kemel.Orm.Entity;
using Kemel.Orm.Schema;
using Kemel.Orm.NQuery.Storage.Table;
using Kemel.Orm.NQuery.Storage.Function;
using Kemel.Orm.Base;

namespace Kemel.Orm.NQuery.Storage.Constraint
{

    public class ConstraintBaseCollection<ParentType, T> : List<T>
        where ParentType : IGetQuery
        where T : ConstraintBase<ParentType>
    {

        public new T Add(T item)
        {
            base.Add(item);
            return item;
        }

        public T FindByColumn(IColumnDefinition column)
        {
            return this.FindByColumn(column.Name);
        }

        public T FindByColumn(string columnName)
        {
            columnName = columnName.ToUpper();
            foreach (T item in this)
            {
                if (item.Column.Compare(columnName))
                {
                    return item;
                }
            }
            return null;
        }

    }

    public class ConstraintBase<ParentType> : ConstraintBase, IGetQuery
        where ParentType : IGetQuery
    {

        private ParentType objParent = default(ParentType);
        public ParentType Parent
        {
            get { return this.objParent; }
            set { this.objParent = value; }
        }

        #region "Methods"

        #region "Comparison"

        #region "Object Value"

        public ParentType Equal(object value)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(value);
            this.Comparison = ComparisonOperator.Equal;
            return this.Parent;
        }

        public ParentType Different(object value)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(value);
            this.Comparison = ComparisonOperator.Different;
            return this.Parent;
        }

        public ParentType GreaterThan(object value)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(value);
            this.Comparison = ComparisonOperator.GreaterThan;
            return this.Parent;
        }

        public ParentType LessThan(object value)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(value);
            this.Comparison = ComparisonOperator.LessThan;
            return this.Parent;
        }

        public ParentType GreaterThanOrEqual(object value)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(value);
            this.Comparison = ComparisonOperator.GreaterThanOrEqual;
            return this.Parent;
        }

        public ParentType LessThanOrEqual(object value)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(value);
            this.Comparison = ComparisonOperator.LessThanOrEqual;
            return this.Parent;
        }

        public ParentType Between(object startValue, object endValue)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(startValue, endValue);
            this.Comparison = ComparisonOperator.Between;
            return this.Parent;
        }

        public ParentType Like(object value)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(value);
            this.Comparison = ComparisonOperator.Like;
            return this.Parent;
        }

        public ParentType NotLike(object value)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(value);
            this.Comparison = ComparisonOperator.NotLike;
            return this.Parent;
        }

        public ParentType In(params object[] values)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(values);
            this.Comparison = ComparisonOperator.In;
            return this.Parent;
        }

        public ParentType NotIn(params object[] values)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(values);
            this.Comparison = ComparisonOperator.NotIn;
            return this.Parent;
        }

        public ParentType In(IEnumerable values)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(values);
            this.Comparison = ComparisonOperator.In;
            return this.Parent;
        }

        public ParentType NotIn(IEnumerable values)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(values);
            this.Comparison = ComparisonOperator.NotIn;
            return this.Parent;
        }

        public ParentType IsNull()
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create("is null");
            this.Comparison = ComparisonOperator.IsNull;
            return this.Parent;
        }

        public ParentType IsNotNull()
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create("not is null");
            this.Comparison = ComparisonOperator.IsNotNull;
            return this.Parent;
        }

        public ParentType OpenParentheses()
        {
            this.Comparison = ComparisonOperator.OpenParentheses;
            return this.Parent;
        }

        public ParentType CloseParentheses()
        {
            this.Comparison = ComparisonOperator.CloseParentheses;
            return this.Parent;
        }

        public ParentType StartsWith(object value)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(value);
            this.Comparison = ComparisonOperator.StartsWith;
            return this.Parent;
        }

        public ParentType EndsWith(object value)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(value);
            this.Comparison = ComparisonOperator.EndsWith;
            return this.Parent;
        }

        public ParentType Contains(object value)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(value);
            this.Comparison = ComparisonOperator.Contains;
            return this.Parent;
        }
        #endregion

        #region "ColumnSchema"

        public ParentType Equal(ColumnSchema column)
        {
            this.Value = StorageFactory.Value.Create(column, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.Equal;
            return this.Parent;
        }

        public ParentType Different(ColumnSchema column)
        {
            this.Value = StorageFactory.Value.Create(column, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.Different;
            return this.Parent;
        }

        public ParentType GreaterThan(ColumnSchema column)
        {
            this.Value = StorageFactory.Value.Create(column, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.GreaterThan;
            return this.Parent;
        }

        public ParentType LessThan(ColumnSchema column)
        {
            this.Value = StorageFactory.Value.Create(column, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.LessThan;
            return this.Parent;
        }

        public ParentType GreaterThanOrEqual(ColumnSchema column)
        {
            this.Value = StorageFactory.Value.Create(column, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.GreaterThanOrEqual;
            return this.Parent;
        }

        public ParentType LessThanOrEqual(ColumnSchema column)
        {
            this.Value = StorageFactory.Value.Create(column, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.LessThanOrEqual;
            return this.Parent;
        }

        public ParentType Between(ColumnSchema startColumnValue, ColumnSchema endColumnValue)
        {
            this.Value = StorageFactory.Value.Create(startColumnValue, endColumnValue, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.Between;
            return this.Parent;
        }

        public ParentType Like(ColumnSchema column)
        {
            this.Value = StorageFactory.Value.Create(column, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.Like;
            return this.Parent;
        }

        public ParentType NotLike(ColumnSchema column)
        {
            this.Value = StorageFactory.Value.Create(column, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.NotLike;
            return this.Parent;
        }

        #endregion

        #region "Table Schema and ColumnName"

        public ParentType Equal(TableSchema table, string columnName)
        {
            this.Value = StorageFactory.Value.CreateValue(table, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.Equal;
            return this.Parent;
        }

        public ParentType Different(TableSchema table, string columnName)
        {
            this.Value = StorageFactory.Value.CreateValue(table, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.Different;
            return this.Parent;
        }

        public ParentType GreaterThan(TableSchema table, string columnName)
        {
            this.Value = StorageFactory.Value.CreateValue(table, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.GreaterThan;
            return this.Parent;
        }

        public ParentType LessThan(TableSchema table, string columnName)
        {
            this.Value = StorageFactory.Value.CreateValue(table, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.LessThan;
            return this.Parent;
        }

        public ParentType GreaterThanOrEqual(TableSchema table, string columnName)
        {
            this.Value = StorageFactory.Value.CreateValue(table, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.GreaterThanOrEqual;
            return this.Parent;
        }

        public ParentType LessThanOrEqual(TableSchema table, string columnName)
        {
            this.Value = StorageFactory.Value.CreateValue(table, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.LessThanOrEqual;
            return this.Parent;
        }

        public ParentType Between(TableSchema table, string startColumnNameValue, string endColumnNameValue)
        {
            this.Value = StorageFactory.Value.CreateValue(table, startColumnNameValue, endColumnNameValue, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.Between;
            return this.Parent;
        }

        public ParentType Between(TableSchema tableStartValue, string startColumnNameValue, TableSchema tableEndValue, string endColumnNameValue)
        {
            this.Value = StorageFactory.Value.CreateValue(tableStartValue, startColumnNameValue, tableEndValue, endColumnNameValue, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.Between;
            return this.Parent;
        }

        public ParentType Like(TableSchema table, string columnName)
        {
            this.Value = StorageFactory.Value.CreateValue(table, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.Like;
            return this.Parent;
        }

        public ParentType NotLike(TableSchema table, string columnName)
        {
            this.Value = StorageFactory.Value.CreateValue(table, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.NotLike;
            return this.Parent;
        }

        #endregion

        #region "Entity and ColumnName"

        public ParentType Equal<TEtt>(string columnName) where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            this.Value = StorageFactory.Value.CreateValue(table, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.Equal;
            return this.Parent;
        }

        public ParentType Different<TEtt>(string columnName) where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            this.Value = StorageFactory.Value.CreateValue(table, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.Different;
            return this.Parent;
        }

        public ParentType GreaterThan<TEtt>(string columnName) where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            this.Value = StorageFactory.Value.CreateValue(table, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.GreaterThan;
            return this.Parent;
        }

        public ParentType LessThan<TEtt>(string columnName) where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            this.Value = StorageFactory.Value.CreateValue(table, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.LessThan;
            return this.Parent;
        }

        public ParentType GreaterThanOrEqual<TEtt>(string columnName) where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            this.Value = StorageFactory.Value.CreateValue(table, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.GreaterThanOrEqual;
            return this.Parent;
        }

        public ParentType LessThanOrEqual<TEtt>(string columnName) where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            this.Value = StorageFactory.Value.CreateValue(table, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.LessThanOrEqual;
            return this.Parent;
        }

        public ParentType Between<TEtt>(string startColumnNameValue, string endColumnNameValue) where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            this.Value = StorageFactory.Value.CreateValue(table, startColumnNameValue, endColumnNameValue, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.Between;
            return this.Parent;
        }

        public ParentType Between<TEttStartValue, TEttEndValue>(string startColumnNameValue, string endColumnNameValue)
            where TEttStartValue : EntityBase
            where TEttEndValue : EntityBase
        {
            TableSchema tableStartValue = SchemaContainer.GetSchema<TEttStartValue>();
            TableSchema tableEndValue = SchemaContainer.GetSchema<TEttEndValue>();
            this.Value = StorageFactory.Value.CreateValue(tableStartValue, startColumnNameValue, tableEndValue, endColumnNameValue, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.Between;
            return this.Parent;
        }

        public ParentType Like<TEtt>(string columnName) where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            this.Value = StorageFactory.Value.CreateValue(table, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.Like;
            return this.Parent;
        }

        public ParentType NotLike<TEtt>(string columnName) where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            this.Value = StorageFactory.Value.CreateValue(table, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.NotLike;
            return this.Parent;
        }

        #endregion

        #region "Table Name and ColumnName"

        public ParentType Equal(string tableName, string columnName)
        {
            this.Value = StorageFactory.Value.CreateValue(tableName, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.Equal;
            return this.Parent;
        }

        public ParentType Different(string tableName, string columnName)
        {
            this.Value = StorageFactory.Value.CreateValue(tableName, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.Different;
            return this.Parent;
        }

        public ParentType GreaterThan(string tableName, string columnName)
        {
            this.Value = StorageFactory.Value.CreateValue(tableName, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.GreaterThan;
            return this.Parent;
        }

        public ParentType LessThan(string tableName, string columnName)
        {
            this.Value = StorageFactory.Value.CreateValue(tableName, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.LessThan;
            return this.Parent;
        }

        public ParentType GreaterThanOrEqual(string tableName, string columnName)
        {
            this.Value = StorageFactory.Value.CreateValue(tableName, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.GreaterThanOrEqual;
            return this.Parent;
        }

        public ParentType LessThanOrEqual(string tableName, string columnName)
        {
            this.Value = StorageFactory.Value.CreateValue(tableName, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.LessThanOrEqual;
            return this.Parent;
        }

        public ParentType Between(string tableName, string startColumnNameValue, string endColumnNameValue)
        {
            this.Value = StorageFactory.Value.CreateValue(tableName, startColumnNameValue, endColumnNameValue, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.Between;
            return this.Parent;
        }

        public ParentType Between(string tableNameStartValue, string startColumnNameValue, string tableNameEndValue, string endColumnNameValue)
        {
            this.Value = StorageFactory.Value.CreateValue(tableNameStartValue, startColumnNameValue, tableNameEndValue, endColumnNameValue, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.Between;
            return this.Parent;
        }

        public ParentType Like(string tableName, string columnName)
        {
            this.Value = StorageFactory.Value.CreateValue(tableName, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.Like;
            return this.Parent;
        }

        public ParentType NotLike(string tableName, string columnName)
        {
            this.Value = StorageFactory.Value.CreateValue(tableName, columnName, this.Parent.GetQuery());
            this.Comparison = ComparisonOperator.NotLike;
            return this.Parent;
        }

        #endregion

        #region "Sub Query"

        public ParentType Equal(Query subQuery)
        {
            this.Value = StorageFactory.Value.Create(subQuery);
            this.Comparison = ComparisonOperator.Equal;
            return this.Parent;
        }

        public ParentType Different(Query subQuery)
        {
            this.Value = StorageFactory.Value.Create(subQuery);
            this.Comparison = ComparisonOperator.Different;
            return this.Parent;
        }

        public ParentType GreaterThan(Query subQuery)
        {
            this.Value = StorageFactory.Value.Create(subQuery);
            this.Comparison = ComparisonOperator.GreaterThan;
            return this.Parent;
        }

        public ParentType LessThan(Query subQuery)
        {
            this.Value = StorageFactory.Value.Create(subQuery);
            this.Comparison = ComparisonOperator.LessThan;
            return this.Parent;
        }

        public ParentType GreaterThanOrEqual(Query subQuery)
        {
            this.Value = StorageFactory.Value.Create(subQuery);
            this.Comparison = ComparisonOperator.GreaterThanOrEqual;
            return this.Parent;
        }

        public ParentType LessThanOrEqual(Query subQuery)
        {
            this.Value = StorageFactory.Value.Create(subQuery);
            this.Comparison = ComparisonOperator.LessThanOrEqual;
            return this.Parent;
        }

        public ParentType Like(Query subQuery)
        {
            this.Value = StorageFactory.Value.Create(subQuery);
            this.Comparison = ComparisonOperator.Like;
            return this.Parent;
        }

        public ParentType NotLike(Query subQuery)
        {
            this.Value = StorageFactory.Value.Create(subQuery);
            this.Comparison = ComparisonOperator.NotLike;
            return this.Parent;
        }

        public ParentType In(Query subQuery)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(subQuery);
            this.Comparison = ComparisonOperator.In;
            return this.Parent;
        }

        public ParentType NotIn(Query subQuery)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(subQuery);
            this.Comparison = ComparisonOperator.NotIn;
            return this.Parent;
        }

        #endregion

        #region "Function"

        public ParentType Equal(StoredFunction stFunction)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(stFunction);
            this.Comparison = ComparisonOperator.Equal;
            return this.Parent;
        }

        public ParentType Different(StoredFunction stFunction)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(stFunction);
            this.Comparison = ComparisonOperator.Different;
            return this.Parent;
        }

        public ParentType GreaterThan(StoredFunction stFunction)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(stFunction);
            this.Comparison = ComparisonOperator.GreaterThan;
            return this.Parent;
        }

        public ParentType LessThan(StoredFunction stFunction)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(stFunction);
            this.Comparison = ComparisonOperator.LessThan;
            return this.Parent;
        }

        public ParentType GreaterThanOrEqual(StoredFunction stFunction)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(stFunction);
            this.Comparison = ComparisonOperator.GreaterThanOrEqual;
            return this.Parent;
        }

        public ParentType LessThanOrEqual(StoredFunction stFunction)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(stFunction);
            this.Comparison = ComparisonOperator.LessThanOrEqual;
            return this.Parent;
        }

        public ParentType Between(StoredFunction startFunction, StoredFunction endFunction)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(startFunction, endFunction);
            this.Comparison = ComparisonOperator.Between;
            return this.Parent;
        }

        public ParentType Like(StoredFunction stFunction)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(stFunction);
            this.Comparison = ComparisonOperator.Like;
            return this.Parent;
        }

        public ParentType NotLike(StoredFunction stFunction)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(stFunction);
            this.Comparison = ComparisonOperator.NotLike;
            return this.Parent;
        }

        public ParentType In(StoredFunction stFunction)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(stFunction);
            this.Comparison = ComparisonOperator.In;
            return this.Parent;
        }

        public ParentType NotIn(StoredFunction stFunction)
        {
            this.Value = NQuery.Storage.StorageFactory.Value.Create(stFunction);
            this.Comparison = ComparisonOperator.NotIn;
            return this.Parent;
        }

        #endregion

        #endregion

        #endregion

        public Query GetQuery()
        {
            return this.Parent.GetQuery();
        }

        public void SetQuery(Query query)
        {
            this.Parent.SetQuery(query);
        }
    }

    public class ConstraintBase : ConditionExpression
    {

        private ConstraintType objType = ConstraintType.Where;
        public ConstraintType Type
        {
            get { return this.objType; }
            set { this.objType = value; }
        }

        public enum ConstraintType
        {
            Where,
            And,
            Or,
            None
        }
    }

}
