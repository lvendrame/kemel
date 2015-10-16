using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Data;

namespace Kemel.Base
{
    public interface ITransactable
    {
        KemelTransaction Transaction {get;set;}
    }
}
