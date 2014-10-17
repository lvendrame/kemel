using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Schema;
using System.Web;

namespace Kemel.Orm.Platform.WebSession
{
    public class SessionSchemaContainer: SchemaContainer
    {
        internal Dictionary<string, TableSchema> SchemaDic
        {
            get
            {
                Dictionary<string, TableSchema> _schemaDic = null;
                if (WebSessionFactory.Session["WSSchemaConteinerDic"] == null)
                {
                    _schemaDic = new Dictionary<string, TableSchema>();
                    WebSessionFactory.Session["WSSchemaConteinerDic"] = _schemaDic;
                }
                else
                {
                    _schemaDic = (WebSessionFactory.Session["WSSchemaConteinerDic"] as Dictionary<string, TableSchema>);
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
