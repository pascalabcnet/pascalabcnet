using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeConverter;
using System.IO;


namespace PascalABCCompiler.TreeConverter.TreeConversion
{
    class SyntaxTreeVisitorsController
    {
        private List<syntax_tree_visitor> syntaxTreeVisitors = new List<syntax_tree_visitor>();
        public List<syntax_tree_visitor> SyntaxTreeVisitors
        {
            get
            {
                return syntaxTreeVisitors;
            }
        }
        public SyntaxTreeVisitorsController()
        {
            ReloadConverters();
        }
        public void ReloadConverters()
        {
            syntaxTreeVisitors.Clear();
            syntaxTreeVisitors.Add(new syntax_tree_visitor());
            string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);
            DirectoryInfo di = new DirectoryInfo(dir);
            FileInfo[] dllfiles = di.GetFiles("*SyntaxTreeVisitor.dll");
            System.Reflection.Assembly asssembly = null;
            foreach (FileInfo fi in dllfiles)
            {
                asssembly = System.Reflection.Assembly.LoadFile(fi.FullName);
                try
                {
                    Type[] types = asssembly.GetTypes();
                    if (asssembly != null)
                    {
                        foreach (Type type in types)
                        {
                            if (type.Name.IndexOf("syntax_tree_visitor") >= 0)
                            {
                                Object obj = Activator.CreateInstance(type);
                                if (obj is syntax_tree_visitor)
                                {
                                    syntaxTreeVisitors.Add((syntax_tree_visitor)obj);
                                }
                            }
                        }
                    }
                }
                catch (System.Reflection.ReflectionTypeLoadException e)
                {
                    Console.Error.WriteLine("SyntaxTreeVisitor {0} reflection error {1}", Path.GetFileName(fi.FullName), e);
                    foreach (var le in e.LoaderExceptions)
                        Console.Error.WriteLine(le);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("SyntaxTreeVisitor {0} loading error {1}", Path.GetFileName(fi.FullName), e);
                }
            }
        }
        public syntax_tree_visitor SelectVisitor(string ext)
        {
            foreach (syntax_tree_visitor stv in SyntaxTreeVisitors)
                foreach (string stvfext in stv.FilesExtensions)
                    if (stvfext.ToLower() == ext)
                        return stv;
            return null;
        }
    }
}
