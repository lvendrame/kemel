using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Entity;
using Kemel.Orm.Constants;
using Kemel.Orm.Entity.Attributes;
using Kemel.Orm.Schema;

namespace Kemel.Orm.DataBase
{
    public class T_Table_Entity: EntityBase
    {
        #region fields
        private System.String name = null;
        #endregion

        #region Properties

        #region public System.String NAME
        /// <summary>
        /// <para>Sem descrição informada.</para>
        /// <para>Permite nulo.</para>
        /// </summary>
        [Identity(false)]
        [PrimaryKey(false)]
        [AllowNull(false)]
        public System.String NAME
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
        #endregion

        #endregion

        #region Definition
        public class Definition
        {
            public const string NAME = "NAME";
        }
        #endregion

        public TableSchema ToTableSchema()
        {
            TableSchema table = new TableSchema();
            table.Name = this.NAME;
            table.SchemaType = SchemaType.Table;
            return table;
        }

        public override string ToString()
        {
            return this.NAME;
        }
    }
}
