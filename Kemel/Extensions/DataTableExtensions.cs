using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Kemel.Schema;
using Kemel.Constants;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml;
using Kemel.Entity;

namespace Kemel
{

    public static class DataTableExtensions
    {

        public static TEtt ToEntity<TEtt>(this DataRow self)
            where TEtt : EntityBase, new()
        {
            TEtt ett_Entity = new TEtt();
            ett_Entity.EntityState = EntityState.Unchanged;

            TableSchema tb = TableSchema.FromEntity<TEtt>();
            DataColumnCollection columns = self.Table.Columns;

            foreach (ColumnSchema col in tb.Columns)
            {
                if (columns.Contains(col.Name))
                {
                    col.SetValue(ett_Entity, self[col.Name]);
                }
            }
            return ett_Entity;
        }

        internal static TEtt ToEntity<TEtt>(this DataRow self, TableSchema tableSchema)
            where TEtt : EntityBase, new()
        {
            TEtt ett_Entity = new TEtt();
            ett_Entity.EntityState = EntityState.Unchanged;

            DataColumnCollection columns = self.Table.Columns;

            foreach (ColumnSchema col in tableSchema.Columns)
            {
                if (columns.Contains(col.Name))
                {
                    col.SetValue(ett_Entity, self[col.Name]);
                }
            }
            return ett_Entity;
        }

        public static List<TEtt> ToEntityList<TEtt>(this DataTable self)
            where TEtt : EntityBase, new()
        {
            TableSchema tableSchema = SchemaContainer.GetSchema<TEtt>();
            List<TEtt> lstEntity = new List<TEtt>();
            foreach (DataRow row in self.Rows)
            {
                lstEntity.Add(row.ToEntity<TEtt>(tableSchema));
            }
            return lstEntity;
        }

    }
}
