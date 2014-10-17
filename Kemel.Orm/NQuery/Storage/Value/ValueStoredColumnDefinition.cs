using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.NQuery.Storage.Column;

namespace Kemel.Orm.NQuery.Storage.Value
{

    public class ValueStoredColumnDefinition : StoredValue.IValueDefinition
    {

        public ValueStoredColumnDefinition(StoredColumn column)
        {
            this.sclValue = column;
        }

        private StoredColumn sclValue;
        public StoredColumn Value
        {
            get { return sclValue; }
            set { sclValue = value; }
        }

        public T GetTypedValue<T>()
        {
            if ((object.ReferenceEquals(typeof(T), typeof(StoredColumn))))
            {
                return (T)(object)this.Value;
            }
            else
            {
                throw new InvalidCastException("Somente é permitido a conversão para o tipo StoredColumn");
            }
        }

        public T GetTypedValue<T>(int index)
        {
            if ((object.ReferenceEquals(typeof(T), typeof(StoredColumn))))
            {
                return (T)(object)this.Value;
            }
            else
            {
                throw new InvalidCastException("Somente é permitido a conversão para o tipo StoredColumn");
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
