using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Base
{
    public interface ITableDefinition
    {
        string Name { get; set; }

        string Alias { get; set; }
        IColumnDefinition GetColumn(string columnName);

        void AddColumn(IColumnDefinition column);
    }

}
