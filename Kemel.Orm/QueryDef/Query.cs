using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Schema;
using Kemel.Orm.Entity;
using System.Diagnostics;
using Kemel.Orm.Data;
using Kemel.Orm.Providers;
using Kemel.Orm.Constants;

namespace Kemel.Orm.QueryDef
{
    public class Query
    {
        #region Properties

        public Query Parent { get; set; }

        public string Alias { get; set; }

        public string ProcedureName { get; set; }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>The index.</value>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>The level.</value>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public QueryType Type { get; set; }

        /// <summary>
        /// Gets or sets the into table.
        /// </summary>
        /// <value>The into table.</value>
        public TableQuery IntoTable { get; set; }
        public TableQuery Function
        {
            get
            {
                return this.IntoTable;
            }
            set
            {
                this.IntoTable = value;
            }
        }

        private TableQueryCollection lstTables = null;
        /// <summary>
        /// Gets the tables.
        /// </summary>
        /// <value>The tables.</value>
        public TableQueryCollection Tables
        {
            get
            {
                if (this.lstTables == null)
                    this.lstTables = new TableQueryCollection();
                return this.lstTables;
            }
        }

        private bool blnIsDistinct = false;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is distinct.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is distinct; otherwise, <c>false</c>.
        /// </value>
        public bool IsDistinct
        {
            get
            {
                return this.blnIsDistinct;
            }
            set
            {
                this.blnIsDistinct = value;
            }
        }

        private int intTopRecords = 0;
        /// <summary>
        /// Gets or sets the top records.
        /// </summary>
        /// <value>The top records.</value>
        public int TopRecords
        {
            get
            {
                return this.intTopRecords;
            }
            set
            {
                this.intTopRecords = value;
            }
        }

        private bool blnIsAutoNoLock = true;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is with nolock on all tables.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is with nolock on all tables; otherwise, <c>false</c>.
        /// </value>
        public bool IsAutoNoLock
        {
            get
            {
                return this.blnIsAutoNoLock;
            }
            set
            {
                this.blnIsAutoNoLock = value;
            }
        }

        private QueryJoinCollection lstJoins = null;
        /// <summary>
        /// Gets the joins.
        /// </summary>
        /// <value>The joins.</value>
        public QueryJoinCollection Joins
        {
            get
            {
                if (this.lstJoins == null)
                    this.lstJoins = new QueryJoinCollection();
                return this.lstJoins;
            }
        }

        private ConstraintCollection lstConstraints = null;
        /// <summary>
        /// Gets the constraints.
        /// </summary>
        /// <value>The constraints.</value>
        public ConstraintCollection Constraints
        {
            get
            {
                if (this.lstConstraints == null)
                    this.lstConstraints = new ConstraintCollection();
                return this.lstConstraints;
            }
        }

        private GroupByCollection lstGroupBy = null;
        /// <summary>
        /// Gets the group by.
        /// </summary>
        /// <value>The group by.</value>
        public GroupByCollection GroupBys
        {
            get
            {
                if (this.lstGroupBy == null)
                    this.lstGroupBy = new GroupByCollection();
                return this.lstGroupBy;
            }
        }

        private OrderByCollection lstOrderBy = null;
        /// <summary>
        /// Gets the order by.
        /// </summary>
        /// <value>The order by.</value>
        public OrderByCollection OrderBys
        {
            get
            {
                if (this.lstOrderBy == null)
                    this.lstOrderBy = new OrderByCollection();
                return this.lstOrderBy;
            }
        }

        private StringColumnCollection lstStringColumn = null;
        /// <summary>
        /// Gets the String Columns.
        /// </summary>
        /// <value>The String Columns.</value>
        public StringColumnCollection StringColumns
        {
            get
            {
                if (this.lstStringColumn == null)
                    this.lstStringColumn = new StringColumnCollection();
                return this.lstStringColumn;
            }
        }

        private HavingConstraintCollection lstHavingConstraints = null;
        /// <summary>
        /// Gets the having constraints.
        /// </summary>
        /// <value>The having constraints.</value>
        public HavingConstraintCollection HavingConstraints
        {
            get
            {
                if (this.lstHavingConstraints == null)
                    this.lstHavingConstraints = new HavingConstraintCollection();
                return this.lstHavingConstraints;
            }
        }

