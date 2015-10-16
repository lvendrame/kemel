using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.DataProvider
{
    public interface IDBSchemaCommands
    {
        string TableList { get; }
        string TableDefinition { get; }
        string AllTablesDefinition { get; }

        string ViewList { get; }
        string ViewDefinition { get; }

        string ProcedureList { get; }
        string ProcedureDefinition { get; }

        string FunctionList { get; }
        string FunctionDefinition { get; }

        string AllDataBases { get; }

    }
}
