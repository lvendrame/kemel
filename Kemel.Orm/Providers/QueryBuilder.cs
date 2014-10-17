using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Constants;
using Kemel.Orm.QueryDef;
using Kemel.Orm.Data;

namespace Kemel.Orm.Providers
{
    public abstract class QueryBuilder
    {
        public QueryBuilder(Provider parent)
        {
            this.Parent = parent;
        }

        public Provider Parent { get; private set; }

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
            get
            {
                return this.blnBlockWriteConstraintType;
            }
            set
            {
                this.blnBlockWriteConstraintType = value;
            }
        }

        #region Select

        public virtual string GetTableNameWithDefs(TableQuery tableQuery)
        {
            if (tableQuery.Parent.IsAutoNoLock || tableQuery.NoLock)
            {
                return string.Concat(this.GetTableNameWithAlias(tableQuery), Punctuation.WHITE_SPACE, DML.NOLOCK);
            }
            else
            {
                return this.GetTableNameWithAlias(tableQuery);
            }
        }

        public virtual string GetJoinType(QueryJoin join)
        {
            switch (join.Type)
            {
                case JoinType.Inner:
                    return DML.INNER_JOIN;
                case JoinType.Left:
                    return DML.LEFT_JOIN;
                case JoinType.Right:
                    return DML.RIGHT_JOIN;
                case JoinType.Full:
                    return DML.FULL_JOIN;
                case JoinType.LeftOuter:
                    return DML.LEFT_OUTER_JOIN;
                case JoinType.RightOuter:
                    return DML.RIGHT_OUTER_JOIN;
                default:
                    return DML.INNER_JOIN;
            }
        }

        public virtual void WriteSelect(OrmCommand command, Query query)
        {
            this.WriteSelect(command, query, new StringBuilder());
        }

        public virtual void WriteSelect(OrmCommand command, Query query, StringBuilder stbQuery)
        {
            StringBuilder stbTables = new StringBuilder();
            StringBuilder stbJoins = new StringBuilder();
            StringBuilder stbColumns = new StringBuilder();

            this.WriteStringColumns(command, query, stbColumns);

            this.WriteSelectTables(command, query, stbTables, stbColumns);

            this.WriteSelectJoins(command, query, stbJoins, stbColumns);

            this.WriteSelectConcats(command, query, stbColumns);

            stbQuery
                .Append(DML.SELECT).Append(Punctuation.WHITE_SPACE);

            this.WriteSelectDistinct(command, query, stbQuery);

            this.WriteSelectTop(command, query, stbQuery);

            stbQuery
                .AppendLine(stbColumns.ToString(0, stbColumns.Length - 2))
                .Append(DML.FROM).Append(Punctuation.WHITE_SPACE)
                .AppendLine(stbTables.ToString(0, stbTables.Length - 3))
                .Append(stbJoins.ToString());

            this.WriteConstraints(command, query, stbQuery);

            this.WriteGroupBys(command, query, stbQuery);

            this.WriteOrderBys(command, query, stbQuery);

            this.WriteUnions(command, query, stbQuery);

            command.CommandText = stbQuery.ToString();
        }

        public virtual void WriteSelectConcats(OrmCommand command, Query query, StringBuilder stbColumns)
        {
            foreach (Concat concat in query.Concats)
            {
                this.WriteSelectConcat(command, query, stbColumns, concat);
                stbColumns.Append(Punctuation.COMMA).Append(Punctuation.WHITE_SPACE);
            }
        }

        public virtual void WriteSelectConcat(OrmCommand command, Query query, StringBuilder stbColumns, Concat concat)
        {
            stbColumns.Append(Punctuation.PARENTHESIS_OPEN);
            for (int i = 0; i < concat.ConcatValues.Count; i++)
            {
                if (i != 0)
                    stbColumns.Append(Punctuation.WHITE_SPACE).Append(this.ConcatOperator).Append(Punctuation.WHITE_SPACE);

                ConcatValue concatValue = concat.ConcatValues[i];
                this.WriteSelectConcatValue(command, query, stbColumns, concat, concatValue);
            }
            stbColumns.Append(Punctuation.PARENTHESIS_CLOSE);

            if (!string.IsNullOrEmpty(concat.Alias))
            {
                stbColumns.Append(Punctuation.WHITE_SPACE).Append(DML.AS)
                    .Append(Punctuation.WHITE_SPACE).Append(concat.Alias);
            }
        }

