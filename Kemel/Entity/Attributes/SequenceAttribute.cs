using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Entity.Attributes
{
    [global::System.AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class SequenceAttribute : Attribute
    {
        private string strSequenceName;

        public SequenceAttribute(string pSequencialName)
        {
            this.strSequenceName = pSequencialName;
        }

        public string SequenceName
        {
            get { return strSequenceName; }
        }
    }
}
