using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Schema;

namespace Kemel.Orm.DataBase.CodeDom
{
    public class GenConfig
    {
        public GenConfig()
        {

        }

        public string NameSpace { get; set; }

        public List<GenItem> Tables { get; set; }

        //public List<GenItem> Procedures { get; set; }

        //public List<GenItem> Views { get; set; }

        //public List<GenItem> Functions { get; set; }

        public Languages Language { get; set; }

        private string strOrmNameSpace = "Kemel.Orm";
        public String OrmNameSpace {
            get
            {
                return strOrmNameSpace;
            }
            set
            {
                strOrmNameSpace = value;
            }
        }

        public bool NewQuery { get; set; }

        //public bool HasProcedure
        //{
        //    get
        //    {
        //        return this.Procedures != null && this.Procedures.Count > 0;
        //    }
        //}

        //public bool HasView
        //{
        //    get
        //    {
        //        return this.Views != null && this.Views.Count > 0;
        //    }
        //}

        //public bool HasFunction
        //{
        //    get
        //    {
        //        return this.Functions != null && this.Functions.Count > 0;
        //    }
        //}

        public GenItem CurrentItem { get; set; }


        public void GenerateCodes()
        {
            EntityGen entityGen = new EntityGen(this);
            SchemaGen schemaGen = new SchemaGen(this);
            DalGen dalGen = new DalGen(this);
            BusinessGen businessGen = new BusinessGen(this);

            for (int i = 0; i < this.Tables.Count; i++)
            {
                this.CurrentItem = this.Tables[i];
                switch (this.CurrentItem.Schema.SchemaType)
                {
                    case Kemel.Orm.Entity.Attributes.SchemaType.None:
                    case Kemel.Orm.Entity.Attributes.SchemaType.Table:
                        GenerateTableClasses(entityGen, schemaGen, dalGen, businessGen);
                        break;
                    case Kemel.Orm.Entity.Attributes.SchemaType.View:
                        GenerateViewClasses(entityGen, schemaGen, dalGen, businessGen);
                        break;
                    case Kemel.Orm.Entity.Attributes.SchemaType.Procedure:
                        GenerateProcedureClasses(entityGen, schemaGen, dalGen, businessGen);
                        break;
                    case Kemel.Orm.Entity.Attributes.SchemaType.ScalarFunction:
                    case Kemel.Orm.Entity.Attributes.SchemaType.TableFunction:
                    case Kemel.Orm.Entity.Attributes.SchemaType.AggregateFunction:
                        GenerateFunctionClasses(entityGen, schemaGen, dalGen, businessGen);
                        break;
                }
            }
        }

        private static void GenerateTableClasses(EntityGen entityGen, SchemaGen schemaGen, DalGen dalGen, BusinessGen businessGen)
        {
            entityGen.GenerateNormalEntity();
            entityGen.GenerateEntityDefinition();
            entityGen.GenerateEntityExtension();
            schemaGen.GenerateNormalSchema();
            dalGen.GenerateNormalDal();
            businessGen.GenerateNormalBusiness();
        }

        private static void GenerateViewClasses(EntityGen entityGen, SchemaGen schemaGen, DalGen dalGen, BusinessGen businessGen)
        {
            entityGen.GenerateViewEntity();
            entityGen.GenerateViewEntityDefinition();
            entityGen.GenerateViewEntityExtension();
            schemaGen.GenerateViewSchema();
            dalGen.GenerateViewDal();
            businessGen.GenerateViewBusiness();
        }

        private static void GenerateProcedureClasses(EntityGen entityGen, SchemaGen schemaGen, DalGen dalGen, BusinessGen businessGen)
        {
            entityGen.GenerateProcedureEntity();
            entityGen.GenerateProcedureEntityDefinition();
            entityGen.GenerateProcedureEntityExtension();
            schemaGen.GenerateProcedureSchema();
            dalGen.GenerateProcedureDal();
            businessGen.GenerateProcedureBusiness();
        }

        private static void GenerateFunctionClasses(EntityGen entityGen, SchemaGen schemaGen, DalGen dalGen, BusinessGen businessGen)
        {
            entityGen.GenerateFunctionEntity();
            entityGen.GenerateFunctionEntityDefinition();
            entityGen.GenerateFunctionEntityExtension();
            schemaGen.GenerateFunctionSchema();
            dalGen.GenerateFunctionDal();
            businessGen.GenerateFunctionBusiness();
        }
    }
}
