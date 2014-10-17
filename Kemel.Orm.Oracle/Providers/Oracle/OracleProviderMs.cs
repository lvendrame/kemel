using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;

namespace Kemel.Orm.Providers.Oracle
{
    public class OracleProviderMs : OracleProvider
    {
        public override Kemel.Orm.Data.OrmConnection GetConnection()
        {
            Kemel.Orm.Data.OrmConnection connection =
                new Kemel.Orm.Data.OrmConnection(
                    new OracleConnection(this.GetConnectionString())
                );
            return connection;
        }
    }
}
