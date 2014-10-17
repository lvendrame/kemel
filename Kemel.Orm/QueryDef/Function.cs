using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Entity;
using Kemel.Orm.Schema;

namespace Kemel.Orm.QueryDef
{
    public class FunctionCollection : List<Function>
    {

        new public Function Add(Function item)
        {
            base.Add(item);
            return item;
        }

    }

    public class Function: TableQuery
    {
        #region .ctro
        public Function(string functionName, Query parent) :
            base(functionName, parent)
        {
        }

        public Function(TableSchema functionSchema, Query parent) :
            base(functionSchema, parent)
        {
        }
        #endregion

        public string FunctionName
        {
            get
            {
                return this.PatternName;
            }
        }

        private SetFunctionValueCollection lstSetValues = null;
        /// <summary>
        /// Gets the set values.
        /// </summary>
        /// <value>The set values.</value>
        public SetFunctionValueCollection SetValues
        {
            get
            {
                if (this.lstSetValues == null)
                    this.lstSetValues = new SetFunctionValueCollection();
                return this.lstSetValues;
            }
        }

        /// <summary>
        /// Adds the specified Set Value.
        /// </summary>
        /// <param name="item">The Set Value.</param>
        /// <returns></returns>
        protected SetFunctionValue Add(SetFunctionValue item)
        {
            return this.SetValues.Add(item);
        }

        #region Set

        /// <summary>
        /// Sets the specified column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public SetFunctionValue Set(ColumnSchema column)
        {
            return this.Add(SetFunctionValue.Set(column, this.Parent));
        }

        /// <summary>
        /// Sets the specified column.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public SetFunctionValue Set(TableSchema table, string columnName)
        {
            return this.Add(SetFunctionValue.Set(table, columnName, this.Parent));
        }

        /// <summary>
        /// Sets the specified column.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public SetFunctionValue Set(string tableName, string columnName)
        {
            return this.Add(SetFunctionValue.Set(tableName, columnName, this.Parent));
        }

        /// <summary>
        /// Sets the specified column.
        /// </summary>
        /// <typeparam name="Entity">The type of the ntity.</typeparam>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public SetFunctionValue Set<TEtt>(string columnName)
            where TEtt : EntityBase
        {
            return this.Add(SetFunctionValue.Set<TEtt>(columnName, this.Parent));
        }

        #endregion

        public new Query End()
        {
            return this.Parent;
        }

        public new Query As(string alias)
        {
            base.As(alias);
            return this.Parent;
        }
    }
}
