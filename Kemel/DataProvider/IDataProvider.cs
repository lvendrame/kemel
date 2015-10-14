using Kemel.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.DataProvider
{
    public interface IDataProvider
    {
        DbProviderFactory Factory { get; }

        Konnection GetConnection();

        IDBSchemaCommands SchemaCommands { get; }

    }
}
