// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PascalABCCompiler.SemanticTreeConverters
{
    public class SemanticTreeConvertersController
    {
        public enum State
        {
            ConnectConverter, Convert
        }
        ICompiler Compiler;

        private List<ISemanticTreeConverter> semanticTreeConverters = new List<ISemanticTreeConverter>();
        public List<ISemanticTreeConverter> SemanticTreeConverters
        {
            get
            {
                return semanticTreeConverters;
            }
        }

        public delegate void ChangeStateDelegate(State State, ISemanticTreeConverter SemanticTreeConverter);
        public event ChangeStateDelegate ChangeState;


        public SemanticTreeConvertersController(ICompiler Compiler)
        {
            this.Compiler = Compiler;
        }

        public void AddConverters()
        {
            AddConverters(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName));
        }
        private void AddConverters(string DirectoryName)
        {
            DirectoryInfo di = new DirectoryInfo(DirectoryName);
            FileInfo[] dllfiles = di.GetFiles("*Conversion.dll");
            System.Reflection.Assembly assembly = null;
            ISemanticTreeConverter Converter;
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
                                if (type.Name.IndexOf("SemanticTreeConverter") >= 0 && type.IsClass)
                                {
                                    Object obj = Activator.CreateInstance(type);
                                    if (obj is ISemanticTreeConverter)
                                    {
                                        Converter = obj as ISemanticTreeConverter;
                                        semanticTreeConverters.Add(Converter);
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

        public SemanticTree.IProgramNode Convert(SemanticTree.IProgramNode ProgramNode)
        {
            foreach (ISemanticTreeConverter SemanticTreeConverter in semanticTreeConverters)
            {
                ChangeState(State.Convert, SemanticTreeConverter);
                ProgramNode = SemanticTreeConverter.Convert(Compiler, ProgramNode);
            }
            return ProgramNode;
        }
    }
}
