using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Starter;
using Kemel.Orm.Providers;
using Kemel.Orm.Platform;

namespace Kemel.Tools.Orm
{
    public class ToolsOrmStarter: OrmStarter
    {
        public ToolsOrmStarter(Type providerType)
        {
            this.ProviderType = providerType;
        }

        public Type ProviderType { get; set; }

        public override Provider CreateProvider()
        {
            return Activator.CreateInstance(ProviderType) as Provider;
        }

        public override PlatformFactory CreatePlatformFactory()
        {
            return new Kemel.Orm.Platform.Windows.WindowsFactory();
        }
    }
}
