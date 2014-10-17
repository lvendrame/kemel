using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Kemel.Orm.NQuery.Storage.Value
{
    public class ValueSubQueryDefinition : StoredValue.IValueDefinition
    {
        public ValueSubQueryDefinition(Query subQuery)
        {
            this.qryValue = subQuery;
        }

        private Query qryValue;
        public Query Value
        {
            get { return qryValue; }
            set { qryValue = value; }
        }

        public T GetTypedValue<T>()
        {
            if ((object.ReferenceEquals(typeof(T), typeof(Query))))
            {
                return (T)(object)this.Value;
            }
            else
            {
                throw new InvalidCastException("Somente é permitido a conversão para o tipo Query");
            }
        }

        public T GetTypedValue<T>(int index)
        {
            if ((object.ReferenceEquals(typeof(T), typeof(Query))))
            {
                return (T)(object)this.Value;
            }
            else
            {
                throw new InvalidCastException("Somente é permitido a conversão para o tipo Query");
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
