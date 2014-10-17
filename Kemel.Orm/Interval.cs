using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Orm
{
    public class Interval<T>
        where T : IComparable
    {
        public Interval()
        {
        }

        public Interval(T start, T end)
        {
            this.tStart = start;
            this.tEnd = end;
        }

        private T tStart;
        public T Start
        {
            get { return tStart; }
            set { tStart = value; }
        }

        private T tEnd;
        public T End
        {
            get { return tEnd; }
            set { tEnd = value; }
        }

        public bool IsValid()
        {
            return this.tStart.CompareTo(this.tEnd) < 1;
        }

    }
}
