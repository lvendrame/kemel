using System;
using System.Collections.Generic;
using System.Text;
using Kemel.Orm.NQuery.Storage.StoredSelect;

namespace Kemel.Entity.Attributes
{
	[global::System.AttributeUsage(AttributeTargets.Property)]
	public sealed class OrderByAttribute : Attribute
	{
		private int index;
		public int Index
		{
			get { return index; }
			set { index = value; }
		}

		private StoredOrderBy.Order enmSortOrder;
        public StoredOrderBy.Order SortOrder
		{
            get { return enmSortOrder; }
            set { enmSortOrder = value; }
		}

        public OrderByAttribute(int index, StoredOrderBy.Order sortOrder)
		{
            this.enmSortOrder = sortOrder;
			this.index = index;
        }

        public OrderByAttribute(int index):
            this(index, StoredOrderBy.Order.Asc)
        {
        }
	}

}
