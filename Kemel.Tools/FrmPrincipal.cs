using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace Kemel.Tools
{
    public partial class FrmPrincipal : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void providerSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddForm(new Orm.FrmProviderSettings());
        }

        public KryptonForm AddForm(KryptonForm form)
        {
            this.AddOwnedForm(form);
            //this.TextExtra = form.Text;
            //form.MdiParent = this;
            form.Show();
            return form;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void entityGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddForm(new Orm.FrmEntityGenerator());
        }

        private void passwordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddForm(new General.FrmPassword());
        }
    }
}
