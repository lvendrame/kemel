using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Kemel.Orm.Data;
using Kemel.Orm.Providers;
using Kemel.Orm.Entity;
using Kemel.Orm.Schema;
using Kemel.Orm.NQuery.Storage.Constraint;
using Kemel.Orm.NQuery.Storage.Column;

namespace Kemel.Orm.NQuery.Storage
{
    public class Executer
    {
        private Query objParent = null;
        public Query Parent
        {
            get { return this.objParent; }
        }

        private OrmTransaction objTransaction = null;
        public OrmTransaction Transaction
        {
            get { return this.objTransaction; }
            set { this.objTransaction = value; }
        }

        internal Executer(Query parent)
        {
            this.objParent = parent;
            this.objParent.Compile();
        }

        public Executer(Query parent, OrmTransaction transaction)
            : this(parent)
        {
            this.Transaction = transaction;
        }

        //Private Function CreateCommand() As OrmCommand
        //    Return Me.Parent.Provider.ReQueryBuilder.GetCommand(Me.Parent, Me.Transaction)
        //End Function

        public OrmCommand CreateCommand()
        {
            return this.Parent.CreateCommand(this.Transaction);
        }

        public List<TEtt> List<TEtt>() where TEtt : EntityBase, new()
        {
            OrmCommand command = this.Parent.CreateCommand(this.Transaction);
            return command.ExecuteList<TEtt>();
        }

        public List<TEtt> List<TEtt>(bool useExtendProperties) where TEtt : EntityBase, new()
        {
            OrmCommand command = this.Parent.CreateCommand(this.Transaction);
            return command.ExecuteList<TEtt>(useExtendProperties);
        }

        public System.Data.DataTable DataTable()
        {
            OrmCommand command = this.Parent.CreateCommand(this.Transaction);
            return command.ExecuteDataTable();
        }

        public int NonQuery()
        {
            OrmCommand command = this.Parent.CreateCommand(this.Transaction);
            return command.ExecuteNonQuery();
        }

