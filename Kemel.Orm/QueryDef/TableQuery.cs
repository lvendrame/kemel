using System;
using System.Collections.Generic;
using Kemel.Orm.Entity;
using Kemel.Orm.Schema;
using System.Text;
using Kemel.Orm.Constants;
using Kemel.Orm.Providers;

namespace Kemel.Orm.QueryDef
{
    public class TableQueryCollection : List<TableQuery>
    {

        new public TableQuery Add(TableQuery item)
        {
            base.Add(item);
            return item;
        }

    }


    public class TableQuery
    {
        #region Properties
        public TableSchema TableSchema { get; set; }
        public string TableName { get; set; }
        public Query SubQuery { get; set; }
        public string Alias { get; set; }

        private bool blnWithNolock = false;
        public bool NoLock
        {
            get
            {
                return this.blnWithNolock;
            }
            set
            {
                this.blnWithNolock = value;
            }
        }

        private FunctionCollection lstFunctions = null;
        public FunctionCollection Functions
        {
            get
            {
                if (this.lstFunctions == null)
                    this.lstFunctions = new FunctionCollection();
                return this.lstFunctions;
            }
        }

        private AggregateCollection lstAggregateds = null;
        public AggregateCollection Aggregateds
        {
            get
            {
                if (this.lstAggregateds == null)
                    this.lstAggregateds = new AggregateCollection();
                return this.lstAggregateds;
            }
        }

        private ColumnQueryCollectoin lstColumns = null;
        public ColumnQueryCollectoin ColumnsQuery
        {
            get
            {
                if (this.lstColumns == null)
                    this.lstColumns = new ColumnQueryCollectoin();
                return this.lstColumns;
            }
        }

        public Query Parent { get; set; }

        public string PatternName
        {
            get
            {
                return (this.TableSchema == null ? this.TableName : this.TableSchema.Name);
            }
        }

        public string ColumnPrefix
        {
            get
            {
                if (string.IsNullOrEmpty(this.Alias))
                {
                    return this.PatternName;
                }
                else
                {
                    return this.Alias;
                }
            }
        }

        public bool HasAlias
        {
            get
            {
                return !string.IsNullOrEmpty(this.Alias);
            }
        }

        #endregion

        #region Static Methods
        public static TableQuery From(TableSchema table, Query parent)
        {
            return new TableQuery(table, parent);
        }

        public static TableQuery From<TEtt>(Query parent)
            where TEtt : EntityBase
        {
            TableSchema table = SchemaContainer.GetSchema<TEtt>();
            return new TableQuery(table, parent);
        }

        public static TableQuery From(EntityBase entity, Query parent)
        {
            TableSchema table = SchemaContainer.GetSchema(entity);
            return new TableQuery(table, parent);
        }

        public static TableQuery From(string tableName, Query parent)
        {
            return new TableQuery(tableName, parent);
        }

        public static TableQuery From(Query subQuery, Query parent)
        {
            return new TableQuery(subQuery, parent);
        }
        #endregion

        #region Methods

        public ColumnSchema FindColumnSchema(string columnName)
        {
            if (this.TableSchema != null)
            {
                columnName = columnName.ToUpper();
                foreach (ColumnSchema column in this.TableSchema.Columns)
                {
                    if (column.Name.ToUpper().Equals(columnName))
                        return column;
                }
            }
            return null;
        }

        public ColumnQuery FindColumnQuery(string columnName)
        {
            columnName = columnName.ToUpper();
            foreach (ColumnQuery column in this.ColumnsQuery)
            {
                if (column.PatternColumnName.ToUpper().Equals(columnName))
                    return column;
            }

            return new ColumnQuery(columnName, this);
        }

        public ColumnQuery FindColumnQuery(Kemel.Orm.Schema.ColumnSchema columnFrom)
        {
            if (columnFrom.Parent.Name == this.TableSchema.Name)
            {
                string columnName = columnFrom.Name.ToUpper();
                foreach (ColumnQuery column in this.ColumnsQuery)
                {
                    if (column.PatternColumnName.ToUpper().Equals(columnName))
                        return column;
                }
            }
            else
            {
                return this.Parent.FindColumnQueryInTables(columnFrom);
            }

            return new ColumnQuery(columnFrom, this);
        }

        public ColumnQuery FindColumnQuery(string tableName, string columnName)
        {
            TableQuery tq = this.Parent.FindTableQuery(tableName);

            if (tq == null)
                throw new OrmException(Messages.TableDoesNotExistInQuery);

            columnName = columnName.ToUpper();
            foreach (ColumnQuery column in tq.ColumnsQuery)
            {
                if (column.PatternColumnName.ToUpper().Equals(columnName))
                    return column;
            }

            return new ColumnQuery(columnName, tq);
        }

