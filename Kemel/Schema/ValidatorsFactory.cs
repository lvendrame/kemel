using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.Schema
{
    public class ValidatorsFactory
    {
        public SchemaValidatorCollection BuilderColumnValidator(ColumnSchema column)
        {
            SchemaValidatorCollection validators = new SchemaValidatorCollection();
            return validators;
        }
        public SchemaValidatorCollection BuilderTableValidator(TableSchema column)
        {
            SchemaValidatorCollection validators = new SchemaValidatorCollection();
            return validators;
        }
    }
}
