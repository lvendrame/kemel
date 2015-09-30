using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.Kuery.Constraint.Behaviors
{
    public interface IConstraintTypeProvider
    {
        void WhereBehavior();

        void AndBehavior();

        void OrBehavior();

        void NoneBehavior();
    }
}
