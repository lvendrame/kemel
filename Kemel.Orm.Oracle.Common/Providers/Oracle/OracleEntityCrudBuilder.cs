using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Properties;
using Kemel.Orm.NQuery.Builder;
using Kemel.Orm.NQuery.Storage;
using Kemel.Orm.Schema;
using Kemel.Orm.Constants;

namespace Kemel.Orm.Providers.Oracle
{
    public class OracleEntityCrudBuilder: EntityCrudBuilder
    {

        public OracleEntityCrudBuilder(Provider parent)
            : base(parent)
        {
        }

        public override Query GetInsert<TEtt>(TEtt ettEntity)
        {
            Query query = StorageFactory.NQuery.Insert(this.Parent).Into<TEtt>();
            TableSchema schema = (TableSchema)query.IntoTable.TableDefinition;

            foreach (ColumnSchema column in schema.Columns)
            {
                if (column.IgnoreColumn)
                {
                    continue;
                }

                if (!column.IsIdentity)
                {
                    object value = column.GetValue(ettEntity);
                    if (value == null && !column.AllowNull)
                    {
                        if (column.IsLogicalExclusionColumn)
                        {
                            column.SetValue(ettEntity, 0);
                            value = 0;
                        }
                        else if (!string.IsNullOrEmpty(column.SequenceName))
                        {
                            query.SetValue(column).Equal(new StringConstant(string.Concat(column.SequenceName, Punctuation.DOT, "nextval")));
                        }
                        else
                        {
                            throw new OrmException(string.Format(Messages.PropertyDoesNotNull, column.Name));
                        }
                    }

                    query.SetValue(column).Equal(value);
                }
            }
            //this.Parent.ExecuteOnQueryBuilder(this, query, ettEntity, CrudType.Insert);
            return query;
        }

        public override Query GetSelectIdentity<TEtt>()
        {
            TableSchema tb = TableSchema.FromEntity<TEtt>();
            bool useBase = true;

            Query query = StorageFactory.NQuery.Select(this.Parent);
            foreach (ColumnSchema column in tb.IdentityColumns)
            {
                if (!string.IsNullOrEmpty(column.SequenceName))
                {
                    query.Column(new StringConstant(string.Concat(column.SequenceName, Punctuation.DOT, "currval")));
                    useBase = false;
                }
            }

            if (useBase)
                return base.GetSelectIdentity<TEtt>();

            query.From("dual");

            //this.Parent.ExecuteOnQueryBuilder(this, query, null, CrudType.SelectIdentity);

            return query;
        }

        public override string GetSequenceNextValue(Schema.ColumnSchema column)
        {
            return string.Concat(column.SequenceName, Punctuation.DOT, "nextval");
        }

        public override string GetSequenceCurrentValue(Schema.ColumnSchema column)
        {
            return string.Format("SELECT {0}.currval FROM dual", column.SequenceName);
        }

    }
}
