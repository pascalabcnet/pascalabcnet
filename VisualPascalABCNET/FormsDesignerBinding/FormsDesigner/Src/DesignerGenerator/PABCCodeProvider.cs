// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using System.CodeDom;

namespace ICSharpCode.FormsDesigner
{
    public static class string_consts
    {
        public static readonly string nr = "\r\n";
        public static readonly string tab = "  ";
        public static readonly string tab2 = tab + tab;
        public static string event_handler_header_trim = tab2;
        public static readonly string begin_designer_region = "{$region FormDesigner}";
        public static readonly string end_designer_region = "{$endregion FormDesigner}";
        public static readonly int min_spec_char = 0;
        public static readonly int max_spec_char = 30;

        public static readonly string begin_unit =
            "Unit {0};" + nr +
            //"{{$reference 'System.Windows.Forms.dll'}}" + nr +
            //"{{$reference 'System.Drawing.dll'}}" + nr +
            nr + "interface" + nr + nr +
            "uses System, System.Drawing, System.Windows.Forms;" +
            nr + nr +
            "type" + nr +
            tab + "{1} = class(Form)" + nr +
            tab + '{' + begin_designer_region + '}' + nr +
            tab + "private" + nr + tab2;

        public static readonly string end_unit =
            tab + end_designer_region + nr +
            tab + "public" + nr +
            tab2 + "constructor;" + nr +
            tab2 + "begin" + nr +
            tab2 + tab + "InitializeComponent;" + nr +
            tab2 + "end;" + nr +
            tab + "end;" + nr + nr +
            "implementation" + nr + nr + "end." + nr;

        public static readonly string void_type = "System.Void";

        public static readonly string main_designer_program =
            "uses {0};" + nr + nr +
            "begin" + nr +
            tab + "System.Windows.Forms.Application.EnableVisualStyles();" + nr +
            tab + "System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);" + nr +
            tab + "System.Windows.Forms.Application.Run(new {1})" + nr +
            "end.";

        public static readonly string xml_form_extention = "fmabc";
    }

    class PABCCodeGenerator : CodeCompiler
    {
        public bool own_output;
        public string UnitName;

        protected override void OutputOperator(CodeBinaryOperatorType op)
        {
            switch (op)
            {
                case CodeBinaryOperatorType.BitwiseAnd:
                case CodeBinaryOperatorType.BooleanAnd: Output.Write("and"); break;
                case CodeBinaryOperatorType.BitwiseOr:
                case CodeBinaryOperatorType.BooleanOr: Output.Write("or"); break;
                default: base.OutputOperator(op); break;
            }
        }

