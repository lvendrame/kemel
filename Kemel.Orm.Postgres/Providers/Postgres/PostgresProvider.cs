using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Kemel.Orm.DataBase;
using Kemel.Orm.Providers;
using Npgsql;
using Kemel.Orm.NQuery.Builder;

namespace Kemel.Orm.Providers.Postgres
{
    public class PostgresProvider : Provider
    {
        /// <summary>
        /// Postgres Connection String omiting tnsnames.ora
        ///<para>Parâmetro 0 - Server </para>
        ///<para>Parâmetro 1 - Port </para>
        ///<para>Parâmetro 2 - Catalog (DataBase) </para>
        ///<para>Parâmetro 3 - Usuário </para>
        ///<para>Parâmetro 4 - Password </para>
        /// </summary>
        public const string PostgresDefaultDatabase = "postgres";
        public const string PostgresSystemDatabase = "('postgres', 'template0', 'template1')";
        public const string PostgresConnectionString = "Server={0};Port={1};Database={2};User Id={3};Password={4};";

        string strConnectionString = string.Empty;
        public override string GetConnectionString()
        {
            if (string.IsNullOrEmpty(strConnectionString))
            {

                strConnectionString = string.Format
                (
                    PostgresConnectionString,
                    this.Credential.DataSource,
                    this.Credential.Port,
                    this.Credential.Catalog,
                    this.Credential.User,
                    this.Credential.Password
                );
            }

            return strConnectionString;
        }

        private QueryBuilder qrbQueryBuilder = null;
        public override QueryBuilder QueryBuilder
        {
            get
            {
                if (this.qrbQueryBuilder == null)
                    this.qrbQueryBuilder = new PostgresQueryBuilder(this);
                return this.qrbQueryBuilder;
            }
        }

        public override EntityCrudBuilder EntityCrudBuilder
        {
            get { return new PostgresEntityCrudBuilder(this); }
        }

        public override void ResetConnections()
        {
            strConnectionString = string.Empty;
        }

        public override string TableList
        {
            get
            {
                StringBuilder stb = new StringBuilder();
                stb.AppendFormat(" SELECT tablename AS \"{0}\" ", "NAME");
                stb.Append("         FROM pg_tables ");
                stb.Append("        WHERE schemaname = 'public' ");
                stb.Append("        ORDER BY tablename ");
                return stb.ToString();
            }
        }

        public override string TableDefinition
        {
            get
            {
                StringBuilder stb = new StringBuilder();
                stb.AppendLine("  SELECT ");
                stb.AppendFormat("       C.TABLE_NAME AS \"{0}\", ", T_Column_Entity.Definition.TABLE);
                stb.AppendFormat("       C.COLUMN_NAME AS \"{0}\", ", T_Column_Entity.Definition.COLUMN);
                stb.AppendFormat("       C.GENERATION_EXPRESSION AS \"{0}\", ", T_Column_Entity.Definition.DESCRIPTION);
                stb.AppendFormat("       C.CHARACTER_MAXIMUM_LENGTH AS \"{0}\", ", T_Column_Entity.Definition.LENGTH);
                stb.AppendFormat("       C.NUMERIC_PRECISION AS \"{0}\", ", T_Column_Entity.Definition.PRECISION);
                stb.AppendFormat("       C.NUMERIC_SCALE AS \"{0}\", ", T_Column_Entity.Definition.SCALE);
                stb.AppendFormat("       C.IS_NULLABLE AS \"{0}\", ", T_Column_Entity.Definition.ALLOW_NULL);
                stb.AppendFormat("       C.IS_IDENTITY AS \"{0}\", ", T_Column_Entity.Definition.IS_IDENTITY);
                stb.Append("             CASE WHEN (SELECT 1 FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE KC WHERE KC.TABLE_NAME = C.TABLE_NAME AND KC.COLUMN_NAME = C.COLUMN_NAME) = 1 ");
                stb.Append("                  THEN 1 ");
                stb.Append("                  ELSE 0 ");
                stb.AppendFormat("        END AS \"{0}\", ", T_Column_Entity.Definition.IS_PRIMARY_KEY);
                stb.AppendFormat("       C.DATA_TYPE AS \"{0}\", ", T_Column_Entity.Definition.TYPE);
                stb.AppendFormat("       NULL AS \"{0}\", ", T_Column_Entity.Definition.TABLE_REF);
                stb.AppendFormat("       NULL  AS \"{0}\" ", T_Column_Entity.Definition.COLUMN_REF);
                stb.AppendFormat("  FROM INFORMATION_SCHEMA.COLUMNS C ");
                stb.Append("       WHERE C.TABLE_NAME = :tableName ");
                stb.Append("       ORDER BY C.ORDINAL_POSITION ");
                return stb.ToString();
            }
        }

