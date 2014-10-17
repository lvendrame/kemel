using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Kemel.Orm.NQuery.Storage.Value
{
    public class ValueIntervalDefinition : StoredValue.IValueDefinition
    {
        public ValueIntervalDefinition(object startValue, object finalValue)
        {
            this.objStartValue = startValue;
            this.objFinalValue = finalValue;
        }

        private object objStartValue;
        public object StartValue
        {
            get { return objStartValue; }
            set { objStartValue = value; }
        }

        private object objFinalValue;
        public object FinalValue
        {
            get { return objFinalValue; }
            set { objFinalValue = value; }
        }

        public T GetTypedValue<T>()
        {
            return (T)this.StartValue;
        }

        public T GetTypedValue<T>(int index)
        {
            if ((index == 0))
            {
                return (T)this.StartValue;
            }
            else if ((index == 1))
            {
                return (T)this.FinalValue;
            }
            else
            {
                throw new IndexOutOfRangeException("Existem apenas 2 valores armazenados");
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
