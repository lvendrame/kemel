using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Schema;
using Kemel.Orm.Entity;
using Kemel.Orm.Constants;

namespace Kemel.Orm.QueryDef
{
    public class QueryJoinCollection : List<QueryJoin>
    {
        new public QueryJoin Add(QueryJoin item)
        {
            base.Add(item);
            return item;
        }
    }

    public class QueryJoin: TableQuery
    {
        #region Properties

        public JoinType Type { get; set; }

        private JoinConditionCollection lstConditions = null;
        public JoinConditionCollection Conditions
        {
            get
            {
                if (this.lstConditions == null)
                    this.lstConditions = new JoinConditionCollection();
                return this.lstConditions;
            }
        }

        #endregion

        #region Constructors
        protected QueryJoin(TableSchema tableSchema, Query parent)
            : base(tableSchema, parent)
        {
        }

        protected QueryJoin(string tableName, Query parent)
            : base(tableName, parent)
        {
        }

        protected QueryJoin(Query subQuery, Query parent)
            : base(subQuery, parent)
        {
        }
        #endregion

        #region Static Methods

        public static QueryJoin Join(TableSchema table, Query parent)
        {
            return new QueryJoin(table, parent);
        }

        public static QueryJoin Join<TEtt>(Query parent)
            where TEtt : EntityBase
        {
            TableSchema table = TableSchema.FromEntity<TEtt>();
            return new QueryJoin(table, parent);
        }

        public static QueryJoin Join(EntityBase entity, Query parent)
        {
            TableSchema table = TableSchema.FromEntity(entity);
            return new QueryJoin(table, parent);
        }

        public static QueryJoin Join(string tableName, Query parent)
        {
            TableSchema table = TableSchema.FromTableName(tableName);
            return new QueryJoin(table, parent);
        }

        public static QueryJoin Join(Query subQuery, Query parent)
        {
            return new QueryJoin(subQuery, parent);
        }

        #endregion

        #region Methods

        new public QueryJoin AllColumns()
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

            return this;
        }

        new public QueryJoin Columns(params string[] columns)
        {
            foreach (string column in columns)
            {
                this.Column(column);
            }

            return this;
        }

        new public ColumnJoin Column(string columnName)
        {
            ColumnJoin column = new ColumnJoin(columnName, this);
            this.ColumnsQuery.Add(column);
            return column;
        }

        new public ColumnJoin Column(ColumnSchema columnSchema)
        {
            ColumnJoin column = new ColumnJoin(columnSchema, this);
            this.ColumnsQuery.Add(column);
            return column;
        }

        new public QueryJoin As(string alias)
        {
            this.Alias = alias;
            return this;
        }

        new public QueryJoin WithNoLock()
        {
            this.NoLock = true;
            return this;
        }

        #region End Join
        public Query AsInner()
        {
            this.Type = JoinType.Inner;
            return this.Parent;
        }

        public Query AsLeft()
        {
            this.Type = JoinType.Left;
            return this.Parent;
        }

        public Query AsRight()
        {
            this.Type = JoinType.Right;
            return this.Parent;
        }

        public Query AsFull()
        {
            this.Type = JoinType.Full;
            return this.Parent;
        }

        public Query AsLeftOuter()
        {
            this.Type = JoinType.LeftOuter;
            return this.Parent;
        }

        public Query AsRightOuter()
        {
            this.Type = JoinType.RightOuter;
            return this.Parent;
        }
        #endregion

        #region Conditions

        public JoinCondition On(ColumnSchema columnFrom)
        {
            if (this.TableSchema != null && this.TableSchema.IsLogicalExclusion)
            {
                JoinCondition retCondition = this.Conditions.Add(JoinCondition.On(columnFrom, this));
                this.Conditions.Add(JoinCondition.And(this.TableSchema.LogicalExclusionColumn, this)).Equal(false);
                return retCondition;
            }
            else
                return this.Conditions.Add(JoinCondition.On(columnFrom, this));
        }

        public JoinCondition And(ColumnSchema columnFrom)
        {
            return this.Conditions.Add(JoinCondition.And(columnFrom, this));
        }

        public JoinCondition Or(ColumnSchema columnFrom)
        {
            return this.Conditions.Add(JoinCondition.Or(columnFrom, this));
        }

        public JoinCondition On(string columnName)
        {
            if (this.TableSchema != null && this.TableSchema.IsLogicalExclusion)
            {
                JoinCondition retCondition = this.Conditions.Add(JoinCondition.On(columnName, this));
                this.Conditions.Add(JoinCondition.And(this.TableSchema.LogicalExclusionColumn, this)).Equal(false);
                return retCondition;
            }
            else
                return this.Conditions.Add(JoinCondition.On(columnName, this));
        }

        public JoinCondition And(string columnName)
        {
            return this.Conditions.Add(JoinCondition.And(columnName, this));
        }

        public JoinCondition Or(string columnName)
        {
            return this.Conditions.Add(JoinCondition.Or(columnName, this));
        }



        public JoinCondition On(string tableName, ColumnSchema columnFrom)
        {
            if (this.TableSchema != null && this.TableSchema.IsLogicalExclusion)
            {
                JoinCondition retCondition = this.Conditions.Add(JoinCondition.On(tableName, columnFrom, this));
                this.Conditions.Add(JoinCondition.And(this.TableSchema.LogicalExclusionColumn, this)).Equal(false);
                return retCondition;
            }
            else
                return this.Conditions.Add(JoinCondition.On(tableName, columnFrom, this));
        }

        public JoinCondition And(string tableName, ColumnSchema columnFrom)
        {
            return this.Conditions.Add(JoinCondition.And(tableName, columnFrom, this));
        }

        public JoinCondition Or(string tableName, ColumnSchema columnFrom)
        {
            return this.Conditions.Add(JoinCondition.Or(tableName, columnFrom, this));
        }

        public JoinCondition On(string tableName, string columnName)
        {
            if (this.TableSchema != null && this.TableSchema.IsLogicalExclusion)
            {
                JoinCondition retCondition = this.Conditions.Add(JoinCondition.On(tableName, columnName, this));
                this.Conditions.Add(JoinCondition.And(this.TableSchema.LogicalExclusionColumn, this)).Equal(false);
                return retCondition;
            }
            else
                return this.Conditions.Add(JoinCondition.On(tableName, columnName, this));
        }

        public JoinCondition And(string tableName, string columnName)
        {
            return this.Conditions.Add(JoinCondition.And(tableName, columnName, this));
        }

        public JoinCondition Or(string tableName, string columnName)
        {
            return this.Conditions.Add(JoinCondition.Or(tableName, columnName, this));
        }

        #endregion

        #endregion
    }
}
