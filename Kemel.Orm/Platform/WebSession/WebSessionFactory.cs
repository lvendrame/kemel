using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using System.Web;

namespace Kemel.Orm.Platform.WebSession
{
    public class WebSessionFactory: PlatformFactory
    {
        private static HttpSessionState _session;
        public static HttpSessionState Session
        {
            get
            {
                if (_session == null && HttpContext.Current != null)
                    _session = HttpContext.Current.Session;
                return _session;
            }
            set
            {
                _session = value;
            }
        }

        public override Kemel.Orm.Schema.SchemaContainer CreateSchemaContainer()
        {
            return new SessionSchemaContainer();
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
