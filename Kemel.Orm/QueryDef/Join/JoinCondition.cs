using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Schema;
using Kemel.Orm.Entity;
using Kemel.Orm.Constants;

namespace Kemel.Orm.QueryDef
{
    public class JoinConditionCollection : List<JoinCondition>
    {
        new public JoinCondition Add(JoinCondition item)
        {
            base.Add(item);
            return item;
        }
    }

    public class JoinCondition
    {
        #region Properties
        public QueryJoin Parent { get; set; }

        public ConstraintType ConstraintType { get; set; }
        public ColumnQuery ColumnFrom { get; set; }
        public ComparisonOperator Operator { get; set; }
        public ColumnQuery ColumnTo { get; set; }

        public object Value { get; set; }

        private Query Query
        {
            get
            {
                return this.Parent.Parent;
            }
        }
        #endregion

        #region Constructor
        private JoinCondition(QueryJoin parent, ConstraintType constraintType, ColumnQuery columnfrom)
        {
            this.Parent = parent;
            this.ConstraintType = constraintType;
            this.ColumnFrom = columnfrom;
        }
        #endregion

        #region Static Methods
        public static JoinCondition On(ColumnSchema columnFrom, QueryJoin parent)
        {
            ColumnQuery column = parent.FindColumnQuery(columnFrom);
            return new JoinCondition(parent, ConstraintType.Where, column);
        }

        public static JoinCondition And(ColumnSchema columnFrom, QueryJoin parent)
        {
            ColumnQuery column = parent.FindColumnQuery(columnFrom);
            return new JoinCondition(parent, ConstraintType.And, column);
        }

        public static JoinCondition Or(ColumnSchema columnFrom, QueryJoin parent)
        {
            ColumnQuery column = parent.FindColumnQuery(columnFrom);
            return new JoinCondition(parent, ConstraintType.Or, column);
        }

        public static JoinCondition On(string columnName, QueryJoin parent)
        {
            ColumnQuery column = parent.FindColumnQuery(columnName);
            if (column == null)
                column = new ColumnQuery(columnName, parent);
            return new JoinCondition(parent, ConstraintType.Where, column);
        }

        public static JoinCondition And(string columnName, QueryJoin parent)
        {
            ColumnQuery column = parent.FindColumnQuery(columnName);
            if (column == null)
                column = new ColumnQuery(columnName, parent);
            return new JoinCondition(parent, ConstraintType.And, column);
        }

        public static JoinCondition Or(string columnName, QueryJoin parent)
        {
            ColumnQuery column = parent.FindColumnQuery(columnName);
            if (column == null)
                column = new ColumnQuery(columnName, parent);
            return new JoinCondition(parent, ConstraintType.Or, column);
        }

        public static JoinCondition On(string tableName, ColumnSchema columnFrom, QueryJoin parent)
        {
            ColumnQuery column = parent.FindColumnQuery(tableName, columnFrom);
            return new JoinCondition(parent, ConstraintType.Where, column);
        }

        public static JoinCondition And(string tableName, ColumnSchema columnFrom, QueryJoin parent)
        {
            ColumnQuery column = parent.FindColumnQuery(tableName, columnFrom);
            return new JoinCondition(parent, ConstraintType.And, column);
        }

        public static JoinCondition Or(string tableName, ColumnSchema columnFrom, QueryJoin parent)
        {
            ColumnQuery column = parent.FindColumnQuery(tableName, columnFrom);
            return new JoinCondition(parent, ConstraintType.Or, column);
        }

        public static JoinCondition On(string tableName, string columnName, QueryJoin parent)
        {
            ColumnQuery column = parent.FindColumnQuery(tableName, columnName);
            if (column == null)
                column = new ColumnQuery(columnName, parent);
            return new JoinCondition(parent, ConstraintType.Where, column);
        }

        public static JoinCondition And(string tableName, string columnName, QueryJoin parent)
        {
            ColumnQuery column = parent.FindColumnQuery(tableName, columnName);
            if (column == null)
                column = new ColumnQuery(columnName, parent);
            return new JoinCondition(parent, ConstraintType.And, column);
        }

