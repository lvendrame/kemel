using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.Base;

namespace Kemel.Orm.NQuery.Storage.Table
{
    public class TableNameDefinition : ITableDefinition
    {
        public TableNameDefinition(string name)
        {
            this.strName = name;
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

        private string strName;
        public string Name
        {
            get { return strName; }
            set { strName = value; }
        }

        private string strAlias;
        public string Alias
        {
            get { return strAlias; }
            set { strAlias = value; }
        }
    }
}
