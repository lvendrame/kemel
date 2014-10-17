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
    public class QueryCompilerFusion
    {
        private Query bruteQuery;

        public QueryCompilerFusion(Query query)
        {
            bruteQuery = query;
        }

        public QueryCompilerFusion(Query query, Query parent)
        {
            bruteQuery = query;
            bruteQuery.Parent = parent;
        }

        public Query Compile()
        {
            this.CompileTables();
            this.CompileColumns();
            this.CompileConstraints();
            this.CompileOrderBys();
            this.CompileGroupBys();
            this.CompilerUnions();
            this.CompileSetValues();

            return bruteQuery;
        }

        private void CompileTables()
        {
            List<StoredTable> lstSQTables = new List<StoredTable>();
            List<StoredJoin> lstSQJoins = new List<StoredJoin>();

            if (bruteQuery.IntoTable != null)
            {
                if (bruteQuery.IntoTable.Type == StoredTable.StoredTypes.SubQuery)
                {
                    lstSQTables.Add(bruteQuery.IntoTable);
                }
            }

            foreach (StoredTable table in bruteQuery.Tables)
            {
                if (table.Type == StoredTable.StoredTypes.SubQuery)
                {
                    lstSQTables.Add(table);
                }
            }

            foreach (StoredJoin join in bruteQuery.Joins)
            {
                if (join.Type == StoredTable.StoredTypes.SubQuery)
                {
                    lstSQJoins.Add(join);
                }
                this.VerifyJoin(join);
            }

            foreach (StoredTable subQueryTable in lstSQTables)
            {
                TableSubQueryDefinition tbdSubQuery = (TableSubQueryDefinition)subQueryTable.TableDefinition;
                QueryCompilerFusion cmp = new QueryCompilerFusion(tbdSubQuery.SubQuery, this.bruteQuery);
                cmp.Compile();
            }

            foreach (StoredJoin subQueryJoin in lstSQJoins)
            {
                TableSubQueryDefinition tbdSubQuery = (TableSubQueryDefinition)subQueryJoin.TableDefinition;
                QueryCompilerFusion cmp = new QueryCompilerFusion(tbdSubQuery.SubQuery, this.bruteQuery);
                cmp.Compile();
            }

        }

        private void CompileColumns()
        {
            foreach (StoredColumn column in bruteQuery.Columns)
            {
                this.VerifyColumn(column);
            }

            if (bruteQuery.IntoTable != null)
            {
                foreach (StoredColumn column in bruteQuery.IntoTable.StoredColumns)
                {
                    this.VerifyColumn(column, bruteQuery.IntoTable);
                    bruteQuery.Columns.Add(column);
                }
                bruteQuery.IntoTable.StoredColumns.Clear();
            }

            foreach (StoredTable table in bruteQuery.Tables)
            {
                foreach (StoredColumn column in table.StoredColumns)
                {
                    this.VerifyColumn(column, table);
                    bruteQuery.Columns.Add(column);
                }
                table.StoredColumns.Clear();
            }

            foreach (StoredJoin join in bruteQuery.Joins)
            {
                foreach (StoredColumn column in join.StoredColumns)
                {
                    this.VerifyColumn(column, join);
                    bruteQuery.Columns.Add(column);
                }
                join.StoredColumns.Clear();
            }

        }

        private void VerifyConcat(StoredConcat concat)
        {
            foreach (StoredValue stValue in concat.Values)
            {
                this.VerifyValue(stValue);
            }
        }

        private void CompileConstraints()
        {
            bool nextTypeIsNone = false;
            foreach (StoredConstraint constraint in bruteQuery.Constraints)
            {
                if ((nextTypeIsNone))
                {
                    nextTypeIsNone = false;
                    constraint.Type = NQuery.Storage.Constraint.ConstraintBase.ConstraintType.None;
                }
                if ((constraint.Comparison == ComparisonOperator.OpenParentheses))
                {
                    nextTypeIsNone = true;
                }

                this.VerifyColumn(constraint.Column);
                if (constraint.Value != null)
                    this.VerifyValue(constraint.Value);
            }
        }

        private void CompileOrderBys()
        {
            foreach (StoredOrderBy orderBy in bruteQuery.OrderBys)
            {
                this.VerifyColumn(orderBy);
            }
        }

        private void CompileGroupBys()
        {
            foreach (StoredGroupBy groupBy in bruteQuery.GroupBys)
            {
                this.VerifyColumn(groupBy);
            }
        }

        private void CompilerUnions()
        {
            foreach (StoredUnion union in bruteQuery.Unions)
            {
                QueryCompilerFusion compiler = new QueryCompilerFusion(union.UnionQuery, bruteQuery);
                compiler.Compile();
            }
        }

        private void CompileSetValues()
        {
            if (bruteQuery.SetValues == null)
                return;

            foreach (SetColumnValue columnValue in bruteQuery.SetValues)
            {
                this.VerifyValue(columnValue.Value);
            }
        }

        private void VerifyValue(StoredValue stValue)
        {
            this.VerifyValue(stValue, null);
        }

        private void VerifyValue(StoredValue stValue, StoredTable stTable)
        {
            if (stValue == null)
                return;

            if ((stValue.Type == StoredValue.StoredTypes.IntervalStoredColumns))
            {
                ValueStoredColumnIntervalDefinition iscDef = (ValueStoredColumnIntervalDefinition)stValue.Value;
                this.VerifyColumn(iscDef.StartValue, stTable);
                this.VerifyColumn(iscDef.FinalValue, stTable);
            }
            else if ((stValue.Type == StoredValue.StoredTypes.StoredColumn))
            {
                ValueStoredColumnDefinition scDef = (ValueStoredColumnDefinition)stValue.Value;
                this.VerifyColumn(scDef.Value, stTable);
            }
            else if ((stValue.Type == StoredValue.StoredTypes.SubQuery))
            {
                ValueSubQueryDefinition ssqDef = (ValueSubQueryDefinition)stValue.Value;
                QueryCompilerFusion cmp = new QueryCompilerFusion(ssqDef.Value, this.bruteQuery);
                cmp.Compile();
                ssqDef.Value.Level = this.bruteQuery.NextLevel;
            }
            else if (stValue.Type == StoredValue.StoredTypes.StoredFunction)
            {
                ValueStoredFunctionDefinition sfDef = (ValueStoredFunctionDefinition)stValue.Value;
                this.VerifyFunction(sfDef.Value);
            }
            else if (stValue.Type == StoredValue.StoredTypes.IntervalStoredFunction)
            {
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

            string tableName = string.Empty;
            if (column.Table != null)
            {
                tableName = column.Table.Alias;
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = column.Table.TableDefinition.Name;
                }
            }
            else if ((column.Type == StoredColumn.StoredTypes.Schema))
            {
                tableName = (column.ColumnDefinition as ColumnSchema).Parent.Name;
            }

            if (!string.IsNullOrEmpty(tableName))
            {
                StoredTable table = null;

                if (stTable != null && stTable.Compare(tableName))
                {
                    table = stTable;
                }
                else
                {
                    table = this.FindTable(tableName);
                }

                if (table == null)
                {
                    throw new OrmException(string.Format("A tabela {0}, requisitada na coluna {1}, não foi encontrada.", tableName, column.ColumnDefinition.Name));
                }
                column.Table = table;
            }
            if (column.Type == StoredColumn.StoredTypes.StoredFunction)
            {
                ColumnFunctionDefinition stcFunction = (ColumnFunctionDefinition)column.ColumnDefinition;
                this.VerifyFunction(stcFunction.StoredFunction);
            }
            else if ((column.Type == StoredColumn.StoredTypes.Concat))
            {
                ColumnConcatDefinition stcConcat = (ColumnConcatDefinition)column.ColumnDefinition;
                this.VerifyConcat(stcConcat.Concat);
            }
        }

        private void VerifyJoin(StoredJoin join)
        {
            bool nextTypeIsNone = false;
            foreach (StoredJoinConstraint condition in join.Conditions)
            {
                if ((nextTypeIsNone))
                {
                    nextTypeIsNone = false;
                    condition.Type = ConstraintBase.ConstraintType.None;
                }
                if ((condition.Comparison == ComparisonOperator.OpenParentheses))
                {
                    nextTypeIsNone = true;
                }

                this.VerifyColumn(condition.Column, join);
                this.VerifyValue(condition.Value, join);
            }
        }

        private void VerifyFunction(StoredFunction storedFunction)
        {
            this.VerifySetColumnValueCollection(storedFunction.SetValues);
        }

        private void VerifySetColumnValueCollection(IEnumerable<SetColumnValue> setColumnValueCollection)
        {
            foreach (SetColumnValue setValue in setColumnValueCollection)
            {
                this.VerifyColumn(setValue);
                this.VerifyValue(setValue.Value);
            }
        }

        #region Finds

        private StoredTable FindTable(string tableName)
        {
            return this.FindTable(tableName, this.bruteQuery);
        }

        private StoredTable FindTable(string tableName, Query returnQuery)
        {
            if (returnQuery == null)
            {
                return null;
            }

            tableName = tableName.ToUpper();

            if (returnQuery.IntoTable != null)
            {
                if (returnQuery.IntoTable.Compare(tableName))
                {
                    return returnQuery.IntoTable;
                }
            }

            foreach (StoredTable table in returnQuery.Tables)
            {
                if (table.Compare(tableName))
                {
                    return table;
                }
            }

            foreach (StoredTable table in returnQuery.Joins)
            {
                if (table.Compare(tableName))
                {
                    return table;
                }
            }

            return FindTable(tableName, bruteQuery.Parent);
        }

        private StoredColumn FindColumn(string columnName)
        {
            return this.FindColumn(columnName, this.bruteQuery);
        }

        private StoredColumn FindColumn(string columnName, Query returnQuery)
        {
            if (returnQuery == null)
            {
                return null;
            }

            columnName = columnName.ToUpper();

            StoredColumn retColumn = null;

            if (returnQuery.IntoTable != null)
            {
                retColumn = FindColumn(columnName, returnQuery.IntoTable, returnQuery);
                if (retColumn != null)
                {
                    return retColumn;
                }
            }

            foreach (StoredTable table in returnQuery.Tables)
            {
                retColumn = FindColumn(columnName, table, returnQuery);
                if (retColumn != null)
                {
                    return retColumn;
                }
            }

            foreach (StoredTable table in returnQuery.Joins)
            {
                retColumn = FindColumn(columnName, table, returnQuery);
                if (retColumn != null)
                {
                    return retColumn;
                }
            }

            return FindColumn(columnName, bruteQuery.Parent);
        }

        private StoredColumn FindColumn(string columnName, StoredTable storedTable, Query query)
        {
            StoredColumn retColumn = null;

            retColumn = storedTable.FindColumn(columnName);
            if (retColumn != null)
            {
                return retColumn;
            }

            if (storedTable.Type == Table.StoredTable.StoredTypes.Schema)
            {
                IColumnDefinition columnDef = storedTable.TableDefinition.GetColumn(columnName);
                if (columnDef != null)
                {
                    return StorageFactory.Column.Create((ColumnSchema)columnDef, storedTable, query);
                }
            }

            return null;
        }

        private StoredColumn FindColumn(string tableName, string columnName)
        {
            return this.FindColumn(tableName, columnName, this.bruteQuery);
        }

        private StoredColumn FindColumn(string tableName, string columnName, Query query)
        {
            StoredTable table = this.FindTable(tableName);

            if (table == null)
            {
                return null;
            }

            StoredColumn column = table.FindColumn(columnName);
            if (column != null)
            {
                return column;
            }

            if (table.Type == StoredTable.StoredTypes.Schema)
            {
                IColumnDefinition columnDef = table.TableDefinition.GetColumn(columnName);
                if (columnDef != null)
                {
                    return StorageFactory.Column.Create((ColumnSchema)columnDef, table, query);
                }
            }
            else if (table.Type == StoredTable.StoredTypes.Name)
            {
                column = StorageFactory.Column.Create(columnName, table, query);
                table.TableDefinition.AddColumn(column.ColumnDefinition);
                return column;
            }

            return null;
        }

        #endregion
    }
}
