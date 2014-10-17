using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Kemel.Orm.Data
{
    public enum TransactionState
    {
        Unstarted,
        Started,
        Commited,
        Rolledback
    }

    public class OrmTransaction: IDbTransaction
    {

        private IDbTransaction dbtTransactionWraper = null;
        public IDbTransaction TransactionWraper
        {
            get
            {
                return this.dbtTransactionWraper;
            }
        }

        private OrmConnection conConnection = null;
        public OrmConnection Connection
        {
            get
            {
                return this.conConnection;
            }
        }

        private TransactionState enmState = TransactionState.Unstarted;
        public TransactionState State
        {
            get
            {
                return this.enmState;
            }
        }

        #region Events

        public event EventHandler OnBegin;
        public event EventHandler OnCommit;
        public event EventHandler OnRollback;

        #endregion

        #region IDbTransaction Members

        System.Data.IDbConnection IDbTransaction.Connection
        {
            get
            {
                return this.Connection.ConnectionWraper;
            }
        }

        public System.Data.IsolationLevel IsolationLevel
        {
            get
            {
                if (this.dbtTransactionWraper == null)
                    return IsolationLevel.Unspecified;

                return this.dbtTransactionWraper.IsolationLevel;
            }
        }

        public OrmTransaction(OrmConnection connection)
        {
            this.conConnection = connection;
        }

        public void Commit()
        {
            if (this.dbtTransactionWraper != null)
            {
                this.enmState = TransactionState.Commited;
                this.dbtTransactionWraper.Commit();
                if (this.OnCommit != null)
                    this.OnCommit(this, new EventArgs());
            }
        }

        public void Rollback()
        {
            if (this.dbtTransactionWraper != null)
            {
                this.enmState = TransactionState.Rolledback;
                this.dbtTransactionWraper.Rollback();
                if (this.OnRollback != null)
                    this.OnRollback(this, new EventArgs());
            }
        }

        public void Begin(IsolationLevel il)
        {
            this.conConnection.Open();
            this.dbtTransactionWraper = this.conConnection.BeginTransaction(il);
            this.enmState = TransactionState.Started;
            if (this.OnBegin != null)
                this.OnBegin(this, new EventArgs());
        }

        public void Begin()
        {
            this.conConnection.Open();
            this.dbtTransactionWraper = this.conConnection.BeginTransaction();
            this.enmState = TransactionState.Started;
            if (this.OnBegin != null)
                this.OnBegin(this, new EventArgs());
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (this.dbtTransactionWraper != null)
                this.dbtTransactionWraper.Dispose();
        }

        #endregion
    }
}
