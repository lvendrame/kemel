using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm;
using Kemel.Orm.Providers;
using Kemel.Orm.Data;
using Kemel.Orm.NQuery.Storage.Column;
using Kemel.Orm.Constants;
using Kemel.Orm.NQuery.Storage.Table;
using System.Text;
using Kemel.Orm.NQuery.Storage.StoredSelect;
using Kemel.Orm.NQuery.Storage.Join;
using Kemel.Orm.NQuery.Storage;
using Kemel.Orm.NQuery.Storage.Function;
using Kemel.Orm.NQuery.Storage.Value;
using Kemel.Orm.NQuery.Storage.Constraint;
using Kemel.Orm.Schema;

namespace Kemel.Orm.NQuery.Builder
{

    public abstract class QueryBuilder
    {

        public QueryBuilder(Provider parent)
        {
            this.m_Parent = parent;
        }

        private Provider m_Parent;
        public Provider Parent
        {
            get { return m_Parent; }
        }

        public const string LEVEL_PREFIX = "_Lv";
        public const string INDEX_PREFIX = "_Idx";
        public const string CONSTANT_STRING_PARAMETER_PREFIX = "csparam_";

        public const string FUNCTION_PARAMETER_PREFIX = "fparam_";
        public abstract string DataBasePrefixParameter { get; }
        public abstract string DataBasePrefixVariable { get; }
        public abstract string ConcatOperator { get; }


        private bool blnBlockWriteConstraintType = true;
        public bool BlockWriteConstraintType
        {
            get { return this.blnBlockWriteConstraintType; }
            set { this.blnBlockWriteConstraintType = value; }
        }

        public virtual OrmCommand GetCommand(Query query)
        {
            return GetCommand(query, null);
        }

        public virtual OrmCommand GetCommand(Query query, OrmTransaction transaction)
        {
            OrmCommand retCommand = null;
            if ((transaction == null))
            {
                retCommand = this.Parent.GetConnection().CreateCommand();
            }
            else
            {
                retCommand = transaction.Connection.CreateCommand(transaction);
            }

            switch (query.Type)
            {
                case Query.QueryType.Select:
                    SelectWriter writerSelect = new SelectWriter(retCommand);
                    WriteSelect(writerSelect, query);
                    break;
                case Query.QueryType.Insert:
                    InsertWriter writerInsert = new InsertWriter(retCommand);
                    WriteInsert(writerInsert, query);
                    break;
                case Query.QueryType.Update:
                    UpdateWriter writerUpdate = new UpdateWriter(retCommand);
                    WriteUpdate(writerUpdate, query);
                    break;
                case Query.QueryType.Delete:
                    DeleteWriter writerDelete = new DeleteWriter(retCommand);
                    WriteDelete(writerDelete, query);
                    break;
                case Query.QueryType.Procedure:
                    DeleteWriter writerProcedure = new DeleteWriter(retCommand);
                    WriteProcedure(writerProcedure, query);
                    break;
            }
            return retCommand;
        }

        #region "Select"

        public virtual string GetJoinType(StoredJoin join)
        {
            switch (join.JoinType)
            {
                case StoredJoin.JoinTypes.Inner:
                    return DML.INNER_JOIN;
                case StoredJoin.JoinTypes.Left:
                    return DML.LEFT_JOIN;
                case StoredJoin.JoinTypes.Right:
                    return DML.RIGHT_JOIN;
                case StoredJoin.JoinTypes.Full:
                    return DML.FULL_JOIN;
                case StoredJoin.JoinTypes.LeftOuter:
                    return DML.LEFT_OUTER_JOIN;
                case StoredJoin.JoinTypes.RightOuter:
                    return DML.RIGHT_OUTER_JOIN;
                default:
                    return DML.INNER_JOIN;
            }
        }

        public virtual string GetSubSelect(OrmCommand command, Query query)
        {
            SelectWriter selectWriter = new SelectWriter(command);
            WriteSelect(selectWriter, query);
            return selectWriter.FinalString.ToString();
        }


        public virtual void WriteSelect(SelectWriter writer, Query query)
        {
            this.WriteSelectColumns(writer, query);

            this.WriteSelectTables(writer, query);

            this.WriteSelectJoins(writer, query);

            this.WriteConstraints(writer, query, writer.Constraints);

            this.WriteGroupBys(writer, query);

            this.WriteOrderBys(writer, query);

            this.WriteUnions(writer, query);

            this.WriteLimit(writer, query);

            this.WriteSelectFinalString(writer, query);

        }

        public const string LIMIT_COLUMN_ALIAS = "RN_LCA_X";
        public const string LIMIT_TABLE_ALIAS = "RN_LTBA_X";

        public virtual void WriteLimit(SelectWriter writer, Query query)
        {
            if (query.LimitRows != null)
            {
                //writer.Columns.Append(Punctuation.COMMA).Append(Punctuation.WHITE_SPACE);

                this.WriteLimitBegin(writer, query);
                this.WriteLimitEnd(writer, query);
            }
        }

        public virtual void WriteLimitBegin(SelectWriter writer, Query query)
        {
            writer.BeginLimit.Append(DML.SELECT).Append(Punctuation.WHITE_SPACE)
                .Append(Punctuation.ASTERISK).Append(Punctuation.WHITE_SPACE)
                .Append(DML.FROM).Append(Punctuation.PARENTHESIS_OPEN);

            writer.BeginLimit.Append(DML.SELECT).Append(Punctuation.WHITE_SPACE)
                .Append(Punctuation.ASTERISK).Append(Punctuation.WHITE_SPACE);

            WriteSelectLimitColumn(writer, query);

            writer.BeginLimit.Append(DML.FROM).Append(Punctuation.PARENTHESIS_OPEN);
        }

        public virtual void WriteLimitEnd(SelectWriter writer, Query query)
        {
            writer.EndLimit.Append(Punctuation.PARENTHESIS_CLOSE).Append(Punctuation.WHITE_SPACE)
                .AppendLine(LIMIT_TABLE_ALIAS)
                .Append(Punctuation.PARENTHESIS_CLOSE).Append(Punctuation.WHITE_SPACE)
                .AppendLine(LIMIT_TABLE_ALIAS)
                .Append(DML.WHERE).Append(Punctuation.WHITE_SPACE)
                .Append(LIMIT_COLUMN_ALIAS).Append(Punctuation.WHITE_SPACE)
                .Append(Comparisons.BETWEEN).Append(Punctuation.WHITE_SPACE)
                .Append(query.LimitRows.Start).Append(Punctuation.WHITE_SPACE)
                .Append(ConstraintTypes.AND).Append(Punctuation.WHITE_SPACE).Append(query.LimitRows.End);
        }