        public ColumnQuery FindColumnQuery(string tableName, Kemel.Orm.Schema.ColumnSchema columnFrom)
        {
            TableQuery tq = this.Parent.FindTableQuery(tableName);

            if (tq == null)
                throw new OrmException(Messages.TableDoesNotExistInQuery);

            if (columnFrom.Parent.Name == tq.TableSchema.Name)
            {
                string columnName = columnFrom.Name.ToUpper();
                foreach (ColumnQuery column in tq.ColumnsQuery)
                {
                    if (column.PatternColumnName.ToUpper().Equals(columnName))
                        return column;
                }
            }
            else
            {
                throw new OrmException(Messages.DontHaveColumnInColumnCollection);
            }

            return new ColumnQuery(columnFrom, tq);
        }

        protected TableQuery(TableSchema tableSchema, Query parent)
        {
            this.TableSchema = tableSchema;
            this.Parent = parent;
        }

        protected TableQuery(string tableName, Query parent)
        {
            this.TableSchema = SchemaContainer.GetSchema(tableName);
            if (this.TableSchema == null)
                this.TableName = tableName;
            this.Parent = parent;
        }

        protected TableQuery(Query subQuery, Query parent)
        {
            this.SubQuery = subQuery;
            this.Parent = parent;
        }

        public Query AllColumns()
        {
            if (this.TableSchema != null)
            {
                foreach (ColumnSchema columnSchema in this.TableSchema.Columns)
                {
                    this.Column(columnSchema);
                }
            }
            else
            {
                this.Column("*");
            }

            return this.Parent;
        }

        public Query Columns(params string[] columns)
        {
            foreach (string column in columns)
            {
                this.Column(column);
            }

            return this.Parent;
        }

        public ColumnQuery Column(string columnName)
        {
            return this.ColumnsQuery.Add(new ColumnQuery(columnName, this));
        }

        public ColumnQuery Column(ColumnSchema columnSchema)
        {
            return this.ColumnsQuery.Add(new ColumnQuery(columnSchema, this));
        }

        public TableQuery WithNoLock()
        {
            this.NoLock = true;
            return this;
        }

        public TableQuery As(string alias)
        {
            this.Alias = alias;
            return this;
        }

        public bool EqualsTableNameOrAlias(string tableName)
        {
            if (this.TableSchema == null)
            {
                if (!string.IsNullOrEmpty(this.TableName) && this.TableName.ToUpper().Equals(tableName))
                    return true;
            }
            else
            {
                if (this.TableSchema.Name.ToUpper().Equals(tableName))
                    return true;
            }

            if (!string.IsNullOrEmpty(this.Alias) && this.Alias.ToUpper().Equals(tableName))
                return true;

            return false;
        }

        public bool EqualsSchemaTableName(string tableName)
        {
            if (this.TableSchema != null)
            {
                return this.TableSchema.Name.ToUpper().Equals(tableName);
            }
            else
            {
                return false;
            }
        }

        public Query End()
        {
            return this.Parent;
        }

        #region Aggregate

        #region Methods Function Parameter

        public Aggregate Count(object parameter)
        {
            return this.Aggregateds.Add(Aggregate.Count(parameter, this));
        }

        public Aggregate Sum(object parameter)
        {
            return this.Aggregateds.Add(Aggregate.Sum(parameter, this));
        }

        public Aggregate Avg(object parameter)
        {
            return this.Aggregateds.Add(Aggregate.Avg(parameter, this));
        }

        public Aggregate Min(object parameter)
        {
            return this.Aggregateds.Add(Aggregate.Min(parameter, this));
        }

        public Aggregate Max(object parameter)
        {
            return this.Aggregateds.Add(Aggregate.Max(parameter, this));
        }

        public Aggregate StDev(object parameter)
        {
            return this.Aggregateds.Add(Aggregate.StDev(parameter, this));
        }

        public Aggregate StDevP(object parameter)
        {
            return this.Aggregateds.Add(Aggregate.StDevP(parameter, this));
        }

        public Aggregate Var(object parameter)
        {
            return this.Aggregateds.Add(Aggregate.Var(parameter, this));
        }

        public Aggregate VarP(object parameter)
        {
            return this.Aggregateds.Add(Aggregate.VarP(parameter, this));
        }

        public Aggregate Convert(object parameter)
        {
            return this.Aggregateds.Add(Aggregate.Convert(parameter, this));
        }

        #endregion

        #region Methods Function Column Name

        public Aggregate CountColumn(string columnName)
        {
            return this.Aggregateds.Add(Aggregate.CountColumn(columnName, this));
        }

