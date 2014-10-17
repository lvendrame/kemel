using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.NQuery.Storage.Table;
using Kemel.Orm.NQuery.Storage.Column;
using Kemel.Orm.Schema;
using Kemel.Orm.NQuery.Storage.Join;
using Kemel.Orm.NQuery.Storage.StoredSelect;
using Kemel.Orm.NQuery.Storage.Value;
using Kemel.Orm.NQuery.Storage.Constraint;
using Kemel.Orm.NQuery.Storage.Function;
using Kemel.Orm.Base;

namespace Kemel.Orm.NQuery.Storage
{
	class QueryCompiler
	{

		private Query bruteQuery;
		private Query parentQuery;
		private Query returnQuery;

		public QueryCompiler(Query query)
		{
			bruteQuery = query;
		}

        public QueryCompiler(Query query, Query parent)
		{
			bruteQuery = query;
			parentQuery = parent;
		}

		public Query Compile()
		{
			returnQuery = new Query(bruteQuery.Type, bruteQuery.Provider);
			returnQuery.Parent = parentQuery;

			this.CopyAttributes();
			this.CopyTables();
			this.CopyColumns();
			this.CopyConstraints();
			this.CopyOrderBys();
			this.CopyGroupBys();
			this.CopyUnions();
            this.CopySetValues();

			return returnQuery;
		}

        private void CopyTables()
		{
			List<StoredTable> lstSQTables = new List<StoredTable>();
			List<StoredJoin> lstSQJoins = new List<StoredJoin>();

			returnQuery.IntoTable = bruteQuery.IntoTable;
			if ((returnQuery.IntoTable != null)) {
				returnQuery.IntoTable.SetQuery(returnQuery);
				if (returnQuery.IntoTable.Type == StoredTable.StoredTypes.SubQuery) {
					lstSQTables.Add(returnQuery.IntoTable);
				}
			}

			foreach (StoredTable table in bruteQuery.Tables) {
				if (table.Type == StoredTable.StoredTypes.SubQuery) {
					lstSQTables.Add(table);
				}
				table.SetQuery(returnQuery);
				returnQuery.Tables.Add(table);
			}

			foreach (StoredJoin @join in bruteQuery.Joins) {
				if (@join.Type == StoredTable.StoredTypes.SubQuery) {
					lstSQJoins.Add(@join);
				}
				returnQuery.Joins.Add(@join);
				this.VerifyJoin(@join);
			}

			foreach (StoredTable subQueryTable in lstSQTables) {
				TableSubQueryDefinition tbdSubQuery = (TableSubQueryDefinition)subQueryTable.TableDefinition;
				QueryCompiler cmp = new QueryCompiler(tbdSubQuery.SubQuery, this.returnQuery);
				tbdSubQuery.SubQuery = cmp.Compile();
			}

			foreach (StoredJoin subQueryJoin in lstSQJoins) {
				TableSubQueryDefinition tbdSubQuery = (TableSubQueryDefinition)subQueryJoin.TableDefinition;
				QueryCompiler cmp = new QueryCompiler(tbdSubQuery.SubQuery, this.returnQuery);
				tbdSubQuery.SubQuery = cmp.Compile();
			}

		}

        private void CopyAttributes()
		{
			returnQuery.Index = bruteQuery.Index;
			returnQuery.IsAutoNoLock = bruteQuery.IsAutoNoLock;
			returnQuery.IsDistinct = bruteQuery.IsDistinct;
			returnQuery.Level = bruteQuery.Level;
			returnQuery.TopRecords = bruteQuery.TopRecords;
		}

        private void CopyColumns()
		{
			foreach (StoredColumn column in bruteQuery.Columns) {
				this.VerifyColumn(column);
				returnQuery.Columns.Add(column);
			}

			if ((returnQuery.IntoTable != null)) {
				foreach (StoredColumn column in returnQuery.IntoTable.StoredColumns) {
					column.SetQuery(returnQuery);
					this.VerifyColumn(column, returnQuery.IntoTable);
					returnQuery.Columns.Add(column);
				}
				returnQuery.IntoTable.StoredColumns.Clear();
			}

			foreach (StoredTable table in returnQuery.Tables) {
				foreach (StoredColumn column in table.StoredColumns) {
					column.SetQuery(returnQuery);
					this.VerifyColumn(column, table);
					returnQuery.Columns.Add(column);
				}
				table.StoredColumns.Clear();
			}

			foreach (StoredJoin join in returnQuery.Joins) {
				foreach (StoredColumn column in join.StoredColumns) {
					column.SetQuery(returnQuery);
					this.VerifyColumn(column, join);
					returnQuery.Columns.Add(column);
				}
				@join.StoredColumns.Clear();
			}

		}

        private void VerifyConcat(StoredConcat concat)
		{
			foreach (StoredValue stValue in concat.Values) {
				this.VerifyValue(stValue);
			}
			concat.SetQuery(returnQuery);
		}

