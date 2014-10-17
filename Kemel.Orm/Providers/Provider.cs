using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Data;
using Kemel.Orm.Schema;
using Kemel.Orm.Starter;
using Kemel.Orm.Platform;
using Kemel.Orm.NQuery.Builder;
using Kemel.Orm.NQuery.Storage;
using Kemel.Orm.Entity;

namespace Kemel.Orm.Providers
{
    public abstract class Provider
    {
        public static OrmStarter Starter { get; private set; }

        private static Dictionary<string, Provider> dicProviders = null;
        private static Dictionary<string, Provider> Providers
        {
            get
            {
                if (dicProviders == null)
                    dicProviders = new Dictionary<string, Provider>();
                return dicProviders;
            }
        }

        public static Provider GetProvider(string key)
        {
            if (string.IsNullOrEmpty(key))
                return FirstInstance;

            if (Providers.ContainsKey(key))
                return Providers[key];
            else
                throw new OrmException(Messages.NullOrmStartesException);
        }

        public static Provider GetProvider(Type entityType)
        {
            return GetProvider(TableSchema.GetKeyProvider(entityType));
        }

        public static Provider GetProvider<T>()
            where T: Entity.EntityBase
        {
            return GetProvider(TableSchema.GetKeyProvider(typeof(T)));
        }

        public static void AddProvider(string key, Provider provider)
        {
            Providers.Add(key, provider);
        }

        public static Provider FirstInstance
        {
            get
            {
                foreach (string key in Providers.Keys)
                {
                    return Providers[key];
                }
                throw new OrmException(Messages.NullOrmStartesException);
            }
        }

        public static bool HasProvider
        {
            get
            {
                return Providers.Count > 0;
            }
        }

        static int keyProviderIdx = 0;
        private void Initialize()
        {
            if(Provider.SchemaContainer == null)
                Provider.SchemaContainer = PlatformFactory.Instance.CreateSchemaContainer();

            if(Provider.CommandCache == null)
                Provider.CommandCache = PlatformFactory.Instance.CreateCommandCache();
        }

        public static SchemaContainer SchemaContainer { get; private set; }
        public static CommandCache CommandCache { get; private set; }

        internal static void SetStarter(OrmStarter starter)
        {
            Starter = starter;
        }

        public static void Create()
        {
            if (string.IsNullOrEmpty(Starter.Credential.KeyProvider))
            {
                Starter.Credential.KeyProvider = keyProviderIdx.ToString();
                keyProviderIdx++;
            }

            Provider provider = Starter.CreateProvider();
            provider.Initialize();
            provider.Credential = Starter.Credential;

            AddProvider(Starter.Credential.KeyProvider, provider);
        }

        public static void Clear()
        {
            if(Provider.dicProviders != null)
                Provider.dicProviders.Clear();
        }

        public Credential Credential { get; private set; }

        public abstract OrmConnection GetConnection();
        public abstract string GetConnectionString();
        //public abstract QueryBuilder QueryBuilder { get; }
        public abstract QueryBuilder QueryBuilder { get; }
        public abstract EntityCrudBuilder EntityCrudBuilder { get; }
        public abstract void ResetConnections();

        #region Gen

        public abstract string TableList { get; }
        public abstract string TableDefinition { get; }
        public abstract string AllTablesDefinition { get; }

        public abstract string ViewList { get; }
        public abstract string ViewDefinition { get; }

        public abstract string ProcedureList { get; }
        public abstract string ProcedureDefinition { get; }

        public abstract string FunctionList { get; }
        public abstract string FunctionDefinition { get; }

        public abstract string AllDataBases { get; }

        #endregion

        public abstract System.Data.DbType ConvertInternalTypeToDbType(string internalType, int precision, int scale);

        public abstract string ConvertDbTypeToFinalDbType(System.Data.DbType dbType);

        public event EventHandler<QueryBuilderArgs> OnQueryBuilder;

        internal bool HasQueryBuilderEvent
        {
            get
            {
                return this.OnQueryBuilder != null;
            }
        }

        internal void ExecuteOnQueryBuilder(EntityCrudBuilder builder, Query qry, EntityBase ett, CrudType crudType)
        {
            if (this.HasQueryBuilderEvent)
            {
                QueryBuilderArgs args = new QueryBuilderArgs();
                args.Query = qry;
                args.Entity = ett;
                args.CrudType = crudType;
                this.OnQueryBuilder(builder, args);
            }
        }

    }

    public class QueryBuilderArgs : EventArgs
    {
        public Query Query { get; set; }
        public EntityBase Entity { get; set; }
        public CrudType CrudType { get; set; }
    }
}
