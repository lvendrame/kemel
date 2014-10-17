using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Platform.WebApplication;
using Kemel.Orm.Platform.WebSession;

namespace Kemel.Orm.Platform.WebMist
{
    public class WebMistFactory : PlatformFactory
    {
        public override Kemel.Orm.Schema.SchemaContainer CreateSchemaContainer()
        {
            return new ApplicationSchemaContainer();
        }

        public override Kemel.Orm.Providers.CommandCache CreateCommandCache()
        {
            return new SessionCommandCache();
        }

        public override Kemel.Orm.Providers.Credential CreateCredential()
        {
            return new SessionCredential();
        }
    }
}
