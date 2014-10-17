using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.NQuery.Storage.Constraint;
using Kemel.Orm.NQuery.Storage.Value;
using Kemel.Orm.NQuery.Storage;
using Kemel.Orm.Schema;
using Kemel.Orm.NQuery.Storage.Table;
using Kemel.Orm.NQuery.Storage.Function;
using Kemel.Orm.Entity;

namespace Kemel.Orm.NQuery.Storage.StoredSelect.Case
{

    public class CaseCondition : ConstraintBase<StoredCaseWhen>
    {

        public CaseCondition(StoredCaseWhen parent)
        {
            this.scwParent = parent;
        }

        private StoredCaseWhen scwParent;
        public StoredCaseWhen CaseWhen
        {
            get { return scwParent; }
        }

        private StoredValue stvThenExpression;
        public StoredValue ThenExpression
        {
            get { return stvThenExpression; }
            set { stvThenExpression = value; }
        }

        #region "So"

        private StoredCaseWhen So(StoredValue expression)
        {
            this.stvThenExpression = expression;
            return this.CaseWhen;
        }

        public StoredCaseWhen So(ColumnSchema column, Query query)
        {
            return this.So(StorageFactory.Value.Create(column, this.CaseWhen.Query));
        }

        public StoredCaseWhen So(ColumnSchema column, StoredTable table, Query query)
        {
            return this.So(StorageFactory.Value.Create(column, table, this.CaseWhen.Query));
        }

        public StoredCaseWhen So(string columnName, StoredTable table, Query query)
        {
            return this.So(StorageFactory.Value.Create(columnName, table, this.CaseWhen.Query));
        }

        public StoredCaseWhen So(object value)
        {
            return this.So(StorageFactory.Value.Create(value));
        }

        public StoredCaseWhen So(Query subQuery)
        {
            return this.So(StorageFactory.Value.Create(subQuery, this.CaseWhen.Query));
        }

        public StoredCaseWhen So(StoredFunction stFunction)
        {
            return this.So(StorageFactory.Value.Create(stFunction));
        }

        public StoredCaseWhen So(TableSchema table, string columnName, Query query)
        {
            return this.So(StorageFactory.Value.CreateValue(table, columnName, this.CaseWhen.Query));
        }

        public StoredCaseWhen So<TEtt>(string columnName, Query query) where TEtt : EntityBase
        {
            return this.So(StorageFactory.Value.CreateValue<TEtt>(columnName, this.CaseWhen.Query));
        }

        public StoredCaseWhen So(string tableName, string columnName, Query query)
        {
            return this.So(StorageFactory.Value.CreateValue(tableName, columnName, this.CaseWhen.Query));
        }

        #endregion

    }

}
