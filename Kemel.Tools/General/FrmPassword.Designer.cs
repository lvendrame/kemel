namespace Kemel.Tools.General
{
    partial class FrmPassword
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.kryptonPanel = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.kryptonHeaderGroup1 = new ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup();
            this.rdbSymmetric = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rdbHash = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.cboAlgoritms = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.btnDecrypt = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnEncrypt = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.lblPassword = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.txtPassOut = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txtPassIn = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txtKey = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel)).BeginInit();
            this.kryptonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).BeginInit();
            this.kryptonHeaderGroup1.Panel.SuspendLayout();
            this.kryptonHeaderGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlgoritms)).BeginInit();
            this.SuspendLayout();
            //
            // kryptonPanel
            //
            this.kryptonPanel.Controls.Add(this.kryptonHeaderGroup1);
            this.kryptonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel.Name = "kryptonPanel";
            this.kryptonPanel.Size = new System.Drawing.Size(439, 411);
            this.kryptonPanel.TabIndex = 0;
            //
            // kryptonHeaderGroup1
            //
            this.kryptonHeaderGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonHeaderGroup1.Location = new System.Drawing.Point(0, 0);
            this.kryptonHeaderGroup1.Name = "kryptonHeaderGroup1";
            //
            // kryptonHeaderGroup1.Panel
            //
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.txtKey);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.rdbSymmetric);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.rdbHash);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.cboAlgoritms);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnDecrypt);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnEncrypt);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.lblPassword);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.txtPassOut);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.txtPassIn);
            this.kryptonHeaderGroup1.Size = new System.Drawing.Size(439, 411);
            this.kryptonHeaderGroup1.TabIndex = 0;
            //
            // rdbSymmetric
            //
            this.rdbSymmetric.Location = new System.Drawing.Point(80, 55);
            this.rdbSymmetric.Name = "rdbSymmetric";
            this.rdbSymmetric.Size = new System.Drawing.Size(75, 19);
            this.rdbSymmetric.TabIndex = 3;
            this.rdbSymmetric.Values.Text = "Symmetric";
            //
            // rdbHash
            //
            this.rdbHash.Location = new System.Drawing.Point(11, 55);
            this.rdbHash.Name = "rdbHash";
            this.rdbHash.Size = new System.Drawing.Size(47, 19);
            this.rdbHash.TabIndex = 2;
            this.rdbHash.Values.Text = "Hash";
            this.rdbHash.CheckedChanged += new System.EventHandler(this.rdbHash_CheckedChanged);
            //
            // cboAlgoritms
            //
            this.cboAlgoritms.DropDownWidth = 260;
            this.cboAlgoritms.Location = new System.Drawing.Point(166, 55);
            this.cboAlgoritms.Name = "cboAlgoritms";
            this.cboAlgoritms.Size = new System.Drawing.Size(260, 21);
            this.cboAlgoritms.TabIndex = 4;
            //
            // btnDecrypt
            //
            this.btnDecrypt.Location = new System.Drawing.Point(243, 106);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(183, 25);
            this.btnDecrypt.TabIndex = 7;
            this.btnDecrypt.Values.Text = "Decrypt";
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            //
            // btnEncrypt
            //
            this.btnEncrypt.Location = new System.Drawing.Point(11, 106);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(183, 25);
            this.btnEncrypt.TabIndex = 6;
            this.btnEncrypt.Values.Text = "Encrypt";
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            //
            // lblPassword
            //
            this.lblPassword.Location = new System.Drawing.Point(11, 3);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(60, 19);
            this.lblPassword.TabIndex = 0;
            this.lblPassword.Values.Text = "Password:";
            //
            // txtPassOut
            //
            this.txtPassOut.Location = new System.Drawing.Point(11, 137);
            this.txtPassOut.Name = "txtPassOut";
            this.txtPassOut.Size = new System.Drawing.Size(415, 20);
            this.txtPassOut.TabIndex = 8;
            //
            // txtPassIn
            //
            this.txtPassIn.Location = new System.Drawing.Point(11, 29);
            this.txtPassIn.Name = "txtPassIn";
            this.txtPassIn.Size = new System.Drawing.Size(415, 20);
            this.txtPassIn.TabIndex = 1;
            //
            // txtKey
            //
            this.txtKey.Location = new System.Drawing.Point(11, 80);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(415, 20);
            this.txtKey.TabIndex = 5;
            //
            // FrmPassword
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 411);
            this.Controls.Add(this.kryptonPanel);
            this.Name = "FrmPassword";
            this.Text = "FrmPassword";
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel)).EndInit();
            this.kryptonPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).EndInit();
            this.kryptonHeaderGroup1.Panel.ResumeLayout(false);
            this.kryptonHeaderGroup1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).EndInit();
            this.kryptonHeaderGroup1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboAlgoritms)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel;
        private ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup kryptonHeaderGroup1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEncrypt;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPassword;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPassIn;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rdbSymmetric;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rdbHash;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox cboAlgoritms;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDecrypt;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPassOut;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtKey;
    }
}

