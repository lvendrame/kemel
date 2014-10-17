using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Schema;
using System.CodeDom;
using Kemel.Orm.Base;
using Kemel.Orm.Constants;

namespace Kemel.Orm.DataBase.CodeDom
{
    public class DalGen: BaseGen
    {
        public DalGen(GenConfig config)
        {
            base.Config = config;
        }

        public const string DalNameSpace = "Dal";
        public const string DalSufix = "_Dal";

        public void GenerateDal(string nameSpace)
        {
            CodeTypeDeclaration typeDeclaration = this.GetDalDeclaration();
            CodeCompileUnit compilerUnit = this.GetUnit(typeDeclaration, nameSpace, this.Config.CurrentItem.Schema.SchemaType);
            this.Config.CurrentItem.Classes.DalClass = this.GetStringFromUnit(compilerUnit);
        }

        public void GenerateNormalDal()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, DalNameSpace);
            this.GenerateDal(nameSpace);
        }

        public void GenerateProcedureDal()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, ProcedureNameSpace, Punctuation.DOT, DalNameSpace);
            this.GenerateDal(nameSpace);
        }

        public void GenerateViewDal()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, ViewNameSpace, Punctuation.DOT, DalNameSpace);
            this.GenerateDal(nameSpace);
        }

        public void GenerateFunctionDal()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, FunctionNameSpace, Punctuation.DOT, DalNameSpace);
            this.GenerateDal(nameSpace);
        }

        public CodeTypeDeclaration GetDalDeclaration()
        {
            TableSchema tableSchema = this.Config.CurrentItem.Schema;
            this.Config.CurrentItem.Classes.DalFileName = string.Concat(tableSchema.Name, DalSufix);
            CodeTypeDeclaration typeDeclaration = new CodeTypeDeclaration(this.Config.CurrentItem.Classes.DalFileName);
            CodeTypeReference dalType = new CodeTypeReference(typeof(Dal<>), CodeTypeReferenceOptions.GenericTypeParameter);
            dalType.TypeArguments.Add(string.Concat(tableSchema.Name, EntityGen.EntitySufix));
            typeDeclaration.BaseTypes.Add(dalType);

            return typeDeclaration;
        }

        public override GenType Type
        {
            get { return GenType.Dal; }
        }
    }
}
