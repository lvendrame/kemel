using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.NQuery.Storage.Table;
using Kemel.Orm.NQuery.Storage.Join;
using Kemel.Orm.NQuery.Storage.Constraint;
using Kemel.Orm.NQuery.Storage.StoredSelect;
using Kemel.Orm.NQuery.Storage.Column;
using Kemel.Orm.NQuery.Storage.Value;
using Kemel.Orm.Providers;
using Kemel.Orm.Schema;
using Kemel.Orm.Entity;
using Kemel.Orm.NQuery.Storage.Function;
using Kemel.Orm.Data;
using Kemel.Orm.NQuery.Storage.Function.Aggregated;

namespace Kemel.Orm.NQuery.Storage
{

	public class Query : IGetQuery
	{

		#region Properties

		private Query qryParent;
		public Query Parent {
			get { return qryParent; }
			set { qryParent = value; }
		}

		private int objIndex = -1;
		/// <summary>
		/// Gets or sets the index.
		/// </summary>
		/// <value>The index.</value>
		public int Index {
			get { return this.objIndex; }
			set { this.objIndex = value; }
		}

		public int NextIndex {
			get {
				this.objIndex = this.objIndex + 1;
				return this.objIndex;
			}
		}

        private int objLevel = 0;
		/// <summary>
		/// Gets or sets the level.
		/// </summary>
		/// <value>The level.</value>
		public int Level {
			get { return this.objLevel; }
			set { this.objLevel = value; }
		}

		private int queryIncLevel = 0;
		public int NextLevel {
			get {
				if (this.Parent == null) {
					queryIncLevel += 1;
					return this.Level + queryIncLevel;
				} else {
					return this.Parent.NextLevel;
				}
			}
		}

        private Provider objProvider = null;
		public Provider Provider {
			get { return this.objProvider; }
			private set { this.objProvider = value; }
		}

		private QueryType objType = QueryType.Select;
		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		/// <value>The type.</value>
		public QueryType Type {
			get { return this.objType; }
			set { this.objType = value; }
		}

        private StoredTable objIntoTable = null;
		/// <summary>
		/// Gets or sets the into table.
		/// </summary>
		/// <value>The into table.</value>
		public StoredTable IntoTable {
			get { return this.objIntoTable; }

			set { this.objIntoTable = value; }
		}

