using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.Base;

namespace Kemel.Orm.NQuery.Storage.Column
{

    public class ColumnAliasDefinition : IColumnDefinition
    {

        public ColumnAliasDefinition(string nickName, ITableDefinition table)
        {
            strAlias = nickName;
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
