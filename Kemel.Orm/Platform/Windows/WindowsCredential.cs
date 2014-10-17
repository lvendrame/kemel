using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Providers;
using System.Web;

namespace Kemel.Orm.Platform.Windows
{
    public class WindowsCredential: Credential
    {
        private string _applicationName = string.Empty;
        public override string ApplicationName
        {
            get
            {
                return _applicationName;
            }
            set
            {
                _applicationName = value;
            }
        }

        private string _dataSource = string.Empty;
        public override string DataSource
        {
            get
            {
                return _dataSource;
            }
            set
            {
                _dataSource = value;
            }
        }

        private string _user = string.Empty;
        public override string User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }

        private string _password = string.Empty;
        public override string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        private string _catalog = string.Empty;
        public override string Catalog
        {
            get
            {
                return _catalog;
            }
            set
            {
                _catalog = value;
            }
        }

        private AuthenticationMode _authenticationMode = AuthenticationMode.SqlUser;
        public override AuthenticationMode AuthenticationMode
        {
            get
            {
                return _authenticationMode;
            }
            set
            {
                _authenticationMode = value;
            }
        }

        private string _owner = string.Empty;
        public override string Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
            }
        }

        private int _port = 1521;
        public override int Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
            }
        }

        private int _connectionTimeOut = 200;
        public override int ConnectionTimeOut
        {
            get
            {
                return _connectionTimeOut;
            }
            set
            {
                _connectionTimeOut = value;
            }
        }

        private string _keyProvider = string.Empty;
        public override string KeyProvider
        {
            get
            {
                return _keyProvider;
            }
            set
            {
                _keyProvider = value;
            }
        }
    }
}
