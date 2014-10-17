using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Constants;
using System.CodeDom.Compiler;
using System.IO;
using Kemel.Orm.Entity.Attributes;

namespace Kemel.Orm.DataBase.CodeDom
{
    public abstract class BaseGen
    {
        public const string ProcedureNameSpace = "Procedure";
        public const string ViewNameSpace = "View";
        public const string FunctionNameSpace = "Function";


        public abstract GenType Type { get; }

        public GenConfig Config { get; set; }

        protected void AddImports(CodeNamespaceImportCollection codenamespaceImportCollection, SchemaType schemaType)
        {
            codenamespaceImportCollection.Add(new CodeNamespaceImport("System"));
            codenamespaceImportCollection.Add(new CodeNamespaceImport("System.Collections.Generic"));
            codenamespaceImportCollection.Add(new CodeNamespaceImport("System.Data"));
            codenamespaceImportCollection.Add(new CodeNamespaceImport("System.Text"));
            codenamespaceImportCollection.Add(new CodeNamespaceImport(this.Config.OrmNameSpace));
            codenamespaceImportCollection.Add(new CodeNamespaceImport(string.Concat(this.Config.OrmNameSpace, ".Base")));
            codenamespaceImportCollection.Add(new CodeNamespaceImport(string.Concat(this.Config.OrmNameSpace, ".Schema")));

            string nameSpace = this.Config.NameSpace;
            switch (schemaType)
            {
                case SchemaType.View:
                    nameSpace = string.Concat(nameSpace, Punctuation.DOT, ViewNameSpace);
                    break;
                case SchemaType.Procedure:
                    nameSpace = string.Concat(nameSpace, Punctuation.DOT, ProcedureNameSpace);
                    break;
                case SchemaType.ScalarFunction:
                case SchemaType.TableFunction:
                case SchemaType.AggregateFunction:
                    nameSpace = string.Concat(nameSpace, Punctuation.DOT, FunctionNameSpace);
                    break;
            }

            switch (this.Type)
            {
                case GenType.Entity:
                    codenamespaceImportCollection.Add(new CodeNamespaceImport(string.Concat(this.Config.OrmNameSpace, ".Entity.Attributes")));
                    break;
                case GenType.Dal:
                    codenamespaceImportCollection.Add(new CodeNamespaceImport(string.Concat(this.Config.OrmNameSpace, ".Data")));
                    codenamespaceImportCollection.Add(new CodeNamespaceImport(string.Concat(this.Config.OrmNameSpace, ".Providers")));
                    codenamespaceImportCollection.Add(new CodeNamespaceImport(string.Concat(this.Config.OrmNameSpace, this.Config.NewQuery ? ".NQuery" : ".QueryDef")));

                    codenamespaceImportCollection.Add(new CodeNamespaceImport(string.Concat(nameSpace, Punctuation.DOT, EntityGen.EntityNameSpace)));

                    //if(this.Config.HasProcedure)
                    //    codenamespaceImportCollection.Add(new CodeNamespaceImport(string.Concat(nameSpace, Punctuation.DOT, ProcedureNameSpace, Punctuation.DOT, EntityGen.EntityNameSpace)));

                    //if (this.Config.HasView)
                    //    codenamespaceImportCollection.Add(new CodeNamespaceImport(string.Concat(nameSpace, Punctuation.DOT, ViewNameSpace, Punctuation.DOT, EntityGen.EntityNameSpace)));

                    //if (this.Config.HasFunction)
                    //    codenamespaceImportCollection.Add(new CodeNamespaceImport(string.Concat(nameSpace, Punctuation.DOT, FunctionNameSpace, Punctuation.DOT, EntityGen.EntityNameSpace)));
                    break;
                case GenType.Business:
                    codenamespaceImportCollection.Add(new CodeNamespaceImport(string.Concat(this.Config.OrmNameSpace, ".Data")));
                    codenamespaceImportCollection.Add(new CodeNamespaceImport(string.Concat(this.Config.OrmNameSpace, ".Providers")));
                    codenamespaceImportCollection.Add(new CodeNamespaceImport(string.Concat(this.Config.OrmNameSpace, this.Config.NewQuery ? ".NQuery" : ".QueryDef")));

                    codenamespaceImportCollection.Add(new CodeNamespaceImport(string.Concat(nameSpace, Punctuation.DOT, EntityGen.EntityNameSpace)));
                    codenamespaceImportCollection.Add(new CodeNamespaceImport(string.Concat(nameSpace, Punctuation.DOT, DalGen.DalNameSpace)));

                    //if (this.Config.HasProcedure)
                    //{
                    //    codenamespaceImportCollection.Add(new CodeNamespaceImport(string.Concat(nameSpace, Punctuation.DOT, ProcedureNameSpace, Punctuation.DOT, EntityGen.EntityNameSpace)));
                    //    codenamespaceImportCollection.Add(new CodeNamespaceImport(string.Concat(nameSpace, Punctuation.DOT, ProcedureNameSpace, Punctuation.DOT, DalGen.DalNameSpace)));
                    //}

                    //if (this.Config.HasView)
                    //{
                    //    codenamespaceImportCollection.Add(new CodeNamespaceImport(string.Concat(nameSpace, Punctuation.DOT, ViewNameSpace, Punctuation.DOT, EntityGen.EntityNameSpace)));
                    //    codenamespaceImportCollection.Add(new CodeNamespaceImport(string.Concat(nameSpace, Punctuation.DOT, ViewNameSpace, Punctuation.DOT, DalGen.DalNameSpace)));
                    //}

                    //if (this.Config.HasFunction)
                    //{
                    //    codenamespaceImportCollection.Add(new CodeNamespaceImport(string.Concat(nameSpace, Punctuation.DOT, FunctionNameSpace, Punctuation.DOT, EntityGen.EntityNameSpace)));
                    //    codenamespaceImportCollection.Add(new CodeNamespaceImport(string.Concat(nameSpace, Punctuation.DOT, FunctionNameSpace, Punctuation.DOT, DalGen.DalNameSpace)));
                    //}
                    break;
            }
        }

