using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Data;
using Kemel.Orm.Providers;
using Kemel.Orm.Entity;
using Kemel.Orm.Schema;

namespace Kemel.Orm.QueryDef
{
    public class Executer
    {
        public Query Parent { get; set; }

        public OrmTransaction Transaction { get; set; }

        public Executer(Query parent)
        {
            this.Parent = parent;
        }

        public Executer(Query parent, OrmTransaction transaction)
        {
            this.Parent = parent;
            this.Transaction = transaction;
        }

        private OrmCommand CreateCommand()
        {
            return this.Transaction == null ?
                this.Parent.Provider.GetConnection().CreateCommand() :
                this.Transaction.Connection.CreateCommand(this.Transaction);
        }

        public List<TEtt> List<TEtt>()
            where TEtt: EntityBase, new()
        {
            OrmCommand command = this.CreateCommand();
            this.Parent.WriteQuery(command);
            return command.ExecuteList<TEtt>();
        }

        public List<TEtt> List<TEtt>(bool useExtendProperties)
            where TEtt : EntityBase, new()
        {
            OrmCommand command = this.CreateCommand();
            this.Parent.WriteQuery(command);
            return command.ExecuteList<TEtt>(useExtendProperties);
        }

        public System.Data.DataTable DataTable()
        {
            OrmCommand command = this.CreateCommand();
            this.Parent.WriteQuery(command);
            return command.ExecuteDataTable();
        }

        public int NonQuery()
        {
            OrmCommand command = this.CreateCommand();
            this.Parent.WriteQuery(command);
            return command.ExecuteNonQuery();
        }

        public int NonQuery<TEtt>(List<TEtt> lstEntities)//, bool blockPK)
            where TEtt: EntityBase
        {
            bool finalizeTransaction = false;
            if (this.Transaction == null)
            {
                this.Transaction = this.Parent.Provider.GetConnection().CreateTransaction();
                finalizeTransaction = true;
            }

            OrmConnection connection = this.Transaction.Connection;

            OrmCommand command = this.CreateCommand();
            this.Parent.WriteQuery(command);

            bool ignoreIdentity = this.Parent.Type == QueryType.Insert;

            int ret = 0;

            if (finalizeTransaction)
                this.Transaction.Begin();
            try
            {
                ret = command.ExecuteNonQuery();

                for (int i = 1; i < lstEntities.Count; i++)
                {
                    //this.SetNewParameters<TEtt>(command, lstEntities[i]);//, blockPK);
                    SetParameters<TEtt>(command, this.Parent, lstEntities[i], ignoreIdentity);
                    ret += command.ExecuteNonQuery();
                }

                if(finalizeTransaction)
                    this.Transaction.Commit();
            }
            catch (Exception ex)
            {
                if (finalizeTransaction)
                    this.Transaction.Rollback();

                throw new OrmException(ex.Message, ex);
            }
            finally
            {
                if (finalizeTransaction)
                    connection.Close();
            }

            return ret;
        }

        //private void SetNewParameters<TEtt>(OrmCommand command, TEtt ettEntity)//, bool blockPK)
        //{
        //    TableSchema schema = this.Parent.IntoTable.TableSchema;
        //    string prefixParam = Provider.Instance.QueryBuilder.DataBasePrefixParameter;

        //    foreach (ColumnSchema column in schema.Columns)
        //    {
        //        if (column.IgnoreColumn)
        //            continue;

        //        if (column.IsIdentity)
        //            continue;

        //        Constraint columnConstraint = this.Parent.Constraints.FindByColumn(column);
        //        //(command.Parameters[string.Concat(prefixParam, column.Name)] as System.Data.IDbDataParameter).Value = column.GetValue(ettEntity);
        //        (command.Parameters[columnConstraint.Column.ParameterName] as System.Data.IDbDataParameter).Value = column.GetValue(ettEntity);
        //    }
        //}

        public object Scalar()
        {
            OrmCommand command = this.CreateCommand();
            this.Parent.WriteQuery(command);
            return command.ExecuteScalar();
        }

        public T Scalar<T>()
        {
            OrmCommand command = this.CreateCommand();
            this.Parent.WriteQuery(command);
            return command.ExecuteScalar<T>();
        }

