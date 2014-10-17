using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Constants;
using Kemel.Orm.NQuery.Builder;
using Kemel.Orm.NQuery.Storage.Table;
using Kemel.Orm.Schema;
using Kemel.Orm.NQuery.Storage.StoredSelect;

namespace Kemel.Orm.Providers.SqlServer
{
    public class SqlServerQueryBuilder: QueryBuilder
    {
        public SqlServerQueryBuilder(Provider parent)
            : base(parent)
        {
        }

        public override string DataBasePrefixParameter
        {
            get { return "@"; }
        }

        public override string DataBasePrefixVariable
        {
            get { return "@"; }
        }

        public override string ConcatOperator
        {
            get { return "+"; }
        }

        //public override void WriteSelectConvert(Kemel.Orm.Data.OrmCommand command, Kemel.Orm.QueryDef.Query query, Kemel.Orm.QueryDef.TableQuery table, StringBuilder stbColumns, Kemel.Orm.QueryDef.Aggregate aggregated, string columnPrefix)
        //{
        //    stbColumns.Append(Functions.CONVERT);

        //    stbColumns.Append(Punctuation.PARENTHESIS_OPEN);
        //    this.WriteSelectAggregatedValue(command, query, table, stbColumns, aggregated, columnPrefix);
        //    stbColumns.Append(Punctuation.COMMA);
        //    stbColumns.Append(Punctuation.WHITE_SPACE);

        //    stbColumns.Append(this.Parent.ConvertDbTypeToFinalDbType(aggregated.TypeToConvert));

        //    if (!string.IsNullOrEmpty(aggregated.ConvertType))
        //    {
        //        stbColumns.Append(Punctuation.COMMA);
        //        stbColumns.Append(Punctuation.WHITE_SPACE);

        //        stbColumns.Append(aggregated.ConvertType);
        //    }

        //    stbColumns.Append(Punctuation.PARENTHESIS_CLOSE);
        //}

        //public override void WriteSelectCast(Kemel.Orm.Data.OrmCommand command, Kemel.Orm.QueryDef.Query query, Kemel.Orm.QueryDef.TableQuery table, StringBuilder stbColumns, Kemel.Orm.QueryDef.Aggregate aggregated, string columnPrefix)
        //{
        //    stbColumns.Append(Functions.CAST);

        //    stbColumns.Append(Punctuation.PARENTHESIS_OPEN);
        //    this.WriteSelectAggregatedValue(command, query, table, stbColumns, aggregated, columnPrefix);

        //    stbColumns.Append(Punctuation.WHITE_SPACE);
        //    stbColumns.Append(DML.AS);
        //    stbColumns.Append(Punctuation.WHITE_SPACE);

        //    stbColumns.Append(this.Parent.ConvertDbTypeToFinalDbType(aggregated.TypeToConvert));

        //    stbColumns.Append(Punctuation.PARENTHESIS_CLOSE);
        //}

        public override void WriteSelectLimitColumn(SelectWriter writer, Kemel.Orm.NQuery.Storage.Query query)
        {
            StringBuilder sbPart = writer.BeginLimit;
            sbPart.Append(", ");
            sbPart.Append("ROW_NUMBER() OVER( order by ");

            if (query.OrderBys.Count != 0)
            {
                for (int index = 0; index <= query.OrderBys.Count - 1; index++)
                {
                    if ((index > 0))
                    {
                        sbPart.Append(Punctuation.COMMA).Append(Punctuation.WHITE_SPACE);
                    }

                    StoredOrderBy orderBy = query.OrderBys[index];

                    if (orderBy.SortOrder != StoredOrderBy.Order.DefaultOrder)
                    {
                        string order = orderBy.SortOrder == StoredOrderBy.Order.Asc ? DML.ASC : DML.DESC;
                        sbPart.AppendFormat("{0}.{1} {2}", LIMIT_TABLE_ALIAS, orderBy.ColumnDefinition.Name, orderBy.SortOrder.ToString());
                    }
                    else
                    {
                        sbPart.AppendFormat("{0}.{1}", LIMIT_TABLE_ALIAS, orderBy.ColumnDefinition.Name);
                    }
                }

            }
            else
            {
                bool addcolumn = true;
                StoredTable st = query.Tables.First();

                Kemel.Orm.NQuery.Storage.Column.StoredColumn sc = null;
                if (st.Type == StoredTable.StoredTypes.Schema)
                {
                    TableSchema tb = st.TableDefinition as TableSchema;
                    ColumnSchema[] columns = tb.GetPrimaryKeys();
                    if (columns.Length != 0)
                    {
                        addcolumn = false;
                        sc = Kemel.Orm.NQuery.Storage.StorageFactory.Column.Create(columns[0], st, query);
                    }
                }
                if (addcolumn)
                {
                    sc = query.Columns[0];
                }

                sbPart.AppendFormat("{0}.{1}", LIMIT_TABLE_ALIAS, sc.ColumnDefinition.Name);
            }

            sbPart.Append(Punctuation.PARENTHESIS_CLOSE).Append(Punctuation.WHITE_SPACE).Append(QueryBuilder.LIMIT_COLUMN_ALIAS).AppendLine();
        }
    }
}