        public virtual void WriteSelectFinalString(SelectWriter writer, Query query)
        {
            writer.FinalString.Append(writer.BeginLimit);

            writer.FinalString.Append(DML.SELECT).Append(Punctuation.WHITE_SPACE);

            this.WriteSelectDistinct(writer, query);

            this.WriteSelectTop(writer, query);

            writer.FinalString.Append(writer.Columns.ToString()).AppendLine();
            if (writer.Tables.Length > 0)
            {
                writer.FinalString.Append(DML.FROM).Append(Punctuation.WHITE_SPACE);
                writer.FinalString.AppendLine(writer.Tables.ToString());
            }
            if ((writer.Joins.Length > 0))
            {
                writer.FinalString.AppendLine(writer.Joins.ToString());
            }
            if ((writer.Constraints.Length > 0))
            {
                writer.FinalString.AppendLine(writer.Constraints.ToString());
            }
            if ((writer.GroupBys.Length > 0))
            {
                writer.FinalString.Append(DML.GROUP_BY).Append(Punctuation.WHITE_SPACE);
                writer.FinalString.AppendLine(writer.GroupBys.ToString());
            }

            if ((writer.OrderBys.Length > 0) && query.LimitRows == null)
            {
                writer.FinalString.Append(DML.ORDER_BY).Append(Punctuation.WHITE_SPACE);
                writer.FinalString.AppendLine(writer.OrderBys.ToString());
            }

            if ((writer.Unions.Length > 0))
            {
                writer.FinalString.AppendLine(writer.Unions.ToString());
            }

            writer.FinalString.Append(writer.EndLimit);

            writer.SetCommandText();
        }

        public virtual void WriteSelectDistinct(SelectWriter writer, Query query)
        {
            if (query.IsDistinct)
            {
                writer.FinalString.Append(DML.DISTINCT).Append(Punctuation.WHITE_SPACE);
            }
        }

        public virtual void WriteSelectTop(SelectWriter writer, Query query)
        {
            if (query.TopRecords > 0)
            {
                writer.FinalString.Append(DML.TOP).Append(Punctuation.WHITE_SPACE).Append(query.TopRecords).Append(Punctuation.WHITE_SPACE);
            }
        }

        public virtual void WriteSelectColumns(SelectWriter writer, Query query)
        {
            for (int index = 0; index <= query.Columns.Count - 1; index++)
            {
                if ((index > 0))
                {
                    writer.Columns.Append(Punctuation.COMMA).Append(Punctuation.WHITE_SPACE);
                }

                StoredColumn column = query.Columns[index];
                WriteSelectColumn(writer, query, column);
            }
        }

        public virtual void WriteSelectColumn(SelectWriter writer, Query query, StoredColumn column)
        {
            WriteColumn(writer, query, column, writer.Columns);
            if ((column.HasAlias))
            {
                if (string.IsNullOrEmpty(column.Alias))
                {
                    writer.Columns.Append(Punctuation.WHITE_SPACE).Append(DML.AS).Append(Punctuation.WHITE_SPACE).Append(column.ColumnDefinition.Alias);
                }
                else
                {
                    writer.Columns.Append(Punctuation.WHITE_SPACE).Append(DML.AS).Append(Punctuation.WHITE_SPACE).Append(column.Alias);
                }
            }
        }

        public abstract void WriteSelectLimitColumn(SelectWriter writer, Query query);

        #region "StoredColumn"

        public virtual void WriteColumnSimple(BaseWriter writer, Query query, StoredColumn column, StringBuilder stbCommandPart)
        {
            switch (column.Type)
            {
                case StoredColumn.StoredTypes.Name:
                    WriteColumnNameSimple(writer, query, column, stbCommandPart);
                    break;
                case StoredColumn.StoredTypes.OnlyAlias:
                    WriteColumnOnlyAliasSimple(writer, query, column, stbCommandPart);
                    break;
                case StoredColumn.StoredTypes.Schema:
                    WriteColumnSchemaSimple(writer, query, column, stbCommandPart);
                    break;
                case StoredColumn.StoredTypes.StoredFunction:
                    WriteColumnStoredFunction(writer, query, column, stbCommandPart);
                    break;
                case StoredColumn.StoredTypes.UnknowTable:
                    WriteColumnUnknowTable(writer, query, column, stbCommandPart);
                    break;
                case StoredColumn.StoredTypes.Concat:
                    WriteColumnStoredConcat(writer, query, column, stbCommandPart);
                    break;
            }
        }

        public virtual void WriteColumnNameSimple(BaseWriter writer, Query query, StoredColumn column, StringBuilder stbCommandPart)
        {
            stbCommandPart.Append(GetColumnName(column));
        }

        public virtual void WriteColumnOnlyAliasSimple(BaseWriter writer, Query query, StoredColumn column, StringBuilder stbCommandPart)
        {
            ColumnAliasDefinition cdef = (ColumnAliasDefinition)column.ColumnDefinition;
            stbCommandPart.Append(cdef.Alias);
        }

        public virtual void WriteColumnSchemaSimple(BaseWriter writer, Query query, StoredColumn column, StringBuilder stbCommandPart)
        {
            stbCommandPart.Append(GetColumnName(column));
        }

        public virtual void WriteColumn(BaseWriter writer, Query query, StoredColumn column, StringBuilder stbCommandPart)
        {
            switch (column.Type)
            {
                case StoredColumn.StoredTypes.Name:
                    WriteColumnName(writer, query, column, stbCommandPart);
                    break;
                case StoredColumn.StoredTypes.OnlyAlias:
                    WriteColumnOnlyAlias(writer, query, column, stbCommandPart);
                    break;
                case StoredColumn.StoredTypes.Schema:
                    WriteColumnSchema(writer, query, column, stbCommandPart);
                    break;
                case StoredColumn.StoredTypes.StoredFunction:
                    WriteColumnStoredFunction(writer, query, column, stbCommandPart);
                    break;
                case StoredColumn.StoredTypes.UnknowTable:
                    WriteColumnUnknowTable(writer, query, column, stbCommandPart);
                    break;
                case StoredColumn.StoredTypes.Concat:
                    WriteColumnStoredConcat(writer, query, column, stbCommandPart);
                    break;
                case StoredColumn.StoredTypes.StringConstant:
                    WriteColumnStringConstant(writer, query, column, stbCommandPart);
                    break;
            }
        }

        public virtual void WriteColumnName(BaseWriter writer, Query query, StoredColumn column, StringBuilder stbCommandPart)
        {
            stbCommandPart.Append(GetColumnNameWithPrefix(column));
        }

        public virtual void WriteColumnOnlyAlias(BaseWriter writer, Query query, StoredColumn column, StringBuilder stbCommandPart)
        {
            ColumnAliasDefinition cdef = (ColumnAliasDefinition)column.ColumnDefinition;
            string prefix = string.Empty;
            if ((column.Table != null))
            {
                prefix = string.Concat(GetTableNameOrAlias(column.Table), Punctuation.DOT);
            }
            stbCommandPart.Append(prefix).Append(cdef.Alias);
        }

        public virtual void WriteColumnSchema(BaseWriter writer, Query query, StoredColumn column, StringBuilder stbCommandPart)
        {
            stbCommandPart.Append(GetColumnNameWithPrefix(column));
        }

