using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using Kemel.Entity;
using Kemel.Entity.Attributes;
using Kemel.Constants;
using Kemel.Base;
using Kemel.Data;

namespace Kemel.Schema
{
    public class ColumnSchema : IColumnDefinition
    {
        public ColumnSchema(PropertyInfo propColumn, TableSchema tableSchema)
        {
            this.Property = propColumn;
            this.Parent = tableSchema;
        }

        public ColumnSchema(TableSchema tableSchema)
        {
            this.Property = null;
            this.Parent = tableSchema;
        }

        #region Properties

        #region Parent
        public TableSchema Parent { get; set; }
        #endregion

        #region Name
        private string _name = null;
        public string Name
        {
            get
            {
                if (_name == null)
                    this._name = ColumnSchema.GetColumnName(this.Property);
                return _name;
            }
            set
            {
                this._name = value;
            }
        }
        #endregion

        #region Alias
        private string _alias = null;
        public string Alias
        {
            get
            {
                if (string.IsNullOrEmpty(_alias) && this.Property != null)
                    this._alias = ColumnSchema.GetColumnAlias(this.Property);
                return _alias;
            }
            set
            {
                this._alias = value;
            }
        }
        #endregion

        #region Type
        private Type _type = null;
        public Type Type
        {
            get
            {
                if (_type == null)
                {
                    if (this.Property == null)
                        _type = ColumnSchema.GetType(this._dbType.Value);
                    else
                    {
                        _type = Nullable.GetUnderlyingType(this.Property.PropertyType);

                        if (_type == null)
                            _type = this.Property.PropertyType;
                    }
                }
                return _type;
            }
            set
            {
                _type = value;
            }
        }
        #endregion

        #region DBType
        private DbType? _dbType = null;
        public DbType DBType
        {
            get
            {
                if (!_dbType.HasValue)
                    _dbType = ColumnSchema.GetDbType(this._type);
                return _dbType.Value;
            }
            set
            {
                _dbType = value;
            }
        }
        #endregion

        #region AllowNull
        private bool? _allowNull = null;
        public bool AllowNull
        {
            get
            {
                if (!_allowNull.HasValue)
                    _allowNull = ColumnSchema.AllowNullProp(this.Property);
                return _allowNull.Value;
            }
            set
            {
                this._allowNull = value;
            }
        }
        #endregion

        #region IsIdentity
        private bool? _isIdentity = null;
        public bool IsIdentity
        {
            get
            {
                if (!_isIdentity.HasValue)
                    _isIdentity = ColumnSchema.IsIdentityProp(this.Property);
                return _isIdentity.Value;
            }
            set
            {
                this._isIdentity = value;
            }
        }
        #endregion

        #region IsPrimaryKey
        private bool? _isPrimaryKey = null;
        public bool IsPrimaryKey
        {
            get
            {
                if (!_isPrimaryKey.HasValue)
                    _isPrimaryKey = ColumnSchema.IsPrimaryKeyProp(this.Property);
                return _isPrimaryKey.Value;
            }
            set
            {
                this._isPrimaryKey = value;
            }
        }
        #endregion

        #region IsForeignKey
        private bool? _isForeignKey = null;
        public bool IsForeignKey
        {
            get
            {
                if (!_isForeignKey.HasValue)
                {
                    if (this.Property != null)
                        _isForeignKey = ColumnSchema.IsForeignKeyProp(this.Property);
                    else
                        _isForeignKey = !string.IsNullOrEmpty(this._referenceTableName);
                }
                return _isForeignKey.Value;
            }
            set
            {
                this._isForeignKey = value;
            }
        }
        #endregion

        #region IsLogicalExclusionColumn
        private bool? _isLogicalExclusionColumn = null;
        public bool IsLogicalExclusionColumn
        {
            get
            {
                if (!_isLogicalExclusionColumn.HasValue)
                {
                    if (this.Property != null)
                        _isLogicalExclusionColumn = ColumnSchema.IsLogicalExclusionColumnProp(this.Property);
                    else
                        _isLogicalExclusionColumn = false;
                }
                return _isLogicalExclusionColumn.Value;
            }
            set
            {
                this._isLogicalExclusionColumn = value;
            }
        }
        #endregion

        #region NumberScale
        private int? _numberScale;
        public int NumberScale
        {
            get
            {
                if (this._numberScale.HasValue)
                    return this._numberScale.Value;
                return -1;
            }
            set
            {
                this._numberScale = value;
            }
        }
        #endregion

