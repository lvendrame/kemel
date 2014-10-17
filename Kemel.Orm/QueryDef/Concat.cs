using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Schema;
using Kemel.Orm.Entity;

namespace Kemel.Orm.QueryDef
{

    public class ConcatCollection : List<Concat>
    {
        new public Concat Add(Concat item)
        {
            base.Add(item);
            return item;
        }
    }

    public class Concat
    {
        public Concat(Query parent)
        {
            this.Parent = parent;
        }

        public Query Parent { get; private set; }

        public string Alias { get; set; }

        private ConcatValueCollection lstConcatValues = null;
        /// <summary>
        /// Gets the Concat Values.
        /// </summary>
        /// <value>The Concat Values.</value>
        public ConcatValueCollection ConcatValues
        {
            get
            {
                if (this.lstConcatValues == null)
                    this.lstConcatValues = new ConcatValueCollection();
                return this.lstConcatValues;
            }
        }

        public Concat Value(object value)
        {
            this.ConcatValues.Add(ConcatValue.ConcatObjectValue(value));
            return this;
        }

        public Concat ConstantValue(string value)
        {
            this.ConcatValues.Add(ConcatValue.ConcatConstantValue(value));
            return this;
        }

        public Concat Aggregate(Aggregate aggregate)
        {
            this.ConcatValues.Add(ConcatValue.ConcatAggregate(aggregate));
            return this;
        }

        public Concat Column(string tableName, string columnName)
        {
            TableQuery tq = this.Parent.FindTableQuery(tableName);
            this.ConcatValues.Add(ConcatValue.ConcatColumn(new ColumnQuery(columnName, tq)));
            return this;
        }

        public Concat Column(TableSchema table, string columnName)
        {
            TableQuery tq = this.Parent.FindTableQuery(table);
            this.ConcatValues.Add(ConcatValue.ConcatColumn(new ColumnQuery(columnName, tq)));
            return this;
        }

        public Concat Column<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            TableQuery tq = this.Parent.FindTableQuery(table);
            this.ConcatValues.Add(ConcatValue.ConcatColumn(new ColumnQuery(columnName, tq)));
            return this;
        }

        public Concat Column(ColumnSchema column)
        {
            TableQuery tq = this.Parent.FindTableQuery(column.Parent);
            this.ConcatValues.Add(ConcatValue.ConcatColumn(new ColumnQuery(column, tq)));
            return this;
        }

        public Concat OpenParentesis()
        {
            this.ConcatValues.Add(ConcatValue.OpenParentesis());
            return this;;
        }

        public Concat CloseParentesis()
        {
            this.ConcatValues.Add(ConcatValue.CloseParentesis());
            return this;
        }

        public Query End()
        {
            return this.Parent;
        }

        public Query As(string alias)
        {
            this.Alias = alias;
            return this.Parent;
        }

    }
}
