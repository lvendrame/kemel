using System;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;
using System.Collections.Generic;

namespace Kemel.Orm.Providers.Oracle
{
    public class OracleProviderx32 : OracleProvider
    {
        public override Kemel.Orm.Data.OrmConnection GetConnection()
        {
            Kemel.Orm.Data.OrmConnection connection =
                new Kemel.Orm.Data.OrmConnection
                    (
                        new OracleConnection(this.GetConnectionString())
                    );
            return connection;
        }
    }
}
