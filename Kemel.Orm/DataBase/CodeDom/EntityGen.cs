using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Schema;
using System.CodeDom;
using Kemel.Orm.Constants;
using Kemel.Orm.Entity.Attributes;

namespace Kemel.Orm.DataBase.CodeDom
{
    public class EntityGen : BaseGen
    {
        public EntityGen(GenConfig config)
        {
            base.Config = config;
        }

        public const string EntityNameSpace = "Entity";
        public const string EntitySufix = "_Entity";

        public override GenType Type
        {
            get { return GenType.Entity; }
        }

        public void GenerateEntity(string nameSpace)
        {
            CodeTypeDeclaration typeDeclaration = this.GetEntityDeclaration();
            CodeCompileUnit compilerUnit = this.GetUnit(typeDeclaration, nameSpace, this.Config.CurrentItem.Schema.SchemaType);
            this.Config.CurrentItem.Classes.EntityClass = this.GetStringFromUnit(compilerUnit);
        }

        public void GenerateNormalEntity()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, EntityNameSpace);
            this.GenerateEntity(nameSpace);
        }

        public void GenerateProcedureEntity()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, ProcedureNameSpace, Punctuation.DOT, EntityNameSpace);
            this.GenerateEntity(nameSpace);
        }

        public void GenerateViewEntity()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, ViewNameSpace, Punctuation.DOT, EntityNameSpace);
            this.GenerateEntity(nameSpace);
        }

        public void GenerateFunctionEntity()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, FunctionNameSpace, Punctuation.DOT, EntityNameSpace);
            this.GenerateEntity(nameSpace);
        }

        private CodeTypeDeclaration GetEntityDeclaration()
        {
            TableSchema schema = this.Config.CurrentItem.Schema;
            this.Config.CurrentItem.Classes.EntityFileName = string.Concat(schema.Name, EntitySufix);
            CodeTypeDeclaration classDeclare = new CodeTypeDeclaration(this.Config.CurrentItem.Classes.EntityFileName);

            if (schema.SchemaType != SchemaType.None)
            {
                classDeclare.CustomAttributes.Add(
                    new CodeAttributeDeclaration(
                     "TableSchemaType",
                    new CodeAttributeArgument(
                        new CodeFieldReferenceExpression(
                            new CodeTypeReferenceExpression(new CodeTypeReference(typeof(SchemaType), CodeTypeReferenceOptions.GlobalReference)),
                            Enum.GetName(typeof(SchemaType), schema.SchemaType)
                        )
                    )));
            }

            classDeclare.IsPartial = true;
            classDeclare.BaseTypes.Add(typeof(Kemel.Orm.Entity.EntityBase));

            foreach (ColumnSchema column in schema.Columns)
            {
                if (!column.IgnoreColumn)
                {
                    this.AddFieldAndProperty(classDeclare, column);
                }
            }

            return classDeclare;
        }

        private void AddFieldAndProperty(CodeTypeDeclaration classDeclare, ColumnSchema column)
        {
            CodeMemberField field = this.GetField(column);
            classDeclare.Members.Add(field);
            classDeclare.Members.Add(this.GetProperty(column, field));
        }

        private CodeMemberField GetField(ColumnSchema column)
        {
            CodeMemberField field = new CodeMemberField();
            field.Name = string.Concat("f_", column.Name.ToLower());

            field.Type = new CodeTypeReference(column.Type);
            field.Attributes = MemberAttributes.Private;
            return field;
        }

        private CodeMemberProperty GetProperty(ColumnSchema column, CodeMemberField field)
        {
            SchemaType schemaType = column.Parent.SchemaType;

            CodeMemberProperty property = new CodeMemberProperty();
            property.Name = column.Name;
            property.Type = new CodeTypeReference(column.Type);
            property.Attributes = MemberAttributes.Public;
            property.HasGet = property.HasSet = true;

            property.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, column.Name));
            property.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, column.Name));

            CodeFieldReferenceExpression codeField = new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(), field.Name);

            property.GetStatements.Add(new CodeMethodReturnStatement(codeField));

            property.SetStatements.Add(
                new CodeAssignStatement(codeField,
                new CodePropertySetValueReferenceExpression()));

            #region schemaType == SchemaType.Table || schemaType == SchemaType.None
            if (schemaType == SchemaType.Table || schemaType == SchemaType.None)
            {

                #region Comments
                property.Comments.Add(new CodeCommentStatement("<summary>", true));

                if (column.IsPrimaryKey)
                    property.Comments.Add(new CodeCommentStatement(Properties.Resources.IsPrimaryKeyInformation, true));

                if (column.IsIdentity)
                    property.Comments.Add(new CodeCommentStatement(Properties.Resources.IsIdentityInformation, true));

                property.Comments.Add(
                    new CodeCommentStatement(column.AllowNull ? Properties.Resources.AllowNullInformation : Properties.Resources.NotAllowNullInformation, true)
                    );

                property.Comments.Add(new CodeCommentStatement("</summary>", true));
                #endregion

                property.CustomAttributes.Add(
                    new CodeAttributeDeclaration(
                    "Identity",
                    new CodeAttributeArgument(new CodePrimitiveExpression(column.IsIdentity))));

                property.CustomAttributes.Add(
                    new CodeAttributeDeclaration(
                    "PrimaryKey",
                    new CodeAttributeArgument(new CodePrimitiveExpression(column.IsPrimaryKey))));

                property.CustomAttributes.Add(
                    new CodeAttributeDeclaration(
                    "AllowNull",
                    new CodeAttributeArgument(new CodePrimitiveExpression(column.AllowNull))));

                if (column.IsLogicalExclusionColumn)
                {
                    property.CustomAttributes.Add(
                        new CodeAttributeDeclaration(
                        "LogicalExclusionColumn"));
                }

                if (column.IsForeignKey)
                {
                    property.CustomAttributes.Add(
                        new CodeAttributeDeclaration(
                        "ForeignKey",
                        new CodeAttributeArgument(
                        new CodeTypeOfExpression(string.Concat(column.ReferenceTableName, EntitySufix)))));
                }
            }
            #endregion

            if (schemaType == SchemaType.Procedure)
            {
               CodeExpression paramDir = new CodeFieldReferenceExpression
                    (
                        new CodeTypeReferenceExpression(typeof(System.Data.ParameterDirection)),
                        column.ParamDirection.ToString()
                    );
                property.CustomAttributes.Add(
                    new CodeAttributeDeclaration(
                    "ParameterDirection",
                    new CodeAttributeArgument(paramDir)));
            }

            if (column.IgnoreColumn)
            {
                property.CustomAttributes.Add(
                    new CodeAttributeDeclaration(
                    "IgnoreProperty"));
            }

            switch (column.DBType)
            {
                case System.Data.DbType.AnsiString:
                case System.Data.DbType.AnsiStringFixedLength:
                case System.Data.DbType.String:
                case System.Data.DbType.StringFixedLength:
                case System.Data.DbType.Xml:
                    property.CustomAttributes.Add(
                    new CodeAttributeDeclaration(
                    "MaxLength",
                    new CodeAttributeArgument(new CodePrimitiveExpression(column.MaxLength))));
                    break;
            }

            return property;
        }

        public void GenerateEntityDefinition()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, EntityNameSpace);
            this.GenerateDefinitionDeclaration(nameSpace);
        }

        public void GenerateProcedureEntityDefinition()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, ProcedureNameSpace, Punctuation.DOT, EntityNameSpace);
            this.GenerateDefinitionDeclaration(nameSpace);
        }

        public void GenerateViewEntityDefinition()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, ViewNameSpace, Punctuation.DOT, EntityNameSpace);
            this.GenerateDefinitionDeclaration(nameSpace);
        }

        public void GenerateFunctionEntityDefinition()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, FunctionNameSpace, Punctuation.DOT, EntityNameSpace);
            this.GenerateDefinitionDeclaration(nameSpace);
        }

        public void GenerateEntityExtension()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, EntityNameSpace);
            this.GenerateExtensionDeclaration(nameSpace);
        }

        public void GenerateProcedureEntityExtension()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, ProcedureNameSpace, Punctuation.DOT, EntityNameSpace);
            this.GenerateExtensionDeclaration(nameSpace);
        }

        public void GenerateViewEntityExtension()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, ViewNameSpace, Punctuation.DOT, EntityNameSpace);
            this.GenerateExtensionDeclaration(nameSpace);
        }

        public void GenerateFunctionEntityExtension()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, FunctionNameSpace, Punctuation.DOT, EntityNameSpace);
            this.GenerateExtensionDeclaration(nameSpace);
        }

        public void GenerateDefinitionDeclaration(string nameSpace)
        {
            CodeTypeDeclaration typeDeclaration = this.GetDefinitionDeclaration();
            CodeCompileUnit compilerUnit = this.GetUnit(typeDeclaration, nameSpace, this.Config.CurrentItem.Schema.SchemaType);
            this.Config.CurrentItem.Classes.DefinitionClass = this.GetStringFromUnit(compilerUnit);
        }

        public void GenerateExtensionDeclaration(string nameSpace)
        {
            CodeTypeDeclaration typeDeclaration = this.GetExtensionDeclaration();
            CodeCompileUnit compilerUnit = this.GetUnit(typeDeclaration, nameSpace, this.Config.CurrentItem.Schema.SchemaType);
            this.Config.CurrentItem.Classes.ExtensionClass = this.GetStringFromUnit(compilerUnit);
        }

        private CodeTypeDeclaration GetDefinitionDeclaration()
        {
            TableSchema schema = this.Config.CurrentItem.Schema;
            CodeTypeDeclaration classDeclare = new CodeTypeDeclaration(this.Config.CurrentItem.Classes.EntityFileName);
            classDeclare.IsPartial = true;

            CodeTypeDeclaration definitionDeclare = new CodeTypeDeclaration("Definition");
            classDeclare.Members.Add(definitionDeclare);

            CodeMemberField field = new CodeMemberField(typeof(string), "TABLE_NAME");
            field.Attributes = MemberAttributes.Const | MemberAttributes.Public;
            field.InitExpression = new CodePrimitiveExpression(schema.Name);
            definitionDeclare.Members.Add(field);

            foreach (ColumnSchema column in schema.Columns)
            {
                if (!column.IgnoreColumn)
                {
                    field = new CodeMemberField(typeof(string), column.Name.ToUpper());
                    field.Attributes = MemberAttributes.Const | MemberAttributes.Public;
                    field.InitExpression = new CodePrimitiveExpression(column.Name);

                    definitionDeclare.Members.Add(field);
                }
            }

            return classDeclare;
        }

        private CodeTypeDeclaration GetExtensionDeclaration()
        {
            TableSchema schema = this.Config.CurrentItem.Schema;
            CodeTypeDeclaration classDeclare = new CodeTypeDeclaration(this.Config.CurrentItem.Classes.EntityFileName);
            classDeclare.IsPartial = true;

            CodeTypeDeclaration extensionDeclare = new CodeTypeDeclaration("Extension");
            classDeclare.Members.Add(extensionDeclare);

            CodeMemberField field;

            foreach (ColumnSchema column in schema.Columns)
            {
                if (column.IgnoreColumn)
                {
                    this.AddFieldAndProperty(classDeclare, column);

                    field = new CodeMemberField(typeof(string), column.Name.ToUpper());
                    field.Attributes = MemberAttributes.Const | MemberAttributes.Public;
                    field.InitExpression = new CodePrimitiveExpression(column.Name);

                    extensionDeclare.Members.Add(field);
                }
            }

            return classDeclare;
        }
    }
}