        private void CopyConstraints()
		{
			bool nextTypeIsNone = false;
			foreach (StoredConstraint constraint in bruteQuery.Constraints) {
				if ((nextTypeIsNone)) {
					nextTypeIsNone = false;
					constraint.Type = NQuery.Storage.Constraint.ConstraintBase.ConstraintType.None;
				}
				if ((constraint.Comparison == ComparisonOperator.OpenParentheses)) {
					nextTypeIsNone = true;
				}
				constraint.SetQuery(returnQuery);
				this.VerifyColumn(constraint.Column);
                if (constraint.Value != null)
				    this.VerifyValue(constraint.Value);
				returnQuery.Constraints.Add(constraint);
			}
		}

        private void CopyOrderBys()
		{
			foreach (StoredOrderBy orderBy in bruteQuery.OrderBys) {
				this.VerifyColumn(orderBy);
				returnQuery.OrderBys.Add(orderBy);
			}
		}

		private void CopyGroupBys()
		{
			foreach (StoredGroupBy groupBy in bruteQuery.GroupBys) {
				this.VerifyColumn(groupBy);
				returnQuery.GroupBys.Add(groupBy);
			}
		}

		private void CopyUnions()
		{
			foreach (StoredUnion union in bruteQuery.Unions) {
				union.SetQuery(returnQuery);
				QueryCompiler compiler = new QueryCompiler(union.UnionQuery, returnQuery);
				union.UnionQuery = compiler.Compile();
				returnQuery.Unions.Add(union);
			}
		}

        private void CopySetValues()
		{
            if (bruteQuery.SetValues == null)
                return;

			foreach (SetColumnValue columnValue in bruteQuery.SetValues) {
                columnValue.SetQuery(returnQuery);
                returnQuery.SetValues.Add(columnValue);
			}
		}

		private void VerifyValue(StoredValue stValue)
		{
			this.VerifyValue(stValue, null);
		}

        private void VerifyValue(StoredValue stValue, StoredTable stTable)
		{
			if ((stValue.Type == StoredValue.StoredTypes.IntervalStoredColumns)) {
				ValueStoredColumnIntervalDefinition iscDef = (ValueStoredColumnIntervalDefinition)stValue.Value;
				this.VerifyColumn(iscDef.StartValue, stTable);
				this.VerifyColumn(iscDef.FinalValue, stTable);
			} else if ((stValue.Type == StoredValue.StoredTypes.StoredColumn)) {
				ValueStoredColumnDefinition scDef = (ValueStoredColumnDefinition)stValue.Value;
				this.VerifyColumn(scDef.Value, stTable);
			} else if ((stValue.Type == StoredValue.StoredTypes.SubQuery)) {
				ValueSubQueryDefinition ssqDef = (ValueSubQueryDefinition)stValue.Value;
				QueryCompiler cmp = new QueryCompiler(ssqDef.Value, this.returnQuery);
				ssqDef.Value = cmp.Compile();
                ssqDef.Value.Level = this.returnQuery.NextLevel;
			} else if ((stValue.Type == StoredValue.StoredTypes.StoredFunction)) {
				ValueStoredFunctionDefinition sfDef = (ValueStoredFunctionDefinition)stValue.Value;
				this.VerifyFunction(sfDef.Value);
			} else if ((stValue.Type == StoredValue.StoredTypes.IntervalStoredFunction)) {
				ValueStoredFunctionIntervalDefinition sfDef = (ValueStoredFunctionIntervalDefinition)stValue.Value;
				this.VerifyFunction(sfDef.StartValue);
				this.VerifyFunction(sfDef.FinalValue);
			}
		}

		private void VerifyColumn(StoredColumn column)
		{
			VerifyColumn(column, null);
		}

        private void VerifyColumn(StoredColumn column, StoredTable stTable)
		{
            if (column == null)
                return;

			column.SetQuery(returnQuery);

			string tableName = string.Empty;
			if ((column.Table != null)) {
				tableName = column.Table.Alias;
                if ((string.IsNullOrEmpty(tableName)))
                {
                    tableName = column.Table.TableDefinition.Name;
                }
			} else if ((column.Type == StoredColumn.StoredTypes.Schema)) {
				tableName = ((ColumnSchema)column.ColumnDefinition).Parent.Name;
			}

			if ((!string.IsNullOrEmpty(tableName))) {
				StoredTable table = null;

				if ((stTable != null && stTable.Compare(tableName))) {
					table = stTable;
				} else {
					table = this.FindTable(tableName);
				}

				if ((table == null)) {
					throw new OrmException(string.Format("A tabela {0}, requisitada na coluna {1}, não foi encontrada.", tableName, column.ColumnDefinition.Name));
				}
				column.Table = table;
			}
			if ((column.Type == StoredColumn.StoredTypes.StoredFunction)) {
				ColumnFunctionDefinition stcFunction = (ColumnFunctionDefinition)column.ColumnDefinition;
				this.VerifyFunction(stcFunction.StoredFunction);
			} else if ((column.Type == StoredColumn.StoredTypes.Concat)) {
				ColumnConcatDefinition stcConcat = (ColumnConcatDefinition)column.ColumnDefinition;
				this.VerifyConcat(stcConcat.Concat);
			}
		}

