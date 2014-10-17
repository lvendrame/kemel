using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.Schema;
using Kemel.Orm.Entity;
using Kemel.Orm.NQuery.Storage.Table;
using Kemel.Orm.NQuery.Storage.Column;
using Kemel.Orm.NQuery.Storage.Value;
using Kemel.Orm.NQuery.Storage.Join;
using Kemel.Orm.NQuery.Storage.Function;
using Kemel.Orm.NQuery.Storage.Constraint;
using Kemel.Orm.NQuery.Storage.StoredSelect;
using Kemel.Orm.Base;
using Kemel.Orm.Providers;
using Kemel.Orm.NQuery.Storage.Function.Aggregated;
using Kemel.Orm.NQuery.Storage.Function.Scalar;
using Kemel.Orm.NQuery.Storage.StoredSelect.Case;

namespace Kemel.Orm.NQuery.Storage
{

    public class StorageFactory
	{

        public class Column
		{

            public static StoredColumn Create(Schema.ColumnSchema columnSchema, Query query)
			{
				return Create(columnSchema, Table.Create(columnSchema.Parent, query), query);
			}

			public static StoredColumn Create(Schema.ColumnSchema columnSchema, StoredTable table, Query query)
			{
				return new StoredColumn(columnSchema, StoredColumn.StoredTypes.Schema, table, query);
			}

			public static StoredColumn Create(string name, StoredTable table, Query query)
			{
				if ((table.Type == StoredTable.StoredTypes.Schema)) {
					IColumnDefinition colDef = table.TableDefinition.GetColumn(name);
					return new StoredColumn(colDef, StoredColumn.StoredTypes.Schema, table, query);
				} else {
					return new StoredColumn(new ColumnNameDefinition(name, table.TableDefinition), StoredColumn.StoredTypes.Name, table, query);
				}
			}

            static internal StoredColumn Create(StoredTable table, string nickName, Query query)
			{
				return new StoredColumn(new ColumnAliasDefinition(nickName, table.TableDefinition), StoredColumn.StoredTypes.OnlyAlias, table, query);
			}

			public static StoredColumn Create(string name, Query query)
			{
				return new StoredColumn(new ColumnUnknowTableDefinition(name), StoredColumn.StoredTypes.UnknowTable, query);
			}

			public static StoredColumn Create(Schema.ColumnSchema columnSchema, string tableName, Query query)
			{
                StoredTable storedTable = query.GetTableByAlias(tableName);
                if(storedTable == null)
                    storedTable = Table.Create(tableName, query);

				return Create(columnSchema, storedTable, query);
			}

			public static StoredColumn Create(string name, string tableName, Query query)
			{
				StoredTable storedTable = Table.Create(tableName, query);
				return Create(name, storedTable, query);
			}

			public static StoredColumn Create(string name, TableSchema tableSchema, Query query)
			{
				StoredTable storedTable = Table.Create(tableSchema, query);
				return Create(name, storedTable, query);
			}

            public static StoredColumn Create(string name, EntityBase entity, Query query)
			{
				StoredTable storedTable = Table.Create(entity, query);
				return Create(name, storedTable, query);
			}

			public static StoredColumn Create<TEtt>(string name, Query query) where TEtt : EntityBase
			{
				StoredTable storedTable = Table.Create<TEtt>(query);
				return Create(name, storedTable, query);
            }

            public static StoredColumn Create(StringConstant constant, Query query)
            {
                return new StoredColumn(new ColumnConstantDefinition(constant), StoredColumn.StoredTypes.StringConstant, query);
            }

			static internal StoredColumn Create(StoredFunction stFunction, StoredTable table, Query query)
			{
				return new StoredColumn(new ColumnFunctionDefinition(stFunction, table.TableDefinition), StoredColumn.StoredTypes.StoredFunction, table, query);
			}

			public static StoredColumn Create(StoredFunction stFunction, Query query)
			{
				return new StoredColumn(new ColumnFunctionDefinition(stFunction), StoredColumn.StoredTypes.StoredFunction, query);
			}

            static internal StoredColumn CreateNick(string tableName, string nickName, Query query)
			{
				StoredTable storedTable = Table.Create(tableName, query);
				return Create(storedTable, nickName, query);
			}

			public static StoredColumn Create(StoredConcat concat, Query query)
			{
				return new StoredColumn(new ColumnConcatDefinition(concat), StoredColumn.StoredTypes.Concat, query);
			}

        }

        public class JoinColumn
		{

			public static StoredJoinColumn Create(Schema.ColumnSchema columnSchema, Query query)
			{
				return new StoredJoinColumn(columnSchema, StoredColumn.StoredTypes.Schema, query);
			}

			public static StoredJoinColumn Create(Schema.ColumnSchema columnSchema, StoredTable table, Query query)
			{
				return new StoredJoinColumn(columnSchema, StoredColumn.StoredTypes.Schema, table, query);
			}

			public static StoredJoinColumn Create(string name, StoredTable table, Query query)
			{
				if ((table.Type == StoredTable.StoredTypes.Schema)) {
					IColumnDefinition colDef = table.TableDefinition.GetColumn(name);
					return new StoredJoinColumn(colDef, StoredColumn.StoredTypes.Schema, table, query);
				} else {
					return new StoredJoinColumn(new ColumnNameDefinition(name, table.TableDefinition), StoredColumn.StoredTypes.Name, table, query);
				}
			}

