using System;
using System.Collections.Generic;
using System.Text;

namespace Kemel.Orm.Entity.Attributes
{
	[global::System.AttributeUsage(AttributeTargets.Property)]
    public sealed class LogicalExclusionColumnAttribute : Attribute
	{

        public LogicalExclusionColumnAttribute()
		{
		}

	}
}
