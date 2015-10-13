using Kemel.Base;
using Kemel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.Schema
{
    public class SchemaValidatorCollection: List<ISchemaValidator>, ISchemaValidator
    {
        public void Validate(CrudOperation crudOperation, object value)
        {
            foreach (ISchemaValidator validator in this)
            {
                validator.Validate(crudOperation, value);
            }
        }
    }
}
