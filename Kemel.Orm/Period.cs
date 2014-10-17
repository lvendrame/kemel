using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Orm
{
    public class Period
    {
        public Period()
        {

        }

        public Period(DateTime? start, DateTime? end)
        {
            this.tStart = start;
            this.tEnd = end;
        }

        private DateTime? tStart;
        public DateTime? Start
        {
            get { return tStart; }
            set { tStart = value; }
        }

        private DateTime? tEnd;
        public DateTime? End
        {
            get { return tEnd; }
            set { tEnd = value; }
        }

        public bool HasStart
        {
            get { return tStart.HasValue; }
        }

        public bool HasEnd
        {
            get { return tEnd.HasValue; }
        }

        public bool HasPeriod
        {
            get { return tEnd.HasValue && tStart.HasValue; }
        }

        public bool IsValid()
        {
            if (this.HasPeriod)
            {
                return this.tStart.Value <= this.tEnd.Value;
            }
            else
            {
                return true;
            }
        }

        public bool IntersectsWith(Period period)
        {
            return !(period.End < this.Start || period.Start > this.End);
        }
    }
}
