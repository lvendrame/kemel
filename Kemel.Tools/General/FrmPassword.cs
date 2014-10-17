using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace Kemel.Tools.General
{
    public partial class FrmPassword : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        public FrmPassword()
        {
            InitializeComponent();
            this.rdbHash.Checked = true;
            rdbHash_CheckedChanged(null, null);
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            InitializeCryptography();
            this.txtPassOut.Text = string.IsNullOrEmpty(txtKey.Text) ? Cryptography.EncryptText(this.txtPassIn.Text) : Cryptography.EncryptText(this.txtPassIn.Text, txtKey.Text);
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            InitializeCryptography();
            this.txtPassOut.Text = string.IsNullOrEmpty(txtKey.Text) ? Cryptography.DecryptText(this.txtPassIn.Text) : Cryptography.DecryptText(this.txtPassIn.Text, txtKey.Text);
        }

        private void InitializeCryptography()
        {
            if (rdbHash.Checked)
                Cryptography.HashAlgorithm = (HashAlgorithmOptions)Enum.Parse(typeof(HashAlgorithmOptions), cboAlgoritms.SelectedValue.ToString());
            else
                Cryptography.SymmetricAlgorithm = (SymmetricAlgorithmOptions)Enum.Parse(typeof(SymmetricAlgorithmOptions), cboAlgoritms.SelectedValue.ToString());
        }

        private void rdbHash_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbHash.Checked)
                cboAlgoritms.DataSource = Enum.GetNames(typeof(Kemel.Tools.General.HashAlgorithmOptions));
            else
                cboAlgoritms.DataSource = Enum.GetNames(typeof(Kemel.Tools.General.SymmetricAlgorithmOptions));
        }
    }
}
