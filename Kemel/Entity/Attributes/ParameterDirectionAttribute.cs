using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Kemel.Entity.Attributes
{
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ParameterDirectionAttribute : Attribute
    {
        private ParameterDirection fenmDirection = ParameterDirection.Input;
        public ParameterDirection Diretion
        {
            get
            {
                return this.fenmDirection;
            }
        }

        public ParameterDirectionAttribute(ParameterDirection penmDirection)
        {
            this.fenmDirection = penmDirection;
        }

        public ParameterDirectionAttribute()
            : this(ParameterDirection.Input)
        {
        }
    }
}
