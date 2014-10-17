using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Kemel.Orm.NQuery.Storage.Value
{
    public class StoredValueCollection : List<StoredValue>
    {
        public new StoredValue Add(StoredValue item)
        {
            base.Add(item);
            return item;
        }
    }

    public class StoredValue : IGenParameter
    {
        public StoredValue(IValueDefinition value, StoredTypes type)
        {
            this.ivdValue = value;
            this.estType = type;
        }

        private IValueDefinition ivdValue;
        public IValueDefinition Value
        {
            get { return ivdValue; }
            set { ivdValue = value; }
        }

        private StoredTypes estType;
        public StoredTypes Type
        {
            get { return estType; }
            set { estType = value; }
        }

        private string strParameterName;
        public string ParameterName
        {
            get { return strParameterName; }
            set { strParameterName = value; }
        }

        public interface IValueDefinition
        {
            object GetValue();
            T GetTypedValue<T>();
            object GetValue(int index);
            T GetTypedValue<T>(int index);
        }

        public enum StoredTypes
        {
            StoredColumn,
            ObjectValue,
            SubQuery,
            Array,
            Interval,
            IntervalStoredColumns,
            StoredFunction,
            IntervalStoredFunction
        }
    }
}