        #region NumberPrecision
        private int? _numberPrecision;
        public int NumberPrecision
        {
            get
            {
                if (this._numberPrecision.HasValue)
                    return this._numberPrecision.Value;
                return -1;
            }
            set
            {
                this._numberPrecision = value;
            }
        }
        #endregion

        #region MaxLength
        private int? _maxLength;
        public int MaxLength
        {
            get
            {
                if (!this._maxLength.HasValue)
                {
                    if (this.Property != null)
                        this._maxLength = ColumnSchema.GetMaxLength(this.Property);
                    else
                        this._maxLength = 10;
                }
                return this._maxLength.Value;
            }
            set
            {
                this._maxLength = value;
            }
        }
        #endregion

        #region ReferenceTableName
        private string _referenceTableName = null;
        public string ReferenceTableName
        {
            get
            {
                if (_referenceTableName == null)
                {
                    this.SetReferences();
                }
                return _referenceTableName;
            }
            set
            {
                _referenceTableName = value;
            }
        }
        #endregion

        #region ReferenceColumnName
        private string _referenceColumnName = null;
        public string ReferenceColumnName
        {
            get
            {
                if (_referenceColumnName == null)
                {
                    this.SetReferences();
                }
                return _referenceColumnName;
            }
            set
            {
                _referenceColumnName = value;
            }
        }
        #endregion

        #region IgnoreColumn
        private bool? _ignoreColumn = null;
        public bool IgnoreColumn
        {
            get
            {
                if (!this._ignoreColumn.HasValue)
                {
                    this._ignoreColumn = this.Property == null ? false : ColumnSchema.IgnoreProperty(this.Property);
                }
                return this._ignoreColumn.Value;
            }
            set
            {
                this._ignoreColumn = value;
            }
        }
        #endregion

        #region ParameterDirection
        private ParameterDirection? _paramDirection = null;
        public ParameterDirection ParamDirection
        {
            get
            {
                if (!this._paramDirection.HasValue)
                {
                    if (this.Property != null)
                    {
                        this._paramDirection = ColumnSchema.GetParameterDirection(this.Property);
                    }
                    else
                    {
                        this._paramDirection = ParameterDirection.Input;
                    }
                }
                return this._paramDirection.Value;
            }
            set
            {
                this._paramDirection = value;
            }
        }
        #endregion

        #region QualifiedName
        public string QualifiedName
        {
            get
            {
                return String.Concat(this.Parent.Name, Punctuation.DOT, this.Name);
            }
        }
        #endregion

        #region SequenceName
        private string _sequenceName = null;
        public string SequenceName
        {
            get
            {
                if (_sequenceName == null)
                    this._sequenceName = ColumnSchema.GetSequenceName(this.Property);
                return _sequenceName;
            }
            set
            {
                this._sequenceName = value;
            }
        }
        #endregion

        #region Property
        public PropertyInfo Property { get; private set; }
        #endregion

        #region UseType
        private ColumnType _useType = ColumnType.None;
        public ColumnType UseType
        {
            get
            {
                if (_useType == ColumnType.None)
                    this._useType = ColumnSchema.GetColumnUseType(this.Property);
                return _useType;
            }
            set
            {
                this._useType = value;
            }
        }
        #endregion

        #region Converter
        private ITypeConverter _converter = null;
        public ITypeConverter Converter
        {
            get
            {
                if (_converter == null)
                    this._converter = ColumnSchema.GetConverter(this.Property);
                return _converter;
            }
            set
            {
                _converter = value;
            }
        }
        #endregion

        #endregion

        #region Methods
        private void SetReferences()
        {
            if (this.IsForeignKey && this.Property != null)
            {
                ForeignKeyAttribute fkAtt = this.Property.GetCustomAttribute<ForeignKeyAttribute>();
                this._referenceColumnName = fkAtt.PropertyColumn;
                this._referenceTableName = TableSchema.GetTableName(fkAtt.EntityType);
            }
            else
            {
                this._referenceColumnName = string.Empty;
                this._referenceTableName = string.Empty;
            }
        }