        public virtual void WriteSelectConcatValue(OrmCommand command, Query query, StringBuilder stbColumns, Concat concat, ConcatValue concatValue)
        {
            if (!string.IsNullOrEmpty(concatValue.ConstantValue))
            {
                stbColumns.Append(Punctuation.APOSTROPHE)
                    .Append(concatValue.ConstantValue)
                    .Append(Punctuation.APOSTROPHE);
            }
            else if (concatValue.IsOpenParentesis)
            {
                stbColumns.Append(Punctuation.PARENTHESIS_OPEN);
            }
            else if (concatValue.IsCloseParentesis)
            {
                stbColumns.Append(Punctuation.PARENTHESIS_CLOSE);
            }
            else if (concatValue.Value != null)
            {
                stbColumns.Append(concatValue.Value);
            }
            else if (concatValue.Aggregate != null)
            {
                this.WriteSelectAggregated(command, query, null, stbColumns, concatValue.Aggregate, QueryBuilder.FUNCTION_PARAMETER_PREFIX);
            }
            else if (concatValue.Column != null)
            {
                stbColumns.Append(this.GetColumnNameWithPrefix(concatValue.Column));
            }
            else
            {
                throw new OrmException(Messages.TypeNotExpected);
            }
        }

        public virtual void WriteUnions(OrmCommand command, Query query, StringBuilder stbQuery)
        {
            foreach (Union union in query.Unions)
            {
                this.WriteUnion(command, query, stbQuery, union);
            }
        }

        public virtual void WriteUnion(OrmCommand command, Query query, StringBuilder stbQuery, Union union)
        {
            stbQuery.AppendLine()
                .Append(union.IsUnionAll ? DML.UNION_ALL : DML.UNION)
                .AppendLine();
            this.WriteSubQuery(command, query, stbQuery, union.UnionQuery);
            this.BlockWriteConstraintType = true;
        }

        public virtual void WriteStringColumns(OrmCommand command, Query query, StringBuilder stbColumns)
        {

            foreach (StringColumn column in query.StringColumns)
            {
                stbColumns.Append(column.ValueWithAlias)
                    .Append(Punctuation.COMMA).Append(Punctuation.WHITE_SPACE);
            }
        }

        public virtual void WriteSelectTop(OrmCommand command, Query query, StringBuilder stbQuery)
        {
            if (query.TopRecords > 0)
                stbQuery.Append(DML.TOP).Append(Punctuation.WHITE_SPACE).Append(query.TopRecords).Append(Punctuation.WHITE_SPACE);
        }

        public virtual void WriteSelectDistinct(OrmCommand command, Query query, StringBuilder stbQuery)
        {

            if (query.IsDistinct)
                stbQuery.Append(DML.DISTINCT).Append(Punctuation.WHITE_SPACE);
        }

        public virtual void WriteGroupBys(OrmCommand command, Query query, StringBuilder stbQuery)
        {
            if (query.GroupBys.Count > 0)
            {
                stbQuery.Append(DML.GROUP_BY).Append(Punctuation.WHITE_SPACE);

                for (int i = 0; i < query.GroupBys.Count; i++)
                {
                    if (i > 0)
                        stbQuery.Append(Punctuation.COMMA).Append(Punctuation.WHITE_SPACE);

                    stbQuery.Append(this.GetColumnNameWithPrefixOrAlias(query.GroupBys[i]));
                }

                stbQuery.Append(Punctuation.WHITE_SPACE);
            }
        }

        public virtual void WriteOrderBys(OrmCommand command, Query query, StringBuilder stbQuery)
        {
            if (query.OrderBys.Count > 0)
            {
                stbQuery.Append(Punctuation.WHITE_SPACE).Append(DML.ORDER_BY).Append(Punctuation.WHITE_SPACE);

                for (int i = 0; i < query.OrderBys.Count; i++)
                {
                    if (i > 0)
                        stbQuery.Append(Punctuation.COMMA).Append(Punctuation.WHITE_SPACE);

                    stbQuery.Append(this.GetOrderColumnDeclaration(query.OrderBys[i]));
                }

                stbQuery.Append(Punctuation.WHITE_SPACE);
            }
        }

        public virtual void WriteSelectTables(OrmCommand command, Query query, StringBuilder stbTables, StringBuilder stbColumns)
        {
            foreach (TableQuery table in query.Tables)
            {
                stbTables.Append(this.GetTableNameWithDefs(table))
                    .AppendLine(Punctuation.COMMA);

                this.WriteSelectColumns(command, query, table, stbColumns);

                this.WriteSelectAggregateds(command, query, table, stbColumns);
            }
        }

        public virtual void WriteSelectColumns(OrmCommand command, Query query, TableQuery table, StringBuilder stbColumns)
        {
            string prefix = table.ColumnPrefix;
            foreach (ColumnQuery column in table.ColumnsQuery)
            {
                stbColumns.Append(prefix).Append(Punctuation.DOT).Append(this.GetColumnNameWithAlias(column))
                    .Append(Punctuation.COMMA).Append(Punctuation.WHITE_SPACE);
            }
        }

        public virtual void WriteSelectAggregateds(OrmCommand command, Query query, TableQuery table, StringBuilder stbColumns)
        {
            string prefix = table.ColumnPrefix;
            foreach (Aggregate aggregated in table.Aggregateds)
            {
                this.WriteSelectAggregated(command, query, table, stbColumns, aggregated, prefix);
                stbColumns.Append(Punctuation.COMMA).Append(Punctuation.WHITE_SPACE);
            }
        }

