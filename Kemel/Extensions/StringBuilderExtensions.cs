using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Entity;
using System.Data;
using Kemel.Constants;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml;

namespace Kemel
{

    public static class StringBuilderExtensions
    {

        public static string RemoveFinalComma(this StringBuilder self)
        {
            return self.ToString(0, self.Length - Punctuation.COMMA.Length);
        }

    }
}
