using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.DataProvider
{
    public class DataProviderContainer
    {

        private static Dictionary<string, IDataProvider> _createdDataProviders = new Dictionary<string, IDataProvider>();

        public static IDataProvider GetDataProvider(string dataKey)
        {
            return _createdDataProviders[dataKey];
        }

        public static IDataProvider CreateDataProvider(string name, string dataKey)
        {
            IDataProvider provider = DataProviderFactory.CreateDataProvider(name);
            _createdDataProviders.Add(dataKey, provider);
            return provider;
        }
    }
}
