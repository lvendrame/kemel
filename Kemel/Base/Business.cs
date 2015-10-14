using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Entity;
using Kemel.Data;
using Kemel.Providers;

namespace Kemel.Base
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

        public KemelTransaction Transaction {
            get
            {
                return this.Dal.Transaction;
            }
            set
            {
                this.Dal.Transaction = value;
            }
        }

        public KemelTransaction CreateTransaction()
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
