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

    public class UpdateWriter : BaseWriter
    {

        public UpdateWriter(OrmCommand command)
            : base(command)
        {
        }

        public UpdateWriter(Provider selectedProvider)
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

        private StringBuilder stbSetValues;
        public StringBuilder SetValues
        {
            get
            {
                if ((stbSetValues == null))
                {
                    stbSetValues = new StringBuilder();
                }
                return stbSetValues;
            }
        }

        private StringBuilder stbConstraints;
        public StringBuilder Constraints
        {
            get
            {
                if ((stbConstraints == null))
                {
                    stbConstraints = new StringBuilder();
                }
                return stbConstraints;
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
