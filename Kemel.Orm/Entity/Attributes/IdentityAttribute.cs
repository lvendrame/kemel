using System;
using System.Collections.Generic;
using System.Text;

namespace Kemel.Orm.Entity.Attributes
{
	[global::System.AttributeUsage(AttributeTargets.Property)]
	public sealed class IdentityAttribute: Attribute
	{

		private bool isIdentity;
		public bool IsIdentity
		{
			get { return isIdentity; }
		}

		public IdentityAttribute(bool isIdentity)
		{
			this.isIdentity = isIdentity;
		}

	}
}