        public virtual void WriteSelectAggregated(OrmCommand command, Query query, TableQuery table, StringBuilder stbColumns, Aggregate aggregated, string columnPrefix)
        {
            if (aggregated.Function == AggregateFunction.Convert)
            {
                this.WriteSelectConvert(command, query, table, stbColumns, aggregated, columnPrefix);
            }
            else if (aggregated.Function == AggregateFunction.Cast)
            {
                this.WriteSelectCast(command, query, table, stbColumns, aggregated, columnPrefix);
            }
            else
            {
                switch (aggregated.Function)
                {
                    case AggregateFunction.Count:
                        stbColumns.Append(Functions.COUNT);
                        break;
                    case AggregateFunction.Sum:
                        stbColumns.Append(Functions.SUM);
                        break;
                    case AggregateFunction.Avg:
                        stbColumns.Append(Functions.AVG);
                        break;
                    case AggregateFunction.Min:
                        stbColumns.Append(Functions.MIN);
                        break;
                    case AggregateFunction.Max:
                        stbColumns.Append(Functions.MAX);
                        break;
                    case AggregateFunction.StDev:
                        stbColumns.Append(Functions.STDEV);
                        break;
                    case AggregateFunction.StDevP:
                        stbColumns.Append(Functions.STDEVP);
                        break;
                    case AggregateFunction.Var:
                        stbColumns.Append(Functions.VAR);
                        break;
                    case AggregateFunction.VarP:
                        stbColumns.Append(Functions.VARP);
                        break;
                    default:
                        break;
                }

                stbColumns.Append(Punctuation.PARENTHESIS_OPEN);
                this.WriteSelectAggregatedValue(command, query, table, stbColumns, aggregated, columnPrefix);
                stbColumns.Append(Punctuation.PARENTHESIS_CLOSE);
            }

            if (!string.IsNullOrEmpty(aggregated.Alias))
            {
                stbColumns.Append(Punctuation.WHITE_SPACE).Append(DML.AS).Append(Punctuation.WHITE_SPACE)
                    .Append(aggregated.Alias);
            }
        }

        public virtual void WriteSelectAggregatedValue(OrmCommand command, Query query, TableQuery table, StringBuilder stbColumns, Aggregate aggregated, string columnPrefix)
        {
            if (aggregated.Column != null)
            {
                stbColumns.Append(columnPrefix);
                stbColumns.Append(Punctuation.DOT);
                stbColumns.Append(aggregated.Column.PatternColumnName);
            }
            else if (aggregated.Parameter != null)
            {
                if (aggregated.Parameter is string)
                {
                    if ("*".Equals(aggregated.Parameter))
                    {
                        stbColumns.Append(aggregated.Parameter);
                    }
                    else
                    {
                        stbColumns.Append(Punctuation.APOSTROPHE)
                            .Append(aggregated.Parameter)
                            .Append(Punctuation.APOSTROPHE);
                    }
                }
                else if (aggregated.Parameter is int)
                {
                    stbColumns.Append(aggregated.Parameter);
                }
                else
                {
                    stbColumns.Append(this.CreateParameter(command,
                        string.Concat(FUNCTION_PARAMETER_PREFIX, INDEX_PREFIX, query.Index, LEVEL_PREFIX, query.Level),
                        aggregated.Parameter));

                    query.Index++;
                }
            }
            else
            {
                this.WriteSelectAggregated(command, query, table, stbColumns, aggregated.SubAggregate, columnPrefix);
            }
        }

        public abstract void WriteSelectConvert(OrmCommand command, Query query, TableQuery table, StringBuilder stbColumns, Aggregate aggregated, string columnPrefix);

        public abstract void WriteSelectCast(OrmCommand command, Query query, TableQuery table, StringBuilder stbColumns, Aggregate aggregated, string columnPrefix);

        public virtual void WriteSelectJoins(OrmCommand command, Query query, StringBuilder stbJoins, StringBuilder stbColumns)
        {
            foreach (QueryJoin join in query.Joins)
            {
                stbJoins
                    .Append(this.GetJoinType(join))
                    .Append(Punctuation.WHITE_SPACE)
                    .Append(this.GetTableNameWithDefs(join));

                this.WriteSelectColumns(command, query, join, stbColumns);

                this.WriteSelectAggregateds(command, query, join, stbColumns);

                this.WriteSelectJoinConditions(command, query, join, stbJoins);

                stbJoins.AppendLine();
            }
        }

        public virtual void WriteSelectJoinConditions(OrmCommand command, Query query, QueryJoin join, StringBuilder stbJoins)
        {
            foreach (JoinCondition condition in join.Conditions)
            {
                this.WriteSelectJoinCondition(command, query, condition, stbJoins);
            }
        }

