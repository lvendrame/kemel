using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Util.Test
{
    public static class General
    {

        public static string RemovePoints(string item)
        {
            char nullChar = '\0';
            return item
                .Replace('.', nullChar)
                .Replace('-', nullChar)
                .Replace(',', nullChar)
                .Replace('/', nullChar)
                .Replace(':', nullChar);
        }

    }
}