        #region ValidateField
        /// <summary>
        /// Validate entity.
        /// </summary>
        public void ValidateField(EntityBase entity)
        {
            object value;
            // Se o campo não for auto-numerado
            if (!this.IsIdentity)
            {
                // Se o campo não permite nulo
                if (!this.AllowNull)
                {
                    // Se o valor do campo é nulo ou está em branco, gera a exceção
                    value = this.GetValue(entity);
                    if (value == null || value.ToString().Length == 0)
                    {
                        throw new OrmException(string.Format(Messages.FieldNotEmpty, this.Name));
                    }
                }
            }

            // Se o campo tem tamanho máximo definido
            if (this.MaxLength != 0)
            {
                value = this.GetValue(entity);
                if (value != null && value.ToString().Length > this.MaxLength)
                {
                    throw new OrmException(string.Format(Messages.FieldGreaterThanMaxLength, this.Name, this.MaxLength));
                }
            }
        }
        #endregion

        #region ValidateField
        /// <summary>
        /// Validate entity.
        /// </summary>
        public void ValidateField(object obj)
        {
            object value;
            // Se o campo não for auto-numerado
            if (!this.IsIdentity)
            {
                // Se o campo não permite nulo
                if (!this.AllowNull)
                {
                    // Se o valor do campo é nulo ou está em branco, gera a exceção
                    value = this.GetValue(obj);
                    if (value == null || value.ToString().Length == 0)
                    {
                        throw new OrmException(string.Format(Messages.FieldNotEmpty, this.Name));
                    }
                }
            }

            // Se o campo tem tamanho máximo definido
            if (this.MaxLength != 0)
            {
                value = this.GetValue(obj);
                if (value != null && value.ToString().Length > this.MaxLength)
                {
                    throw new OrmException(string.Format(Messages.FieldGreaterThanMaxLength, this.Name, this.MaxLength));
                }
            }
        }
        #endregion

        #region Static Methods

        #region ToColumnPatternName
        /// <summary>
        /// Returns defined pattern column name
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static string ToColumnPatternName(string columnName)
        {
            return columnName.ToUpper();
        }
        #endregion

        #region AllowNullProp
        /// <summary>
        /// AllowNullProp
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static bool AllowNullProp(PropertyInfo propInfo)
        {
            bool blnAllow = false;
            AllowNullAttribute att = propInfo.GetCustomAttribute<AllowNullAttribute>();
            if (att != null)
                blnAllow = att.AllowNull;

            return blnAllow;
        }
        #endregion

        #region IsIdentityProp
        /// <summary>
        /// IsIdentityProp
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static bool IsIdentityProp(PropertyInfo propInfo)
        {
            bool blnIdentity = false;

            IdentityAttribute att = propInfo.GetCustomAttribute<IdentityAttribute>();
            if (att != null)
                blnIdentity = att.IsIdentity;

            return blnIdentity;
        }
        #endregion

        #region IgnoreProperty
        /// <summary>
        /// Ignore Property
        /// </summary>
        /// <returns></returns>
        public static bool IgnoreProperty(PropertyInfo propInfo)
        {
            return propInfo.GetCustomAttribute<IgnorePropertyAttribute>() != null;
        }
        #endregion

        #region IsPrimaryKeyProp
        /// <summary>
        /// IsPrimaryKeyProp
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static bool IsPrimaryKeyProp(PropertyInfo propInfo)
        {
            bool blnKey = false;

            PrimaryKeyAttribute att = propInfo.GetCustomAttribute<PrimaryKeyAttribute>();
            if (att != null)
                blnKey = att.IsPrimaryKey;

            return blnKey;
        }
        #endregion

        #region IsLogicalExclusionColumnProp
        /// <summary>
        /// IsLogicalExclusionColumn
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static bool IsLogicalExclusionColumnProp(PropertyInfo propInfo)
        {
            bool blnKey = false;

            LogicalExclusionColumnAttribute att = propInfo.GetCustomAttribute<LogicalExclusionColumnAttribute>();
            if (att != null)
                blnKey = true;

            return blnKey;
        }
        #endregion

        #region IsColumn
        /// <summary>
        /// IsLogicalExclusionColumn
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static bool IsColumn(PropertyInfo propInfo)
        {
            bool blnKey = true;

            IsNotColumnAttribute att = propInfo.GetCustomAttribute<IsNotColumnAttribute>();
            if (att != null)
                blnKey = false;

            return blnKey;
        }
        #endregion

