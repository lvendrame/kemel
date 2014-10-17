using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Kemel.Orm.Providers;
using System.Reflection;
using Kemel.Orm.Starter;
using Kemel.Orm.Data;
using Kemel.Orm.DataBase;
using Kemel.Orm.Providers.Oracle;
using Kemel.Orm.Providers.SqlServer;
using Kemel.Orm.DataBase.CodeDom;

namespace Kemel.Tools.Orm
{
    public partial class FrmProviderSettings : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        public static Languages SelectedLanguage { get; set; }
        public static string OrmNameSpace { get; set; }
        public static bool NewQuery { get; set; }

        public FrmProviderSettings()
        {
            InitializeComponent();
            this.LoadProviders();
            this.rdbNewQuery.Checked = NewQuery;
            this.rdbOldQuery.Checked = !NewQuery;
        }

        public List<Type> Providers { get; set; }
        private void LoadProviders()
        {
            Type tp = typeof(Kemel.Orm.Providers.Oracle.OracleProviderMs);
            tp = typeof(Kemel.Orm.Providers.Oracle.OracleProviderx32);
            tp = typeof(Kemel.Orm.Providers.Oracle.OracleProviderx64);
            tp = typeof(Kemel.Orm.Providers.SqlServer.SqlServerProvider);
            tp = typeof(Kemel.Orm.Providers.MySQL.MySQLProvider);
            tp = typeof(Kemel.Orm.Providers.Postgres.PostgresProvider);

            this.Providers = new List<Type>();
            Type providerType = typeof(Provider);
            this.cboProviderType.ValueMember = "FullName";
            this.cboProviderType.DisplayMember = "Name";

            foreach (AssemblyName asmName in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                FindProviderTypes(providerType, Assembly.Load(asmName).GetExportedTypes());
            }

            this.cboProviderType.DataSource = this.Providers;
        }

        private void FindProviderTypes(Type providerType, Type[] types)
        {
            foreach (Type type in types)
            {
                if (type.IsSubclassOf(providerType))
                {
                    this.Providers.Add(type);
                }
            }
        }

        public OrmStarter GetInstanceOfStarter()
        {
            new ToolsOrmStarter(cboProviderType.SelectedItem as Type).Initialize();
            return Provider.Starter;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Provider.Clear();
            GetInstanceOfStarter();
            Credential credential = Provider.Starter.Credential;
            credential.DataSource = txtDataSource.Text;
            credential.User = txtLogin.Text;
            credential.Password = txtPassword.Text;

            if ((cboProviderType.SelectedItem as Type).Name.Contains("Oracle"))
            {
                credential.DataSource = txtDataSource.Text.Substring(0, txtDataSource.Text.IndexOf("@"));
                credential.Catalog = txtDataSource.Text.Substring(txtDataSource.Text.IndexOf("@") + 1);
                credential.Port = (txtPort.Text == "" ? 0 : int.Parse(txtPort.Text));
            }
            else if ((cboProviderType.SelectedItem as Type).Name.Contains("MySQL"))
            {
                credential.Port = (txtPort.Text == "" ? 0 : int.Parse(txtPort.Text));
            }
            else if ((cboProviderType.SelectedItem as Type).Name.Contains("Postgres"))
            {
                credential.Port = (txtPort.Text == "" ? 0 : int.Parse(txtPort.Text));
            }

            if (Provider.HasProvider)
            {
                Provider.FirstInstance.ResetConnections();
            }

            Provider.Create();
            OrmConnection connection = Provider.FirstInstance.GetConnection();

            try
            {
                connection.Open();
                connection.Close();
                lblStatus.Text = "OK";
                lblStatus.ForeColor = System.Drawing.Color.Green;
                this.ChangeDataBaseControls();
            }
            catch(Exception expErro)
            {
                lblStatus.Text = "Error";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show(expErro.Message);
            }
        }

        private void ChangeDataBaseControls()
        {
            nudConnectionTimeout.Enabled = true;
            txtPort.Enabled = true;
            btnSave.Enabled = true;

            if (!(cboProviderType.SelectedItem as Type).Name.Contains("Oracle"))
            {
                cboDataBase.Enabled = true;

                using (BusinessDataBase objNgDataBase = new BusinessDataBase())
                {
                    cboDataBase.DataSource = objNgDataBase.GetAllDataBase();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Provider.FirstInstance.ResetConnections();

            Credential credential = Provider.Starter.Credential;
            credential.DataSource = txtDataSource.Text;
            credential.User = txtLogin.Text;
            credential.Password = txtPassword.Text;

            if ((cboProviderType.SelectedItem as Type).Name.Contains("Oracle"))
            {
                credential.DataSource = txtDataSource.Text.Substring(0, txtDataSource.Text.IndexOf("@"));
                credential.Catalog = txtDataSource.Text.Substring(txtDataSource.Text.IndexOf("@") + 1);
                credential.Port = (txtPort.Text == "" ? 0 : int.Parse(txtPort.Text));
            }
            else if ((cboProviderType.SelectedItem as Type).Name.Contains("MySQL"))
            {
                credential.Catalog = cboDataBase.SelectedItem.ToString();
                credential.Port = (txtPort.Text == "" ? 0 : int.Parse(txtPort.Text));
            }
            else if ((cboProviderType.SelectedItem as Type).Name.Contains("Postgres"))
            {
                credential.Catalog = cboDataBase.SelectedItem.ToString();
                credential.Port = (txtPort.Text == "" ? 0 : int.Parse(txtPort.Text));
            }
            else
            {
                credential.Catalog = cboDataBase.SelectedItem.ToString();
            }

            credential.ConnectionTimeOut = Convert.ToInt32(nudConnectionTimeout.Value);
            credential.Port = Convert.ToInt32(txtPort.Text);
            credential.AuthenticationMode = AuthenticationMode.SqlUser;
            credential.ApplicationName = "Kemel.Framework.Tools";

            FrmProviderSettings.SelectedLanguage = (cboLanguage.SelectedIndex == 0) ? Languages.CSharp : Languages.VBNet;
            FrmProviderSettings.OrmNameSpace = txtOrmNameSpace.Text;
            FrmProviderSettings.NewQuery = rdbNewQuery.Checked;

            this.Close();
        }

        private void cboProviderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cboProviderType.SelectedItem as Type).Name.Contains("Oracle"))
            {
                txtDataSource.Text = "vmsdigiora01@vmsdigiora01";
                txtPort.Enabled = true;
                txtPort.Text = "1521";
            }
            else if ((cboProviderType.SelectedItem as Type).Name.Contains("MySQL"))
            {
                txtPort.Enabled = true;
                txtPort.Text = "3306";

            }
            else if ((cboProviderType.SelectedItem as Type).Name.Contains("Postgres"))
            {
                txtPort.Enabled = true;
                txtPort.Text = "5432";
            }
            else
            {
                txtPort.Enabled = false;
            }
        }
    }
}