        public virtual void WriteColumnStoredFunction(BaseWriter writer, Query query, StoredColumn column, StringBuilder stbCommandPart)
        {
            ColumnFunctionDefinition cdef = (ColumnFunctionDefinition)column.ColumnDefinition;
            WriteFunction(writer, query, cdef.StoredFunction, stbCommandPart);
        }

        public virtual void WriteColumnStoredConcat(BaseWriter writer, Query query, StoredColumn column, StringBuilder stbCommandPart)
        {
            ColumnConcatDefinition cdef = (ColumnConcatDefinition)column.ColumnDefinition;
            WriteConcat(writer, query, cdef.Concat, stbCommandPart);
        }

        public virtual void WriteColumnUnknowTable(BaseWriter writer, Query query, StoredColumn column, StringBuilder stbCommandPart)
        {
            stbCommandPart.Append(GetColumnName(column));
        }

        public virtual void WriteColumnStringConstant(BaseWriter writer, Query query, StoredColumn column, StringBuilder stbCommandPart)
        {
            ColumnConstantDefinition ccd = (column.ColumnDefinition as ColumnConstantDefinition);
            stbCommandPart.Append(ccd.Name);
        }

        public virtual void WriteConcat(BaseWriter writer, Query query, StoredConcat concat, StringBuilder stbCommandPart)
        {
            stbCommandPart.Append(Punctuation.PARENTHESIS_OPEN);
            for (int i = 0; i <= concat.Values.Count - 1; i++)
            {
                if (i != 0)
                {
                    stbCommandPart.Append(Punctuation.WHITE_SPACE).Append(this.ConcatOperator).Append(Punctuation.WHITE_SPACE);
                }

                StoredValue concatValue = concat.Values[i];
                this.WriteStoredValue(writer, query, concatValue, Prefix.CONCAT_PARAM_NAME, null, stbCommandPart);
            }
            stbCommandPart.Append(Punctuation.PARENTHESIS_CLOSE);

            if (!string.IsNullOrEmpty(concat.Alias))
            {
                stbCommandPart.Append(Punctuation.WHITE_SPACE).Append(DML.AS).Append(Punctuation.WHITE_SPACE).Append(concat.Alias);
            }
        }

        //    Public Overridable Sub WriteSelectConcatValue(ByVal command As OrmCommand, ByVal query As Query, ByVal stbColumns As StringBuilder, ByVal concat As Concat, ByVal concatValue As ConcatValue)
        //        If Not String.IsNullOrEmpty(concatValue.ConstantValue) Then
        //            stbColumns.Append(Punctuation.APOSTROPHE).Append(concatValue.ConstantValue).Append(Punctuation.APOSTROPHE)
        //        ElseIf concatValue.IsOpenParentesis Then
        //            stbColumns.Append(Punctuation.PARENTHESIS_OPEN)
        //        ElseIf concatValue.IsCloseParentesis Then
        //            stbColumns.Append(Punctuation.PARENTHESIS_CLOSE)
        //        ElseIf concatValue.Value IsNot Nothing Then
        //            stbColumns.Append(concatValue.Value)
        //        ElseIf concatValue.Aggregate IsNot Nothing Then
        //            Me.WriteSelectAggregated(command, query, Nothing, stbColumns, concatValue.Aggregate, QueryBuilder.FUNCTION_PARAMETER_PREFIX)
        //        ElseIf concatValue.Column IsNot Nothing Then
        //            stbColumns.Append(Me.GetColumnNameWithPrefix(concatValue.Column))
        //        Else
        //            Throw New OrmException(Messages.TypeNotExpected)
        //        End If
        //    End Sub

        #endregion


        public virtual void WriteSetColumnValue(BaseWriter writer, Query query, SetColumnValue setValue, StringBuilder stbCommandFirstPart, StringBuilder stbCommandSecondPart, SetColumnValue.SetValueType type)
        {
            StringBuilder columnName = new StringBuilder();
            WriteColumnSimple(writer, query, setValue, columnName);

            StringBuilder stbCommandPart = stbCommandFirstPart;
            if ((type == SetColumnValue.SetValueType.Parts))
            {
                stbCommandPart = stbCommandSecondPart;
                stbCommandFirstPart.Append(columnName);
            }
            else if ((type == SetColumnValue.SetValueType.Define))
            {
                stbCommandFirstPart.Append(columnName).Append(Punctuation.WHITE_SPACE).Append(Constants.Comparisons.EQUAL).Append(Punctuation.WHITE_SPACE);
            }

            ITypeConverter converter = GetConverterFromColumn(setValue);

            WriteStoredValue(writer, query, setValue.Value, columnName.ToString(), converter, stbCommandPart);
        }

        #region "Stored Value"

        public virtual void WriteStoredValue(BaseWriter writer, Query query, StoredValue stValue, string parameterName, ITypeConverter converter, StringBuilder stbCommandPart)
        {
            switch (stValue.Type)
            {
                case StoredValue.StoredTypes.Array:
                    WriteStoredValueArray(writer, query, stValue, parameterName, converter, stbCommandPart);
                    break;
                case StoredValue.StoredTypes.Interval:
                    WriteStoredValueInterval(writer, query, stValue, parameterName, converter, stbCommandPart);
                    break;
                case StoredValue.StoredTypes.IntervalStoredColumns:
                    WriteStoredValueIntervalStoredColumns(writer, query, stValue, parameterName, stbCommandPart);
                    break;
                case StoredValue.StoredTypes.IntervalStoredFunction:
                    WriteStoredValueIntervalStoredFunction(writer, query, stValue, parameterName, stbCommandPart);
                    break;
                case StoredValue.StoredTypes.ObjectValue:
                    WriteStoredValueObjectValue(writer, query, stValue, parameterName, converter, stbCommandPart);
                    break;
                case StoredValue.StoredTypes.StoredColumn:
                    WriteStoredValueStoredColumn(writer, query, stValue, parameterName, stbCommandPart);
                    break;
                case StoredValue.StoredTypes.StoredFunction:
                    WriteStoredValueStoredFunction(writer, query, stValue, parameterName, stbCommandPart);
                    break;
                case StoredValue.StoredTypes.SubQuery:
                    WriteStoredValueSubQuery(writer, query, stValue, parameterName, stbCommandPart);
                    break;
            }
        }

        public virtual void WriteStoredValueArray(BaseWriter writer, Query query, StoredValue stValue, string columnName, ITypeConverter converter, StringBuilder stbCommandPart)
        {
            ValueArrayDefinition svaValues = (ValueArrayDefinition)stValue.Value;
            stbCommandPart.Append(Punctuation.PARENTHESIS_OPEN);
            for (int index = 0; index <= svaValues.Values.Count - 1; index++)
            {
                if ((index > 0))
                {
                    stbCommandPart.Append(Punctuation.COMMA).Append(Punctuation.WHITE_SPACE);
                }

                object value = svaValues.Values[index];
                WriteObjectValue(writer, query, columnName, value, converter, stbCommandPart, stValue);
            }
            stbCommandPart.Append(Punctuation.PARENTHESIS_CLOSE);
        }

