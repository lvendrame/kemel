using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.Schema;
using Kemel.Orm.NQuery.Storage.Column;
using Kemel.Orm.NQuery.Storage.Function;
using Kemel.Orm.Base;

namespace Kemel.Orm.NQuery.Storage.Table
{

    public class StoredTableCollection : List<StoredTable>
    {

        public new StoredTable Add(StoredTable item)
        {
            base.Add(item);
            return item;
        }

        public StoredTable Find(string name)
        {
            name = name.ToUpper();

            foreach (StoredTable table in this)
            {
                if ((table.Compare(name)))
                {
                    return table;
                }
            }
            return null;
        }

        public StoredTable Find(ITableDefinition tableDef)
        {
            return this.Find(tableDef.Name);
        }

    }

    public class StoredTable : IGetQuery
    {

        public StoredTable(ITableDefinition tableDef, StoredTypes type, Query parent)
        {
            this.objParent = parent;
            this.itdTableDefinition = tableDef;
            this.estType = type;
        }

        #region "Properties"

        private ITableDefinition itdTableDefinition;
        public ITableDefinition TableDefinition
        {
            get { return itdTableDefinition; }
        }

        private string objOwner = null;
        public string Owner
        {
            get { return this.objOwner; }
            set { this.objOwner = value; }
        }

        private string objAlias = null;
        public string Alias
        {
            get { return this.objAlias; }
            set { this.objAlias = value; }
        }

        private bool blnWithNolock = false;
        public bool NoLock
        {
            get { return this.blnWithNolock; }
            set { this.blnWithNolock = value; }
        }

        private StoredColumnCollectoin lstColumns = null;
        public StoredColumnCollectoin StoredColumns
        {
            get
            {
                if (this.lstColumns == null)
                {
                    this.lstColumns = new StoredColumnCollectoin();
                }
                return this.lstColumns;
            }
        }

        private Query objParent = null;
        public Query Parent
        {
            get { return this.objParent; }
            set { this.objParent = value; }
        }

        public string ColumnPrefix
        {
            get
            {
                if (string.IsNullOrEmpty(this.Alias))
                {
                    return this.TableDefinition.Name;
                }
                else
                {
                    return this.Alias;
                }
            }
        }

        public bool HasAlias
        {
            get { return !string.IsNullOrEmpty(this.Alias); }
        }

        #endregion

        #region "Methods"

        public StoredColumn FindOrAddColumn(IColumnDefinition column)
        {
            StoredColumn col = this.FindColumn(column.Name);
            if ((col.Type == StoredColumn.StoredTypes.Schema))
            {
                return this.Column((ColumnSchema)column);
            }
            else
            {
                return this.Column(column.Name);
            }
        }

        public StoredColumn FindColumn(string columnName)
        {
            return this.StoredColumns.Find(columnName);
        }

        //Public Function FindColumn(ByVal column As IColumnDefinition) As StoredColumn
        //    Dim tq As StoredTable = Me.TableDefinition.GetColumn(column.Name)

        //    If tq Is Nothing Then
        //        Throw New OrmException(Messages.TableDoesNotExistInQuery)
        //    End If

        //    Return tq.FindOrAddColumn(column)
        //End Function

        public Query AllColumns()
        {
            if ((this.Type == StoredTypes.Schema))
            {
                TableSchema tbSchema = (TableSchema)this.TableDefinition;
                foreach (ColumnSchema colSchema in tbSchema.Columns)
                {
                    if ((!colSchema.IgnoreColumn))
                    {
                        this.Column(colSchema);
                    }
                }
            }
            else
            {
                this.StoredColumns.Add(StorageFactory.Column.Create("*", this, this.Parent));
            }
            return this.Parent;
        }

        public Query Columns(params string[] columnsNames)
        {
            foreach (string column in columnsNames)
            {
                this.Column(column);
            }
            return this.Parent;
        }

        public StoredColumn Column(string columnName)
        {
            return this.StoredColumns.Add(StorageFactory.Column.Create(columnName, this, this.Parent));
        }

        public StoredColumn Column(Schema.ColumnSchema columnSchema)
        {
            return this.StoredColumns.Add(StorageFactory.Column.Create(columnSchema, this, this.Parent));
        }

        public StoredColumn Column(StoredFunction stFunction)
        {
            StoredColumn retColumn = StorageFactory.Column.Create(stFunction, this.Parent);
            return this.StoredColumns.Add(retColumn);
        }

        public StoredColumn Max(ColumnSchema column)
        {
            return this.Column(StorageFactory.SFunction.Max.Create(this.Parent).SetValue(column));
        }

        public StoredTable WithNoLock()
        {
            this.NoLock = true;
            return this;
        }

        public StoredTable As(string p_alias)
        {
            this.Alias = p_alias;
            return this;
        }

        public StoredTable SetOwner(string owner)
        {
            this.objOwner = owner;
            return this;
        }

        public bool EqualsTableNameOrAlias(string tableName)
        {
            tableName = tableName.ToUpper();
            if (this.EqualsTableName(tableName))
            {
                return true;
            }
            else if (this.HasAlias)
            {
                return this.Alias.ToUpper().Equals(tableName);
            }
            else
            {
                return false;
            }
        }

        public bool EqualsTableName(string tableName)
        {
            tableName = tableName.ToUpper();
            return this.TableDefinition.Name.ToUpper().Equals(tableName);
        }

        public Query EndTable()
        {
            return this.Parent;
        }

        #endregion

        public Join.StoredJoin AsJoin()
        {
            return this as Join.StoredJoin;
        }

        private StoredTypes estType;
        public StoredTypes Type
        {
            get { return estType; }
        }

        public enum StoredTypes
        {
            Schema,
            Name,
            SubQuery,
            StoredFunction
        }

        public bool Compare(string name)
        {
            if ((!string.IsNullOrEmpty(itdTableDefinition.Name)))
            {
                if ((itdTableDefinition.Name.ToUpper().Equals(name)))
                {
                    return true;
                }
            }
            else if ((!string.IsNullOrEmpty(itdTableDefinition.Alias)))
            {
                if ((itdTableDefinition.Alias.ToUpper().Equals(name)))
                {
                    return true;
                }
            }

            if ((!string.IsNullOrEmpty(this.Alias)))
            {
                if ((this.Alias.ToUpper().Equals(name)))
                {
                    return true;
                }
            }

            return false;
        }

        public Query GetQuery()
        {
            return this.Parent;
        }

        public void SetQuery(Query query)
        {
            this.Parent = query;
        }

    }

}
