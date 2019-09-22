// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace VisualPascalABC
{
	[Serializable]
	public class OpenedFileInfo
	{
		public string FileName;
        public bool InProject;
		public int CaretLine;
		public int CaretColumn;
	}
	
	[Serializable]
	public class WatchExprInfo
	{
		public string Expression;
		
		public WatchExprInfo(string expression)
		{
			Expression = expression;
		}
	}
	
	[Serializable]
	public class UserBreakpointInfo
	{
		public string FileName;
		public int Line;
		public bool IsConditional;
		public int Kind;
		public string Expression;
	}
	
	[Serializable]
	public class UserProjectSettings
	{
		public string CurrentDocument;
        public bool CurrentDocumentIsInProject;
		public OpenedFileInfo[] OpenDocuments;
		public WatchExprInfo[] WatchExprs;
		public UserBreakpointInfo[] Breakpoints;
	}
	
	public class ProjectUserOptionsManager
	{
		private static string opt_ext = ".opt";
		
		public static UserProjectSettings MakeOptions()
		{
			UserProjectSettings options = new UserProjectSettings();
			List<OpenedFileInfo> openedDocuments = new List<OpenedFileInfo>();
            List<OpenedFileInfo> openedProjectDocuments = new List<OpenedFileInfo>();
            foreach (CodeFileDocumentControl cfdc in VisualPABCSingleton.MainForm.OpenDocuments.Values)
			{
				OpenedFileInfo fi = new OpenedFileInfo();
                if (ProjectFactory.Instance.CurrentProject.ContainsSourceFile(cfdc.FileName))
                {
                    fi.FileName = PascalABCCompiler.Tools.RelativePathTo(Path.GetDirectoryName(ProjectFactory.Instance.CurrentProject.Path),cfdc.FileName);
                    fi.InProject = true;
                }
                else
                {
                    fi.FileName = cfdc.FileName;
                }
                openedDocuments.Add(fi);
                fi.CaretLine = cfdc.TextEditor.CaretLine;
                fi.CaretColumn = cfdc.TextEditor.CaretColumn;
			}
            if (ProjectFactory.Instance.CurrentProject.ContainsSourceFile(VisualPABCSingleton.MainForm.CurrentCodeFileDocument.FileName))
            {
                options.CurrentDocument = PascalABCCompiler.Tools.RelativePathTo(Path.GetDirectoryName(ProjectFactory.Instance.CurrentProject.Path), VisualPABCSingleton.MainForm.CurrentCodeFileDocument.FileName);
                options.CurrentDocumentIsInProject = true;
            }
            else
                options.CurrentDocument = WorkbenchServiceFactory.DocumentService.CurrentCodeFileDocument.FileName;
			options.OpenDocuments = openedDocuments.ToArray();
			string[] exprs = VisualPABCSingleton.MainForm.DebugWatchListWindow.GetExpressions();
			List<WatchExprInfo> watches = new List<WatchExprInfo>();
			foreach (string s in exprs)
			{
				watches.Add(new WatchExprInfo(s));
			}
			options.WatchExprs = watches.ToArray();
			return options;
		}
		
		public static void SaveOptions(string ProjectPath)
		{
			try
			{
				UserProjectSettings setts = MakeOptions();
				string file_name = Path.ChangeExtension(ProjectPath,opt_ext);
				System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				bf.Serialize(new FileStream(file_name, FileMode.Create),setts);
			}
			catch (Exception e)
			{
				
			}
		}
		
		public static UserProjectSettings LoadOptions(string ProjectPath)
		{
			UserProjectSettings setts = null;
			try
			{
				string file_name = Path.ChangeExtension(ProjectPath,opt_ext);
				System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				return bf.Deserialize(new FileStream(file_name, FileMode.Open)) as UserProjectSettings;
			}
			catch (Exception)
			{
				
			}
			return setts;
		}
	}
}
