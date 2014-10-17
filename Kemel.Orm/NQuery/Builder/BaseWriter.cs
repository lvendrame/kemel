using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.Data;
using Kemel.Orm.Providers;
using System.Text;

namespace Kemel.Orm.NQuery.Builder
{

    public abstract class BaseWriter
    {

        public BaseWriter(OrmCommand command)
        {
            cmdCommand = command;
        }

        public BaseWriter(Provider selectedProvider)
        {
            cmdCommand = selectedProvider.GetConnection().CreateCommand();
        }

        private OrmCommand cmdCommand;
        public OrmCommand Command
        {
            get { return cmdCommand; }
        }

        private StringBuilder stbFinalString;
        public StringBuilder FinalString
        {
            get
            {
                if ((stbFinalString == null))
                {
                    stbFinalString = new StringBuilder();
                }
                return stbFinalString;
            }
        }

        public void SetCommandText()
        {
            this.Command.CommandText = this.FinalString.ToString();
        }

    }

}
