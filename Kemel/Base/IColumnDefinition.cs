using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Base
{
    public interface IColumnDefinition
    {
        string Name { get; set; }
        string Alias { get; set; }

        ITableDefinition Table { get; }
    }

}
