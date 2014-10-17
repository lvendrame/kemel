using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Kemel.Orm.Providers;

namespace Kemel.Orm.Configuration
{
    public class CredentialElement: ConfigurationElement
    {
        public const string TAG = "credential";

        #region Properties

        /// <summary>
        /// Nome da Aplicação
        /// </summary>
        [ConfigurationProperty("applicationName", IsRequired=true)]
        public string ApplicationName
        {
            get
            {
                return (string)this["applicationName"];
            }
            set
            {
                this["applicationName"] = value;
            }
        }

        /// <summary>
        /// Nome ou IP do Servidor
        /// </summary>
        [ConfigurationProperty("dataSource", IsRequired = true)]
        public string DataSource
        {
            get
            {
                return (string)this["dataSource"];
            }
            set
            {
                this["dataSource"] = value;
            }
        }

        /// <summary>
        /// Usuário do servidor
        /// </summary>
        [ConfigurationProperty("user", IsRequired = true)]
        public string User
        {
            get
            {
                return (string)this["user"];
            }
            set
            {
                this["user"] = value;
            }
        }

        /// <summary>
        /// Senha de acesso ao servidor
        /// </summary>
        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get
            {
                return (string)this["password"];
            }
            set
            {
                this["password"] = value;
            }
        }

        /// <summary>
        /// DataBase
        /// </summary>
        [ConfigurationProperty("catalog")]
        public string Catalog
        {
            get
            {
                return (string)this["catalog"];
            }
            set
            {
                this["catalog"] = value;
            }
        }

        /// <summary>
        /// Forma de autenticação - Windows ou SQL
        /// </summary>
        [ConfigurationProperty("authenticationMode")]
        public AuthenticationMode? AuthenticationMode
        {
            get
            {
                return (AuthenticationMode?)this["authenticationMode"];
            }
            set
            {
                this["authenticationMode"] = value;
            }
        }

        /// <summary>
        /// DB Owner
        /// </summary>
        [ConfigurationProperty("owner")]
        public string Owner
        {
            get
            {
                return (string)this["owner"];
            }
            set
            {
                this["owner"] = value;
            }
        }

        /// <summary>
        /// Número da porta
        /// </summary>
        [ConfigurationProperty("port")]
        public int? Port
        {
            get
            {
                return (int?)this["port"];
            }
            set
            {
                this["port"] = value;
            }
        }

        /// <summary>
        /// Connection TimeOut
        /// </summary>
        [ConfigurationProperty("connectionTimeOut")]
        public int? ConnectionTimeOut
        {
            get
            {
                return (int?)this["connectionTimeOut"];
            }
            set
            {
                this["connectionTimeOut"] = value;
            }
        }

        /// <summary>
        /// Key Provider
        /// </summary>
        [ConfigurationProperty("keyProvider")]
        public string KeyProvider
        {
            get
            {
                return (string)this["keyProvider"];
            }
            set
            {
                this["keyProvider"] = value;
            }
        }

        /// <summary>
        /// Starter
        /// </summary>
        [ConfigurationProperty("starter")]
        public string Starter
        {
            get
            {
                return (string)this["starter"];
            }
            set
            {
                this["starter"] = value;
            }
        }

        #endregion
    }
}
