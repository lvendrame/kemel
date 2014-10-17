using System;
using System.Collections.Generic;
using System.Text;

namespace Kemel.Orm.Entity.Attributes
{
	[global::System.AttributeUsage(AttributeTargets.Property)]
	public sealed class AllowNullAttribute: Attribute
	{

		private bool allowNull;
		public bool AllowNull
		{
			get { return allowNull; }
		}

		public AllowNullAttribute(bool allowNull)
		{
			this.allowNull = allowNull;
		}

	}
}
