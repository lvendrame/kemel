using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Providers;
using System.Web;

namespace Kemel.Orm.Platform.WebApplication
{
    public class ApplicationCredential: Credential
    {
        public ApplicationCredential()
        {
            this.AuthenticationMode = AuthenticationMode.SqlUser;
            this.ApplicationName = "Kemel.Orm";
            this.ConnectionTimeOut = 200;
            this.Owner = "";
            this.Port = 0;
        }

        public override string ApplicationName
        {
            get
            {
                return WebApplicationFactory.Application["WACredential_ApplicationName"].ToString();
            }
            set
            {
                WebApplicationFactory.Application["WACredential_ApplicationName"] = value;
            }
        }

        public override string DataSource
        {
            get
            {
                return WebApplicationFactory.Application["WACredential_DataSource"].ToString();
            }
            set
            {
                WebApplicationFactory.Application["WACredential_DataSource"] = value;
            }
        }

        public override string User
        {
            get
            {
                return WebApplicationFactory.Application["WACredential_User"].ToString();
            }
            set
            {
                WebApplicationFactory.Application["WACredential_User"] = value;
            }
        }

        public override string Password
        {
            get
            {
                return WebApplicationFactory.Application["WACredential_Password"].ToString();
            }
            set
            {
                WebApplicationFactory.Application["WACredential_Password"] = value;
            }
        }

        public override string Catalog
        {
            get
            {
                return WebApplicationFactory.Application["WACredential_Catalog"].ToString();
            }
            set
            {
                WebApplicationFactory.Application["WACredential_Catalog"] = value;
            }
        }

        public override AuthenticationMode AuthenticationMode
        {
            get
            {
                return (AuthenticationMode)WebApplicationFactory.Application["WACredential_AuthenticationMode"];
            }
            set
            {
                WebApplicationFactory.Application["WACredential_AuthenticationMode"] = value;
            }
        }

        public override string Owner
        {
            get
            {
                return WebApplicationFactory.Application["WACredential_Owner"].ToString();
            }
            set
            {
                WebApplicationFactory.Application["WACredential_Owner"] = value;
            }
        }

        public override int Port
        {
            get
            {
                return Convert.ToInt32(WebApplicationFactory.Application["WACredential_Port"]);
            }
            set
            {
                WebApplicationFactory.Application["WACredential_Port"] = value;
            }
        }

        public override int ConnectionTimeOut
        {
            get
            {
                return Convert.ToInt32(WebApplicationFactory.Application["WACredential_ConnectionTimeOut"]);
            }
            set
            {
                WebApplicationFactory.Application["WACredential_ConnectionTimeOut"] = value;
            }
        }

        public override string KeyProvider
        {
            get
            {
                if (WebApplicationFactory.Application["WACredential_KeyProvider"] == null)
                    return string.Empty;
                else
                    return WebApplicationFactory.Application["WACredential_KeyProvider"].ToString();
            }
            set
            {
                WebApplicationFactory.Application["WACredential_KeyProvider"] = value;
            }
        }
    }
}
