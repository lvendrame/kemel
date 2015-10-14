using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.Kuery.Constraint.Behaviors
{
    public class ConstraintTypeBehavior: ProviderContainer<IConstraintTypeProvider, ConstraintType>
    {
        public ConstraintTypeBehavior(IConstraintTypeProvider constraintTypeProvider)
            : base(constraintTypeProvider)
        {
        }

        public override void DoBehavior(ConstraintType enumType)
        {
            switch (enumType)
            {
                case ConstraintType.Where:
                    this.Provider.WhereBehavior();
                    break;
                case ConstraintType.And:
                    this.Provider.AndBehavior();
                    break;
                case ConstraintType.Or:
                    this.Provider.OrBehavior();
                    break;
                case ConstraintType.None:
                    this.Provider.NoneBehavior();
                    break;
                default:
                    base.ThrowInvalidEnumType();
                    break;
            }
        }

    }
}
