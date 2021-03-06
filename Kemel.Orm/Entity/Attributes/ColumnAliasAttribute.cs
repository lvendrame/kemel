using System;
using System.Collections.Generic;
using System.Text;

namespace Kemel.Orm.Entity.Attributes
{
	[global::System.AttributeUsage(AttributeTargets.Property)]
	public sealed class ColumnAliasAttribute : Attribute
	{
		private string alias;

		public string Alias
		{
			get
			{
				return this.alias;
			}
		}

		public ColumnAliasAttribute(string alias)
		{
			this.alias = alias;
		}

	}

}
