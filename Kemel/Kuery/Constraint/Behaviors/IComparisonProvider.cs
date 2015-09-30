using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.Kuery.Constraint.Behaviors
{
    public interface IComparisonProvider
    {
        void EqualBehavior();

        void DifferentBehavior();

        void GreaterThanBehavior();

        void LessThanBehavior();

        void GreaterThanOrEqualBehavior();

        void LessThanOrEqualBehavior();

        void BetweenBehavior();

        void LikeBehavior();

        void NotLikeBehavior();

        void InBehavior();

        void NotInBehavior();

        void IsNullBehavior();

        void IsNotNullBehavior();

        void OpenParenthesesBehavior();

        void CloseParenthesesBehavior();

        void StartsWithBehavior();

        void EndsWithBehavior();

        void ContainsBehavior();
    }
}