			static internal StoredJoinColumn Create(StoredTable table, string nickName, Query query)
			{
				return new StoredJoinColumn(new ColumnAliasDefinition(nickName, table.TableDefinition), StoredColumn.StoredTypes.OnlyAlias, table, query);
			}

			public static StoredJoinColumn Create(string name, Query query)
			{
				return new StoredJoinColumn(new ColumnUnknowTableDefinition(name), StoredColumn.StoredTypes.UnknowTable, query);
			}

			static internal StoredJoinColumn Create(StoredFunction stFunction, StoredTable table, Query query)
			{
				return new StoredJoinColumn(new ColumnFunctionDefinition(stFunction, table.TableDefinition), StoredColumn.StoredTypes.StoredFunction, table, query);
			}

			public static StoredJoinColumn Create(StoredFunction stFunction, Query query)
			{
				return new StoredJoinColumn(new ColumnFunctionDefinition(stFunction), StoredColumn.StoredTypes.StoredFunction, query);
			}

		}

        public class Table
		{

			public static StoredTable Create(TableSchema table, Query parent)
			{
				return new StoredTable(table, StoredTable.StoredTypes.Schema, parent);
			}

			public static StoredTable Create<TEtt>(Query parent) where TEtt : EntityBase
			{
				TableSchema table = SchemaContainer.GetSchema<TEtt>();
				return Create(table, parent);
			}

			public static StoredTable Create(EntityBase entity, Query parent)
			{
				TableSchema table = SchemaContainer.GetSchema(entity);
				return Create(table, parent);
			}

			public static StoredTable Create(string tableName, Query parent)
			{
				TableSchema table = SchemaContainer.GetSchema(tableName);
				if (table != null) {
					return Create(table, parent);
				} else {
					return new StoredTable(new TableNameDefinition(tableName), StoredTable.StoredTypes.Name, parent);
				}
			}

			public static StoredTable Create(Query subQuery, Query parent)
			{
				subQuery.Parent = parent;
				return new StoredTable(new TableSubQueryDefinition(subQuery), StoredTable.StoredTypes.SubQuery, parent);
			}

			public static StoredTable Create(StoredFunction stFunction, Query parent)
			{
				return new StoredTable(new TableFunctionDefinition(stFunction), StoredTable.StoredTypes.SubQuery, parent);
			}

		}

        public class Join
		{

			public static StoredJoin Create(TableSchema table, Query parent)
			{
				return new StoredJoin(table, StoredTable.StoredTypes.Schema, parent);
			}

			public static StoredJoin Create<TEtt>(Query parent) where TEtt : EntityBase
			{
				TableSchema table = SchemaContainer.GetSchema<TEtt>();
				return Create(table, parent);
			}

			public static StoredJoin Create(EntityBase entity, Query parent)
			{
				TableSchema table = SchemaContainer.GetSchema(entity);
				return Create(table, parent);
			}

			public static StoredJoin Create(string tableName, Query parent)
			{
				TableSchema table = SchemaContainer.GetSchema(tableName);
				if (table != null) {
					return Create(table, parent);
				} else {
					return new StoredJoin(new TableNameDefinition(tableName), StoredTable.StoredTypes.Name, parent);
				}
			}

			public static StoredJoin Create(Query subQuery, Query parent)
			{
				subQuery.Parent = parent;
				return new StoredJoin(new TableSubQueryDefinition(subQuery), StoredTable.StoredTypes.SubQuery, parent);
			}

			public static StoredJoin Create(StoredFunction stFunction, Query parent)
			{
				return new StoredJoin(new TableFunctionDefinition(stFunction), StoredTable.StoredTypes.SubQuery, parent);
			}

		}

        public class Value
		{

			public static StoredValue Create(ColumnSchema column, Query query)
			{
				return new StoredValue(new ValueStoredColumnDefinition(StorageFactory.Column.Create(column, query)), StoredValue.StoredTypes.StoredColumn);
			}

			public static StoredValue Create(ColumnSchema column, StoredTable table, Query query)
			{
				return new StoredValue(new ValueStoredColumnDefinition(StorageFactory.Column.Create(column, table, query)), StoredValue.StoredTypes.StoredColumn);
			}

			public static StoredValue Create(string columnName, StoredTable table, Query query)
			{
				return new StoredValue(new ValueStoredColumnDefinition(StorageFactory.Column.Create(columnName, table, query)), StoredValue.StoredTypes.StoredColumn);
			}

			public static StoredValue Create(object value)
			{
				return new StoredValue(new ValueObjectDefinition(value), StoredValue.StoredTypes.ObjectValue);
			}

			public static StoredValue Create(object startValue, object endValue)
			{
				return new StoredValue(new ValueIntervalDefinition(startValue, endValue), StoredValue.StoredTypes.Interval);
			}

			public static StoredValue Create(Query subQuery)
			{
				return new StoredValue(new ValueSubQueryDefinition(subQuery), StoredValue.StoredTypes.SubQuery);
			}

			public static StoredValue Create(StoredFunction stFunction)
			{
				return new StoredValue(new ValueStoredFunctionDefinition(stFunction), StoredValue.StoredTypes.StoredFunction);
			}