        public virtual void WriteSelectJoinCondition(OrmCommand command, Query query, JoinCondition condition, StringBuilder stbJoins)
        {
            stbJoins.Append(Punctuation.WHITE_SPACE);

            //On, And, Or
            this.WriteSelectJoinConditionConstraint(command, query, condition, stbJoins);

            //Column Name
            this.WriteSelectJoinConditionColumn(command, query, condition.ColumnFrom, stbJoins);

            //=, <>, >, <, <=, >=
            this.WriteSelectJoinConditionOperator(command, query, condition, stbJoins);

            //Column Name
            if (condition.Value == null)
                this.WriteSelectJoinConditionColumn(command, query, condition.ColumnTo, stbJoins);
            else
                this.WriteSelectJoinConditionValue(command, query, condition, stbJoins);
        }

        public virtual void WriteSelectJoinConditionOperator(OrmCommand command, Query query, JoinCondition condition, StringBuilder stbJoins)
        {
            switch (condition.Operator)
            {
                case ComparisonOperator.Equal:
                    stbJoins.Append(Comparisons.EQUAL).Append(Punctuation.WHITE_SPACE);
                    break;
                case ComparisonOperator.Different:
                    stbJoins.Append(Comparisons.DIFFERENT).Append(Punctuation.WHITE_SPACE);
                    break;
                case ComparisonOperator.GreaterThan:
                    stbJoins.Append(Comparisons.GREATER_THAN).Append(Punctuation.WHITE_SPACE);
                    break;
                case ComparisonOperator.LessThan:
                    stbJoins.Append(Comparisons.LESS_THAN).Append(Punctuation.WHITE_SPACE);
                    break;
                case ComparisonOperator.GreaterThanOrEqual:
                    stbJoins.Append(Comparisons.GREATER_THAN_OR_EQUAL).Append(Punctuation.WHITE_SPACE);
                    break;
                case ComparisonOperator.LessThanOrEqual:
                    stbJoins.Append(Comparisons.LESS_THAN_OR_EQUAL).Append(Punctuation.WHITE_SPACE);
                    break;
                case ComparisonOperator.IsNull:
                    stbJoins.Append(Comparisons.IS_NULL).Append(Punctuation.WHITE_SPACE);
                    break;
                case ComparisonOperator.IsNotNull:
                    stbJoins.Append(Comparisons.IS_NOT_NULL).Append(Punctuation.WHITE_SPACE);
                    break;
                default:
                    throw new OrmException(Kemel.Orm.Messages.InvalidOperator);
            }
        }

        public virtual void WriteSelectJoinConditionConstraint(OrmCommand command, Query query, JoinCondition condition, StringBuilder stbJoins)
        {
            switch (condition.ConstraintType)
            {
                case ConstraintType.Where:
                    stbJoins.Append(ConstraintTypes.ON).Append(Punctuation.WHITE_SPACE);
                    break;
                case ConstraintType.And:
                    stbJoins.Append(ConstraintTypes.AND).Append(Punctuation.WHITE_SPACE);
                    break;
                case ConstraintType.Or:
                    stbJoins.Append(ConstraintTypes.OR).Append(Punctuation.WHITE_SPACE);
                    break;
            }
        }

        public virtual void WriteSelectJoinConditionColumn(OrmCommand command, Query query, ColumnQuery columnQuery, StringBuilder stbJoins)
        {
            stbJoins
                .Append(columnQuery.Parent.ColumnPrefix).Append(Punctuation.DOT).Append(columnQuery.PatternColumnName)
                .Append(Punctuation.WHITE_SPACE);
        }

        public virtual void WriteSelectJoinConditionValue(OrmCommand command, Query query, JoinCondition condition, StringBuilder stbJoins)
        {
            stbJoins
                .Append(this.DataBasePrefixVariable)
                .Append(this.CreateJoinParameter(command, query, condition))
                .Append(Punctuation.WHITE_SPACE);
        }

        public virtual void WriteSubQuery(OrmCommand command, Query query, StringBuilder stbQuery, Query subQuery)
        {
            subQuery.Level = query.NextLevel;

            this.BlockWriteConstraintType = true;
            stbQuery.AppendLine(Punctuation.PARENTHESIS_OPEN);
            this.WriteSelect(command, subQuery, stbQuery);
            stbQuery.AppendLine(Punctuation.PARENTHESIS_CLOSE);

            this.BlockWriteConstraintType = false;
        }

