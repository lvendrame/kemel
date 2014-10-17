using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Kemel.Orm.Configuration
{
    public class CredentialCollection : ConfigurationElementCollection
    {

        public const string TAG = "credentials";

        public CredentialCollection()
        {
            Console.WriteLine("ServiceCollection Constructor");
        }

        public CredentialElement this[int index]
        {
            get { return (CredentialElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(CredentialElement credentialElement)
        {
            BaseAdd(credentialElement);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new CredentialElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CredentialElement)element).KeyProvider;
        }

        public void Remove(CredentialElement credentialElement)
        {
            BaseRemove(credentialElement.Port);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }


    }
}
