using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Orm.Starter.Oracle.Web
{
    public class OracleStarter: OrmStarter
    {
        public override Kemel.Orm.Providers.Provider CreateProvider()
        {
            return new Providers.Oracle.OracleProviderMs();
        }

        public override Kemel.Orm.Platform.PlatformFactory CreatePlatformFactory()
        {
            return new Platform.WebSession.WebSessionFactory();
        }
    }
}