        public virtual string CreateJoinParameter(OrmCommand command, Query query, JoinCondition condition)
        {
            string parameterName = string.Concat(condition.ColumnFrom.PatternColumnName,
                INDEX_PREFIX, query.Index,
                LEVEL_PREFIX, query.Level);
            query.Index++;

            System.Data.IDbDataParameter parameter = command.AddParameter();
            parameter.Direction = System.Data.ParameterDirection.Input;
            parameter.ParameterName = this.DataBasePrefixParameter + parameterName;
            parameter.SourceColumn = condition.ColumnFrom.PatternColumnName;

            if (condition.Value == null)
            {
                parameter.DbType = System.Data.DbType.AnsiString;
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.DbType = condition.Value.GetType().GetDbType();
                parameter.Value = condition.Value;
            }

            if (condition.ColumnFrom != null)
                condition.ColumnFrom.ParameterName = parameterName;

            return parameterName;
        }

        #endregion

        #region Constraint

        public virtual void WriteConstraints(OrmCommand command, Query query, StringBuilder stbQuery)
        {
            if (query.Constraints.Count != 0)
                stbQuery.Append(DML.WHERE).Append(Punctuation.WHITE_SPACE);

            foreach (Constraint constraint in query.Constraints)
            {
                this.WriteConstraint(command, query, stbQuery, constraint);
            }

            this.BlockWriteConstraintType = true;
        }

        public virtual void WriteConstraint(OrmCommand command, Query query, StringBuilder stbQuery, Constraint constraint)
        {
            if (this.BlockWriteConstraintType)
            {
                this.BlockWriteConstraintType = false;
            }
            else if (constraint.Comparison != ComparisonOperator.CloseParentheses)
            {
                this.WriteConstraintType(command, query, stbQuery, constraint);
            }

            if (constraint.Comparison != ComparisonOperator.OpenParentheses && constraint.Comparison != ComparisonOperator.CloseParentheses)
            {
                if (constraint.Column != null)
                    this.WriteConstraintColumn(command, query, stbQuery, constraint.Column);
                else
                    stbQuery.Append(constraint.ConstantStringValue).Append(Punctuation.WHITE_SPACE);
            }

            this.WriteConstraintOperator(command, query, stbQuery, constraint);
        }

        public virtual void WriteConstraintType(OrmCommand command, Query query, StringBuilder stbQuery, Constraint constraint)
        {
            switch (constraint.Type)
            {
                case ConstraintType.Where:
                    stbQuery.Append(ConstraintTypes.WHERE).Append(Punctuation.WHITE_SPACE);
                    break;
                case ConstraintType.And:
                    stbQuery.Append(ConstraintTypes.AND).Append(Punctuation.WHITE_SPACE);
                    break;
                case ConstraintType.Or:
                    stbQuery.Append(ConstraintTypes.OR).Append(Punctuation.WHITE_SPACE);
                    break;
            }
        }

        public virtual void WriteConstraintColumn(OrmCommand command, Query query, StringBuilder stbQuery, ColumnQuery columnQuery)
        {
            stbQuery.Append(columnQuery.Parent.ColumnPrefix).Append(Punctuation.DOT).Append(columnQuery.PatternColumnName)
                .Append(Punctuation.WHITE_SPACE);
        }