        public Aggregate SumColumn(string columnName)
        {
            return this.Aggregateds.Add(Aggregate.SumColumn(columnName, this));
        }

        public Aggregate AvgColumn(string columnName)
        {
            return this.Aggregateds.Add(Aggregate.AvgColumn(columnName, this));
        }

        public Aggregate MinColumn(string columnName)
        {
            return this.Aggregateds.Add(Aggregate.MinColumn(columnName, this));
        }

        public Aggregate MaxColumn(string columnName)
        {
            return this.Aggregateds.Add(Aggregate.MaxColumn(columnName, this));
        }

        public Aggregate StDevColumn(string columnName)
        {
            return this.Aggregateds.Add(Aggregate.StDevColumn(columnName, this));
        }

        public Aggregate StDevPColumn(string columnName)
        {
            return this.Aggregateds.Add(Aggregate.StDevPColumn(columnName, this));
        }

        public Aggregate VarColumn(string columnName)
        {
            return this.Aggregateds.Add(Aggregate.VarColumn(columnName, this));
        }

        public Aggregate VarPColumn(string columnName)
        {
            return this.Aggregateds.Add(Aggregate.VarPColumn(columnName, this));
        }

        public Aggregate ConvertColumn(string columnName)
        {
            return this.Aggregateds.Add(Aggregate.ConvertColumn(columnName, this));
        }

        #endregion

        #region Methods Function Column

        public Aggregate Count(ColumnSchema column)
        {
            return this.Aggregateds.Add(Aggregate.CountColumn(column, this));
        }

        public Aggregate Sum(ColumnSchema column)
        {
            return this.Aggregateds.Add(Aggregate.SumColumn(column, this));
        }

        public Aggregate Avg(ColumnSchema column)
        {
            return this.Aggregateds.Add(Aggregate.AvgColumn(column, this));
        }

        public Aggregate Min(ColumnSchema column)
        {
            return this.Aggregateds.Add(Aggregate.MinColumn(column, this));
        }

        public Aggregate Max(ColumnSchema column)
        {
            return this.Aggregateds.Add(Aggregate.MaxColumn(column, this));
        }

        public Aggregate StDev(ColumnSchema column)
        {
            return this.Aggregateds.Add(Aggregate.StDevColumn(column, this));
        }

        public Aggregate StDevP(ColumnSchema column)
        {
            return this.Aggregateds.Add(Aggregate.StDevPColumn(column, this));
        }

        public Aggregate Var(ColumnSchema column)
        {
            return this.Aggregateds.Add(Aggregate.VarColumn(column, this));
        }

        public Aggregate VarP(ColumnSchema column)
        {
            return this.Aggregateds.Add(Aggregate.VarPColumn(column, this));
        }

        public Aggregate Convert(ColumnSchema column)
        {
            return this.Aggregateds.Add(Aggregate.ConvertColumn(column, this));
        }

        #endregion

        #region Methods Function Sub-Aggregate

        public Aggregate Count(Aggregate subAggregate)
        {
            return this.Aggregateds.Add(Aggregate.Count(subAggregate, this));
        }

        public Aggregate Sum(Aggregate subAggregate)
        {
            return this.Aggregateds.Add(Aggregate.Sum(subAggregate, this));
        }

        public Aggregate Avg(Aggregate subAggregate)
        {
            return this.Aggregateds.Add(Aggregate.Avg(subAggregate, this));
        }

        public Aggregate Min(Aggregate subAggregate)
        {
            return this.Aggregateds.Add(Aggregate.Min(subAggregate, this));
        }

        public Aggregate Max(Aggregate subAggregate)
        {
            return this.Aggregateds.Add(Aggregate.Max(subAggregate, this));
        }

        public Aggregate StDev(Aggregate subAggregate)
        {
            return this.Aggregateds.Add(Aggregate.StDev(subAggregate, this));
        }

        public Aggregate StDevP(Aggregate subAggregate)
        {
            return this.Aggregateds.Add(Aggregate.StDevP(subAggregate, this));
        }

        public Aggregate Var(Aggregate subAggregate)
        {
            return this.Aggregateds.Add(Aggregate.Var(subAggregate, this));
        }

        public Aggregate VarP(Aggregate subAggregate)
        {
            return this.Aggregateds.Add(Aggregate.VarP(subAggregate, this));
        }

        public Aggregate Convert(Aggregate subAggregate)
        {
            return this.Aggregateds.Add(Aggregate.Convert(subAggregate, this));
        }

        #endregion

        #endregion

        #endregion

        public QueryJoin AsQueryJoin()
        {
            return this as QueryJoin;
        }
    }
}
