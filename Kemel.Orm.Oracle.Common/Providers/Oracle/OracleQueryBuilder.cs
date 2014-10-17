using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.NQuery;
using Kemel.Orm.NQuery.Builder;
using Kemel.Orm.NQuery.Storage;
using Kemel.Orm.Constants;
using Kemel.Orm.NQuery.Storage.Table;

namespace Kemel.Orm.Providers.Oracle
{
    public class OracleQueryBuilder : QueryBuilder
    {
        public OracleQueryBuilder(Provider parent)
            : base(parent)
        {
        }

        public override string DataBasePrefixParameter
        {
            get { return string.Empty; }
        }

        public override string DataBasePrefixVariable
        {
            get { return ":"; }
        }

        public override string ConcatOperator
        {
            get { return "||"; }
        }

        //public override void WriteSelectConvert(Kemel.Orm.Data.OrmCommand command, Query query, StoredTable table, StringBuilder stbColumns, Aggregate aggregated, string columnPrefix)
        //{
        //    throw new NotImplementedException();
        //}

        //public override void WriteSelectCast(Kemel.Orm.Data.OrmCommand command, Kemel.Orm.QueryDef.Query query, Kemel.Orm.QueryDef.TableQuery table, StringBuilder stbColumns, Kemel.Orm.QueryDef.Aggregate aggregated, string columnPrefix)
        //{
        //    throw new NotImplementedException();
        //}

        //public override string GetColumnNameWithAlias(Kemel.Orm.NQuery.Storage.Column.StoredColumn column)
        //{
        //    if (column.HasAlias)
        //        return string.Concat(column.PatternColumnName, Punctuation.WHITE_SPACE, DML.AS, Punctuation.WHITE_SPACE,
        //            Punctuation.QUOTATION_MARKS, column.Alias, Punctuation.QUOTATION_MARKS);
        //    else
        //        return column.PatternColumnName;
        //}

        public override string GetTableNameWithDefs(StoredTable table)
        {
            return this.GetTableNameWithAlias(table);
        }

        public override void WriteSelectLimitColumn(SelectWriter writer, Query query)
        {
            StringBuilder sbPart = writer.BeginLimit;
            sbPart.Append("rownum").Append(Punctuation.WHITE_SPACE).Append(QueryBuilder.LIMIT_COLUMN_ALIAS);
        }
    }
}
