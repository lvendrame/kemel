using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Kemel.Orm.Schema;
using Kemel.Orm.DataBase;
using Kemel.Orm.DataBase.CodeDom;
using System.IO;
using Kemel.Orm.Constants;

namespace Kemel.Tools.Orm
{
    public partial class FrmEntityGenerator : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        public string OriginalCaption { get; set; }
        public FrmEntityGenerator()
        {
            InitializeComponent();
            this.TablesDefinitions = new Dictionary<string, GenItem>();
            this.BindComboTypes();
            this.BindComboEntityType();
            this.BindComboInputDirection();
            this.OriginalCaption = this.Text;
            this.Text = string.Format("{0} - {1}", this.OriginalCaption, "Empty Document");
        }

        private void BindComboTypes()
        {
            this.BindComboWithEnum(cboType, typeof(DbType));
        }

        private void BindComboEntityType()
        {
            this.BindComboWithEnum(cboEntityType, typeof(Kemel.Orm.Entity.Attributes.SchemaType));
        }

        private void BindComboInputDirection()
        {
            this.BindComboWithEnum(cboInputDirection, typeof(ParameterDirection));
        }

        private void BindComboWithEnum(ComboBox combo, Type type)
        {
            Array vecObject = Enum.GetValues(type);
            for (int i = 0; i < vecObject.Length; i++)
            {
                combo.Items.Add(vecObject.GetValue(i));
            }
        }

        private void btnSelectTables_Click(object sender, EventArgs e)
        {
            FrmDialogSelectTables frmSelectTable = new FrmDialogSelectTables();            

            if (frmSelectTable.ShowDialog() == DialogResult.OK)
            {
                for (int i = cklTables.Items.Count - 1; i > -1 ; i--)
                {
                    T_Table_Entity tbItem = cklTables.Items[i] as T_Table_Entity;
                    int index = frmSelectTable.IndexOfTable(tbItem);
                    if (index != -1)
                    {
                        frmSelectTable.SelectedTables.RemoveAt(index);
                    }
                }

                foreach (T_Table_Entity table in frmSelectTable.SelectedTables)
                {
                    cklTables.SetItemChecked(cklTables.Items.Add(table), true);
                }
            }
        }

        public Dictionary<string, GenItem> TablesDefinitions { get; private set; }

        int oldSelectedIndexTable = -1;
        public GenItem CurrentItem { get; private set; }
        private void cklTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (oldSelectedIndexTable != -1)
            {
                this.SaveSchema(this.cklTables.Items[oldSelectedIndexTable] as T_Table_Entity);
            }
            oldSelectedIndexTable = cklTables.SelectedIndex;

            if (this.cklTables.SelectedIndex == -1)
            {
                if (ltbColumns.Items.Count > 0)
                {
                    try
                    {
                        ltbColumns.Items.Clear();
                    }
                    catch 
                    {
                        ltbColumns.DataSource = null;
                    }
                }
                return;
            }

            T_Table_Entity table = this.cklTables.SelectedItem as T_Table_Entity;

            CurrentItem = CreateGenItem(table);
            this.LoadSchema();

