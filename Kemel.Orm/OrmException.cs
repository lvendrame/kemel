using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Orm
{
    [global::System.Serializable]
    public class OrmException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public OrmException() { }
        public OrmException(string message) : base(message) { }
        public OrmException(string message, Exception inner) : base(message, inner) { }
        protected OrmException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
