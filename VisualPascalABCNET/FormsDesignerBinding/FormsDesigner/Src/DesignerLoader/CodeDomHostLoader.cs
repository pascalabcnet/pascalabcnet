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
using System.Xml;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Drawing;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using ICSharpCode.FormsDesigner.Services;

namespace ICSharpCode.FormsDesigner
{
    /// <summary>
    /// Inherits from CodeDomDesignerLoader. It can generate C# or VB code
    /// for a HostSurface. This loader does not support parsing a 
    /// C# or VB file.
    /// </summary>
	public class CodeDomHostLoader : CodeDomDesignerLoader, INameCreationService
	{
		CSharpCodeProvider _csCodeProvider = new CSharpCodeProvider();
		CodeCompileUnit codeCompileUnit = null;
        CodeGen cg = null;
		TypeResolutionService _trs = null;
		private string executable;
        string unitFile;
		private Process run;
        string formFile = null;
        IDesignerHost host;
        private DesignSurface Designer; //roman//

        string INameCreationService.CreateName(IContainer container, Type dataType)
        {
            string name = Char.ToLower(dataType.Name[0]) + dataType.Name.Substring(1);
            int number = 1;
            if (container != null)
            {
                while (container.Components[name + number.ToString()] != null)
                {
                    ++number;
                }
            }
            return name + number.ToString();
        }

        bool INameCreationService.IsValidName(string name)
        {
            if (name == null)
                return false;
            if (name.Length == 0)
                return false;
            IContainer container = host.Container;
            if (container != null) 
            {
				if (container.Components[name] != null)
					return false;
			}
            if (!(name[0] == '_' || (name[0] >= 'a' && name[0] <= 'z') || (name[0] >= 'A' && name[0] <= 'Z')))
                return false;
            for (int i = 1; i < name.Length; i++)
                if (!(name[i] == '_' || (name[i] >= 'a' && name[i] <= 'z') || (name[i] >= 'A' && name[i] <= 'Z') || char.IsDigit(name[i])))
                    return false;
            return true;
        }

		public CodeDomHostLoader(IDesignerHost host, string formFile, string unitFile)
		{
            _trs = host.GetService(typeof(ITypeResolutionService)) as TypeResolutionService;
            this.formFile = formFile;
            this.unitFile = unitFile;
            this.host = host;
		}

        protected override ITypeResolutionService TypeResolutionService
		{
			get
			{
				return _trs;
			}
		}

		protected override CodeDomProvider CodeDomProvider
		{
			get
			{
				return _csCodeProvider;
			}
		}

