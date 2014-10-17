using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Orm.QueryDef
{
    public class HavingConstraintCollection : List<HavingConstraint>
    {
        new public HavingConstraint Add(HavingConstraint item)
        {
            base.Add(item);
            return item;
        }
    }

    public class HavingConstraint
    {

    }
}
