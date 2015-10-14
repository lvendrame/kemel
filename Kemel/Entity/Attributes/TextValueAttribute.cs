using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Entity.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TextValueAttribute : Attribute
    {
        public TextValueAttribute(string strTextField, string strValueField)
        {
            this.TextField = strTextField;
            this.ValueField = strValueField;
        }

        public string TextField { get; private set; }
        public string ValueField { get; private set; }
    }
}
