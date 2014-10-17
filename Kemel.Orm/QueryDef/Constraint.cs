using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Schema;
using Kemel.Orm.Entity;

namespace Kemel.Orm.QueryDef
{
    public class ConstraintCollection : List<Constraint>
    {
        new public Constraint Add(Constraint item)
        {
            base.Add(item);
            return item;
        }

        public Constraint FindByColumn(ColumnSchema column)
        {
            foreach (Constraint item in this)
            {
                if (item.Column.PatternColumnName.Equals(column.Name))
                    return item;
            }
            return null;
        }
    }

    public class Constraint
    {
        #region Properties

        public string StartParameterName { get; internal set; }
        public string EndParameterName { get; internal set; }

        private List<string> lstParametersNames = null;
        public List<string> ParametersNames
        {
            get
            {
                if (this.lstParametersNames == null)
                    this.lstParametersNames = new List<string>();
                return this.lstParametersNames;
            }
        }


        public ConstraintType Type { get; set; }
        public ColumnQuery Column { get; set; }
        public ComparisonOperator Comparison { get; set; }
        public string ConstantStringValue { get; set; }
        public object Value { get; set; }
        public object EndValue { get; set; }
        public ColumnQuery ColumnValue { get; set; }
        public ColumnQuery ColumnEndValue { get; set; }

        private Query qrySubQuery = null;
        public Query SubQuery
        {
            get
            {
                return this.qrySubQuery;
            }
            set
            {
                this.qrySubQuery = value;
                this.qrySubQuery.Parent = this.Parent;
            }
        }
        public Function Function { get; set; }

        private List<object> lstValues = null;
        public List<object> Values
        {
            get
            {
                if (this.lstValues == null)
                    this.lstValues = new List<object>();
                return this.lstValues;
            }
        }

        public Query Parent { get; set; }

        #endregion

        #region Constructors

        private Constraint(ConstraintType type, string tableName, string columnName, Query parent)
        {
            this.Type = type;
            this.Column = new ColumnQuery(columnName, parent.FindTableQuery(tableName));
            this.Parent = parent;
        }

        private Constraint(ConstraintType type, TableSchema table, string columnName, Query parent)
        {
            this.Type = type;
            this.Column = new ColumnQuery(columnName, parent.FindTableQuery(table));
            this.Parent = parent;
        }

        private Constraint(ConstraintType type, ColumnSchema column, Query parent)
        {
            this.Type = type;
            this.Column = parent.FindColumnQueryInTables(column);
            this.Parent = parent;
        }

        //private Constraint(ConstraintType type, string tableName, string columnName, Query parent)
        //{
        //    TableQuery tableQuery = parent.FindTableQuery(tableName);
        //    this.Type = type;
        //    this.Column = tableQuery.FindColumnQuery(columnName);

        //    if(this.Column == null)
        //        this.Column = new ColumnQuery(columnName, tableQuery);

        //    this.Parent = parent;
        //}

        private Constraint(ConstraintType type, string constantStringValue, Query parent)
        {
            this.Type = type;
            this.ConstantStringValue = constantStringValue;
            this.Parent = parent;
        }

        private Constraint(ConstraintType type, Query parent)
        {
            this.Type = type;
            this.Parent = parent;
        }

        #endregion

        #region Static Methods

        public static Constraint Where(ColumnSchema column, Query parent)
        {
            return new Constraint(ConstraintType.Where, column, parent);
        }

        public static Constraint And(ColumnSchema column, Query parent)
        {
            return new Constraint(ConstraintType.And, column, parent);
        }

        public static Constraint Or(ColumnSchema column, Query parent)
        {
            return new Constraint(ConstraintType.Or, column, parent);
        }

        public static Constraint Where(TableSchema table, string columnName, Query parent)
        {
            return new Constraint(ConstraintType.Where, table[columnName], parent);
        }

        public static Constraint And(TableSchema table, string columnName, Query parent)
        {
            return new Constraint(ConstraintType.And, table[columnName], parent);
        }

        public static Constraint Or(TableSchema table, string columnName, Query parent)
        {
            return new Constraint(ConstraintType.Or, table[columnName], parent);
        }

        public static Constraint Where(string tableName, string columnName, Query parent)
        {
            return new Constraint(ConstraintType.Where, tableName, columnName, parent);
        }

