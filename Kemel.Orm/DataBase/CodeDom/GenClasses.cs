using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Orm.DataBase.CodeDom
{
    public class GenClasses
    {
        public string BusinessClass { get; set; }
        public string DalClass { get; set; }
        public string EntityClass { get; set; }
        public string SchemaClass { get; set; }
        public string DefinitionClass { get; set; }
        public string ExtensionClass { get; set; }

        public string BusinessFileName { get; set; }
        public string DalFileName { get; set; }
        public string EntityFileName { get; set; }
    }
}