        public virtual void WriteStoredValueInterval(BaseWriter writer, Query query, StoredValue stValue, string columnName, ITypeConverter converter, StringBuilder stbCommandPart)
        {
            ValueIntervalDefinition sviValues = (ValueIntervalDefinition)stValue.Value;

            WriteObjectValue(writer, query, columnName.ToString(), sviValues.StartValue, converter, stbCommandPart, stValue);
            stbCommandPart.Append(Punctuation.WHITE_SPACE).Append(Constants.ConstraintTypes.AND).Append(Punctuation.WHITE_SPACE);
            WriteObjectValue(writer, query, columnName.ToString(), sviValues.FinalValue, converter, stbCommandPart, stValue);

        }

        public virtual void WriteStoredValueStoredColumn(BaseWriter writer, Query query, StoredValue stValue, string columnName, StringBuilder stbCommandPart)
        {
            ValueStoredColumnDefinition scvValue = (ValueStoredColumnDefinition)stValue.Value;
            WriteColumn(writer, query, scvValue.Value, stbCommandPart);
        }

        public virtual void WriteStoredValueStoredFunction(BaseWriter writer, Query query, StoredValue stValue, string columnName, StringBuilder stbCommandPart)
        {
            ValueStoredFunctionDefinition scfValue = (ValueStoredFunctionDefinition)stValue.Value;
            WriteFunction(writer, query, scfValue.Value, stbCommandPart);
        }

        public virtual void WriteStoredValueIntervalStoredColumns(BaseWriter writer, Query query, StoredValue stValue, string columnName, StringBuilder stbCommandPart)
        {
            ValueStoredColumnIntervalDefinition scivValue = (ValueStoredColumnIntervalDefinition)stValue.Value;

            WriteColumn(writer, query, scivValue.StartValue, stbCommandPart);
            stbCommandPart.Append(Punctuation.WHITE_SPACE).Append(Constants.ConstraintTypes.AND).Append(Punctuation.WHITE_SPACE);
            WriteColumn(writer, query, scivValue.FinalValue, stbCommandPart);
        }

        public virtual void WriteStoredValueIntervalStoredFunction(BaseWriter writer, Query query, StoredValue stValue, string columnName, StringBuilder stbCommandPart)
        {
            ValueStoredFunctionIntervalDefinition sfiValue = (ValueStoredFunctionIntervalDefinition)stValue.Value;

            WriteFunction(writer, query, sfiValue.StartValue, stbCommandPart);
            stbCommandPart.Append(Punctuation.WHITE_SPACE).Append(Constants.ConstraintTypes.AND).Append(Punctuation.WHITE_SPACE);
            WriteFunction(writer, query, sfiValue.FinalValue, stbCommandPart);
        }

        public virtual void WriteStoredValueObjectValue(BaseWriter writer, Query query, StoredValue stValue, string columnName, ITypeConverter converter, StringBuilder stbCommandPart)
        {
            ValueObjectDefinition svValue = (ValueObjectDefinition)stValue.Value;
            WriteObjectValue(writer, query, columnName, svValue.Value, converter, stbCommandPart, stValue);
        }

        public virtual void WriteStoredValueSubQuery(BaseWriter writer, Query query, StoredValue stValue, string columnName, StringBuilder stbCommandPart)
        {
            ValueSubQueryDefinition sqValue = (ValueSubQueryDefinition)stValue.Value;
            this.WriteSubQuery(writer, query, sqValue.Value, stbCommandPart);
        }

        public virtual void WriteObjectValue(BaseWriter writer, Query query, string columnName, object value, ITypeConverter converter, StringBuilder stbCommandPart, IGenParameter stParameter)
        {
            if (value is StringConstant)
            {
                StringConstant stcValue = value as StringConstant;
                if(stcValue.AddApostrophe)
                {
                    stbCommandPart.Append(Punctuation.APOSTROPHE).Append(((StringConstant)value).Constant).Append(Punctuation.APOSTROPHE);
                }
                else
                {
                    stbCommandPart.Append(((StringConstant)value).Constant);
                }
            }
            else
            {
                if (converter != null)
                    value = converter.ConvertTo(value);

                string parameterName = CreateParameter(writer.Command, columnName, query, value);
                stParameter.ParameterName = string.Concat(DataBasePrefixParameter, parameterName);
                stbCommandPart.Append(string.Concat(DataBasePrefixVariable, parameterName));
            }
        }

        private void SetObjectValue(ValueObjectDefinition valueDefinition, object newValue)
        {
            if (valueDefinition.Value is StringConstant)
            {
                valueDefinition.Value = new StringConstant(newValue.ToString());
            }
            else
            {
                valueDefinition.Value = newValue;
            }
        }

        #endregion

        private void WriteSubQuery(BaseWriter writer, Query query, Query subQuery, StringBuilder stbCommandPart)
        {
            stbCommandPart.AppendLine(Punctuation.PARENTHESIS_OPEN);

            SelectWriter subQueryWriter = new SelectWriter(writer.Command);
            WriteSelect(subQueryWriter, subQuery);
            stbCommandPart.Append(subQueryWriter.FinalString);
            stbCommandPart.AppendLine(Punctuation.PARENTHESIS_CLOSE);
        }

        #region "Select StoredTable"


        public virtual void WriteSelectTables(SelectWriter writer, Query query)
        {
            for (int index = 0; index <= query.Tables.Count - 1; index++)
            {
                if ((index > 0))
                {
                    writer.Tables.AppendLine(Punctuation.COMMA);
                }

                StoredTable table = query.Tables[index];
                WriteSelectTable(writer, query, table);
            }
        }

        public virtual void WriteSelectTable(SelectWriter writer, Query query, StoredTable table)
        {
            switch (table.Type)
            {
                case StoredTable.StoredTypes.Schema:
                case StoredTable.StoredTypes.Name:
                    writer.Tables.Append(this.GetTableNameWithDefs(table));
                    break;
                case StoredTable.StoredTypes.SubQuery:
                    WriteSubQuery(writer, query, (table.TableDefinition as TableSubQueryDefinition).SubQuery, writer.Tables);
                    writer.Tables.Append(Punctuation.WHITE_SPACE).Append(DML.AS).Append(Punctuation.WHITE_SPACE)
                        .Append(this.GetTableNameOrAlias(table));
                    break;
                case StoredTable.StoredTypes.StoredFunction:
                    writer.Tables.Append(Punctuation.PARENTHESIS_OPEN);
                    WriteFunction(writer, query, (table.TableDefinition as TableFunctionDefinition).StoredFunction, writer.Tables);
                    writer.Tables.Append(Punctuation.PARENTHESIS_CLOSE).Append(Punctuation.WHITE_SPACE).Append(DML.AS).Append(Punctuation.WHITE_SPACE)
                        .Append(this.GetTableNameOrAlias(table));
                    break;
            }
        }

        #endregion

        #region "StoredJoin"

        public virtual void WriteSelectJoins(SelectWriter writer, Query query)
        {
            foreach (StoredJoin join in query.Joins)
            {
                WriteSelectJoin(writer, query, join);
            }
        }