        private SetValueCollection lstSetValues = null;
        /// <summary>
        /// Gets the set values.
        /// </summary>
        /// <value>The set values.</value>
        public SetValueCollection SetValues
        {
            get
            {
                if (this.lstSetValues == null)
                    this.lstSetValues = new SetValueCollection();
                return this.lstSetValues;
            }
        }

        private UnionCollection lstUnion = null;
        /// <summary>
        /// Gets the unions.
        /// </summary>
        /// <value>The order by.</value>
        public UnionCollection Unions
        {
            get
            {
                if (this.lstUnion == null)
                    this.lstUnion = new UnionCollection();
                return this.lstUnion;
            }
        }

        private ConcatCollection lstConcats = null;
        /// <summary>
        /// Gets the unions.
        /// </summary>
        /// <value>The order by.</value>
        public ConcatCollection Concats
        {
            get
            {
                if (this.lstConcats == null)
                    this.lstConcats = new ConcatCollection();
                return this.lstConcats;
            }
        }

        private int queryIncLevel = 0;
        public int NextLevel
        {
            get
            {
                if (this.Parent == null)
                {
                    queryIncLevel++;
                    return this.Level + queryIncLevel;
                }
                else
                {
                    return this.Parent.NextLevel;
                }
            }
        }

        public Provider Provider { get; private set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Query"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        private Query(QueryType type, Provider provider)
        {
            this.Type = type;
            this.Provider = provider;
        }
        #endregion

        #region Static Methods

        /// <summary>
        /// Initialize Select query.
        /// </summary>
        /// <returns></returns>
        internal static Query Select(Provider provider)
        {
            return new Query(QueryType.Select, provider);
        }

        /// <summary>
        /// Initialize Insert query.
        /// </summary>
        /// <returns></returns>
        internal static Query Insert(Provider provider)
        {
            return new Query(QueryType.Insert, provider);
        }

        /// <summary>
        /// Initialize Update query.
        /// </summary>
        /// <returns></returns>
        internal static Query Update(Provider provider)
        {
            return new Query(QueryType.Update, provider);
        }

        /// <summary>
        /// Initialize Delete query.
        /// </summary>
        /// <returns></returns>
        internal static Query Delete(Provider provider)
        {
            return new Query(QueryType.Delete, provider);
        }

        /// <summary>
        /// Initialize Procedure query.
        /// </summary>
        /// <returns></returns>
        internal static Query Procedure(string procedureName, Provider provider)
        {
            Query query = new Query(QueryType.Procedure, provider);
            query.ProcedureName = procedureName;
            return query;
        }

        /// <summary>
        /// Initialize Procedure query.
        /// </summary>
        /// <returns></returns>
        internal static Query Procedure<TEtt>()
            where TEtt: EntityBase
        {
            Provider provider = Provider.GetProvider<TEtt>();
            Query query = new Query(QueryType.Procedure, provider);
            query.Into<TEtt>();
            query.ProcedureName = query.IntoTable.PatternName;
            return query;
        }

        #endregion

        #region Select Methods

        /// <summary>
        /// Distinct.
        /// </summary>
        /// <returns></returns>
        public Query Distinct()
        {
            this.blnIsDistinct = true;
            return this;
        }

        /// <summary>
        /// Top records.
        /// </summary>
        /// <param name="topRecords">The top records.</param>
        /// <returns></returns>
        public Query Top(int topRecords)
        {
            this.intTopRecords = topRecords;
            return this;
        }

        public Query DisableAutoNoLock()
        {
            this.blnIsAutoNoLock = false;
            return this;
        }

        #region From

        /// <summary>
        /// Froms the specified table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public TableQuery From(TableSchema table)
        {
            return this.Add(TableQuery.From(table, this));
        }

        /// <summary>
        /// Froms the specified entity.
        /// </summary>
        /// <typeparam name="Entity">The type of the Entity.</typeparam>
        /// <returns></returns>
        public TableQuery From<TEtt>()
            where TEtt : EntityBase
        {
            return this.Add(TableQuery.From<TEtt>(this));
        }

        /// <summary>
        /// Froms the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public TableQuery From(EntityBase entity)
        {
            return this.Add(TableQuery.From(entity, this));
        }

        /// <summary>
        /// Froms the specified table name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public TableQuery From(string tableName)
        {
            return this.Add(TableQuery.From(tableName, this));
        }

        /// <summary>
        /// Froms the specified table name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public TableQuery From(Query subQuery)
        {
            subQuery.Parent = this;
            return this.Add(TableQuery.From(subQuery, this));
        }

        #endregion

        #region Join

        /// <summary>
        /// Joins the specified table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public QueryJoin Join(TableSchema table)
        {
            return this.Add(QueryJoin.Join(table, this));
        }

        /// <summary>
        /// Joins the specified entity.
        /// </summary>
        /// <typeparam name="Entity">The type of the entity.</typeparam>
        /// <returns></returns>
        public QueryJoin Join<TEtt>()
            where TEtt : EntityBase
        {
            return this.Add(QueryJoin.Join<TEtt>(this));
        }

        /// <summary>
        /// Joins the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public QueryJoin Join(EntityBase entity)
        {
            return this.Add(QueryJoin.Join(entity, this));
        }

        /// <summary>
        /// Joins the specified table name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public QueryJoin Join(string tableName)
        {
            return this.Add(QueryJoin.Join(tableName, this));
        }

        /// <summary>
        /// Joins the specified sub-query.
        /// </summary>
        /// <param name="subQuery">The sub-query.</param>
        /// <returns></returns>
        public QueryJoin Join(Query subQuery)
        {
            subQuery.Parent = this;
            return this.Add(QueryJoin.Join(subQuery, this));
        }

        #endregion

        #region Constraints

        /// <summary>
        /// Where the column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public Constraint Where(ColumnSchema column)
        {
            if(this.Constraints.Count == 0)
                return this.Add(Constraint.Where(column, this));
            else
                return this.Add(Constraint.And(column, this));
        }

        /// <summary>
        /// And the column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public Constraint And(ColumnSchema column)
        {
            if (this.Constraints.Count == 0)
                return this.Add(Constraint.Where(column, this));
            else
                return this.Add(Constraint.And(column, this));
        }

        /// <summary>
        /// Or the column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public Constraint Or(ColumnSchema column)
        {
            return this.Add(Constraint.Or(column, this));
        }

        /// <summary>
        /// Where the specified Column Name.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public Constraint Where(TableSchema table, string columnName)
        {
            if(this.Constraints.Count == 0)
                return this.Add(Constraint.Where(table, columnName, this));
            else
                return this.Add(Constraint.And(table, columnName, this));
        }

        /// <summary>
        /// And the specified Column Name.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public Constraint And(TableSchema table, string columnName)
        {
            if (this.Constraints.Count == 0)
                return this.Add(Constraint.Where(table, columnName, this));
            else
                return this.Add(Constraint.And(table, columnName, this));
        }

        /// <summary>
        /// Or the specified Column Name.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public Constraint Or(TableSchema table, string columnName)
        {
            return this.Add(Constraint.Or(table, columnName, this));
        }

        /// <summary>
        /// Where the specified Constant string value.
        /// </summary>
        /// <param name="constantValue">Constant string value.</param>
        /// <returns></returns>
        public Constraint Where(string constantValue)
        {
            if(this.Constraints.Count == 0)
                return this.Add(Constraint.Where(constantValue, this));
            else
                return this.Add(Constraint.And(constantValue, this));
        }

        /// <summary>
        /// And the specified Constant string value.
        /// </summary>
        /// <param name="constantValue">Constant string value.</param>
        /// <returns></returns>
        public Constraint And(string constantValue)
        {
            if (this.Constraints.Count == 0)
                return this.Add(Constraint.Where(constantValue, this));
            else
                return this.Add(Constraint.And(constantValue, this));
        }

        /// <summary>
        /// Or the specified Constant string value.
        /// </summary>
        /// <param name="constantValue">Constant string value.</param>
        /// <returns></returns>
        public Constraint Or(string constantValue)
        {
            return this.Add(Constraint.Or(constantValue, this));
        }

        /// <summary>
        /// Where the specified Column Name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public Constraint Where(string tableName, string columnName)
        {
            if(this.Constraints.Count == 0)
                return this.Add(Constraint.Where(tableName, columnName, this));
            else
                return this.Add(Constraint.And(tableName, columnName, this));
        }

        /// <summary>
        /// And the specified Column Name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public Constraint And(string tableName, string columnName)
        {
            if (this.Constraints.Count == 0)
                return this.Add(Constraint.Where(tableName, columnName, this));
            else
                return this.Add(Constraint.And(tableName, columnName, this));
        }

        /// <summary>
        /// Or the specified Column Name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public Constraint Or(string tableName, string columnName)
        {
            return this.Add(Constraint.Or(tableName, columnName, this));
        }

        /// <summary>
        /// Where the specified Column Name.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public Constraint Where<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            if(this.Constraints.Count == 0)
                return this.Add(Constraint.Where<TEtt>(columnName, this));
            else
                return this.Add(Constraint.And<TEtt>(columnName, this));
        }

