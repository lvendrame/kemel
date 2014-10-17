using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Kemel.Orm.Entity;
using Kemel.Orm.Schema;
using System.Reflection;
using Kemel.Orm.Entity.Attributes;

namespace Kemel.Orm.Data
{
    public class OrmCommand : IDbCommand
    {

        #region Properties

        private IDbCommand cmdWraperCommand = null;
        public IDbCommand WraperCommand
        {
            get
            {
                return this.cmdWraperCommand;
            }
        }

        private OrmTransaction transaction;

        private int intMaxAttempts = 1;
        public int MaxAttempts
        {
            get
            {
                return this.intMaxAttempts;
            }
            set
            {
                this.intMaxAttempts = value;
            }
        }

        private StringBuilder stbWriterCommand = null;
        public StringBuilder WriterCommand
        {
            get
            {
                if (this.stbWriterCommand == null)
                    this.stbWriterCommand = new StringBuilder();
                return this.stbWriterCommand;
            }
        }

        #endregion

        private bool PrepareTransaction()
        {
            if (this.transaction != null)
            {
                if (this.transaction.State != TransactionState.Started)
                    this.transaction.Begin();
                this.Transaction = transaction.TransactionWraper;
                return true;
            }
            return false;
        }

        internal OrmCommand(IDbCommand command)
        {
            this.cmdWraperCommand = command;
        }

        internal OrmCommand(IDbCommand command, OrmTransaction transaction)
        {
            this.cmdWraperCommand = command;
            this.transaction = transaction;
        }

        #region IDbCommand Members

        public void Cancel()
        {
            this.cmdWraperCommand.Cancel();
        }

        public string CommandText
        {
            get
            {
                return this.cmdWraperCommand.CommandText;
            }
            set
            {
                this.cmdWraperCommand.CommandText = value;
            }
        }

        public int CommandTimeout
        {
            get
            {
                return this.cmdWraperCommand.CommandTimeout;
            }
            set
            {
                this.cmdWraperCommand.CommandTimeout = value;
            }
        }

        public System.Data.CommandType CommandType
        {
            get
            {
                return this.cmdWraperCommand.CommandType;
            }
            set
            {
                this.cmdWraperCommand.CommandType = value;
            }
        }

        public System.Data.IDbConnection Connection
        {
            get
            {
                return this.cmdWraperCommand.Connection;
            }
            set
            {
                this.cmdWraperCommand.Connection = value;
            }
        }

        System.Data.IDbDataParameter IDbCommand.CreateParameter()
        {
            return this.cmdWraperCommand.CreateParameter();
        }

        public IDbDataParameter AddParameter()
        {
            IDbDataParameter parameter = this.cmdWraperCommand.CreateParameter();
            this.Parameters.Add(parameter);
            return parameter;
        }

        public IDbDataParameter AddParameter(string parameterName)
        {
            IDbDataParameter parameter = this.cmdWraperCommand.CreateParameter();
            parameter.ParameterName = parameterName;
            this.Parameters.Add(parameter);
            return parameter;
        }

        public IDbDataParameter this[int index]
        {
            get
            {
                return this.cmdWraperCommand.Parameters[index] as IDbDataParameter;
            }
            set
            {
                this.cmdWraperCommand.Parameters[index] = value;
            }
        }

        public IDbDataParameter this[string parameterName]
        {
            get
            {
                return this.cmdWraperCommand.Parameters[parameterName] as IDbDataParameter;
            }
            set
            {
                this.cmdWraperCommand.Parameters[parameterName] = value;
            }
        }

        public System.Data.IDataParameterCollection Parameters
        {
            get
            {
                return this.cmdWraperCommand.Parameters;
            }
        }

        #region public int ExecuteNonQuery()
        public int ExecuteNonQuery()
        {
            return this.PrepareTransaction() ?
                this.ExecuteNonQueryWithTransaction() :
                this.ExecuteNonQueryWithoutTransaction();
        }

        private int ExecuteNonQueryWithTransaction()
        {
            int retValue = 0;
            AttemptsControl attemptsControl = new AttemptsControl(this.intMaxAttempts);
            do
            {
                try
                {
                    retValue = this.cmdWraperCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    if (attemptsControl.ThrowException())
                        throw new OrmException(ex.Message, ex);
                }

            } while (attemptsControl.TryAgain);
            return retValue;
        }