        public virtual void WriteSelectJoin(SelectWriter writer, Query query, StoredJoin join)
        {

            writer.Joins.Append(this.GetJoinType(join));

            switch (join.Type)
            {
                case StoredTable.StoredTypes.Schema:
                case StoredTable.StoredTypes.Name:
                    writer.Joins.Append(Punctuation.WHITE_SPACE).Append(this.GetTableNameWithDefs(join));
                    break;
                case StoredTable.StoredTypes.SubQuery:
                    writer.Joins.Append(Punctuation.WHITE_SPACE);
                    WriteSubQuery(writer, query, (join.TableDefinition as TableSubQueryDefinition).SubQuery, writer.Joins);
                    writer.Joins.Append(Punctuation.WHITE_SPACE).Append(DML.AS)
                        .Append(Punctuation.WHITE_SPACE).Append(this.GetTableNameOrAlias(join));
                    break;
                case StoredTable.StoredTypes.StoredFunction:
                    writer.Joins.Append(Punctuation.WHITE_SPACE).Append(Punctuation.PARENTHESIS_OPEN);
                    WriteFunction(writer, query, (join.TableDefinition as TableFunctionDefinition).StoredFunction, writer.Joins);
                    writer.Joins.Append(Punctuation.PARENTHESIS_CLOSE).Append(Punctuation.WHITE_SPACE).Append(DML.AS)
                        .Append(Punctuation.WHITE_SPACE).Append(this.GetTableNameOrAlias(join));
                    break;
            }

            this.WriteSelectJoinConditions(writer, query, join);

            writer.Joins.AppendLine();

        }

        public virtual void WriteSelectJoinConditions(SelectWriter writer, Query query, StoredJoin join)
        {
            foreach (StoredJoinConstraint condition in join.Conditions)
            {
                this.WriteSelectJoinCondition(writer, query, join, condition);
            }
        }

        #endregion

        #region "OrderBy and GroupBy"

        public virtual void WriteGroupBys(SelectWriter writer, Query query)
        {
            for (int index = 0; index <= query.GroupBys.Count - 1; index++)
            {
                if ((index > 0))
                {
                    writer.GroupBys.Append(Punctuation.COMMA).Append(Punctuation.WHITE_SPACE);
                }

                StoredGroupBy groupBy = query.GroupBys[index];
                WriteGroupBy(writer, query, groupBy);
            }
        }

        public virtual void WriteGroupBy(SelectWriter writer, Query query, StoredGroupBy groupBy)
        {
            writer.GroupBys.Append(this.GetColumnNameWithPrefixOrAlias(groupBy));
        }

        public virtual void WriteOrderBys(SelectWriter writer, Query query)
        {
            for (int index = 0; index <= query.OrderBys.Count - 1; index++)
            {
                if ((index > 0))
                {
                    writer.OrderBys.Append(Punctuation.COMMA).Append(Punctuation.WHITE_SPACE);
                }

                StoredOrderBy orderBy = query.OrderBys[index];
                WriteOrderBy(writer, query, orderBy);
            }
        }

        public virtual void WriteOrderBy(SelectWriter writer, Query query, StoredOrderBy orderBy)
        {
            writer.OrderBys.Append(this.GetOrderColumnDeclaration(orderBy, query));
        }

        #endregion

        #region "Union"

        public virtual void WriteUnions(SelectWriter writer, Query query)
        {
            foreach (StoredUnion union in query.Unions)
            {
                this.WriteUnion(writer, query, union);
            }
        }

        public virtual void WriteUnion(SelectWriter writer, Query query, StoredUnion union)
        {
            writer.Unions.AppendLine().Append(union.IsUnionAll ? DML.UNION_ALL : DML.UNION).AppendLine();
            this.WriteSubQuery(writer, query, union.UnionQuery, writer.Unions);
        }

        #endregion

        #endregion

        #region "Function"

        public virtual void WriteFunction(BaseWriter writer, Query query, StoredFunction stFunction, StringBuilder stbCommandPart)
        {
            if ((string.IsNullOrEmpty(stFunction.Owner)))
            {
                stbCommandPart.Append(stFunction.Name);
            }
            else
            {
                stbCommandPart.Append(stFunction.Owner).Append(Punctuation.DOT).Append(stFunction.Name);
            }
            stbCommandPart.Append(Punctuation.PARENTHESIS_OPEN);

            for (int index = 0; index <= stFunction.SetValues.Count - 1; index++)
            {
                if ((index > 0))
                {
                    stbCommandPart.Append(Punctuation.COMMA).Append(Punctuation.WHITE_SPACE);
                }

                SetColumnValue setValue = stFunction.SetValues[index];
                WriteSetColumnValue(writer, query, setValue, stbCommandPart, null, stFunction.SetValues.Type);
            }

            stbCommandPart.Append(Punctuation.PARENTHESIS_CLOSE);
        }


        //    Public Overridable Sub WriteSelectAggregateds(ByVal command As OrmCommand, ByVal query As Query, ByVal table As TableQuery, ByVal stbColumns As StringBuilder)
        //        Dim prefix As String = table.ColumnPrefix
        //        For Each aggregated As Aggregate In table.Aggregateds
        //            Me.WriteSelectAggregated(command, query, table, stbColumns, aggregated, prefix)
        //            stbColumns.Append(Punctuation.COMMA).Append(Punctuation.WHITE_SPACE)
        //        Next
        //    End Sub

        //    Public Overridable Sub WriteSelectAggregated(ByVal command As OrmCommand, ByVal query As Query, ByVal table As TableQuery, ByVal stbColumns As StringBuilder, ByVal aggregated As Aggregate, ByVal columnPrefix As String)
        //        If aggregated.AggregateFunction = AggregateFunction.ConvertF Then
        //            Me.WriteSelectConvert(command, query, table, stbColumns, aggregated, columnPrefix)
        //        ElseIf aggregated.AggregateFunction = AggregateFunction.Cast Then
        //            Me.WriteSelectCast(command, query, table, stbColumns, aggregated, columnPrefix)
        //        Else
        //            Select Case aggregated.AggregateFunction
        //                Case AggregateFunction.Count
        //                    stbColumns.Append(CFunctions.C_COUNT)
        //                    Exit Select
        //                Case AggregateFunction.Sum
        //                    stbColumns.Append(CFunctions.C_SUM)
        //                    Exit Select
        //                Case AggregateFunction.Avg
        //                    stbColumns.Append(CFunctions.C_AVG)
        //                    Exit Select
        //                Case AggregateFunction.Min
        //                    stbColumns.Append(CFunctions.C_MIN)
        //                    Exit Select
        //                Case AggregateFunction.Max
        //                    stbColumns.Append(CFunctions.C_MAX)
        //                    Exit Select
        //                Case AggregateFunction.StDev
        //                    stbColumns.Append(CFunctions.C_STDEV)
        //                    Exit Select
        //                Case AggregateFunction.StDevP
        //                    stbColumns.Append(CFunctions.C_STDEVP)
        //                    Exit Select
        //                Case AggregateFunction.Var
        //                    stbColumns.Append(CFunctions.C_VAR)
        //                    Exit Select
        //                Case AggregateFunction.VarP
        //                    stbColumns.Append(CFunctions.C_VARP)
        //                    Exit Select
        //                Case Else
        //                    Exit Select
        //            End Select