			public static StoredValue Create(StoredFunction startFunction, StoredFunction endFunction)
			{
				return new StoredValue(new ValueStoredFunctionIntervalDefinition(startFunction, endFunction), StoredValue.StoredTypes.IntervalStoredFunction);
			}

			public static StoredValue Create(IEnumerable values)
			{
				return new StoredValue(new ValueArrayDefinition(values), StoredValue.StoredTypes.Array);
			}

			public static StoredValue Create(object[] values)
			{
				return new StoredValue(new ValueArrayDefinition(values), StoredValue.StoredTypes.Array);
			}

			public static StoredValue Create(List<object> values)
			{
				return new StoredValue(new ValueArrayDefinition(values), StoredValue.StoredTypes.Array);
			}

			public static StoredValue Create(ColumnSchema startColumn, ColumnSchema endColumn, StoredTable table, Query query)
			{
				return Create(StorageFactory.Column.Create(startColumn, table, query), StorageFactory.Column.Create(endColumn, table, query));
			}

			public static StoredValue Create(string startColumnName, string endColumnName, StoredTable table, Query query)
			{
				return Create(StorageFactory.Column.Create(startColumnName, table, query), StorageFactory.Column.Create(endColumnName, table, query));
			}

			public static StoredValue Create(ColumnSchema startColumn, StoredTable startTable, ColumnSchema endColumn, StoredTable endTable, Query query)
			{
				return Create(StorageFactory.Column.Create(startColumn, startTable, query), StorageFactory.Column.Create(endColumn, endTable, query));
			}

			public static StoredValue Create(string startColumnName, StoredTable startTable, string endColumnName, StoredTable endTable, Query query)
			{
				return Create(StorageFactory.Column.Create(startColumnName, startTable, query), StorageFactory.Column.Create(endColumnName, endTable, query));
			}

			public static StoredValue Create(ColumnSchema startColumn, string endColumnName, StoredTable table, Query query)
			{
				return Create(StorageFactory.Column.Create(startColumn, table, query), StorageFactory.Column.Create(endColumnName, table, query));
			}

			public static StoredValue Create(string startColumnName, ColumnSchema endColumn, StoredTable table, Query query)
			{
				return Create(StorageFactory.Column.Create(startColumnName, table, query), StorageFactory.Column.Create(endColumn, table, query));
			}

			public static StoredValue Create(ColumnSchema startColumn, StoredTable startTable, string endColumnName, StoredTable endTable, Query query)
			{
				return Create(StorageFactory.Column.Create(startColumn, startTable, query), StorageFactory.Column.Create(endColumnName, endTable, query));
			}

			public static StoredValue Create(string startColumnName, StoredTable startTable, ColumnSchema endColumn, StoredTable endTable, Query query)
			{
				return Create(StorageFactory.Column.Create(startColumnName, startTable, query), StorageFactory.Column.Create(endColumn, endTable, query));
			}

			private static StoredValue Create(StoredColumn startColumn, StoredColumn endColumn)
			{
				return new StoredValue(new ValueStoredColumnIntervalDefinition(startColumn, endColumn), StoredValue.StoredTypes.IntervalStoredColumns);
			}

			public static StoredValue Create(ColumnSchema startColumn, ColumnSchema endColumn, Query query)
			{
				return Create(StorageFactory.Column.Create(startColumn, query), StorageFactory.Column.Create(endColumn, query));
			}

			public static StoredValue CreateValue(TableSchema table, string columnName, Query query)
			{
				ColumnSchema colSc = table.Columns[columnName];
				StoredTable stTable = StorageFactory.Table.Create(table, query);
				return Create(colSc, stTable, query);
			}

			public static StoredValue CreateValue(TableSchema table, string startColumnName, string endColumnName, Query query)
			{
				ColumnSchema startCol = table.Columns[startColumnName];
				ColumnSchema endCol = table.Columns[endColumnName];
				StoredTable stTable = StorageFactory.Table.Create(table, query);
				return Create(startCol, endCol, stTable, query);
			}

			public static StoredValue CreateValue(TableSchema startTable, string startColumnName, TableSchema endTable, string endColumnName, Query query)
			{
				ColumnSchema startCol = startTable.Columns[startColumnName];
				ColumnSchema endCol = endTable.Columns[endColumnName];
				StoredTable stStartTable = StorageFactory.Table.Create(startTable, query);
				StoredTable stEndTable = StorageFactory.Table.Create(endTable, query);
				return Create(startCol, stStartTable, endCol, stEndTable, query);
			}

			public static StoredValue CreateValue<TEtt>(string columnName, Query query) where TEtt : EntityBase
			{
				StoredTable stTable = StorageFactory.Table.Create<TEtt>(query);
				return Create(columnName, stTable, query);
			}

			public static StoredValue CreateValue(string tableName, string columnName, Query query)
			{
				StoredTable stTable = StorageFactory.Table.Create(tableName, query);
				return Create(columnName, stTable, query);
			}

			public static StoredValue CreateValue(string tableName, string startColumnName, string endColumnName, Query query)
			{
				StoredTable stTable = StorageFactory.Table.Create(tableName, query);
				return Create(startColumnName, endColumnName, stTable, query);
			}

