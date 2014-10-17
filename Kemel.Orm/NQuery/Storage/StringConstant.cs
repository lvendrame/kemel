using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Kemel.Orm.NQuery.Storage
{

    public class StringConstant
    {

        public StringConstant(string constant)
        {
            this.strConstant = constant;
        }

        private string strConstant;
        public string Constant
        {
            get { return strConstant; }
            set { strConstant = value; }
        }

        public override string ToString()
        {
            return this.strConstant;
        }

        public StringConstant Apostrophe()
        {
            this.blnAddApostrophe = true;
            return this;
        }

        private bool blnAddApostrophe = false;
        public bool AddApostrophe
        {
            get { return blnAddApostrophe; }
            set { blnAddApostrophe = value; }
        }

    }

}
