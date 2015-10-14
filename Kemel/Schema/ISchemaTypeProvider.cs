using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.Schema
{
    public interface ISchemaTypeProvider
    {
        void TableBehavior();

        void ViewBehavior();

        void ProcedureBehavior();

        void ScalarFunctionBehavior();

        void TableFunctionBehavior();

        void AggregateFunctionBehavior();

        void NoneBehavior();
    }
}
