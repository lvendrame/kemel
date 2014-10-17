using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Orm.Platform.Windows
{
    public class WindowsFactory: PlatformFactory
    {
        public override Kemel.Orm.Schema.SchemaContainer CreateSchemaContainer()
        {
            return new WindowsSchemaContainer();
        }

        public override Kemel.Orm.Providers.CommandCache CreateCommandCache()
        {
            return new WindowsCommandCache();
        }

        public override Kemel.Orm.Providers.Credential CreateCredential()
        {
            return new WindowsCredential();
        }
    }
}
