using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Schema;

namespace Kemel.Orm.DataBase.CodeDom
{
    public class GenItem
    {
        public GenItem()
        {
            this.Classes = new GenClasses();
        }

        public string SchemaName { get; set; }
        public bool IsEditableTable { get; set; }

        public TableSchema Schema { get; set; }
        public string BusinessName { get; set; }

        public bool GenerateDal { get; set; }
        public bool GenerateBusiness { get; set; }

        public GenClasses Classes { get; set; }
    }
}
