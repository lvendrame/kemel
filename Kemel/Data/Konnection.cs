using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Kemel.Data
{
    public class Konnection: System.Data.IDbConnection
    {

        private IDbConnection conConnectionWraper = null;
        public IDbConnection ConnectionWraper
        {
            get
            {
                return this.conConnectionWraper;
            }
        }

        public Konnection(IDbConnection connection)
        {
            this.conConnectionWraper = connection;
        }

        #region IDbConnection Members

        public KemelTransaction CreateTransaction()
        {
            return new KemelTransaction(this);
        }

        internal IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return this.conConnectionWraper.BeginTransaction(il);
        }

        internal IDbTransaction BeginTransaction()
        {
            return this.conConnectionWraper.BeginTransaction();
        }

        IDbTransaction IDbConnection.BeginTransaction(IsolationLevel il)
        {
            return this.conConnectionWraper.BeginTransaction(il);
        }

        IDbTransaction IDbConnection.BeginTransaction()
        {
            return this.conConnectionWraper.BeginTransaction();
        }

        void IDbConnection.ChangeDatabase(string databaseName)
        {
            this.conConnectionWraper.ChangeDatabase(databaseName);
        }

        public void Close()
        {
            if(this.conConnectionWraper.State != ConnectionState.Closed)
                this.conConnectionWraper.Close();
        }

        public string ConnectionString
        {
            get
            {
                return this.conConnectionWraper.ConnectionString;
            }
            set
            {
                this.conConnectionWraper.ConnectionString = value;
            }
        }

        public int ConnectionTimeout
        {
            get
            {
                return this.conConnectionWraper.ConnectionTimeout;
            }
        }

        public Kommand CreateCommand()
        {
            return new Kommand(this.conConnectionWraper.CreateCommand());
        }

        public Kommand CreateCommand(KemelTransaction transaction)
        {
            return new Kommand(this.conConnectionWraper.CreateCommand(), transaction);
        }

        IDbCommand IDbConnection.CreateCommand()
        {
            return this.conConnectionWraper.CreateCommand();
        }

        public string Database
        {
            get
            {
                return this.conConnectionWraper.Database;
            }
        }

        public void Open()
        {
            try
            {
                if (this.conConnectionWraper.State == ConnectionState.Closed)
                    this.conConnectionWraper.Open();
            }
            catch(Exception expErro)
            {
                throw expErro;
            }
        }

        public ConnectionState State
        {
            get
            {
                return this.conConnectionWraper.State;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.conConnectionWraper.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
