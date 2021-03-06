using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Orm.Starter.MySQL.Web
{
    public class SqlServer2005ApplicationStarter: OrmStarter
    {
        public override Kemel.Orm.Providers.Provider CreateProvider()
        {
            return new Providers.MySQL.MySQLProvider();
        }

        public override Kemel.Orm.Platform.PlatformFactory CreatePlatformFactory()
        {
            return new Platform.WebApplication.WebApplicationFactory();
        }
    }
}
