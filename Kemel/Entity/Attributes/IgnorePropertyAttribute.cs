using System;
using System.Collections.Generic;
using System.Text;

namespace Kemel.Entity.Attributes
{
	[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	public sealed class IgnorePropertyAttribute : Attribute
	{}

}
