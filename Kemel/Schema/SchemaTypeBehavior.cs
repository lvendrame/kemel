using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.Schema
{
    public class SchemaTypeBehavior : BehaviorProviderContainer<ISchemaTypeProvider, SchemaType>
    {
        public SchemaTypeBehavior(ISchemaTypeProvider schemaTypeProvider)
            : base(schemaTypeProvider)
        {
        }

        public override void DoBehavior(SchemaType enumType)
        {
            switch (enumType)
            {
                case SchemaType.Table:
                    this.Provider.TableBehavior();
                    break;
                case SchemaType.View:
                    this.Provider.ViewBehavior();
                    break;
                case SchemaType.Procedure:
                    this.Provider.ProcedureBehavior();
                    break;
                case SchemaType.ScalarFunction:
                    this.Provider.ScalarFunctionBehavior();
                    break;
                case SchemaType.TableFunction:
                    this.Provider.TableFunctionBehavior();
                    break;
                case SchemaType.AggregateFunction:
                    this.Provider.AggregateFunctionBehavior();
                    break;
                case SchemaType.None:
                    this.Provider.NoneBehavior();
                    break;
                default:
                    base.ThrowInvalidEnumType();
                    break;
            }
        }
    }
}
