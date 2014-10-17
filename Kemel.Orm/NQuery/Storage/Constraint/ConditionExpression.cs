using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Kemel.Orm.NQuery.Storage.Column;
using Kemel.Orm.NQuery.Storage.Value;
using Kemel.Orm.NQuery.Storage.Constraint;

namespace Kemel.Orm.NQuery.Storage.Constraint
{

    public class ConditionExpression
    {

        private StoredColumn objColumn = null;
        public StoredColumn Column
        {
            get { return this.objColumn; }
            set { this.objColumn = value; }
        }

        private ComparisonOperator objComparison = ComparisonOperator.Equal;
        public ComparisonOperator Comparison
        {
            get { return this.objComparison; }
            set { this.objComparison = value; }
        }

        private StoredValue objValue = null;
        public StoredValue Value
        {
            get { return this.objValue; }
            set { this.objValue = value; }
        }

    }

}