        //            stbColumns.Append(Punctuation.PARENTHESIS_OPEN)
        //            Me.WriteSelectAggregatedValue(command, query, table, stbColumns, aggregated, columnPrefix)
        //            stbColumns.Append(Punctuation.PARENTHESIS_CLOSE)
        //        End If

        //        If Not String.IsNullOrEmpty(aggregated.Alias) Then
        //            stbColumns.Append(Punctuation.WHITE_SPACE).Append(DML.AS).Append(Punctuation.WHITE_SPACE).Append(aggregated.Alias)
        //        End If
        //    End Sub

        //    Public Overridable Sub WriteSelectAggregatedValue(ByVal command As OrmCommand, ByVal query As Query, ByVal table As TableQuery, ByVal stbColumns As StringBuilder, ByVal aggregated As Aggregate, ByVal columnPrefix As String)
        //        If aggregated.Column IsNot Nothing Then
        //            stbColumns.Append(columnPrefix)
        //            stbColumns.Append(Punctuation.DOT)
        //            stbColumns.Append(aggregated.Column.PatternColumnName)
        //        ElseIf aggregated.Parameter IsNot Nothing Then
        //            If TypeOf aggregated.Parameter Is String Then
        //                If "*".Equals(aggregated.Parameter) Then
        //                    stbColumns.Append(aggregated.Parameter)
        //                Else
        //                    stbColumns.Append(Punctuation.APOSTROPHE).Append(aggregated.Parameter).Append(Punctuation.APOSTROPHE)
        //                End If
        //            ElseIf TypeOf aggregated.Parameter Is Integer Then
        //                stbColumns.Append(aggregated.Parameter)
        //            Else
        //                stbColumns.Append(Me.CreateParameter(command, String.Concat(FUNCTION_PARAMETER_PREFIX, INDEX_PREFIX, query.Index, LEVEL_PREFIX, query.Level), aggregated.Parameter))

        //                query.Index += 1
        //            End If
        //        Else
        //            Me.WriteSelectAggregated(command, query, table, stbColumns, aggregated.SubAggregate, columnPrefix)
        //        End If
        //    End Sub

        //    Public MustOverride Sub WriteSelectConvert(ByVal command As OrmCommand, ByVal query As Query, ByVal table As TableQuery, ByVal stbColumns As StringBuilder, ByVal aggregated As Aggregate, ByVal columnPrefix As String)

        //    Public MustOverride Sub WriteSelectCast(ByVal command As OrmCommand, ByVal query As Query, ByVal table As TableQuery, ByVal stbColumns As StringBuilder, ByVal aggregated As Aggregate, ByVal columnPrefix As String)

        #endregion

        #region "Constraints"

        public virtual void WriteSelectJoinCondition(SelectWriter writer, Query query, StoredJoin join, StoredJoinConstraint condition)
        {
            writer.Joins.Append(Punctuation.WHITE_SPACE);

            WriteConstraint<StoredJoin>(writer, query, condition, writer.Joins);
        }

        public virtual void WriteConstraints(BaseWriter writer, Query query, StringBuilder stbCommandPart)
        {
            foreach (StoredConstraint constraint in query.Constraints)
            {
                WriteConstraint<Query>(writer, query, constraint, stbCommandPart);
            }
        }

        public virtual void WriteConstraint<T>(BaseWriter writer, Query query, ConstraintBase<T> constraint, StringBuilder stbCommandPart) where T : IGetQuery
        {
            //Where, On, And, Or
            if ((object.ReferenceEquals(typeof(T), typeof(StoredJoin))))
            {
                WriteJoinConstraintType(writer, query, constraint, stbCommandPart);
            }
            else
            {
                WriteConstraintType(writer, query, constraint, stbCommandPart);
            }


            if ((constraint.Comparison == ComparisonOperator.OpenParentheses || constraint.Comparison == ComparisonOperator.CloseParentheses))
            {
                //=, <>, >, <, <=, >=
                WriteConstraintComparison(writer, query, constraint, stbCommandPart);
            }
            else
            {
                ITypeConverter converter = GetConverterFromColumn(constraint.Column);
                //Column Name
                WriteColumn(writer, query, constraint.Column, stbCommandPart);
                stbCommandPart.Append(Punctuation.WHITE_SPACE);

                //=, <>, >, <, <=, >=
                WriteConstraintComparison(writer, query, constraint, stbCommandPart);

                //Value
                if (constraint.Comparison != ComparisonOperator.IsNotNull && constraint.Comparison != ComparisonOperator.IsNull)
                {
                    WriteStoredValue(writer, query, constraint.Value, GetColumnName(constraint.Column), converter, stbCommandPart);
                    constraint.Column.ParameterName = constraint.Value.ParameterName;
                }
            }

            stbCommandPart.AppendLine();

        }

        public ITypeConverter GetConverterFromColumn(StoredColumn storedColumn)
        {
            if(storedColumn.Type == StoredColumn.StoredTypes.Schema)
            {
                ColumnSchema schema = storedColumn.ColumnDefinition as ColumnSchema;
                return schema.Converter;
            }
            return null;
        }

        public virtual void WriteConstraintType(BaseWriter writer, Query query, ConstraintBase constraint, StringBuilder stbCommandPart)
        {
            switch (constraint.Type)
            {
                case ConstraintBase.ConstraintType.Where:
                    stbCommandPart.Append(ConstraintTypes.WHERE).Append(Punctuation.WHITE_SPACE);
                    break;
                case ConstraintBase.ConstraintType.And:
                    stbCommandPart.Append(ConstraintTypes.AND).Append(Punctuation.WHITE_SPACE);
                    break;
                case ConstraintBase.ConstraintType.Or:
                    stbCommandPart.Append(ConstraintTypes.OR).Append(Punctuation.WHITE_SPACE);
                    break;
            }
        }

        public virtual void WriteJoinConstraintType(BaseWriter writer, Query query, ConstraintBase constraint, StringBuilder stbCommandPart)
        {
            switch (constraint.Type)
            {
                case ConstraintBase.ConstraintType.Where:
                    stbCommandPart.Append(ConstraintTypes.ON).Append(Punctuation.WHITE_SPACE);
                    break;
                case ConstraintBase.ConstraintType.And:
                    stbCommandPart.Append(ConstraintTypes.AND).Append(Punctuation.WHITE_SPACE);
                    break;
                case ConstraintBase.ConstraintType.Or:
                    stbCommandPart.Append(ConstraintTypes.OR).Append(Punctuation.WHITE_SPACE);
                    break;
            }
        }