        public virtual void WriteConstraintOperator(OrmCommand command, Query query, StringBuilder stbQuery, Constraint constraint)
        {
            switch (constraint.Comparison)
            {
                case ComparisonOperator.Equal:
                    stbQuery.Append(Comparisons.EQUAL).Append(Punctuation.WHITE_SPACE);
                    this.WriteConstraintSimpleValue(command, query, stbQuery, constraint);
                    break;
                case ComparisonOperator.Different:
                    stbQuery.Append(Comparisons.DIFFERENT).Append(Punctuation.WHITE_SPACE);
                    this.WriteConstraintSimpleValue(command, query, stbQuery, constraint);
                    break;
                case ComparisonOperator.GreaterThan:
                    stbQuery.Append(Comparisons.GREATER_THAN).Append(Punctuation.WHITE_SPACE);
                    this.WriteConstraintSimpleValue(command, query, stbQuery, constraint);
                    break;
                case ComparisonOperator.LessThan:
                    stbQuery.Append(Comparisons.LESS_THAN).Append(Punctuation.WHITE_SPACE);
                    this.WriteConstraintSimpleValue(command, query, stbQuery, constraint);
                    break;
                case ComparisonOperator.GreaterThanOrEqual:
                    stbQuery.Append(Comparisons.GREATER_THAN_OR_EQUAL).Append(Punctuation.WHITE_SPACE);
                    this.WriteConstraintSimpleValue(command, query, stbQuery, constraint);
                    break;
                case ComparisonOperator.LessThanOrEqual:
                    stbQuery.Append(Comparisons.LESS_THAN_OR_EQUAL).Append(Punctuation.WHITE_SPACE);
                    this.WriteConstraintSimpleValue(command, query, stbQuery, constraint);
                    break;
                case ComparisonOperator.Between:
                    stbQuery.Append(Comparisons.BETWEEN).Append(Punctuation.WHITE_SPACE);
                    this.WriteConstraintStartValueAndEndValue(command, query, stbQuery, constraint);
                    break;
                case ComparisonOperator.Like:
                    stbQuery.Append(Comparisons.LIKE).Append(Punctuation.WHITE_SPACE);
                    this.WriteConstraintSimpleValue(command, query, stbQuery, constraint);
                    break;
                case ComparisonOperator.NotLike:
                    stbQuery.Append(Comparisons.NOT_LIKE).Append(Punctuation.WHITE_SPACE);
                    this.WriteConstraintSimpleValue(command, query, stbQuery, constraint);
                    break;
                case ComparisonOperator.In:
                    stbQuery.Append(Comparisons.IN);
                    if (constraint.Function == null)
                        this.WriteConstraintValuesList(command, query, stbQuery, constraint);
                    else
                        this.WriteConstraintSimpleValue(command, query, stbQuery, constraint);
                    break;
                case ComparisonOperator.NotIn:
                    stbQuery.Append(Comparisons.NOT_IN);
                    this.WriteConstraintValuesList(command, query, stbQuery, constraint);
                    break;
                case ComparisonOperator.IsNull:
                    stbQuery.Append(Comparisons.IS_NULL).Append(Punctuation.WHITE_SPACE);
                    break;
                case ComparisonOperator.IsNotNull:
                    stbQuery.Append(Comparisons.IS_NOT_NULL).Append(Punctuation.WHITE_SPACE);
                    break;
                case ComparisonOperator.OpenParentheses:
                    stbQuery.Append(Punctuation.PARENTHESIS_OPEN);
                    this.BlockWriteConstraintType = true;
                    break;
                case ComparisonOperator.CloseParentheses:
                    stbQuery.Append(Punctuation.PARENTHESIS_CLOSE);
                    break;
                case ComparisonOperator.StartsWith:
                    stbQuery.Append(Comparisons.LIKE).Append(Punctuation.WHITE_SPACE);
                    constraint.Value = string.Concat("%", constraint.Value.ToString());
                    this.WriteConstraintSimpleValue(command, query, stbQuery, constraint);
                    break;
                case ComparisonOperator.EndsWith:
                    stbQuery.Append(Comparisons.LIKE).Append(Punctuation.WHITE_SPACE);
                    constraint.Value = string.Concat(constraint.Value.ToString(), "%");
                    this.WriteConstraintSimpleValue(command, query, stbQuery, constraint);
                    break;
                case ComparisonOperator.Contains:
                    stbQuery.Append(Comparisons.LIKE).Append(Punctuation.WHITE_SPACE);
                    constraint.Value = string.Concat("%", constraint.Value.ToString(), "%");
                    this.WriteConstraintSimpleValue(command, query, stbQuery, constraint);
                    break;
                default:
                    throw new OrmException(Messages.InvalidOperator);
            }
            stbQuery.AppendLine();
        }

        public virtual void WriteConstraintValuesList(OrmCommand command, Query query, StringBuilder stbQuery, Constraint constraint)
        {
            if (constraint.SubQuery != null)
            {
                this.WriteSubQuery(command, query, stbQuery, constraint.SubQuery);
            }
            else
            {
                stbQuery.Append(Punctuation.PARENTHESIS_OPEN);
                for (int i = 0; i < constraint.Values.Count; i++)
                {
                    if (i > 0)
                        stbQuery.Append(Punctuation.COMMA).Append(Punctuation.WHITE_SPACE);

                    stbQuery.Append(this.DataBasePrefixVariable)
                        .Append(
                            constraint.ParametersNames.AddItem(
                                this.CreateConstraintParameter(command, query, constraint, constraint.Values[i])
                            )
                        );
                }
                stbQuery.Append(Punctuation.PARENTHESIS_CLOSE);
            }
        }

        public virtual void WriteConstraintSimpleValue(OrmCommand command, Query query, StringBuilder stbQuery, Constraint constraint)
        {
            if (constraint.SubQuery != null)
            {
                this.WriteSubQuery(command, query, stbQuery, constraint.SubQuery);
            }
            else if (constraint.Function != null)
            {
                stbQuery.Append(Punctuation.PARENTHESIS_OPEN);
                this.WriteFunction(command, query, stbQuery, constraint.Function);
                stbQuery.Append(Punctuation.PARENTHESIS_CLOSE);
            }
            else if (constraint.ColumnValue != null)
            {
                stbQuery
                    .Append(constraint.ColumnValue.Parent.ColumnPrefix)
                    .Append(Punctuation.DOT)
                    .Append(constraint.ColumnValue.PatternColumnName);
            }
            else
            {
                constraint.StartParameterName = this.CreateConstraintParameter(command, query, constraint, constraint.Value);
                stbQuery.Append(this.DataBasePrefixVariable).Append(constraint.StartParameterName);
            }
        }