        #region GetMaxLength
        /// <summary>
        /// GetMaxLength
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static int GetMaxLength(PropertyInfo propInfo)
        {
            int intMaxLength = 0;

            MaxLengthAttribute att = propInfo.GetCustomAttribute<MaxLengthAttribute>();
            if (att != null)
                intMaxLength = att.Length;

            return intMaxLength;
        }
        #endregion

        #region GetColumnName
        /// <summary>
        /// GetColumnName
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static string GetColumnName(PropertyInfo propInfo)
        {
            return propInfo.Name;
        }
        #endregion

        #region GetColumnAlias
        /// <summary>
        /// GetColumnAlias
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static string GetColumnAlias(PropertyInfo propInfo)
        {
            string alias = string.Empty;

            ColumnAliasAttribute att = propInfo.GetCustomAttribute<ColumnAliasAttribute>();
            if (att != null)
                alias = att.Alias;

            return alias;
        }
        #endregion

        #region GetSequenceName
        /// <summary>
        /// GetSequenceName
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static string GetSequenceName(PropertyInfo propInfo)
        {
            string seqName = string.Empty;

            SequenceAttribute att = propInfo.GetCustomAttribute<SequenceAttribute>();
            if (att != null)
                seqName = att.SequenceName;

            return seqName;
        }
        #endregion

        #region IsForeignKeyProp
        /// <summary>
        /// IsForeignKey
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static bool IsForeignKeyProp(PropertyInfo propInfo)
        {
            return (propInfo.GetCustomAttribute<ForeignKeyAttribute>() != null);
        }
        #endregion

        #region IsForeignKeyFromEntity
        /// <summary>
        /// IsForeignKeyFromEntity
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static bool IsForeignKeyFromEntity(PropertyInfo propInfo, EntityBase entity)
        {
            bool isForeignKey = false;

            ForeignKeyAttribute att = propInfo.GetCustomAttribute<ForeignKeyAttribute>();
            if (att != null)
                isForeignKey = att.EntityType == entity.GetType();

            return isForeignKey;
        }
        #endregion

        #region IsForeignKeyFromEntity
        /// <summary>
        /// IsForeignKeyFromEntity
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static bool IsForeignKeyFromEntity<TEtt>(PropertyInfo propInfo)
            where TEtt : EntityBase
        {
            bool isForeignKey = false;

            ForeignKeyAttribute att = propInfo.GetCustomAttribute<ForeignKeyAttribute>();
            if (att != null)
                isForeignKey = att.EntityType == typeof(TEtt);

            return isForeignKey;
        }
        #endregion

        #region GetParameterDirection
        /// <summary>
        /// GetParameterDirection
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static ParameterDirection GetParameterDirection(PropertyInfo propInfo)
        {
            ParameterDirection enmDirection = ParameterDirection.Input;

            ParameterDirectionAttribute att = propInfo.GetCustomAttribute<ParameterDirectionAttribute>();
            if (att != null)
                enmDirection = att.Diretion;

            return enmDirection;
        }
        #endregion

        #region GetColumnUseType
        /// <summary>
        /// GetColumnUseType
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static ColumnType GetColumnUseType(PropertyInfo propInfo)
        {
            ColumnType enmColumnUseType = ColumnType.None;

            ColumnUseTypeAttribute att = propInfo.GetCustomAttribute<ColumnUseTypeAttribute>();
            if (att != null)
                enmColumnUseType = att.Type;

            return enmColumnUseType;
        }
        #endregion

        #region GetConverter
        /// <summary>
        /// GetConverter
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static ITypeConverter GetConverter(PropertyInfo propInfo)
        {
            ConverterAttribute att = propInfo.GetCustomAttribute<ConverterAttribute>();
            if (att != null)
                return att.Converter;

            return null;
        }
        #endregion

        #endregion

        public static Type GetType(string dbType, int precision, int scale)
        {
            dbType = dbType.ToLower();
            Type ret = null;

            switch (dbType)
            {
                case "text":
                case "varchar":
                case "varchar2":
                case "long":
                case "char": ret = typeof(System.String);
                    break;
                case "int": ret = typeof(System.Int32?);
                    break;
                case "tinyint": ret = typeof(System.Byte?);
                    break;
                case "decimal": ret = typeof(System.Decimal?);
                    break;
                case "number":
                case "numeric":
                    if (scale == 0 && precision == 1)
                        ret = typeof(System.Byte?);
                    else if (scale == 0)
                        ret = typeof(System.Int32?);
                    else
                        ret = typeof(System.Decimal?);
                    break;
                case "date":
                case "smalldatetime":
                case "timestamp":
                case "datetime": ret = typeof(System.DateTime?);
                    break;
                case "bit": ret = typeof(System.Boolean?);
                    break;
            }
            return ret;
        }

