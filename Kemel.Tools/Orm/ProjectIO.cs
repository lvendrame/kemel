using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.DataBase.CodeDom;
using System.Xml;
using Kemel.Orm.Schema;
using Kemel.Orm.Entity.Attributes;
using System.IO;
using System.IO.Compression;

namespace Kemel.Tools.Orm
{
    public class ProjectIO
    {
        public ProjectIO(string fileName)
        {
            this.Path = fileName;
        }

        public const string GENERATE_ITEMS_TAG = "GenerateItems";
        public const string ITEM_TAG = "GenItem";
        public const string SCHEMA_TAG = "SchemaItem";
        public const string COLUMN_TAG = "Column";

        public const string BUSINESS_NAME_ATT = "BusinessName";
        public const string IS_EDITABLE_TABLE_ATT = "IsEditableTable";
        public const string GEN_DAL_ATT = "GenerateDal";
        public const string GEN_BUSINESS_ATT = "GenerateBusiness";

        public const string ALIAS_ATT = "Alias";
        public const string NAME_ATT = "Name";
        public const string SCHEMA_TYPE_ATT = "SchemaType";
        public const string ALLOW_NULL_ATT = "AllowNull";
        public const string DB_TYPE_ATT = "DBType";
        public const string IGNORE_COLUMN_ATT = "IgnoreColumn";
        public const string IS_FOREIGN_KEY_ATT = "IsForeignKey";
        public const string IS_IDENTITY_ATT = "IsIdentity";
        public const string IS_LOGICAL_EXCLUSION_COLUMN_ATT = "IsLogicalExclusionColumn";
        public const string IS_PRIMARY_KEY_ATT = "IsPrimaryKey";
        public const string MAX_LENGTH_ATT = "MaxLength";
        public const string NUMBER_PRECISION_ATT = "NumberPrecision";
        public const string NUMBER_SCALE_ATT = "NumberScale";
        public const string PARAM_DIRECTION_ATT = "ParamDirection";
        public const string REFERENCE_COLUMN_NAME_ATT = "ReferenceColumnName";
        public const string REFERENCE_TABLE_NAME_ATT = "ReferenceTableName";

        public string Path { get; private set; }
        public List<GenItem> Items { get; private set; }

        public void Save(List<GenItem> lstItems)
        {
            MemoryStream menStream = new System.IO.MemoryStream();

            XmlWriter writer = XmlWriter.Create(menStream);
            writer.WriteStartElement(GENERATE_ITEMS_TAG);
            foreach (GenItem item in lstItems)
            {
                this.WriteItem(writer, item);
            }
            writer.WriteEndElement();
            writer.Close();

            this.Compress(menStream, this.Path);
        }

        private void WriteItem(XmlWriter writer, GenItem item)
        {
            writer.WriteStartElement(ITEM_TAG);
            writer.WriteAttributeString(BUSINESS_NAME_ATT, item.BusinessName);
            writer.WriteAttributeString(IS_EDITABLE_TABLE_ATT, item.IsEditableTable.ToString());
            writer.WriteAttributeString(GEN_DAL_ATT, item.GenerateDal.ToString());
            writer.WriteAttributeString(GEN_BUSINESS_ATT, item.GenerateBusiness.ToString());

            this.WriteSchema(writer, item.Schema);

            writer.WriteEndElement();
        }

        private void WriteSchema(XmlWriter writer, TableSchema tableSchema)
        {
            writer.WriteStartElement(SCHEMA_TAG);
            writer.WriteAttributeString(ALIAS_ATT, tableSchema.Alias);
            writer.WriteAttributeString(NAME_ATT, tableSchema.Name);
            writer.WriteAttributeString(SCHEMA_TYPE_ATT, tableSchema.SchemaType.ToString());

            foreach (ColumnSchema column in tableSchema.Columns)
            {
                this.WriteColumn(writer, column);
            }
            writer.WriteEndElement();
        }

        private void WriteColumn(XmlWriter writer, ColumnSchema column)
        {
            writer.WriteStartElement(COLUMN_TAG);
            writer.WriteAttributeString(ALIAS_ATT, column.Alias);
            writer.WriteAttributeString(ALLOW_NULL_ATT, column.AllowNull.ToString());
            writer.WriteAttributeString(DB_TYPE_ATT, column.DBType.ToString());
            writer.WriteAttributeString(IGNORE_COLUMN_ATT, column.IgnoreColumn.ToString());
            writer.WriteAttributeString(IS_FOREIGN_KEY_ATT, column.IsForeignKey.ToString());
            writer.WriteAttributeString(IS_IDENTITY_ATT, column.IsIdentity.ToString());
            writer.WriteAttributeString(IS_LOGICAL_EXCLUSION_COLUMN_ATT, column.IsLogicalExclusionColumn.ToString());
            writer.WriteAttributeString(IS_PRIMARY_KEY_ATT, column.IsPrimaryKey.ToString());
            writer.WriteAttributeString(MAX_LENGTH_ATT, column.MaxLength.ToString());
            writer.WriteAttributeString(NAME_ATT, column.Name);
            writer.WriteAttributeString(NUMBER_PRECISION_ATT, column.NumberPrecision.ToString());
            writer.WriteAttributeString(NUMBER_SCALE_ATT, column.NumberScale.ToString());
            writer.WriteAttributeString(PARAM_DIRECTION_ATT, column.ParamDirection.ToString());
            writer.WriteAttributeString(REFERENCE_COLUMN_NAME_ATT, column.ReferenceColumnName);
            writer.WriteAttributeString(REFERENCE_TABLE_NAME_ATT, column.ReferenceTableName);

            writer.WriteEndElement();
        }

