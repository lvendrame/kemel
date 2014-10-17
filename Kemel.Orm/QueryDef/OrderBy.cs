using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Schema;
using Kemel.Orm.Constants;

namespace Kemel.Orm.QueryDef
{
    public class OrderByCollection : List<OrderBy>
    {

        new public OrderBy Add(OrderBy item)
        {
            base.Add(item);
            return item;
        }
    }


    public class OrderBy: ColumnQuery
    {


        public OrderBySortOrder SortOrder { get; set; }

        #region Constructors

        public OrderBy(ColumnSchema columnSchema, TableQuery parent, OrderBySortOrder sortOrder)
            : base(columnSchema, parent)
        {
            this.SortOrder = sortOrder;
        }

        public OrderBy(string columnName, TableQuery parent, OrderBySortOrder sortOrder)
            : base(columnName, parent)
        {
            this.SortOrder = sortOrder;
        }

        public OrderBy(ColumnSchema columnSchema, TableQuery parent)
            : this(columnSchema, parent, OrderBySortOrder.Default)
        {
        }

        public OrderBy(string columnName, TableQuery parent)
            : this(columnName, parent, OrderBySortOrder.Default)
        {
        }

        public OrderBy(string alias)
            : base(alias)
        {
            this.SortOrder = OrderBySortOrder.Default;
        }

        #endregion

        #region Methods

        public Query Asc()
        {
            this.SortOrder = OrderBySortOrder.Asc;
            return this.Parent.Parent;
        }

        public Query Desc()
        {
            this.SortOrder = OrderBySortOrder.Desc;
            return this.Parent.Parent;
        }

        new public Query As(string alias)
        {
            return base.As(alias).Parent;
        }

        new public Query End()
        {
            return base.End().Parent;
        }

        #endregion

    }
}
