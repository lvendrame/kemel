using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Entity.Attributes
{
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class OwnerAttribute : Attribute
    {
        public string Owner { get; set; }

        // This is a positional argument
        public OwnerAttribute(string owner)
        {
            this.Owner = owner;
        }
    }
}