        public List<GenItem> Load()
        {

            Stream input = this.Decompress(this.Path);

            List<GenItem> lstItens = new List<GenItem>();
            XmlReader reader = XmlReader.Create(input);
            reader.ReadStartElement(GENERATE_ITEMS_TAG);

            do
            {
                if (reader.Name == ITEM_TAG && reader.IsStartElement())
                {
                    GenItem item = new GenItem();
                    this.ReadItem(reader, item);
                    lstItens.Add(item);
                }

                if (reader.Name == GENERATE_ITEMS_TAG)
                {
                    break;
                }
            }
            while (reader.Read());

            reader.ReadEndElement();
            reader.Close();
            return lstItens;
        }

        private void ReadItem(XmlReader reader, GenItem item)
        {
            reader.MoveToFirstAttribute();
            item.BusinessName = reader.ReadContentAsString();
            reader.MoveToNextAttribute();
            item.IsEditableTable = bool.Parse(reader.Value);
            reader.MoveToNextAttribute();
            item.GenerateDal = bool.Parse(reader.Value);
            reader.MoveToNextAttribute();
            item.GenerateBusiness = bool.Parse(reader.Value);

            while (reader.Read())
            {
                if (reader.Name == SCHEMA_TAG && reader.IsStartElement())
                {
                    item.Schema = new TableSchema();
                    this.ReadSchema(reader, item.Schema);
                    item.SchemaName = item.Schema.Name;
                }


                if (reader.Name == SCHEMA_TAG && !reader.IsStartElement())
                {
                    break;
                }
            }
        }

        private void ReadSchema(XmlReader reader, TableSchema tableSchema)
        {
            reader.MoveToFirstAttribute();
            tableSchema.Alias = reader.Value;
            reader.MoveToNextAttribute();
            tableSchema.Name = reader.Value;
            reader.MoveToNextAttribute();
            tableSchema.SchemaType = (SchemaType)Enum.Parse(typeof(SchemaType), reader.Value);

            while (reader.Read())
            {
                if (reader.Name == COLUMN_TAG)
                {
                    ColumnSchema column = new ColumnSchema(tableSchema);
                    tableSchema.Columns.Add(column);
                    this.ReadColumn(reader, column);
                }

                if (reader.Name == SCHEMA_TAG)
                    return;
            }
        }

        private void ReadColumn(XmlReader reader, ColumnSchema column)
        {
            reader.MoveToFirstAttribute();
            column.Alias = reader.Value;
            reader.MoveToNextAttribute();
            column.AllowNull = bool.Parse(reader.Value);
            reader.MoveToNextAttribute();
            column.DBType = (System.Data.DbType)Enum.Parse(typeof(System.Data.DbType), reader.Value);
            reader.MoveToNextAttribute();
            column.IgnoreColumn = bool.Parse(reader.Value);
            reader.MoveToNextAttribute();
            column.IsForeignKey = bool.Parse(reader.Value);
            reader.MoveToNextAttribute();
            column.IsIdentity = bool.Parse(reader.Value);
            reader.MoveToNextAttribute();
            column.IsLogicalExclusionColumn = bool.Parse(reader.Value);
            reader.MoveToNextAttribute();
            column.IsPrimaryKey = bool.Parse(reader.Value);
            reader.MoveToNextAttribute();
            column.MaxLength = string.IsNullOrEmpty(reader.Value) ? 0 : reader.ReadContentAsInt();
            reader.MoveToNextAttribute();
            column.Name = reader.Value;
            reader.MoveToNextAttribute();
            column.NumberPrecision = string.IsNullOrEmpty(reader.Value) ? 0 : reader.ReadContentAsInt();
            reader.MoveToNextAttribute();
            column.NumberScale = string.IsNullOrEmpty(reader.Value) ? 0 : reader.ReadContentAsInt();
            reader.MoveToNextAttribute();
            column.ParamDirection = (System.Data.ParameterDirection)Enum.Parse(typeof(System.Data.ParameterDirection), reader.Value);
            reader.MoveToNextAttribute();
            column.ReferenceColumnName = reader.Value;
            reader.MoveToNextAttribute();
            column.ReferenceTableName = reader.Value;
        }

        private void Compress(MemoryStream memStream, string fileName)
        {
            try
            {
                memStream.Seek(0, SeekOrigin.Begin);
                using (FileStream destFile = File.Create(fileName))
                {
                    using (DeflateStream compstream = new DeflateStream(destFile, CompressionMode.Compress))
                    {
                        const int buf_size = 8192;
                        byte[] buffer = new byte[buf_size];
                        int bytes_read = 0;
                        do
                        {
                            bytes_read = memStream.Read(buffer, 0, buf_size);
                            compstream.Write(buffer, 0, bytes_read);
                        } while (bytes_read != 0);
                    }
                    destFile.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Stream Decompress(string fileName)
        {
            MemoryStream memStream = new MemoryStream();
            try
            {
                using (FileStream sourceFile = File.OpenRead(fileName))
                {
                    using (DeflateStream compstream = new DeflateStream(sourceFile, CompressionMode.Decompress))
                    {
                        int theByte = compstream.ReadByte();
                        while (theByte != -1)
                        {
                            memStream.WriteByte((byte)theByte);
                            theByte = compstream.ReadByte();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            memStream.Seek(0, SeekOrigin.Begin);
            return memStream;
        }
    }
}
