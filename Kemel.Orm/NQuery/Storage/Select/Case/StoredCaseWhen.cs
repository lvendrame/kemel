using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.NQuery.Storage;
using Kemel.Orm.NQuery.Storage.Value;
using Kemel.Orm.Schema;
using Kemel.Orm.NQuery.Storage.Table;
using Kemel.Orm.NQuery.Storage.Function;
using Kemel.Orm.Entity;

namespace Kemel.Orm.NQuery.Storage.StoredSelect.Case
{

    public class StoredCaseWhenCollection : List<StoredCaseWhen>
    {

        public new StoredCaseWhen Add(StoredCaseWhen item)
        {
            base.Add(item);
            return item;
        }

    }

    public class StoredCaseWhen : IGetQuery
    {

        public StoredCaseWhen(Query query)
        {
            this.qryQuery = query;
        }

        private Query qryQuery;
        public Query Query
        {
            get { return qryQuery; }
            set { qryQuery = value; }
        }

        public NQuery.Storage.Query GetQuery()
        {
            return qryQuery;
        }

        public void SetQuery(NQuery.Storage.Query query)
        {
            qryQuery = query;
        }

        private StoredValue stvElseExpression;
        public StoredValue ElseExpression
        {
            get { return stvElseExpression; }
            set { stvElseExpression = value; }
        }

        private List<CaseCondition> lstWhen;
        public List<CaseCondition> Whens
        {
            get
            {
                if ((lstWhen == null))
                {
                    lstWhen = new List<CaseCondition>();
                }
                return lstWhen;
            }
        }

        #region "Constraints"

        private CaseCondition Add(CaseCondition condition)
        {
            this.Whens.Add(condition);
            return condition;
        }

        /// <summary>
        /// Where the column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public CaseCondition WhenValue(ColumnSchema column)
        {
            return this.Add(StorageFactory.CaseConstraint.CreateCondition(StorageFactory.Column.Create(column, this.Query), this));
        }

        /// <summary>
        /// Where the specified Column Name.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public CaseCondition WhenValue(TableSchema table, string columnName)
        {
            return this.Add(StorageFactory.CaseConstraint.CreateCondition(StorageFactory.Column.Create(columnName, table, this.Query), this));
        }

        /// <summary>
        /// Where the specified Constant string value.
        /// </summary>
        /// <param name="columnName">Constant string value.</param>
        /// <returns></returns>
        public CaseCondition WhenValue(string columnName)
        {
            return this.Add(StorageFactory.CaseConstraint.CreateCondition(StorageFactory.Column.Create(columnName, this.Query), this));
        }

        /// <summary>
        /// Where the specified Column Name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public CaseCondition WhenValue(string tableName, string columnName)
        {
            return this.Add(StorageFactory.CaseConstraint.CreateCondition(StorageFactory.Column.Create(columnName, tableName, this.Query), this));
        }

        /// <summary>
        /// Where the specified Column Name.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public CaseCondition WhenValue<TEtt>(string columnName) where TEtt : EntityBase
        {
            return this.Add(StorageFactory.CaseConstraint.CreateCondition(StorageFactory.Column.Create<TEtt>(columnName, this.Query), this));
        }

        #endregion

        #region "ElseDo"

        private StoredCaseWhen ElseDo(StoredValue expression)
        {
            this.stvElseExpression = expression;
            return this;
        }

        public StoredCaseWhen ElseDo(ColumnSchema column, Query query)
        {
            return this.ElseDo(StorageFactory.Value.Create(column, this.Query));
        }

        public StoredCaseWhen ElseDo(ColumnSchema column, StoredTable table, Query query)
        {
            return this.ElseDo(StorageFactory.Value.Create(column, table, this.Query));
        }

        public StoredCaseWhen ElseDo(string columnName, StoredTable table, Query query)
        {
            return this.ElseDo(StorageFactory.Value.Create(columnName, table, this.Query));
        }

        public StoredCaseWhen ElseDo(object value)
        {
            return this.ElseDo(StorageFactory.Value.Create(value));
        }

        public StoredCaseWhen ElseDo(Query subQuery)
        {
            return this.ElseDo(StorageFactory.Value.Create(subQuery, this.Query));
        }

        public StoredCaseWhen ElseDo(StoredFunction stFunction)
        {
            return this.ElseDo(StorageFactory.Value.Create(stFunction));
        }

        public StoredCaseWhen ElseDo(TableSchema table, string columnName, Query query)
        {
            return this.ElseDo(StorageFactory.Value.CreateValue(table, columnName, this.Query));
        }

        public StoredCaseWhen ElseDo<TEtt>(string columnName, Query query) where TEtt : EntityBase
        {
            return this.ElseDo(StorageFactory.Value.CreateValue<TEtt>(columnName, this.Query));
        }

        public StoredCaseWhen ElseDo(string tableName, string columnName, Query query)
        {
            return this.ElseDo(StorageFactory.Value.CreateValue(tableName, columnName, this.Query));
        }

        #endregion

    }

}
