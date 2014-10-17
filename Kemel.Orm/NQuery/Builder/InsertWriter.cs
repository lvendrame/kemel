using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.Providers;
using System.Text;
using Kemel.Orm.Data;

namespace Kemel.Orm.NQuery.Builder
{

    public class InsertWriter : BaseWriter
    {

        public InsertWriter(OrmCommand command)
            : base(command)
        {
        }

        public InsertWriter(Provider selectedProvider)
            : base(selectedProvider)
        {
        }

        private StringBuilder stbIntoTable;
        public StringBuilder IntoTable
        {
            get
            {
                if ((stbIntoTable == null))
                {
                    stbIntoTable = new StringBuilder();
                }
                return stbIntoTable;
            }
        }

        private StringBuilder stbColumns;
        public StringBuilder Columns
        {
            get
            {
                if ((stbColumns == null))
                {
                    stbColumns = new StringBuilder();
                }
                return stbColumns;
            }
        }

        private StringBuilder stbValues;
        public StringBuilder Values
        {
            get
            {
                if ((stbValues == null))
                {
                    stbValues = new StringBuilder();
                }
                return stbValues;
            }
        }

        private SelectWriter slwSelect;
        public SelectWriter SelectW
        {
            get
            {
                if ((slwSelect == null))
                {
                    slwSelect = new SelectWriter(this.Command);
                }
                return slwSelect;
            }
        }

    }

}
