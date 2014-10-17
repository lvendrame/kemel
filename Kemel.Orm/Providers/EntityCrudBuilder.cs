using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Data;
using System.Reflection;
using Kemel.Orm.Schema;
using System.Data;
using Kemel.Orm.Entity;
using Kemel.Orm.Constants;
using Kemel.Orm.QueryDef;

namespace Kemel.Orm.Providers
{
    public enum BuildParamFlag
    {
        BlockIdentity = 0x01,
        BlockPrimarykey = 0x02,
        BlockNull = 0x04,
        None = 0x08
    }

    public enum CrudType
    {
        Insert,
        Update,
        Delete,
        DeleteById,
        Select,
        SelectById
    }

    public abstract class EntityCrudBuilder
    {
        public Provider Parent { get; protected set; }

        public EntityCrudBuilder(Provider parent)
        {
            this.Parent = parent;
        }

        public virtual Query GetInsert<TEtt>(TEtt ettEntity)
            where TEtt : EntityBase
        {
            Query query = Query.Insert(this.Parent).Into<TEtt>();
            TableSchema schema = query.IntoTable.TableSchema;

            foreach (ColumnSchema column in schema.Columns)
            {
                if (column.IgnoreColumn)
                    continue;

                if (!column.IsIdentity)
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

                    query.Set(column).Equal(value);
                }
            }

            return query;
        }

        public virtual Query GetUpdate<TEtt>(TEtt ettEntity)
            where TEtt : EntityBase
        {
            return this.GetUpdate<TEtt>(ettEntity, true);
        }

        public virtual Query GetUpdate<TEtt>(TEtt ettEntity, bool throwWithoutPK)
            where TEtt : EntityBase
        {
            Query query = Query.Update(this.Parent).Into<TEtt>();
            TableSchema schema = query.IntoTable.TableSchema;

            ColumnSchema columnPk = null;
            foreach (ColumnSchema column in schema.Columns)
            {
                if (column.IgnoreColumn)
                    continue;

                if (column.IsPrimaryKey)
                {
                    columnPk = column;
                }
                else
                {
                    object value = column.GetValue(ettEntity);

                    if(value == null && !column.AllowNull)
                        throw new OrmException(string.Format(Kemel.Orm.Messages.PropertyDoesNotNull, column.Name));

                    //if (value != null || !throwWithoutPK)
                    query.Set(column).Equal(value);
                }
            }

            object pkValue = columnPk.GetValue(ettEntity);

            if (pkValue == null)
            {
                if (throwWithoutPK)
                    throw new OrmException(string.Format(Kemel.Orm.Messages.PropertyDoesNotNull, columnPk.Name));
            }
            else
            {
                query.Where(columnPk).Equal(pkValue);
            }

            return query;
        }

        public virtual Query GetDelete<TEtt>(TEtt ettEntity)
            where TEtt : EntityBase
        {
            Query query = Query.Delete(this.Parent).Into<TEtt>();
            TableSchema schema = query.IntoTable.TableSchema;

            bool addWhere = true;
            foreach (ColumnSchema column in schema.Columns)
            {
                if (column.IgnoreColumn)
                    continue;

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

            return query;
        }

        public virtual Query GetDeleteById<TEtt>(TEtt ettEntity)
            where TEtt : EntityBase
        {
            Query query = Query.Delete(this.Parent).Into<TEtt>();
            TableSchema schema = query.IntoTable.TableSchema;

            bool pkIsNull = true;
            foreach (ColumnSchema column in schema.Columns)
            {
                if (column.IgnoreColumn)
                    continue;

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
                throw new OrmException(Kemel.Orm.Messages.PrimaryKeyIsNull);

            return query;
        }

        public virtual Query GetSelect<TEtt>()
            where TEtt : EntityBase
        {
            return Query.Select(this.Parent).From<TEtt>().AllColumns();
        }

        public virtual Query GetSelect<TEtt>(TEtt ettEntity)
            where TEtt : EntityBase
        {
            Query query = Query.Select(this.Parent).From<TEtt>().AllColumns();
            TableSchema schema = query.Tables[0].TableSchema;

            bool addWhere = true;
            foreach (ColumnSchema column in schema.Columns)
            {
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

            return query;
        }

        public virtual Query GetSelectById<TEtt>(TEtt pett_Entity)
            where TEtt : EntityBase
        {
            Query query = Query.Select(this.Parent).From<TEtt>().AllColumns();
            TableSchema schema = query.Tables[0].TableSchema;

            bool pkIsNull = true;

            foreach (ColumnSchema column in schema.Columns)
            {
                if (column.IgnoreColumn)
                    continue;

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
                throw new OrmException(Kemel.Orm.Messages.PrimaryKeyIsNull);

            return query;
        }

        public virtual Query GetSelectWithColumnAlias<TEtt>()
            where TEtt : EntityBase
        {
            TableQuery tableQuery = Query.Select(this.Parent).From<TEtt>();

            foreach (ColumnSchema column in tableQuery.TableSchema.Columns)
            {
                if (column.IgnoreColumn)
                    continue;

                if (string.IsNullOrEmpty(column.Alias))
                    tableQuery.Column(column);
                else
                    tableQuery.Column(column).As(column.Alias);
            }

            return tableQuery.Parent;
        }

        public virtual Query GetSelectWithColumnAlias<TEtt>(TEtt ettEntity)
            where TEtt : EntityBase
        {
            TableQuery tableQuery = Query.Select(this.Parent).From<TEtt>();
            Query query = tableQuery.Parent;

            bool addWhere = true;
            foreach (ColumnSchema column in tableQuery.TableSchema.Columns)
            {
                if (column.IgnoreColumn)
                    continue;

                if (string.IsNullOrEmpty(column.Alias))
                    tableQuery.Column(column);
                else
                    tableQuery.Column(column).As(column.Alias);

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

            return query;
        }

        public virtual Query GetSelectByIdWithColumnAlias<TEtt>(TEtt ettEntity)
            where TEtt : EntityBase
        {
            TableQuery tableQuery = Query.Select(this.Parent).From<TEtt>();
            Query query = tableQuery.Parent;

            bool pkIsNull = true;
            foreach (ColumnSchema column in tableQuery.TableSchema.Columns)
            {
                if (column.IgnoreColumn)
                    continue;

                if (string.IsNullOrEmpty(column.Alias))
                    tableQuery.Column(column);
                else
                    tableQuery.Column(column).As(column.Alias);

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
                throw new OrmException(Kemel.Orm.Messages.PrimaryKeyIsNull);

            return query;
        }

        public virtual Query GetProcedure<TEtt>(TEtt ettEntity)
            where TEtt : EntityBase
        {
            Query query = Query.Procedure<TEtt>();
            TableSchema schema = query.IntoTable.TableSchema;

            foreach (ColumnSchema column in schema.Columns)
            {
                if (column.IgnoreColumn)
                    continue;

                if (column.ParamDirection == ParameterDirection.Input || column.ParamDirection == ParameterDirection.InputOutput)
                    query.Set(column).Equal(column.GetValue(ettEntity));
                else
                    query.Set(column).Direction(column.ParamDirection);
            }

            return query;
        }

        public virtual Query GetSelectIdentity<TEtt>()
            where TEtt : EntityBase
        {
            Query query = Query.Select(this.Parent);
            TableQuery tableQuery = query.From<TEtt>();
            foreach (ColumnSchema column in tableQuery.TableSchema.IdentityColumns)
            {
                tableQuery.Max(column).As(column.Name);
            }

            return query;
        }
    }
}