        public static JoinCondition Or(string tableName, string columnName, QueryJoin parent)
        {
            ColumnQuery column = parent.FindColumnQuery(tableName, columnName);
            if (column == null)
                column = new ColumnQuery(columnName, parent);
            return new JoinCondition(parent, ConstraintType.Or, column);
        }
        #endregion

        #region Operators

        #region ColumnSchema
        public QueryJoin Equal(ColumnSchema columnTo)
        {
            this.ColumnTo = this.Query.FindColumnQueryInTables(columnTo);
            this.Operator = ComparisonOperator.Equal;
            return this.Parent;
        }

        public QueryJoin Different(ColumnSchema columnTo)
        {
            this.ColumnTo = this.Query.FindColumnQueryInTables(columnTo);
            this.Operator = ComparisonOperator.Different;
            return this.Parent;
        }

        public QueryJoin GreaterThan(ColumnSchema columnTo)
        {
            this.ColumnTo = this.Query.FindColumnQueryInTables(columnTo);
            this.Operator = ComparisonOperator.GreaterThan;
            return this.Parent;
        }

        public QueryJoin LessThan(ColumnSchema columnTo)
        {
            this.ColumnTo = this.Query.FindColumnQueryInTables(columnTo);
            this.Operator = ComparisonOperator.LessThan;
            return this.Parent;
        }

        public QueryJoin GreaterThanOrEqual(ColumnSchema columnTo)
        {
            this.ColumnTo = this.Query.FindColumnQueryInTables(columnTo);
            this.Operator = ComparisonOperator.GreaterThanOrEqual;
            return this.Parent;
        }

        public QueryJoin LessOrEqualThan(ColumnSchema columnTo)
        {
            this.ColumnTo = this.Query.FindColumnQueryInTables(columnTo);
            this.Operator = ComparisonOperator.LessThanOrEqual;
            return this.Parent;
        }

        public QueryJoin IsNull()
        {
            this.ColumnTo = null;
            this.Operator = ComparisonOperator.IsNull;
            return this.Parent;
        }

        public QueryJoin IsNotNull()
        {
            this.ColumnTo = null;
            this.Operator = ComparisonOperator.IsNotNull;
            return this.Parent;
        }
        #endregion

        #region columnName
        public QueryJoin Equal(TableSchema table, string columnName)
        {
            TableQuery tq = this.Query.FindTableQuery(table);
            this.ColumnTo = new ColumnQuery(columnName, tq);
            this.Operator = ComparisonOperator.Equal;
            return this.Parent;
        }

        public QueryJoin Different(TableSchema table, string columnName)
        {
            TableQuery tq = this.Query.FindTableQuery(table);
            this.ColumnTo = new ColumnQuery(columnName, tq);
            this.Operator = ComparisonOperator.Different;
            return this.Parent;
        }

        public QueryJoin GreaterThan(TableSchema table, string columnName)
        {
            TableQuery tq = this.Query.FindTableQuery(table);
            this.ColumnTo = new ColumnQuery(columnName, tq);
            this.Operator = ComparisonOperator.GreaterThan;
            return this.Parent;
        }

        public QueryJoin LessThan(TableSchema table, string columnName)
        {
            TableQuery tq = this.Query.FindTableQuery(table);
            this.ColumnTo = new ColumnQuery(columnName, tq);
            this.Operator = ComparisonOperator.LessThan;
            return this.Parent;
        }

        public QueryJoin GreaterThanOrEqual(TableSchema table, string columnName)
        {
            TableQuery tq = this.Query.FindTableQuery(table);
            this.ColumnTo = new ColumnQuery(columnName, tq);
            this.Operator = ComparisonOperator.GreaterThanOrEqual;
            return this.Parent;
        }

        public QueryJoin LessOrEqualThan(TableSchema table, string columnName)
        {
            TableQuery tq = this.Query.FindTableQuery(table);
            this.ColumnTo = new ColumnQuery(columnName, tq);
            this.Operator = ComparisonOperator.LessThanOrEqual;
            return this.Parent;
        }
        #endregion

        #region entity e columnName
        public QueryJoin Equal<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = this.Query.FindTableQuery(table);
            this.ColumnTo = new ColumnQuery(columnName, tq);
            this.Operator = ComparisonOperator.Equal;
            return this.Parent;
        }

