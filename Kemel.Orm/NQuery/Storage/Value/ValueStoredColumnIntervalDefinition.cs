using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.NQuery.Storage.Column;

namespace Kemel.Orm.NQuery.Storage.Value
{

    public class ValueStoredColumnIntervalDefinition : StoredValue.IValueDefinition
    {

        public ValueStoredColumnIntervalDefinition(StoredColumn startColumn, StoredColumn endColumn)
        {
            this.sclStartValue = startColumn;
            this.sclFinalValue = endColumn;
        }

        private StoredColumn sclStartValue;
        public StoredColumn StartValue
        {
            get { return sclStartValue; }
            set { sclStartValue = value; }
        }

        private StoredColumn sclFinalValue;
        public StoredColumn FinalValue
        {
            get { return sclFinalValue; }
            set { sclFinalValue = value; }
        }

        public T GetTypedValue<T>()
        {
            if ((object.ReferenceEquals(typeof(T), typeof(StoredColumn))))
            {
                return (T)(object)this.StartValue;
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
                throw new InvalidCastException("Somente é permitido a conversão para o tipo StoredColumn");
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