			public static StoredValue CreateValue(string startTableName, string startColumnName, string endTableName, string endColumnName, Query query)
			{
				StoredTable stStartTable = StorageFactory.Table.Create(startTableName, query);
				StoredTable stEndTable = StorageFactory.Table.Create(endTableName, query);
				return Create(startColumnName, stStartTable, endColumnName, stEndTable, query);
			}


            public static StoredValue CreateValue(string columnName, Query query)
            {
                return new StoredValue(new ValueStoredColumnDefinition(
                    Column.Create(columnName, query)), StoredValue.StoredTypes.StoredColumn);
            }
        }

        public class Constraint
		{

			public static StoredConstraint Create(StoredColumn storedColumn, StoredConstraint.ConstraintType type, Query parent)
			{
				StoredConstraint ret = new StoredConstraint();
				ret.Column = storedColumn;
				ret.Type = type;
				ret.Parent = parent;
				return ret;
			}

			public static StoredConstraint Create(StoredColumn storedColumn, StoredConstraint.ConstraintType type, ComparisonOperator comparison, Query parent)
			{
				StoredConstraint ret = new StoredConstraint();
				ret.Column = storedColumn;
				ret.Type = type;
				ret.Parent = parent;
				return ret;
			}

			public static StoredConstraint Create(ComparisonOperator comparison, Query parent)
			{
				StoredConstraint ret = new StoredConstraint();
				ret.Type = StoredConstraint.ConstraintType.None;
				ret.Comparison = comparison;
				ret.Parent = parent;
				return ret;
			}

			public static StoredConstraint Create(StoredConstraint.ConstraintType type, ComparisonOperator comparison, Query parent)
			{
				StoredConstraint ret = new StoredConstraint();
				ret.Type = type;
				ret.Comparison = comparison;
				ret.Parent = parent;
				return ret;
			}

			public static StoredConstraint CreateWhere(StoredColumn storedColumn, Query parent)
			{
				return Create(storedColumn, StoredConstraint.ConstraintType.Where, parent);
			}

			public static StoredConstraint CreateAnd(StoredColumn storedColumn, Query parent)
			{
				return Create(storedColumn, StoredConstraint.ConstraintType.And, parent);
			}

			public static StoredConstraint CreateOr(StoredColumn storedColumn, Query parent)
			{
				return Create(storedColumn, StoredConstraint.ConstraintType.Or, parent);
			}

			public static StoredConstraint CreateWhereOpenParentesis(StoredColumn storedColumn, Query parent)
			{
				return Create(storedColumn, StoredConstraint.ConstraintType.Where, ComparisonOperator.OpenParentheses, parent);
			}

			public static StoredConstraint CreateAndOpenParentesis(StoredColumn storedColumn, Query parent)
			{
				return Create(storedColumn, StoredConstraint.ConstraintType.And, ComparisonOperator.OpenParentheses, parent);
			}

			public static StoredConstraint CreateOrOpenParentesis(StoredColumn storedColumn, Query parent)
			{
				return Create(storedColumn, StoredConstraint.ConstraintType.Or, ComparisonOperator.OpenParentheses, parent);
			}

			public static StoredConstraint CreateWhereOpenParentesis(Query parent)
			{
				return Create(StoredConstraint.ConstraintType.Where, ComparisonOperator.OpenParentheses, parent);
			}

			public static StoredConstraint CreateAndOpenParentesis(Query parent)
			{
				return Create(StoredConstraint.ConstraintType.And, ComparisonOperator.OpenParentheses, parent);
			}

			public static StoredConstraint CreateOrOpenParentesis(Query parent)
			{
				return Create(StoredConstraint.ConstraintType.Or, ComparisonOperator.OpenParentheses, parent);
			}

			public static StoredConstraint CreateOpenParentesis(Query parent)
			{
				return Create(ComparisonOperator.OpenParentheses, parent);
			}

			public static StoredConstraint CreateCloseParentesis(Query parent)
			{
				return Create(ComparisonOperator.CloseParentheses, parent);
			}

		}

        public class JoinConstraint
		{

			public static StoredJoinConstraint Create(StoredColumn storedColumn, StoredJoinConstraint.ConstraintType type, StoredJoin parent)
			{
				StoredJoinConstraint ret = new StoredJoinConstraint();
				ret.Column = storedColumn;
				ret.Type = type;
				ret.Parent = parent;
				return ret;
			}

			public static StoredJoinConstraint Create(StoredColumn storedColumn, StoredJoinConstraint.ConstraintType type, ComparisonOperator comparison, StoredJoin parent)
			{
				StoredJoinConstraint ret = new StoredJoinConstraint();
				ret.Column = storedColumn;
				ret.Type = type;
				ret.Parent = parent;
				return ret;
			}

			public static StoredJoinConstraint Create(ComparisonOperator comparison, StoredJoin parent)
			{
				StoredJoinConstraint ret = new StoredJoinConstraint();
				ret.Type = StoredJoinConstraint.ConstraintType.None;
				ret.Comparison = comparison;
				ret.Parent = parent;
				return ret;
			}

