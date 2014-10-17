using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Schema;
using Kemel.Orm.Constants;

namespace Kemel.Orm.QueryDef
{
    public class ColumnQueryCollectoin : List<ColumnQuery>
    {
        new public ColumnQuery Add(ColumnQuery item)
        {
            base.Add(item);
            return item;
        }
    }

    public class ColumnQuery
    {
        public string ParameterName { get; set; }

        public string Alias { get; set; }
        public bool IsAggregate { get; set; }
        public ColumnSchema ColumnSchema { get; set; }
        public string ColumnName { get; set; }

        public string PatternColumnName
        {
            get
            {
                return (this.ColumnSchema == null ? this.ColumnName : this.ColumnSchema.Name);
            }
        }

        public bool HasAlias
        {
            get
            {
                return !string.IsNullOrEmpty(this.Alias);
            }
        }

        public TableQuery Parent { get; set; }

        public ColumnQuery(ColumnSchema columnSchema, TableQuery parent)
        {
            this.Parent = parent;
            this.ColumnSchema = columnSchema;
        }

        public ColumnQuery(string columnName, TableQuery parent)
        {
            this.Parent = parent;
            this.ColumnSchema = parent.FindColumnSchema(columnName);

            if (this.ColumnSchema == null)
                this.ColumnName = columnName;
        }

        internal ColumnQuery(string aliasName)
        {
            this.Parent = null;
            this.ColumnSchema = null;
            this.Alias = aliasName;
        }

        public TableQuery As(string alias)
        {
            this.Alias = alias;
            return this.Parent;
        }

        public TableQuery End()
        {
            return this.Parent;
        }
    }
}