        protected override string CmdArgsFromParameters(CompilerParameters options)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override string CompilerName
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        protected override string FileExtension
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        protected override void ProcessCompilerOutputLine(CompilerResults results, string line)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override string CreateEscapedIdentifier(string value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override string CreateValidIdentifier(string value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateArgumentReferenceExpression(System.CodeDom.CodeArgumentReferenceExpression e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateArrayCreateExpression(System.CodeDom.CodeArrayCreateExpression e)
        {
            //throw new Exception("The method or operation is not implemented.");
            Output.Write("new ");
            OutputType(e.CreateType);
            Output.Write("[");
            Output.Write(e.Initializers.Count);
            Output.Write("]");
            if (e.Initializers.Count > 0)
            {
                Output.Write("(");
                GenerateExpression(e.Initializers[0]);
                for (int i = 1; i < e.Initializers.Count; i++)
                {
                    Output.Write(", ");
                    GenerateExpression(e.Initializers[i]);
                }
                Output.Write(")");
            }
        }

        protected override void GenerateArrayIndexerExpression(System.CodeDom.CodeArrayIndexerExpression e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateAssignStatement(System.CodeDom.CodeAssignStatement e)
        {
            GenerateExpression(e.Left);
            Output.Write(" := ");
            GenerateExpression(e.Right);
            Output.WriteLine(";");
        }

        protected override void GenerateAttachEventStatement(System.CodeDom.CodeAttachEventStatement e)
        {
            GenerateExpression(e.Event);
            Output.Write(" += ");
            GenerateExpression(e.Listener);
            Output.WriteLine(";");
        }

        protected override void GenerateAttributeDeclarationsEnd(System.CodeDom.CodeAttributeDeclarationCollection attributes)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateAttributeDeclarationsStart(System.CodeDom.CodeAttributeDeclarationCollection attributes)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateBaseReferenceExpression(System.CodeDom.CodeBaseReferenceExpression e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateCastExpression(System.CodeDom.CodeCastExpression e)
        {
            Output.Write("(");
            OutputType(e.TargetType);
            Output.Write("(");
            GenerateExpression(e.Expression);
            Output.Write("))");
            //throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateComment(System.CodeDom.CodeComment e)
        {
            Output.WriteLine("// " + e.Text);
        }

        protected override void GenerateConditionStatement(System.CodeDom.CodeConditionStatement e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateConstructor(System.CodeDom.CodeConstructor e, System.CodeDom.CodeTypeDeclaration c)
        {
            //Output.Write("constructor");
            //OutputMethodSignature(e.Attributes, e.ReturnType, "", e.Parameters, false, false);
            //GenerateBlock(e.Statements);
            //Output.WriteLine(";");
        }

        protected void GenerateBlock(CodeStatementCollection statements)
        {
            OutputStartingBrace();
            GenerateStatements(statements);
            OutputEndingBrace();
        }

        void OutputMethodSignature(MemberAttributes attributes, CodeTypeReference returnType, string name,
                CodeParameterDeclarationExpressionCollection parameters, bool isSpecialName, bool isDelegateMethod)
        {
            OutputIdentifier(name);

            // generate parameters
            if (parameters != null)
                OutputParametersDeclarations(parameters);

            // return type
            //OutputType(returnType);
            Output.WriteLine(";");
        }

        protected virtual void OutputStartingBrace()
        {
            Output.WriteLine("begin");
            Indent++;
        }

        protected virtual void OutputEndingBrace()
        {
            Indent--;
            Output.Write("end");
        }

        protected override void GenerateDelegateCreateExpression(System.CodeDom.CodeDelegateCreateExpression e)
        {
            Output.Write(e.MethodName);
        }

        protected override void GenerateDelegateInvokeExpression(System.CodeDom.CodeDelegateInvokeExpression e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateEntryPointMethod(System.CodeDom.CodeEntryPointMethod e, System.CodeDom.CodeTypeDeclaration c)
        {
            //ничего не делаем
        }

        protected override void GenerateEvent(System.CodeDom.CodeMemberEvent e, System.CodeDom.CodeTypeDeclaration c)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateEventReferenceExpression(System.CodeDom.CodeEventReferenceExpression e)
        {
            GenerateExpression(e.TargetObject);
            Output.Write(".");
            Output.Write(e.EventName);
        }

        protected override void GenerateExpressionStatement(System.CodeDom.CodeExpressionStatement e)
        {
            GenerateExpression(e.Expression);
            Output.WriteLine(";");
        }

        protected override void GenerateField(System.CodeDom.CodeMemberField e)
        {
            if (!own_output)
            {
                Output.Write(e.Name);
                Output.Write(": ");
                OutputType(e.Type);
                Output.WriteLine(";");
            }
        }

        protected override void GenerateFieldReferenceExpression(System.CodeDom.CodeFieldReferenceExpression e)
        {
            GenerateExpression(e.TargetObject);
            Output.Write(".");
            Output.Write(e.FieldName);
        }

        protected override void GenerateGotoStatement(System.CodeDom.CodeGotoStatement e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateIndexerExpression(System.CodeDom.CodeIndexerExpression e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateIterationStatement(System.CodeDom.CodeIterationStatement e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateLabeledStatement(System.CodeDom.CodeLabeledStatement e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateLinePragmaEnd(System.CodeDom.CodeLinePragma e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateLinePragmaStart(System.CodeDom.CodeLinePragma e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateMethod(System.CodeDom.CodeMemberMethod e, System.CodeDom.CodeTypeDeclaration c)
        {
            if (e.Name == "InitializeComponent" && own_output || !own_output && e.Name != "InitializeComponent")
            {
                if (e.ReturnType.BaseType == string_consts.void_type)
                {
                    Output.Write("procedure ");
                }
                else
                {
                    Output.Write("function ");
                }
                OutputMethodSignature(e.Attributes, e.ReturnType, e.Name, e.Parameters, false, false);
                GenerateBlock(e.Statements);
                Output.WriteLine(";");
            }
            else if (!own_output && e.Name == "InitializeComponent")
            {
                Output.WriteLine(string.Format("{{$include {0}}}",UnitName+"."+c.Name+".inc"));
            }
        }

        protected override void GenerateMethodInvokeExpression(System.CodeDom.CodeMethodInvokeExpression methodInvoke)
        {
            CodeMethodReferenceExpression methodRef = methodInvoke.Method;
            if (methodRef.TargetObject != null)
            {
                GenerateExpression(methodRef.TargetObject);
                Output.Write(".");
            }
            Output.Write(methodRef.MethodName);
            OutputParameters(methodInvoke.Parameters);
            //Output.Write(";");
        }

        protected override void GenerateMethodReferenceExpression(System.CodeDom.CodeMethodReferenceExpression e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateMethodReturnStatement(System.CodeDom.CodeMethodReturnStatement e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateNamespaceEnd(System.CodeDom.CodeNamespace e)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateNamespaceImport(System.CodeDom.CodeNamespaceImport e)
        {
            Output.Write(e.Namespace);
        }

        protected override void GenerateNamespace(CodeNamespace e)
        {
            GenerateNamespaceStart(e);

            //GenerateNamespaceImports(e);

            GenerateTypes(e);
            GenerateNamespaceEnd(e);
        }

        public void GenerateNamespaceImports(CodeNamespace e)
        {
            Indent++;
            CodeNamespaceImportCollection imports = e.Imports;
            int imports_count = imports.Count;
            if (imports_count > 0)
            {
                //Output.Write(string_consts.tab);
                Output.WriteLine();
                GenerateNamespaceImport(imports[0]);
                for (int i = 1; i < imports_count; ++i)
                {
                    Output.WriteLine(",");
                    //Output.Write(string_consts.tab);
                    GenerateNamespaceImport(imports[i]);
                }
                Output.WriteLine(";");
            }
            Indent--;
        }

        protected override void GenerateNamespaceStart(System.CodeDom.CodeNamespace e)
        {
            //if (e.Name != null && e.Name.Length > 0)
            //{
            //    Output.WriteLine("Unit " + e.Name + ";");
            //}
        }

        protected override void GenerateObjectCreateExpression(System.CodeDom.CodeObjectCreateExpression e)
        {
            Output.Write("new ");
            OutputType(e.CreateType);
            OutputParameters(e.Parameters);
        }

        protected override void GenerateProperty(System.CodeDom.CodeMemberProperty e, System.CodeDom.CodeTypeDeclaration c)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GeneratePropertyReferenceExpression(System.CodeDom.CodePropertyReferenceExpression e)
        {
            GenerateExpression(e.TargetObject);
            Output.Write(".");
            Output.Write(e.PropertyName);
        }

        protected override void GeneratePropertySetValueReferenceExpression(System.CodeDom.CodePropertySetValueReferenceExpression e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateRemoveEventStatement(System.CodeDom.CodeRemoveEventStatement e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateSnippetExpression(System.CodeDom.CodeSnippetExpression e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateSnippetMember(System.CodeDom.CodeSnippetTypeMember e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateThisReferenceExpression(System.CodeDom.CodeThisReferenceExpression e)
        {
            Output.Write("self");
        }

        protected override void GenerateThrowExceptionStatement(System.CodeDom.CodeThrowExceptionStatement e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateTryCatchFinallyStatement(System.CodeDom.CodeTryCatchFinallyStatement e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateTypeConstructor(System.CodeDom.CodeTypeConstructor e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateTypeEnd(System.CodeDom.CodeTypeDeclaration e)
        {
            //Output.WriteLine("end;");
            Indent--;
        }

        protected override void GenerateTypeStart(System.CodeDom.CodeTypeDeclaration e)
        {
            //Output.WriteLine("type " + e.Name + " = class" + "(" + e.BaseTypes[0].BaseType + ")");
            Indent++;
        }

        protected override void GenerateVariableDeclarationStatement(System.CodeDom.CodeVariableDeclarationStatement e)
        {
            Output.Write("var " + e.Name + ": ");
            OutputType(e.Type);
            if (e.InitExpression != null)
            {
                Output.Write(" := ");
                GenerateExpression(e.InitExpression);
            }
            Output.WriteLine(";");
            //throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateVariableReferenceExpression(System.CodeDom.CodeVariableReferenceExpression e)
        {
            Output.Write(e.VariableName);
        }

        protected override string GetTypeOutput(System.CodeDom.CodeTypeReference value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override bool IsValidIdentifier(string value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override string NullToken
        {
            //get { throw new Exception("The method or operation is not implemented."); }
            get { return "nil"; }
        }

        protected override void OutputType(System.CodeDom.CodeTypeReference typeRef)
        {
            if (!own_output && typeRef.BaseType != "System.Windows.Forms.ContextMenuStrip")
                Output.Write(typeRef.BaseType.Replace("System.Windows.Forms.Label", "&Label").Replace("System.Windows.Forms.", ""));
            else
                Output.Write(typeRef.BaseType);
        }

        protected override string QuoteSnippetString(string value)
        {
            if (value.Length == 0)
            {
                return "''";
            }
            StringBuilder b = new StringBuilder(value.Length + 5);
            bool inside = false;
            //b.Append('\'');
            for (int i = 0; i < value.Length; ++i)
            {
                int code = (int)value[i];
                if (code >= string_consts.min_spec_char && code <= string_consts.max_spec_char)
                {
                    if (inside)
                    {
                        b.Append('\'');
                        inside = false;
                    }
                    b.Append('#');
                    b.Append(code);
                }
                else
                {
                    if (!inside)
                    {
                        b.Append('\'');
                        inside = true;
                    }
                    if (value[i] == '\'')
                    {
                        b.Append("''");
                    }
                    else
                    {
                        b.Append(value[i]);
                    }
                }
            }
            if (inside)
            {
                b.Append('\'');
                inside = false;
            }
            //b.Append('\'');
            //Output.Write(value);
            return b.ToString();
        }

        protected override bool Supports(GeneratorSupport support)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GenerateParameterDeclarationExpression(CodeParameterDeclarationExpression e)
        {
            Output.Write(e.Name);
            Output.Write(": ");
            OutputType(e.Type);
        }

        protected virtual void OutputParametersDeclarations(CodeParameterDeclarationExpressionCollection parameters)
        {
            int count = parameters.Count;
            if (count == 0) return;
            Output.Write("(");
            GenerateParameterDeclarationExpression(parameters[0]);
            for (int i = 1; i < count; ++i)
            {
                Output.Write("; ");
                GenerateParameterDeclarationExpression(parameters[i]);
            }
            Output.Write(")");
        }

        protected virtual void OutputParameters(CodeExpressionCollection parameters)
        {
            int count = parameters.Count;
            Output.Write("(");
            if (count > 0)
            {
                GenerateExpression(parameters[0]);
                for (int i = 1; i < count; ++i)
                {
                    Output.Write(", ");
                    GenerateExpression(parameters[i]);
                }
            }
            Output.Write(")");
        }

    }
}
