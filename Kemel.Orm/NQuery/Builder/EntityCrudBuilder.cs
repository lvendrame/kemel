using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.Providers;
using Kemel.Orm.Entity;
using Kemel.Orm.NQuery.Storage;
using Kemel.Orm.Schema;
using Kemel.Orm.NQuery.Storage.Table;

namespace Kemel.Orm.NQuery.Builder
{

    public enum BuildParamFlag
    {
        BlockIdentity = 0x1,
        BlockPrimarykey = 0x2,
        BlockNull = 0x4,
        None = 0x8
    }

    public enum CrudType
    {
        Insert,
        Update,
        Delete,
        DeleteById,
        Select,
        SelectById,
        SelectIdentity,
        Procedure
    }

    public abstract class EntityCrudBuilder
    {

        private Provider objParent = null;
        public Provider Parent
        {
            get { return this.objParent; }
            protected set { this.objParent = value; }
        }


        public EntityCrudBuilder(Provider parent)
        {
            this.Parent = parent;
        }

        public abstract string GetSequenceNextValue(ColumnSchema column);
        public abstract string GetSequenceCurrentValue(ColumnSchema column);

        public virtual Query GetInsert<TEtt>(TEtt ettEntity) where TEtt : EntityBase
        {
            Query query = StorageFactory.NQuery.Insert(this.Parent).Into<TEtt>();
            TableSchema schema = (TableSchema)query.IntoTable.TableDefinition;

            foreach (ColumnSchema column in schema.Columns)
            {
                if (column.IgnoreColumn)
                {
                    continue;
                }

                if (!column.IsIdentity)
                {
                    if (!string.IsNullOrEmpty(column.SequenceName))
                    {
                        query.SetValue(column).Equal(
                            new StringConstant(GetSequenceNextValue(column))
                            );
                    }
                    else
                    {
                        object value = column.GetValue(ettEntity);
                        if (value == null && !column.AllowNull)
                        {
                            if (column.IsLogicalExclusionColumn)
                            {
                                column.SetValue(ettEntity, 0);
                                value = 0;
                            }
                            else
                            {
                                throw new OrmException(string.Format(Messages.PropertyDoesNotNull, column.Name));
                            }
                        }

                        query.SetValue(column).Equal(value);
                    }
                }
            }
            this.Parent.ExecuteOnQueryBuilder(this, query, ettEntity, CrudType.Insert);
            return query;
        }

        public virtual Query GetUpdate<TEtt>(TEtt ettEntity) where TEtt : EntityBase
        {
            return this.GetUpdate<TEtt>(ettEntity, true);
        }

        public virtual Query GetUpdate<TEtt>(TEtt ettEntity, bool throwWithoutPK) where TEtt : EntityBase
        {
            Query query = StorageFactory.NQuery.Update(this.Parent).Into<TEtt>();
            TableSchema schema = (TableSchema)query.IntoTable.TableDefinition;

            List<ColumnSchema> lstColumnPk = new List<ColumnSchema>();

            bool hasPK = false;
            foreach (ColumnSchema column in schema.Columns)
            {
                if (column.IgnoreColumn)
                {
                    continue;
                }
                if (column.IsPrimaryKey)
                {
                    hasPK = true;
                    object pkValue = column.GetValue(ettEntity);

                    if (pkValue == null)
                    {
                        if (throwWithoutPK)
                        {
                            throw new OrmException(string.Format(Kemel.Orm.Messages.PropertyDoesNotNull, column.Name));
                        }
                    }
                    else
                    {
                        query.Where(column).Equal(pkValue);
                    }
                }
                else
                {
                    object value = column.GetValue(ettEntity);

                    if (value == null && !column.AllowNull)
                    {
                        if (column.IsLogicalExclusionColumn)
                            value = false;
                        else
                            throw new OrmException(string.Format(Kemel.Orm.Messages.PropertyDoesNotNull, column.Name));
                    }

                    //if (value != null || !throwWithoutPK)
                    query.SetValue(column).Equal(value);
                }
            }

            if (!hasPK)
            {
                throw new OrmException(Kemel.Orm.Messages.PrimaryKeyIsNull);
            }

            this.Parent.ExecuteOnQueryBuilder(this, query, ettEntity, CrudType.Update);
            return query;
        }

