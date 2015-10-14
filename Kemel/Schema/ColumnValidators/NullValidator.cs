using Kemel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.Schema.ColumnValidators
{
    public class NullValidator: ISchemaValidator
    {
        public NullValidator(ColumnSchema column)
        {
            this.Column = column;
        }

        public ColumnSchema Column { get; set; }

        public void Validate(Base.CrudOperation crudOperation, object value)
        {
            switch (crudOperation)
            {
                case Kemel.Base.CrudOperation.Create:
                case Kemel.Base.CrudOperation.Update:
                case Kemel.Base.CrudOperation.Delete:
                    if (value == null || string.IsNullOrEmpty(value.ToString()))
                    {
                        throw new KemelException(string.Format(Messages.FieldNotEmpty, this.Column.Name));
                    }
                    break;
            }
        }
    }
}
