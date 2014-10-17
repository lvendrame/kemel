using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.NQuery.Storage.Table;
using Kemel.Orm.NQuery.Storage;
using Kemel.Orm.Schema;
using Kemel.Orm.Base;
using Kemel.Orm.NQuery.Storage.Column;

namespace Kemel.Orm.NQuery.Storage.Join
{

    public class StoredJoinCollection : List<StoredJoin>
    {

        public new StoredJoin Add(StoredJoin item)
        {
            base.Add(item);
            return item;
        }

        public StoredJoin Find(string name)
        {
            name = name.ToUpper();

            foreach (StoredJoin table in this)
            {
                if ((table.Compare(name)))
                {
                    return table;
                }
            }
            return null;
        }

        public StoredJoin Find(ITableDefinition tableDef)
        {
            return this.Find(tableDef.Name);
        }

    }

    public class StoredJoin : StoredTable
    {

        private JoinTypes objType = JoinTypes.Inner;
        public JoinTypes JoinType
        {
            get { return this.objType; }
            set { this.objType = value; }
        }

        private StoredJoinConstraintCollection lstConditions = null;
        public StoredJoinConstraintCollection Conditions
        {
            get
            {
                if (this.lstConditions == null)
                {
                    this.lstConditions = new StoredJoinConstraintCollection();
                }
                return this.lstConditions;
            }
        }

        public StoredJoin(ITableDefinition tableDef, StoredTypes type, Query parent)
            : base(tableDef, type, parent)
        {
        }

        #region "Methods"

        #region "Shadows"

        public new StoredJoin AllColumns()
        {
            if ((this.Type == StoredTypes.Schema))
            {
                TableSchema tbSchema = (TableSchema)this.TableDefinition;
                foreach (ColumnSchema colSchema in tbSchema.Columns)
                {
                    this.Column(colSchema);
                }
            }
            else
            {
                this.StoredColumns.Add(StorageFactory.JoinColumn.Create("*", this, this.Parent));
            }
            return this;
        }

        public new StoredJoin Columns(params string[] pColumns)
        {
            foreach (string column in pColumns)
            {
                this.Column(column);
            }

            return this;
        }

        public new StoredJoinColumn Column(string columnName)
        {
            StoredJoinColumn retCol = StorageFactory.JoinColumn.Create(columnName, this, this.Parent);
            this.StoredColumns.Add(retCol);
            return retCol;
        }

        public new StoredJoinColumn Column(ColumnSchema columnSchema)
        {
            StoredJoinColumn retCol = StorageFactory.JoinColumn.Create(columnSchema, this, this.Parent);
            this.StoredColumns.Add(retCol);
            return retCol;
        }

        public new StoredJoin As(string p_alias)
        {
            this.Alias = p_alias;
            return this;
        }

        public new StoredJoin WithNoLock()
        {
            this.NoLock = true;
            return this;
        }

        #endregion

        #region "End Join"
        public Query AsInner()
        {
            this.JoinType = JoinTypes.Inner;
            return this.Parent;
        }

        public Query AsLeft()
        {
            this.JoinType = JoinTypes.Left;
            return this.Parent;
        }

        public Query AsRight()
        {
            this.JoinType = JoinTypes.Right;
            return this.Parent;
        }

        public Query AsFull()
        {
            this.JoinType = JoinTypes.Full;
            return this.Parent;
        }

        public Query AsLeftOuter()
        {
            this.JoinType = JoinTypes.LeftOuter;
            return this.Parent;
        }

        public Query AsRightOuter()
        {
            this.JoinType = JoinTypes.RightOuter;
            return this.Parent;
        }
        #endregion

        #region "Conditions"

        private StoredColumn CreateColumn(ColumnSchema column)
        {
            StoredColumn retCol = null;
            if (this.Type == StoredTypes.Schema)
            {
                TableSchema tbSch = this.TableDefinition as TableSchema;
                if (tbSch.Name.Equals(column.Parent.Name))
                {
                    retCol = StorageFactory.Column.Create(column, this, this.Parent);
                }
            }

            if(retCol == null)
                retCol = StorageFactory.Column.Create(column, this.Parent);

            return retCol;
        }

        private StoredColumn CreateColumn(string columnName)
        {
            StoredColumn retCol = null;
            if (this.Type == StoredTypes.Schema)
            {
                TableSchema tbSch = this.TableDefinition as TableSchema;
                if (tbSch.GetColumn(columnName) != null)
                {
                    retCol = StorageFactory.Column.Create(columnName, this, this.Parent);
                }
            }

            if (retCol == null)
                retCol = StorageFactory.Column.Create(columnName, this.Parent);

            return retCol;
        }

