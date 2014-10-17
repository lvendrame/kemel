using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Entity;
using Kemel.Orm.Data;
using Kemel.Orm.Providers;

namespace Kemel.Orm.Base
{
    public class Business<TEtt, TDal>: IDisposable, ITransactable
        where TEtt : EntityBase, new()
        where TDal : Dal<TEtt>, new()
    {

        public Provider CurrentProvider
        {
            get
            {
                return this.Dal.CurrentProvider;
            }
        }


        private TDal dalDal = null;
        public TDal Dal
        {
            get
            {
                if (this.dalDal == null)
                    this.dalDal = new TDal();
                return this.dalDal;
            }
        }

        public OrmTransaction Transaction {
            get
            {
                return this.Dal.Transaction;
            }
            set
            {
                this.Dal.Transaction = value;
            }
        }

        public OrmTransaction CreateTransaction()
        {
            return this.Dal.CreateTransaction();
        }

        public void BeginTransaction()
        {
            this.Dal.BeginTransaction();
        }

        public List<ITransactable> Transactables
        {
            get
            {
                return this.Dal.Transactables;
            }
        }


        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