        private StoredTableCollection lstTables = null;
		/// <summary>
		/// Gets the tables.
		/// </summary>
		/// <value>The tables.</value>
		public StoredTableCollection Tables {
			get {
				if (this.lstTables == null) {
					this.lstTables = new StoredTableCollection();
				}
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
		public bool IsDistinct {
			get { return this.blnIsDistinct; }
			set { this.blnIsDistinct = value; }
		}

        private int intTopRecords = 0;
		/// <summary>
		/// Gets or sets the top records.
		/// </summary>
		/// <value>The top records.</value>
		public int TopRecords {
			get { return this.intTopRecords; }
			set { this.intTopRecords = value; }
		}

		private bool blnIsAutoNoLock = true;
		/// <summary>
		/// Gets or sets a value indicating whether this instance is with nolock on all tables.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is with nolock on all tables; otherwise, <c>false</c>.
		/// </value>
		public bool IsAutoNoLock {
			get { return this.blnIsAutoNoLock; }
			set { this.blnIsAutoNoLock = value; }
		}

        private StoredJoinCollection lstJoins = null;
		/// <summary>
		/// Gets the joins.
		/// </summary>
		/// <value>The joins.</value>
		public StoredJoinCollection Joins {
			get {
				if (this.lstJoins == null) {
					this.lstJoins = new StoredJoinCollection();
				}
				return this.lstJoins;
			}
		}

		private StoredConstraintCollection lstConstraints = null;
		/// <summary>
		/// Gets the constraints.
		/// </summary>
		/// <value>The constraints.</value>
		public StoredConstraintCollection Constraints {
			get {
				if (this.lstConstraints == null) {
					this.lstConstraints = new StoredConstraintCollection();
				}
				return this.lstConstraints;
			}
		}

        private StoredGroupByCollection lstGroupBy = null;
		/// <summary>
		/// Gets the group by.
		/// </summary>
		/// <value>The group by.</value>
		public StoredGroupByCollection GroupBys {
			get {
				if (this.lstGroupBy == null) {
					this.lstGroupBy = new StoredGroupByCollection();
				}
				return this.lstGroupBy;
			}
		}

		private StoredOrderByCollection lstOrderBy = null;
		/// <summary>
		/// Gets the order by.
		/// </summary>
		/// <value>The order by.</value>
		public StoredOrderByCollection OrderBys {
			get {
				if (this.lstOrderBy == null) {
					this.lstOrderBy = new StoredOrderByCollection();
				}
				return this.lstOrderBy;
			}
		}

        private StoredColumnCollectoin lstStoredColumn = null;
		/// <summary>
		/// Gets the Stored Columns.
		/// </summary>
		/// <value>The Stored Columns.</value>
		public StoredColumnCollectoin Columns {
			get {
				if (this.lstStoredColumn == null) {
					this.lstStoredColumn = new StoredColumnCollectoin();
				}
				return this.lstStoredColumn;
			}
		}

        private SetColumnValueCollection lstSetValues = null;
		/// <summary>
		/// Gets the set values.
		/// </summary>
		/// <value>The set values.</value>
		public SetColumnValueCollection SetValues {
			get {
				if (this.lstSetValues == null) {
                    if ((this.Type == QueryType.Update))
                    {
                        this.lstSetValues = new SetColumnValueCollection(SetColumnValue.SetValueType.Define);
                    }
                    else if ((this.Type == QueryType.Insert))
                    {
                        this.lstSetValues = new SetColumnValueCollection(SetColumnValue.SetValueType.Parts);
                    }
                    else if (this.Type == QueryType.Procedure)
                    {
                        this.lstSetValues = new SetColumnValueCollection(SetColumnValue.SetValueType.Parameter);
                    }
                    else
                    {
                        this.lstSetValues = new SetColumnValueCollection(SetColumnValue.SetValueType.Define);
                    }
				}
				return this.lstSetValues;
			}
		}

        private StoredUnionCollection lstUnion = null;
		/// <summary>
		/// Gets the unions.
		/// </summary>
		/// <value>The Union.</value>
		public StoredUnionCollection Unions {
			get {
				if (this.lstUnion == null) {
					this.lstUnion = new StoredUnionCollection();
				}
				return this.lstUnion;
			}
		}

        private Interval<int> itiLimitRows = null;
        public Interval<int> LimitRows
        {
            get
            {
                return itiLimitRows;
            }
            set
            {
                itiLimitRows = value;
            }
        }

		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="SQuery"/> class.
		/// </summary>
		/// <param name="type">The type.</param>
		internal Query(QueryType type, Provider provider)
		{
			this.Type = type;
			this.Provider = provider;
		}
        #endregion

		#region Select Methods

		#region "Other Select Methods"

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

		#endregion

        #region "From"

		/// <summary>
		/// Froms the specified table.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <returns></returns>
		public StoredTable From(TableSchema table)
		{
			return this.Add(StorageFactory.Table.Create(table, this));
		}

		/// <summary>
		/// Froms the specified entity.
		/// </summary>
		/// <typeparam name="TEtt">The type of the Entity.</typeparam>
		/// <returns></returns>
		public StoredTable From<TEtt>() where TEtt : EntityBase
		{
			return this.Add(StorageFactory.Table.Create<TEtt>(this));
		}

		/// <summary>
		/// Froms the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		public StoredTable From(EntityBase entity)
		{
			return this.Add(StorageFactory.Table.Create(entity, this));
		}

		/// <summary>
		/// Froms the specified table name.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <returns></returns>
		public StoredTable From(string tableName)
		{
			return this.Add(StorageFactory.Table.Create(tableName, this));
		}

		/// <summary>
		/// Froms the specified sub-Query.
		/// </summary>
		/// <param name="subQuery">Sub-Query.</param>
		/// <returns></returns>
		public StoredTable From(Query subQuery)
		{
			return this.Add(StorageFactory.Table.Create(subQuery, this));
		}

		/// <summary>
		/// Froms the specified Function.
		/// </summary>
		/// <param name="stFunction">Function.</param>
		/// <returns></returns>
		public StoredTable From(StoredFunction stFunction)
		{
			return this.Add(StorageFactory.Table.Create(stFunction, this));
		}

		#endregion

		#region "Join"

		/// <summary>
		/// Joins the specified table.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <returns></returns>
		public StoredJoin Join(TableSchema table)
		{
			return this.Add(StorageFactory.Join.Create(table, this));
		}

		/// <summary>
		/// Joins the specified entity.
		/// </summary>
		/// <typeparam name="TEtt">The type of the entity.</typeparam>
		/// <returns></returns>
		public StoredJoin Join<TEtt>() where TEtt : EntityBase
		{
			return this.Add(StorageFactory.Join.Create<TEtt>(this));
		}

		/// <summary>
		/// Joins the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		public StoredJoin Join(EntityBase entity)
		{
			return this.Add(StorageFactory.Join.Create(entity, this));
		}

		/// <summary>
		/// Joins the specified table name.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <returns></returns>
		public StoredJoin Join(string tableName)
		{
			return this.Add(StorageFactory.Join.Create(tableName, this));
		}

		/// <summary>
		/// Joins the specified sub-query.
		/// </summary>
		/// <param name="subQuery">The sub-query.</param>
		/// <returns></returns>
		public StoredJoin Join(Query subQuery)
		{
			return this.Add(StorageFactory.Join.Create(subQuery, this));
		}

		#endregion

		#region "Constraints"

		/// <summary>
		/// Where the column.
		/// </summary>
		/// <param name="column">The column.</param>
		/// <returns></returns>
		public StoredConstraint Where(ColumnSchema column)
		{
			if (this.Constraints.Count == 0) {
				return this.Add(StorageFactory.Constraint.CreateWhere(StorageFactory.Column.Create(column, this), this));
			} else {
				return this.Add(StorageFactory.Constraint.CreateAnd(StorageFactory.Column.Create(column, this), this));
			}
		}

		/// <summary>
		/// And the column.
		/// </summary>
		/// <param name="column">The column.</param>
		/// <returns></returns>
		public StoredConstraint And(ColumnSchema column)
		{
			if (this.Constraints.Count == 0) {
				return this.Add(StorageFactory.Constraint.CreateWhere(StorageFactory.Column.Create(column, this), this));
			} else {
				return this.Add(StorageFactory.Constraint.CreateAnd(StorageFactory.Column.Create(column, this), this));
			}
		}

		/// <summary>
		/// Or the column.
		/// </summary>
		/// <param name="column">The column.</param>
		/// <returns></returns>
		public StoredConstraint Or(ColumnSchema column)
		{
			return this.Add(StorageFactory.Constraint.CreateOr(StorageFactory.Column.Create(column, this), this));
		}

		/// <summary>
		/// Where the specified Column Name.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public StoredConstraint Where(TableSchema table, string columnName)
		{
			if (this.Constraints.Count == 0) {
				return this.Add(StorageFactory.Constraint.CreateWhere(StorageFactory.Column.Create(columnName, table, this), this));
			} else {
				return this.Add(StorageFactory.Constraint.CreateAnd(StorageFactory.Column.Create(columnName, table, this), this));
			}
		}

		/// <summary>
		/// And the specified Column Name.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public StoredConstraint And(TableSchema table, string columnName)
		{
			if (this.Constraints.Count == 0) {
				return this.Add(StorageFactory.Constraint.CreateWhere(StorageFactory.Column.Create(columnName, table, this), this));
			} else {
				return this.Add(StorageFactory.Constraint.CreateAnd(StorageFactory.Column.Create(columnName, table, this), this));
			}
		}

		/// <summary>
		/// Or the specified Column Name.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public StoredConstraint Or(TableSchema table, string columnName)
		{
			return this.Add(StorageFactory.Constraint.CreateOr(StorageFactory.Column.Create(columnName, table, this), this));
		}

		/// <summary>
		/// Where the specified Constant string value.
		/// </summary>
		/// <param name="columnName">Constant string value.</param>
		/// <returns></returns>
		public StoredConstraint Where(string columnName)
		{
			if (this.Constraints.Count == 0) {
				return this.Add(StorageFactory.Constraint.CreateWhere(StorageFactory.Column.Create(columnName, this), this));
			} else {
				return this.Add(StorageFactory.Constraint.CreateAnd(StorageFactory.Column.Create(columnName, this), this));
			}
		}

		/// <summary>
		/// And the specified Constant string value.
		/// </summary>
		/// <param name="columnName">Constant string value.</param>
		/// <returns></returns>
		public StoredConstraint And(string columnName)
		{
			if (this.Constraints.Count == 0) {
				return this.Add(StorageFactory.Constraint.CreateWhere(StorageFactory.Column.Create(columnName, this), this));
			} else {
				return this.Add(StorageFactory.Constraint.CreateAnd(StorageFactory.Column.Create(columnName, this), this));
			}
		}

		/// <summary>
		/// Or the specified Constant string value.
		/// </summary>
		/// <param name="columnName">Constant string value.</param>
		/// <returns></returns>
		public StoredConstraint Or(string columnName)
		{
			return this.Add(StorageFactory.Constraint.CreateOr(StorageFactory.Column.Create(columnName, this), this));
		}

		/// <summary>
		/// Where the specified Column Name.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public StoredConstraint Where(string tableName, string columnName)
		{
			if (this.Constraints.Count == 0) {
				return this.Add(StorageFactory.Constraint.CreateWhere(StorageFactory.Column.Create(columnName, tableName, this), this));
			} else {
				return this.Add(StorageFactory.Constraint.CreateAnd(StorageFactory.Column.Create(columnName, tableName, this), this));
			}
		}

		/// <summary>
		/// And the specified Column Name.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public StoredConstraint And(string tableName, string columnName)
		{
			if (this.Constraints.Count == 0) {
				return this.Add(StorageFactory.Constraint.CreateWhere(StorageFactory.Column.Create(columnName, tableName, this), this));
			} else {
				return this.Add(StorageFactory.Constraint.CreateAnd(StorageFactory.Column.Create(columnName, tableName, this), this));
			}
		}

		/// <summary>
		/// Or the specified Column Name.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public StoredConstraint Or(string tableName, string columnName)
		{
			return this.Add(StorageFactory.Constraint.CreateOr(StorageFactory.Column.Create(columnName, tableName, this), this));
		}

		/// <summary>
		/// Where the specified Column Name.
		/// </summary>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public StoredConstraint Where<TEtt>(string columnName) where TEtt : EntityBase
		{
			if (this.Constraints.Count == 0) {
				return this.Add(StorageFactory.Constraint.CreateWhere(StorageFactory.Column.Create<TEtt>(columnName, this), this));
			} else {
				return this.Add(StorageFactory.Constraint.CreateAnd(StorageFactory.Column.Create<TEtt>(columnName, this), this));
			}
		}

		/// <summary>
		/// And the specified Column Name.
		/// </summary>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public StoredConstraint And<TEtt>(string columnName) where TEtt : EntityBase
		{
			if (this.Constraints.Count == 0) {
				return this.Add(StorageFactory.Constraint.CreateWhere(StorageFactory.Column.Create<TEtt>(columnName, this), this));
			} else {
				return this.Add(StorageFactory.Constraint.CreateAnd(StorageFactory.Column.Create<TEtt>(columnName, this), this));
			}
		}

		/// <summary>
		/// Or the specified Column Name.
		/// </summary>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public StoredConstraint Or<TEtt>(string columnName) where TEtt : EntityBase
		{
			return this.Add(StorageFactory.Constraint.CreateOr(StorageFactory.Column.Create<TEtt>(columnName, this), this));
		}

		public Query CloseParentesis()
		{
			this.Add(StorageFactory.Constraint.CreateCloseParentesis(this));
			return this;
        }

        /// <summary>
        /// Where the column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public StoredConstraint Where(StoredFunction function)
        {
            if (this.Constraints.Count == 0)
            {
                return this.Add(StorageFactory.Constraint.CreateWhere(StorageFactory.Column.Create(function, this), this));
            }
            else
            {
                return this.Add(StorageFactory.Constraint.CreateAnd(StorageFactory.Column.Create(function, this), this));
            }
        }

        /// <summary>
        /// And the column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public StoredConstraint And(StoredFunction function)
        {
            if (this.Constraints.Count == 0)
            {
                return this.Add(StorageFactory.Constraint.CreateWhere(StorageFactory.Column.Create(function, this), this));
            }
            else
            {
                return this.Add(StorageFactory.Constraint.CreateAnd(StorageFactory.Column.Create(function, this), this));
            }
        }

        /// <summary>
        /// Or the column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public StoredConstraint Or(StoredFunction function)
        {
            return this.Add(StorageFactory.Constraint.CreateOr(StorageFactory.Column.Create(function, this), this));
        }

		#region "Open Parentesis"

		/// <summary>
		/// Open Parentesis
		/// </summary>
		/// <returns></returns>
		public Query OpenParentesis()
		{
			this.Add(StorageFactory.Constraint.CreateOpenParentesis(this));
			return this;
		}

		/// <summary>
		/// Where Open Parentesis
		/// </summary>
		/// <returns></returns>
		public Query WhereOpenParentesis()
		{
			this.Add(StorageFactory.Constraint.CreateWhereOpenParentesis(this));
			return this;
		}

		/// <summary>
		/// And Open Parentesis
		/// </summary>
		/// <returns></returns>
		public Query AndOpenParentesis()
		{
			this.Add(StorageFactory.Constraint.CreateAndOpenParentesis(this));
			return this;
		}

		/// <summary>
		/// Or Open Parentesis
		/// </summary>
		/// <returns></returns>
		public Query OrOpenParentesis()
		{
			this.Add(StorageFactory.Constraint.CreateOrOpenParentesis(this));
			return this;
		}

		/// <summary>
		/// Where the column.
		/// </summary>
		/// <param name="column">The column.</param>
		/// <returns></returns>
		public StoredConstraint WhereOpenParentesis(ColumnSchema column)
		{
			this.Add(StorageFactory.Constraint.CreateWhereOpenParentesis(this));
			return this.Where(column);
		}

		/// <summary>
		/// And the column.
		/// </summary>
		/// <param name="column">The column.</param>
		/// <returns></returns>
		public StoredConstraint AndOpenParentesis(ColumnSchema column)
		{
			this.Add(StorageFactory.Constraint.CreateAndOpenParentesis(this));
			return this.And(column);
		}

		/// <summary>
		/// Or the column.
		/// </summary>
		/// <param name="column">The column.</param>
		/// <returns></returns>
		public StoredConstraint OrOpenParentesis(ColumnSchema column)
		{
			this.Add(StorageFactory.Constraint.CreateOrOpenParentesis(this));
			return this.Or(column);
		}

		/// <summary>
		/// Where the specified Column Name.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public StoredConstraint WhereOpenParentesis(TableSchema table, string columnName)
		{
			this.Add(StorageFactory.Constraint.CreateWhereOpenParentesis(this));
			return this.Where(table, columnName);
		}

		/// <summary>
		/// And the specified Column Name.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public StoredConstraint AndOpenParentesis(TableSchema table, string columnName)
		{
			this.Add(StorageFactory.Constraint.CreateAndOpenParentesis(this));
			return this.And(table, columnName);
		}

		/// <summary>
		/// Or the specified Column Name.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public StoredConstraint OrOpenParentesis(TableSchema table, string columnName)
		{
			this.Add(StorageFactory.Constraint.CreateOrOpenParentesis(this));
			return this.Or(table, columnName);
		}

		/// <summary>
		/// Where the specified Column Name.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public StoredConstraint WhereOpenParentesis(string tableName, string columnName)
		{
			this.Add(StorageFactory.Constraint.CreateWhereOpenParentesis(this));
			return this.Where(tableName, columnName);
		}

		/// <summary>
		/// And the specified Column Name.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public StoredConstraint AndOpenParentesis(string tableName, string columnName)
		{
			this.Add(StorageFactory.Constraint.CreateAndOpenParentesis(this));
			return this.And(tableName, columnName);
		}

		/// <summary>
		/// Or the specified Column Name.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public StoredConstraint OrOpenParentesis(string tableName, string columnName)
		{
			this.Add(StorageFactory.Constraint.CreateOrOpenParentesis(this));
			return this.Or(tableName, columnName);
		}

		/// <summary>
		/// Where the specified Column Name.
		/// </summary>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public StoredConstraint WhereOpenParentesis<TEtt>(string columnName) where TEtt : EntityBase
		{
			this.Add(StorageFactory.Constraint.CreateWhereOpenParentesis(this));
			return this.Where<TEtt>(columnName);
		}

		/// <summary>
		/// And the specified Column Name.
		/// </summary>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public StoredConstraint AndOpenParentesis<TEtt>(string columnName) where TEtt : EntityBase
		{
			this.Add(StorageFactory.Constraint.CreateAndOpenParentesis(this));
			return this.And<TEtt>(columnName);
		}

		/// <summary>
		/// Or the specified Column Name.
		/// </summary>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public StoredConstraint OrOpenParentesis<TEtt>(string columnName) where TEtt : EntityBase
		{
			this.Add(StorageFactory.Constraint.CreateOrOpenParentesis(this));
			return this.Or<TEtt>(columnName);
		}

		#endregion

		#endregion

		#region "Order By"

		public Query OrderBy(params ColumnSchema[] columns)
		{
			foreach (ColumnSchema column in columns) {
				this.OrderBy(column);
			}
			return this;
		}

		public Query OrderBy(params string[] columns)
		{
			foreach (string column in columns) {
				this.OrderBy(column);
			}
			return this;
		}

		public Query OrderBy(TableSchema table, params ColumnSchema[] columns)
		{
			foreach (ColumnSchema column in columns) {
				this.OrderBy(table, column);
			}
			return this;
		}

		public Query OrderBy(TableSchema table, params string[] columns)
		{
			foreach (string column in columns) {
				this.OrderBy(table, column);
			}
			return this;
		}

		public Query OrderBy(string tableName, params string[] columns)
		{
			foreach (string column in columns) {
				this.OrderBy(tableName, column);
			}
			return this;
		}

		public Query OrderBy<TEtt>(params string[] columns) where TEtt : EntityBase
		{
			TableSchema table = SchemaContainer.GetSchema<TEtt>();
			foreach (string column in columns) {
				this.OrderBy(table, column);
			}
			return this;
		}

		public StoredOrderBy OrderBy(ColumnSchema column)
		{
			return this.Add(StorageFactory.OrderBy.Create(column, this));
		}

		public StoredOrderBy OrderBy(TableSchema table, string columnName)
		{
			return this.Add(StorageFactory.OrderBy.Create(columnName, table, this));
		}

		public StoredOrderBy OrderBy(string tableName, string columnName)
		{
			return this.Add(StorageFactory.OrderBy.Create(columnName, tableName, this));
		}

		public StoredOrderBy OrderBy<TEtt>(string columnName) where TEtt : EntityBase
		{
			TableSchema table = SchemaContainer.GetSchema<TEtt>();
			return this.Add(StorageFactory.OrderBy.Create(columnName, table, this));
		}

		public StoredOrderBy OrderBy(string columnName)
		{
			return this.Add(StorageFactory.OrderBy.Create(columnName, this));
		}

		#endregion

		#region "Group By"

		public Query GroupBy(params ColumnSchema[] columns)
		{
			foreach (ColumnSchema column in columns) {
				this.GroupBy(column);
			}
			return this;
		}

		public Query GroupBy(params string[] columns)
		{
			foreach (string column in columns) {
				this.GroupBy(column);
			}
			return this;
		}

		public Query GroupBy(TableSchema table, params ColumnSchema[] columns)
		{
			foreach (ColumnSchema column in columns) {
				this.GroupBy(table, column);
			}
			return this;
		}

		public Query GroupBy(TableSchema table, params string[] columns)
		{
			foreach (string column in columns) {
				this.GroupBy(table, column);
			}
			return this;
		}

		public Query GroupBy(string tableName, params string[] columns)
		{
			foreach (string column in columns) {
				this.GroupBy(tableName, column);
			}
			return this;
		}

		public Query GroupBy<TEtt>(params string[] columns) where TEtt : EntityBase
		{
			TableSchema table = SchemaContainer.GetSchema<TEtt>();
			foreach (string column in columns) {
				this.GroupBy(table, column);
			}
			return this;
		}

		public Query GroupBy(ColumnSchema column)
		{
			this.Add(StorageFactory.GroupBy.Create(column, this));
			return this;
		}

		public Query GroupBy(TableSchema table, string columnName)
		{
			this.Add(StorageFactory.GroupBy.Create(columnName, table, this));
			return this;
		}

		public Query GroupBy(string tableName, string columnName)
		{
			this.Add(StorageFactory.GroupBy.Create(columnName, tableName, this));
			return this;
		}

		public Query GroupBy<TEtt>(string columnName) where TEtt : EntityBase
		{
			TableSchema table = SchemaContainer.GetSchema<TEtt>();
			this.Add(StorageFactory.GroupBy.Create(columnName, table, this));
			return this;
		}

		public Query GroupBy(string columnName)
		{
			this.Add(StorageFactory.GroupBy.Create(columnName, this));
			return this;
		}

		#endregion

		#region "Union"

		public Query Union(Query query)
		{
			StoredUnion stUnion = new StoredUnion(this, false, query);
			this.Add(stUnion);
			return this;
		}

		public Query UnionAll(Query query)
		{
			StoredUnion stUnion = new StoredUnion(this, true, query);
			this.Add(stUnion);
			return this;
		}

		#endregion

		#region "Concat"

		public StoredConcat Concat()
		{
			StoredConcat retConcat = new StoredConcat(this);
			this.Add(retConcat);
			return retConcat;
		}

		public StoredConcat Concat(object objValue)
		{
			StoredConcat retConcat = new StoredConcat(this);
			this.Add(retConcat);
			return retConcat.AndValue(objValue);
		}

		public StoredConcat Concat(ColumnSchema column)
		{
			StoredConcat retConcat = new StoredConcat(this);
			this.Add(retConcat);
			return retConcat.AndValue(column);
		}

		public StoredConcat Concat(string columnName)
		{
			StoredConcat retConcat = new StoredConcat(this);
			this.Add(retConcat);
			return retConcat.AndValue(columnName);
		}

		public StoredConcat Concat(string columnName, TableSchema table)
		{
			StoredConcat retConcat = new StoredConcat(this);
			this.Add(retConcat);
			return retConcat.AndValue(columnName, table);
		}

		public StoredConcat Concat<TEtt>(string columnName) where TEtt : EntityBase
		{
			StoredConcat retConcat = new StoredConcat(this);
			this.Add(retConcat);
			return retConcat.AndValue<TEtt>(columnName);
		}

		public StoredConcat Concat(string tableName, string columnName)
		{
			StoredConcat retConcat = new StoredConcat(this);
			this.Add(retConcat);
			return retConcat.AndValue(tableName, columnName);
		}

		public StoredConcat Concat(StoredFunction stFunction)
		{
			StoredConcat retConcat = new StoredConcat(this);
			this.Add(retConcat);
			return retConcat.AndValue(stFunction);
		}

		#endregion

		#region "Column"

		public StoredColumn Column(ColumnSchema columnSchema)
		{
			StoredColumn retColumn = StorageFactory.Column.Create(columnSchema, this);
			return this.Add(retColumn);
		}

		public StoredColumn Column(string columnName)
		{
			StoredColumn retColumn = StorageFactory.Column.Create(columnName, this);
			return this.Add(retColumn);
		}

		public StoredColumn Column(string columnName, TableSchema table)
		{
			StoredColumn retColumn = StorageFactory.Column.Create(columnName, table, this);
			return this.Add(retColumn);
		}

		public StoredColumn Column<TEtt>(string columnName) where TEtt : EntityBase
		{
			StoredColumn retColumn = StorageFactory.Column.Create<TEtt>(columnName, this);
			return this.Add(retColumn);
		}

		public StoredColumn Column(string tableName, string columnName)
		{
			StoredColumn retColumn = StorageFactory.Column.Create(columnName, tableName, this);
			return this.Add(retColumn);
		}

		public StoredColumn Column(StoredFunction stFunction)
		{
			StoredColumn retColumn = StorageFactory.Column.Create(stFunction, this);
			return this.Add(retColumn);
		}

        public StoredColumn Column(StringConstant constant)
        {
            StoredColumn retColumn = StorageFactory.Column.Create(constant, this);
            return this.Add(retColumn);
        }

		#endregion

		#endregion

		#region "Insert Methods"

		#region "Into"

		/// <summary>
		/// Into the specified table.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <returns></returns>
		public Query Into(TableSchema table)
		{
			return Into(StorageFactory.Table.Create(table, this));
		}

		/// <summary>
		/// Into the specified table.
		/// </summary>
		/// <typeparam name="TEtt">The type of the Entity.</typeparam>
		/// <returns></returns>
		public Query Into<TEtt>() where TEtt : EntityBase
		{
			return Into(StorageFactory.Table.Create<TEtt>(this));
		}

		/// <summary>
		/// Into the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		public Query Into(EntityBase entity)
		{
			return Into(StorageFactory.Table.Create(entity, this));
		}

		/// <summary>
		/// Into the specified table name.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <returns></returns>
		public Query Into(string tableName)
		{
			return Into(StorageFactory.Table.Create(tableName, this));
		}

		private Query Into(StoredTable table)
		{
			this.IntoTable = table;

			if ((this.IntoTable.Type == StoredTable.StoredTypes.Schema)) {
				TableSchema tbSchema = (TableSchema)this.IntoTable.TableDefinition;
				if (this.Type == QueryType.Delete && tbSchema.IsLogicalExclusion) {
					this.Type = QueryType.Update;
					this.SetValue(tbSchema.LogicalExclusionColumn).Equal(true);
				}
			}
			return this;
		}

		#endregion

		#region "Set"

		/// <summary>
		/// Sets the specified column.
		/// </summary>
		/// <param name="columnName">The column.</param>
		/// <returns></returns>
		public SetColumnValue SetValue(string columnName)
		{
			return this.Add(StorageFactory.SetColumnValue.Create(columnName, this));
		}

		/// <summary>
		/// Sets the specified column.
		/// </summary>
		/// <param name="column">The column.</param>
		/// <returns></returns>
		public SetColumnValue SetValue(ColumnSchema column)
		{
			return this.Add(StorageFactory.SetColumnValue.Create(column, this));
		}

		/// <summary>
		/// Sets the specified column.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public SetColumnValue SetValue(TableSchema table, string columnName)
		{
			return this.Add(StorageFactory.SetColumnValue.Create(columnName, table, this));
		}

		/// <summary>
		/// Sets the specified column.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public SetColumnValue SetValue(string tableName, string columnName)
		{
			return this.Add(StorageFactory.SetColumnValue.Create(tableName, columnName, this));
		}

		/// <summary>
		/// Sets the specified column.
		/// </summary>
		/// <typeparam name="TEtt">The type of the ntity.</typeparam>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public SetColumnValue SetValue<TEtt>(string columnName) where TEtt : EntityBase
		{
			return this.Add(StorageFactory.SetColumnValue.Create<TEtt>(columnName, this));
		}

		#endregion

		#endregion

		#region Adds

		/// <summary>
		/// Adds the specified table.
		/// </summary>
		/// <param name="item">The table.</param>
		/// <returns></returns>
		protected StoredTable Add(StoredTable item)
		{
			if ((item.Type == StoredTable.StoredTypes.Schema)) {
				TableSchema schema = (TableSchema)item.TableDefinition;
				if ((schema.IsLogicalExclusion)) {
					this.Where(schema.LogicalExclusionColumn).Equal(0);
				}
			}
			return this.Tables.Add(item);
		}

		/// <summary>
		/// Adds the specified join.
		/// </summary>
		/// <param name="item">The join.</param>
		/// <returns></returns>
		protected StoredJoin Add(StoredJoin item)
		{
			return this.Joins.Add(item);
		}

		/// <summary>
		/// Adds the specified Constraint.
		/// </summary>
		/// <param name="item">The Constraint.</param>
		/// <returns></returns>
		protected StoredConstraint Add(StoredConstraint item)
		{
			return this.Constraints.Add(item);
		}

		/// <summary>
		/// Adds the specified Group By.
		/// </summary>
		/// <param name="item">The Group By.</param>
		/// <returns></returns>
		protected StoredGroupBy Add(StoredGroupBy item)
		{
			return this.GroupBys.Add(item);
		}

		/// <summary>
		/// Adds the specified Order By.
		/// </summary>
		/// <param name="item">The Order By.</param>
		/// <returns></returns>
		protected StoredOrderBy Add(StoredOrderBy item)
		{
			return this.OrderBys.Add(item);
		}

		/// <summary>
		/// Adds the specified Concat.
		/// </summary>
		/// <param name="item">The Concat.</param>
		/// <returns></returns>
		protected StoredConcat Add(StoredConcat item)
		{
			this.Columns.Add(StorageFactory.Column.Create(item, this));
			return item;
		}

		/// <summary>
		/// Adds the specified Set Value.
		/// </summary>
		/// <param name="item">The Set Value.</param>
		/// <returns></returns>
		protected SetColumnValue Add(SetColumnValue item)
		{
			return this.SetValues.Add(item);
		}

		/// <summary>
		/// Adds the specified StringColumn.
		/// </summary>
		/// <param name="item">The StringColumn.</param>
		/// <returns></returns>
		protected StoredColumn Add(StoredColumn item)
		{
			return this.Columns.Add(item);
		}

		/// <summary>
		/// Adds the specified Union Query.
		/// </summary>
		/// <param name="item">The Union Query.</param>
		/// <returns></returns>
		protected StoredUnion Add(StoredUnion item)
		{
			return this.Unions.Add(item);
		}

		#endregion

        #region Functions

        #region Count

        public FunctionCount Count(ColumnSchema columnSchema)
        {
            FunctionCount function = StorageFactory.SFunction.Count.Create(this);
            function.SetValue(columnSchema);
            this.Column(function);
            return function;
        }

        public FunctionCount Count(string columnName)
        {
            FunctionCount function = StorageFactory.SFunction.Count.Create(this);
            function.SetValue(columnName);
            this.Column(function);
            return function;
        }

        public FunctionCount Count(string tableName, string columnName)
        {
            FunctionCount function = StorageFactory.SFunction.Count.Create(this);
            function.SetValue(tableName, columnName);
            this.Column(function);
            return function;
        }

        public FunctionCount Count(TableSchema tableSchema, string columnName)
        {
            FunctionCount function = StorageFactory.SFunction.Count.Create(this);
            function.SetValue(columnName, tableSchema);
            this.Column(function);
            return function;
        }

        public FunctionCount Count<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            FunctionCount function = StorageFactory.SFunction.Count.Create(this);
            function.SetValue<TEtt>(columnName);
            this.Column(function);
            return function;
        }

        public FunctionCount Count()
        {
            FunctionCount function = StorageFactory.SFunction.Count.Create(this);
            function.SetValue(new StringConstant("*"));
            this.Column(function);
            return function;
        }

        #endregion

        #region Max

        public FunctionMax Max(ColumnSchema columnSchema)
        {
            FunctionMax function = StorageFactory.SFunction.Max.Create(this);
            function.SetValue(columnSchema);
            this.Column(function);
            return function;
        }

        public FunctionMax Max(string columnName)
        {
            FunctionMax function = StorageFactory.SFunction.Max.Create(this);
            function.SetValue(columnName);
            this.Column(function);
            return function;
        }

        public FunctionMax Max(string tableName, string columnName)
        {
            FunctionMax function = StorageFactory.SFunction.Max.Create(this);
            function.SetValue(columnName, tableName);
            this.Column(function);
            return function;
        }

        public FunctionMax Max(TableSchema tableSchema, string columnName)
        {
            FunctionMax function = StorageFactory.SFunction.Max.Create(this);
            function.SetValue(columnName, tableSchema);
            this.Column(function);
            return function;
        }

        public FunctionMax Max<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            FunctionMax function = StorageFactory.SFunction.Max.Create(this);
            function.SetValue<TEtt>(columnName);
            this.Column(function);
            return function;
        }

