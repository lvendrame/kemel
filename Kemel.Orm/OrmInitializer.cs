using Kemel.Orm.Configuration;
using Kemel.Orm.Platform;
using Kemel.Orm.Providers;
using Kemel.Orm.Starter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kemel.Orm
{
    public static class OrmInitializer
    {
        public static void Initialize()
        {
            DigiOrmSection config = Configuration.DigiOrmSection.GetConfig;
            if (config != null)
            {
                if (config.Credentials != null && config.Credentials.Count != 0)
                {
                    bool setFactory = true;
                    foreach (CredentialElement item in config.Credentials)
                    {
                        CreateProvider(item, ref setFactory);
                    }
                }
            }
        }

        private static void CreateProvider(CredentialElement item, ref bool setFactory)
        {
            OrmStarter starter = GetStarter(item.Starter);
            if (setFactory)
            {
                PlatformFactory.SetPlatformFactory(starter);
                setFactory = false;
            }

            starter.FillCredential(item);
            Provider.SetStarter(starter);
            Provider.Create();
        }

        private static OrmStarter GetStarter(string starterType)
        {
            string[] reflect = starterType.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
            Assembly asm = Assembly.Load(reflect[0].Trim());
            Type tp = asm.GetType(reflect[1].Trim());
            return Activator.CreateInstance(tp) as OrmStarter;
        }
    }
}