        public virtual Query GetDelete<TEtt>(TEtt ettEntity) where TEtt : EntityBase
        {
            Query query = StorageFactory.NQuery.Delete(this.Parent).Into<TEtt>();
            TableSchema schema = (TableSchema)query.IntoTable.TableDefinition;

            bool addWhere = true;
            foreach (ColumnSchema column in schema.Columns)
            {
                if (column.IgnoreColumn)
                {
                    continue;
                }

                object value = column.GetValue(ettEntity);

                if (value != null)
                {
                    if (addWhere)
                    {
                        query.Where(column).Equal(value);
                        addWhere = false;
                    }
                    else
                    {
                        query.And(column).Equal(value);
                    }
                }
            }

            this.Parent.ExecuteOnQueryBuilder(this, query, ettEntity, CrudType.Delete);
            return query;
        }

        public virtual Query GetDeleteById<TEtt>(TEtt ettEntity) where TEtt : EntityBase
        {
            Query query = StorageFactory.NQuery.Delete(this.Parent).Into<TEtt>();
            TableSchema schema = (TableSchema)query.IntoTable.TableDefinition;

            bool pkIsNull = true;
            foreach (ColumnSchema column in schema.Columns)
            {
                if (column.IgnoreColumn)
                {
                    continue;
                }

                if (column.IsPrimaryKey)
                {
                    object value = column.GetValue(ettEntity);

                    if (value != null)
                    {
                        query.Where(column).Equal(value);
                        pkIsNull = false;
                    }
                }
            }

            if (pkIsNull)
            {
                throw new OrmException(Kemel.Orm.Messages.PrimaryKeyIsNull);
            }

            this.Parent.ExecuteOnQueryBuilder(this, query, ettEntity, CrudType.DeleteById);
            return query;
        }

        public virtual Query GetSelect<TEtt>() where TEtt : EntityBase
        {
            Query qry = StorageFactory.NQuery.Select(this.Parent).From<TEtt>().AllColumns();
            this.Parent.ExecuteOnQueryBuilder(this, qry, null, CrudType.Select);
            return qry;
        }

        public virtual Query GetSelect<TEtt>(TEtt ettEntity) where TEtt : EntityBase
        {
            Query query = StorageFactory.NQuery.Select(this.Parent).From<TEtt>().AllColumns();
            TableSchema schema = (TableSchema)query.Tables[0].TableDefinition;

            bool addWhere = true;
            foreach (ColumnSchema column in schema.Columns)
            {
                if (column.IgnoreColumn)
                {
                    continue;
                }

                object value = column.GetValue(ettEntity);
                if (value != null)
                {
                    if (addWhere)
                    {
                        query.Where(column).Equal(value);
                        addWhere = false;
                    }
                    else
                    {
                        query.And(column).Equal(value);
                    }
                }
            }

            this.Parent.ExecuteOnQueryBuilder(this, query, ettEntity, CrudType.Select);
            return query;
        }

        public virtual Query GetSelectById<TEtt>(TEtt pett_Entity) where TEtt : EntityBase
        {
            Query query = StorageFactory.NQuery.Select(this.Parent).From<TEtt>().AllColumns();
            TableSchema schema = (TableSchema)query.Tables[0].TableDefinition;

            bool pkIsNull = true;

            foreach (ColumnSchema column in schema.Columns)
            {
                if (column.IgnoreColumn)
                {
                    continue;
                }

                if (column.IsPrimaryKey)
                {
                    object value = column.GetValue(pett_Entity);
                    if (value != null)
                    {
                        query.Where(column).Equal(value);
                        pkIsNull = false;
                    }
                }
            }

            if (pkIsNull)
            {
                throw new OrmException(Kemel.Orm.Messages.PrimaryKeyIsNull);
            }

            this.Parent.ExecuteOnQueryBuilder(this, query, pett_Entity, CrudType.SelectById);
            return query;
        }