        #endregion

        #region Min

        public FunctionMin Min(ColumnSchema columnSchema)
        {
            FunctionMin function = StorageFactory.SFunction.Min.Create(this);
            function.SetValue(columnSchema);
            this.Column(function);
            return function;
        }

        public FunctionMin Min(string columnName)
        {
            FunctionMin function = StorageFactory.SFunction.Min.Create(this);
            function.SetValue(columnName);
            this.Column(function);
            return function;
        }

        public FunctionMin Min(string tableName, string columnName)
        {
            FunctionMin function = StorageFactory.SFunction.Min.Create(this);
            function.SetValue(columnName, tableName);
            this.Column(function);
            return function;
        }

        public FunctionMin Min(TableSchema tableSchema, string columnName)
        {
            FunctionMin function = StorageFactory.SFunction.Min.Create(this);
            function.SetValue(columnName, tableSchema);
            this.Column(function);
            return function;
        }

        public FunctionMin Min<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            FunctionMin function = StorageFactory.SFunction.Min.Create(this);
            function.SetValue<TEtt>(columnName);
            this.Column(function);
            return function;
        }

        #endregion

        #region Avg

        public FunctionAvg Avg(ColumnSchema columnSchema)
        {
            FunctionAvg function = StorageFactory.SFunction.Avg.Create(this);
            function.SetValue(columnSchema);
            this.Column(function);
            return function;
        }

