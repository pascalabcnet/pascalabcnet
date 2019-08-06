// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using PascalABCCompiler.SyntaxTreeConverters;

namespace PascalABCCompiler.SyntaxTreeConverters
{
    public class SyntaxTreeConvertersController
    {
        public enum State
        {
            ConnectConverter, Convert
        }
        ICompiler Compiler;

        private List<ISyntaxTreeConverter> syntaxTreeConverters = new List<ISyntaxTreeConverter>();
        public List<ISyntaxTreeConverter> SyntaxTreeConverters
        {
            get
            {
                return syntaxTreeConverters;
            }
        }

        public delegate void ChangeStateDelegate(State State, ISyntaxTreeConverter SyntaxTreeConverter);
        public event ChangeStateDelegate ChangeState;


        public SyntaxTreeConvertersController(ICompiler Compiler)
        {
            this.Compiler = Compiler;
        }

        public void AddStandardConverters()
        {
            var st = new StandardSyntaxTreeConverter();
            syntaxTreeConverters.Add(st);
        }
        public void AddConverters()
        {
            AddStandardConverters();
            AddExternalConvertersAsPlugins(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName));
        }
        private void AddExternalConvertersAsPlugins(string DirectoryName)
        {
            DirectoryInfo di = new DirectoryInfo(DirectoryName);
            List<FileInfo> dllfiles = di.GetFiles("*ConversionSyntaxExternal.dll").ToList(); // Это только YieldConversionSyntax.dll
            //List<FileInfo> standartdllfile = di.GetFiles("StandardSyntaxTreeConverter.dll").ToList();
            //if (standartdllfile.Count>0)
            //    dllfiles.Insert(0, standartdllfile[0]);
            //dllfiles.Insert(0,new FileInfo("StandardSyntaxTreeConverter.dll"));

            System.Reflection.Assembly assembly = null;
            ISyntaxTreeConverter Converter;
            foreach (FileInfo fi in dllfiles)
            {
                try
                {
                    assembly = System.Reflection.Assembly.LoadFile(fi.FullName);
                    try
                    {
                        if (assembly != null)
                        {
                            Type[] types = assembly.GetTypes();
                            foreach (Type type in types)
                            {
                                if (type.Name.IndexOf("SyntaxTreeConverter") >= 0 && type.IsClass)
                                {
                                    Object obj = Activator.CreateInstance(type);
                                    if (obj is ISyntaxTreeConverter)
                                    {
                                        Converter = obj as ISyntaxTreeConverter;
                                        syntaxTreeConverters.Add(Converter);
                                        ChangeState(State.ConnectConverter, Converter);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //ErrorList.Add(new PluginLoadingError(fi.FullName, e));
                    }

                }
                catch (Exception)
                {
                    //если dll не нетовская
                }
            }

        }

        public syntax_tree_node Convert(syntax_tree_node root)
        {
            foreach (ISyntaxTreeConverter SyntaxTreeConverter in syntaxTreeConverters)
            {
                ChangeState(State.Convert, SyntaxTreeConverter);
                root = SyntaxTreeConverter.Convert(root);
            }
            return root;
        }
    }
}
