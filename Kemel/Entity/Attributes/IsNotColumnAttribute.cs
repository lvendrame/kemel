using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Entity.Attributes
{
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class IsNotColumnAttribute : Attribute
    {

        // This is a positional argument
        public IsNotColumnAttribute()
        {
        }
    }
}
