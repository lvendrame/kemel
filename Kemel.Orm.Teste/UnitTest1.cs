using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using Kemel.Orm.Teste.Bll.Entity;
using Kemel.Orm.Teste.Bll.Business;
using Kemel.Orm.NQuery.Storage;

namespace Kemel.Orm.Teste
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            Initialize();
        }

        public static void Initialize()
        {
            Kemel.Orm.OrmInitializer.Initialize();

            //Kemel.Orm.Starter.Windows.SqlServer2005Starter sqlStarter = new Kemel.Orm.Starter.Windows.SqlServer2005Starter();
            //sqlStarter.Initialize();

            //sqlStarter.Credential.ApplicationName = "Kemel.Orm.Teste";
            //sqlStarter.Credential.AuthenticationMode = Kemel.Orm.Providers.AuthenticationMode.SqlUser;
            //sqlStarter.Credential.Catalog = "DIGI_TESTE";
            //sqlStarter.Credential.DataSource = "VMSDIGISQL01\\DEV";
            //sqlStarter.Credential.User = "sa";
            //sqlStarter.Credential.Password = "x!rsa5)f";

            //Kemel.Orm.Providers.Provider.Create();
        }

        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Dummy()
        {
        }

        [TestMethod]
        public void InsertSimples()
        {
            T_PESSOA_Entity ett = new T_PESSOA_Entity();
            ett.NOME = Kemel.Util.Test.Names.GetFullName();
            ett.INATIVO = false;

            BusinessPessoa bllPessoa = new BusinessPessoa();
            bllPessoa.Dal.Insert(ett);
        }

        [TestMethod]
        public void UpdateSimples()
        {
            Random rd = new Random();

            BusinessPessoa bllPessoa = new BusinessPessoa();

            T_PESSOA_Entity ett = bllPessoa.Dal.ReadById(9);
            ett.NOME = Kemel.Util.Test.Names.GetFullName();
            bllPessoa.Dal.Update(ett);
        }

        [TestMethod]
        public void DeleteLogicoSimples()
        {
            BusinessPessoa bllPessoa = new BusinessPessoa();

            bllPessoa.Dal.DeleteById(12);
        }

        [TestMethod]
        public void SelecionarTodoItems()
        {
            BusinessPessoa bllPessoa = new BusinessPessoa();

            List<T_PESSOA_Entity> lstPessoas = bllPessoa.Dal.ReadAll();
        }

        [TestMethod]
        public void InsertComRef()
        {
            Random rd = new Random();

            T_TIPO_EMPRESA_Entity ettTE = new T_TIPO_EMPRESA_Entity();
            ettTE.DESCRICAO = "Tipo Empresa " + rd.Next().ToString();

            BusinessTipoEmpresa bllTipoEmpresa = new BusinessTipoEmpresa();
            bllTipoEmpresa.Dal.Insert(ettTE);

            T_EMPRESA_Entity ettEm = new T_EMPRESA_Entity();
            ettEm.NOME = Kemel.Util.Test.Names.GetCompanyName();
            ettEm.DESCRICAO = "Descrição da Empresa " + ettEm.NOME;
            ettEm.TIPO_EMPRESA = ettTE.CODIGO;
            ettEm.INATIVO = false;

            BusinessEmpresa bllEmpresa = new BusinessEmpresa();
            bllEmpresa.Dal.Insert(ettEm);
        }

        [TestMethod]
        public void Delete()
        {
            BusinessTipoEmpresa bllTipoEmpresa = new BusinessTipoEmpresa();

            bllTipoEmpresa.Dal.DeleteById(2);
        }

        [TestMethod]
        public void QuerySimples()
        {
            BusinessPessoa bllPessoa = new BusinessPessoa();

            DataTable dtPessoas = bllPessoa.QuerySimples();
        }

        [TestMethod]
        public void QueryComRepeticaoDeTabelaNoJoin()
        {
            BusinessPessoa bllPessoa = new BusinessPessoa();

            DataTable dtPessoas = bllPessoa.QueryComRepeticaoDeTabelaNoJoin();
        }

        [TestMethod]
        public void ExtendedProperties()
        {
            BusinessPessoa bllPessoa = new BusinessPessoa();

            List<T_EMPRESA_Entity> lstA = bllPessoa.ExtendedProperties();
        }

        [TestMethod]
        public void DataTableJoin()
        {
            BusinessPessoa bllPessoa = new BusinessPessoa();

            DataTable dtA = bllPessoa.DataTableJoin();
        }

        [TestMethod]
        public void QuerySimplesComLimit()
        {
            BusinessPessoa bllPessoa = new BusinessPessoa();

            DataTable dtPessoas = bllPessoa.QuerySimplesComLimit();
        }

        [TestMethod]
        public void GeradorCPF()
        {
            string dig = Kemel.Util.Test.Documents.GenerateCPFDigits("017749449");
            Assert.AreEqual(dig, "28");

            Assert.IsTrue(Kemel.Util.Test.Documents.ValidateCPF("017.749.449-28"));

            Assert.AreEqual(Kemel.Util.Test.Documents.GenerateCPF().Length, 14);
        }

        [TestMethod]
        public void GeradorCNPJ()
        {
            string dig = Kemel.Util.Test.Documents.GenerateCNPJDigits("336831110001");
            Assert.AreEqual(dig, "07");

            Assert.IsTrue(Kemel.Util.Test.Documents.ValidateCNPJ("33.683.111/0001-07"));

            Assert.AreEqual(Kemel.Util.Test.Documents.GenerateCNPJ().Length, 18);
        }

        [TestMethod]
        public void Cidades()
        {
            Kemel.Util.Test.States state = Kemel.Util.Test.Address.GetRandomState();

            string name = Kemel.Util.Test.Address.GetStateName(state);

            string cidade = Kemel.Util.Test.Address.GetAnyCityFromState(state);
        }

        [TestMethod]
        public void Telefones()
        {
            Kemel.Util.Test.States state = Kemel.Util.Test.Address.GetRandomState();

            int ddd = Kemel.Util.Test.Telephone.GetAnyDDD();
            ddd = Kemel.Util.Test.Telephone.GetAnyDDDFromState(state);

            string phone = Kemel.Util.Test.Telephone.GenerateTelephoneNumber();
            phone = Kemel.Util.Test.Telephone.GenerateCompleteTelephoneNumber();
            phone = Kemel.Util.Test.Telephone.GenerateCompleteTelephoneNumberFromDDD(ddd);
            phone = Kemel.Util.Test.Telephone.GenerateCompleteTelephoneNumberFromState(state);

            phone = Kemel.Util.Test.Telephone.GenerateCellphoneNumber(ddd);
            phone = Kemel.Util.Test.Telephone.GenerateCellphoneNumber(11);
            phone = Kemel.Util.Test.Telephone.GenerateCompleteCellphoneNumber();
            phone = Kemel.Util.Test.Telephone.GenerateCompleteCellphoneNumberFromState(state);
            phone = Kemel.Util.Test.Telephone.GenerateCompleteCellphoneNumberFromDDD(ddd);
            phone = Kemel.Util.Test.Telephone.GenerateCompleteCellphoneNumberFromDDD(11);
        }

        [TestMethod]
        public void Endereco()
        {
            string aux = Kemel.Util.Test.Address.GenerateZipNumber();
            aux = Kemel.Util.Test.Address.GenerateStreetName();
        }

        [TestMethod]
        public void EntidadeTeste()
        {
            Kemel.Util.Test.EntityTest test = new Kemel.Util.Test.EntityTest();
            test.ToString();
        }

        [TestMethod]
        public void LoremIpsum()
        {
            string aux = Kemel.Util.Test.Others.GenerateLoremIpsum(30);
            Assert.AreEqual(aux.Length, 30);

            aux = Kemel.Util.Test.Others.GenerateLoremIpsum(15000);
            Assert.AreEqual(aux.Length, 15000);

            aux = Kemel.Util.Test.Others.GenerateLoremIpsumParagraphs(5);
            //Assert.AreEqual(aux.Length, 15000);

            aux = Kemel.Util.Test.Others.GenerateLoremIpsumParagraphs(30, 30);
            //Assert.AreEqual(aux.Length, 15000);
        }
    }
}
