using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Providers;
using Kemel.Orm.Platform;
using Kemel.Orm.Configuration;

namespace Kemel.Orm.Starter
{
    public abstract class OrmStarter
    {
        public abstract Provider CreateProvider();
        public abstract PlatformFactory CreatePlatformFactory();

        public Credential Credential { get; private set; }

        public void Initialize()
        {
            PlatformFactory.SetPlatformFactory(this);
            Provider.SetStarter(this);
            this.Credential = PlatformFactory.Instance.CreateCredential();

            DigiOrmSection config = Configuration.DigiOrmSection.GetConfig;
            if (config != null)
            {
                if (config.Credentials != null)
                {
                    foreach (CredentialElement item in config.Credentials)
                    {
                        this.FillCredential(item);
                        Provider.Create();
                    }
                }
                else if (config.Credential != null)
                {
                    this.FillCredential(config.Credential);
                    Provider.Create();
                }
            }
        }

        public void FillCredential(CredentialElement credential)
        {

            if (this.Credential == null)
                this.Credential = PlatformFactory.Instance.CreateCredential();

            if (!string.IsNullOrEmpty(credential.ApplicationName))
                this.Credential.ApplicationName = credential.ApplicationName;

            if (credential.AuthenticationMode.HasValue)
                this.Credential.AuthenticationMode = credential.AuthenticationMode.Value;

            if (!string.IsNullOrEmpty(credential.Catalog))
                this.Credential.Catalog = credential.Catalog;

            if (credential.ConnectionTimeOut.HasValue)
                this.Credential.ConnectionTimeOut = credential.ConnectionTimeOut.Value;

            if (!string.IsNullOrEmpty(credential.DataSource))
                this.Credential.DataSource = credential.DataSource;

            if (!string.IsNullOrEmpty(credential.KeyProvider))
                this.Credential.KeyProvider = credential.KeyProvider;

            if (!string.IsNullOrEmpty(credential.Owner))
                this.Credential.Owner = credential.Owner;

            if (!string.IsNullOrEmpty(credential.Password))
                this.Credential.Password = credential.Password;

            if (credential.Port.HasValue)
                this.Credential.Port = credential.Port.Value;

            if (!string.IsNullOrEmpty(credential.User))
                this.Credential.User = credential.User;
        }

    }
}
