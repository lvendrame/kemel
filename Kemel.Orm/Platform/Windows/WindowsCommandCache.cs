using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Providers;
using System.Web;
using Kemel.Orm.NQuery.Builder;

namespace Kemel.Orm.Platform.Windows
{
    public class WindowsCommandCache: CommandCache
    {
        private static Dictionary<string, string> _commandDic = null;
        internal Dictionary<string, string> CommandDic
        {
            get
            {
                if (_commandDic == null)
                {
                    _commandDic = new Dictionary<string, string>();
                }

                return _commandDic;
            }
        }

        public override string Add<TEtt>(CrudType crud, string command)
        {
            return this.Add(
                this.GetCommandName<TEtt>(crud),
                command);
        }

        public override string Add(string name, string command)
        {
            this.CommandDic.Add(name, command);
            return command;
        }

        public override bool Contains<TEtt>(CrudType crud)
        {
            return this.CommandDic.ContainsKey(this.GetCommandName<TEtt>(crud));
        }

        public override bool Contains(string commandName)
        {
            return this.CommandDic.ContainsKey(commandName);
        }

        public override string Get<TEtt>(CrudType crud)
        {
            return this[this.GetCommandName<TEtt>(crud)];
        }

        public override string this[string commandName]
        {
            get
            {
                return this.CommandDic[commandName];
            }
            set
            {
                this.CommandDic[commandName] = value;
            }
        }
    }
}
