using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Schema;
using System.Web;

namespace Kemel.Orm.Platform.Windows
{
    public class WindowsSchemaContainer: SchemaContainer
    {

        private static Dictionary<string, TableSchema> _schemaDic = null;
        internal Dictionary<string, TableSchema> SchemaDic
        {
            get
            {
                if (_schemaDic == null)
                {
                    _schemaDic = new Dictionary<string, TableSchema>();
                }

                return _schemaDic;
            }
        }

        public override TableSchema Add(TableSchema tableSchema)
        {
            this.SchemaDic.Add(tableSchema.Name, tableSchema);
            return tableSchema;
        }

        public override bool ContainsKey(string tableName)
        {
            return this.SchemaDic.ContainsKey(tableName);
        }

        public override TableSchema this[string tableName]
        {
            get
            {
                return this.SchemaDic[tableName];
            }
            set
            {
                this.SchemaDic[tableName] = value;
            }
        }
    }
}