			public static StoredJoinConstraint Create(StoredJoinConstraint.ConstraintType type, ComparisonOperator comparison, StoredJoin parent)
			{
				StoredJoinConstraint ret = new StoredJoinConstraint();
				ret.Type = type;
				ret.Comparison = comparison;
				ret.Parent = parent;
				return ret;
			}

			public static StoredJoinConstraint CreateWhere(StoredColumn storedColumn, StoredJoin parent)
			{
				return Create(storedColumn, StoredJoinConstraint.ConstraintType.Where, parent);
			}

			public static StoredJoinConstraint CreateAnd(StoredColumn storedColumn, StoredJoin parent)
			{
				return Create(storedColumn, StoredJoinConstraint.ConstraintType.And, parent);
			}

			public static StoredJoinConstraint CreateOr(StoredColumn storedColumn, StoredJoin parent)
			{
				return Create(storedColumn, StoredJoinConstraint.ConstraintType.Or, parent);
			}

			public static StoredJoinConstraint CreateWhereOpenParentesis(StoredColumn storedColumn, StoredJoin parent)
			{
				return Create(storedColumn, StoredJoinConstraint.ConstraintType.Where, ComparisonOperator.OpenParentheses, parent);
			}

			public static StoredJoinConstraint CreateAndOpenParentesis(StoredColumn storedColumn, StoredJoin parent)
			{
				return Create(storedColumn, StoredJoinConstraint.ConstraintType.And, ComparisonOperator.OpenParentheses, parent);
			}

			public static StoredJoinConstraint CreateOrOpenParentesis(StoredColumn storedColumn, StoredJoin parent)
			{
				return Create(storedColumn, StoredJoinConstraint.ConstraintType.Or, ComparisonOperator.OpenParentheses, parent);
			}

			public static StoredJoinConstraint CreateWhereOpenParentesis(StoredJoin parent)
			{
				return Create(StoredJoinConstraint.ConstraintType.Where, ComparisonOperator.OpenParentheses, parent);
			}

			public static StoredJoinConstraint CreateAndOpenParentesis(StoredJoin parent)
			{
				return Create(StoredJoinConstraint.ConstraintType.And, ComparisonOperator.OpenParentheses, parent);
			}

			public static StoredJoinConstraint CreateOrOpenParentesis(StoredJoin parent)
			{
				return Create(StoredJoinConstraint.ConstraintType.Or, ComparisonOperator.OpenParentheses, parent);
			}

			public static StoredJoinConstraint CreateOpenParentesis(StoredJoin parent)
			{
				return Create(ComparisonOperator.OpenParentheses, parent);
			}

			public static StoredJoinConstraint CreateCloseParentesis(StoredJoin parent)
			{
				return Create(ComparisonOperator.OpenParentheses, parent);
			}

		}

        public class CaseConstraint
		{

			public static CaseCondition Create(StoredColumn storedColumn, StoredCaseWhen parent)
			{
				CaseCondition ret = new CaseCondition(parent);
				ret.Column = storedColumn;
				ret.Type = StoredConstraint.ConstraintType.None;
				return ret;
			}

			public static CaseCondition Create(StoredColumn storedColumn, ComparisonOperator comparison, StoredCaseWhen parent)
			{
				CaseCondition ret = new CaseCondition(parent);
				ret.Column = storedColumn;
				ret.Type = StoredConstraint.ConstraintType.None;
				return ret;
			}

			public static CaseCondition Create(ComparisonOperator comparison, StoredCaseWhen parent)
			{
				CaseCondition ret = new CaseCondition(parent);
				ret.Comparison = comparison;
				ret.Type = StoredConstraint.ConstraintType.None;
				return ret;
			}

			public static CaseCondition CreateCondition(StoredColumn storedColumn, StoredCaseWhen parent)
			{
				return Create(storedColumn, parent);
			}

		}

        public class SetColumnValue
		{

			public static SetColumnValueRet<T> Create<T>(Schema.ColumnSchema columnSchema, Query query, T parent)
			{
				return Create<T>(columnSchema, Table.Create(columnSchema.Parent, query), query, parent);
			}

			public static SetColumnValueRet<T> Create<T>(Schema.ColumnSchema columnSchema, StoredTable table, Query query, T parent)
			{
				return new SetColumnValueRet<T>(columnSchema, StoredColumn.StoredTypes.Schema, table, query, parent);
			}

			public static SetColumnValueRet<T> Create<T>(string name, StoredTable table, Query query, T parent)
			{
				if ((table.Type == StoredTable.StoredTypes.Schema)) {
					IColumnDefinition colDef = table.TableDefinition.GetColumn(name);
					return new SetColumnValueRet<T>(colDef, StoredColumn.StoredTypes.Schema, table, query, parent);
				} else {
					return new SetColumnValueRet<T>(new ColumnNameDefinition(name, table.TableDefinition), StoredColumn.StoredTypes.Name, table, query, parent);
				}
			}

			static internal SetColumnValueRet<T> Create<T>(StoredTable table, string nickName, Query query, T parent)
			{
				return new SetColumnValueRet<T>(new ColumnAliasDefinition(nickName, table.TableDefinition), StoredColumn.StoredTypes.OnlyAlias, table, query, parent);
			}

			public static SetColumnValueRet<T> Create<T>(string name, Query query, T parent)
			{
				return new SetColumnValueRet<T>(new ColumnUnknowTableDefinition(name), StoredColumn.StoredTypes.UnknowTable, query, parent);
			}

