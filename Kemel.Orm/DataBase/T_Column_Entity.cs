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
    public class T_Column_Entity: EntityBase
    {
        #region fields
        private System.String table = null;
        private System.String column = null;
        private System.String description = null;
        private System.Int32? length = null;
        private System.Int32? precision = null;
        private System.Int32? scale = null;
        private System.Boolean? allow_null = null;
        private System.Boolean? is_identity = null;
        private System.Boolean? is_primary_key = null;
        private System.String type = null;
        private System.String table_ref = null;
        private System.String column_ref = null;
        #endregion

        #region Properties

        #region public System.String TABLE
        /// <summary>
        /// <para>Sem descrição informada.</para>
        /// <para>Permite nulo.</para>
        /// </summary>
        [Identity(false)]
        [PrimaryKey(false)]
        [AllowNull(false)]
        public System.String TABLE
        {
            get
            {
                return this.table;
            }
            set
            {
                this.table = value;
            }
        }
        #endregion

        #region public System.String COLUMN
        /// <summary>
        /// <para>Sem descrição informada.</para>
        /// <para>Permite nulo.</para>
        /// </summary>
        [Identity(false)]
        [PrimaryKey(false)]
        [AllowNull(false)]
        public System.String COLUMN
        {
            get
            {
                return this.column;
            }
            set
            {
                this.column = value;
            }
        }
        #endregion

        #region public System.String DESCRIPTION
        /// <summary>
        /// <para>Sem descrição informada.</para>
        /// <para>Permite nulo.</para>
        /// </summary>
        [Identity(false)]
        [PrimaryKey(false)]
        [AllowNull(true)]
        public System.String DESCRIPTION
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }
        #endregion

        #region public System.Int32? LENGTH
        /// <summary>
        /// <para>Sem descrição informada.</para>
        /// <para>Permite nulo.</para>
        /// </summary>
        [Identity(false)]
        [PrimaryKey(false)]
        [AllowNull(true)]
        public System.Int32? LENGTH
        {
            get
            {
                return this.length;
            }
            set
            {
                this.length = value;
            }
        }
        #endregion

        #region public System.Int32? PRECISION
        /// <summary>
        /// <para>Sem descrição informada.</para>
        /// <para>Permite nulo.</para>
        /// </summary>
        [Identity(false)]
        [PrimaryKey(false)]
        [AllowNull(true)]
        public System.Int32? PRECISION
        {
            get
            {
                return this.precision;
            }
            set
            {
                this.precision = value;
            }
        }
        #endregion

        #region public System.Int32? SCALE
        /// <summary>
        /// <para>Sem descrição informada.</para>
        /// <para>Permite nulo.</para>
        /// </summary>
        [Identity(false)]
        [PrimaryKey(false)]
        [AllowNull(true)]
        public System.Int32? SCALE
        {
            get
            {
                return this.scale;
            }
            set
            {
                this.scale = value;
            }
        }
        #endregion

        #region public System.Boolean? ALLOW_NULL
        /// <summary>
        /// <para>Sem descrição informada.</para>
        /// <para>Permite nulo.</para>
        /// </summary>
        [Identity(false)]
        [PrimaryKey(false)]
        [AllowNull(true)]
        public System.Boolean? ALLOW_NULL
        {
            get
            {
                return this.allow_null;
            }
            set
            {
                this.allow_null = value;
            }
        }
        #endregion

        #region public System.Boolean? IS_IDENTITY
        /// <summary>
        /// <para>Sem descrição informada.</para>
        /// <para>Permite nulo.</para>
        /// </summary>
        [Identity(false)]
        [PrimaryKey(false)]
        [AllowNull(true)]
        public System.Boolean? IS_IDENTITY
        {
            get
            {
                return this.is_identity;
            }
            set
            {
                this.is_identity = value;
            }
        }
        #endregion

        #region public System.Boolean? IS_PRIMARY_KEY
        /// <summary>
        /// <para>Sem descrição informada.</para>
        /// <para>Permite nulo.</para>
        /// </summary>
        [Identity(false)]
        [PrimaryKey(false)]
        [AllowNull(true)]
        public System.Boolean? IS_PRIMARY_KEY
        {
            get
            {
                return this.is_primary_key;
            }
            set
            {
                this.is_primary_key = value;
            }
        }
        #endregion

        #region public System.String TYPE
        /// <summary>
        /// <para>Sem descrição informada.</para>
        /// <para>Permite nulo.</para>
        /// </summary>
        [Identity(false)]
        [PrimaryKey(false)]
        [AllowNull(false)]
        public System.String TYPE
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
        #endregion

        #region public System.String TABLE_REF
        /// <summary>
        /// <para>Sem descrição informada.</para>
        /// <para>Permite nulo.</para>
        /// </summary>
        [Identity(false)]
        [PrimaryKey(false)]
        [AllowNull(true)]
        public System.String TABLE_REF
        {
            get
            {
                return this.table_ref;
            }
            set
            {
                this.table_ref = value;
            }
        }
        #endregion

        #region public System.String COLUMN_REF
        /// <summary>
        /// <para>Sem descrição informada.</para>
        /// <para>Permite nulo.</para>
        /// </summary>
        [Identity(false)]
        [PrimaryKey(false)]
        [AllowNull(true)]
        public System.String COLUMN_REF
        {
            get
            {
                return this.column_ref;
            }
            set
            {
                this.column_ref = value;
            }
        }
        #endregion

        #region public System.String FULL_DESC
        /// <summary>
        /// <para>Sem descrição informada.</para>
        /// <para>Permite nulo.</para>
        /// </summary>
        [IgnoreProperty()]
        public System.String FULL_DESC
        {
            get
            {
                return this.table + Punctuation.DOT + this.column;
            }
        }
        #endregion

        #endregion

        #region Definition
        public class Definition
        {
            public const string TABLE = "TABLE";
            public const string COLUMN = "COLUMN";
            public const string DESCRIPTION = "DESCRIPTION";
            public const string LENGTH = "LENGTH";
            public const string PRECISION = "PRECISION";
            public const string SCALE = "SCALE";
            public const string ALLOW_NULL = "ALLOW_NULL";
            public const string IS_IDENTITY = "IS_IDENTITY";
            public const string IS_PRIMARY_KEY = "IS_PRIMARY_KEY";
            public const string TYPE = "TYPE";
            public const string TABLE_REF = "TABLE_REF";
            public const string COLUMN_REF = "COLUMN_REF";
        }
        #endregion

        public ColumnSchema ToColumnSchema(TableSchema tableSchema)
        {
            this.CleanType();

            ColumnSchema columnShema = new ColumnSchema(tableSchema);
            columnShema.AllowNull = this.ALLOW_NULL.Value;
            columnShema.IsIdentity = this.IS_IDENTITY.Value;
            columnShema.IsPrimaryKey = this.IS_PRIMARY_KEY.Value;
            columnShema.MaxLength = this.LENGTH.GetValueOrDefault(0);
            columnShema.Name = this.COLUMN;
            columnShema.NumberPrecision = this.PRECISION.GetValueOrDefault(0);
            columnShema.NumberScale = this.SCALE.GetValueOrDefault(0);
            columnShema.ReferenceTableName = this.TABLE_REF;
            columnShema.ReferenceColumnName = this.COLUMN_REF;

            columnShema.DBType = Providers.Provider.FirstInstance.ConvertInternalTypeToDbType(this.TYPE, columnShema.NumberPrecision, columnShema.NumberScale);

            tableSchema.Columns.Add(columnShema);
            return columnShema;
        }

        private void CleanType()
        {
            if (this.TYPE.Contains('('))
            {
                string[] strVars = this.TYPE.Split('(');
                this.TYPE = strVars[0];
                try
                {
                    if(this.PRECISION != 0)
                        this.PRECISION = Convert.ToInt32(strVars[1].Replace(")", string.Empty));
                }
                catch { this.PRECISION = 0; }
            }
        }
    }
}
