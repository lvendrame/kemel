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
    public class BusinessGen: BaseGen
    {
        public BusinessGen(GenConfig config)
        {
            base.Config = config;
        }

        public const string BusinessNameSpace = "Business";
        public const string BusinessNamePrefix = "Business";

        public void GenerateBusiness(string nameSpace)
        {
            CodeTypeDeclaration typeDeclaration = this.GetBusinessDeclaration(this.Config.CurrentItem);
            CodeCompileUnit compilerUnit = this.GetUnit(typeDeclaration, nameSpace, this.Config.CurrentItem.Schema.SchemaType);
            this.Config.CurrentItem.Classes.BusinessClass = this.GetStringFromUnit(compilerUnit);
        }

        public void GenerateNormalBusiness()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, BusinessNameSpace);
            this.GenerateBusiness(nameSpace);
        }

        public void GenerateProcedureBusiness()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, ProcedureNameSpace, Punctuation.DOT, BusinessNameSpace);
            this.GenerateBusiness(nameSpace);
        }

        public void GenerateViewBusiness()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, ViewNameSpace, Punctuation.DOT, BusinessNameSpace);
            this.GenerateBusiness(nameSpace);
        }

        public void GenerateFunctionBusiness()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, FunctionNameSpace, Punctuation.DOT, BusinessNameSpace);
            this.GenerateBusiness(nameSpace);
        }

        public CodeTypeDeclaration GetBusinessDeclaration(GenItem item)
        {
            item.Classes.BusinessFileName = string.Concat(BusinessNamePrefix, item.BusinessName);
            CodeTypeDeclaration typeDeclaration = new CodeTypeDeclaration(item.Classes.BusinessFileName);
            CodeTypeReference businessType = new CodeTypeReference(typeof(Business<,>), CodeTypeReferenceOptions.GenericTypeParameter);
            businessType.TypeArguments.Add(string.Concat(item.Schema.Name, EntityGen.EntitySufix));
            businessType.TypeArguments.Add(string.Concat(item.Schema.Name, DalGen.DalSufix));
            typeDeclaration.BaseTypes.Add(businessType);

            return typeDeclaration;
        }

        public override GenType Type
        {
            get { return GenType.Business; }
        }
    }
}
