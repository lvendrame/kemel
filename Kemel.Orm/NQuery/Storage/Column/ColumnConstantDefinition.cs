using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Base;

namespace Kemel.Orm.NQuery.Storage.Column
{
    public class ColumnConstantDefinition: IColumnDefinition
    {
        public ColumnConstantDefinition(StringConstant constant)
        {
            stcConstant = constant;
            itdTable = null;
        }

        #region IColumnDefinition Members

        private StringConstant stcConstant;
        public string Name
        {
            get { return stcConstant.ToString(); }
            set { stcConstant.Constant = value; }
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

        #endregion
    }
}
