using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.DataBase;
using System.Data;
using Kemel.Orm.NQuery.Builder;

namespace Kemel.Orm.Providers.SqlServer
{
    public class SqlServerProvider: Provider
    {
        /// <summary>
        /// SQL Server Connection String using SQL Authentication
        ///<para>Parâmetro 0 - DataSource </para>
        ///<para>Parâmetro 1 - Catalog (DataBase) </para>
        ///<para>Parâmetro 2 - Usuário </para>
        ///<para>Parâmetro 3 - Password </para>
        ///<para>Parâmetro 4 - TimeOut </para>
        ///<para>Parâmetro 5 - Application Name </para>
        /// </summary>
        public const string SQL_Server_Authentication = "Data Source={0};Initial Catalog={1};User Id={2};Password={3}; Connection Timeout={4};Application Name={5};";

        /// <summary>
        /// SQL Server Connection String using Windows Authentication
        ///<para>Parâmetro 0 - DataSource </para>
        ///<para>Parâmetro 1 - Catalog (DataBase) </para>
        ///<para>Parâmetro 2 - Application Name </para>
        ///<para>Parâmetro 3 - TimeOut </para>
        /// </summary>
        public const string SQL_Server_Windows_Authentication = "Integrated Security=SSPI;Persist Security Info=False;Data Source={0};Initial Catalog={1};Application Name={2}; Connection Timeout={3};";


        public override Kemel.Orm.Data.OrmConnection GetConnection()
        {
            Kemel.Orm.Data.OrmConnection connection =
                new Kemel.Orm.Data.OrmConnection(
                    new System.Data.SqlClient.SqlConnection(this.GetConnectionString())
                );
            return connection;
        }

        string strConnectionString = null;
        public override string GetConnectionString()
        {
            if (string.IsNullOrEmpty(this.strConnectionString))
            {
                if (this.Credential.AuthenticationMode == AuthenticationMode.WindowsADUser)
                    this.strConnectionString = string.Format(SQL_Server_Windows_Authentication,
                                                                   this.Credential.DataSource,
                                                                   this.Credential.Catalog,
                                                                   this.Credential.ApplicationName,
                                                                   this.Credential.ConnectionTimeOut.ToString());
                else
                    this.strConnectionString = string.Format(SQL_Server_Authentication,
                                                                   this.Credential.DataSource,
                                                                   this.Credential.Catalog,
                                                                   this.Credential.User,
                                                                   this.Credential.Password,
                                                                   this.Credential.ConnectionTimeOut.ToString(),
                                                                   this.Credential.ApplicationName);
            }
            return strConnectionString;
        }

        private SqlServerQueryBuilder sqbQueryBuilder = null;
        public override QueryBuilder QueryBuilder
        {
            get
            {
                if (this.sqbQueryBuilder == null)
                    this.sqbQueryBuilder = new SqlServerQueryBuilder(this);
                return this.sqbQueryBuilder;
            }
        }

        public override void ResetConnections()
        {
            this.strConnectionString = null;
        }

        private SqlEntityCrudBuilder scbEntityCrudBuilder = null;
        public override EntityCrudBuilder EntityCrudBuilder
        {
            get
            {
                if (scbEntityCrudBuilder == null)
                    scbEntityCrudBuilder = new SqlEntityCrudBuilder(this);
                return scbEntityCrudBuilder;
            }
        }

        public override string TableList
        {
            get { return ("SELECT NAME FROM SYS.OBJECTS WHERE TYPE = 'u' ORDER BY NAME"); }
        }

