using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.Kuery.Behaviors
{
    public interface IQueryTypeProvider
    {
        void SelectBehavior();

        void InsertBehavior();

        void DeleteBehavior();

        void UpdateBehavior();

        void ProcedureBehavior();

        void FunctionBehavior();
    }
}
