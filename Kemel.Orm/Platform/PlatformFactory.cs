using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Providers;
using Kemel.Orm.Starter;

namespace Kemel.Orm.Platform
{
    public abstract class PlatformFactory
    {

        private static PlatformFactory pfcInstance = null;
        public static PlatformFactory Instance
        {
            get
            {
                return pfcInstance;
            }
        }

        public static void SetPlatformFactory(OrmStarter starter)
        {
            pfcInstance = starter.CreatePlatformFactory();
        }

        public abstract Schema.SchemaContainer CreateSchemaContainer();
        public abstract Providers.CommandCache CreateCommandCache();
        public abstract Providers.Credential CreateCredential();
    }
}
