using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.Exceptions
{
    [Serializable]
    public class KemelException : Exception
    {
        public KemelException() { }
        public KemelException(string message) : base(message) { }
        public KemelException(string message, Exception inner) : base(message, inner) { }
        protected KemelException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}