        public QueryJoin Different<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = this.Query.FindTableQuery(table);
            this.ColumnTo = new ColumnQuery(columnName, tq);
            this.Operator = ComparisonOperator.Different;
            return this.Parent;
        }

        public QueryJoin GreaterThan<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = this.Query.FindTableQuery(table);
            this.ColumnTo = new ColumnQuery(columnName, tq);
            this.Operator = ComparisonOperator.GreaterThan;
            return this.Parent;
        }

        public QueryJoin LessThan<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = this.Query.FindTableQuery(table);
            this.ColumnTo = new ColumnQuery(columnName, tq);
            this.Operator = ComparisonOperator.LessThan;
            return this.Parent;
        }

        public QueryJoin GreaterThanOrEqual<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = this.Query.FindTableQuery(table);
            this.ColumnTo = new ColumnQuery(columnName, tq);
            this.Operator = ComparisonOperator.GreaterThanOrEqual;
            return this.Parent;
        }

        public QueryJoin LessOrEqualThan<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = this.Query.FindTableQuery(table);
            this.ColumnTo = new ColumnQuery(columnName, tq);
            this.Operator = ComparisonOperator.LessThanOrEqual;
            return this.Parent;
        }
        #endregion

        #region tableName e columnName
        public QueryJoin Equal(string tableName, string columnName)
        {
            TableQuery tq = this.Query.FindTableQuery(tableName);
            this.ColumnTo = new ColumnQuery(columnName, tq);
            this.Operator = ComparisonOperator.Equal;
            return this.Parent;
        }

        public QueryJoin Different(string tableName, string columnName)
        {
            TableQuery tq = this.Query.FindTableQuery(tableName);
            this.ColumnTo = new ColumnQuery(columnName, tq);
            this.Operator = ComparisonOperator.Different;
            return this.Parent;
        }

        public QueryJoin GreaterThan(string tableName, string columnName)
        {
            TableQuery tq = this.Query.FindTableQuery(tableName);
            this.ColumnTo = new ColumnQuery(columnName, tq);
            this.Operator = ComparisonOperator.GreaterThan;
            return this.Parent;
        }

        public QueryJoin LessThan(string tableName, string columnName)
        {
            TableQuery tq = this.Query.FindTableQuery(tableName);
            this.ColumnTo = new ColumnQuery(columnName, tq);
            this.Operator = ComparisonOperator.LessThan;
            return this.Parent;
        }

        public QueryJoin GreaterThanOrEqual(string tableName, string columnName)
        {
            TableQuery tq = this.Query.FindTableQuery(tableName);
            this.ColumnTo = new ColumnQuery(columnName, tq);
            this.Operator = ComparisonOperator.GreaterThanOrEqual;
            return this.Parent;
        }

        public QueryJoin LessOrEqualThan(string tableName, string columnName)
        {
            TableQuery tq = this.Query.FindTableQuery(tableName);
            this.ColumnTo = new ColumnQuery(columnName, tq);
            this.Operator = ComparisonOperator.LessThanOrEqual;
            return this.Parent;
        }
        #endregion

        #region Values

        public QueryJoin Equal(object value)
        {
            this.Value = value;
            this.Operator = ComparisonOperator.Equal;
            return this.Parent;
        }

        public QueryJoin Different(object value)
        {
            this.Value = value;
            this.Operator = ComparisonOperator.Different;
            return this.Parent;
        }

        public QueryJoin GreaterThan(object value)
        {
            this.Value = value;
            this.Operator = ComparisonOperator.GreaterThan;
            return this.Parent;
        }

        public QueryJoin LessThan(object value)
        {
            this.Value = value;
            this.Operator = ComparisonOperator.LessThan;
            return this.Parent;
        }

        public QueryJoin GreaterThanOrEqual(object value)
        {
            this.Value = value;
            this.Operator = ComparisonOperator.GreaterThanOrEqual;
            return this.Parent;
        }

        public QueryJoin LessOrEqualThan(object value)
        {
            this.Value = value;
            this.Operator = ComparisonOperator.LessThanOrEqual;
            return this.Parent;
        }

        #endregion

        #endregion
    }
}