        public string BaseNameSpace {
            get
            {
                return Config.Language == Languages.CSharp ? this.Config.NameSpace : string.Empty;
            }
        }

        public string BaseNameSpaceWithDot
        {
            get
            {
                return Config.Language == Languages.CSharp ? string.Concat(this.Config.NameSpace, Punctuation.DOT) : string.Empty;
            }
        }

        private CodeDomProvider GetCodeProvider(Languages language)
        {
            switch (language)
            {
                case Languages.CSharp:
                    return new Microsoft.CSharp.CSharpCodeProvider();
                case Languages.VBNet:
                    return new Microsoft.VisualBasic.VBCodeProvider();
                default:
                    return new Microsoft.CSharp.CSharpCodeProvider();
            }
        }

        public string GetStringFromUnit(CodeCompileUnit compileUnit)
        {
            CodeDomProvider compiler = this.GetCodeProvider(this.Config.Language);
            CodeGeneratorOptions optionComp = new CodeGeneratorOptions();
            optionComp.BlankLinesBetweenMembers = true;

            StringWriter writer = new StringWriter();
            compiler.GenerateCodeFromCompileUnit(compileUnit, writer, optionComp);
            return writer.ToString().Replace("Kemel.Orm", this.Config.OrmNameSpace);
        }

        public System.Reflection.Assembly GetAssemblyFromUnit(CodeCompileUnit compileUnit)
        {
            CodeDomProvider compiler = this.GetCodeProvider(this.Config.Language);
            CodeGeneratorOptions optionComp = new CodeGeneratorOptions();
            optionComp.BlankLinesBetweenMembers = true;

            return compiler.CompileAssemblyFromDom(new CompilerParameters(), compileUnit).CompiledAssembly;
        }

        public CodeCompileUnit GetUnit(CodeTypeDeclaration classDeclaration, string nameSpace, SchemaType schemaType)
        {
            CodeCompileUnit compilerUnit = new CodeCompileUnit();

            CodeNamespace codeNameSpace = new CodeNamespace(nameSpace);
            compilerUnit.Namespaces.Add(codeNameSpace);

            this.AddImports(codeNameSpace.Imports, schemaType);

            codeNameSpace.Types.Add(classDeclaration);

            return compilerUnit;
        }
    }
}
