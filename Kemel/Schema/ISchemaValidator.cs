using Kemel.Base;
using Kemel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.Schema
{
    public interface ISchemaValidator
    {
        void Validate(CrudOperation crudOperation, object value);
    }
}
