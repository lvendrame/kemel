using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Constants;

namespace Kemel.Orm.Schema
{
    public class ColumnSchemaCollection: List<ColumnSchema>
    {
        public ColumnSchema AddRet(ColumnSchema columnSchema)
        {
            this.Add(columnSchema);
            return columnSchema;
        }

        public ColumnSchema this[string columnName]
        {
            get
            {
                return this[this.FindIndexByNameOrThrow(columnName)];
            }
            set
            {
                this[this.FindIndexByNameOrThrow(columnName)] = value;
            }

        }

        public int FindIndexByNameOrThrow(string columnName)
        {
            int index = this.FindIndexByName(columnName);
            if (index == -1)
                throw new OrmException(Messages.DontHaveColumnInColumnCollection);

            return index;
        }

        public int FindIndexByName(string columnName)
        {
            columnName = ColumnSchema.ToColumnPatternName(columnName);
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Name.ToUpper().Equals(columnName))
                    return i;
            }
            return -1;
        }

        public ColumnSchema FindColumnByName(string columnName)
        {
            columnName = ColumnSchema.ToColumnPatternName(columnName);
            foreach (ColumnSchema column in this)
            {
                if (column.Name.Equals(columnName))
                    return column;
            }
            return null;
        }

        public bool HasColumn(string columnName)
        {
            columnName = ColumnSchema.ToColumnPatternName(columnName);
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Name.Equals(columnName))
                    return true;
            }
            return false;
        }

        public string GetColumnsList(bool useColumnAlias)
        {
            StringBuilder stbColumnList = new StringBuilder();
            #region useColumnAlias
            if (useColumnAlias)
            {
                foreach (ColumnSchema column in this)
                {
                    if (column.IgnoreColumn)
                        continue;

                    if (column.Alias.Length == 0)
                    {
                        stbColumnList.Append(column.Name + Punctuation.COMMA);
                    }
                    else
                    {
                        stbColumnList.Append(column.Name + Punctuation.WHITE_SPACE + column.Alias + Punctuation.COMMA);
                    }
                }
            }
            #endregion
            #region !useColumnAlias
            else
            {
                foreach (ColumnSchema column in this)
                {
                    if (column.IgnoreColumn)
                        continue;

                    stbColumnList.Append(column.Name + Punctuation.COMMA);
                }
            }
            #endregion
            return stbColumnList.ToString(0, stbColumnList.Length - Punctuation.COMMA.Length);
        }

        public string GetColumnsList()
        {
            return GetColumnsList(false);
        }
    }
}