			public static SetColumnValueRet<T> Create<T>(Schema.ColumnSchema columnSchema, string tableName, Query query, T parent)
			{
				StoredTable storedTable = Table.Create(tableName, query);
				return Create<T>(columnSchema, storedTable, query, parent);
			}

			public static SetColumnValueRet<T> Create<T>(string name, string tableName, Query query, T parent)
			{
				StoredTable storedTable = Table.Create(tableName, query);
				return Create<T>(name, storedTable, query, parent);
			}

			public static SetColumnValueRet<T> Create<T>(string name, TableSchema tableSchema, Query query, T parent)
			{
				StoredTable storedTable = Table.Create(tableSchema, query);
				return Create<T>(name, storedTable, query, parent);
			}

			public static SetColumnValueRet<T> Create<T>(string name, EntityBase entity, Query query, T parent)
			{
				StoredTable storedTable = Table.Create(entity, query);
				return Create<T>(name, storedTable, query, parent);
			}

			public static SetColumnValueRet<T> Create<TEtt, T>(string name, Query query, T parent) where TEtt : EntityBase
			{
				StoredTable storedTable = Table.Create<TEtt>(query);
				return Create<T>(name, storedTable, query, parent);
			}

			static internal SetColumnValueRet<T> Create<T>(StoredFunction stFunction, StoredTable table, Query query, T parent)
			{
				return new SetColumnValueRet<T>(new ColumnFunctionDefinition(stFunction, table.TableDefinition), StoredColumn.StoredTypes.StoredFunction, table, query, parent);
			}

			public static SetColumnValueRet<T> Create<T>(StoredFunction stFunction, Query query, T parent)
			{
				return new SetColumnValueRet<T>(new ColumnFunctionDefinition(stFunction), StoredColumn.StoredTypes.StoredFunction, query, parent);
			}

			static internal SetColumnValueRet<T> CreateNick<T>(string tableName, string nickName, Query query, T parent)
			{
				StoredTable storedTable = Table.Create(tableName, query);
				return Create<T>(storedTable, nickName, query, parent);
			}

			public static Storage.Value.SetColumnValue Create(Schema.ColumnSchema columnSchema, Query query)
			{
				return Create(columnSchema, Table.Create(columnSchema.Parent, query), query);
			}

			public static Storage.Value.SetColumnValue Create(Schema.ColumnSchema columnSchema, StoredTable table, Query query)
			{
				return new Storage.Value.SetColumnValue(columnSchema, StoredColumn.StoredTypes.Schema, table, query);
			}

			public static Storage.Value.SetColumnValue Create(string name, StoredTable table, Query query)
			{
				if ((table.Type == StoredTable.StoredTypes.Schema)) {
					IColumnDefinition colDef = table.TableDefinition.GetColumn(name);
					return new Storage.Value.SetColumnValue(colDef, StoredColumn.StoredTypes.Schema, table, query);
				} else {
					return new Storage.Value.SetColumnValue(new ColumnNameDefinition(name, table.TableDefinition), StoredColumn.StoredTypes.Name, table, query);
				}
			}

			static internal Storage.Value.SetColumnValue Create(StoredTable table, string nickName, Query query)
			{
				return new Storage.Value.SetColumnValue(new ColumnAliasDefinition(nickName, table.TableDefinition), StoredColumn.StoredTypes.OnlyAlias, table, query);
			}

			public static Storage.Value.SetColumnValue Create(string name, Query query)
			{
				return new Storage.Value.SetColumnValue(new ColumnUnknowTableDefinition(name), StoredColumn.StoredTypes.UnknowTable, query);
			}

			public static Storage.Value.SetColumnValue Create(Schema.ColumnSchema columnSchema, string tableName, Query query)
			{
				StoredTable storedTable = Table.Create(tableName, query);
				return Create(columnSchema, storedTable, query);
			}

			public static Storage.Value.SetColumnValue Create(string name, string tableName, Query query)
			{
				StoredTable storedTable = Table.Create(tableName, query);
				return Create(name, storedTable, query);
			}

			public static Storage.Value.SetColumnValue Create(string name, TableSchema tableSchema, Query query)
			{
				StoredTable storedTable = Table.Create(tableSchema, query);
				return Create(name, storedTable, query);
			}

			public static Storage.Value.SetColumnValue Create(string name, EntityBase entity, Query query)
			{
				StoredTable storedTable = Table.Create(entity, query);
				return Create(name, storedTable, query);
			}

			public static Storage.Value.SetColumnValue Create<TEtt>(string name, Query query) where TEtt : EntityBase
			{
				StoredTable storedTable = Table.Create<TEtt>(query);
				return Create(name, storedTable, query);
			}

			static internal Storage.Value.SetColumnValue Create(StoredFunction stFunction, StoredTable table, Query query)
			{
				return new Storage.Value.SetColumnValue(new ColumnFunctionDefinition(stFunction, table.TableDefinition), StoredColumn.StoredTypes.StoredFunction, table, query);
			}

			public static Storage.Value.SetColumnValue Create(StoredFunction stFunction, Query query)
			{
				return new Storage.Value.SetColumnValue(new ColumnFunctionDefinition(stFunction), StoredColumn.StoredTypes.StoredFunction, query);
			}