        public StoredJoinConstraint On(ColumnSchema columnFrom)
        {
            StoredJoinConstraint retCondition = this.Conditions.Add(StorageFactory.JoinConstraint.CreateWhere(
                   CreateColumn(columnFrom), this));

            if (this.Type == StoredTypes.Schema && (this.TableDefinition as TableSchema).IsLogicalExclusion)
            {
                TableSchema tbSch = this.TableDefinition as TableSchema;
                this.Conditions.Add(StorageFactory.JoinConstraint.CreateAnd(
                    StorageFactory.Column.Create(tbSch.LogicalExclusionColumn, this, this.Parent), this))
                    .Equal(false);
            }

            return retCondition;
        }

        public StoredJoinConstraint And(ColumnSchema columnFrom)
        {
            return this.Conditions.Add(StorageFactory.JoinConstraint.CreateAnd(
                   CreateColumn(columnFrom), this));
        }

        public StoredJoinConstraint Or(ColumnSchema columnFrom)
        {
            return this.Conditions.Add(StorageFactory.JoinConstraint.CreateOr(
                   CreateColumn(columnFrom), this));
        }

        public StoredJoinConstraint On(string columnName)
        {
            StoredJoinConstraint retCondition = this.Conditions.Add(StorageFactory.JoinConstraint.CreateWhere(
                   CreateColumn(columnName), this));

            if (this.Type == StoredTypes.Schema && (this.TableDefinition as TableSchema).IsLogicalExclusion)
            {
                TableSchema tbSch = this.TableDefinition as TableSchema;
                this.Conditions.Add(StorageFactory.JoinConstraint.CreateAnd(
                    StorageFactory.Column.Create(tbSch.LogicalExclusionColumn, this, this.Parent), this))
                    .Equal(false);
            }

            return retCondition;
        }

        public StoredJoinConstraint And(string columnName)
        {
            return this.Conditions.Add(StorageFactory.JoinConstraint.CreateAnd(
                   CreateColumn(columnName), this));
        }

        public StoredJoinConstraint Or(string columnName)
        {
            return this.Conditions.Add(StorageFactory.JoinConstraint.CreateOr(
                   CreateColumn(columnName), this));
        }

        public StoredJoinConstraint On(string tableName, ColumnSchema columnFrom)
        {
            StoredJoinConstraint retCondition = this.Conditions.Add(StorageFactory.JoinConstraint.CreateWhere(
                   StorageFactory.Column.Create(columnFrom, tableName, this.Parent), this));

            if (this.Type == StoredTypes.Schema && (this.TableDefinition as TableSchema).IsLogicalExclusion)
            {
                TableSchema tbSch = this.TableDefinition as TableSchema;
                this.Conditions.Add(StorageFactory.JoinConstraint.CreateAnd(
                    StorageFactory.Column.Create(tbSch.LogicalExclusionColumn, this, this.Parent), this))
                    .Equal(false);
            }

            return retCondition;
        }

        public StoredJoinConstraint And(string tableName, ColumnSchema columnFrom)
        {
            return this.Conditions.Add(StorageFactory.JoinConstraint.CreateAnd(
                   StorageFactory.Column.Create(columnFrom, tableName, this.Parent), this));
        }

        public StoredJoinConstraint Or(string tableName, ColumnSchema columnFrom)
        {
            return this.Conditions.Add(StorageFactory.JoinConstraint.CreateOr(
                   StorageFactory.Column.Create(columnFrom, tableName, this.Parent), this));
        }

        public StoredJoinConstraint On(string tableName, string columnName)
        {
            StoredJoinConstraint retCondition = this.Conditions.Add(StorageFactory.JoinConstraint.CreateWhere(
                   StorageFactory.Column.Create(columnName, tableName, this.Parent), this));

            if (this.Type == StoredTypes.Schema && (this.TableDefinition as TableSchema).IsLogicalExclusion)
            {
                TableSchema tbSch = this.TableDefinition as TableSchema;
                this.Conditions.Add(StorageFactory.JoinConstraint.CreateAnd(
                    StorageFactory.Column.Create(tbSch.LogicalExclusionColumn, this.Parent), this))
                    .Equal(false);
            }

            return retCondition;
        }

        public StoredJoinConstraint And(string tableName, string columnName)
        {
            return this.Conditions.Add(StorageFactory.JoinConstraint.CreateAnd(
                   StorageFactory.Column.Create(columnName, tableName, this.Parent), this));
        }

        public StoredJoinConstraint Or(string tableName, string columnName)
        {
            return this.Conditions.Add(StorageFactory.JoinConstraint.CreateOr(
                   StorageFactory.Column.Create(columnName, tableName, this.Parent), this));
        }

        #endregion

        #endregion

        public enum JoinTypes
        {
            Inner,
            Left,
            Right,
            Full,
            LeftOuter,
            RightOuter
        }

    }

}
