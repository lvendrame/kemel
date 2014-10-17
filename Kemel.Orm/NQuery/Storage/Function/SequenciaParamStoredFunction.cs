using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.NQuery.Storage.Value;
using Kemel.Orm.Schema;
using Kemel.Orm.NQuery.Storage.Column;
using Kemel.Orm.Entity;
using Kemel.Orm.NQuery.Storage.Table;

namespace Kemel.Orm.NQuery.Storage.Function
{

    public class SequenciaParamStoredFunction : StoredFunction
    {

        public SequenciaParamStoredFunction(string name)
            : base(name)
        {
        }

        public SequenciaParamStoredFunction(string name, Query query)
            : base(name, query)
        {
        }

        public SequenciaParamStoredFunction(string name, string owner)
            : base(name, owner)
        {
        }

        public SequenciaParamStoredFunction(string name, string owner, Query query)
            : base(name, owner, query)
        {
        }

        private int intCurrentParamIndex = -1;
        public int CurrentParamIndex
        {
            get { return intCurrentParamIndex; }
        }

        public string CurrentParamName
        {
            get { return string.Concat("param_", intCurrentParamIndex); }
        }

        public string NextParamName
        {
            get
            {
                intCurrentParamIndex = intCurrentParamIndex + 1;
                return this.CurrentParamName;
            }
        }


        #region "Methods"

        public SequenciaParamStoredFunction SetValue(object objValue)
        {
            this.SetParameter(this.NextParamName).Equal(objValue);
            return this;
        }

        public SequenciaParamStoredFunction SetValue(ColumnSchema column)
        {
            this.SetParameter(this.NextParamName).Equal(column);
            return this;
        }

        public SequenciaParamStoredFunction SetValue(string columnName, TableSchema table)
        {
            this.SetParameter(this.NextParamName).Equal(columnName, table);
            return this;
        }

        public SequenciaParamStoredFunction SetValue<TEtt>(string columnName) where TEtt : EntityBase
        {
            this.SetParameter(this.NextParamName).Equal<TEtt>(columnName);
            return this;
        }

        public SequenciaParamStoredFunction SetValue(string tableName, string columnName)
        {
            this.SetParameter(this.NextParamName).Equal(tableName, columnName);
            return this;
        }

        public SequenciaParamStoredFunction SetValueConst(string columnName)
        {
            this.SetParameter(this.NextParamName).EqualC(columnName);
            return this;
        }

        public SequenciaParamStoredFunction NullValue()
        {
            this.SetParameter(this.NextParamName).NullValue();
            return this;
        }

        #endregion

    }

}
