using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.NQuery.Storage.Function;

namespace Kemel.Orm.NQuery.Storage.Value
{

    public class ValueStoredFunctionIntervalDefinition : StoredValue.IValueDefinition
    {

        public ValueStoredFunctionIntervalDefinition(StoredFunction startFunction, StoredFunction endFunction)
        {
            this.sclStartValue = startFunction;
            this.sclFinalValue = endFunction;
        }

        private StoredFunction sclStartValue;
        public StoredFunction StartValue
        {
            get { return sclStartValue; }
            set { sclStartValue = value; }
        }

        private StoredFunction sclFinalValue;
        public StoredFunction FinalValue
        {
            get { return sclFinalValue; }
            set { sclFinalValue = value; }
        }

        public T GetTypedValue<T>()
        {
            if ((object.ReferenceEquals(typeof(T), typeof(StoredFunction))))
            {
                return (T)(object)this.StartValue;
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
                if ((index == 0))
                {
                    return (T)(object)this.StartValue;
                }
                else if ((index == 1))
                {
                    return (T)(object)this.FinalValue;
                }
                else
                {
                    throw new IndexOutOfRangeException("Existem apenas 2 valores armazenados");
                }
            }
            else
            {
                throw new InvalidCastException("Somente é permitido a conversão para o tipo StoredFunction");
            }
        }

        public object GetValue()
        {
            return this.StartValue;
        }

        public object GetValue(int index)
        {
            if ((index == 0))
            {
                return this.StartValue;
            }
            else if ((index == 1))
            {
                return this.FinalValue;
            }
            else
            {
                throw new IndexOutOfRangeException("Existem apenas 2 valores armazenados");
            }
        }

    }

}
