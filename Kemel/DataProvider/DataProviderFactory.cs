using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.DataProvider
{
    public class DataProviderFactory
    {

        private static Dictionary<string, Type> _registredDataProviders = new Dictionary<string, Type>();

        public static void RegisterDataProvider(string name, Type dataProviderType)
        {
            _registredDataProviders.Add(name, dataProviderType);
        }

        public static IDataProvider CreateDataProvider(string name)
        {
            return Activator.CreateInstance(_registredDataProviders[name]) as IDataProvider;
        }

    }
}
