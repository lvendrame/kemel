using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.NQuery.Storage.Function;
using Kemel.Orm.Base;

namespace Kemel.Orm.NQuery.Storage.Table
{

    public class TableFunctionDefinition : ITableDefinition
    {

        public TableFunctionDefinition(StoredFunction stFunction)
        {
            this.stfFunction = stFunction;
        }

        private List<IColumnDefinition> lstColumns = new List<IColumnDefinition>();
        public List<IColumnDefinition> Columns
        {
            get { return lstColumns; }
            set { lstColumns = value; }
        }

        public void AddColumn(IColumnDefinition column)
        {
            this.lstColumns.Add(column);
        }

        public IColumnDefinition GetColumn(string columnName)
        {
            columnName = columnName.ToUpper();
            foreach (IColumnDefinition column in lstColumns)
            {
                if ((column.Name.ToUpper().Equals(columnName)))
                {
                    return column;
                }
            }
            return null;
        }

        private StoredFunction stfFunction;
        public StoredFunction StoredFunction
        {
            get { return stfFunction; }
            set { stfFunction = value; }
        }

        public string Name
        {
            get { return stfFunction.Name; }
            set { stfFunction.Name = value; }
        }

        private string strAlias;
        public string Alias
        {
            get { return strAlias; }
            set { strAlias = value; }
        }

    }

}
