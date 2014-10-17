namespace Kemel.Tools.Orm
{
    partial class FrmDialogSelectTables
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
            this.components = new System.ComponentModel.Container();
            this.kryptonManager = new ComponentFactory.Krypton.Toolkit.KryptonManager(this.components);
            this.kryptonPanel = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.kryptonHeaderGroup1 = new ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup();
            this.btnOK = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCleanSelection = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSelectAll = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.cklTables = new ComponentFactory.Krypton.Toolkit.KryptonCheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel)).BeginInit();
            this.kryptonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).BeginInit();
            this.kryptonHeaderGroup1.Panel.SuspendLayout();
            this.kryptonHeaderGroup1.SuspendLayout();
            this.SuspendLayout();
            //
            // kryptonManager
            //
            this.kryptonManager.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.Office2007Black;
            //
            // kryptonPanel
            //
            this.kryptonPanel.Controls.Add(this.kryptonHeaderGroup1);
            this.kryptonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel.Name = "kryptonPanel";
            this.kryptonPanel.Size = new System.Drawing.Size(465, 417);
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
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnOK);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnCleanSelection);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnSelectAll);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.cklTables);
            this.kryptonHeaderGroup1.Size = new System.Drawing.Size(465, 417);
            this.kryptonHeaderGroup1.TabIndex = 0;
            //
            // btnOK
            //
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(289, 333);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(163, 25);
            this.btnOK.TabIndex = 3;
            this.btnOK.Values.Text = "&Ok";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            //
            // btnCleanSelection
            //
            this.btnCleanSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCleanSelection.Location = new System.Drawing.Point(152, 333);
            this.btnCleanSelection.Name = "btnCleanSelection";
            this.btnCleanSelection.Size = new System.Drawing.Size(131, 25);
            this.btnCleanSelection.TabIndex = 2;
            this.btnCleanSelection.Values.Text = "&Clean Selection";
            this.btnCleanSelection.Click += new System.EventHandler(this.btnCleanSelection_Click);
            //
            // btnSelectAll
            //
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectAll.Location = new System.Drawing.Point(11, 333);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(135, 25);
            this.btnSelectAll.TabIndex = 1;
            this.btnSelectAll.Values.Text = "Select &All";
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            //
            // cklTables
            //
            this.cklTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cklTables.CheckOnClick = true;
            this.cklTables.Location = new System.Drawing.Point(11, 12);
            this.cklTables.Name = "cklTables";
            this.cklTables.Size = new System.Drawing.Size(441, 315);
            this.cklTables.TabIndex = 0;
            //
            // FrmDialogSelectTables
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 417);
            this.Controls.Add(this.kryptonPanel);
            this.MinimizeBox = false;
            this.Name = "FrmDialogSelectTables";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Select Tables";
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel)).EndInit();
            this.kryptonPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).EndInit();
            this.kryptonHeaderGroup1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).EndInit();
            this.kryptonHeaderGroup1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonManager kryptonManager;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel;
        private ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup kryptonHeaderGroup1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnOK;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCleanSelection;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSelectAll;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckedListBox cklTables;
    }
}

