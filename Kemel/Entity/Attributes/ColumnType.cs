using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Entity.Attributes
{
    public enum ColumnType
    {
        Normal,
        OnlySelect,
        OnlyConstraint,
        None
    }
}
