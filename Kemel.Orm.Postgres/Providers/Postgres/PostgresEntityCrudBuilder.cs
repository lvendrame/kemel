using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Kemel.Orm.Properties;
using Kemel.Orm.Providers;
using Kemel.Orm.NQuery.Builder;
using Kemel.Orm.Constants;

namespace Kemel.Orm.Providers.Postgres
{
    public class PostgresEntityCrudBuilder : EntityCrudBuilder
    {
        public PostgresEntityCrudBuilder(Provider parent) : base(parent)
        {

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