        public static void Save<TEtt>(List<TEtt> lstEntity, OrmTransaction ormTransaction)
            where TEtt : EntityBase, new()
        {
            Query qryInsert = null;
            Query qrySelectIdentity = null;
            Query qryUpdate = null;
            Query qryDelete = null;

            OrmCommand cmdInsert = null;
            OrmCommand cmdSelectIdentity = null;
            OrmCommand cmdUpdate = null;
            OrmCommand cmdDelete = null;

            bool createTransaction = false;

            Provider provider = Provider.GetProvider<TEtt>();
            if (ormTransaction == null)
            {
                createTransaction = true;
                ormTransaction = provider.GetConnection().CreateTransaction();
                ormTransaction.Begin();
            }

            qrySelectIdentity = provider.EntityCrudBuilder.GetSelectIdentity<TEtt>();
            cmdSelectIdentity = ormTransaction.Connection.CreateCommand(ormTransaction);
            qrySelectIdentity.WriteQuery(cmdSelectIdentity);

            TableSchema schema = qrySelectIdentity.Tables[0].TableSchema;

            try
            {
                foreach (TEtt ett in lstEntity)
                {
                    switch (ett.EntityState)
                    {
                        #region case EntityItemState.Added
                        case EntityItemState.Added:
                            if (cmdInsert == null)
                            {
                                qryInsert = provider.EntityCrudBuilder.GetInsert<TEtt>(ett);
                                cmdInsert = ormTransaction.Connection.CreateCommand(ormTransaction);
                                qryInsert.WriteQuery(cmdInsert);
                            }
                            else
                            {
                                SetParameters<TEtt>(cmdInsert, qryInsert, ett, true);
                            }
                            cmdInsert.ExecuteNonQuery();
                            SetIdentityValues<TEtt>(ett, cmdSelectIdentity.ExecuteList<TEtt>().First(), schema);
                            break;
                        #endregion
                        #region case EntityItemState.Deleted
                        case EntityItemState.Deleted:
                            if (cmdDelete == null)
                            {
                                qryDelete = provider.EntityCrudBuilder.GetDeleteById<TEtt>(ett);
                                cmdDelete = ormTransaction.Connection.CreateCommand(ormTransaction);
                                qryDelete.WriteQuery(cmdDelete);
                            }
                            else
                            {
                                SetParameters<TEtt>(cmdDelete, qryDelete, ett, false);
                            }
                            cmdDelete.ExecuteNonQuery();
                            break;
                        #endregion
                        #region case EntityItemState.Modified
                        case EntityItemState.Modified:
                            if (cmdUpdate == null)
                            {
                                qryUpdate = provider.EntityCrudBuilder.GetUpdate<TEtt>(ett);
                                cmdUpdate = ormTransaction.Connection.CreateCommand(ormTransaction);
                                qryUpdate.WriteQuery(cmdUpdate);
                            }
                            else
                            {
                                SetParameters<TEtt>(cmdUpdate, qryUpdate, ett, false);
                            }
                            cmdUpdate.ExecuteNonQuery();
                            break;
                        #endregion
                    }
                }

                if (createTransaction)
                    ormTransaction.Commit();
            }
            catch (Exception ex)
            {
                if (createTransaction)
                    ormTransaction.Rollback();

                throw ex;
            }
        }

        public static void SetIdentityValues<TEtt>(TEtt ett, TEtt pIdentityValues, TableSchema schema)
            where TEtt : EntityBase
        {
            foreach (ColumnSchema column in schema.IdentityColumns)
            {
                column.SetValue(ett,
                    column.GetValue(pIdentityValues));
            }
        }

        public static void SetParameters<TEtt>(OrmCommand command, Query query, TEtt ett, bool ignoreIdentity)
            where TEtt : EntityBase
        {
            Provider provider = Provider.GetProvider<TEtt>();
            string prefixParam = provider.QueryBuilder.DataBasePrefixParameter;

            foreach (Constraint constraint in query.Constraints)
            {
                ColumnQuery columnQuery = constraint.Column;

                if (columnQuery != null && columnQuery.ColumnSchema != null)
                {
                    if (columnQuery.ColumnSchema.IgnoreColumn || (ignoreIdentity && columnQuery.ColumnSchema.IsIdentity))
                        continue;

                    object value = columnQuery.ColumnSchema.GetValue(ett);
                    if (value == null)
                        value = DBNull.Value;

                    (command.Parameters[prefixParam + columnQuery.ParameterName] as System.Data.IDbDataParameter).Value
                        = value;
                }
            }

            foreach (SetValue setValue in query.SetValues)
            {
                ColumnSchema colSchema = setValue.ColumnSchema;

                if (colSchema != null)
                {
                    if (colSchema.IgnoreColumn || colSchema.IsIdentity)
                        continue;

                    object value = colSchema.GetValue(ett);
                    if (value == null)
                        value = DBNull.Value;

                    (command.Parameters[prefixParam + setValue.ParameterName] as System.Data.IDbDataParameter).Value
                        = value;
                }
            }
        }

        public OrmCommand GetCommand()
        {
            OrmCommand command = this.CreateCommand();
            this.Parent.WriteQuery(command);
            return command;
        }
    }
}
