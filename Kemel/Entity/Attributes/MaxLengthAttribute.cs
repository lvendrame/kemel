using System;
using System.Collections.Generic;
using System.Text;

namespace Kemel.Entity.Attributes
{
	[global::System.AttributeUsage(AttributeTargets.Property)]
	public sealed class MaxLengthAttribute : Attribute
	{
		#region Fields
		private int maxLength;
		#endregion

		#region Properties
		public int Length
		{
			get { return maxLength; }
			set { maxLength = value; }
		}
		#endregion

		// This is a positional argument.
		public MaxLengthAttribute(int maxLength)
		{
			this.maxLength = maxLength;
		}
	}

}