        private void VerifyJoin(StoredJoin join)
		{
			join.SetQuery(returnQuery);
			bool nextTypeIsNone = false;
			foreach (StoredJoinConstraint condition in join.Conditions) {
				if ((nextTypeIsNone)) {
					nextTypeIsNone = false;
					condition.Type = ConstraintBase.ConstraintType.None;
				}
				if ((condition.Comparison == ComparisonOperator.OpenParentheses)) {
					nextTypeIsNone = true;
				}

				condition.SetQuery(returnQuery);
				this.VerifyColumn(condition.Column, join);
				this.VerifyValue(condition.Value, join);
			}
		}

		private void VerifyFunction(StoredFunction storedFunction)
		{
			storedFunction.SetQuery(returnQuery);
			this.VerifySetColumnValueCollection(storedFunction.SetValues);
		}

        private void VerifySetColumnValueCollection(IEnumerable<SetColumnValue> setColumnValueCollection)
		{
			foreach (SetColumnValue setValue in setColumnValueCollection) {
				setValue.SetQuery(returnQuery);
				this.VerifyColumn(setValue);
				this.VerifyValue(setValue.Value);
			}
		}

		#region "Finds"
		private StoredTable FindTable(string tableName)
		{
			return this.FindTable(tableName, this.returnQuery);
		}

        private StoredTable FindTable(string tableName, Query returnQuery)
		{

			if ((returnQuery == null)) {
				return null;
			}

			tableName = tableName.ToUpper();

			if ((returnQuery.IntoTable != null)) {
				if ((returnQuery.IntoTable.Compare(tableName))) {
					return returnQuery.IntoTable;
				}
			}

			foreach (StoredTable table in returnQuery.Tables) {
				if ((table.Compare(tableName))) {
					return table;
				}
			}

			foreach (StoredTable table in returnQuery.Joins) {
				if ((table.Compare(tableName))) {
					return table;
				}
			}

			return FindTable(tableName, parentQuery);
		}

        private StoredColumn FindColumn(string columnName)
		{
			return this.FindColumn(columnName, this.returnQuery);
		}

        private StoredColumn FindColumn(string columnName, Query returnQuery)
		{

			if ((returnQuery == null)) {
				return null;
			}

			columnName = columnName.ToUpper();

			StoredColumn retColumn = null;

			if ((returnQuery.IntoTable != null)) {
				retColumn = FindColumn(columnName, returnQuery.IntoTable, returnQuery);
				if ((retColumn != null)) {
					return retColumn;
				}
			}

			foreach (StoredTable table in returnQuery.Tables) {
				retColumn = FindColumn(columnName, table, returnQuery);
				if ((retColumn != null)) {
					return retColumn;
				}
			}

			foreach (StoredTable table in returnQuery.Joins) {
				retColumn = FindColumn(columnName, table, returnQuery);
				if ((retColumn != null)) {
					return retColumn;
				}
			}

			return FindColumn(columnName, parentQuery);
		}

        private StoredColumn FindColumn(string columnName, StoredTable storedTable, Query query)
		{
			StoredColumn retColumn = null;

			retColumn = storedTable.FindColumn(columnName);
			if ((retColumn != null)) {
				return retColumn;
			}

			if ((storedTable.Type == Table.StoredTable.StoredTypes.Schema)) {
				IColumnDefinition columnDef = storedTable.TableDefinition.GetColumn(columnName);
				if ((columnDef != null)) {
					return StorageFactory.Column.Create((ColumnSchema)columnDef, storedTable, query);
				}
			}

			return null;
		}

		private StoredColumn FindColumn(string tableName, string columnName)
		{
			return this.FindColumn(tableName, columnName, this.returnQuery);
		}

        private StoredColumn FindColumn(string tableName, string columnName, Query query)
		{

			StoredTable table = this.FindTable(tableName);

			if ((table == null)) {
				return null;
			}

			StoredColumn column = table.FindColumn(columnName);
			if ((column != null)) {
				return column;
			}

			if ((table.Type == StoredTable.StoredTypes.Schema)) {
				IColumnDefinition columnDef = table.TableDefinition.GetColumn(columnName);
				if ((columnDef != null)) {
					return StorageFactory.Column.Create((ColumnSchema)columnDef, table, query);
				}
			} else if ((table.Type == StoredTable.StoredTypes.Name)) {
				column = StorageFactory.Column.Create(columnName, table, query);
				table.TableDefinition.AddColumn(column.ColumnDefinition);
				return column;
			}

			return null;
		}
		#endregion

    }
}
