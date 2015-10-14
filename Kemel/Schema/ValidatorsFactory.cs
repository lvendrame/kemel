using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.Schema
{
    public class ValidatorsFactory
    {
        public static ISchemaValidator BuilderColumnValidator(ColumnSchema column)
        {
            SchemaValidatorCollection validators = new SchemaValidatorCollection();

            if (column.IsIdentity)
            {
                validators.Add(new ColumnValidators.IdentityValidator(column));
            }

            if(column.MaxLength != 0)
            {
                validators.Add(new ColumnValidators.MaxLengthValidator(column));
            }

            if (!column.AllowNull)
            {
                validators.Add(new ColumnValidators.NullValidator(column));
            }

            return validators;
        }

    }
}
