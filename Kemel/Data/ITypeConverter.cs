using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Data
{
    public interface ITypeConverter
    {
        object ConvertTo(object value);
        object ConvertFrom(object value);
    }
}
