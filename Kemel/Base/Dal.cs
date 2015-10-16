using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Entity;
using Kemel.DataProvider;
using Kemel.Data;
using Kemel.Schema;

namespace Kemel.Base
{
    public class Dal<TEtt>: ITransactable
        where TEtt: EntityBase, new()
    {
        private IDataProvider prvCurrentProvider;
        public IDataProvider CurrentProvider
        {
            get
            {
                if (prvCurrentProvider == null)
                    prvCurrentProvider = Provider.GetProvider(this.GetType().BaseType.GetGenericArguments()[0]);
                return prvCurrentProvider;
            }
        }

        public KemelTransaction Transaction { get; set; }

        private List<ITransactable> lstTransactables = null;
        public List<ITransactable> Transactables
        {
            get
            {
                if (lstTransactables == null)
                    lstTransactables = new List<ITransactable>();
                return lstTransactables;
            }
        }

        #region CUD

        #region Entity

        public virtual TEtt Save(TEtt ettEntity)
        {
            switch (ettEntity.EntityState)
            {
                case EntityItemState.Added:
                    this.Insert(ettEntity);
                    break;
                case EntityItemState.Deleted:
                    this.DeleteById(ettEntity);
                    break;
                case EntityItemState.Modified:
                    this.Update(ettEntity);
                    break;
            }
            return ettEntity;
        }

        public virtual TEtt Insert(TEtt ettEntity)
        {
            bool createTransaction = false;
            if (this.Transaction == null)
            {
                createTransaction = true;
                this.Transaction = this.CurrentProvider.GetConnection().CreateTransaction();
                this.Transaction.Begin();
            }


            try
            {
                Query query = CurrentProvider.EntityCrudBuilder.GetInsert<TEtt>(ettEntity);
                query.Execute(this.Transaction).NonQuery();
                ettEntity.EntityState = EntityItemState.Unchanged;

                if (TableSchema.FromEntity<TEtt>().IdentityColumns.Count > 0)
                {
                    Query getIdentity = CurrentProvider.EntityCrudBuilder.GetSelectIdentity<TEtt>();

                    object value = getIdentity.Execute(this.Transaction).Scalar();
                    Executer.SetIdentityValues<TEtt>(ettEntity, value);

                    //TEtt ettIdentity = getIdentity.Execute(this.Transaction).List<TEtt>().First();
                    //Executer.SetIdentityValues<TEtt>(ettEntity, ettIdentity, getIdentity.Tables[0].TableDefinition as TableSchema);
                }

                if (createTransaction)
                {
                    this.Transaction.Commit();
                    this.Transaction = null;
                }
            }
            catch (Exception ex)
            {
                if (createTransaction)
                {
                    this.Transaction.Rollback();
                    this.Transaction = null;
                }

                throw ex;
            }

            return ettEntity;
        }

        public virtual int Update(TEtt ettEntity)
        {
            Query query = CurrentProvider.EntityCrudBuilder.GetUpdate<TEtt>(ettEntity);
            int rowsAffected = query.Execute(this.Transaction).NonQuery();
            ettEntity.EntityState = EntityItemState.Unchanged;
            return rowsAffected;
        }

        public virtual int Delete(TEtt ettEntity)
        {
            Query query = CurrentProvider.EntityCrudBuilder.GetDelete<TEtt>(ettEntity);
            int rowsAffected = query.Execute(this.Transaction).NonQuery();
            ettEntity.EntityState = EntityItemState.Unchanged;
            return rowsAffected;
        }

        public virtual int DeleteById(TEtt ettEntity)
        {
            Query query = CurrentProvider.EntityCrudBuilder.GetDeleteById<TEtt>(ettEntity);
            int rowsAffected = query.Execute(this.Transaction).NonQuery();
            ettEntity.EntityState = EntityItemState.Unchanged;
            return rowsAffected;
        }

        public virtual int DeleteById(object id)
        {
            Query qry = StorageFactory.NQuery.Delete(this.CurrentProvider).Into<TEtt>();

            ColumnSchema[] columns = (qry.IntoTable.TableDefinition as TableSchema).GetPrimaryKeys();

            if (columns != null && columns.Length > 0)
            {
                qry.And(columns[0]).Equal(id);
            }

            int rowsAffected = qry.Execute().NonQuery();
            return rowsAffected;
        }

        #endregion

        #region List<TEtt>

        public virtual void Save(List<TEtt> lstEntity)
        {
            if (lstEntity.Count == 0)
                return;

            foreach (TEtt entity in lstEntity)
            {
                this.Save(entity);
            }
        }

        public virtual int Insert(List<TEtt> lstEntity)
        {
            if (lstEntity.Count == 0)
                return 0;

            Query query = CurrentProvider.EntityCrudBuilder.GetInsert<TEtt>(lstEntity[0]);
            int rowsAffected = query.Execute(this.Transaction).NonQuery<TEtt>(lstEntity);
            return rowsAffected;
        }

        public virtual int Update(List<TEtt> lstEntity)
        {
            if (lstEntity.Count == 0)
                return 0;

            Query query = CurrentProvider.EntityCrudBuilder.GetUpdate<TEtt>(lstEntity[0]);
            int rowsAffected = query.Execute(this.Transaction).NonQuery<TEtt>(lstEntity);
            return rowsAffected;
        }

        public virtual int Delete(List<TEtt> lstEntity)
        {
            if (lstEntity.Count == 0)
                return 0;

            Query query = CurrentProvider.EntityCrudBuilder.GetDelete<TEtt>(lstEntity[0]);
            int rowsAffected = query.Execute(this.Transaction).NonQuery<TEtt>(lstEntity);
            return rowsAffected;
        }

        public virtual int DeleteById(List<TEtt> lstEntity)
        {
            if (lstEntity.Count == 0)
                return 0;

            Query query = CurrentProvider.EntityCrudBuilder.GetDeleteById<TEtt>(lstEntity[0]);
            int rowsAffected = query.Execute(this.Transaction).NonQuery<TEtt>(lstEntity);
            return rowsAffected;
        }

        #endregion

        #endregion

        #region Read
        public virtual TEtt ReadById(TEtt ett)
        {
            Query qry = CurrentProvider.EntityCrudBuilder.GetSelectById<TEtt>(ett);
            return qry.Execute().List<TEtt>().First();
        }

        public virtual TEtt ReadById(object id)
        {
            Query qry = StorageFactory.NQuery.Select(this.CurrentProvider).From<TEtt>().AllColumns();

            ColumnSchema[] columns = (qry.Tables[0].TableDefinition as TableSchema).GetPrimaryKeys();

            if (columns != null && columns.Length > 0)
            {
                qry.And(columns[0]).Equal(id);
            }

            return qry.Execute().List<TEtt>().First();
        }

        public virtual List<TEtt> ReadByField(TEtt ett)
        {
            Query qry = CurrentProvider.EntityCrudBuilder.GetSelect<TEtt>(ett);
            return qry.Execute().List<TEtt>();
        }

        public virtual List<TEtt> ReadAll()
        {
            Query qry = CurrentProvider.EntityCrudBuilder.GetSelect<TEtt>();
            return qry.Execute().List<TEtt>();
        }
        #endregion


        public void BeginTransaction()
        {
            this.CreateTransaction();
            this.Transaction.Begin();
        }

        public KemelTransaction CreateTransaction()
        {
            Konnection connection = this.CurrentProvider.GetConnection();
            this.Transaction = connection.CreateTransaction();

            foreach (ITransactable item in this.Transactables)
            {
                item.Transaction = this.Transaction;
            }

            return this.Transaction;
        }

        #region Query

        /// <summary>
        /// Initialize Select query.
        /// </summary>
        /// <returns></returns>
        protected Query Select()
        {
            return StorageFactory.NQuery.Select(this.CurrentProvider);
        }

        /// <summary>
        /// Initialize Insert query.
        /// </summary>
        /// <returns></returns>
        protected Query Insert()
        {
            return StorageFactory.NQuery.Insert(this.CurrentProvider);
        }

        /// <summary>
        /// Initialize Update query.
        /// </summary>
        /// <returns></returns>
        protected Query Update()
        {
            return StorageFactory.NQuery.Update(this.CurrentProvider);
        }

        /// <summary>
        /// Initialize Delete query.
        /// </summary>
        /// <returns></returns>
        protected Query Delete()
        {
            return StorageFactory.NQuery.Delete(this.CurrentProvider);
        }

        /// <summary>
        /// Initialize Procedure query.
        /// </summary>
        /// <returns></returns>
        protected Query Procedure(string procedureName)
        {
            return StorageFactory.NQuery.Procedure(procedureName, this.CurrentProvider);
        }

        /// <summary>
        /// Initialize Procedure query.
        /// </summary>
        /// <returns></returns>
        protected Query Procedure<TEntity>()
            where TEntity : EntityBase
        {
            return StorageFactory.NQuery.Procedure<TEntity>();
        }

        #endregion
    }
}
