using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.Providers;
using System.Text;
using Kemel.Orm.Data;

namespace Kemel.Orm.NQuery.Builder
{
    public class SelectWriter : BaseWriter
    {
        public SelectWriter(OrmCommand command)
            : base(command)
        {
        }

        public SelectWriter(Provider selectedProvider)
            : base(selectedProvider)
        {
        }

        private StringBuilder stbBeginLimit;
        public StringBuilder BeginLimit
        {
            get
            {
                if ((stbBeginLimit == null))
                {
                    stbBeginLimit = new StringBuilder();
                }
                return stbBeginLimit;
            }
        }

        private StringBuilder stbEndLimit;
        public StringBuilder EndLimit
        {
            get
            {
                if ((stbEndLimit == null))
                {
                    stbEndLimit = new StringBuilder();
                }
                return stbEndLimit;
            }
        }

        private StringBuilder stbColumns;
        public StringBuilder Columns
        {
            get
            {
                if ((stbColumns == null))
                {
                    stbColumns = new StringBuilder();
                }
                return stbColumns;
            }
        }

        private StringBuilder stbTables;
        public StringBuilder Tables
        {
            get
            {
                if ((stbTables == null))
                {
                    stbTables = new StringBuilder();
                }
                return stbTables;
            }
        }

        private StringBuilder stbJoins;
        public StringBuilder Joins
        {
            get
            {
                if ((stbJoins == null))
                {
                    stbJoins = new StringBuilder();
                }
                return stbJoins;
            }
        }

        private StringBuilder stbConstraints;
        public StringBuilder Constraints
        {
            get
            {
                if ((stbConstraints == null))
                {
                    stbConstraints = new StringBuilder();
                }
                return stbConstraints;
            }
        }

        private StringBuilder stbGroupBys;
        public StringBuilder GroupBys
        {
            get
            {
                if ((stbGroupBys == null))
                {
                    stbGroupBys = new StringBuilder();
                }
                return stbGroupBys;
            }
        }

        private StringBuilder stbOrderBys;
        public StringBuilder OrderBys
        {
            get
            {
                if ((stbOrderBys == null))
                {
                    stbOrderBys = new StringBuilder();
                }
                return stbOrderBys;
            }
        }

        private StringBuilder stbUnions;
        public StringBuilder Unions
        {
            get
            {
                if ((stbUnions == null))
                {
                    stbUnions = new StringBuilder();
                }
                return stbUnions;
            }
        }
    }
}
