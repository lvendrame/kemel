using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Kemel.Orm.NQuery.Storage.Value
{
    public class ValueObjectDefinition : StoredValue.IValueDefinition
    {
        public ValueObjectDefinition(object value)
        {
            this.objValue = value;
        }

        private object objValue;
        public object Value
        {
            get { return objValue; }
            set { objValue = value; }
        }

        public T GetTypedValue<T>()
        {
            return (T)this.Value;
        }

        public T GetTypedValue<T>(int index)
        {
            return (T)this.Value;
        }

        public object GetValue()
        {
            return this.Value;
        }

        public object GetValue(int index)
        {
            return this.Value;
        }
    }
}