            if (ltbColumns.Items.Count == 0)
            {
                ltbColumns.SelectedIndex = -1;
            }
            else
            {
                ltbColumns.SelectedIndex = 0;
            }
            ltbColumns_SelectedIndexChanged(null, null);
        }

        private GenItem CreateGenItem(T_Table_Entity table)
        {
            if (this.TablesDefinitions.ContainsKey(table.NAME))
            {
                return this.TablesDefinitions[table.NAME];
            }
            else
            {
                GenItem item = new GenItem();
                using (BusinessDataBase objNgDataBase = new BusinessDataBase())
                {
                    item.Schema = table.ToTableSchema();
                    item.SchemaName = table.NAME;
                    item.BusinessName = this.GenBusinessName(item.Schema.Name);
                    item.IsEditableTable = addNewColumn;
                    item.GenerateBusiness = true;
                    item.GenerateDal = true;

                    if (addNewColumn)
                    {
                        this.AddColumn(item.Schema);
                    }
                    else
                    {
                        List<T_Column_Entity> lstColumns = objNgDataBase.GetTableDefinition(table.NAME);
                        foreach (T_Column_Entity column in lstColumns)
                        {
                            column.ToColumnSchema(item.Schema);
                        }
                    }

                    this.TablesDefinitions.Add(table.NAME, item);
                }
                return item;
            }
        }
        
        private ColumnSchema AddColumn(TableSchema tableSchema)
        {
            T_Column_Entity column = new T_Column_Entity();
            column.ALLOW_NULL = false;
            column.COLUMN = "Coluna_01";
            column.IS_IDENTITY = false;
            column.IS_PRIMARY_KEY = false;
            column.TYPE = "varchar";
            column.TABLE = tableSchema.Name;
            ColumnSchema retColSchema = column.ToColumnSchema(tableSchema);
            retColSchema.ParamDirection = ParameterDirection.Input;            
            return retColSchema;
        }

        private void LoadSchema()
        {
            this.ltbColumns.DataSource = null;
            this.ltbColumns.Items.Clear();

            this.txtTableName.Text = this.CurrentItem.Schema.Name ?? string.Empty;
            this.txtTableAlias.Text = this.CurrentItem.Schema.Alias ?? string.Empty;
            this.cboEntityType.SelectedItem = this.CurrentItem.Schema.SchemaType;
            this.chkGenDal.Checked = this.CurrentItem.GenerateDal;
            this.chkGenBusiness.Checked = this.CurrentItem.GenerateBusiness;

            if (string.IsNullOrEmpty(this.CurrentItem.BusinessName))
            {
                this.txtBusinessName.Text = this.GenBusinessName(this.CurrentItem.Schema.Name);
            }
            else
            {
                this.txtBusinessName.Text = this.CurrentItem.BusinessName;
            }
            chkIgnoreProperty.Enabled = this.CurrentItem.IsEditableTable;
            txtTableName.Enabled = this.CurrentItem.IsEditableTable;
            txtColumnName.Enabled = this.CurrentItem.IsEditableTable;
            cboEntityType.Enabled = this.CurrentItem.IsEditableTable;

            this.ltbColumns.DisplayMember = "Name";
            this.ltbColumns.ValueMember = "Name";
            this.ltbColumns.DataSource = this.CurrentItem.Schema.Columns;

            if (this.ltbColumns.Items.Count == 0)
            {
                this.ltbColumns.SelectedIndex = -1;
                oldSelectedIndexColumn = -1;
            }
            else
            {
                this.ltbColumns.SelectedIndex = 0;
                oldSelectedIndexColumn = 0;
            }

            isProcSchema = this.CurrentItem.Schema.SchemaType == Kemel.Orm.Entity.Attributes.SchemaType.Procedure;
            this.lblInputDirection.Visible = isProcSchema;
            this.cboInputDirection.Visible = isProcSchema;

            this.LoadColumn();
        }
        bool isProcSchema = false;

        private string GenBusinessName(string tableName)
        {
            if (tableName.StartsWith("T_"))
                tableName = tableName.Remove(0, 2);

            tableName = tableName.ToLower();
            bool nextUpperCase = true;

            char[] charArray = new char[tableName.Length];
            for (int i = 0; i < tableName.Length; i++)
            {
                if (tableName[i] == '_')
                {
                    tableName = tableName.Remove(i, 1);
                    nextUpperCase = true;
                    i--;
                }
                else if (nextUpperCase)
                {
                    nextUpperCase = false;
                    charArray[i] = Char.ToUpper(tableName[i]);
                }
                else
                {
                    charArray[i] = tableName[i];
                }
            }

            return new string(charArray).Replace("\0", string.Empty);
        }

        private void SaveSchema(T_Table_Entity tbEntity)
        {
            this.CurrentItem.Schema.Name = this.txtTableName.Text;
            this.CurrentItem.Schema.Alias = this.txtTableAlias.Text;
            this.CurrentItem.BusinessName = this.txtBusinessName.Text;
            this.CurrentItem.GenerateDal = this.chkGenDal.Checked;
            this.CurrentItem.GenerateBusiness = this.chkGenBusiness.Checked;

            if (cboEntityType.SelectedItem != null)
                this.CurrentItem.Schema.SchemaType = (Kemel.Orm.Entity.Attributes.SchemaType)cboEntityType.SelectedItem;

            if (this.CurrentItem.SchemaName != this.CurrentItem.Schema.Name)
            {
                this.TablesDefinitions.Remove(this.CurrentItem.SchemaName);
                this.CurrentItem.SchemaName = this.CurrentItem.Schema.Name;
                this.TablesDefinitions.Add(this.CurrentItem.SchemaName, this.CurrentItem);
                
                tbEntity.NAME = this.CurrentItem.SchemaName;
                cklTables.Update();
            }

            this.SaveColumn();
        }

        public ColumnSchema CurrentColumn { get; private set; }
        int oldSelectedIndexColumn = -1;
        private void ltbColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (oldSelectedIndexColumn != -1)
                this.SaveColumn();

            oldSelectedIndexColumn = ltbColumns.SelectedIndex;

            this.CurrentColumn = ltbColumns.SelectedItem as ColumnSchema;
            this.LoadColumn();
        }

        private void LoadColumn()
        {
            if (this.CurrentColumn != null)
            {
                this.txtColumnName.Text = this.CurrentColumn.Name;
                this.txtColumnAlias.Text = this.CurrentColumn.Alias;
                this.chkIsPrimaryKey.Checked = this.CurrentColumn.IsPrimaryKey;
                this.chkIsIdentity.Checked = this.CurrentColumn.IsIdentity;
                this.chkAllowNull.Checked = this.CurrentColumn.AllowNull;
                this.chkIsForeignKey.Checked = this.CurrentColumn.IsForeignKey;
                this.chkIsLogicalExclusionColumn.Checked = this.CurrentColumn.IsLogicalExclusionColumn;
                this.txtReferencedTable.Text = this.CurrentColumn.ReferenceTableName;
                this.txtReferencedColumn.Text = this.CurrentColumn.ReferenceColumnName;
                this.cboType.SelectedItem = this.CurrentColumn.DBType;
                this.chkIgnoreProperty.Checked = this.CurrentColumn.IgnoreColumn;
                if (isProcSchema)
                {
                    this.cboInputDirection.SelectedItem = this.CurrentColumn.ParamDirection;
                }
            }
            else
            {
                this.txtColumnName.Text = string.Empty;
                this.txtColumnAlias.Text = string.Empty;
                this.chkIsPrimaryKey.Checked = false;
                this.chkIsIdentity.Checked = false;
                this.chkAllowNull.Checked = false;
                this.chkIsForeignKey.Checked = false;
                this.chkIsLogicalExclusionColumn.Checked = false;
                this.txtReferencedTable.Text = string.Empty;
                this.txtReferencedColumn.Text = string.Empty;
                this.chkIgnoreProperty.Checked = false;
            }
        }

        private void SaveColumn()
        {
            if (this.CurrentColumn != null)
            {
                this.CurrentColumn.Name = this.txtColumnName.Text;
                this.CurrentColumn.Alias = this.txtColumnAlias.Text;
                this.CurrentColumn.IsLogicalExclusionColumn = this.chkIsLogicalExclusionColumn.Checked;
                if (cboType.SelectedItem != null)
                    this.CurrentColumn.DBType = (DbType)cboType.SelectedItem;
                this.CurrentColumn.IgnoreColumn = this.chkIgnoreProperty.Checked;
                
                if (isProcSchema)
                {
                    this.CurrentColumn.ParamDirection = (ParameterDirection)this.cboInputDirection.SelectedItem;
                }
            }
        }

        private void GenerateTable(DirectoryOrmStructure directories, GenItem item, string extension)
        {
            File.AppendAllText(
                Path.Combine(directories.Entity.FullName, string.Concat(item.Classes.EntityFileName, Punctuation.DOT, extension)),
                item.Classes.EntityClass);
            File.AppendAllText(
                Path.Combine(directories.Definition.FullName, string.Concat(item.Classes.EntityFileName, Punctuation.DOT, extension)),
                item.Classes.DefinitionClass);
            File.AppendAllText(
                Path.Combine(directories.Schema.FullName, string.Concat(item.Classes.EntityFileName, Punctuation.DOT, extension)),
                item.Classes.SchemaClass);
            File.AppendAllText(
                Path.Combine(directories.Extension.FullName, string.Concat(item.Classes.EntityFileName, Punctuation.DOT, extension)),
                item.Classes.ExtensionClass);
            if (item.GenerateDal)
            {
                File.AppendAllText(
                    Path.Combine(directories.Dal.FullName, string.Concat(item.Classes.DalFileName, Punctuation.DOT, extension)),
                    item.Classes.DalClass);
            }
            if (item.GenerateBusiness)
            {
                File.AppendAllText(
                    Path.Combine(directories.Business.FullName, string.Concat(item.Classes.BusinessFileName, Punctuation.DOT, extension)),
                    item.Classes.BusinessClass);
            }
        }

        private void DeleteFiles(FileInfo[] files)
        {
            foreach (FileInfo file in files)
            {
                file.Delete();
            }
        }

        private List<GenItem> GetSelectedTables()
        {
            List<GenItem> lstGen = new List<GenItem>();
            foreach (int index in this.cklTables.CheckedIndices)
            {
                T_Table_Entity table = (this.cklTables.Items[index] as T_Table_Entity);
                this.CurrentItem = this.CreateGenItem(table);

                lstGen.Add(this.TablesDefinitions[table.NAME]);
            }
            return lstGen;
        }

        private string GenerateCodes(List<GenItem> list)
        {
            GenConfig config = new GenConfig();
            config.Tables = list;
            config.NameSpace = this.txtNamespace.Text.Trim();
            config.Language = FrmProviderSettings.SelectedLanguage;
            config.OrmNameSpace = FrmProviderSettings.OrmNameSpace;
            config.NewQuery = FrmProviderSettings.NewQuery;
            config.GenerateCodes();

            switch (config.Language)
            {
                case Languages.CSharp:
                    return "cs";
                case Languages.VBNet:
                    return "vb";
                default:
                    return "cs";
            }
        }

        int tableIdx = 1;
        bool addNewColumn = false;
        private void btnAddTable_Click(object sender, EventArgs e)
        {
            AddTable(Kemel.Orm.Entity.Attributes.SchemaType.Table);
        }

        private void AddTable(Kemel.Orm.Entity.Attributes.SchemaType type)
        {
            T_Table_Entity table = new T_Table_Entity();
            table.NAME = "Nova_Tabela_" + tableIdx.ToString();
            addNewColumn = true;
            this.CreateGenItem(table).Schema.SchemaType = type;
            addNewColumn = false;
            cklTables.SetItemChecked(cklTables.Items.Add(table), true);
            cklTables.SelectedIndex = cklTables.Items.Count - 1;

            tableIdx++;
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            if (cklTables.SelectedItem != null)
            {
                oldSelectedIndexTable = -1;
                cklTables.Items.RemoveAt(cklTables.SelectedIndex);
                cklTables.SelectedIndex = cklTables.Items.Count - 1;
            }
        }

        private void btnAddColumn_Click(object sender, EventArgs e)
        {
            this.SaveSchema(this.cklTables.Items[this.cklTables.SelectedIndex] as T_Table_Entity);
            ColumnSchema colum = this.AddColumn(this.CurrentItem.Schema);
            colum.IgnoreColumn = !this.CurrentItem.IsEditableTable;
            this.LoadSchema();
            this.ltbColumns.SelectedIndex = this.ltbColumns.Items.Count - 1;
        }

        private void btnDeleteColumn_Click(object sender, EventArgs e)
        {
            this.CurrentItem.Schema.Columns.RemoveAt(this.ltbColumns.SelectedIndex);
            this.LoadSchema();
            this.ltbColumns.SelectedIndex = this.ltbColumns.Items.Count - 1;
        }

        private void chkIgnoreProperty_CheckedChanged(object sender, EventArgs e)
        {
            txtColumnName.Enabled = this.CurrentItem.IsEditableTable || chkIgnoreProperty.Checked;
        }

        private void cboEntityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            isProcSchema =  Kemel.Orm.Entity.Attributes.SchemaType.Procedure == (Kemel.Orm.Entity.Attributes.SchemaType)cboEntityType.SelectedItem;
            if (isProcSchema && this.CurrentColumn != null)
            {
                this.cboInputDirection.SelectedItem = this.CurrentColumn.ParamDirection;
            }
            lblInputDirection.Visible = isProcSchema;
            cboInputDirection.Visible = isProcSchema;
        }

        private void generateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCurrentSchema();

            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (string.IsNullOrEmpty(this.FileName))
                    fbd.RootFolder = Environment.SpecialFolder.MyComputer;
                else
                    fbd.SelectedPath = Path.GetDirectoryName(this.FileName);

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    List<GenItem> lstItems = this.GetSelectedTables();
                    string extension = this.GenerateCodes(lstItems);
                    DirectoryOrmStructure directories = new DirectoryOrmStructure(fbd.SelectedPath, EntityGen.EntityNameSpace,
                                    DalGen.DalNameSpace, BusinessGen.BusinessNameSpace);

                    if (MessageBox.Show("Todos os arquivos do diretório serão excluídos. Deseja continuar?", "DigiSystem Tools", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        directories.CleanDirectories();

                        foreach (GenItem item in lstItems)
                        {
                            switch (item.Schema.SchemaType)
                            {
                                case Kemel.Orm.Entity.Attributes.SchemaType.Table:
                                    this.GenerateTable(directories, item, extension);
                                    break;
                                case Kemel.Orm.Entity.Attributes.SchemaType.View:
                                    this.GenerateTable(directories.View, item, extension);
                                    break;
                                case Kemel.Orm.Entity.Attributes.SchemaType.Procedure:
                                    this.GenerateTable(directories.Procedure, item, extension);
                                    break;
                                case Kemel.Orm.Entity.Attributes.SchemaType.ScalarFunction:
                                case Kemel.Orm.Entity.Attributes.SchemaType.TableFunction:
                                case Kemel.Orm.Entity.Attributes.SchemaType.AggregateFunction:
                                    this.GenerateTable(directories.Function, item, extension);
                                    break;
                            }
                        }

                        MessageBox.Show("A geração dos arquivos foi concluída");
                    }
                    else
                    {
                        MessageBox.Show("Operação cancelada.");
                    }
                }
            }
        }

        private void SaveCurrentSchema()
        {
            if (this.cklTables.SelectedIndex != -1)
            {
                this.SaveSchema(this.cklTables.SelectedItem as T_Table_Entity);
            }
        }

        private void addTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddTable(Kemel.Orm.Entity.Attributes.SchemaType.Table);
        }

        private void addProcedureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddTable(Kemel.Orm.Entity.Attributes.SchemaType.Procedure);
        }

        private void addViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddTable(Kemel.Orm.Entity.Attributes.SchemaType.View);
        }

        private void addAggregatedFunctionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddTable(Kemel.Orm.Entity.Attributes.SchemaType.AggregateFunction);
        }

        private void addScalarFunctionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddTable(Kemel.Orm.Entity.Attributes.SchemaType.ScalarFunction);
        }

        private void addTableFunctionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddTable(Kemel.Orm.Entity.Attributes.SchemaType.TableFunction);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string FileName { get; private set; }
        public const string FILTER = "Project Entity Generator|*.peg";
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SaveCurrentSchema();
            if (string.IsNullOrEmpty(this.FileName))
            {
                this.saveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                ProjectIO writer = new ProjectIO(this.FileName);
                writer.Save(this.GetSelectedTables());
                MessageBox.Show("File saved", "Kemel.Tools", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = FILTER;
            try
            {                
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    ProjectIO reader = new ProjectIO(ofd.FileName);
                    List<GenItem> lstItens = reader.Load();

                    this.cklTables.Items.Clear();
                    this.TablesDefinitions.Clear();
                    foreach (GenItem table in lstItens)
                    {
                        T_Table_Entity ettTable = new T_Table_Entity();
                        ettTable.NAME = table.Schema.Name;

                        this.TablesDefinitions.Add(ettTable.NAME, table);
                        cklTables.SetItemChecked(cklTables.Items.Add(ettTable), true);
                    }
                    this.FileName = ofd.FileName;
                    this.Text = string.Format("{0} - {1}", this.OriginalCaption, this.FileName);
                    this.txtNamespace.Text = Path.GetFileNameWithoutExtension(this.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Kemel.Tools", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SaveCurrentSchema();
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = FILTER;

            if (!string.IsNullOrEmpty(this.txtNamespace.Text))
            {
                sfd.FileName = string.Format("{0}.peg", this.txtNamespace.Text);
            }
            try
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ProjectIO writer = new ProjectIO(sfd.FileName);
                    writer.Save(this.GetSelectedTables());
                    this.FileName = sfd.FileName;
                    this.Text = string.Format("{0} - {1}", this.OriginalCaption, this.FileName);
                    MessageBox.Show("File saved", "Kemel.Tools", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Kemel.Tools", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}