        public virtual Query GetSelectWithColumnAlias<TEtt>() where TEtt : EntityBase
        {
            StoredTable tableQuery = StorageFactory.NQuery.Select(this.Parent).From<TEtt>();
            TableSchema table = (TableSchema)tableQuery.TableDefinition;
            foreach (ColumnSchema column in table.Columns)
            {
                if (column.IgnoreColumn)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(column.Alias))
                {
                    tableQuery.Column(column);
                }
                else
                {
                    tableQuery.Column(column).As(column.Alias);
                }
            }

            this.Parent.ExecuteOnQueryBuilder(this, tableQuery.Parent, null, CrudType.Select);
            return tableQuery.Parent;
        }

        public virtual Query GetSelectWithColumnAlias<TEtt>(TEtt ettEntity) where TEtt : EntityBase
        {
            StoredTable stTable = StorageFactory.NQuery.Select(this.Parent).From<TEtt>();
            TableSchema table = (TableSchema)stTable.TableDefinition;
            Query query = stTable.Parent;

            bool addWhere = true;
            foreach (ColumnSchema column in table.Columns)
            {
                if (column.IgnoreColumn)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(column.Alias))
                {
                    stTable.Column(column);
                }
                else
                {
                    stTable.Column(column).As(column.Alias);
                }

                object value = column.GetValue(ettEntity);
                if (value != null)
                {
                    if (addWhere)
                    {
                        query.Where(column).Equal(value);
                        addWhere = false;
                    }
                    else
                    {
                        query.And(column).Equal(value);
                    }
                }
            }

            this.Parent.ExecuteOnQueryBuilder(this, query, ettEntity, CrudType.Select);
            return query;
        }

        public virtual Query GetSelectByIdWithColumnAlias<TEtt>(TEtt ettEntity) where TEtt : EntityBase
        {
            StoredTable stTable = StorageFactory.NQuery.Select(this.Parent).From<TEtt>();
            TableSchema table = (TableSchema)stTable.TableDefinition;
            Query query = stTable.Parent;

            bool pkIsNull = true;
            foreach (ColumnSchema column in table.Columns)
            {
                if (column.IgnoreColumn)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(column.Alias))
                {
                    stTable.Column(column);
                }
                else
                {
                    stTable.Column(column).As(column.Alias);
                }

                if (column.IsPrimaryKey)
                {
                    object value = column.GetValue(ettEntity);
                    if (value != null)
                    {
                        query.Where(column).Equal(value);
                        pkIsNull = false;
                    }
                }
            }

            if (pkIsNull)
            {
                throw new OrmException(Kemel.Orm.Messages.PrimaryKeyIsNull);
            }

            this.Parent.ExecuteOnQueryBuilder(this, query, ettEntity, CrudType.SelectById);
            return query;
        }

        public virtual Query GetProcedure<TEtt>(TEtt ettEntity) where TEtt : EntityBase
        {
            Query query = StorageFactory.NQuery.Procedure<TEtt>();
            TableSchema schema = (TableSchema)query.IntoTable.TableDefinition;

            foreach (ColumnSchema column in schema.Columns)
            {
                if (column.IgnoreColumn)
                {
                    continue;
                }

                if (column.ParamDirection == ParameterDirection.Input || column.ParamDirection == ParameterDirection.InputOutput)
                {
                    query.SetValue(column).Equal(column.GetValue(ettEntity));
                }
                else
                {
                    query.SetValue(column).Direction(column.ParamDirection);
                }
            }

            this.Parent.ExecuteOnQueryBuilder(this, query, ettEntity, CrudType.Procedure);
            return query;
        }

        public virtual Query GetSelectIdentity<TEtt>() where TEtt : EntityBase
        {
            Query query = StorageFactory.NQuery.Select(this.Parent);
            StoredTable stTable = query.From<TEtt>();
            TableSchema table = (TableSchema)stTable.TableDefinition;
            foreach (ColumnSchema column in table.IdentityColumns)
            {
                stTable.Max(column).As(column.Name);
            }

            this.Parent.ExecuteOnQueryBuilder(this, query, null, CrudType.SelectIdentity);
            return query;
        }
    }

}