        /// <summary>
        /// Bootstrap method - loads a blank Form
        /// </summary>
        /// <returns></returns>
        protected override CodeCompileUnit Parse()
        {
            if (formFile == null)
            {
                CodeCompileUnit ccu = null;

                DesignSurface ds = new DesignSurface();
                ds.BeginLoad(typeof(Form));
                IDesignerHost idh = (IDesignerHost)ds.GetService(typeof(IDesignerHost));
                idh.RootComponent.Site.Name = GetNextFormName();

                cg = new CodeGen();
                ccu = cg.GetCodeCompileUnit(idh);

                AssemblyName[] names = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
                for (int i = 0; i < names.Length; i++)
                {
                    Assembly assembly = Assembly.Load(names[i]);
                    ccu.ReferencedAssemblies.Add(assembly.Location);
                }

                codeCompileUnit = ccu;
                Designer = ds;
                codeCompileUnit = ccu;
                return ccu;
            }
            else
            {
                FileStream fs = new FileStream(formFile, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                CodeCompileUnit ccu = (CodeCompileUnit)formatter.Deserialize(fs);
                fs.Close();
                codeCompileUnit = ccu;
                return ccu;
            }
        }

        protected string GetNextFormName()
        {
            return "Form1";
        }

        /// <summary>
        /// When the Loader is Flushed this method is called. The base class
        /// (CodeDomDesignerLoader) creates the CodeCompileUnit. We
        /// simply cache it and use this when we need to generate code from it.
        /// To generate the code we use CodeProvider.
        /// </summary>
		protected override void Write(CodeCompileUnit unit)
		{
			codeCompileUnit = unit;
		}

		protected override void OnEndLoad(bool successful, ICollection errors)
		{
			base.OnEndLoad(successful, errors);
			if (errors != null)
			{
				IEnumerator ie = errors.GetEnumerator();
				while (ie.MoveNext())
					System.Diagnostics.Trace.WriteLine(ie.Current.ToString());
			}
		}

		#region Public methods

        /// <summary>
        /// Flushes the host and returns the updated CodeCompileUnit
        /// </summary>
        /// <returns></returns>
		public CodeCompileUnit GetCodeCompileUnit()
		{
            Flush();
            return codeCompileUnit;
		}

        /// <summary>
        /// This method writes out the contents of our designer in C# and VB.
		/// It generates code from our codeCompileUnit using CodeRpovider
        /// </summary>
        public string GetCode()
		{
            Flush();
            StringWriter sw;
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BlankLinesBetweenMembers = false;
            options.BracingStyle = "C";
            options.ElseOnClosing = false;
            options.IndentString = "    ";

            // PABC Code Generation
            sw = new StringWriter();
            DesignerResourceService resourceService = host.GetService(typeof(IResourceService)) as DesignerResourceService;
            if (resourceService.ResourcesFileNames != null && resourceService.ResourcesFileNames.Count > 0)
            {
                for (int i = 0; i < resourceService.ResourcesFileNames.Count; i++)
                {
                    sw.WriteLine("{$resource " + Path.GetFileName(resourceService.ResourcesFileNames[i]) + '}');
                    sw.Write("    ");
                }
            }
            ICodeGenerator abc = new PABCCodeGenerator();
            PABCCodeGenerator pabc = abc as PABCCodeGenerator;
            pabc.UnitName = Path.GetFileNameWithoutExtension(unitFile);
            abc.GenerateCodeFromCompileUnit(codeCompileUnit, sw, options);
            PABCCodeGenerator inc_abc = new PABCCodeGenerator();
            inc_abc.own_output = true;
            StreamWriter inc_sw = new StreamWriter(Path.Combine(Path.GetDirectoryName(VisualPascalABC.ProjectFactory.Instance.CurrentProject.Path),/*codeCompileUnit.Namespaces[0].Types[0].Name+".inc"*/pabc.UnitName + "." + getFormName() + ".inc"));
            (inc_abc as ICodeGenerator).GenerateCodeFromCompileUnit(codeCompileUnit, inc_sw, options);
            inc_sw.Flush();
            inc_sw.Close();
            return sw.ToString();
		}

        private string getFormName()
        {
            return (this.host.RootComponent as Control).Name;
        }

		#endregion

		#region Build and Run

        /// <summary>
		/// Called when we want to build an executable. Returns true if we succeeded.
        /// </summary>
        public bool Build()
		{
			Flush();

			// If we haven't already chosen a spot to write the executable to,
			// do so now.
			if (executable == null)
			{
				SaveFileDialog dlg = new SaveFileDialog();

				dlg.DefaultExt = "exe";
				dlg.Filter = "Executables|*.exe";
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					executable = dlg.FileName;
				}
			}

			if (executable != null)
			{
				// We need to collect the parameters that our compiler will use.
				CompilerParameters cp = new CompilerParameters();
				AssemblyName[] assemblyNames = Assembly.GetEntryAssembly().GetReferencedAssemblies();

				foreach (AssemblyName an in assemblyNames)
				{
					Assembly assembly = Assembly.Load(an);
					cp.ReferencedAssemblies.Add(assembly.Location);
				}

				cp.GenerateExecutable = true;
				cp.OutputAssembly = executable;

				// Remember our main class is not Form, but Form1 (or whatever the user calls it)!
				cp.MainClass = "DesignerHostSample." + this.LoaderHost.RootComponent.Site.Name;

                CSharpCodeProvider cc = new CSharpCodeProvider();
				CompilerResults cr = cc.CompileAssemblyFromDom(cp, codeCompileUnit);

				if (cr.Errors.HasErrors)
				{
					string errors = "";

					foreach (CompilerError error in cr.Errors)
					{
						errors += error.ErrorText + "\n";
					}

					MessageBox.Show(errors, "Errors during compile.");
				}

				return !cr.Errors.HasErrors;
			}

			return false;
		}

        /// <summary>
        /// Here we build the executable and then run it. We make sure to not start
		/// two of the same process.
        /// </summary>
        public void Run()
		{
			if ((run == null) || (run.HasExited))
			{
				if (Build())
				{
					run = new Process();
					run.StartInfo.FileName = executable;
					run.Start();
				}
			}
		}

        /// <summary>
        /// Just in case the red X in the upper right isn't good enough,
		/// we can kill our process here.
        /// </summary>
        public void Stop()
		{
			if ((run != null) && (!run.HasExited))
			{
				run.Kill();
			}
		}

		#endregion

	}// class
}// namespace
