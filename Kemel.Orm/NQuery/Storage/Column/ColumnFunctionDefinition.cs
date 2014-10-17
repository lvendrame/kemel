using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.NQuery.Storage.Function;
using Kemel.Orm.Base;

namespace Kemel.Orm.NQuery.Storage.Column
{

    public class ColumnFunctionDefinition : IColumnDefinition
    {

        public ColumnFunctionDefinition(StoredFunction stFunction, ITableDefinition table)
        {
            this.stfFunction = stFunction;
            itdTable = table;
        }

        public ColumnFunctionDefinition(StoredFunction stFunction)
        {
            this.stfFunction = stFunction;
            itdTable = null;
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

        public string Alias
        {
            get { return stfFunction.Alias; }
            set { stfFunction.Alias = value; }
        }

        private ITableDefinition itdTable;
        public ITableDefinition Table
        {
            get { return itdTable; }
        }

    }

}
