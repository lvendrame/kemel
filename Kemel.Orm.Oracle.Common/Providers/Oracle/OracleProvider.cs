using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Kemel.Orm.DataBase;
using Kemel.Orm.NQuery.Builder;

namespace Kemel.Orm.Providers.Oracle
{
    public abstract class OracleProvider: Provider
    {
        /// <summary>
        /// ORACLE Connection String omiting tnsnames.ora
        ///<para>Parâmetro 0 - DataSource </para>
        ///<para>Parâmetro 1 - Port </para>
        ///<para>Parâmetro 2 - Catalog (DataBase) </para>
        ///<para>Parâmetro 3 - Usuário </para>
        ///<para>Parâmetro 4 - Password </para>
        /// </summary>
        public const string OracleConnectionString = "DATA SOURCE=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVICE_NAME={2})));User Id={3};Password={4};";

        string strConnectionString = string.Empty;
        public override string GetConnectionString()
        {
            if (string.IsNullOrEmpty(strConnectionString))
            {
                strConnectionString = string.Format(
                    OracleConnectionString,
                    this.Credential.DataSource,
                    this.Credential.Port,
                    this.Credential.Catalog,
                    this.Credential.User,
                    this.Credential.Password);
            }
            return strConnectionString;
        }

        private QueryBuilder qrbQueryBuilder = null;
        public override QueryBuilder QueryBuilder
        {
            get
            {
                if (this.qrbQueryBuilder == null)
                    this.qrbQueryBuilder = new OracleQueryBuilder(this);
                return this.qrbQueryBuilder;
            }
        }

        public override EntityCrudBuilder EntityCrudBuilder
        {
            get { return new OracleEntityCrudBuilder(this); }
        }

        public override void ResetConnections()
        {
            strConnectionString = string.Empty;
        }

        public override string TableList
        {
            get { return ("SELECT TABLE_NAME AS NAME FROM ALL_TABLES ORDER BY 1"); }
        }

