using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.NQuery.Builder;
using Kemel.Orm.NQuery.Storage;

namespace Kemel.Orm.Providers.SqlServer
{
    public class SqlEntityCrudBuilder: EntityCrudBuilder
    {
        public SqlEntityCrudBuilder(Provider parent)
            : base(parent)
        {
        }

        public override Query GetSelectIdentity<TEtt>()
        {
            Query query = StorageFactory.NQuery.Select(this.Parent);
            query.Column(new StringConstant("@@identity"));

            this.Parent.ExecuteOnQueryBuilder(this, query, null, CrudType.SelectIdentity);

            return query;
        }

        public override string GetSequenceNextValue(Schema.ColumnSchema column)
        {
            return string.Concat("NEXT VALUE FOR ", column.SequenceName);
        }

        public override string GetSequenceCurrentValue(Schema.ColumnSchema column)
        {
            return string.Format("SELECT current_value FROM sys.sequences WHERE name = '{0}'", column.SequenceName);
        }
    }
}
