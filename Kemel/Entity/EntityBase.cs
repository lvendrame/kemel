using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using Kemel.Orm.Constants;
using Kemel.Exceptions;
//using Kemel.Orm.Entity.Attributes;

namespace Kemel.Entity
{
    [Serializable()]
    public class EntityBase : ICloneable, IDisposable
    {
        [NonSerialized()]
        public EntityState EntityState = EntityState.Added;

        public DataRow ToDataRow(DataTable dt)
        {
            PropertyInfo[] props = this.GetType().GetPublicDeclaredProperties();

            DataRow row = dt.NewRow();
            foreach (PropertyInfo prop in props)
            {
                if (dt.Columns.Contains(prop.Name))
                {
                    object value = prop.GetValue(this, null);
                    row[prop.Name] = value != null ? value : DBNull.Value;
                }
            }
            return row;
        }

        public DataRow ToDataRow()
        {
            DataTable dt = new DataTable(this.GetType().Name.Replace(Sufix.ENTITY_NAME, string.Empty));

            PropertyInfo[] props = this.GetType().GetPublicDeclaredProperties();
            foreach (PropertyInfo propI in props)
            {
                Type collType = Nullable.GetUnderlyingType(propI.PropertyType);
                if (collType == null)
                {
                    collType = propI.PropertyType;
                }

                dt.Columns.Add(propI.Name, collType);
            }
            DataRow row = dt.NewRow();
            foreach (PropertyInfo prop in props)
            {
                object value = prop.GetValue(this, null);
                row[prop.Name] = value != null ? value : DBNull.Value;
            }
            return row;
        }

        public void ToDataRow(DataRow row)
        {
            PropertyInfo[] props = this.GetType().GetPublicDeclaredProperties();
            foreach (PropertyInfo prop in props)
            {
                if (row.Table.Columns.Contains(prop.Name))
                {
                    object value = prop.GetValue(this, null);
                    row[prop.Name] = value != null ? value : DBNull.Value;
                }
            }
        }

        #region Extended Properties

        [IsNotColumn()]
        public bool HasExtendProperties
        {
            get
            {
                if (this.objExtendedFields != null)
                    return this.objExtendedFields.Keys.Count != 0;
                return false;
            }
        }

        private Dictionary<string, object> objExtendedFields = null;

        [IsNotColumn()]
        public object this[string strFieldName]
        {
            get
            {
                strFieldName = strFieldName.ToUpper();
                if (this.ContainsField(strFieldName))
                    return objExtendedFields[strFieldName];
                else
                    throw new KemelException(Messages.InvalidField);
            }
            set
            {
                strFieldName = strFieldName.ToUpper();
                if (this.ContainsField(strFieldName))
                {
                    objExtendedFields[strFieldName] = value;
                }
                else
                {
                    if (this.objExtendedFields == null)
                        this.objExtendedFields = new Dictionary<string, object>();
                    this.objExtendedFields.Add(strFieldName, value);
                }
            }
        }

        public void Add(string strFieldName, object value)
        {
            this[strFieldName] = value;
        }

        public bool ContainsField(string strFieldName)
        {
            strFieldName = strFieldName.ToUpper();
            return this.objExtendedFields != null &&
                this.objExtendedFields.ContainsKey(strFieldName);
        }

        public T GetField<T>(string strFieldName)
        {
            Type typeT = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            object value = this[strFieldName];

            //Mudar esta parte, pois deve considerar os nullables também
            if (typeof(T).IsNullable() && Convert.IsDBNull(value))
            {
                value = null;
                return (T)value;
            }
            else
            {
                return (T)Convert.ChangeType(value, typeT);
            }


        }

        public bool TryGetField<T>(string strFieldName, out T tValue)
        {
            if (this.ContainsField(strFieldName))
            {
                try
                {
                    tValue = this.GetField<T>(strFieldName);
                    return true;
                }
                catch (Exception)
                {
                    tValue = default(T);
                    return false;
                }
            }
            else
            {
                tValue = default(T);
                return false;
            }
        }

        public List<KeyValuePair<string, Type>> GetFieldsNames()
        {
            List<KeyValuePair<string, Type>> lstRet = new List<KeyValuePair<string, Type>>();

            foreach (string key in this.objExtendedFields.Keys)
            {
                object obj = this.objExtendedFields[key];

                KeyValuePair<string, Type> item = new KeyValuePair<string, Type>(key, obj.GetType().GetFinalValueType());
                lstRet.Add(item);
            }

            return lstRet;
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            PropertyInfo[] props = this.GetType().GetPublicDeclaredProperties();
            object ret = Activator.CreateInstance(this.GetType());
            foreach (PropertyInfo prop in props)
            {
                prop.SetValue(ret, prop.GetValue(this, null), null);
            }
            (ret as EntityBase).EntityState = this.EntityState;
            return ret;
        }

        public TEtt Clone<TEtt>() where TEtt : EntityBase, new()
        {
            PropertyInfo[] props = typeof(TEtt).GetPublicDeclaredProperties();
            TEtt ret = new TEtt();
            foreach (PropertyInfo prop in props)
            {
                prop.SetValue(ret, prop.GetValue(this, null), null);
            }
            ret.EntityState = this.EntityState;
            return ret;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
