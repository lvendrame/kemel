using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Entity;
using Kemel.Providers;

namespace Kemel.Schema
{
    public abstract class SchemaContainer
    {


        public static TableSchema GetSchema<TEtt>()
            where TEtt : EntityBase
        {
            return SchemaContainer.GetSchema(typeof(TEtt));
        }

        public static TableSchema GetSchema(EntityBase entity)
        {
            return GetSchema(entity.GetType());
        }

        public static TableSchema GetSchema(Type entityType)
        {
            string tableName = TableSchema.GetTableName(entityType);

            if (Provider.SchemaContainer.ContainsKey(tableName))
            {
                return Provider.SchemaContainer[tableName];
            }
            else
            {
                return Provider.SchemaContainer.Add(new TableSchema(entityType));
            }
        }

        public static TableSchema GetSchema(string tableName)
        {
            TableSchema schema = TableSchema.FromTableName(tableName);
            if (schema == null)
                return null;
            else
                return Provider.SchemaContainer.Add(schema);
        }

        public abstract TableSchema Add(TableSchema tableSchema);

        public abstract bool ContainsKey(string tableName);

        public abstract TableSchema this[string tableName]
        {
            get;
            set;
        }
    }
}
