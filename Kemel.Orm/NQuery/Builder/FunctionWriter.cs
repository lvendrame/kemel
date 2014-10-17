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

    public class FunctionWriter : BaseWriter
    {

        public FunctionWriter(OrmCommand command)
            : base(command)
        {
        }

        public FunctionWriter(Provider selectedProvider)
            : base(selectedProvider)
        {
        }

        private StringBuilder stbName;
        public StringBuilder Name
        {
            get
            {
                if ((stbName == null))
                {
                    stbName = new StringBuilder();
                }
                return stbName;
            }
        }

        private StringBuilder stbParameters;
        public StringBuilder Parameters
        {
            get
            {
                if ((stbParameters == null))
                {
                    stbParameters = new StringBuilder();
                }
                return stbParameters;
            }
        }

    }

}
