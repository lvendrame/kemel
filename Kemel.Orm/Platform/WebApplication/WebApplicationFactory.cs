using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kemel.Orm.Platform.WebApplication
{
    public class WebApplicationFactory: PlatformFactory
    {
        private static HttpApplicationState _application;
        public static HttpApplicationState Application
        {
            get
            {
                if (_application == null && HttpContext.Current != null)
                    _application = HttpContext.Current.Application;
                return _application;
            }
            set
            {
                _application = value;
            }
        }

        public override Kemel.Orm.Schema.SchemaContainer CreateSchemaContainer()
        {
            return new ApplicationSchemaContainer();
        }

        public override Kemel.Orm.Providers.CommandCache CreateCommandCache()
        {
            return new ApplicationCommandCache();
        }

        public override Kemel.Orm.Providers.Credential CreateCredential()
        {
            return new ApplicationCredential();
        }
    }
}
