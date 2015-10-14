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

    public static class MemberInfoExtensions
    {
        
        public static CustomAttribute GetCustomAttribute<CustomAttribute>(this MemberInfo self)
            where CustomAttribute : Attribute
        {
            CustomAttribute retAtt = null;
            object[] objattributes = self.GetCustomAttributes(typeof(CustomAttribute), true);
            if (objattributes.Length != 0)
            {
                retAtt = (objattributes[0] as CustomAttribute);
            }
            return retAtt;
        }

        public static List<CustomAttribute> GetCustomAttributes<CustomAttribute>(this MemberInfo self)
            where CustomAttribute : Attribute
        {
            List<CustomAttribute> lstAtt = new List<CustomAttribute>();
            object[] objattributes = self.GetCustomAttributes(typeof(CustomAttribute), true);
            if (objattributes.Length != 0)
            {
                foreach (object att in objattributes)
                {
                    lstAtt.Add(att as CustomAttribute);
                }
            }
            return lstAtt;
        }

    }
}
