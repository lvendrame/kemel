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
    public class DeleteWriter : BaseWriter
    {
        public DeleteWriter(OrmCommand command)
            : base(command)
        {
        }

        public DeleteWriter(Provider selectedProvider)
            : base(selectedProvider)
        {
        }

        private StringBuilder stbTable;
        public StringBuilder Table
        {
            get
            {
                if ((stbTable == null))
                {
                    stbTable = new StringBuilder();
                }
                return stbTable;
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
    }
}
