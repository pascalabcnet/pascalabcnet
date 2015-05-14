using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.CodeDom.Compiler;
using System.CodeDom;
using Microsoft.CSharp;
using Microsoft.VisualBasic;

namespace ICSharpCode.FormsDesigner
{
	/// <summary>
	/// This is used by the CodeDomHostLoader to generate the CodeCompleUnit
	/// </summary>
	internal class CodeGen 
	{
		private CodeCompileUnit codeCompileUnit;
		private CodeNamespace ns;
		private CodeTypeDeclaration myDesignerClass = new CodeTypeDeclaration();
		private CodeMemberMethod initializeComponent = new CodeMemberMethod();
		private IDesignerHost host;
		private IComponent root;

		private static readonly Attribute[] propertyAttributes = new Attribute[] 
		{
			DesignOnlyAttribute.No
		};

        /// <summary>
        /// This function generates the default CodeCompileUnit template
        /// </summary>
        public CodeCompileUnit GetCodeCompileUnit(IDesignerHost host)
		{
            this.host = host;
            IDesignerHost idh = (IDesignerHost)this.host.GetService(typeof(IDesignerHost));
            root = idh.RootComponent;
            Hashtable nametable = new Hashtable(idh.Container.Components.Count);

            ns = new CodeNamespace("DesignerHostSample");
            myDesignerClass = new CodeTypeDeclaration();
            initializeComponent = new CodeMemberMethod();

            CodeCompileUnit code = new CodeCompileUnit();

            // Imports
            ns.Imports.Add(new CodeNamespaceImport("System"));
            ns.Imports.Add(new CodeNamespaceImport("System.ComponentModel"));
            ns.Imports.Add(new CodeNamespaceImport("System.Windows.Forms"));
            code.Namespaces.Add(ns);
            myDesignerClass = new CodeTypeDeclaration(root.Site.Name);
            myDesignerClass.BaseTypes.Add(typeof(Form).FullName);

            IDesignerSerializationManager manager = host.GetService(typeof(IDesignerSerializationManager)) as IDesignerSerializationManager;

            ns.Types.Add(myDesignerClass);

            // Constructor
            CodeConstructor con = new CodeConstructor();

            con.Attributes = MemberAttributes.Public;
            con.Statements.Add(new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), "InitializeComponent")));
            myDesignerClass.Members.Add(con);

            // Main
            CodeEntryPointMethod main = new CodeEntryPointMethod();

            main.Name = "Main";
            main.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            main.CustomAttributes.Add(new CodeAttributeDeclaration("System.STAThreadAttribute"));
            main.Statements.Add(new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(System.Windows.Forms.Application)), "Run"), new CodeExpression[] {
				new CodeObjectCreateExpression(new CodeTypeReference(root.Site.Name))
			}));
            myDesignerClass.Members.Add(main);

            // InitializeComponent
            initializeComponent.Name = "InitializeComponent";
            initializeComponent.Attributes = MemberAttributes.Private;
            initializeComponent.ReturnType = new CodeTypeReference(typeof(void));
            initializeComponent.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Text"), new CodePrimitiveExpression(root.Site.Name))); //roman//
            myDesignerClass.Members.Add(initializeComponent);
            codeCompileUnit = code;
            return codeCompileUnit;
		}
        
	}// class
} // namespace