        /// <summary>
        /// And the specified Column Name.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public Constraint And<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            if(this.Constraints.Count == 0)
                return this.Add(Constraint.Where<TEtt>(columnName, this));
            else
                return this.Add(Constraint.And<TEtt>(columnName, this));
        }

        /// <summary>
        /// Or the specified Column Name.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public Constraint Or<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            return this.Add(Constraint.Or<TEtt>(columnName, this));
        }

        public Query CloseParentesis()
        {
            this.Add(Constraint.CloseParentesis(this));
            return this;
        }

        #region Open Parentesis

        /// <summary>
        /// Open Parentesis
        /// </summary>
        /// <returns></returns>
        public Query OpenParentesis()
        {
            this.Add(Constraint.OpenParentesis(ConstraintType.None, this));
            return this;
        }

        /// <summary>
        /// Where Open Parentesis
        /// </summary>
        /// <returns></returns>
        public Query WhereOpenParentesis()
        {
            this.Add(Constraint.OpenParentesis(ConstraintType.Where, this));
            return this;
        }

        /// <summary>
        /// And Open Parentesis
        /// </summary>
        /// <returns></returns>
        public Query AndOpenParentesis()
        {
            this.Add(Constraint.OpenParentesis(ConstraintType.And, this));
            return this;
        }

        /// <summary>
        /// Or Open Parentesis
        /// </summary>
        /// <returns></returns>
        public Query OrOpenParentesis()
        {
            this.Add(Constraint.OpenParentesis(ConstraintType.Or, this));
            return this;
        }

        /// <summary>
        /// Where the column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public Constraint WhereOpenParentesis(ColumnSchema column)
        {
            this.Add(Constraint.OpenParentesis(ConstraintType.Where, this));
            return this.Add(Constraint.Where(column, this));
        }

        /// <summary>
        /// And the column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public Constraint AndOpenParentesis(ColumnSchema column)
        {
            this.Add(Constraint.OpenParentesis(ConstraintType.And, this));
            return this.Add(Constraint.And(column, this));
        }

        /// <summary>
        /// Or the column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public Constraint OrOpenParentesis(ColumnSchema column)
        {
            this.Add(Constraint.OpenParentesis(ConstraintType.Or, this));
            return this.Add(Constraint.Or(column, this));
        }

        /// <summary>
        /// Where the specified Column Name.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public Constraint WhereOpenParentesis(TableSchema table, string columnName)
        {
            this.Add(Constraint.OpenParentesis(ConstraintType.Where, this));
            return this.Add(Constraint.Where(table, columnName, this));
        }

        /// <summary>
        /// And the specified Column Name.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public Constraint AndOpenParentesis(TableSchema table, string columnName)
        {
            this.Add(Constraint.OpenParentesis(ConstraintType.And, this));
            return this.Add(Constraint.And(table, columnName, this));
        }

        /// <summary>
        /// Or the specified Column Name.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public Constraint OrOpenParentesis(TableSchema table, string columnName)
        {
            this.Add(Constraint.OpenParentesis(ConstraintType.Or, this));
            return this.Add(Constraint.Or(table, columnName, this));
        }

        /// <summary>
        /// Where the specified Column Name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public Constraint WhereOpenParentesis(string tableName, string columnName)
        {
            this.Add(Constraint.OpenParentesis(ConstraintType.Where, this));
            return this.Add(Constraint.Where(tableName, columnName, this));
        }

        /// <summary>
        /// And the specified Column Name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public Constraint AndOpenParentesis(string tableName, string columnName)
        {
            this.Add(Constraint.OpenParentesis(ConstraintType.And, this));
            return this.Add(Constraint.And(tableName, columnName, this));
        }

        /// <summary>
        /// Or the specified Column Name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public Constraint OrOpenParentesis(string tableName, string columnName)
        {
            this.Add(Constraint.OpenParentesis(ConstraintType.Or, this));
            return this.Add(Constraint.Or(tableName, columnName, this));
        }

        /// <summary>
        /// Where the specified Column Name.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public Constraint WhereOpenParentesis<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            this.Add(Constraint.OpenParentesis(ConstraintType.Where, this));
            return this.Add(Constraint.Where<TEtt>(columnName, this));
        }

        /// <summary>
        /// And the specified Column Name.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public Constraint AndOpenParentesis<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            this.Add(Constraint.OpenParentesis(ConstraintType.And, this));
            return this.Add(Constraint.And<TEtt>(columnName, this));
        }

        /// <summary>
        /// Or the specified Column Name.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public Constraint OrOpenParentesis<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            this.Add(Constraint.OpenParentesis(ConstraintType.Or, this));
            return this.Add(Constraint.Or<TEtt>(columnName, this));
        }

        #endregion

        #endregion

        #region Order By

        public Query OrderBy(TableSchema table, params string[] columns)
        {
            TableQuery tq = this.FindTableQuery(table);
            foreach (string column in columns)
            {
                this.Add(new OrderBy(column, tq));
            }
            return this;
        }

        public Query OrderBy(string tableName, params string[] columns)
        {
            TableQuery tq = this.FindTableQuery(tableName);
            foreach (string column in columns)
            {
                this.Add(new OrderBy(column, tq));
            }
            return this;
        }

        public OrderBy OrderBy(ColumnSchema column)
        {
            TableQuery tq = this.FindTableQuery(column.Parent);
            return this.Add(new OrderBy(column, tq));
        }

        public OrderBy OrderBy(TableSchema table, string columnName)
        {
            TableQuery tq = this.FindTableQuery(table);
            return this.Add(new OrderBy(columnName, tq));
        }

        public OrderBy OrderBy(string tableName, string columnName)
        {
            TableQuery tq = this.FindTableQuery(tableName);
            return this.Add(new OrderBy(columnName, tq));
        }

        public Query OrderBy<TEtt>(params string[] columns)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = this.FindTableQuery(table);
            foreach (string column in columns)
            {
                this.Add(new OrderBy(column, tq));
            }
            return this;
        }

        public OrderBy OrderBy<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = this.FindTableQuery(table);
            return this.Add(new OrderBy(columnName, tq));
        }

        public OrderBy OrderBy(string alias)
        {
            return this.Add(new OrderBy(alias));
        }

        #endregion

        #region Group By

        public Query GroupBy(TableSchema table, params string[] columns)
        {
            TableQuery tq = this.FindTableQuery(table);
            foreach (string column in columns)
            {
                this.Add(new GroupBy(column, tq));
            }
            return this;
        }

        public Query GroupBy(string tableName, params string[] columns)
        {
            TableQuery tq = this.FindTableQuery(tableName);
            foreach (string column in columns)
            {
                this.Add(new GroupBy(column, tq));
            }
            return this;
        }

        public Query GroupBy(params ColumnSchema[] columns)
        {
            foreach (ColumnSchema column in columns)
            {
                TableQuery tq = this.FindTableQuery(column.Parent);
                this.Add(new GroupBy(column, tq));
            }
            return this;
        }

        public Query GroupBy<TEtt>(params string[] columns)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = this.FindTableQuery(table);
            foreach (string column in columns)
            {
                this.Add(new GroupBy(column, tq));
            }
            return this;
        }

        public Query GroupBy(string alias)
        {
            this.Add(new GroupBy(alias));
            return this;
        }

        #endregion

        #region Union

        public Query Union(Query query)
        {
            Union union = new Union(this, false, query);
            this.Add(union);
            return this;
        }

        public Query UnionAll(Query query)
        {
            Union union = new Union(this, true, query);
            this.Add(union);
            return this;
        }

        #endregion

        #region Concat

        public Concat Concat()
        {
            Concat concat = new Concat(this);
            this.Add(concat);
            return concat;
        }

        #endregion

        public StringColumn ConstantValueColumn(string constantValue)
        {
            return this.Add(new StringColumn(this, constantValue));
        }

        #endregion

        #region Insert Methods

        #region Into

        /// <summary>
        /// Into the specified table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public Query Into(TableSchema table)
        {
            return Into(TableQuery.From(table, this));
        }

        /// <summary>
        /// Into the specified table.
        /// </summary>
        /// <typeparam name="Entity">The type of the Entity.</typeparam>
        /// <returns></returns>
        public Query Into<TEtt>()
            where TEtt : EntityBase
        {
            return Into(TableQuery.From<TEtt>(this));
        }

        /// <summary>
        /// Into the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public Query Into(EntityBase entity)
        {
            return Into(TableQuery.From(entity, this));
        }

        /// <summary>
        /// Into the specified table name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public Query Into(string tableName)
        {
            return Into(TableQuery.From(tableName, this));
        }

        private Query Into(TableQuery tableQuery)
        {
            this.IntoTable = tableQuery;

            if (this.Type == QueryType.Delete && tableQuery.TableSchema != null && tableQuery.TableSchema.IsLogicalExclusion)
            {
                this.Type = QueryType.Update;
                this.Set(tableQuery.TableSchema.LogicalExclusionColumn).Equal(true);
            }

            return this;
        }

        #endregion

        #region Set

        /// <summary>
        /// Sets the specified column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public SetValue Set(ColumnSchema column)
        {
            return this.Add(SetValue.Set(column, this));
        }

        /// <summary>
        /// Sets the specified column.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public SetValue Set(TableSchema table, string columnName)
        {
            return this.Add(SetValue.Set(table, columnName, this));
        }

        /// <summary>
        /// Sets the specified column.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public SetValue Set(string tableName, string columnName)
        {
            return this.Add(SetValue.Set(tableName, columnName, this));
        }

        /// <summary>
        /// Sets the specified column.
        /// </summary>
        /// <typeparam name="Entity">The type of the ntity.</typeparam>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public SetValue Set<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            return this.Add(SetValue.Set<TEtt>(columnName, this));
        }

        #endregion

        #endregion

        #region Adds

        /// <summary>
        /// Adds the specified table.
        /// </summary>
        /// <param name="item">The table.</param>
        /// <returns></returns>
        protected TableQuery Add(TableQuery item)
        {
            if (item.TableSchema != null && item.TableSchema.IsLogicalExclusion)
            {
                this.Tables.Add(item);
                this.Where(item.TableSchema.LogicalExclusionColumn).Equal(false);
                return item;
            }
            else
                return this.Tables.Add(item);
        }

        /// <summary>
        /// Adds the specified join.
        /// </summary>
        /// <param name="item">The join.</param>
        /// <returns></returns>
        protected QueryJoin Add(QueryJoin item)
        {
            return this.Joins.Add(item);
        }

        /// <summary>
        /// Adds the specified Constraint.
        /// </summary>
        /// <param name="item">The Constraint.</param>
        /// <returns></returns>
        protected Constraint Add(Constraint item)
        {
            return this.Constraints.Add(item);
        }

        /// <summary>
        /// Adds the specified Group By.
        /// </summary>
        /// <param name="item">The Group By.</param>
        /// <returns></returns>
        protected GroupBy Add(GroupBy item)
        {
            return this.GroupBys.Add(item);
        }

        /// <summary>
        /// Adds the specified Order By.
        /// </summary>
        /// <param name="item">The Order By.</param>
        /// <returns></returns>
        protected OrderBy Add(OrderBy item)
        {
            return this.OrderBys.Add(item);
        }

        /// <summary>
        /// Adds the specified Having Constraint.
        /// </summary>
        /// <param name="item">The Having Constraint.</param>
        /// <returns></returns>
        protected HavingConstraint Add(HavingConstraint item)
        {
            return this.HavingConstraints.Add(item);
        }

        /// <summary>
        /// Adds the specified Set Value.
        /// </summary>
        /// <param name="item">The Set Value.</param>
        /// <returns></returns>
        protected SetValue Add(SetValue item)
        {
            return this.SetValues.Add(item);
        }

        /// <summary>
        /// Adds the specified StringColumn.
        /// </summary>
        /// <param name="item">The StringColumn.</param>
        /// <returns></returns>
        protected StringColumn Add(StringColumn item)
        {
            return this.StringColumns.Add(item);
        }

        /// <summary>
        /// Adds the specified Union Query.
        /// </summary>
        /// <param name="item">The Union Query.</param>
        /// <returns></returns>
        protected Union Add(Union item)
        {
            return this.Unions.Add(item);
        }

        /// <summary>
        /// Adds the specified Concat.
        /// </summary>
        /// <param name="item">The Concat.</param>
        /// <returns></returns>
        protected Concat Add(Concat item)
        {
            return this.Concats.Add(item);
        }

        #endregion

        #region Auxiliar Methods

        /// <summary>
        /// Finds the column query in tables.
        /// </summary>
        /// <param name="columnTo">The column to.</param>
        /// <returns></returns>
        public ColumnQuery FindColumnQueryInTables(ColumnSchema columnTo)
        {
            string columnName = columnTo.Name.ToUpper();
            TableQuery tq = this.FindTableQuery(columnTo.Parent);
            foreach (ColumnQuery column in tq.ColumnsQuery)
            {
                if (column.ColumnSchema != null)
                {
                    if (column.ColumnSchema.Name.ToUpper().Equals(columnName))
                        return column;
                }
            }

            return new ColumnQuery(columnTo, tq);
        }

        /// <summary>
        /// Finds the table query.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public TableQuery FindTableQuery(TableSchema table)
        {
            string tableName = table.Name.ToUpper();

            for (int i = this.Joins.Count - 1; i >= 0; i--)
            {
                TableQuery aux = this.Joins[i];
                if (aux.EqualsSchemaTableName(tableName))
                    return aux;
            }

            for (int i = this.Tables.Count - 1; i >= 0; i--)
            {
                TableQuery aux = this.Tables[i];
                if (aux.EqualsSchemaTableName(tableName))
                    return aux;
            }

            if (this.IntoTable != null && this.IntoTable.EqualsSchemaTableName(tableName))
                return this.IntoTable;

            for (int i = 0; i < this.Constraints.Count; i++)
            {
                Constraint constraint = this.Constraints[i];
                if (constraint.Function != null && constraint.Function.EqualsSchemaTableName(tableName))
                {
                    return constraint.Function;
                }
            }

            if (this.Parent != null)
            {
                TableQuery tq = this.Parent.FindTableQuery(table);
                if (tq != null)
                    return tq;
            }

            throw new OrmException(Messages.TableDoesNotExistInQuery);
        }

        /// <summary>
        /// Finds the table query.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public TableQuery FindTableQuery(string tableName)
        {
            tableName = tableName.ToUpper();

            for (int i = this.Joins.Count - 1; i >= 0; i--)
            {
                TableQuery aux = this.Joins[i];
                if (aux.EqualsTableNameOrAlias(tableName))
                    return aux;
            }

            for (int i = this.Tables.Count - 1; i >= 0; i--)
            {
                TableQuery aux = this.Tables[i];
                if (aux.EqualsTableNameOrAlias(tableName))
                    return aux;
            }

            if (this.IntoTable != null && this.IntoTable.EqualsTableNameOrAlias(tableName))
                return this.IntoTable;

            for (int i = 0; i < this.Constraints.Count; i++)
            {
                Constraint constraint = this.Constraints[i];
                if (constraint.Function != null && constraint.Function.EqualsTableNameOrAlias(tableName))
                {
                    return constraint.Function;
                }
            }

            if (this.Parent != null)
            {
                TableQuery tq = this.Parent.FindTableQuery(tableName);
                if (tq != null)
                    return tq;
            }

            throw new OrmException(Messages.TableDoesNotExistInQuery);
        }

        public void WriteQuery(OrmCommand command)
        {
            QueryBuilder builder = this.Provider.QueryBuilder;

            switch (this.Type)
            {
                case QueryType.Select:
                    builder.WriteSelect(command, this);
                    break;
                case QueryType.Insert:
                    builder.WriteInsert(command, this);
                    break;
                case QueryType.Delete:
                    builder.WriteDelete(command, this);
                    break;
                case QueryType.Update:
                    builder.WriteUpdate(command, this);
                    break;
                case QueryType.Procedure:
                    builder.WriteProcedure(command, this);
                    break;
                case QueryType.Function:
                    //builder.WriteFunction(command, this);
                    break;
            }
        }

        public Query End()
        {
            return this.Parent;
        }

        public Query As(string alias)
        {
            this.Alias = alias;
            return this.Parent;
        }

        #endregion

        public Executer Execute()
        {
            return new Executer(this);
        }

        public Executer Execute(OrmTransaction transaction)
        {
            return new Executer(this, transaction);
        }
    }
}
