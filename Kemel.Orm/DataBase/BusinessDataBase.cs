using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Base;
using Kemel.Orm.Data;
using System.Data;

namespace Kemel.Orm.DataBase
{
    public class BusinessDataBase : Business<T_Column_Entity, DalDataBase>
    {
        public List<T_Table_Entity> GetTableList()
        {
            OrmCommand command = this.CurrentProvider.GetConnection().CreateCommand();
            command.CommandText = this.CurrentProvider.TableList;
            return command.ExecuteList<T_Table_Entity>();
        }

        public List<T_Column_Entity> GetTableDefinition(string tableName)
        {
            OrmCommand command = this.CurrentProvider.GetConnection().CreateCommand();
            command.CommandText = string.Format(this.CurrentProvider.TableDefinition,
                this.CurrentProvider.QueryBuilder.DataBasePrefixVariable +
                this.CurrentProvider.QueryBuilder.CreateParameter(command, "tableName", tableName));
            return command.ExecuteList<T_Column_Entity>();
        }

        public List<T_Column_Entity> GetAllTableDefinition()
        {
            OrmCommand command = this.CurrentProvider.GetConnection().CreateCommand();
            command.CommandText = this.CurrentProvider.AllTablesDefinition;
            return command.ExecuteList<T_Column_Entity>();
        }

        public List<string> GetAllDataBase()
        {
            OrmCommand command = this.CurrentProvider.GetConnection().CreateCommand();
            command.CommandText = this.CurrentProvider.AllDataBases;
            DataTable dt = command.ExecuteDataTable();

            List<string> lstRet = new List<string>();

            foreach (DataRow row in dt.Rows)
            {
                lstRet.Add(row[0].ToString());
            }

            return lstRet;
        }
    }
}