        public virtual void WriteFunction(OrmCommand command, Query query, StringBuilder stbQuery, Function function)
        {
            stbQuery.Append(function.FunctionName)
                .Append(Punctuation.PARENTHESIS_OPEN);

            for (int i = 0; i < function.SetValues.Count; i++)
            {
                SetFunctionValue setValue = function.SetValues[i];
                if (i > 0)
                    stbQuery.Append(Punctuation.COMMA)
                        .Append(Punctuation.WHITE_SPACE);

                stbQuery
                    .Append(this.DataBasePrefixVariable)
                    .Append(this.CreateSetParameter(command, query, setValue));
            }
            stbQuery.Append(Punctuation.PARENTHESIS_CLOSE);

            if (function.HasAlias)
            {
                stbQuery.Append(Punctuation.WHITE_SPACE)
                    .Append(DML.AS)
                    .Append(Punctuation.WHITE_SPACE)
                    .Append(function.Alias);
            }
            stbQuery.Append(Punctuation.WHITE_SPACE);
        }

        public virtual void WriteConstraintStartValueAndEndValue(OrmCommand command, Query query, StringBuilder stbQuery, Constraint constraint)
        {
            if (constraint.ColumnValue != null)
            {
                stbQuery
                    .Append(constraint.ColumnValue.Parent.ColumnPrefix)
                    .Append(Punctuation.DOT)
                    .Append(constraint.ColumnValue.PatternColumnName)
                        .Append(Punctuation.WHITE_SPACE)
                    .Append(ConstraintTypes.AND)
                        .Append(Punctuation.WHITE_SPACE)
                    .Append(constraint.ColumnEndValue.Parent.ColumnPrefix)
                    .Append(Punctuation.DOT)
                    .Append(constraint.ColumnEndValue.PatternColumnName);
            }
            else
            {
                constraint.StartParameterName = this.CreateConstraintParameter(command, query, constraint, constraint.Value);
                constraint.EndParameterName = this.CreateConstraintParameter(command, query, constraint, constraint.EndValue);

                stbQuery
                    .Append(this.DataBasePrefixVariable).Append(constraint.StartParameterName)
                        .Append(Punctuation.WHITE_SPACE)
                    .Append(ConstraintTypes.AND)
                        .Append(Punctuation.WHITE_SPACE)
                    .Append(this.DataBasePrefixVariable).Append(constraint.EndParameterName);
            }
        }

        public virtual string CreateConstraintParameter(OrmCommand command, Query query, Constraint constraint, object value)
        {
            string parameterName = null;

            if (constraint.Column != null)
            {
                parameterName = string.Concat(constraint.Column.PatternColumnName,
                    INDEX_PREFIX, query.Index,
                    LEVEL_PREFIX, query.Level);
            }
            else
            {
                parameterName = string.Concat(CONSTANT_STRING_PARAMETER_PREFIX,
                    INDEX_PREFIX, query.Index,
                    LEVEL_PREFIX, query.Level);
            }
            query.Index++;

            System.Data.IDbDataParameter parameter = command.AddParameter();
            parameter.Direction = System.Data.ParameterDirection.Input;
            parameter.ParameterName = this.DataBasePrefixParameter + parameterName;

            if (constraint.Column != null)
                parameter.SourceColumn = constraint.Column.PatternColumnName;

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

            if (constraint.Column != null)
                constraint.Column.ParameterName = parameterName;

            return parameterName;
        }

        #endregion

        #region Insert

        public virtual void WriteInsert(OrmCommand command, Query query)
        {
            StringBuilder stbInsert = new StringBuilder()
                .Append(DML.INSERT).Append(Punctuation.WHITE_SPACE)
                .Append(query.IntoTable.PatternName)
                .Append(Punctuation.PARENTHESIS_OPEN);

            StringBuilder stbValues = new StringBuilder()
                .Append(DML.VALUES).Append(Punctuation.PARENTHESIS_OPEN);

            for (int i = 0; i < query.SetValues.Count; i++)
            {
                SetValue setValue = query.SetValues[i];

                if (i > 0)
                {
                    stbInsert.Append(Punctuation.COMMA).Append(Punctuation.WHITE_SPACE);
                    stbValues.Append(Punctuation.COMMA).Append(Punctuation.WHITE_SPACE);
                }

                stbInsert.Append(setValue.PatternColumnName);
                stbValues.Append(this.DataBasePrefixVariable).Append(this.CreateSetParameter(command, query, setValue));
            }

            stbInsert.AppendLine(Punctuation.PARENTHESIS_CLOSE);
            stbValues.Append(Punctuation.PARENTHESIS_CLOSE);
            stbInsert.AppendLine(stbValues.ToString());

            command.CommandText = stbInsert.ToString();
        }

