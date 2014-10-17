using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Kemel.Orm.Starter.Postgres.Web
{
    public class SqlServer2005ApplicationStarter: OrmStarter
    {
        public override Kemel.Orm.Providers.Provider CreateProvider()
        {
            return new Providers.Postgres.PostgresProvider();
        }

        public override Kemel.Orm.Platform.PlatformFactory CreatePlatformFactory()
        {
            return new Platform.WebApplication.WebApplicationFactory();
        }
    }
}
