using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Orm.Base
{
    public interface IColumnDefinition
    {
        string Name { get; set; }
        string Alias { get; set; }

        ITableDefinition Table { get; }
    }

}
