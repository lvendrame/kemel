using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.Entity
{
    public interface IEntityStateProvider
    {
        void AddedBehavior();

        void DeletedBehavior();

        void UnchangedBehavior();

        void ModifiedBehavior();
    }
}
