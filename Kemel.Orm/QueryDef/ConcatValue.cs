using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Schema;

namespace Kemel.Orm.QueryDef
{
    public class ConcatValueCollection: List<ConcatValue>
    {
        new public ConcatValue Add(ConcatValue item)
        {
            base.Add(item);
            return item;
        }
    }

    public class ConcatValue
    {
        internal ConcatValue()
        {
            this.IsCloseParentesis = false;
            this.IsOpenParentesis = false;
        }

        internal static ConcatValue ConcatObjectValue(object value)
        {
            ConcatValue concatValue = new ConcatValue();
            concatValue.Value = value;
            return concatValue;
        }

        internal static ConcatValue ConcatConstantValue(string value)
        {
            ConcatValue concatValue = new ConcatValue();
            concatValue.ConstantValue = value;
            return concatValue;
        }

        internal static ConcatValue ConcatAggregate(Aggregate aggregate)
        {
            ConcatValue concatValue = new ConcatValue();
            concatValue.Aggregate = aggregate;
            return concatValue;
        }

        internal static ConcatValue ConcatColumn(ColumnQuery column)
        {
            ConcatValue concatValue = new ConcatValue();
            concatValue.Column = column;
            return concatValue;
        }

        internal static ConcatValue OpenParentesis()
        {
            ConcatValue concatValue = new ConcatValue();
            concatValue.IsOpenParentesis = true;
            return concatValue;
        }

        internal static ConcatValue CloseParentesis()
        {
            ConcatValue concatValue = new ConcatValue();
            concatValue.IsCloseParentesis = true;
            return concatValue;
        }


        public object Value { get; private set; }
        public string ConstantValue { get; private set; }
        public Aggregate Aggregate { get; private set; }
        public ColumnQuery Column { get; private set; }
        public bool IsOpenParentesis { get; private set; }
        public bool IsCloseParentesis { get; private set; }
    }
}
