using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Entity.Attributes;
using System.Reflection;
using Kemel.Entity;
using Kemel.Constants;
using Kemel.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kemel.Schema
{
    public class TableSchema : ITableDefinition
    {
        public TableSchema()
        {
            this.Columns = new ColumnSchemaCollection();
        }

        public TableSchema(Type entityType)
            : this()
        {
            this.EntityType = entityType;
            this.SetColumnsFromType();
        }

        #region Properties
        #region Name
        private string strName = null;
        public string Name
        {
            get
            {
                if (this.strName == null)
                    this.strName = TableSchema.GetTableName(this.EntityType);
                return this.strName;
            }
            set
            {
                this.strName = value;
            }
        }
        #endregion

        #region Alias
        private string strAlias = null;
        public string Alias
        {
            get
            {
                if (string.IsNullOrEmpty(this.strAlias) && this.EntityType != null)
                    this.strAlias = TableSchema.GetTableAlias(this.EntityType);
                return this.strAlias;
            }
            set
            {
                this.strAlias = value;
            }
        }
        #endregion

        #region SchemaType
        private SchemaType enmSchemaType = SchemaType.None;
        public SchemaType SchemaType
        {
            get
            {
                if (this.enmSchemaType == SchemaType.None)
                    this.enmSchemaType = TableSchema.GetSchemaType(this.EntityType);
                return this.enmSchemaType;
            }
            set
            {
                this.enmSchemaType = value;
            }
        }
        #endregion

        #region Owner
        private string strOwner = null;
        public string Owner
        {
            get
            {
                if (this.strOwner == null)
                    this.strOwner = TableSchema.GetOwner(this.EntityType);
                return this.strOwner;
            }
            set
            {
                this.strOwner = value;
            }
        }
        #endregion

        #region KeyProvider
        private string strKeyProvider = null;
        public string KeyProvider
        {
            get
            {
                if (this.strKeyProvider == null)
                    this.strKeyProvider = TableSchema.GetKeyProvider(this.EntityType);
                return this.strKeyProvider;
            }
            set
            {
                this.strKeyProvider = value;
            }
        }
        #endregion

        public Type EntityType { get; set; }

        public ColumnSchema clsLogicalExclusionColumn = null;
        public ColumnSchema LogicalExclusionColumn
        {
            get
            {
                if (!blnIsLogicalExclusion.HasValue)
                {
                    blnIsLogicalExclusion = false;
                    foreach (ColumnSchema column in this.Columns)
                    {
                        if (column.IsLogicalExclusionColumn)
                        {
                            clsLogicalExclusionColumn = column;
                            blnIsLogicalExclusion = true;
                            break;
                        }
                    }
                }
                return clsLogicalExclusionColumn;
            }
        }


        private bool? blnIsLogicalExclusion = null;
        public bool IsLogicalExclusion
        {
            get
            {
                if (!blnIsLogicalExclusion.HasValue)
                {
                    blnIsLogicalExclusion = false;
                    foreach (ColumnSchema column in this.Columns)
                    {
                        if (column.IsLogicalExclusionColumn)
                        {
                            clsLogicalExclusionColumn = column;
                            blnIsLogicalExclusion = true;
                            break;
                        }
                    }
                }
                return blnIsLogicalExclusion.Value;
            }
        }

        private ColumnSchemaCollection cscIdentityColumns = null;
        public ColumnSchemaCollection IdentityColumns
        {
            get
            {
                if (cscIdentityColumns == null)
                {
                    cscIdentityColumns = new ColumnSchemaCollection();

                    foreach (ColumnSchema column in this.Columns)
                    {
                        if (column.IsIdentity)
                            cscIdentityColumns.Add(column);
                    }
                }
                return cscIdentityColumns;
            }
        }
        #endregion

        #region Columns

        public ColumnSchemaCollection Columns { get; set; }

        public ColumnSchema this[int index]
        {
            get
            {
                return this.Columns[index];
            }
            set
            {
                this[index] = value;
            }
        }

        public ColumnSchema this[string name]
        {
            get
            {
                return this.Columns[name];
            }
            set
            {
                this[name] = value;
            }
        }

        #endregion

        #region SetColumnsFromType
        public void SetColumnsFromType()
        {
            foreach (PropertyInfo prop in this.EntityType.GetPublicDeclaredProperties())
            {
                if(ColumnSchema.IsColumn(prop))
                    this.Columns.Add(new ColumnSchema(prop, this));
            }
        }
        #endregion

        #region Validate
        ///// <summary>
        ///// Validate entity.
        ///// </summary>
        public void Validate(CrudOperation crudOperation, EntityBase entity)
        {
            foreach (ColumnSchema column in this.Columns)
            {
                column.ValidateField(crudOperation, entity);
            }
        }
        #endregion

        #region Static Methods

        #region GetTableAlias
        /// <summary>
        /// GetTableAlias
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static string GetTableAlias(Type entityType)
        {
            TableAliasAttribute att = entityType.GetCustomAttribute<TableAliasAttribute>();
            return (att != null) ? att.Alias : string.Empty;
        }
        #endregion

        #region GetTableAlias<TEtt>
        /// <summary>
        /// GetTableAlias
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static string GetTableAlias<TEtt>() where TEtt : EntityBase
        {
            return GetTableAlias(typeof(TEtt));
        }
        #endregion

        #region GetTableName
        /// <summary>
        /// GetTableName
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static string GetTableName(Type entityType)
        {
            TableAttribute att = entityType.GetCustomAttribute<TableAttribute>();
            if (att != null)
            {
                return att.Name;
            }
            else
            {
                return entityType.Name.Replace(Sufix.ENTITY_NAME, string.Empty);
            }
        }
        #endregion

        #region GetTableName<TEtt>
        /// <summary>
        /// GetTableName
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static string GetTableName<TEtt>() where TEtt : EntityBase
        {
            return GetTableName(typeof(TEtt));
        }
        #endregion

        #region GetOwner<TEtt>
        /// <summary>
        /// GetTableName
        /// </summary>
        /// <typeparam name="TEtt"></typeparam>
        /// <returns></returns>
        public static string GetOwner<TEtt>() where TEtt : EntityBase
        {
            OwnerAttribute att = typeof(TEtt).GetCustomAttribute<OwnerAttribute>();
            return (att != null) ? att.Owner : string.Empty;
        }
        #endregion

        #region GetOwner
        /// <summary>
        /// GetTableName
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static string GetOwner(Type entityType)
        {
            OwnerAttribute att = entityType.GetCustomAttribute<OwnerAttribute>();
            return (att != null) ? att.Owner : string.Empty;
        }
        #endregion

        #region GetPrimaryKeys
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public ColumnSchema[] GetPrimaryKeys()
        {
            List<ColumnSchema> lst = new List<ColumnSchema>();
            foreach (ColumnSchema column in this.Columns)
            {
                if (column.IsPrimaryKey)
                    lst.Add(column);
            }
            return lst.ToArray();
        }
        #endregion

        #region GetSchemaType
        /// <summary>
        /// GetSchemaType
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        private static SchemaType GetSchemaType(Type entityType)
        {
            TableSchemaTypeAttribute att = entityType.GetCustomAttribute<TableSchemaTypeAttribute>();
            return att != null ? att.SchemaType : SchemaType.Table;
        }
        #endregion

        #region GetKeyProvider
        /// <summary>
        /// GetTableAlias
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static string GetKeyProvider(Type entityType)
        {
            KeyProviderAttribute att = entityType.GetCustomAttribute<KeyProviderAttribute>();
            return (att != null) ? att.Key : string.Empty;
        }
        #endregion

        public static TableSchema FromEntity<TEtt>()
            where TEtt : EntityBase
        {
            return SchemaContainer.GetSchema<TEtt>();
        }

        public static TableSchema FromEntity(Type entityType)
        {
            return SchemaContainer.GetSchema(entityType);
        }

        public static TableSchema FromEntity(EntityBase entity)
        {
            return SchemaContainer.GetSchema(entity);
        }

        public static TableSchema FromTableName(string tableName)
        {
            Type tp = Type.GetType(tableName);

            if (tp == null)
                tp = Type.GetType(tableName + Sufix.ENTITY_NAME);

            if (tp == null)
            {
                Assembly entryAsm = Assembly.GetEntryAssembly();
                if (entryAsm != null)
                {
                    return FindTypeInAssembly(tableName, entryAsm);
                }
                else
                {
                    Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (Assembly asm in asms)
                    {
                        try
                        {
                            if (asm.ManifestModule.GetType().Namespace != "System.Reflection.Emit" && !asm.GlobalAssemblyCache)
                            {
                                TableSchema schema = FindTypeInAssembly(tableName, asm);
                                if (schema != null)
                                    return schema;
                            }
                        }
                        catch
                        {
                        }
                    }
                    return null;
                }
            }
            else
            {
                return new TableSchema(tp);
            }
        }

        public static TableSchema FindTypeInAssembly(string tableName, Assembly asm)
        {
            Type[] types = asm.GetExportedTypes();
            Type tp = FindEntityType(tableName, types);
            if (tp == null)
            {
                AssemblyName[] asms = asm.GetReferencedAssemblies();
                foreach (AssemblyName asmName in asms)
                {
                    types = Assembly.Load(asmName).GetExportedTypes();
                    tp = FindEntityType(tableName, types);
                    if (tp != null)
                        return new TableSchema(tp);
                }
                return null;
            }
            else
            {
                return new TableSchema(tp);
            }
        }

        private static Type FindEntityType(string tableName, Type[] types)
        {
            tableName = tableName.ToUpper();
            foreach (Type type in types)
            {
                if(tableName.Equals(TableSchema.GetTableName(type).ToUpper()))
                    return type;
            }
            return null;
        }

        #endregion

        #region ITableDefinition Members


        public IColumnDefinition GetColumn(string columnName)
        {
            return this[columnName];
        }

        public void AddColumn(IColumnDefinition column)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