        //, bool blockPK)
        public int NonQuery<TEtt>(List<TEtt> lstEntities) where TEtt : EntityBase
        {
            bool finalizeTransaction = false;
            if (this.Transaction == null)
            {
                this.Transaction = this.Parent.Provider.GetConnection().CreateTransaction();
                finalizeTransaction = true;
            }

            OrmConnection connection = this.Transaction.Connection;

            OrmCommand command = this.Parent.CreateCommand(this.Transaction);

            bool ignoreIdentity = this.Parent.Type == Query.QueryType.Insert;

            int ret = 0;

            if (finalizeTransaction)
            {
                this.Transaction.Begin();
            }
            try
            {
                ret = command.ExecuteNonQuery();

                for (int i = 1; i <= lstEntities.Count - 1; i++)
                {
                    //this.SetNewParameters<TEtt>(command, lstEntities[i]);//, blockPK);
                    SetParameters<TEtt>(command, this.Parent, lstEntities[i], ignoreIdentity);
                    ret += command.ExecuteNonQuery();
                }

                if (finalizeTransaction)
                {
                    this.Transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                if (finalizeTransaction)
                {
                    this.Transaction.Rollback();
                }

                throw new OrmException(ex.Message, ex);
            }
            finally
            {
                if (finalizeTransaction)
                {
                    connection.Close();
                }
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
            OrmCommand command = this.Parent.CreateCommand(this.Transaction);
            return command.ExecuteScalar();
        }

        public T Scalar<T>()
        {
            OrmCommand command = this.Parent.CreateCommand(this.Transaction);
            return command.ExecuteScalar<T>();
        }

        //Public Shared Sub Save(Of TEtt As {EntityBase, New})(lstEntity As List(Of TEtt), ormTransaction As OrmTransaction)
        //    Dim qryInsert As SQuery = Nothing
        //    Dim qrySelectIdentity As SQuery = Nothing
        //    Dim qryUpdate As SQuery = Nothing
        //    Dim qryDelete As SQuery = Nothing

        //    Dim cmdInsert As OrmCommand = Nothing
        //    Dim cmdSelectIdentity As OrmCommand = Nothing
        //    Dim cmdUpdate As OrmCommand = Nothing
        //    Dim cmdDelete As OrmCommand = Nothing

        //    Dim createTransaction As Boolean = False

        //    Dim provider__1 As Provider = Provider.GetProvider(Of TEtt)()
        //    If ormTransaction Is Nothing Then
        //        createTransaction = True
        //        ormTransaction = provider__1.GetConnection().CreateTransaction()
        //        ormTransaction.Begin()
        //    End If

        //    qrySelectIdentity = provider__1.EntityCrudBuilder.GetSelectIdentity(Of TEtt)()
        //    cmdSelectIdentity = ormTransaction.Connection.CreateCommand(ormTransaction)
        //    qrySelectIdentity.WriteQuery(cmdSelectIdentity)

        //    Dim schema As TableSchema = qrySelectIdentity.Tables(0).TableSchema

        //    Try
        //        For Each ett As TEtt In lstEntity
        //            Select Case ett.EntityState
        //                #Region "case EntityItemState.Added"
        //                Case EntityItemState.Added
        //                    If cmdInsert Is Nothing Then
        //                        qryInsert = provider__1.EntityCrudBuilder.GetInsert(Of TEtt)(ett)
        //                        cmdInsert = ormTransaction.Connection.CreateCommand(ormTransaction)
        //                        qryInsert.WriteQuery(cmdInsert)
        //                    Else
        //                        SetParameters(Of TEtt)(cmdInsert, qryInsert, ett, True)
        //                    End If
        //                    cmdInsert.ExecuteNonQuery()
        //                    SetIdentityValues(Of TEtt)(ett, cmdSelectIdentity.ExecuteList(Of TEtt)().First(), schema)
        //                    Exit Select
        //                    #End Region
        //                    #Region "case EntityItemState.Deleted"
        //                Case EntityItemState.Deleted
        //                    If cmdDelete Is Nothing Then
        //                        qryDelete = provider__1.EntityCrudBuilder.GetDeleteById(Of TEtt)(ett)
        //                        cmdDelete = ormTransaction.Connection.CreateCommand(ormTransaction)
        //                        qryDelete.WriteQuery(cmdDelete)
        //                    Else
        //                        SetParameters(Of TEtt)(cmdDelete, qryDelete, ett, False)
        //                    End If
        //                    cmdDelete.ExecuteNonQuery()
        //                    Exit Select
        //                    #End Region
        //                    #Region "case EntityItemState.Modified"
        //                Case EntityItemState.Modified
        //                    If cmdUpdate Is Nothing Then
        //                        qryUpdate = provider__1.EntityCrudBuilder.GetUpdate(Of TEtt)(ett)
        //                        cmdUpdate = ormTransaction.Connection.CreateCommand(ormTransaction)
        //                        qryUpdate.WriteQuery(cmdUpdate)
        //                    Else
        //                        SetParameters(Of TEtt)(cmdUpdate, qryUpdate, ett, False)
        //                    End If
        //                    cmdUpdate.ExecuteNonQuery()
        //                    Exit Select
        //                    #End Region
        //            End Select
        //        Next

        //        If createTransaction Then
        //            ormTransaction.Commit()
        //        End If
        //    Catch ex As Exception
        //        If createTransaction Then
        //            ormTransaction.Rollback()
        //        End If

        //        Throw ex
        //    End Try
        //End Sub

        public static void SetIdentityValues<TEtt>(TEtt ett, object value) where TEtt : EntityBase
        {
            TableSchema schema = TableSchema.FromEntity<TEtt>();
            foreach (ColumnSchema column in schema.IdentityColumns)
            {
                column.SetValue(ett, value);
            }
        }

        public static void SetIdentityValues<TEtt>(TEtt ett, TEtt pIdentityValues, TableSchema schema) where TEtt : EntityBase
        {
            foreach (ColumnSchema column in schema.IdentityColumns)
            {
                column.SetValue(ett, column.GetValue(pIdentityValues));
            }
        }

        public static void SetParameters<TEtt>(OrmCommand command, Query query, TEtt ett, bool ignoreIdentity) where TEtt : EntityBase
        {
            Provider provider__1 = Provider.GetProvider<TEtt>();
            string prefixParam = provider__1.QueryBuilder.DataBasePrefixParameter;

            foreach (StoredConstraint constraint in query.Constraints)
            {
                StoredColumn column = constraint.Column;

                if (column != null && column.Type == StoredColumn.StoredTypes.Schema)
                {
                    ColumnSchema colSchema = (ColumnSchema)column.ColumnDefinition;
                    if (colSchema.IgnoreColumn || (ignoreIdentity && colSchema.IsIdentity))
                    {
                        continue;
                    }

                    object value = colSchema.GetValue(ett);
                    if (value == null)
                    {
                        value = DBNull.Value;
                    }

                    (command.Parameters[prefixParam + column.ParameterName] as System.Data.IDbDataParameter).Value = value;
                }
            }
        }

        //Public Function GetCommand() As OrmCommand
        //    'Dim command As OrmCommand = Me.CreateCommand()
        //    'Me.Parent.WriteQuery(command)
        //    'Return command
        //    Return Me.CreateCommand()
        //End Function
    }
}
