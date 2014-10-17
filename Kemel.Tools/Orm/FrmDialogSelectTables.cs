using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Kemel.Orm.DataBase;

namespace Kemel.Tools.Orm
{
    public partial class FrmDialogSelectTables : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        public FrmDialogSelectTables()
        {
            InitializeComponent();

            using (BusinessDataBase objNgDataBase = new BusinessDataBase())
            {
               List<T_Table_Entity> lstTable = objNgDataBase.GetTableList();

               foreach (T_Table_Entity table in lstTable)
               {
                   cklTables.Items.Add(table);
               }
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < cklTables.Items.Count; i++)
            {
                cklTables.SetItemChecked(i, true);
            }
        }

        private void btnCleanSelection_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < cklTables.Items.Count; i++)
            {
                cklTables.SetItemChecked(i, false);
            }
        }

        public List<T_Table_Entity> SelectedTables { get; private set; }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.SelectedTables = new List<T_Table_Entity>();
            foreach (int selIndex in cklTables.CheckedIndices)
            {
                this.SelectedTables.Add(cklTables.Items[selIndex] as T_Table_Entity);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        internal int IndexOfTable(T_Table_Entity tbItem)
        {
            for (int i = 0; i < this.SelectedTables.Count; i++)
            {
                T_Table_Entity item = this.SelectedTables[i];
                if (item.NAME.Equals(tbItem.NAME))
                    return i;
            }
            return -1;
        }
    }
}
