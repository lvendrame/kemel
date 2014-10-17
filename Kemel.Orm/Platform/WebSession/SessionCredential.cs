using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Providers;
using System.Web;

namespace Kemel.Orm.Platform.WebSession
{
    public class SessionCredential: Credential
    {
        public SessionCredential()
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
                return WebSessionFactory.Session["WSCredential_ApplicationName"].ToString();
            }
            set
            {
                WebSessionFactory.Session["WSCredential_ApplicationName"] = value;
            }
        }

        public override string DataSource
        {
            get
            {
                return WebSessionFactory.Session["WSCredential_DataSource"].ToString();
            }
            set
            {
                WebSessionFactory.Session["WSCredential_DataSource"] = value;
            }
        }

        public override string User
        {
            get
            {
                return WebSessionFactory.Session["WSCredential_User"].ToString();
            }
            set
            {
                WebSessionFactory.Session["WSCredential_User"] = value;
            }
        }

        public override string Password
        {
            get
            {
                return WebSessionFactory.Session["WSCredential_Password"].ToString();
            }
            set
            {
                WebSessionFactory.Session["WSCredential_Password"] = value;
            }
        }

        public override string Catalog
        {
            get
            {
                return WebSessionFactory.Session["WSCredential_Catalog"].ToString();
            }
            set
            {
                WebSessionFactory.Session["WSCredential_Catalog"] = value;
            }
        }

        public override AuthenticationMode AuthenticationMode
        {
            get
            {
                return (AuthenticationMode)WebSessionFactory.Session["WSCredential_AuthenticationMode"];
            }
            set
            {
                WebSessionFactory.Session["WSCredential_AuthenticationMode"] = value;
            }
        }

        public override string Owner
        {
            get
            {
                return WebSessionFactory.Session["WSCredential_Owner"].ToString();
            }
            set
            {
                WebSessionFactory.Session["WSCredential_Owner"] = value;
            }
        }

        public override int Port
        {
            get
            {
                return Convert.ToInt32(WebSessionFactory.Session["WSCredential_Port"]);
            }
            set
            {
                WebSessionFactory.Session["WSCredential_Port"] = value;
            }
        }

        public override int ConnectionTimeOut
        {
            get
            {
                return Convert.ToInt32(WebSessionFactory.Session["WSCredential_ConnectionTimeOut"]);
            }
            set
            {
                WebSessionFactory.Session["WSCredential_ConnectionTimeOut"] = value;
            }
        }

        public override string KeyProvider
        {
            get
            {
                if (WebSessionFactory.Session["WSCredential_KeyProvider"] == null)
                    return string.Empty;
                else
                    return WebSessionFactory.Session["WSCredential_KeyProvider"].ToString();
            }
            set
            {
                WebSessionFactory.Session["WSCredential_KeyProvider"] = value;
            }
        }
    }
}
