using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kemel.Orm.Schema;
using System.CodeDom;
using Kemel.Orm.Constants;

namespace Kemel.Orm.DataBase.CodeDom
{
    public class SchemaGen: BaseGen
    {
        public SchemaGen(GenConfig config)
        {
            base.Config = config;
        }

        public void GenerateSchema(string nameSpace)
        {
            CodeTypeDeclaration typeDeclaration = this.GetSchemaDeclaration(this.Config.CurrentItem.Schema);
            CodeCompileUnit compilerUnit = this.GetUnit(typeDeclaration, nameSpace, this.Config.CurrentItem.Schema.SchemaType);
            this.Config.CurrentItem.Classes.SchemaClass = this.GetStringFromUnit(compilerUnit);
        }

        public void GenerateNormalSchema()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, EntityGen.EntityNameSpace);
            this.GenerateSchema(nameSpace);
        }

        public void GenerateProcedureSchema()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, ProcedureNameSpace, Punctuation.DOT, EntityGen.EntityNameSpace);
            this.GenerateSchema(nameSpace);
        }

        public void GenerateViewSchema()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, ViewNameSpace, Punctuation.DOT, EntityGen.EntityNameSpace);
            this.GenerateSchema(nameSpace);
        }

        public void GenerateFunctionSchema()
        {
            string nameSpace = string.Concat(this.BaseNameSpaceWithDot, FunctionNameSpace, Punctuation.DOT, EntityGen.EntityNameSpace);
            this.GenerateSchema(nameSpace);
        }


        public override GenType Type
        {
            get { return GenType.Entity; }
        }

        private CodeTypeDeclaration GetSchemaDeclaration(TableSchema tableSchema)
        {
            CodeTypeDeclaration classDeclare = new CodeTypeDeclaration(string.Concat(tableSchema.Name, EntityGen.EntitySufix));
            classDeclare.IsPartial = true;

            CodeTypeDeclaration definitionDeclare = new CodeTypeDeclaration("Schema");
            classDeclare.Members.Add(definitionDeclare);

            #region CodeMemberField
            CodeMemberField field = new CodeMemberField(typeof(TableSchema), "f_Table");
            field.Attributes = MemberAttributes.Static | MemberAttributes.Private;
            field.InitExpression = new CodePrimitiveExpression(null);
            definitionDeclare.Members.Add(field);

            CodeFieldReferenceExpression codeField = new CodeFieldReferenceExpression(
                        new CodeTypeReferenceExpression("Schema"), field.Name);
            #endregion

            #region CodeMemberProperty
            CodeMemberProperty prop = new CodeMemberProperty();
            prop.Type = new CodeTypeReference(typeof(TableSchema));
            prop.Name = "Table";
            prop.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            prop.HasGet = true;
            prop.HasSet = false;

            CodeConditionStatement condition = new CodeConditionStatement();

            CodeBinaryOperatorExpression ifCondition = new CodeBinaryOperatorExpression();
            ifCondition.Left = new CodeVariableReferenceExpression(field.Name);
            ifCondition.Operator = CodeBinaryOperatorType.IdentityEquality;
            ifCondition.Right = new CodePrimitiveExpression(null);
            condition.Condition = ifCondition;

            //SchemaContainer.GetSchema<TEtt>();
            CodeMethodInvokeExpression invokeGetSchema = new CodeMethodInvokeExpression(
                      new CodeMethodReferenceExpression(
                         new CodeTypeReferenceExpression("SchemaContainer"),
                             "GetSchema",
                                 new CodeTypeReference[] {
                                    new CodeTypeReference(string.Concat(tableSchema.Name, EntityGen.EntitySufix)),}),
                                           new CodeExpression[0]);


            condition.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(field.Name), invokeGetSchema));

            prop.GetStatements.Add(condition);
            prop.GetStatements.Add(new CodeMethodReturnStatement(codeField));

            definitionDeclare.Members.Add(prop);
            #endregion

            foreach (ColumnSchema column in tableSchema.Columns)
            {
                if (!column.IgnoreColumn)
                {
                    this.GenerateColumnSchemaField(definitionDeclare.Members, prop, column);
                }
            }

            return classDeclare;
        }

        private void GenerateColumnSchemaField(CodeTypeMemberCollection codeTypeMemberCollection, CodeMemberProperty tableSchemaProperty, ColumnSchema column)
        {

            #region CodeMemberField
            CodeMemberField field = new CodeMemberField(typeof(ColumnSchema), string.Concat("f_", column.Name));
            field.Attributes = MemberAttributes.Static | MemberAttributes.Private;
            field.InitExpression = new CodePrimitiveExpression(null);
            codeTypeMemberCollection.Add(field);

            CodeFieldReferenceExpression codeField = new CodeFieldReferenceExpression(
                        new CodeTypeReferenceExpression("Schema"), field.Name);
            #endregion

            #region CodeMemberProperty
            CodeMemberProperty prop = new CodeMemberProperty();
            prop.Type = new CodeTypeReference(typeof(ColumnSchema));
            prop.Name = column.Name;
            prop.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            prop.HasGet = true;
            prop.HasSet = false;

            CodeConditionStatement condition = new CodeConditionStatement();

            CodeBinaryOperatorExpression ifCondition = new CodeBinaryOperatorExpression();
            ifCondition.Left = new CodeVariableReferenceExpression(field.Name);
            ifCondition.Operator = CodeBinaryOperatorType.IdentityEquality;
            ifCondition.Right = new CodePrimitiveExpression(null);
            condition.Condition = ifCondition;

            CodeIndexerExpression columnIndex = new CodeIndexerExpression(
                new CodePropertyReferenceExpression(
                    new CodePropertyReferenceExpression(new CodeTypeReferenceExpression("Schema"), tableSchemaProperty.Name),
                    "Columns"),
                new CodePrimitiveExpression(column.Name));


            condition.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(field.Name), columnIndex));

            prop.GetStatements.Add(condition);
            prop.GetStatements.Add(new CodeMethodReturnStatement(codeField));

            codeTypeMemberCollection.Add(prop);
            #endregion
        }
    }
}
