using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.Kuery.Constraint.Behaviors
{
    public class ComparisonBehavior: BehaviorProviderContainer<IComparisonProvider, ComparisonOperator>
    {
        public ComparisonBehavior(IComparisonProvider comparisonProvider)
            : base(comparisonProvider)
        {
        }

        public override void DoBehavior(ComparisonOperator enumType)
        {
            switch (enumType)
            {
                case ComparisonOperator.Equal:
                    this.Provider.EqualBehavior();
                    break;
                case ComparisonOperator.Different:
                    this.Provider.DifferentBehavior();
                    break;
                case ComparisonOperator.GreaterThan:
                    this.Provider.GreaterThanBehavior();
                    break;
                case ComparisonOperator.LessThan:
                    this.Provider.LessThanBehavior();
                    break;
                case ComparisonOperator.GreaterThanOrEqual:
                    this.Provider.GreaterThanOrEqualBehavior();
                    break;
                case ComparisonOperator.LessThanOrEqual:
                    this.Provider.LessThanOrEqualBehavior();
                    break;
                case ComparisonOperator.Between:
                    this.Provider.BetweenBehavior();
                    break;
                case ComparisonOperator.Like:
                    this.Provider.LikeBehavior();
                    break;
                case ComparisonOperator.NotLike:
                    this.Provider.NotLikeBehavior();
                    break;
                case ComparisonOperator.In:
                    this.Provider.InBehavior();
                    break;
                case ComparisonOperator.NotIn:
                    this.Provider.NotInBehavior();
                    break;
                case ComparisonOperator.IsNull:
                    this.Provider.IsNullBehavior();
                    break;
                case ComparisonOperator.IsNotNull:
                    this.Provider.IsNotNullBehavior();
                    break;
                case ComparisonOperator.OpenParentheses:
                    this.Provider.OpenParenthesesBehavior();
                    break;
                case ComparisonOperator.CloseParentheses:
                    this.Provider.CloseParenthesesBehavior();
                    break;
                case ComparisonOperator.StartsWith:
                    this.Provider.StartsWithBehavior();
                    break;
                case ComparisonOperator.EndsWith:
                    this.Provider.EndsWithBehavior();
                    break;
                case ComparisonOperator.Contains:
                    this.Provider.ContainsBehavior();
                    break;
                default:
                    base.ThrowInvalidEnumType();
                    break;
            }
        }

    }
}