        public virtual string CreateSetParameter(OrmCommand command, Query query, SetValue setValue)
        {
            string parameterName = setValue.PatternColumnName;
            query.Index++;

            System.Data.IDbDataParameter parameter = command.AddParameter();
            parameter.Direction = System.Data.ParameterDirection.Input;
            parameter.ParameterName = this.DataBasePrefixParameter + parameterName;
            parameter.SourceColumn = setValue.PatternColumnName;

            if (setValue.Value == null)
            {
                parameter.DbType = System.Data.DbType.AnsiString;
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.DbType = setValue.Value.GetType().GetDbType();
                parameter.Value = setValue.Value;
            }

            setValue.ParameterName = parameterName;
            return parameterName;
        }

        public virtual string CreateSetParameter(OrmCommand command, Query query, SetFunctionValue setValue)
        {
            string parameterName = setValue.PatternColumnName;
            query.Index++;

            System.Data.IDbDataParameter parameter = command.AddParameter();
            parameter.Direction = System.Data.ParameterDirection.Input;
            parameter.ParameterName = this.DataBasePrefixParameter + parameterName;
            parameter.SourceColumn = setValue.PatternColumnName;

            if (setValue.Value == null)
            {
                parameter.DbType = System.Data.DbType.AnsiString;
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.DbType = setValue.Value.GetType().GetDbType();
                parameter.Value = setValue.Value;
            }

            setValue.ParameterName = parameterName;
            return parameterName;
        }

        #endregion

        #region Delete

        public virtual void WriteDelete(OrmCommand command, Query query)
        {
            StringBuilder stbDelete = new StringBuilder()
                .Append(DML.DELETE).Append(Punctuation.WHITE_SPACE)
                .AppendLine(query.IntoTable.PatternName);

            this.WriteConstraints(command, query, stbDelete);

            command.CommandText = stbDelete.ToString();
        }

        #endregion

        #region Update

        public virtual void WriteUpdate(OrmCommand command, Query query)
        {
            StringBuilder stbUpdate = new StringBuilder()
                .Append(DML.UPDATE).Append(Punctuation.WHITE_SPACE)
                .AppendLine(query.IntoTable.PatternName)
                .Append(DML.SET).Append(Punctuation.WHITE_SPACE);

            for (int i = 0; i < query.SetValues.Count; i++)
            {
                SetValue setValue = query.SetValues[i];

                if (i > 0)
                {
                    stbUpdate.Append(Punctuation.COMMA).Append(Punctuation.WHITE_SPACE);
                }

                stbUpdate.Append(setValue.PatternColumnName)
                        .Append(Punctuation.WHITE_SPACE)
                        .Append(Comparisons.EQUAL)
                        .Append(Punctuation.WHITE_SPACE)
                        .Append(this.DataBasePrefixVariable).Append(this.CreateSetParameter(command, query, setValue));
            }

            stbUpdate.AppendLine();

            this.WriteConstraints(command, query, stbUpdate);

            command.CommandText = stbUpdate.ToString();
        }

        #endregion

        #region Procedure
        public virtual void WriteProcedure(OrmCommand command, Query query)
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = query.ProcedureName;

            foreach (SetValue setValue in query.SetValues)
            {
                this.CreateSetParameter(command, query, setValue);
            }
        }
        #endregion

        #region General

        public virtual string CreateParameter(OrmCommand command, string parameterName, object value)
        {
            System.Data.IDbDataParameter parameter = command.AddParameter(parameterName);
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

        public virtual string GetColumnNameWithAlias(ColumnQuery column)
        {
            if (column.HasAlias)
                return string.Concat(column.PatternColumnName, Punctuation.WHITE_SPACE, DML.AS, Punctuation.WHITE_SPACE, column.Alias);
            else
                return column.PatternColumnName;
        }

        public virtual string GetColumnNameWithPrefixOrAlias(ColumnQuery column)
        {
            if (column.HasAlias)
                return column.Alias;
            else
                return string.Concat(column.Parent.ColumnPrefix, Punctuation.DOT, column.PatternColumnName);
        }

        public virtual string GetColumnNameWithPrefix(ColumnQuery column)
        {
            return string.Concat(column.Parent.ColumnPrefix, Punctuation.DOT, column.PatternColumnName);
        }

        public virtual string GetOrderColumnDeclaration(OrderBy order)
        {
            string sufix = string.Empty;
            switch (order.SortOrder)
            {
                case OrderBySortOrder.Asc:
                    sufix = string.Concat(Punctuation.WHITE_SPACE, DML.ASC);
                    break;
                case OrderBySortOrder.Desc:
                    sufix = string.Concat(Punctuation.WHITE_SPACE, DML.DESC);
                    break;
            }

            return string.Concat(this.GetColumnNameWithPrefixOrAlias(order), sufix);
        }

        public virtual string GetTableNameWithAlias(TableQuery table)
        {
            StringBuilder stbName = new StringBuilder()
                    .Append(table.PatternName);

            if (!string.IsNullOrEmpty(table.Alias))
                stbName
                    .Append(Punctuation.WHITE_SPACE)
                    .Append(table.Alias);

            return stbName.ToString();
        }

        #endregion
    }
}
