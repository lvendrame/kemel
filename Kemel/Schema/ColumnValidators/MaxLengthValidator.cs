using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.Schema.ColumnValidators
{
    public class MaxLengthValidator: ISchemaValidator
    {
        public MaxLengthValidator(int maxLength)
        {
            this.MaxLength = maxLength;
        }

        public int MaxLength { get; set; }

        public void Validate(Base.CrudOperation crudOperation, object value)
        {
            if (value != null && value.ToString().Length > this.MaxLength)
            {
                throw new OrmException(string.Format(Messages.FieldGreaterThanMaxLength, this.Name, this.MaxLength));
            }
        }
    }
}
