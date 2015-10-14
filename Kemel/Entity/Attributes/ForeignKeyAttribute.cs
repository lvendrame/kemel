using System;
using System.Collections.Generic;
using System.Text;

namespace Kemel.Entity.Attributes
{
	[global::System.AttributeUsage(AttributeTargets.Property)]
	public sealed class ForeignKeyAttribute: Attribute
	{

		private Type entityType;
		public Type EntityType
		{
			get
			{
				return this.entityType;
			}
		}

        private string strPropertyColumn;
        public string PropertyColumn
        {
            get
            {
                return this.strPropertyColumn;
            }
        }

        public ForeignKeyAttribute(Type entityType, string propertyColumn)
            : base()
		{
            if (entityType.BaseType == typeof(EntityBase))
            {
                this.entityType = entityType;
                this.strPropertyColumn = propertyColumn;
            }
            else
                throw new OrmException("Type not extends Entity");
        }

        public ForeignKeyAttribute(Type entityType)
            : base()
        {
            if (entityType.BaseType == typeof(EntityBase))
            {
                this.entityType = entityType;
                this.strPropertyColumn = "";
            }
            else
                throw new OrmException("Type not extends Entity");
        }

	}
}
