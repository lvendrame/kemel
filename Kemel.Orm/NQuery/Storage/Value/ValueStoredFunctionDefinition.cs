using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.NQuery.Storage.Function;

namespace Kemel.Orm.NQuery.Storage.Value
{
    public class ValueStoredFunctionDefinition : StoredValue.IValueDefinition
    {
        public ValueStoredFunctionDefinition(StoredFunction stFunction)
        {
            this.sclValue = stFunction;
        }

        private StoredFunction sclValue;
        public StoredFunction Value
        {
            get { return sclValue; }
            set { sclValue = value; }
        }

        public T GetTypedValue<T>()
        {
            if ((object.ReferenceEquals(typeof(T), typeof(StoredFunction))))
            {
                return (T)(object)this.Value;
            }
            else
            {
                throw new InvalidCastException("Somente é permitido a conversão para o tipo StoredFunction");
            }
        }

        public T GetTypedValue<T>(int index)
        {
            if ((object.ReferenceEquals(typeof(T), typeof(StoredFunction))))
            {
                return (T)(object)this.Value;
            }
            else
            {
                throw new InvalidCastException("Somente é permitido a conversão para o tipo StoredFunction");
            }
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
