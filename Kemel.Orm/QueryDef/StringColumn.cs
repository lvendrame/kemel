using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Constants;

namespace Kemel.Orm.QueryDef
{
    public class StringColumnCollection : List<StringColumn>
    {
        new public StringColumn Add(StringColumn item)
        {
            base.Add(item);
            return item;
        }

    }

    public class StringColumn
    {
        public string Value { get; set; }
        public string Alias { get; set; }

        public Query Parent { get; set; }


        public StringColumn(Query parent, string value)
        {
            this.Parent = parent;
            this.Value = value;
        }

        public Query End()
        {
            return this.Parent;
        }

        public Query As(string alias)
        {
            this.Alias = alias;
            return this.Parent;
        }

        public string ValueWithAlias
        {
            get
            {
                return
                    string.IsNullOrEmpty(this.Alias) ?
                    this.Value :
                    string.Concat(this.Value, Punctuation.WHITE_SPACE, DML.AS, Punctuation.WHITE_SPACE, this.Alias);
            }
        }
    }
}