        public override string TableDefinition
        {
            get
            {
                StringBuilder stb = new StringBuilder();
                stb.AppendFormat(" SELECT DISTINCT ");
                stb.AppendFormat("        utc.TABLE_NAME \"{0}\", ", T_Column_Entity.Definition.TABLE);
                stb.AppendFormat("        utc.column_name \"{0}\", ", T_Column_Entity.Definition.COLUMN);
                stb.AppendFormat("        ucc.comments \"{0}\",  ", T_Column_Entity.Definition.DESCRIPTION);
                stb.AppendFormat("        utc.char_col_decl_length \"{0}\", ", T_Column_Entity.Definition.LENGTH);
                stb.AppendFormat("        utc.data_precision \"{0}\",  ", T_Column_Entity.Definition.PRECISION);
                stb.AppendFormat("        utc.data_scale \"{0}\",  ", T_Column_Entity.Definition.SCALE);
                stb.AppendFormat("        (CASE WHEN utc.nullable = 'Y' THEN 1 ELSE 0 END) \"{0}\",  ", T_Column_Entity.Definition.ALLOW_NULL);
                stb.AppendFormat("        (CASE WHEN ut.TRIGGER_NAME IS NULL THEN 0 ELSE 1 END) \"{0}\", ", T_Column_Entity.Definition.IS_IDENTITY);
                stb.AppendFormat("        (CASE WHEN upkc.column_name IS NULL THEN 0 ELSE 1 END) \"{0}\", ", T_Column_Entity.Definition.IS_PRIMARY_KEY);
                stb.AppendFormat("        utc.DATA_TYPE \"{0}\", ", T_Column_Entity.Definition.TYPE);
                stb.AppendFormat("        ufkc.TABELA_REF \"{0}\", ", T_Column_Entity.Definition.TABLE_REF);
                stb.AppendFormat("        ufkc.COLUNA_REF \"{0}\" ", T_Column_Entity.Definition.COLUMN_REF);
                stb.AppendLine("     FROM all_tab_columns utc, all_col_comments ucc, all_TRIGGER_COLS ut, ");
                stb.AppendLine("          (SELECT ufkc.*, ufkpkc.TABLE_NAME \"TABELA_REF\", ufkpkc.COLUMN_NAME \"COLUNA_REF\" ");
                stb.AppendLine("             FROM all_cons_columns ufkc, all_constraints ucfk, all_constraints ucfk_pk, all_cons_columns ufkpkc ");
                stb.AppendLine("            WHERE ucfk.constraint_type = 'R' AND ufkc.constraint_name = ucfk.constraint_name ");
                stb.AppendLine("              AND ucfk.r_constraint_name = ucfk_pk.constraint_name ");
                stb.AppendLine("              AND ucfk_pk.constraint_type = 'P'  AND ucfk_pk.constraint_name = ufkpkc.constraint_name) ufkc, ");
                stb.AppendLine("          (SELECT upkc.* FROM all_cons_columns upkc, all_constraints ucpk ");
                stb.AppendLine("            WHERE ucpk.constraint_type = 'P' AND upkc.constraint_name = ucpk.constraint_name) upkc ");
                stb.AppendLine("    WHERE utc.TABLE_NAME = ucc.TABLE_NAME(+) and utc.column_name = ucc.column_name(+) ");
                stb.AppendLine("      AND utc.TABLE_NAME = ut.TABLE_NAME(+) and utc.column_name = ut.column_name(+) and ut.COLUMN_USAGE(+) = 'NEW OUT' ");
                stb.AppendLine("      AND utc.TABLE_NAME = ufkc.TABLE_NAME(+) and utc.column_name = ufkc.column_name(+) ");
                stb.AppendLine("      AND utc.TABLE_NAME = upkc.TABLE_NAME(+) and utc.column_name = upkc.column_name(+) ");
                stb.AppendLine("      AND utc.TABLE_NAME = :tableName ");
                stb.AppendLine("    ORDER BY 1, 2 ");

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
            get { return ("SELECT VIEW_NAME AS NAME FROM USER_VIEWS ORDER BY 1"); }
        }

        public override string ViewDefinition
        {
            get
            {
                StringBuilder stb = new StringBuilder();
                stb.AppendFormat(" SELECT DISTINCT ");
                stb.AppendFormat("        utc.TABLE_NAME \"{0}\", ", T_Column_Entity.Definition.TABLE);
                stb.AppendFormat("        utc.column_name \"{0}\", ", T_Column_Entity.Definition.COLUMN);
                stb.AppendFormat("        ucc.comments \"{0}\",  ", T_Column_Entity.Definition.DESCRIPTION);
                stb.AppendFormat("        utc.char_col_decl_length \"{0}\", ", T_Column_Entity.Definition.LENGTH);
                stb.AppendFormat("        utc.data_precision \"{0}\",  ", T_Column_Entity.Definition.PRECISION);
                stb.AppendFormat("        utc.data_scale \"{0}\",  ", T_Column_Entity.Definition.SCALE);
                stb.AppendFormat("        (CASE WHEN utc.nullable = 'Y' THEN 1 ELSE 0 END) \"{0}\",  ", T_Column_Entity.Definition.ALLOW_NULL);
                stb.AppendFormat("        (CASE WHEN ut.TRIGGER_NAME IS NULL THEN 0 ELSE 1 END) \"{0}\", ", T_Column_Entity.Definition.IS_IDENTITY);
                stb.AppendFormat("        (CASE WHEN upkc.column_name IS NULL THEN 0 ELSE 1 END) \"{0}\", ", T_Column_Entity.Definition.IS_PRIMARY_KEY);
                stb.AppendFormat("        utc.DATA_TYPE \"{0}\", ", T_Column_Entity.Definition.TYPE);
                stb.AppendFormat("        ufkc.TABELA_REF \"{0}\", ", T_Column_Entity.Definition.TABLE_REF);
                stb.AppendFormat("        ufkc.COLUNA_REF \"{0}\" ", T_Column_Entity.Definition.COLUMN_REF);
                stb.AppendLine("     FROM user_tab_columns utc, user_col_comments ucc, USER_TRIGGER_COLS ut, ");
                stb.AppendLine("          (SELECT ufkc.*, ufkpkc.TABLE_NAME \"TABELA_REF\", ufkpkc.COLUMN_NAME \"COLUNA_REF\" ");
                stb.AppendLine("             FROM user_cons_columns ufkc, user_constraints ucfk, user_constraints ucfk_pk, user_cons_columns ufkpkc ");
                stb.AppendLine("            WHERE ucfk.constraint_type = 'R' AND ufkc.constraint_name = ucfk.constraint_name ");
                stb.AppendLine("              AND ucfk.r_constraint_name = ucfk_pk.constraint_name ");
                stb.AppendLine("              AND ucfk_pk.constraint_type = 'P'  AND ucfk_pk.constraint_name = ufkpkc.constraint_name) ufkc, ");
                stb.AppendLine("          (SELECT upkc.* FROM user_cons_columns upkc, user_constraints ucpk ");
                stb.AppendLine("            WHERE ucpk.constraint_type = 'P' AND upkc.constraint_name = ucpk.constraint_name) upkc ");
                stb.AppendLine("    WHERE utc.TABLE_NAME = ucc.TABLE_NAME(+) and utc.column_name = ucc.column_name(+) ");
                stb.AppendLine("      AND utc.TABLE_NAME = ut.TABLE_NAME(+) and utc.column_name = ut.column_name(+) and ut.COLUMN_USAGE(+) = 'NEW OUT' ");
                stb.AppendLine("      AND utc.TABLE_NAME = ufkc.TABLE_NAME(+) and utc.column_name = ufkc.column_name(+) ");
                stb.AppendLine("      AND utc.TABLE_NAME = upkc.TABLE_NAME(+) and utc.column_name = upkc.column_name(+) ");
                stb.AppendLine("      AND utc.TABLE_NAME = :tableName ");
                stb.AppendLine("    ORDER BY 1, 2 ");

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
            get { return "SELECT username FROM sys.All_Users order by username"; }
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
                    if (scale == 0)
                        return DbType.Int32;
                    else
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
                case "nvarchar2":
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
                    return DbType.DateTime;
                case "tinyint":
                    return DbType.Byte;
                case "udt":
                    return DbType.Object;
                case "uniqueidentifier":
                    return DbType.Guid;
                case "varbinary":
                    return DbType.Object;
                case "varchar2":
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
    }
}