        public override string TableDefinition
        {
            get
            {
                StringBuilder stb = new StringBuilder();
                stb.AppendFormat("SELECT DISTINCT SO1.name '{0}', ", T_Column_Entity.Definition.TABLE);
                stb.AppendFormat("SC1.name '{0}',  ", T_Column_Entity.Definition.COLUMN);
                stb.AppendFormat("SEP.VALUE '{0}',  ", T_Column_Entity.Definition.DESCRIPTION);
                stb.AppendFormat("SC1.MAX_LENGTH '{0}', ", T_Column_Entity.Definition.LENGTH);
                stb.AppendFormat("SC1.PRECISION '{0}',  ", T_Column_Entity.Definition.PRECISION);
                stb.AppendFormat("SC1.SCALE '{0}',  ", T_Column_Entity.Definition.SCALE);
                stb.AppendFormat("SC1.IS_NULLABLE '{0}',  ", T_Column_Entity.Definition.ALLOW_NULL);
                stb.AppendFormat("SC1.IS_IDENTITY '{0}', ", T_Column_Entity.Definition.IS_IDENTITY).AppendLine();
                stb.AppendLine("CASE ");
                stb.AppendLine("	WHEN SI.IS_PRIMARY_KEY IS NULL THEN CAST(0 AS BIT) ");
                stb.AppendLine("	ELSE SI.IS_PRIMARY_KEY ");
                stb.AppendFormat("END '{0}', ", T_Column_Entity.Definition.IS_PRIMARY_KEY);
                stb.AppendFormat("TP.NAME '{0}', ", T_Column_Entity.Definition.TYPE);
                stb.AppendFormat("SO2.name '{0}', ", T_Column_Entity.Definition.TABLE_REF);
                stb.AppendFormat("SC2.name '{0}' ", T_Column_Entity.Definition.COLUMN_REF);
                stb.AppendLine("FROM SYS.OBJECTS SO1 ");
                stb.AppendLine("INNER JOIN SYS.COLUMNS SC1 ");
                stb.AppendLine("on SC1.object_id = SO1.object_id ");
                stb.AppendLine("INNER JOIN SYS.TYPES TP ");
                stb.AppendLine("ON SC1.SYSTEM_TYPE_ID = TP.SYSTEM_TYPE_ID  AND SC1.USER_TYPE_ID = TP.USER_TYPE_ID ");
                stb.AppendLine("LEFT JOIN SYS.EXTENDED_PROPERTIES SEP  ");
                stb.AppendLine("ON SEP.MINOR_ID = SC1.COLUMN_ID AND SEP.MAJOR_ID = SC1.OBJECT_ID  ");
                stb.AppendLine("LEFT JOIN SYSFOREIGNKEYS SFK ");
                stb.AppendLine("on SFK.fkeyid = SO1.object_id AND SC1.column_id = SFK.fkey ");
                stb.AppendLine("LEFT JOIN SYS.OBJECTS SO2 ");
                stb.AppendLine("on SFK.rkeyid = SO2.object_id ");
                stb.AppendLine("LEFT JOIN SYS.COLUMNS SC2 ");
                stb.AppendLine("on SC2.object_id = SO2.object_id and SFK.rkeyid = SO2.object_id AND SC2.column_id = SFK.rkey ");
                stb.AppendLine("LEFT JOIN SYS.INDEX_COLUMNS SIC  ");
                stb.AppendLine("on SC1.column_id = SIC.column_id and SC1.object_id = SIC.object_id ");
                stb.AppendLine("LEFT JOIN SYS.INDEXES SI  ");
                stb.AppendLine("on SI.index_id = SIC.index_id and SI.object_id = SIC.object_id ");
                stb.AppendLine("WHERE SO1.type = 'u' ");
                stb.AppendLine("AND SO1.NAME = {0}");
                return stb.ToString();
            }
        }

        public override string AllTablesDefinition
        {
            get
            {
                StringBuilder stb = new StringBuilder();
                stb.AppendFormat("SELECT	SO1.name '{0}', ", T_Column_Entity.Definition.TABLE);
                stb.AppendFormat("SC1.name '{0}',  ", T_Column_Entity.Definition.COLUMN);
                stb.AppendFormat("SEP.VALUE '{0}',  ", T_Column_Entity.Definition.DESCRIPTION);
                stb.AppendFormat("SC1.MAX_LENGTH '{0}', ", T_Column_Entity.Definition.LENGTH);
                stb.AppendFormat("SC1.PRECISION '{0}',  ", T_Column_Entity.Definition.PRECISION);
                stb.AppendFormat("SC1.SCALE '{0}',  ", T_Column_Entity.Definition.SCALE);
                stb.AppendFormat("SC1.IS_NULLABLE '{0}',  ", T_Column_Entity.Definition.ALLOW_NULL);
                stb.AppendFormat("SC1.IS_IDENTITY '{0}', ", T_Column_Entity.Definition.IS_IDENTITY);
                stb.AppendLine("CASE ");
                stb.AppendLine("	WHEN SI.IS_PRIMARY_KEY IS NULL THEN CAST(0 AS BIT) ");
                stb.AppendLine("	ELSE SI.IS_PRIMARY_KEY ");
                stb.AppendFormat("END '{0}', ", T_Column_Entity.Definition.IS_PRIMARY_KEY);
                stb.AppendFormat("TP.NAME '{0}', ", T_Column_Entity.Definition.TYPE);
                stb.AppendFormat("SO2.name '{0}', ", T_Column_Entity.Definition.TABLE_REF);
                stb.AppendFormat("SC2.name '{0}' ", T_Column_Entity.Definition.COLUMN_REF);
                stb.AppendLine("FROM SYS.OBJECTS SO1 ");
                stb.AppendLine("INNER JOIN SYS.COLUMNS SC1 ");
                stb.AppendLine("on SC1.object_id = SO1.object_id ");
                stb.AppendLine("INNER JOIN SYS.TYPES TP ");
                stb.AppendLine("ON SC1.SYSTEM_TYPE_ID = TP.SYSTEM_TYPE_ID  AND SC1.USER_TYPE_ID = TP.USER_TYPE_ID ");
                stb.AppendLine("LEFT JOIN SYS.EXTENDED_PROPERTIES SEP  ");
                stb.AppendLine("ON SEP.MINOR_ID = SC1.COLUMN_ID AND SEP.MAJOR_ID = SC1.OBJECT_ID  ");
                stb.AppendLine("LEFT JOIN SYSFOREIGNKEYS SFK ");
                stb.AppendLine("on SFK.fkeyid = SO1.object_id AND SC1.column_id = SFK.fkey ");
                stb.AppendLine("LEFT JOIN SYS.OBJECTS SO2 ");
                stb.AppendLine("on SFK.rkeyid = SO2.object_id ");
                stb.AppendLine("LEFT JOIN SYS.COLUMNS SC2 ");
                stb.AppendLine("on SC2.object_id = SO2.object_id and SFK.rkeyid = SO2.object_id AND SC2.column_id = SFK.rkey ");
                stb.AppendLine("LEFT JOIN SYS.INDEX_COLUMNS SIC  ");
                stb.AppendLine("on SC1.column_id = SIC.column_id and SC1.object_id = SIC.object_id ");
                stb.AppendLine("LEFT JOIN SYS.INDEXES SI  ");
                stb.AppendLine("on SI.index_id = SIC.index_id and SI.object_id = SIC.object_id ");
                stb.AppendLine("WHERE SO1.type = 'u' ");
                stb.AppendLine("ORDER BY SO1.NAME, SC1.NAME");
                return stb.ToString();
            }
        }

