using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Orm.Starter.Web
{
    public class SqlServer2005Starter: OrmStarter
    {
        public override Kemel.Orm.Providers.Provider CreateProvider()
        {
            return new Providers.SqlServer.SqlServerProvider();
        }

        public override Kemel.Orm.Platform.PlatformFactory CreatePlatformFactory()
        {
            return new Platform.WebSession.WebSessionFactory();
        }
    }
}