        public override string AllTablesDefinition
        {
            get
            {
                return "SELECT DISTINCT " +
                        "utc.TABLE_NAME \"TABELA\",    " +
                        "utc.column_name \"COLUNA\",    " +
                        "ucc.comments \"DESCRICAO\",    " +
                        "utc.char_col_decl_length \"TAMANHO\",    " +
                        "utc.data_precision \"PRECISAO\",    " +
                        "utc.data_scale \"ESCALA\",    " +
                        "CASE    " +
                        "  WHEN utc.nullable = 'Y' THEN 1    " +
                        "  ELSE 0    " +
                        "END \"ACEITA_NULO\",    " +
                        "CASE    " +
                        "  WHEN ut.TRIGGER_NAME IS NULL THEN 0    " +
                        "  ELSE 1    " +
                        "END \"E_IDENTITY\",    " +
                        "CASE    " +
                        "  WHEN upkc.column_name IS NULL THEN 0    " +
                        "  ELSE 1    " +
                        "END \"E_PRIMARY_KEY\",    " +
                        "utc.DATA_TYPE \"TIPO\",    " +
                        "ufkc.TABELA_REF \"TABELA_REF\",    " +
                        "ufkc.COLUNA_REF \"COLUNA_REF\"  " +
                        "FROM user_tab_columns utc, user_col_comments ucc, USER_TRIGGER_COLS ut, " +
                        "    (select ufkc.*, ufkpkc.TABLE_NAME \"TABELA_REF\", ufkpkc.COLUMN_NAME \"COLUNA_REF\"  " +
                        "      FROM user_cons_columns ufkc, user_constraints ucfk, user_constraints ucfk_pk, user_cons_columns ufkpkc " +
                        "      WHERE ucfk.constraint_type = 'R' AND ufkc.constraint_name = ucfk.constraint_name " +
                        "      AND ucfk.r_constraint_name = ucfk_pk.constraint_name " +
                        "      AND ucfk_pk.constraint_type = 'P'  AND ucfk_pk.constraint_name = ufkpkc.constraint_name   " +
                        "    ) ufkc, " +
                        "  ( SELECT upkc.* FROM user_cons_columns upkc, user_constraints ucpk " +
                        "    WHERE ucpk.constraint_type = 'P' AND upkc.constraint_name = ucpk.constraint_name " +
                        "    ) upkc " +
                        "WHERE utc.TABLE_NAME = ucc.TABLE_NAME(+) and utc.column_name = ucc.column_name(+) " +
                        "AND utc.TABLE_NAME = ut.TABLE_NAME(+) and utc.column_name = ut.column_name(+) and ut.COLUMN_USAGE(+) = 'NEW OUT' " +
                        "AND utc.TABLE_NAME = ufkc.TABLE_NAME(+) and utc.column_name = ufkc.column_name(+) " +
                        "AND utc.TABLE_NAME = upkc.TABLE_NAME(+) and utc.column_name = upkc.column_name(+) " +
                        "ORDER BY 1, 2 ";
            }
        }

        public override string ViewList
        {
            get { return ("SELECT TABLE_NAME AS NAME FROM VIEWS ORDER BY TABLE_NAME"); }
        }

