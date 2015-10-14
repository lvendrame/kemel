using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Entity;
using System.Data;
using Kemel.Orm.Schema;
using Kemel.Orm.Constants;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml;

namespace Kemel
{

    public static class stringExtensions
    {
        public static string RemoveFinalComma(this string self)
        {
            return self.Substring(0, self.Length - Punctuation.COMMA.Length);
        }
    }
}
