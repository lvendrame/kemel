using System;
using System.Collections.Generic;
using System.Text;

namespace Kemel.Entity.Attributes
{
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class TableAliasAttribute : Attribute
	{
		private string alias;

		public string Alias
		{
			get
			{
				return this.alias;
			}
		}

		public TableAliasAttribute(string alias)
		{
			this.alias = alias;
		}

	}

}
