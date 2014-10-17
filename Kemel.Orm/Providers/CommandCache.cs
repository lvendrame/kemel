using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Entity;
using Kemel.Orm.NQuery.Builder;

namespace Kemel.Orm.Providers
{
    public abstract class CommandCache
    {
        public string GetCommandName<TEtt>(CrudType crud)
            where TEtt : EntityBase
        {
            return string.Concat(crud.ToString(), "_", typeof(TEtt).Name);
        }

        public abstract string Add<TEtt>(CrudType crud, string command)
            where TEtt: EntityBase;

        public abstract string Add(string name, string command);

        public abstract bool Contains<TEtt>(CrudType crud)
            where TEtt : EntityBase;

        public abstract bool Contains(string commandName);

        public abstract string Get<TEtt>(CrudType crud)
            where TEtt : EntityBase;

        public abstract string this[string commandName]
        {
            get;
            set;
        }

    }
}
