using Kemel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.Schema.ColumnValidators
{
    public class MaxLengthValidator: ISchemaValidator
    {
        public MaxLengthValidator(ColumnSchema column)
        {
            this.Column = column;
        }

        public ColumnSchema Column { get; set; }

        public void Validate(Base.CrudOperation crudOperation, object value)
        {
            if (value != null && value.ToString().Length > this.Column.MaxLength)
            {
                throw new KemelException(string.Format(Messages.FieldGreaterThanMaxLength, this.Column.Name, this.Column.MaxLength));
            }
        }
    }
}