        public FunctionAvg Avg(string columnName)
        {
            FunctionAvg function = StorageFactory.SFunction.Avg.Create(this);
            function.SetValue(columnName);
            this.Column(function);
            return function;
        }

        public FunctionAvg Avg(string tableName, string columnName)
        {
            FunctionAvg function = StorageFactory.SFunction.Avg.Create(this);
            function.SetValue(columnName, tableName);
            this.Column(function);
            return function;
        }

        public FunctionAvg Avg(TableSchema tableSchema, string columnName)
        {
            FunctionAvg function = StorageFactory.SFunction.Avg.Create(this);
            function.SetValue(columnName, tableSchema);
            this.Column(function);
            return function;
        }

        public FunctionAvg Avg<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            FunctionAvg function = StorageFactory.SFunction.Avg.Create(this);
            function.SetValue<TEtt>(columnName);
            this.Column(function);
            return function;
        }

        #endregion

        #region Sum

        public FunctionSum Sum(ColumnSchema columnSchema)
        {
            FunctionSum function = StorageFactory.SFunction.Sum.Create(this);
            function.SetValue(columnSchema);
            this.Column(function);
            return function;
        }

        public FunctionSum Sum(string columnName)
        {
            FunctionSum function = StorageFactory.SFunction.Sum.Create(this);
            function.SetValue(columnName);
            this.Column(function);
            return function;
        }

        public FunctionSum Sum(string tableName, string columnName)
        {
            FunctionSum function = StorageFactory.SFunction.Sum.Create(this);
            function.SetValue(columnName, tableName);
            this.Column(function);
            return function;
        }

        public FunctionSum Sum(TableSchema tableSchema, string columnName)
        {
            FunctionSum function = StorageFactory.SFunction.Sum.Create(this);
            function.SetValue(columnName, tableSchema);
            this.Column(function);
            return function;
        }

        public FunctionSum Sum<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            FunctionSum function = StorageFactory.SFunction.Sum.Create(this);
            function.SetValue<TEtt>(columnName);
            this.Column(function);
            return function;
        }

        #endregion

        #region Call

        public SequenciaParamStoredFunction Call(string functionName)
        {
            return StorageFactory.SFunction.SequenciaParam.Create(functionName, this);
        }

        public SequenciaParamStoredFunction Call(string owner, string functionName)
        {
            return StorageFactory.SFunction.SequenciaParam.Create(functionName, owner, this);
        }

        public SequenciaParamStoredFunction ColumnCall(string functionName)
        {
            SequenciaParamStoredFunction function = this.Call(functionName);
            this.Column(function);
            return function;
        }

        public SequenciaParamStoredFunction ColumnCall(string owner, string functionName)
        {
            SequenciaParamStoredFunction function = this.Call(owner, functionName);
            this.Column(function);
            return function;
        }

        #endregion

        #endregion

        #region Limit

        public Query Limit(int startRow, int endRow)
        {
            return this.Limit(new Interval<int>(startRow, endRow));
        }

        public Query Limit(Interval<int> rowsInterval)
        {
            this.LimitRows = rowsInterval;
            return this;
        }

        #endregion

        public Query GetQuery()
		{
			return this;
		}

        public void SetQuery(Query query)
		{
		}

        public enum QueryType
		{
			Select,
			Insert,
			Delete,
			Update,
			Procedure,
			Function
		}

        private bool isCompiled = false;
        public void Compile()
        {
            if (!isCompiled)
            {
                QueryCompilerFusion compiler = new QueryCompilerFusion(this);
                compiler.Compile();
                isCompiled = true;
            }
        }

        internal OrmCommand CreateCommand(OrmTransaction transaction)
        {
            NQuery.Builder.QueryBuilder builder = this.Provider.QueryBuilder;
            return transaction == null ? builder.GetCommand(this) : builder.GetCommand(this, transaction);
        }

        public Executer Execute()
        {
            return new Executer(this);
        }

        public Executer Execute(OrmTransaction transaction)
        {
            return new Executer(this, transaction);
        }

        public StoredTable GetTableByAlias(string alias)
        {
            foreach (StoredTable tb in this.Tables)
            {
                if (tb.Alias != null && tb.Alias.Equals(alias))
                    return tb;
            }

            if (this.IntoTable != null && this.IntoTable.Alias != null && this.IntoTable.Alias.Equals(alias))
                return this.IntoTable;

            foreach (StoredTable tb in this.Joins)
            {
                if (tb.Alias != null && tb.Alias.Equals(alias))
                    return tb;
            }

            return null;
        }
    }

}
