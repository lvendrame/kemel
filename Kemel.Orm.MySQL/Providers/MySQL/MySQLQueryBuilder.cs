using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Constants;
using Kemel.Orm.Providers;
using Kemel.Orm.NQuery.Builder;
using Kemel.Orm.NQuery.Storage.Table;

namespace Kemel.Orm.Providers.MySQL
{
    public class MySQLQueryBuilder : QueryBuilder
    {
        public MySQLQueryBuilder(Provider parent) : base(parent)
        {

        }

        public override string DataBasePrefixParameter
        {
            get { return string.Empty; }
        }

        public override string DataBasePrefixVariable
        {
            get { return "?"; }
        }

        public override string ConcatOperator
        {
            get { return ","; }
        }

        public const string ConcatCommand = "concat";

        //public override void WriteSelectConcat(Kemel.Orm.Data.OrmCommand command, Query query, StringBuilder stbColumns, Concat concat)
        //{
        //    stbColumns.Append(Punctuation.WHITE_SPACE).Append(ConcatCommand);
        //    stbColumns.Append(Punctuation.PARENTHESIS_OPEN);

        //    for (int i = 0; i < concat.ConcatValues.Count; i++)
        //    {
        //        if (i != 0)
        //        {
        //            stbColumns.Append(Punctuation.WHITE_SPACE).Append(this.ConcatOperator).Append(Punctuation.WHITE_SPACE);
        //        }

        //        ConcatValue concatValue = concat.ConcatValues[i];
        //        this.WriteSelectConcatValue(command, query, stbColumns, concat, concatValue);
        //    }

        //    stbColumns.Append(Punctuation.PARENTHESIS_CLOSE);

        //    if (!string.IsNullOrEmpty(concat.Alias))
        //    {
        //        stbColumns.Append(Punctuation.WHITE_SPACE).Append(DML.AS)
        //            .Append(Punctuation.WHITE_SPACE).Append(concat.Alias);
        //    }
        //}

        //public override void WriteSelectConvert(Kemel.Orm.Data.OrmCommand command,
        //    Kemel.Orm.QueryDef.Query query, Kemel.Orm.QueryDef.TableQuery table,
        //    StringBuilder stbColumns, Kemel.Orm.QueryDef.Aggregate aggregated, string columnPrefix)
        //{
        //    throw new NotImplementedException();
        //}

        //public override void WriteSelectCast(Kemel.Orm.Data.OrmCommand command,
        //    Kemel.Orm.QueryDef.Query query, Kemel.Orm.QueryDef.TableQuery table,
        //    StringBuilder stbColumns, Kemel.Orm.QueryDef.Aggregate aggregated, string columnPrefix)
        //{
        //    throw new NotImplementedException();
        //}

        //public override string GetColumnNameWithAlias(ColumnQuery column)
        //{
        //    if (column.HasAlias)
        //    {
        //        return string.Concat(column.PatternColumnName, Punctuation.WHITE_SPACE,
        //            DML.AS, Punctuation.WHITE_SPACE, Punctuation.QUOTATION_MARKS,
        //            column.Alias, Punctuation.QUOTATION_MARKS);
        //    }
        //    else
        //    {
        //        return column.PatternColumnName;
        //    }
        //}

        public override string GetTableNameWithDefs(StoredTable table)
        {
            return this.GetTableNameWithAlias(table);
        }

        public override void WriteLimitBegin(SelectWriter writer, Kemel.Orm.NQuery.Storage.Query query)
        {

        }

        public override void WriteLimitEnd(SelectWriter writer, Kemel.Orm.NQuery.Storage.Query query)
        {
            writer.EndLimit.Append("LIMIT").Append(Punctuation.WHITE_SPACE)
                .Append(query.LimitRows.Start).Append(Punctuation.COMMA)
                .Append(Punctuation.WHITE_SPACE).Append(query.LimitRows.End);
        }

        public override void WriteSelectLimitColumn(SelectWriter writer, Kemel.Orm.NQuery.Storage.Query query)
        {

        }
    }
}
