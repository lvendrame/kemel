using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Orm.Entity
{
    public enum EntityItemState
    {
        Added = 1,
        Deleted,
        Unchanged,
        Modified
    }
}
