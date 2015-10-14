using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Entity;
using System.Data;
using Kemel.Orm.Schema;
using Kemel.Orm.Constants;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml;

namespace Kemel
{
    public static class ListExtensions
    {
        public static T First<T>(this List<T> self)
        {
            if (self.Count == 0)
                return default(T);

            return self[0];
        }

        public static T Last<T>(this List<T> self)
        {
            if (self.Count == 0)
                return default(T);

            return self[self.Count - 1];
        }

        public static T AddItem<T>(this List<T> self, T item)
        {
            self.Add(item);
            return item;
        }

        public static bool IsEntityList<T>(this List<T> self)
        {
            return typeof(T).IsEntity();
        }

        public static void Dispose<T>(this List<T> self)
        {
            for (int i = self.Count - 1; i >= 0; i--)
            {
                T aux = self[i];
                self.RemoveAt(i);
                GC.SuppressFinalize(aux);
            }
            self.Clear();
            GC.SuppressFinalize(self);
        }

        public static DataTable ToDataTable<T>(this List<T> self)
            where T:EntityBase
        {
            TableSchema schema = SchemaContainer.GetSchema<T>();

            DataTable dt = new DataTable(schema.Name);

            foreach (ColumnSchema column in schema.Columns)
            {
                dt.Columns.Add(column.Name, column.Type);
            }

            bool verifyExtensionProperties = true;

            foreach (T ett in self)
            {
                List<string> lstExtensions = new List<string>();

                if (verifyExtensionProperties)
                {
                    if (ett.HasExtendProperties)
                    {
                        foreach (KeyValuePair<string, Type> item in ett.GetFieldsNames())
                        {
                            dt.Columns.Add(item.Key, item.Value);
                            lstExtensions.Add(item.Key);
                        }
                    }

                    verifyExtensionProperties = false;
                }

                DataRow row = dt.NewRow();
                foreach (ColumnSchema column in schema.Columns)
                {
                    row[column.Name] = column.GetValue(ett) ?? DBNull.Value;
                }

                foreach (string extProp in lstExtensions)
                {
                    row[extProp] = ett[extProp];
                }

                dt.Rows.Add(row);
            }
            return dt;
        }

        public static string WriteXml<T>(this List<T> self)
            where T : EntityBase
        {
            XmlSerializer xSer = new XmlSerializer(self.GetType());
            StringBuilder stbXml = new StringBuilder();
            xSer.Serialize(XmlWriter.Create(stbXml), self);
            return stbXml.ToString();
        }

        public static void WriteXml<T>(this List<T> self, string pstr_FileName)
            where T:EntityBase
        {
            XmlSerializer xSer = new XmlSerializer(self.GetType());
            XmlWriter writer = XmlWriter.Create(pstr_FileName);
            xSer.Serialize(writer, self);
            writer.Close();
        }

        public static void CreateFromXml<T>(this List<T> self, string pstr_Xml)
            where T:EntityBase
        {
            XmlSerializer xSer = new XmlSerializer(typeof(List<T>));
            XmlReader reader = XmlReader.Create(new System.IO.StringReader(pstr_Xml));
            if (!xSer.CanDeserialize(reader))
                throw new DataException("O Xml não é valido para o tipo " + typeof(List<T>).Name);

            self.AddRange(xSer.Deserialize(reader) as List<T>);
        }

        public static void CreateFromXmlFile<T>(this List<T> self, string pstr_XmlFileName)
            where T:EntityBase
        {
            XmlSerializer xSer = new XmlSerializer(typeof(List<T>));
            XmlReader reader = XmlReader.Create(new System.IO.StreamReader(pstr_XmlFileName));
            if (!xSer.CanDeserialize(reader))
                throw new DataException("O arquivo Xml não é valido para o tipo " + typeof(List<T>).Name);

            self.AddRange(xSer.Deserialize(reader) as List<T>);
        }

        #region Find and Sort

        public static TEtt Find<TEtt>(this List<TEtt> self, string pstr_FieldName, object findValue)
            where TEtt : EntityBase
        {
            PropertyInfo prop = typeof(TEtt).GetProperty(pstr_FieldName);
            if (prop == null)
                throw new DataException(string.Format("A entidade {0} não possue o campo {1}", typeof(TEtt).Name, pstr_FieldName));

            foreach (TEtt ett in self)
            {
                object obj = prop.GetValue(ett, null);
                if (findValue.Equals(obj))
                {
                    return ett;
                }
            }
            return null;
        }

