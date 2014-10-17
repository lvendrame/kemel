using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Orm.Entity.Attributes
{
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class KeyProviderAttribute : Attribute
    {
        private string strKey;

        public KeyProviderAttribute(string key)
        {
            this.strKey = key;
        }

        public string Key
        {
            get { return strKey; }
        }
    }
}
