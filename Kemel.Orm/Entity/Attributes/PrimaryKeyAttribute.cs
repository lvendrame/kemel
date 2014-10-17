using System;
using System.Collections.Generic;
using System.Text;

namespace Kemel.Orm.Entity.Attributes
{
	[global::System.AttributeUsage(AttributeTargets.Property)]
    public sealed class PrimaryKeyAttribute : Attribute
	{
		private bool isPrimaryKey;

		public bool IsPrimaryKey
		{
			get { return isPrimaryKey; }
		}


		public PrimaryKeyAttribute(bool isKey)
		{
			this.isPrimaryKey = isKey;
		}
	}
}
