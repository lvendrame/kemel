namespace Kemel.Tools.Orm
{
    partial class FrmProviderSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmProviderSettings));
            this.kryptonPanel = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.kryptonHeaderGroup1 = new ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup();
            this.rdbNewQuery = new System.Windows.Forms.RadioButton();
            this.rdbOldQuery = new System.Windows.Forms.RadioButton();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.nudConnectionTimeout = new ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown();
            this.btnTest = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtOrmNameSpace = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txtPort = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txtPassword = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txtLogin = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txtDataSource = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.lblConnectionTimeOut = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lblPort = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lblPassword = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lblLogin = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lblDataSource = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.cboLanguage = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.cboDataBase = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.cboProviderType = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.lblDataBase = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lblProviderType = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel)).BeginInit();
            this.kryptonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).BeginInit();
            this.kryptonHeaderGroup1.Panel.SuspendLayout();
            this.kryptonHeaderGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboLanguage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDataBase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProviderType)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            //
            // kryptonPanel
            //
            this.kryptonPanel.Controls.Add(this.kryptonHeaderGroup1);
            this.kryptonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel.Name = "kryptonPanel";
            this.kryptonPanel.Size = new System.Drawing.Size(616, 382);
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
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.groupBox1);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.lblStatus);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.label1);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnSave);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.nudConnectionTimeout);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnTest);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.txtOrmNameSpace);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.txtPort);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.txtPassword);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.txtLogin);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.txtDataSource);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.lblConnectionTimeOut);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.kryptonLabel1);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.kryptonLabel2);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.lblPort);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.lblPassword);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.lblLogin);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.lblDataSource);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.cboLanguage);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.cboDataBase);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.cboProviderType);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.lblDataBase);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.lblProviderType);
            this.kryptonHeaderGroup1.Size = new System.Drawing.Size(616, 382);
            this.kryptonHeaderGroup1.TabIndex = 0;
            this.kryptonHeaderGroup1.ValuesPrimary.Heading = "Settings";
            this.kryptonHeaderGroup1.ValuesSecondary.Heading = "Provider Settings";
            //
            // rdbNewQuery
            //
            this.rdbNewQuery.AutoSize = true;
            this.rdbNewQuery.BackColor = System.Drawing.SystemColors.Window;
            this.rdbNewQuery.Location = new System.Drawing.Point(61, 19);
            this.rdbNewQuery.Name = "rdbNewQuery";
            this.rdbNewQuery.Size = new System.Drawing.Size(43, 17);
            this.rdbNewQuery.TabIndex = 22;
            this.rdbNewQuery.Text = "Yes";
            this.rdbNewQuery.UseVisualStyleBackColor = false;
            //
            // rdbOldQuery
            //
            this.rdbOldQuery.AutoSize = true;
            this.rdbOldQuery.BackColor = System.Drawing.SystemColors.Window;
            this.rdbOldQuery.Checked = true;
            this.rdbOldQuery.Location = new System.Drawing.Point(6, 19);
            this.rdbOldQuery.Name = "rdbOldQuery";
            this.rdbOldQuery.Size = new System.Drawing.Size(39, 17);
            this.rdbOldQuery.TabIndex = 22;
            this.rdbOldQuery.TabStop = true;
            this.rdbOldQuery.Text = "No";
            this.rdbOldQuery.UseVisualStyleBackColor = false;
            //
            // lblStatus
            //
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Location = new System.Drawing.Point(55, 243);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(77, 13);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "-";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(12, 243);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Status:";
            //
            // btnSave
            //
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(463, 270);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 25);
            this.btnSave.TabIndex = 21;
            this.btnSave.Values.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // nudConnectionTimeout
            //
            this.nudConnectionTimeout.Enabled = false;
            this.nudConnectionTimeout.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudConnectionTimeout.Location = new System.Drawing.Point(328, 31);
            this.nudConnectionTimeout.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudConnectionTimeout.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudConnectionTimeout.Name = "nudConnectionTimeout";
            this.nudConnectionTimeout.Size = new System.Drawing.Size(120, 22);
            this.nudConnectionTimeout.TabIndex = 14;
            this.nudConnectionTimeout.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            //
            // btnTest
            //
            this.btnTest.Location = new System.Drawing.Point(146, 213);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(131, 25);
            this.btnTest.TabIndex = 8;
            this.btnTest.Values.Text = "Test Connection";
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            //
            // txtOrmNameSpace
            //
            this.txtOrmNameSpace.Location = new System.Drawing.Point(327, 190);
            this.txtOrmNameSpace.Name = "txtOrmNameSpace";
            this.txtOrmNameSpace.Size = new System.Drawing.Size(266, 20);
            this.txtOrmNameSpace.TabIndex = 20;
            this.txtOrmNameSpace.Text = "Kemel.Orm";
            //
            // txtPort
            //
            this.txtPort.Enabled = false;
            this.txtPort.Location = new System.Drawing.Point(327, 82);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(266, 20);
            this.txtPort.TabIndex = 16;
            this.txtPort.Text = "1521";
            //
            // txtPassword
            //
            this.txtPassword.Location = new System.Drawing.Point(11, 187);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(266, 20);
            this.txtPassword.TabIndex = 7;
            this.txtPassword.Text = "x!rsa5)f";
            //
            // txtLogin
            //
            this.txtLogin.Location = new System.Drawing.Point(11, 135);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(266, 20);
            this.txtLogin.TabIndex = 5;
            this.txtLogin.Text = "sa";
            //
            // txtDataSource
            //
            this.txtDataSource.Location = new System.Drawing.Point(11, 83);
            this.txtDataSource.Name = "txtDataSource";
            this.txtDataSource.Size = new System.Drawing.Size(266, 20);
            this.txtDataSource.TabIndex = 3;
            this.txtDataSource.Text = "vmsdigisql01\\dev";
            //
            // lblConnectionTimeOut
            //
            this.lblConnectionTimeOut.Location = new System.Drawing.Point(328, 4);
            this.lblConnectionTimeOut.Name = "lblConnectionTimeOut";
            this.lblConnectionTimeOut.Size = new System.Drawing.Size(127, 20);
            this.lblConnectionTimeOut.TabIndex = 13;
            this.lblConnectionTimeOut.Values.Text = "Connection Time Out";
            //
            // kryptonLabel1
            //
            this.kryptonLabel1.Location = new System.Drawing.Point(328, 111);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(64, 20);
            this.kryptonLabel1.TabIndex = 17;
            this.kryptonLabel1.Values.Text = "Language";
            //
            // kryptonLabel2
            //
            this.kryptonLabel2.Location = new System.Drawing.Point(328, 164);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(103, 20);
            this.kryptonLabel2.TabIndex = 19;
            this.kryptonLabel2.Values.Text = "Orm NameSpace";
            //
            // lblPort
            //
            this.lblPort.Location = new System.Drawing.Point(328, 56);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(33, 20);
            this.lblPort.TabIndex = 15;
            this.lblPort.Values.Text = "Port";
            //
            // lblPassword
            //
            this.lblPassword.Location = new System.Drawing.Point(12, 161);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(62, 20);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Values.Text = "Password";
            //
            // lblLogin
            //
            this.lblLogin.Location = new System.Drawing.Point(11, 109);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(41, 20);
            this.lblLogin.TabIndex = 4;
            this.lblLogin.Values.Text = "Login";
            //
            // lblDataSource
            //
            this.lblDataSource.Location = new System.Drawing.Point(11, 57);
            this.lblDataSource.Name = "lblDataSource";
            this.lblDataSource.Size = new System.Drawing.Size(73, 20);
            this.lblDataSource.TabIndex = 2;
            this.lblDataSource.Values.Text = "DataSource";
            //
            // cboLanguage
            //
            this.cboLanguage.DropDownWidth = 266;
            this.cboLanguage.Items.AddRange(new object[] {
            "C#",
            "VB.Net"});
            this.cboLanguage.Location = new System.Drawing.Point(327, 137);
            this.cboLanguage.Name = "cboLanguage";
            this.cboLanguage.Size = new System.Drawing.Size(266, 21);
            this.cboLanguage.TabIndex = 18;
            this.cboLanguage.Text = "Select...";
            //
            // cboDataBase
            //
            this.cboDataBase.DropDownWidth = 266;
            this.cboDataBase.Enabled = false;
            this.cboDataBase.Location = new System.Drawing.Point(11, 296);
            this.cboDataBase.Name = "cboDataBase";
            this.cboDataBase.Size = new System.Drawing.Size(266, 21);
            this.cboDataBase.TabIndex = 12;
            this.cboDataBase.Text = "Select...";
            //
            // cboProviderType
            //
            this.cboProviderType.DropDownWidth = 266;
            this.cboProviderType.Location = new System.Drawing.Point(11, 30);
            this.cboProviderType.Name = "cboProviderType";
            this.cboProviderType.Size = new System.Drawing.Size(266, 21);
            this.cboProviderType.TabIndex = 1;
            this.cboProviderType.Text = "Select...";
            this.cboProviderType.SelectedIndexChanged += new System.EventHandler(this.cboProviderType_SelectedIndexChanged);
            //
            // lblDataBase
            //
            this.lblDataBase.Location = new System.Drawing.Point(11, 270);
            this.lblDataBase.Name = "lblDataBase";
            this.lblDataBase.Size = new System.Drawing.Size(61, 20);
            this.lblDataBase.TabIndex = 11;
            this.lblDataBase.Values.Text = "DataBase";
            //
            // lblProviderType
            //
            this.lblProviderType.Location = new System.Drawing.Point(11, 4);
            this.lblProviderType.Name = "lblProviderType";
            this.lblProviderType.Size = new System.Drawing.Size(85, 20);
            this.lblProviderType.TabIndex = 0;
            this.lblProviderType.Values.Text = "Provider Type";
            //
            // groupBox1
            //
            this.groupBox1.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox1.Controls.Add(this.rdbOldQuery);
            this.groupBox1.Controls.Add(this.rdbNewQuery);
            this.groupBox1.Location = new System.Drawing.Point(327, 215);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(266, 49);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "New Query";
            //
            // FrmProviderSettings
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 382);
            this.Controls.Add(this.kryptonPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmProviderSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Provider Settings";
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel)).EndInit();
            this.kryptonPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).EndInit();
            this.kryptonHeaderGroup1.Panel.ResumeLayout(false);
            this.kryptonHeaderGroup1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).EndInit();
            this.kryptonHeaderGroup1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboLanguage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDataBase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProviderType)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel;
        private ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup kryptonHeaderGroup1;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox cboProviderType;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProviderType;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDataSource;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDataSource;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnTest;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPassword;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLogin;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPassword;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLogin;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox cboDataBase;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDataBase;
        private ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown nudConnectionTimeout;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblConnectionTimeOut;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPort;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPort;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSave;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox cboLanguage;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label1;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtOrmNameSpace;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private System.Windows.Forms.RadioButton rdbOldQuery;
        private System.Windows.Forms.RadioButton rdbNewQuery;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

