using System;
using System.Collections.Generic;
using System.Text;

namespace Kemel.Orm.Entity.Attributes
{
	[global::System.AttributeUsage(AttributeTargets.Class)]
	public sealed class TableNameAttribute : Attribute
	{
		private string name;

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public TableNameAttribute(string name)
		{
			this.name = name;
		}

	}

}