        public virtual void WriteConstraintComparison(BaseWriter writer, Query query, ConstraintBase constraint, StringBuilder stbCommandPart)
        {
            switch (constraint.Comparison)
            {
                case ComparisonOperator.Equal:
                    stbCommandPart.Append(Comparisons.EQUAL).Append(Punctuation.WHITE_SPACE);
                    break;
                case ComparisonOperator.Different:
                    stbCommandPart.Append(Comparisons.DIFFERENT).Append(Punctuation.WHITE_SPACE);
                    break;
                case ComparisonOperator.GreaterThan:
                    stbCommandPart.Append(Comparisons.GREATER_THAN).Append(Punctuation.WHITE_SPACE);
                    break;
                case ComparisonOperator.LessThan:
                    stbCommandPart.Append(Comparisons.LESS_THAN).Append(Punctuation.WHITE_SPACE);
                    break;
                case ComparisonOperator.GreaterThanOrEqual:
                    stbCommandPart.Append(Comparisons.GREATER_THAN_OR_EQUAL).Append(Punctuation.WHITE_SPACE);
                    break;
                case ComparisonOperator.LessThanOrEqual:
                    stbCommandPart.Append(Comparisons.LESS_THAN_OR_EQUAL).Append(Punctuation.WHITE_SPACE);
                    break;
                case ComparisonOperator.Between:
                    stbCommandPart.Append(Comparisons.BETWEEN).Append(Punctuation.WHITE_SPACE);
                    break;
                case ComparisonOperator.Like:
                    stbCommandPart.Append(Comparisons.LIKE).Append(Punctuation.WHITE_SPACE);
                    break;
                case ComparisonOperator.NotLike:
                    stbCommandPart.Append(Comparisons.NOT_LIKE).Append(Punctuation.WHITE_SPACE);
                    break;
                case ComparisonOperator.In:
                    stbCommandPart.Append(Comparisons.IN);
                    break;
                case ComparisonOperator.NotIn:
                    stbCommandPart.Append(Comparisons.NOT_IN);
                    break;
                case ComparisonOperator.IsNull:
                    stbCommandPart.Append(Comparisons.IS_NULL).Append(Punctuation.WHITE_SPACE);
                    break;
                case ComparisonOperator.IsNotNull:
                    stbCommandPart.Append(Comparisons.IS_NOT_NULL).Append(Punctuation.WHITE_SPACE);
                    break;
                case ComparisonOperator.OpenParentheses:
                    stbCommandPart.Append(Punctuation.PARENTHESIS_OPEN);
                    this.BlockWriteConstraintType = true;
                    break;
                case ComparisonOperator.CloseParentheses:
                    stbCommandPart.Append(Punctuation.PARENTHESIS_CLOSE);
                    break;
                case ComparisonOperator.StartsWith:
                    stbCommandPart.Append(Comparisons.LIKE).Append(Punctuation.WHITE_SPACE);
                    if ((constraint.Value.Type == StoredValue.StoredTypes.ObjectValue))
                    {
                        ValueObjectDefinition value = (ValueObjectDefinition)constraint.Value.Value;
                        SetObjectValue(value, string.Concat(value.Value.ToString(), "%"));
                    }
                    break;
                case ComparisonOperator.EndsWith:
                    stbCommandPart.Append(Comparisons.LIKE).Append(Punctuation.WHITE_SPACE);
                    if ((constraint.Value.Type == StoredValue.StoredTypes.ObjectValue))
                    {
                        ValueObjectDefinition value = (ValueObjectDefinition)constraint.Value.Value;
                        SetObjectValue(value, string.Concat("%", value.Value.ToString()));
                    }
                    break;
                case ComparisonOperator.Contains:
                    stbCommandPart.Append(Comparisons.LIKE).Append(Punctuation.WHITE_SPACE);
                    if ((constraint.Value.Type == StoredValue.StoredTypes.ObjectValue))
                    {
                        ValueObjectDefinition value = (ValueObjectDefinition)constraint.Value.Value;
                        SetObjectValue(value, string.Concat("%", value.Value.ToString(), "%"));
                    }
                    break;
                default:
                    throw new OrmException(Messages.InvalidOperator);
            }
        }

        #endregion

        #region "Insert"

        public virtual void WriteInsert(InsertWriter writer, Query query)
        {
            writer.IntoTable.Append(GetTableNameWithOwner(query.IntoTable));

            if ((query.SetValues.Count > 0))
            {
                WriterInsertValues(writer, query);
            }
            else
            {
                WriterInsertSelect(writer, query);
            }
            WriteInsertFinalString(writer, query);
        }

        public virtual void WriterInsertValues(InsertWriter writer, Query query)
        {
            for (int index = 0; index <= query.SetValues.Count - 1; index++)
            {
                if ((index > 0))
                {
                    writer.Columns.Append(Punctuation.COMMA).Append(Punctuation.WHITE_SPACE);
                    writer.Values.Append(Punctuation.COMMA).Append(Punctuation.WHITE_SPACE);
                }

                SetColumnValue value = query.SetValues[index];
                WriteSetColumnValue(writer, query, value, writer.Columns, writer.Values, query.SetValues.Type);
            }
        }

        public virtual void WriterInsertSelect(InsertWriter writer, Query query)
        {
            for (int index = 0; index <= query.Columns.Count - 1; index++)
            {
                if ((index > 0))
                {
                    writer.Columns.Append(Punctuation.COMMA).Append(Punctuation.WHITE_SPACE);
                }

                StoredColumn column = query.Columns[index];
                WriteColumn(writer, query, column, writer.Columns);
            }

            WriteSelect(writer.SelectW, query);
            writer.Values.Append(writer.SelectW.FinalString);
        }

        public virtual void WriteInsertFinalString(InsertWriter writer, Query query)
        {
            writer.FinalString.Append(DML.INSERT).Append(Punctuation.WHITE_SPACE).Append(writer.IntoTable.ToString()).Append(Punctuation.PARENTHESIS_OPEN).Append(writer.Columns.ToString()).Append(Punctuation.PARENTHESIS_CLOSE);

            if ((query.SetValues.Count > 0))
            {
                writer.FinalString.AppendLine(DML.VALUES).Append(Punctuation.PARENTHESIS_OPEN).Append(writer.Values.ToString()).Append(Punctuation.PARENTHESIS_CLOSE);
            }
            else
            {
                writer.FinalString.AppendLine(writer.Values.ToString());
            }

            writer.SetCommandText();
        }

        #endregion

        #region "Delete"

        public virtual void WriteDelete(DeleteWriter writer, Query query)
        {
            writer.Table.Append(GetTableNameWithOwner(query.IntoTable));
            WriteConstraints(writer, query, writer.Constraints);
            WriteDeleteFinalString(writer, query);
        }

        public virtual void WriteDeleteFinalString(DeleteWriter writer, Query query)
        {
            writer.FinalString.Append(DML.DELETE).Append(Punctuation.WHITE_SPACE).AppendLine(writer.Table.ToString()).AppendLine(writer.Constraints.ToString());

            writer.SetCommandText();
        }

        #endregion

        #region "Update"

