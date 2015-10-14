using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.Kuery.Behaviors
{
    public class QueryTypeBehavior: ProviderContainer<IQueryTypeProvider, QueryType>
    {
        public QueryTypeBehavior(IQueryTypeProvider queryTypeProvider)
            : base(queryTypeProvider)
        {
        }

        public override void DoBehavior(QueryType enumType)
        {
            switch (enumType)
            {
                case QueryType.Select:
                    this.Provider.SelectBehavior();
                    break;
                case QueryType.Insert:
                    this.Provider.InsertBehavior();
                    break;
                case QueryType.Delete:
                    this.Provider.DeleteBehavior();
                    break;
                case QueryType.Update:
                    this.Provider.UpdateBehavior();
                    break;
                case QueryType.Procedure:
                    this.Provider.ProcedureBehavior();
                    break;
                case QueryType.Function:
                    this.Provider.FunctionBehavior();
                    break;
                default:
                    base.ThrowInvalidEnumType();
                    break;
            }
        }
    }
}