        public override string ViewDefinition
        {
            get
            {
                StringBuilder stb = new StringBuilder();
                stb.AppendLine(" SELECT DISTINCT ");
                stb.AppendFormat("      V.TABLE_NAME '{0}',", T_Column_Entity.Definition.TABLE);
                stb.AppendFormat("      C.COLUMN_NAME '{0}',", T_Column_Entity.Definition.COLUMN);
                stb.AppendFormat("      C.COLUMN_COMMENT '{0}',", T_Column_Entity.Definition.DESCRIPTION);
                stb.AppendFormat("      C.CHARACTER_MAXIMUM_LENGTH '{0}',", T_Column_Entity.Definition.LENGTH);
                stb.AppendFormat("      C.NUMERIC_PRECISION '{0}',", T_Column_Entity.Definition.PRECISION);
                stb.AppendFormat("      C.NUMERIC_SCALE '{0}',", T_Column_Entity.Definition.SCALE);
                stb.AppendFormat("      (CASE WHEN C.IS_NULLABLE = 'NO' THEN 0 ELSE 1 END) '{0}',", T_Column_Entity.Definition.ALLOW_NULL);
                stb.AppendFormat("      (CASE WHEN C.EXTRA = 'auto_increment' THEN 1 ELSE 0 END) '{0}',", T_Column_Entity.Definition.IS_IDENTITY);
                stb.AppendFormat("      (CASE WHEN C.COLUMN_KEY = 'PRI' THEN 1 ELSE 0 END) '{0}',", T_Column_Entity.Definition.IS_PRIMARY_KEY);
                stb.AppendFormat("      C.COLUMN_TYPE '{0}',", T_Column_Entity.Definition.TYPE);
                stb.AppendFormat("      NULL '{0}',", T_Column_Entity.Definition.TABLE_REF);
                stb.AppendFormat("      NULL '{0}'", T_Column_Entity.Definition.COLUMN_REF);
                stb.AppendLine("   FROM VIEWS V ");
                stb.AppendLine("  INNER JOIN COLUMNS C ON C.TABLE_SCHEMA = V.TABLE_SCHEMA ");
                stb.AppendLine("    AND C.TABLE_NAME = V.TABLE_NAME ");
                stb.AppendLine("  WHERE V.TABLE_SCHEMA NOT IN ('performance_schema', 'mysql') ");
                stb.AppendLine("    AND V.TABLE_NAME = ?tableName ");
                stb.AppendLine("  ORDER BY C.ORDINAL_POSITION ");
                return stb.ToString();
            }
        }

        public override string ProcedureList
        {
            get { return ("SELECT * FROM USER_OBJECTS WHERE OBJECT_TYPE = 'PROCEDURE' ORDER BY 1"); }
        }

        public override string ProcedureDefinition
        {
            get { throw new NotImplementedException(); }
        }

        public override string FunctionList
        {
            get { return ("SELECT * FROM USER_OBJECTS WHERE OBJECT_TYPE = 'FUNCTION' ORDER BY 1"); }
        }

        public override string FunctionDefinition
        {
            get { throw new NotImplementedException(); }
        }

        public override string AllDataBases
        {
            get
            {
                StringBuilder stb = new StringBuilder();
                stb.Append(" SELECT DISTINCT datname  ");
                stb.Append("   FROM pg_database ");
                stb.Append("  WHERE datname NOT IN ").Append(PostgresSystemDatabase);
                stb.Append("  ORDER BY datname ");
                return stb.ToString();
            }
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
                case "number":
                    return DbType.Decimal;
                case "float":
                    return DbType.Double;
                case "image":
                    return DbType.Object;
                case "integer":
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
                    return DbType.Object;
                case "varchar":
                    return DbType.String;
                case "variant":
                    return DbType.Object;
                case "xml":
                    return DbType.Xml;
                default:
                    return DbType.Object;
            }
        }

        public override string ConvertDbTypeToFinalDbType(System.Data.DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiString: return "VARCHAR";
                case DbType.Binary: return "VARBINARY";
                case DbType.Byte: return "NUMBER";
                case DbType.Boolean: return "BYTE";
                case DbType.Currency: return "DECIMAL";
                case DbType.Date: return "DATE";
                case DbType.DateTime: return "TIMESTAMP";
                case DbType.Decimal: return "DECIMAL";
                case DbType.Double: return "FLOAT";
                case DbType.Guid: return "UNIQUEIDENTIFIER";
                case DbType.Int16: return "NUMBER";
                case DbType.Int32: return "NUMBER";
                case DbType.Int64: return "NUMBER";
                case DbType.Object: return "VARIANT";
                case DbType.Single: return "NUMBER";
                case DbType.String: return "NVARCHAR"; ;
                case DbType.AnsiStringFixedLength: return "CHAR";
                case DbType.StringFixedLength: return "NCHAR";
                default: throw new OrmException(string.Format("Unknow DbType. DbType: {0}", dbType));
            }
        }

        public override Kemel.Orm.Data.OrmConnection GetConnection()
        {
            Kemel.Orm.Data.OrmConnection connection =
                new Kemel.Orm.Data.OrmConnection
                (
                    new NpgsqlConnection(this.GetConnectionString())
                );
            return connection;
        }
    }
}
