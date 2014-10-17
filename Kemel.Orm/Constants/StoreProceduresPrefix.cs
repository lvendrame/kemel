using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Orm.Constants
{
    public static class StoreProceduresPrefix
    {
        public const string INSERT = "Insert_";
        public const string UPDATE = "Update_";
        public const string DELETE = "Delete_";
        public const string DELETE_BY_ID = "DeleteById_";
        public const string SELECT_ALL = "SelectAll_";
        public const string SELECT_BY_ID = "SelectById_";
        public const string SELECT_BY_COMMAND = "SelectByCommand_";
    }
}