			static internal Storage.Value.SetColumnValue CreateNick<T>(string tableName, string nickName, Query query)
			{
				StoredTable storedTable = Table.Create(tableName, query);
				return Create(storedTable, nickName, query);
			}

		}

        public class SFunction
		{

			public static StoredFunction Create(string name)
			{
				return new StoredFunction(name);
			}

			public static StoredFunction Create(string name, string owner)
			{
				return new StoredFunction(name, owner);
			}

			public static StoredFunction Create(string name, Query query)
			{
				return new StoredFunction(name, query);
			}

			public static StoredFunction Create(string name, string owner, Query query)
			{
				return new StoredFunction(name, owner, query);
			}

			public class Avg
			{

				public static FunctionAvg Create()
				{
					return new FunctionAvg();
				}

				public static FunctionAvg Create(Query query)
				{
					return new FunctionAvg(query);
				}

			}

			public class Count
			{

				public static FunctionCount Create()
				{
					return new FunctionCount();
				}

				public static FunctionCount Create(Query query)
				{
					return new FunctionCount(query);
				}

			}

			public class Max
			{

				public static FunctionMax Create()
				{
					return new FunctionMax();
				}

				public static FunctionMax Create(Query query)
				{
					return new FunctionMax(query);
				}

			}

			public class Min
			{

				public static FunctionMin Create()
				{
					return new FunctionMin();
				}

				public static FunctionMin Create(Query query)
				{
					return new FunctionMin(query);
				}

			}

			public class Sum
			{

				public static FunctionSum Create()
				{
					return new FunctionSum();
				}

				public static FunctionSum Create(Query query)
				{
					return new FunctionSum(query);
				}

			}

			public class Convert
			{

				public static FunctionConvert Create()
				{
					return new FunctionConvert();
				}

				public static FunctionConvert Create(Query query)
				{
					return new FunctionConvert(query);
				}

			}

            public class SequenciaParam
            {

                public static SequenciaParamStoredFunction Create(string name)
                {
                    return new SequenciaParamStoredFunction(name);
                }

                public static SequenciaParamStoredFunction Create(string name, string owner)
                {
                    return new SequenciaParamStoredFunction(name, owner);
                }

                public static SequenciaParamStoredFunction Create(string name, Query query)
                {
                    return new SequenciaParamStoredFunction(name, query);
                }

                public static SequenciaParamStoredFunction Create(string name, string owner, Query query)
                {
                    return new SequenciaParamStoredFunction(name, owner, query);
                }

            }

        }

        public class OrderBy
		{

			public static StoredOrderBy Create(Schema.ColumnSchema columnSchema, Query query)
			{
				return Create(columnSchema, Table.Create(columnSchema.Parent, query), query);
			}

			public static StoredOrderBy Create(Schema.ColumnSchema columnSchema, StoredTable table, Query query)
			{
				return new StoredOrderBy(columnSchema, StoredColumn.StoredTypes.Schema, table, query);
			}

			public static StoredOrderBy Create(string name, StoredTable table, Query query)
			{
				if ((table.Type == StoredTable.StoredTypes.Schema)) {
					IColumnDefinition colDef = table.TableDefinition.GetColumn(name);
					return new StoredOrderBy(colDef, StoredColumn.StoredTypes.Schema, table, query);
				} else {
					return new StoredOrderBy(new ColumnNameDefinition(name, table.TableDefinition), StoredColumn.StoredTypes.Name, table, query);
				}
			}

			static internal StoredOrderBy Create(StoredTable table, string nickName, Query query)
			{
				return new StoredOrderBy(new ColumnAliasDefinition(nickName, table.TableDefinition), StoredColumn.StoredTypes.OnlyAlias, table, query);
			}

			public static StoredOrderBy Create(string name, Query query)
			{
				return new StoredOrderBy(new ColumnUnknowTableDefinition(name), StoredColumn.StoredTypes.UnknowTable, query);
			}

			public static StoredOrderBy Create(Schema.ColumnSchema columnSchema, string tableName, Query query)
			{
				StoredTable storedTable = Table.Create(tableName, query);
				return Create(columnSchema, storedTable, query);
			}

			public static StoredOrderBy Create(string name, string tableName, Query query)
			{
				StoredTable storedTable = Table.Create(tableName, query);
				return Create(name, storedTable, query);
			}

			public static StoredOrderBy Create(string name, TableSchema tableSchema, Query query)
			{
				StoredTable storedTable = Table.Create(tableSchema, query);
				return Create(name, storedTable, query);
			}

			public static StoredOrderBy Create(string name, EntityBase entity, Query query)
			{
				StoredTable storedTable = Table.Create(entity, query);
				return Create(name, storedTable, query);
			}

			public static StoredOrderBy Create<TEtt>(string name, Query query) where TEtt : EntityBase
			{
				StoredTable storedTable = Table.Create<TEtt>(query);
				return Create(name, storedTable, query);
			}

			static internal StoredOrderBy Create(StoredFunction stFunction, StoredTable table, Query query)
			{
				return new StoredOrderBy(new ColumnFunctionDefinition(stFunction, table.TableDefinition), StoredColumn.StoredTypes.StoredFunction, table, query);
			}

