using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.NQuery.Storage.Constraint;

namespace Kemel.Orm.NQuery.Storage.Join
{
    public class StoredJoinConstraintCollection : ConstraintBaseCollection<StoredJoin, StoredJoinConstraint>
    {
    }

    public class StoredJoinConstraint : ConstraintBase<StoredJoin>
    {
    }
}
