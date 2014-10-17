using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Data;

namespace Kemel.Orm.Base
{
    public interface ITransactable
    {
        OrmTransaction Transaction {get;set;}
    }
}