        private int ExecuteNonQueryWithoutTransaction()
        {
            int retValue = 0;
            AttemptsControl attemptsControl = new AttemptsControl(this.intMaxAttempts);
            do
            {
                try
                {
                    this.OpenConnection();
                    retValue = this.cmdWraperCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    if (attemptsControl.ThrowException())
                        throw new OrmException(ex.Message, ex);
                }
                finally
                {
                    this.CloseConnection();
                }

            } while (attemptsControl.TryAgain);
            return retValue;
        }
        #endregion

        #region public IDataReader ExecuteReader(CommandBehavior behavior)
        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            return this.PrepareTransaction() ?
                this.ExecuteReaderWithTransaction(behavior) :
                this.ExecuteReaderWithoutTransaction(behavior);
        }

        private IDataReader ExecuteReaderWithTransaction(CommandBehavior behavior)
        {
            IDataReader retDR = null;
            AttemptsControl attemptsControl = new AttemptsControl(this.intMaxAttempts);
            do
            {
                try
                {
                    retDR = this.cmdWraperCommand.ExecuteReader(behavior);
                }
                catch (Exception ex)
                {
                    if (attemptsControl.ThrowException())
                        throw new OrmException(ex.Message, ex);
                }

            } while (attemptsControl.TryAgain);
            return retDR;
        }

        private IDataReader ExecuteReaderWithoutTransaction(CommandBehavior behavior)
        {
            IDataReader retDR = null;
            AttemptsControl attemptsControl = new AttemptsControl(this.intMaxAttempts);
            do
            {
                try
                {
                    this.OpenConnection();
                    retDR = this.cmdWraperCommand.ExecuteReader(behavior);
                }
                catch (Exception ex)
                {
                    if (attemptsControl.ThrowException())
                        throw new OrmException(ex.Message, ex);
                }
                finally
                {
                    this.CloseConnection();
                }

            } while (attemptsControl.TryAgain);
            return retDR;
        }
        #endregion

        #region public IDataReader ExecuteReader()
        public IDataReader ExecuteReader()
        {
            return this.PrepareTransaction() ?
                this.ExecuteReaderWithTransaction() :
                this.ExecuteReaderWithoutTransaction();
        }

        private IDataReader ExecuteReaderWithTransaction()
        {
            IDataReader retDR = null;
            AttemptsControl attemptsControl = new AttemptsControl(this.intMaxAttempts);
            do
            {
                try
                {
                    retDR = this.cmdWraperCommand.ExecuteReader();
                }
                catch (Exception ex)
                {
                    if (attemptsControl.ThrowException())
                        throw new OrmException(ex.Message, ex);
                }

            } while (attemptsControl.TryAgain);
            return retDR;
        }

        private IDataReader ExecuteReaderWithoutTransaction()
        {
            IDataReader retDR = null;
            AttemptsControl attemptsControl = new AttemptsControl(this.intMaxAttempts);
            do
            {
                try
                {
                    this.OpenConnection();
                    retDR = this.cmdWraperCommand.ExecuteReader();
                }
                catch (Exception ex)
                {
                    if (attemptsControl.ThrowException())
                        throw new OrmException(ex.Message, ex);
                }
                finally
                {
                    this.CloseConnection();
                }

            } while (attemptsControl.TryAgain);
            return retDR;
        }
        #endregion

        #region public object ExecuteScalar()
        public object ExecuteScalar()
        {
            return this.PrepareTransaction() ?
                this.ExecuteScalarWithTransaction() :
                this.ExecuteScalarWithoutTransaction();
        }

        private object ExecuteScalarWithTransaction()
        {
            object retValue = null;
            AttemptsControl attemptsControl = new AttemptsControl(this.intMaxAttempts);
            do
            {
                try
                {
                    retValue = this.cmdWraperCommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    if (attemptsControl.ThrowException())
                        throw new OrmException(ex.Message, ex);
                }

            } while (attemptsControl.TryAgain);
            return retValue;
        }

        private object ExecuteScalarWithoutTransaction()
        {
            object retValue = null;
            AttemptsControl attemptsControl = new AttemptsControl(this.intMaxAttempts);
            do
            {
                try
                {
                    this.OpenConnection();
                    retValue = this.cmdWraperCommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    if (attemptsControl.ThrowException())
                        throw new OrmException(ex.Message, ex);
                }
                finally
                {
                    this.CloseConnection();
                }

            } while (attemptsControl.TryAgain);
            return retValue;
        }
        #endregion