        public override string ViewList
        {
            get { throw new NotImplementedException(); }
        }

        public override string ViewDefinition
        {
            get { throw new NotImplementedException(); }
        }

        public override string ProcedureList
        {
            get { return "SELECT SPECIFIC_NAME NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE'"; }
        }

        public override string ProcedureDefinition
        {
            get { throw new NotImplementedException(); }
        }

        public override string FunctionList
        {
            get { return "SELECT SPECIFIC_NAME NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'FUNCTION'"; }
        }

        public override string FunctionDefinition
        {
            get { throw new NotImplementedException(); }
        }

        public override string AllDataBases
        {
            get { return "select name from master..sysdatabases order by name"; }
        }

        public override System.Data.DbType ConvertInternalTypeToDbType(string internalType, int precision, int scale)
        {
            internalType = internalType.ToLower();

            switch (internalType)
            {
                case "bigint":
                    return DbType.Int64;
                case "binary":
                    return DbType.Byte;
                case "bit":
                    return DbType.Boolean;
                case "char":
                    return DbType.AnsiStringFixedLength;
                case "date":
                    return DbType.Date;
                case "datetime":
                    return DbType.DateTime;
                case "datetime2":
                    return DbType.DateTime2;
                case "datetimeoffset":
                    return DbType.DateTimeOffset;
                case "decimal":
                    return DbType.Decimal;
                case "float":
                    return DbType.Double;
                case "image":
                    return DbType.Object;
                case "int":
                    return DbType.Int32;
                case "money":
                    return DbType.Currency;
                case "nchar":
                    return DbType.StringFixedLength;
                case "ntext":
                    return DbType.String;
                case "nvarchar":
                    return DbType.String;
                case "real":
                    return DbType.Single;
                case "smalldatetime":
                    return DbType.DateTime;
                case "smallint":
                    return DbType.Int16;
                case "smallmoney":
                    return DbType.Decimal;
                case "structured":
                    return DbType.Object;
                case "text":
                    return DbType.AnsiString;
                case "time":
                    return DbType.Time;
                case "timestamp":
                    return DbType.Object;
                case "tinyint":
                    return DbType.Byte;
                case "udt":
                    return DbType.Object;
                case "uniqueidentifier":
                    return DbType.Guid;
                case "varbinary":
                    return DbType.Binary;
                case "varchar":
                    return DbType.String;
                case "variant":
                    return DbType.Object;
                case "xml":
                    return DbType.Xml;
                case "numeric":
                    if (scale == 0 && precision == 1)
                        return DbType.Byte;
                    else if (scale == 0)
                        return DbType.Int32;
                    else
                        return DbType.Decimal;
                default:
                    return DbType.Object;
            }
        }

        public override string ConvertDbTypeToFinalDbType(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiString: return "VARCHAR";
                case DbType.Binary: return "VARBINARY";
                case DbType.Byte: return "TINYINT";
                case DbType.Boolean: return "BIT";
                case DbType.Currency: return "MONEY";
                case DbType.Date: return "DATETIME";
                case DbType.DateTime: return "DATETIME";
                case DbType.Decimal: return "DECIMAL";
                case DbType.Double: return "FLOAT";
                case DbType.Guid: return "UNIQUEIDENTIFIER";
                case DbType.Int16: return "SMALLINT";
                case DbType.Int32: return "INT";
                case DbType.Int64: return "BIGINT";
                case DbType.Object: return "VARIANT";
                case DbType.Single: return "REAL";
                case DbType.String: return "NVARCHAR";;
                case DbType.AnsiStringFixedLength: return "CHAR";
                case DbType.StringFixedLength: return "NCHAR";
                default: throw new OrmException(string.Format("Unknow DbType. DbType: {0}", dbType));
            }

        }
    }

}
