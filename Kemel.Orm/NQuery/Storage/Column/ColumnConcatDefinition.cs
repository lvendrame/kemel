using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.NQuery.Storage.StoredSelect;
using Kemel.Orm.Base;

namespace Kemel.Orm.NQuery.Storage.Column
{

    public class ColumnConcatDefinition : IColumnDefinition
    {

        public ColumnConcatDefinition(StoredConcat stConcat)
        {
            this.stcConcat = stConcat;
            itdTable = null;
        }

        private StoredConcat stcConcat;
        public StoredConcat Concat
        {
            get { return stcConcat; }
            set { stcConcat = value; }
        }

        public string Name
        {
            get { return stcConcat.Alias; }
            set { stcConcat.Alias = value; }
        }

        public string Alias
        {
            get { return stcConcat.Alias; }
            set { stcConcat.Alias = value; }
        }

        private ITableDefinition itdTable;
        public ITableDefinition Table
        {
            get { return itdTable; }
        }

    }

}
