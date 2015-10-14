using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.Schema.ColumnValidators
{
    public class IdentityValidator: ISchemaValidator
    {
        public void Validate(Base.CrudOperation crudOperation, object value)
        {
            throw new NotImplementedException();
        }
    }
}