        public virtual void WriteUpdate(UpdateWriter writer, Query query)
        {
            writer.IntoTable.Append(GetTableNameWithOwner(query.IntoTable));

            for (int index = 0; index <= query.SetValues.Count - 1; index++)
            {
                SetColumnValue setValue = query.SetValues[index];

                if (index > 0)
                {
                    writer.SetValues.Append(Punctuation.COMMA).Append(Punctuation.WHITE_SPACE);
                }

                WriteSetColumnValue(writer, query, setValue, writer.SetValues, null, query.SetValues.Type);
            }

            this.WriteConstraints(writer, query, writer.Constraints);

            this.WriteUpdateFinalString(writer, query);
        }

        public virtual void WriteUpdateFinalString(UpdateWriter writer, Query query)
        {
            writer.FinalString.Append(DML.UPDATE).Append(Punctuation.WHITE_SPACE).AppendLine(writer.IntoTable.ToString()).Append(DML.SET).Append(Punctuation.WHITE_SPACE);
            writer.FinalString.AppendLine(writer.SetValues.ToString());
            writer.FinalString.Append(writer.Constraints.ToString());
            writer.SetCommandText();
        }

        #endregion

        #region "Procedure"
        public virtual void WriteProcedure(BaseWriter writer, Query query)
        {
            writer.Command.CommandType = System.Data.CommandType.StoredProcedure;
            writer.Command.CommandText = this.GetProcedureName(query);

            foreach (SetColumnValue setValue in query.SetValues)
            {
                StringBuilder columnName = new StringBuilder();
                WriteColumnSimple(writer, query, setValue, columnName);
                this.CreateParameter(writer.Command, columnName.ToString(), setValue.Value.Value.GetValue());
            }
        }

        public virtual string GetProcedureName(Query query)
        {
            return GetTableNameWithOwner(query.IntoTable);
        }
        #endregion

        #region "General"

        public virtual string CreateParameter(OrmCommand command, string parameterName, object value)
        {
            System.Data.IDbDataParameter parameter = command.AddParameter();
            parameter.Direction = System.Data.ParameterDirection.Input;
            parameter.ParameterName = this.DataBasePrefixParameter + parameterName;

            if (value == null)
            {
                parameter.DbType = System.Data.DbType.AnsiString;
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.DbType = value.GetType().GetDbType();
                parameter.Value = value;
            }

            return parameterName;
        }

        public virtual string CreateParameter(OrmCommand command, string columnName, Query query, object value)
        {
            string parameterName = string.Concat(columnName, INDEX_PREFIX, query.NextIndex, LEVEL_PREFIX, query.Level);
            return CreateParameter(command, parameterName, value);
        }

        public virtual string GetColumnNameWithAlias(StoredColumn column)
        {
            if (column.HasAlias)
            {
                return string.Concat(column.ColumnDefinition.Name, Punctuation.WHITE_SPACE, DML.AS, Punctuation.WHITE_SPACE, column.Alias);
            }
            else
            {
                return column.ColumnDefinition.Name;
            }
        }

        public virtual string GetColumnNameWithAliasAndPrefix(StoredColumn column)
        {
            string prefix = string.Empty;
            if ((column.Table != null))
            {
                prefix = string.Concat(GetTableNameOrAlias(column.Table), Punctuation.DOT);
            }
            if (column.HasAlias)
            {
                return string.Concat(prefix, column.ColumnDefinition.Name, Punctuation.WHITE_SPACE, DML.AS, Punctuation.WHITE_SPACE, column.Alias);
            }
            else
            {
                return string.Concat(prefix, column.ColumnDefinition.Name);
            }
        }

        public virtual string GetColumnNameWithPrefixOrAlias(StoredColumn column)
        {
            if (column.HasAlias)
            {
                return column.Alias;
            }
            else
            {
                return GetColumnNameWithPrefix(column);
            }
        }

        public virtual string GetColumnName(StoredColumn column)
        {
            return column.ColumnDefinition.Name;
        }

        public virtual string GetColumnNameOrAlias(StoredColumn column)
        {
            if (column.HasAlias)
            {
                return column.Alias;
            }
            else
            {
                return column.ColumnDefinition.Name;
            }
        }

        public virtual string GetColumnNameWithPrefix(StoredColumn column)
        {
            if ((column.Table == null))
            {
                return column.ColumnDefinition.Name;
            }
            else
            {
                return string.Concat(GetTableNameOrAlias(column.Table), Punctuation.DOT, column.ColumnDefinition.Name);
            }
        }

        public virtual string GetOrderColumnDeclaration(StoredOrderBy order, Query query)
        {
            string sufix = string.Empty;
            switch (order.SortOrder)
            {
                case StoredOrderBy.Order.Asc:
                    sufix = string.Concat(Punctuation.WHITE_SPACE, DML.ASC);
                    break;
                case StoredOrderBy.Order.Desc:
                    sufix = string.Concat(Punctuation.WHITE_SPACE, DML.DESC);
                    break;
            }

            if (query.LimitRows != null)
            {
                string table = this.GetColumnPrefix(order);
                return string.Concat(table, this.GetColumnNameOrAlias(order), sufix);
            }
            else
            {
                return string.Concat(this.GetColumnNameWithPrefixOrAlias(order), sufix);
            }
        }

        private string GetColumnPrefix(StoredColumn column)
        {
            string table = string.Empty;
            if (column.Table != null)
            {
                table = column.Table.ColumnPrefix;
            }
            else if (column.ColumnDefinition.Table != null)
            {
                if (!string.IsNullOrEmpty(column.ColumnDefinition.Table.Alias))
                    table = column.ColumnDefinition.Table.Alias;
                else
                    table = column.ColumnDefinition.Table.Name;
            }
            else
            {
                return table;
            }
            table += Punctuation.DOT;
            return table;
        }

        public virtual string GetTableNameWithDefs(StoredTable table)
        {
            if (table.Parent.IsAutoNoLock || table.NoLock)
            {
                return string.Concat(this.GetTableNameWithAlias(table), Punctuation.WHITE_SPACE, DML.NOLOCK);
            }
            else
            {
                return this.GetTableNameWithAlias(table);
            }
        }

        public virtual string GetTableNameWithAlias(StoredTable table)
        {
            StringBuilder stbName = new StringBuilder().Append(table.TableDefinition.Name);

            if (!string.IsNullOrEmpty(table.Alias))
            {
                stbName.Append(Punctuation.WHITE_SPACE).Append(table.Alias);
            }

            return stbName.ToString();
        }

        public virtual string GetTableNameOrAlias(StoredTable table)
        {
            if (string.IsNullOrEmpty(table.Alias))
            {
                return table.TableDefinition.Name;
            }
            else
            {
                return table.Alias;
            }
        }

        public virtual string GetTableNameWithOwner(StoredTable table)
        {
            if (!string.IsNullOrEmpty(table.Owner))
            {
                return string.Concat(table.Owner, Punctuation.DOT, table.TableDefinition.Name);
            }
            else
            {
                return table.TableDefinition.Name;
            }
        }

        #endregion

    }

}