			public static StoredOrderBy Create(StoredFunction stFunction, Query query)
			{
				return new StoredOrderBy(new ColumnFunctionDefinition(stFunction), StoredColumn.StoredTypes.StoredFunction, query);
			}

		}

        public class GroupBy
		{

			public static StoredGroupBy Create(Schema.ColumnSchema columnSchema, Query query)
			{
				return Create(columnSchema, Table.Create(columnSchema.Parent, query), query);
			}

			public static StoredGroupBy Create(Schema.ColumnSchema columnSchema, StoredTable table, Query query)
			{
				return new StoredGroupBy(columnSchema, StoredColumn.StoredTypes.Schema, table, query);
			}

			public static StoredGroupBy Create(string name, StoredTable table, Query query)
			{
				if ((table.Type == StoredTable.StoredTypes.Schema)) {
					IColumnDefinition colDef = table.TableDefinition.GetColumn(name);
					return new StoredGroupBy(colDef, StoredColumn.StoredTypes.Schema, table, query);
				} else {
					return new StoredGroupBy(new ColumnNameDefinition(name, table.TableDefinition), StoredColumn.StoredTypes.Name, table, query);
				}
			}

			static internal StoredGroupBy Create(StoredTable table, string nickName, Query query)
			{
				return new StoredGroupBy(new ColumnAliasDefinition(nickName, table.TableDefinition), StoredColumn.StoredTypes.OnlyAlias, table, query);
			}

			public static StoredGroupBy Create(string name, Query query)
			{
				return new StoredGroupBy(new ColumnUnknowTableDefinition(name), StoredColumn.StoredTypes.UnknowTable, query);
			}

			public static StoredGroupBy Create(Schema.ColumnSchema columnSchema, string tableName, Query query)
			{
				StoredTable storedTable = Table.Create(tableName, query);
				return Create(columnSchema, storedTable, query);
			}

			public static StoredGroupBy Create(string name, string tableName, Query query)
			{
				StoredTable storedTable = Table.Create(tableName, query);
				return Create(name, storedTable, query);
			}

			public static StoredGroupBy Create(string name, TableSchema tableSchema, Query query)
			{
				StoredTable storedTable = Table.Create(tableSchema, query);
				return Create(name, storedTable, query);
			}

			public static StoredGroupBy Create(string name, EntityBase entity, Query query)
			{
				StoredTable storedTable = Table.Create(entity, query);
				return Create(name, storedTable, query);
			}

			public static StoredGroupBy Create<TEtt>(string name, Query query) where TEtt : EntityBase
			{
				StoredTable storedTable = Table.Create<TEtt>(query);
				return Create(name, storedTable, query);
			}

			static internal StoredGroupBy Create(StoredFunction stFunction, StoredTable table, Query query)
			{
				return new StoredGroupBy(new ColumnFunctionDefinition(stFunction, table.TableDefinition), StoredColumn.StoredTypes.StoredFunction, table, query);
			}

			public static StoredGroupBy Create(StoredFunction stFunction, Query query)
			{
				return new StoredGroupBy(new ColumnFunctionDefinition(stFunction), StoredColumn.StoredTypes.StoredFunction, query);
			}

		}

        public class CaseFactory
		{

			/// <summary>
			/// Where the column.
			/// </summary>
			/// <param name="column">The column.</param>
			/// <returns></returns>
			public StoredCaseWhen Create(Query query)
			{
				return new StoredCaseWhen(query);
			}

			/// <summary>
			/// Where the column.
			/// </summary>
			/// <param name="column">The column.</param>
			/// <returns></returns>
			public StoredCase Create(StoredValue value, Query query)
			{
				return new StoredCase();
			}

		}

		public class Having
		{

		}

        public class NQuery
		{

			/// <summary>
			/// Initialize Select query.
			/// </summary>
			/// <returns></returns>
			static public Query Select(Provider provider)
			{
				return new Query(Query.QueryType.Select, provider);
			}

			/// <summary>
			/// Initialize Insert query.
			/// </summary>
			/// <returns></returns>
            static public Query Insert(Provider provider)
			{
				return new Query(Query.QueryType.Insert, provider);
			}

			/// <summary>
			/// Initialize Update query.
			/// </summary>
			/// <returns></returns>
            static public Query Update(Provider provider)
			{
				return new Query(Query.QueryType.Update, provider);
			}

			/// <summary>
			/// Initialize Delete query.
			/// </summary>
			/// <returns></returns>
            static public Query Delete(Provider provider)
			{
				return new Query(Query.QueryType.Delete, provider);
			}

			/// <summary>
			/// Initialize Procedure query.
			/// </summary>
			/// <returns></returns>
            static public Query Procedure(string procedureName, Provider provider)
			{
				Query query = new Query(Query.QueryType.Procedure, provider);
				query.Into(procedureName);
				return query;
			}

			/// <summary>
			/// Initialize Procedure query.
			/// </summary>
			/// <returns></returns>
            static public Query Procedure<TEtt>() where TEtt : EntityBase
			{
				Provider provider = Provider.GetProvider<TEtt>();
				Query query = new Query(Query.QueryType.Procedure, provider);
				query.Into<TEtt>();
				return query;
			}

		}

    }

}
