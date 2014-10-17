using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Orm.Data
{
    public interface ITypeConverter
    {
        object ConvertTo(object value);
        object ConvertFrom(object value);
    }
}
