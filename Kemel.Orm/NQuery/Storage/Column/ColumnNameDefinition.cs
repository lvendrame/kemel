using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.Base;

namespace Kemel.Orm.NQuery.Storage.Column
{

    public class ColumnNameDefinition : IColumnDefinition
    {

        public ColumnNameDefinition(string name, ITableDefinition table)
        {
            strName = name;
            itdTable = table;
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

        private ITableDefinition itdTable;
        public ITableDefinition Table
        {
            get { return itdTable; }
        }

    }

}