        public static List<TEtt> FindAll<TEtt>(this List<TEtt> self, string pstr_FieldName, object findValue)
            where TEtt : EntityBase
        {
            PropertyInfo prop = typeof(TEtt).GetProperty(pstr_FieldName);
            if (prop == null)
                throw new DataException(string.Format("A entidade {0} não possue o campo {1}", typeof(TEtt).Name, pstr_FieldName));

            List<TEtt> elsRet = new List<TEtt>();
            foreach (TEtt ett in self)
            {
                object obj = prop.GetValue(ett, null);
                if (findValue.Equals(obj))
                {
                    elsRet.Add(ett);
                }
            }
            return elsRet;
        }

        public static TEtt Find<TEtt>(this List<TEtt> self, TEtt ett)
            where TEtt : EntityBase
        {
            Finder<TEtt> finder = new Finder<TEtt>();
            finder.Initialize(ett);

            foreach (TEtt ettComp in self)
            {
                if (finder.Compare(ettComp))
                {
                    return ettComp;
                }
            }
            return null;
        }

        public static List<TEtt> FindAll<TEtt>(this List<TEtt> self, TEtt ett)
            where TEtt : EntityBase
        {
            Finder<TEtt> finder = new Finder<TEtt>();
            finder.Initialize(ett);

            List<TEtt> elsRet = new List<TEtt>();
            foreach (TEtt ettComp in self)
            {
                if (finder.Compare(ettComp))
                {
                    elsRet.Add(ettComp);
                }
            }
            return elsRet;
        }

        public static void Sort<TEtt>(this List<TEtt> self, params string[] pvstr_FieldName)
            where TEtt : EntityBase
        {
            Sorter<TEtt> sorter = new Sorter<TEtt>(pvstr_FieldName);
            self.Sort(sorter.MultiplyCompareSort);
        }

        public static void Sort<TEtt>(this List<TEtt> self, string pstr_FieldName)
            where TEtt : EntityBase
        {
            self.Sort(pstr_FieldName, true);
        }

        public static void Sort<TEtt>(this List<TEtt> self, string pstr_FieldName, bool pbln_Ascending)
            where TEtt : EntityBase
        {
            Sorter<TEtt> sorter = new Sorter<TEtt>(pstr_FieldName);
            if (pbln_Ascending)
                self.Sort(sorter.SorterAscending);
            else
                self.Sort(sorter.SorterDescending);
        }

        #endregion

        #region internal class

