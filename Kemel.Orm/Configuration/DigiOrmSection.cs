using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Kemel.Orm.Configuration
{
    public class DigiOrmSection: ConfigurationSection
    {

        public const string TAG = "digiOrm";

        // Properties
        [ConfigurationProperty(CredentialElement.TAG, IsRequired = false)]
        public CredentialElement Credential
        {
            get
            {
                return (CredentialElement)this[CredentialElement.TAG];
            }
            set
            {
                this[CredentialElement.TAG] = value;
            }
        }

        [ConfigurationProperty(CredentialCollection.TAG, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(CredentialCollection),
            AddItemName = "credential",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public CredentialCollection Credentials
        {
            get
            {
                return (CredentialCollection)base["credentials"];
            }
        }

        public static DigiOrmSection GetConfig
        {
            get
            {
                return (DigiOrmSection)ConfigurationManager.GetSection(TAG);
            }
        }

    }
}
