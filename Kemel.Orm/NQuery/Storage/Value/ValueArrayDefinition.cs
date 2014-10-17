using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Kemel.Orm.NQuery.Storage.Value
{
    public class ValueArrayDefinition : StoredValue.IValueDefinition
    {
        public ValueArrayDefinition(List<object> values)
        {
            this.lstValues = values;
        }

        public ValueArrayDefinition(object[] values)
        {
            this.lstValues = new List<object>();
            this.lstValues.AddRange(values);
        }

        public ValueArrayDefinition(IEnumerable values)
        {
            this.lstValues = new List<object>();
            foreach (object obj in values)
            {
                this.lstValues.Add(obj);
            }
        }

        private List<object> lstValues;
        public List<object> Values
        {
            get { return lstValues; }
            set { lstValues = value; }
        }

        public T GetTypedValue<T>()
        {
            return (T)this.Values[0];
        }

        public T GetTypedValue<T>(int index)
        {
            return (T)this.Values[index];
        }

        public object GetValue()
        {
            return this.Values[0];
        }

        public object GetValue(int index)
        {
            return this.Values[index];
        }
    }
}