        private class Sorter<TEtt>
            where TEtt: EntityBase
        {
            public Sorter(string pstr_FieldName)
            {
                PropertyInfo prop = typeof(TEtt).GetProperty(pstr_FieldName);
                if (prop == null)
                    throw new DataException("Impossível ordenar, campo " + pstr_FieldName + " não existe na entidade " + typeof(TEtt).Name + ".");

                this.Initialize(prop);
            }

            public Sorter(string[] pvstr_FieldName)
            {
                PropertyInfo[] vecProp = new PropertyInfo[pvstr_FieldName.Length];
                for (int i = 0; i < pvstr_FieldName.Length; i++)
                {
                    PropertyInfo prop = typeof(TEtt).GetProperty(pvstr_FieldName[i]);
                    if (prop == null)
                        throw new DataException("Impossível ordenar, campo " + pvstr_FieldName[i] + " não existe na entidade " + typeof(TEtt).Name + ".");

                    vecProp[i] = prop;
                }

                this.Initialize(vecProp);
            }

            private void Initialize(PropertyInfo prop)
            {
                lstSortItem = new List<SortItem>();
                SortItem item = new SortItem();
                item.prop = prop;
                item.Initialize();
                lstSortItem.Add(item);
            }

            private void Initialize(PropertyInfo[] vecProp)
            {
                lstSortItem = new List<SortItem>();

                for (int i = 0; i < vecProp.Length; i++)
                {
                    SortItem item = new SortItem();
                    item.prop = vecProp[i];
                    item.Initialize();
                    lstSortItem.Add(item);
                }
            }

            private List<SortItem> lstSortItem = null;

            public int SorterAscending(TEtt ettA, TEtt ettB)
            {
                return lstSortItem[0].Comparison(ettA, ettB);
            }

            public int SorterDescending(TEtt ettA, TEtt ettB)
            {
                return lstSortItem[0].Comparison(ettB, ettA);
            }

            public int MultiplyCompareSort(TEtt ettA, TEtt ettB)
            {
                int retComp = 0;
                for (int i = 0; i < lstSortItem.Count; i++)
                {
                    retComp = lstSortItem[i].Comparison(ettA, ettB);
                    if (retComp != 0)
                        return retComp;
                }
                return retComp;
            }

            private class SortItem
            {
                public PropertyInfo prop = null;
                private event Comparison<TEtt> Comparer;

                public int Comparison(TEtt ettA, TEtt ettB)
                {
                    int retVal = NullComparer(ettA, ettB);

                    return retVal == 2 ?
                        this.Comparer(ettA, ettB) :
                        retVal;
                }

                public int StringComparer(TEtt ettA, TEtt ettB)
                {
                    return string.Compare(prop.GetValue(ettA, null).ToString(),
                        prop.GetValue(ettB, null).ToString());
                }

                public int IntComparer(TEtt ettA, TEtt ettB)
                {
                    int vA = (int)prop.GetValue(ettA, null);
                    int vB = (int)prop.GetValue(ettB, null);

                    if (vA == vB)
                        return 0;
                    else if (vA > vB)
                        return 1;
                    else
                        return -1;
                }

                public int DoubleComparer(TEtt ettA, TEtt ettB)
                {
                    double vA = (double)prop.GetValue(ettA, null);
                    double vB = (double)prop.GetValue(ettB, null);

                    if (vA == vB)
                        return 0;
                    else if (vA > vB)
                        return 1;
                    else
                        return -1;
                }

                public int FloatComparer(TEtt ettA, TEtt ettB)
                {
                    float vA = (float)prop.GetValue(ettA, null);
                    float vB = (float)prop.GetValue(ettB, null);

                    if (vA == vB)
                        return 0;
                    else if (vA > vB)
                        return 1;
                    else
                        return -1;
                }

                public int DecimalComparer(TEtt ettA, TEtt ettB)
                {
                    decimal vA = (decimal)prop.GetValue(ettA, null);
                    decimal vB = (decimal)prop.GetValue(ettB, null);

                    if (vA == vB)
                        return 0;
                    else if (vA > vB)
                        return 1;
                    else
                        return -1;
                }

                public int CharComparer(TEtt ettA, TEtt ettB)
                {
                    char vA = (char)prop.GetValue(ettA, null);
                    char vB = (char)prop.GetValue(ettB, null);

                    if (vA == vB)
                        return 0;
                    else if (vA > vB)
                        return 1;
                    else
                        return -1;
                }

                public int DateComparer(TEtt ettA, TEtt ettB)
                {
                    DateTime vA = (DateTime)prop.GetValue(ettA, null);
                    DateTime vB = (DateTime)prop.GetValue(ettB, null);

                    if (vA == vB)
                        return 0;
                    else if (vA > vB)
                        return 1;
                    else
                        return -1;
                }

                public int NullComparer(TEtt ettA, TEtt ettB)
                {
                    object vA = prop.GetValue(ettA, null);
                    object vB = prop.GetValue(ettB, null);

                    if (vA != null && vB != null)
                        return 2;
                    else if (vA == null && vB == null)
                        return 0;
                    else if (vA == null)
                        return -1;
                    else
                        return 1;
                }

                public void Initialize()
                {
                    Type tp = Nullable.GetUnderlyingType(this.prop.PropertyType);
                    if (tp == null)
                        tp = this.prop.PropertyType;

                    if (tp == typeof(string))
                    {
                        this.Comparer += new Comparison<TEtt>(this.StringComparer);
                    }
                    else if (tp == typeof(int))
                    {
                        this.Comparer += new Comparison<TEtt>(this.IntComparer);
                    }
                    else if (tp == typeof(decimal))
                    {
                        this.Comparer += new Comparison<TEtt>(this.DecimalComparer);
                    }
                    else if (tp == typeof(double))
                    {
                        this.Comparer += new Comparison<TEtt>(this.DoubleComparer);
                    }
                    else if (tp == typeof(float))
                    {
                        this.Comparer += new Comparison<TEtt>(this.FloatComparer);
                    }
                    else if (tp == typeof(char))
                    {
                        this.Comparer += new Comparison<TEtt>(this.CharComparer);
                    }
                    else if (tp == typeof(DateTime))
                    {
                        this.Comparer += new Comparison<TEtt>(this.DateComparer);
                    }
                }
            }

        }

        private class Finder<TEtt>
            where TEtt: EntityBase
        {
            public void Initialize(TEtt ett)
            {
                lstItems = new List<FindItem>();
                foreach (PropertyInfo prop in typeof(TEtt).GetPublicDeclaredProperties())
                {
                    object value = prop.GetValue(ett, null);
                    if (value != null)
                    {
                        FindItem item = new FindItem();
                        item.Prop = prop;
                        item.Value = value;
                        lstItems.Add(item);
                    }
                }
            }

            public bool Compare(TEtt ett)
            {
                foreach (FindItem item in this.lstItems)
                {
                    object value = item.Prop.GetValue(ett, null);
                    if (value == null || !item.Value.Equals(value))
                        return false;
                }
                return true;
            }

            private List<FindItem> lstItems;

            private class FindItem
            {
                private PropertyInfo prop;
                public PropertyInfo Prop
                {
                    get { return prop; }
                    set { prop = value; }
                }

                private object objValue;
                public object Value
                {
                    get { return objValue; }
                    set { objValue = value; }
                }
            }
        }

        #endregion
    }
}