        public static Constraint And(string tableName, string columnName, Query parent)
        {
            return new Constraint(ConstraintType.And, tableName, columnName, parent);
        }

        public static Constraint Or(string tableName, string columnName, Query parent)
        {
            return new Constraint(ConstraintType.Or, tableName, columnName, parent);
        }

        public static Constraint Where(string constantStringValue, Query parent)
        {
            return new Constraint(ConstraintType.Where, constantStringValue, parent);
        }

        public static Constraint And(string constantStringValue, Query parent)
        {
            return new Constraint(ConstraintType.And, constantStringValue, parent);
        }

        public static Constraint Or(string constantStringValue, Query parent)
        {
            return new Constraint(ConstraintType.Or, constantStringValue, parent);
        }

        public static Constraint Where<TEtt>(string columnName, Query parent)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            return new Constraint(ConstraintType.Where, table[columnName], parent);
        }

        public static Constraint And<TEtt>(string columnName, Query parent)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            return new Constraint(ConstraintType.And, table[columnName], parent);
        }

        public static Constraint Or<TEtt>(string columnName, Query parent)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            return new Constraint(ConstraintType.Or, table[columnName], parent);
        }

        public static Constraint OpenParentesis(ConstraintType type, Query parent)
        {
            Constraint constraint = new Constraint(type, parent);
            constraint.Comparison = ComparisonOperator.OpenParentheses;
            return constraint;
        }

        public static Constraint CloseParentesis(Query parent)
        {
            Constraint constraint = new Constraint(ConstraintType.And, parent);
            constraint.Comparison = ComparisonOperator.CloseParentheses;
            return constraint;
        }

        #endregion

        #region Methods

        #region Comparison

        #region object value
        public Query Equal(object value)
        {
            this.Value = value;
            this.Comparison = ComparisonOperator.Equal;
            return this.Parent;
        }

        public Query Different(object value)
        {
            this.Value = value;
            this.Comparison = ComparisonOperator.Different;
            return this.Parent;
        }

        public Query GreaterThan(object value)
        {
            this.Value = value;
            this.Comparison = ComparisonOperator.GreaterThan;
            return this.Parent;
        }

        public Query LessThan(object value)
        {
            this.Value = value;
            this.Comparison = ComparisonOperator.LessThan;
            return this.Parent;
        }

        public Query GreaterThanOrEqual(object value)
        {
            this.Value = value;
            this.Comparison = ComparisonOperator.GreaterThanOrEqual;
            return this.Parent;
        }

        public Query LessThanOrEqual(object value)
        {
            this.Value = value;
            this.Comparison = ComparisonOperator.LessThanOrEqual;
            return this.Parent;
        }

        public Query Between(object startValue, object endValue)
        {
            this.Value = startValue;
            this.EndValue = endValue;
            this.Comparison = ComparisonOperator.Between;
            return this.Parent;
        }

        public Query Like(object value)
        {
            this.Value = value;
            this.Comparison = ComparisonOperator.Like;
            return this.Parent;
        }

        public Query NotLike(object value)
        {
            this.Value = value;
            this.Comparison = ComparisonOperator.NotLike;
            return this.Parent;
        }

        public Query In(params object[] values)
        {
            this.Values.AddRange(values);
            this.Comparison = ComparisonOperator.In;
            return this.Parent;
        }

        public Query NotIn(params object[] values)
        {
            this.Values.AddRange(values);
            this.Comparison = ComparisonOperator.NotIn;
            return this.Parent;
        }

        public Query IsNull()
        {
            this.Comparison = ComparisonOperator.IsNull;
            return this.Parent;
        }

        public Query IsNotNull()
        {
            this.Comparison = ComparisonOperator.IsNotNull;
            return this.Parent;
        }

        public Query OpenParentheses()
        {
            this.Comparison = ComparisonOperator.OpenParentheses;
            return this.Parent;
        }

        public Query CloseParentheses()
        {
            this.Comparison = ComparisonOperator.CloseParentheses;
            return this.Parent;
        }

        public Query StartsWith(object value)
        {
            this.Value = value;
            this.Comparison = ComparisonOperator.StartsWith;
            return this.Parent;
        }

        public Query EndsWith(object value)
        {
            this.Value = value;
            this.Comparison = ComparisonOperator.EndsWith;
            return this.Parent;
        }

        public Query Contains(object value)
        {
            this.Value = value;
            this.Comparison = ComparisonOperator.Contains;
            return this.Parent;
        }
        #endregion

        #region ColumnSchema

        public Query Equal(ColumnSchema column)
        {
            this.ColumnValue = this.Parent.FindColumnQueryInTables(column);
            this.Comparison = ComparisonOperator.Equal;
            return this.Parent;
        }

        public Query Different(ColumnSchema column)
        {
            this.ColumnValue = this.Parent.FindColumnQueryInTables(column);
            this.Comparison = ComparisonOperator.Different;
            return this.Parent;
        }

        public Query GreaterThan(ColumnSchema column)
        {
            this.ColumnValue = this.Parent.FindColumnQueryInTables(column);
            this.Comparison = ComparisonOperator.GreaterThan;
            return this.Parent;
        }

        public Query LessThan(ColumnSchema column)
        {
            this.ColumnValue = this.Parent.FindColumnQueryInTables(column);
            this.Comparison = ComparisonOperator.LessThan;
            return this.Parent;
        }

        public Query GreaterThanOrEqual(ColumnSchema column)
        {
            this.ColumnValue = this.Parent.FindColumnQueryInTables(column);
            this.Comparison = ComparisonOperator.GreaterThanOrEqual;
            return this.Parent;
        }

        public Query LessThanOrEqual(ColumnSchema column)
        {
            this.ColumnValue = this.Parent.FindColumnQueryInTables(column);
            this.Comparison = ComparisonOperator.LessThanOrEqual;
            return this.Parent;
        }

        public Query Between(ColumnSchema startColumnValue, ColumnSchema endColumnValue)
        {
            this.ColumnValue = this.Parent.FindColumnQueryInTables(startColumnValue);
            this.ColumnEndValue = this.Parent.FindColumnQueryInTables(endColumnValue);
            this.Comparison = ComparisonOperator.Between;
            return this.Parent;
        }

        public Query Like(ColumnSchema column)
        {
            this.ColumnValue = this.Parent.FindColumnQueryInTables(column);
            this.Comparison = ComparisonOperator.Like;
            return this.Parent;
        }

        public Query NotLike(ColumnSchema column)
        {
            this.ColumnValue = this.Parent.FindColumnQueryInTables(column);
            this.Comparison = ComparisonOperator.NotLike;
            return this.Parent;
        }

        #endregion

        #region Table Schema and ColumnName

        public Query Equal(TableSchema table, string columnName)
        {
            TableQuery tq = this.Parent.FindTableQuery(table);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.Equal;
            return this.Parent;
        }

        public Query Different(TableSchema table, string columnName)
        {
            TableQuery tq = this.Parent.FindTableQuery(table);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.Different;
            return this.Parent;
        }

        public Query GreaterThan(TableSchema table, string columnName)
        {
            TableQuery tq = this.Parent.FindTableQuery(table);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.GreaterThan;
            return this.Parent;
        }

        public Query LessThan(TableSchema table, string columnName)
        {
            TableQuery tq = this.Parent.FindTableQuery(table);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.LessThan;
            return this.Parent;
        }

        public Query GreaterThanOrEqual(TableSchema table, string columnName)
        {
            TableQuery tq = this.Parent.FindTableQuery(table);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.GreaterThanOrEqual;
            return this.Parent;
        }

        public Query LessThanOrEqual(TableSchema table, string columnName)
        {
            TableQuery tq = this.Parent.FindTableQuery(table);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.LessThanOrEqual;
            return this.Parent;
        }

        public Query Between(TableSchema table, string startColumnNameValue, string endColumnNameValue)
        {
            TableQuery tq = this.Parent.FindTableQuery(table);
            this.ColumnValue = new ColumnQuery(startColumnNameValue, tq);
            this.ColumnEndValue = new ColumnQuery(endColumnNameValue, tq);
            this.Comparison = ComparisonOperator.Between;
            return this.Parent;
        }

        public Query Between(TableSchema tableStartValue, string startColumnNameValue, TableSchema tableEndValue, string endColumnNameValue)
        {
            TableQuery tqSV = this.Parent.FindTableQuery(tableStartValue);
            this.ColumnValue = new ColumnQuery(startColumnNameValue, tqSV);
            TableQuery tqEV = this.Parent.FindTableQuery(tableEndValue);
            this.ColumnEndValue = new ColumnQuery(endColumnNameValue, tqEV);
            this.Comparison = ComparisonOperator.Between;
            return this.Parent;
        }

        public Query Like(TableSchema table, string columnName)
        {
            TableQuery tq = this.Parent.FindTableQuery(table);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.Like;
            return this.Parent;
        }

        public Query NotLike(TableSchema table, string columnName)
        {
            TableQuery tq = this.Parent.FindTableQuery(table);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.NotLike;
            return this.Parent;
        }

        #endregion

        #region Entity and ColumnName

        public Query Equal<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = this.Parent.FindTableQuery(table);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.Equal;
            return this.Parent;
        }

        public Query Different<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = this.Parent.FindTableQuery(table);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.Different;
            return this.Parent;
        }

        public Query GreaterThan<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = this.Parent.FindTableQuery(table);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.GreaterThan;
            return this.Parent;
        }

        public Query LessThan<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = this.Parent.FindTableQuery(table);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.LessThan;
            return this.Parent;
        }

        public Query GreaterThanOrEqual<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = this.Parent.FindTableQuery(table);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.GreaterThanOrEqual;
            return this.Parent;
        }

        public Query LessThanOrEqual<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = this.Parent.FindTableQuery(table);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.LessThanOrEqual;
            return this.Parent;
        }

        public Query Between<TEtt>(string startColumnNameValue, string endColumnNameValue)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = this.Parent.FindTableQuery(table);
            this.ColumnValue = new ColumnQuery(startColumnNameValue, tq);
            this.ColumnEndValue = new ColumnQuery(endColumnNameValue, tq);
            this.Comparison = ComparisonOperator.Between;
            return this.Parent;
        }

        public Query Between<TEttStartValue, TEttEndValue>(string startColumnNameValue, string endColumnNameValue)
            where TEttStartValue : EntityBase
            where TEttEndValue : EntityBase
        {
            TableSchema tableStartValue = SchemaContainer.GetSchema<TEttStartValue>();
            TableQuery tqSV = this.Parent.FindTableQuery(tableStartValue);
            this.ColumnValue = new ColumnQuery(startColumnNameValue, tqSV);

            TableSchema tableEndValue = SchemaContainer.GetSchema<TEttEndValue>();
            TableQuery tqEV = this.Parent.FindTableQuery(tableEndValue);
            this.ColumnEndValue = new ColumnQuery(endColumnNameValue, tqEV);

            this.Comparison = ComparisonOperator.Between;
            return this.Parent;
        }

        public Query Like<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = this.Parent.FindTableQuery(table);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.Like;
            return this.Parent;
        }

        public Query NotLike<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = this.Parent.FindTableQuery(table);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.NotLike;
            return this.Parent;
        }

        #endregion

        #region Table Name and ColumnName

        public Query Equal(string tableName, string columnName)
        {
            TableQuery tq = this.Parent.FindTableQuery(tableName);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.Equal;
            return this.Parent;
        }

        public Query Different(string tableName, string columnName)
        {
            TableQuery tq = this.Parent.FindTableQuery(tableName);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.Different;
            return this.Parent;
        }

        public Query GreaterThan(string tableName, string columnName)
        {
            TableQuery tq = this.Parent.FindTableQuery(tableName);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.GreaterThan;
            return this.Parent;
        }

        public Query LessThan(string tableName, string columnName)
        {
            TableQuery tq = this.Parent.FindTableQuery(tableName);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.LessThan;
            return this.Parent;
        }

        public Query GreaterThanOrEqual(string tableName, string columnName)
        {
            TableQuery tq = this.Parent.FindTableQuery(tableName);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.GreaterThanOrEqual;
            return this.Parent;
        }

        public Query LessThanOrEqual(string tableName, string columnName)
        {
            TableQuery tq = this.Parent.FindTableQuery(tableName);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.LessThanOrEqual;
            return this.Parent;
        }

        public Query Between(string tableName, string startColumnNameValue, string endColumnNameValue)
        {
            TableQuery tq = this.Parent.FindTableQuery(tableName);
            this.ColumnValue = new ColumnQuery(startColumnNameValue, tq);
            this.ColumnEndValue = new ColumnQuery(endColumnNameValue, tq);
            this.Comparison = ComparisonOperator.Between;
            return this.Parent;
        }

        public Query Between(string tableNameStartValue, string startColumnNameValue, string tableNameEndValue, string endColumnNameValue)
        {
            TableQuery tqSV = this.Parent.FindTableQuery(tableNameStartValue);
            this.ColumnValue = new ColumnQuery(startColumnNameValue, tqSV);
            TableQuery tqEV = this.Parent.FindTableQuery(tableNameEndValue);
            this.ColumnEndValue = new ColumnQuery(endColumnNameValue, tqEV);
            this.Comparison = ComparisonOperator.Between;
            return this.Parent;
        }

        public Query Like(string tableName, string columnName)
        {
            TableQuery tq = this.Parent.FindTableQuery(tableName);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.Like;
            return this.Parent;
        }

        public Query NotLike(string tableName, string columnName)
        {
            TableQuery tq = this.Parent.FindTableQuery(tableName);
            this.ColumnValue = new ColumnQuery(columnName, tq);
            this.Comparison = ComparisonOperator.NotLike;
            return this.Parent;
        }

        #endregion

        #region Sub Query

        public Query Equal(Query subQuery)
        {
            this.SubQuery = subQuery;
            this.Comparison = ComparisonOperator.Equal;
            return this.Parent;
        }

        public Query Different(Query subQuery)
        {
            this.SubQuery = subQuery;
            this.Comparison = ComparisonOperator.Different;
            return this.Parent;
        }

        public Query GreaterThan(Query subQuery)
        {
            this.SubQuery = subQuery;
            this.Comparison = ComparisonOperator.GreaterThan;
            return this.Parent;
        }

        public Query LessThan(Query subQuery)
        {
            this.SubQuery = subQuery;
            this.Comparison = ComparisonOperator.LessThan;
            return this.Parent;
        }

        public Query GreaterThanOrEqual(Query subQuery)
        {
            this.SubQuery = subQuery;
            this.Comparison = ComparisonOperator.GreaterThanOrEqual;
            return this.Parent;
        }

        public Query LessThanOrEqual(Query subQuery)
        {
            this.SubQuery = subQuery;
            this.Comparison = ComparisonOperator.LessThanOrEqual;
            return this.Parent;
        }

        public Query Like(Query subQuery)
        {
            this.SubQuery = subQuery;
            this.Comparison = ComparisonOperator.Like;
            return this.Parent;
        }

        public Query NotLike(Query subQuery)
        {
            this.SubQuery = subQuery;
            this.Comparison = ComparisonOperator.NotLike;
            return this.Parent;
        }

        public Query In(Query subQuery)
        {
            this.SubQuery = subQuery;
            this.Comparison = ComparisonOperator.In;
            return this.Parent;
        }

        public Query NotIn(Query subQuery)
        {
            this.SubQuery = subQuery;
            this.Comparison = ComparisonOperator.NotIn;
            return this.Parent;
        }

        #endregion

        #region Function

        #region functionName
        public Function EqualFunc(string functionName)
        {
            this.Function = new Function(functionName, this.Parent);
            this.Comparison = ComparisonOperator.Equal;
            return this.Function;
        }

        public Function DifferentFunc(string functionName)
        {
            this.Function = new Function(functionName, this.Parent);
            this.Comparison = ComparisonOperator.Different;
            return this.Function;
        }

        public Function GreaterThanFunc(string functionName)
        {
            this.Function = new Function(functionName, this.Parent);
            this.Comparison = ComparisonOperator.GreaterThan;
            return this.Function;
        }

        public Function LessThanFunc(string functionName)
        {
            this.Function = new Function(functionName, this.Parent);
            this.Comparison = ComparisonOperator.LessThan;
            return this.Function;
        }

        public Function GreaterThanOrEqualFunc(string functionName)
        {
            this.Function = new Function(functionName, this.Parent);
            this.Comparison = ComparisonOperator.GreaterThanOrEqual;
            return this.Function;
        }

        public Function LessThanOrEqualFunc(string functionName)
        {
            this.Function = new Function(functionName, this.Parent);
            this.Comparison = ComparisonOperator.LessThanOrEqual;
            return this.Function;
        }

        public Function LikeFunc(string functionName)
        {
            this.Function = new Function(functionName, this.Parent);
            this.Comparison = ComparisonOperator.Like;
            return this.Function;
        }

        public Function NotLikeFunc(string functionName)
        {
            this.Function = new Function(functionName, this.Parent);
            this.Comparison = ComparisonOperator.NotLike;
            return this.Function;
        }

        public Function InFunc(string functionName)
        {
            this.Function = new Function(functionName, this.Parent);
            this.Comparison = ComparisonOperator.In;
            return this.Function;
        }

        public Function NotInFunc(string functionName)
        {
            this.Function = new Function(functionName, this.Parent);
            this.Comparison = ComparisonOperator.NotIn;
            return this.Function;
        }
        #endregion

        #region Schema
        public Function EqualFunc<TEtt>()
            where TEtt: EntityBase
        {
            TableSchema functionSchema = TableSchema.FromEntity<TEtt>();
            this.Function = new Function(functionSchema, this.Parent);
            this.Comparison = ComparisonOperator.Equal;
            return this.Function;
        }

        public Function DifferentFunc<TEtt>()
            where TEtt : EntityBase
        {
            TableSchema functionSchema = TableSchema.FromEntity<TEtt>();
            this.Function = new Function(functionSchema, this.Parent);
            this.Comparison = ComparisonOperator.Different;
            return this.Function;
        }

        public Function GreaterThanFunc<TEtt>()
            where TEtt : EntityBase
        {
            TableSchema functionSchema = TableSchema.FromEntity<TEtt>();
            this.Function = new Function(functionSchema, this.Parent);
            this.Comparison = ComparisonOperator.GreaterThan;
            return this.Function;
        }

        public Function LessThanFunc<TEtt>()
            where TEtt : EntityBase
        {
            TableSchema functionSchema = TableSchema.FromEntity<TEtt>();
            this.Function = new Function(functionSchema, this.Parent);
            this.Comparison = ComparisonOperator.LessThan;
            return this.Function;
        }

        public Function GreaterThanOrEqualFunc<TEtt>()
            where TEtt : EntityBase
        {
            TableSchema functionSchema = TableSchema.FromEntity<TEtt>();
            this.Function = new Function(functionSchema, this.Parent);
            this.Comparison = ComparisonOperator.GreaterThanOrEqual;
            return this.Function;
        }

        public Function LessThanOrEqualFunc<TEtt>()
            where TEtt : EntityBase
        {
            TableSchema functionSchema = TableSchema.FromEntity<TEtt>();
            this.Function = new Function(functionSchema, this.Parent);
            this.Comparison = ComparisonOperator.LessThanOrEqual;
            return this.Function;
        }

        public Function LikeFunc<TEtt>()
            where TEtt : EntityBase
        {
            TableSchema functionSchema = TableSchema.FromEntity<TEtt>();
            this.Function = new Function(functionSchema, this.Parent);
            this.Comparison = ComparisonOperator.Like;
            return this.Function;
        }

        public Function NotLikeFunc<TEtt>()
            where TEtt : EntityBase
        {
            TableSchema functionSchema = TableSchema.FromEntity<TEtt>();
            this.Function = new Function(functionSchema, this.Parent);
            this.Comparison = ComparisonOperator.NotLike;
            return this.Function;
        }

        public Function InFunc<TEtt>()
            where TEtt : EntityBase
        {
            TableSchema functionSchema = TableSchema.FromEntity<TEtt>();
            this.Function = new Function(functionSchema, this.Parent);
            this.Comparison = ComparisonOperator.In;
            return this.Function;
        }

        public Function NotInFunc<TEtt>()
            where TEtt : EntityBase
        {
            TableSchema functionSchema = TableSchema.FromEntity<TEtt>();
            this.Function = new Function(functionSchema, this.Parent);
            this.Comparison = ComparisonOperator.NotIn;
            return this.Function;
        }
        #endregion

        #endregion

        #endregion

        #endregion
    }
}