        public static Type GetType(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.Xml:
                case DbType.StringFixedLength:
                    return typeof(System.String);
                case DbType.Binary:
                    return typeof(System.Byte[]);
                case DbType.Boolean:
                    return typeof(System.Boolean?);
                case DbType.Byte:
                    return typeof(System.Byte?);
                case DbType.Date:
                case DbType.DateTime:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                    return typeof(System.DateTime?);
                case DbType.Decimal:
                case DbType.Currency:
                case DbType.Double:
                case DbType.VarNumeric:
                case DbType.Single:
                    return typeof(System.Double?);
                case DbType.Guid:
                    return typeof(System.Guid?);
                case DbType.Int16:
                    return typeof(System.Int16?);
                case DbType.Int32:
                    return typeof(System.Int32?);
                case DbType.Int64:
                    return typeof(System.Int64?);
                case DbType.Object:
                    return typeof(System.Object);
                case DbType.SByte:
                    return typeof(System.SByte?);
                case DbType.Time:
                    return typeof(System.TimeSpan?);
                case DbType.UInt16:
                    return typeof(System.UInt16?);
                case DbType.UInt32:
                    return typeof(System.UInt32?);
                case DbType.UInt64:
                    return typeof(System.UInt64?);
                default:
                    return typeof(System.Object);
            }
        }

        public static DbType GetDbType(Type type)
        {
            if (type == typeof(string))
            {
                return DbType.AnsiString;
            }
            else if (type == typeof(int))
            {
                return DbType.Int32;
            }
            else if (type == typeof(double))
            {
                return DbType.Decimal;
            }
            else if (type == typeof(decimal))
            {
                return DbType.Decimal;
            }
            else if (type == typeof(bool))
            {
                return DbType.Boolean;
            }
            else if (type == typeof(DateTime))
            {
                return DbType.DateTime;
            }
            else if (type == typeof(char))
            {
                return DbType.AnsiString;
            }
            else if (type == typeof(short))
            {
                return DbType.Int16;
            }
            else if (type == typeof(long))
            {
                return DbType.Int64;
            }
            else if (type == typeof(float))
            {
                return DbType.Double;
            }
            else if (type == typeof(Guid))
            {
                return DbType.Guid;
            }
            else if (type == typeof(byte))
            {
                return DbType.Byte;
            }
            else if (type == typeof(object))
            {
                return DbType.Object;
            }
            else if (type == typeof(byte[]))
            {
                return DbType.Binary;
            }
            else if (type == typeof(TimeSpan))
            {
                return DbType.Time;
            }
            else if (type == typeof(DBNull))
            {
                return DbType.AnsiString;
            }
            else
            {
                return DbType.Object;
            }
        }

        public object GetValue(object ett)
        {
            return Property.GetValue(ett, null);
        }

        public Nullable<T> GetValueS<T>(object ett)
            where T : struct
        {
            object value = this.Property.GetValue(ett, null);
            if (value == null)
            {
                return null;
            }
            else if (this.Property.PropertyType == typeof(T))
            {
                return (T)value;
            }
            else
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
        }

        public T GetValueC<T>(object ett)
            where T : class
        {
            object value = this.Property.GetValue(ett, null);
            if (value == null)
            {
                return null;
            }
            else if (this.Property.PropertyType == typeof(T))
            {
                return (T)value;
            }
            else
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
        }

        public object GetValue(object ett, Type tp)
        {
            object value = this.Property.GetValue(ett, null);
            if (value == null)
            {
                return null;
            }
            else if (this.Property.PropertyType == tp)
            {
                return value;
            }
            else
            {
                return Convert.ChangeType(value, tp);
            }
        }

        public void SetValue(object ett, object value)
        {
            if (value == null || Convert.IsDBNull(value))
            {
                this.Property.SetValue(ett, null, null);
            }
            else if (this.Type == value.GetType())
            {
                this.Property.SetValue(ett, value, null);
            }
            else
            {
                this.Property.SetValue(ett, Convert.ChangeType(value, this.Type), null);
            }
        }

        #endregion

        #region IColumnDefinition Members


        public ITableDefinition Table
        {
            get { return this.Parent; }
        }

        #endregion
    }
}