        public T ExecuteScalar<T>()
        {
            object retValue = this.ExecuteScalar();
            if (Convert.IsDBNull(retValue))
                return default(T);

            Type t = typeof(T).GetFinalValueType();
            if (t == retValue.GetType())
                return (T)retValue;
            else
                return (T)Convert.ChangeType(retValue, t);
        }

        public void Prepare()
        {
            this.cmdWraperCommand.Prepare();
        }

        public System.Data.IDbTransaction Transaction
        {
            get
            {
                return this.cmdWraperCommand.Transaction;
            }
            set
            {
                this.cmdWraperCommand.Transaction = value;
            }
        }

        public System.Data.UpdateRowSource UpdatedRowSource
        {
            get
            {
                return this.cmdWraperCommand.UpdatedRowSource;
            }
            set
            {
                this.cmdWraperCommand.UpdatedRowSource = value;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.cmdWraperCommand.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion

        public List<T> ExecuteList<T>()
            where T : EntityBase, new()
        {
            return this.ExecuteList<T>(false);
        }

        #region public List<T> ExecuteList<T>(bool useExtendProperties)
        public List<T> ExecuteList<T>(bool useExtendProperties)
            where T : EntityBase, new()
        {
            return this.PrepareTransaction() ?
                this.ExecuteListWithTransaction<T>(useExtendProperties) :
                this.ExecuteListWithoutTransaction<T>(useExtendProperties);
        }

        private List<T> ExecuteListWithTransaction<T>(bool useExtendProperties)
            where T : EntityBase, new()
        {
            List<T> lstEntity = new List<T>();
            IDataReader dr = null;

            AttemptsControl attempsControl = new AttemptsControl(this.intMaxAttempts);
            do
            {
                try
                {
                    dr = this.cmdWraperCommand.ExecuteReader();
                    if (dr.Read())
                    {
                        int countFields = dr.FieldCount;

                        #region Get Position and Properties
                        string columnName = string.Empty;
                        List<FillEttItem> lstFillItems = new List<FillEttItem>(countFields);
                        for (int indField = 0; indField < countFields; indField++)
                        {
                            columnName = dr.GetName(indField);
                            PropertyInfo prop = typeof(T).GetProperty(columnName);

                            PropertyFinder propFinder = new PropertyFinder();
                            propFinder.FindProperties<T>(columnName);

                            if (propFinder.HasSubProperties)
                            {
                                foreach(SubProperties subProp in propFinder.SubProperties)
                                {
                                    FillEttItem fei = new FillEttItem();
                                    fei.HasSubProperty = true;
                                    fei.ReflectedSubProperties = subProp.ToArray();
                                    fei.IndexColumn = indField;
                                    fei.DataBaseType = dr.GetFieldType(indField);

                                    PropertyInfo fnProp = subProp[subProp.Count - 1];
                                    ConverterAttribute ca = fnProp.GetCustomAttribute<ConverterAttribute>();
                                    if (ca != null)
                                        fei.Converter = ca.Converter;

                                    lstFillItems.Add(fei);
                                }
                            }

                            if (prop != null)
                            {
                                FillEttItem fei = new FillEttItem();
                                fei.Property = prop;
                                fei.IndexColumn = indField;
                                fei.DataBaseType = dr.GetFieldType(indField);

                                ConverterAttribute ca = prop.GetCustomAttribute<ConverterAttribute>();
                                if (ca != null)
                                    fei.Converter = ca.Converter;

                                lstFillItems.Add(fei);
                            }
                            else if (useExtendProperties)
                            {
                                FillEttItem fei = new FillEttItem();
                                fei.IsExtendProperty = true;
                                fei.IndexColumn = indField;
                                fei.ExtendPropertyName = columnName;
                                lstFillItems.Add(fei);
                            }
                        }
                        #endregion

                        #region Fill Entities
                        do
                        {
                            T obj = new T();
                            obj.EntityState = EntityItemState.Unchanged;
                            #region Fill Fields
                            for (int indFEI = 0; indFEI < lstFillItems.Count; indFEI++)
                            {
                                lstFillItems[indFEI].SetValue(obj, dr);
                            }
                            #endregion
                            lstEntity.Add(obj);
                        }
                        while (dr.Read());
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    if (attempsControl.ThrowException())
                        throw new OrmException(ex.Message, ex);
                }
                finally
                {
                    if (dr != null)
                        dr.Close();
                }
            } while (attempsControl.TryAgain);

            return lstEntity;
        }

        private List<T> ExecuteListWithoutTransaction<T>(bool useExtendProperties)
            where T : EntityBase, new()
        {
            List<T> lstEntity = new List<T>();
            IDataReader dr = null;

            AttemptsControl attempsControl = new AttemptsControl(this.intMaxAttempts);
            do
            {
                try
                {
                    this.OpenConnection();

                    dr = this.cmdWraperCommand.ExecuteReader();
                    if (dr.Read())
                    {
                        int countFields = dr.FieldCount;

                        #region Get Position and Properties
                        string columnName = string.Empty;
                        List<FillEttItem> lstFillItems = new List<FillEttItem>(countFields);
                        for (int indField = 0; indField < countFields; indField++)
                        {
                            columnName = dr.GetName(indField);
                            PropertyInfo prop = typeof(T).GetProperty(columnName);

                            PropertyFinder propFinder = new PropertyFinder();
                            propFinder.FindProperties<T>(columnName);

                            if (propFinder.HasSubProperties)
                            {
                                foreach (SubProperties subProp in propFinder.SubProperties)
                                {
                                    FillEttItem fei = new FillEttItem();
                                    fei.HasSubProperty = true;
                                    fei.ReflectedSubProperties = subProp.ToArray();
                                    fei.IndexColumn = indField;
                                    fei.DataBaseType = dr.GetFieldType(indField);

                                    PropertyInfo fnProp = subProp[subProp.Count - 1];
                                    ConverterAttribute ca = fnProp.GetCustomAttribute<ConverterAttribute>();
                                    if (ca != null)
                                        fei.Converter = ca.Converter;

                                    lstFillItems.Add(fei);
                                }
                            }

                            if (prop != null)
                            {
                                FillEttItem fei = new FillEttItem();
                                fei.Property = prop;
                                fei.IndexColumn = indField;
                                fei.DataBaseType = dr.GetFieldType(indField);

                                ConverterAttribute ca = prop.GetCustomAttribute<ConverterAttribute>();
                                if (ca != null)
                                    fei.Converter = ca.Converter;

                                lstFillItems.Add(fei);
                            }
                            else if (useExtendProperties)
                            {
                                FillEttItem fei = new FillEttItem();
                                fei.IsExtendProperty = true;
                                fei.IndexColumn = indField;
                                fei.ExtendPropertyName = columnName;
                                lstFillItems.Add(fei);
                            }
                        }
                        #endregion

                        #region Fill Entities
                        do
                        {
                            T obj = new T();
                            obj.EntityState = EntityItemState.Unchanged;
                            #region Fill Fields
                            for (int indFEI = 0; indFEI < lstFillItems.Count; indFEI++)
                            {
                                lstFillItems[indFEI].SetValue(obj, dr);
                            }
                            #endregion
                            lstEntity.Add(obj);
                        }
                        while (dr.Read());
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    if (attempsControl.ThrowException())
                        throw new OrmException(ex.Message, ex);
                }
                finally
                {
                    this.CloseConnection();
                    if (dr != null)
                        dr.Close();
                }
            } while (attempsControl.TryAgain);

            return lstEntity;
        }
        #endregion

        #region public DataTable ExecuteDataTable()
        public DataTable ExecuteDataTable()
        {
            return this.PrepareTransaction() ?
                this.ExecuteDataTableWithTransaction() :
                this.ExecuteDataTableWithoutTransaction();
        }

        private DataTable ExecuteDataTableWithTransaction()
        {
            DataTable dt = new DataTable();
            AttemptsControl attemptsControl = new AttemptsControl(this.intMaxAttempts);
            do
            {
                IDataReader dr = null;
                try
                {
                    dr = this.cmdWraperCommand.ExecuteReader();
                    dt.FromDataReader(dr);
                }
                catch (Exception ex)
                {
                    if (attemptsControl.ThrowException())
                        throw new OrmException(ex.Message, ex);
                }
                finally
                {
                    if (dr != null)
                        dr.Close();
                }

            } while (attemptsControl.TryAgain);
            return dt;
        }

        private DataTable ExecuteDataTableWithoutTransaction()
        {
            DataTable dt = new DataTable();
            AttemptsControl attemptsControl = new AttemptsControl(this.intMaxAttempts);
            do
            {
                IDataReader dr = null;
                try
                {
                    this.OpenConnection();
                    dr = this.cmdWraperCommand.ExecuteReader();
                    dt.FromDataReader(dr);
                }
                catch (Exception ex)
                {
                    if (attemptsControl.ThrowException())
                        throw new OrmException(ex.Message, ex);
                }
                finally
                {
                    this.CloseConnection();
                    if (dr != null)
                        dr.Close();
                }

            } while (attemptsControl.TryAgain);
            return dt;
        }
        #endregion

        private void OpenConnection()
        {
            if (this.Connection.State == ConnectionState.Closed)
                this.Connection.Open();
        }

        private void CloseConnection()
        {
            if (this.Connection.State != ConnectionState.Closed)
                this.Connection.Close();
        }

        public int ParameterIndex
        {
            get
            {
                return this.Parameters.Count;
            }
        }

        #region Attempts Use
        /*
            AttemptsControl attemptsControl = new AttemptsControl(this.intMaxAttempts);
            do
            {
                try
                {

                }
                catch (Exception ex)
                {
                    if (attemptsControl.ThrowException())
                        throw new OrmException(ex.Message, ex);
                }
                finally
                {
                }

            } while (attemptsControl.TryAgain);
        */
        #endregion

        #region Auxiliar Classes

        #region PropertyFinder

        internal class PropertyFinder
        {

            public bool HasSubProperties
            {
                get
                {
                    return this.SubProperties.Count > 0;
                }
            }

            private List<SubProperties> lstSubProperties = new List<SubProperties>();
            public List<SubProperties> SubProperties
            {
                get
                {
                    return lstSubProperties;
                }
            }

            public void FindProperties<T>(string columnName)
            {
                columnName = columnName.ToLower();
                Type tp = typeof(T);
                Stack<PropertyInfo> stkProp = new Stack<PropertyInfo>();
                this.FindProperties(tp, columnName, stkProp);
            }

            private void FindProperties(Type tp, string columnName, Stack<PropertyInfo> stkProp)
            {
                PropertyInfo[] props = tp.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (PropertyInfo prop in props)
                {
                    stkProp.Push(prop);
                    if (prop.PropertyType.IsEntity())
                        FindProperty(prop.PropertyType, columnName, stkProp);

                    stkProp.Pop();
                }
            }

            private void FindProperty(Type tp, string columnName, Stack<PropertyInfo> stkProp)
            {
                PropertyInfo[] props = tp.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                foreach (PropertyInfo prop in props)
                {
                    stkProp.Push(prop);

                    if (prop.Name.ToLower().Equals(columnName))
                    {
                        this.SubProperties.Add(new SubProperties(stkProp));
                    }
                    else if (prop.PropertyType.IsEntity())
                    {
                        FindProperty(prop.PropertyType, columnName, stkProp);
                    }

                    stkProp.Pop();
                }
            }

        }

        internal class SubProperties : List<PropertyInfo>
        {
            public SubProperties(Stack<PropertyInfo> stkProps)
            {
                PropertyInfo[] vetP = stkProps.ToArray();
                Array.Reverse(vetP);
                this.AddRange(vetP);
            }
        }

        #endregion

        #region FillEttItem
        protected class FillEttItem : IDisposable
        {
            private PropertyInfo prop;
            public PropertyInfo Property
            {
                get { return prop; }
                set
                {
                    prop = value;

                    tpSimpleType = Nullable.GetUnderlyingType(prop.PropertyType);
                    if (tpSimpleType == null)
                        tpSimpleType = prop.PropertyType;

                    if (tpDataBaseType != null)
                        blnIsSameType = tpSimpleType == tpDataBaseType;
                }
            }

            private PropertyInfo[] subProps;
            public PropertyInfo[] ReflectedSubProperties
            {
                get
                {
                    return subProps;
                }
                set
                {
                    subProps = value;

                    prop = subProps[subProps.Length - 1];
                    tpSimpleType = Nullable.GetUnderlyingType(prop.PropertyType);
                    if (tpSimpleType == null)
                        tpSimpleType = prop.PropertyType;

                    if (tpDataBaseType != null)
                        blnIsSameType = tpSimpleType == tpDataBaseType;
                }
            }

            public ITypeConverter Converter { get; set; }

            private bool blnHasSubProperty = false;
            public bool HasSubProperty
            {
                get
                {
                    return blnHasSubProperty;
                }
                set
                {
                    blnHasSubProperty = value;
                }
            }

            private Type tpSimpleType;
            public Type SimpleType
            {
                get { return tpSimpleType; }
            }

            private bool blnIsExtendProperty = false;
            public bool IsExtendProperty
            {
                get
                {
                    return this.blnIsExtendProperty;
                }
                set
                {
                    this.blnIsExtendProperty = value;
                }
            }

            private int intIndexColumn;
            public int IndexColumn
            {
                get { return intIndexColumn; }
                set { intIndexColumn = value; }
            }

            private string strExtendPropertyName = null;
            public string ExtendPropertyName
            {
                get
                {
                    return this.strExtendPropertyName;
                }
                set
                {
                    this.strExtendPropertyName = value;
                }
            }

            private Type tpDataBaseType;
            public Type DataBaseType
            {
                get { return tpDataBaseType; }
                set
                {
                    tpDataBaseType = value;

                    if (tpSimpleType != null)
                        blnIsSameType = tpSimpleType == tpDataBaseType;
                }
            }

            protected bool blnIsSameType;
            public bool IsSameType
            {
                get { return blnIsSameType; }
            }

            public virtual void SetValue(object obj, object value)
            {
                this.SetValue(this.Property, obj, value);
            }

            public virtual void SetValue(PropertyInfo prop, object obj, object value)
            {
                if (value != null && !Convert.IsDBNull(value))
                {
                    if (blnIsSameType)
                    {
                        prop.SetValue(obj, value, null);
                    }
                    else
                    {
                        if (Converter != null)
                        {
                            prop.SetValue(obj, Converter.ConvertFrom(value), null);
                        }
                        else
                        {
                            prop.SetValue(obj, Convert.ChangeType(value, tpSimpleType), null);
                        }
                    }
                }
            }

            public virtual void SetValue(EntityBase obj, IDataReader dr)
            {
                object value = dr.GetValue(this.intIndexColumn);
                if (this.IsExtendProperty)
                {
                    obj[this.ExtendPropertyName] = value;
                }
                else if (this.HasSubProperty)
                {
                    this.SetValueOnSubProperty(obj, value);
                }
                else
                {
                    this.SetValue(obj, value);
                }
            }

            public virtual void SetValueOnSubProperty(EntityBase obj, object value)
            {
                for (int index = 0; index < this.ReflectedSubProperties.Length - 1; index++)
                {
                    PropertyInfo propRef = this.ReflectedSubProperties[index];
                    object aux = propRef.GetValue(obj, null);
                    if (aux == null)
                    {
                        aux = Activator.CreateInstance(propRef.PropertyType);
                        propRef.SetValue(obj, aux, null);
                    }
                    obj = aux as EntityBase;
                }
                this.SetValue(prop, obj, value);
            }

            #region IDisposable Members

            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }

            #endregion
        }
        #endregion

        #region AttemptsControl

        internal class AttemptsControl
        {
            public AttemptsControl(int intMaxAttempts)
            {
                this.intMaxAttempts = intMaxAttempts;
            }

            private bool blnTryAgain = false;
            public bool TryAgain
            {
                get
                {
                    return this.blnTryAgain;
                }
            }

            private int intAttempt = 1;
            public int Attempt
            {
                get
                {
                    return this.intAttempt;
                }
            }

            private int intMaxAttempts = 1;
            public int MaxAttempts
            {
                get
                {
                    return this.intMaxAttempts;
                }
                set
                {
                    this.intMaxAttempts = value;
                }
            }

            private int intSleepTimeBetweenAttempts = 30;
            public int SleepTimeBetweenAttempts
            {
                get
                {
                    return this.intSleepTimeBetweenAttempts;
                }
            }

            public bool ThrowException()
            {
                if (this.intAttempt < this.intMaxAttempts)
                {
                    this.intAttempt++;
                    this.blnTryAgain = true;
                    System.Threading.Thread.Sleep(this.intSleepTimeBetweenAttempts);
                    return false;
                }
                this.intAttempt = 1;
                this.blnTryAgain = false;
                return true;
            }
        }

        #endregion

        #endregion
    }
}
