using System;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler;
using PascalABCCompiler.Errors;
using PascalABCCompiler.Preprocessor_2.Errors;
using System.IO;
using PascalABCCompiler.SyntaxTree;
using System.Text.RegularExpressions;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.Preprocessor_2;

namespace PascalABCCompiler.Preprocessor2
{

    public class Preprocessor2 : IPreprocessor
    {
        public string DEFINE = "define";
        public string UNDEF = "undef";
        public string INCLUDE = "include";
        public string IFDEF = "ifdef";
        public string IFNDEF = "ifndef";
        public string ENDIF = "endif";
        public string ELSE = "else";
        public string ASP = "asp";
        public string ENDASP = "endasp";
        public string ASPDATA = "aspdata";
        
        PascalABCCompiler.Preprocessor2.PascalPreprocessor2LanguageParser preprocessor_parser = new PascalPreprocessor2LanguageParser();
        public StatesManager sm = new StatesManager();
        public List<compiler_directive> Directives = new List<compiler_directive>();
        public SourceContextMap SourceContextMap;
        public TextWriter tw;
        //public string SourceText;
        //public string[] SourceLines;
        public int CurrentLineNumber = 1;
        public int Position = 0;
        public List<Error> Errors;
        public string CurrentFileName;
        public List<string> ProcessedFileNames = new List<string>();

        Dictionary<string, string> DefineDirectives = new Dictionary<string, string>();
        
        //Aspects
        public string CurrentAspectDirective = "";
        public bool InAspect = false;
        public Dictionary<string, Aspect> AspectsMap = new Dictionary<string, Aspect>();
        public List<string> AspectsNames = new List<string>();
        //-------
        
        public int newLineLength = Environment.NewLine.Length;

        SourceFilesProviderDelegate sourceFilesProvider;
        public SourceFilesProviderDelegate SourceFilesProvider
        {
            get
            {
                return sourceFilesProvider;
            }
            set
            {
                sourceFilesProvider = value;
            }
        }

        List<compiler_directive> compilerDirectives = null;
        public List<compiler_directive> CompilerDirectives
        {
            get
            {
                return compilerDirectives;
            }
        }

        
        public void AddDefineDirective(string name, string text, compiler_directive _compiler_directive)
        {
            if (!DefineDirectives.ContainsKey(name))
            {
                DefineDirectives.Add(name, text);
            }
            else
            {
                Errors.Add(new NameDefineError(name, CurrentFileName, _compiler_directive.source_context));
            }
        }

        public void DeleteDefineDirective(string name, compiler_directive _compiler_directive)
        {
            if (name != null)
            {
                if (DefineDirectives.ContainsKey(name))
                {
                    DefineDirectives.Remove(name);
                }
                else
                {
                    Errors.Add(new NameUndefError(name, CurrentFileName, _compiler_directive.source_context));
                }
            }
        }

        public bool DefineDirectiveDefinded(string name)
        {
            return DefineDirectives.ContainsKey(name);
        }

        public string getFirstIdent(string s)
        {
            s.Trim();
            int n = s.IndexOf(' ');
            if (n > 0)
                return s.Substring(0, s.IndexOf(' '));
            else
                return s;
        }

        public string getSecondIdent(string s)
        {
            string s1 = getFirstIdent(s);
            return s.Substring(s1.Length, s.Length - s1.Length).Trim();
        }

        public Preprocessor2(SourceFilesProviderDelegate sourceFilesProvider)
        {
            this.sourceFilesProvider = sourceFilesProvider;
            this.sm.Reset();
        }

        public string Build(string[] fileNames, List<Error> errors, SourceContextMap sourceContextMap)
        {
            compilerDirectives = new List<compiler_directive>();
            StringWriter sw = new StringWriter();
            tw = sw;
            Errors = errors;
            SourceContextMap = sourceContextMap;
            //AspectsMap.Clear();
            ProcessedFileNames.Clear();
            DefineDirectives.Clear();
            foreach (string name in fileNames)
            {
                ProcessFile(name);
                if (sm.GetEndifCount() != 0)
                    Errors.Add(new UnexpectedEOF(CurrentFileName, new SourceContext(int.MaxValue, 1, int.MaxValue, 1)));
                if (errors.Count > 0)
                    return null;
            }
            return sw.ToString();
        }

        //Aspects
        public string GetAspects(List<compiler_directive> cd_list, SourceContextMap sourceContextMap)
        {
            try
            {
                compilerDirectives = new List<compiler_directive>(cd_list);
                //AspectsMap.Clear();

                //GetStructures                
                foreach (compiler_directive cd in compilerDirectives)
                {
                    if (cd.Name.text == ASP)
                    {
                        if (!(AspectsNames.Contains(cd.Directive.text)))
                        //if (!(AspectsMap.ContainsKey(cd.Directive.text)))
                        {
                            AspectsNames.Add(cd.Directive.text);
                            AspectsMap.Add(cd.Directive.text, new Aspect(cd.Directive.text));
                            AspectsMap[cd.Directive.text].AddBegin(cd.source_context);
                        }
                        else                        
                            AspectsMap[cd.Directive.text].AddBegin(cd.source_context);                       
                    }
                    //if (cd.Name.text == )
                }
                foreach (compiler_directive cd in compilerDirectives)
                {
                    if (cd.Name.text == ENDASP)
                    {
                        if (!AspectsNames.Contains(cd.Directive.text))
                        {
                            //Error - unexp end of aspect                        
                        }
                        else
                            AspectsMap[cd.Directive.text].AddEnd(cd.source_context);
                        //AspectsMap.Add(cd.Directive.text, new Aspect(cd.Directive.text, false, cd.source_context));                
                    }
                }
                //Get Text
                StringWriter sw = new StringWriter();
                //SourceContextMap = sourceContextMap;
                List<SourceContext> beg;
                List<SourceContext> end;
                string CurrentFileName;
                string CurrentFileText;

                foreach (string str in AspectsMap.Keys)
                {
                    beg = AspectsMap[str].GetBeginList();
                    end = AspectsMap[str].GetEndList();
              
                    for (int i = 0; i < beg.Count; i++)
                    {
                        CurrentFileName = beg[i].FileName;
                        CurrentFileText = (string)SourceFilesProvider(CurrentFileName, SourceFileOperation.GetText);
                        sw.WriteLine(CurrentFileText.Substring(beg[i].Position, end[i].Position - beg[i].Position + end[i].Length));
                    }                    
                    //AspectsMap[str].GetBeginList();
                }
                return sw.ToString();
            }
            catch (Exception e)
            {
                this.Errors.Add(new PascalABCCompiler.Errors.Error("Error in aspects reading"));
                return "Error in aspects reading";
            }
        }

        public string WriteAspects(string aspects, string proj)
        {

            StringWriter sw = new StringWriter();
            List<SourceContext> beg;
            List<SourceContext> end;
            string CurrentAspectText;
            string CurrentFileText;
            string CurrentFileName;
            string CurrentPath;
            if (proj != "")
                CurrentPath = proj + "\\";
            else
                CurrentPath = "";
                                    
            foreach (string str in AspectsMap.Keys)
            {                                    
                beg = AspectsMap[str].GetBeginList();
                end = AspectsMap[str].GetEndList();
                int asp_num = 0;
                int asp_num_file = 0;
                string asp_file_name = CurrentPath + beg[0].FileName;                                
                for (int i = 0; i < beg.Count; i++)
                {
                    ///Files names & asp_num?
                    CurrentFileName = CurrentPath + beg[i].FileName;
                    CurrentFileText = (string)SourceFilesProvider(CurrentFileName, SourceFileOperation.GetText);
                    if (asp_file_name != CurrentFileName)
                    {
                        asp_num_file = 0;
                        asp_file_name = CurrentFileName;
                    }
                    if (aspects.IndexOf("{$asp " + str + "}", asp_num)>=0)
                    {
                        CurrentAspectText = aspects.Substring(aspects.IndexOf("{$asp " + str + "}", asp_num), aspects.IndexOf("{$endasp " + str + "}", asp_num) - aspects.IndexOf("{$asp " + str + "}", asp_num) + ("{$endasp " + str + "}").Length);
                        asp_num = aspects.IndexOf("{$endasp " + str + "}", asp_num) + ("{$endasp " + str + "}").Length;

                        int d = CurrentFileText.IndexOf("{$asp " + str + "}", asp_num_file);
                        int c = CurrentFileText.IndexOf("{$endasp " + str + "}", asp_num_file);
                        if (d >= 0 && c >= 0)
                        {
                            CurrentFileText = CurrentFileText.Substring(0, asp_num_file) + CurrentFileText.Substring(asp_num_file, d - asp_num_file) + CurrentAspectText + CurrentFileText.Substring(c + ("{$endasp " + str + "}").Length);
                            
                            c = d + CurrentAspectText.Length;

                            asp_num_file = c + ("{$endasp " + str + "}").Length; ;
                        }
                        else
                            break;

                        StreamWriter swrt = new StreamWriter(Path.GetDirectoryName(CurrentFileName) + "\\" + Path.GetFileNameWithoutExtension(CurrentFileName) + ".pas", false, Encoding.UTF8);
                        swrt.Write(CurrentFileText);
                        swrt.Close();
                    }
                }                
            }
            return sw.ToString();
        }

        public string WriteAspect(string aspects, string proj, string str)
        {

            StringWriter sw = new StringWriter();
            List<SourceContext> beg;
            List<SourceContext> end;
            string CurrentAspectText;
            string CurrentFileText;
            string CurrentFileName;
            string CurrentPath;
            if (proj != "")
                CurrentPath = proj + "\\";
            else
                CurrentPath = "";

                beg = AspectsMap[str].GetBeginList();
                end = AspectsMap[str].GetEndList();
                int asp_num = 0;
                int asp_num_file = 0;
                string asp_file_name = CurrentPath + beg[0].FileName;
                for (int i = 0; i < beg.Count; i++)
                {
                    CurrentFileName = CurrentPath + beg[i].FileName;
                    CurrentFileText = (string)SourceFilesProvider(CurrentFileName, SourceFileOperation.GetText);
                    if (asp_file_name != CurrentFileName)
                    {
                        asp_num_file = 0;
                        asp_file_name = CurrentFileName;
                    }
                    if (aspects.IndexOf("{$asp " + str + "}", asp_num) >= 0)
                    {
                        CurrentAspectText = aspects.Substring(aspects.IndexOf("{$asp " + str + "}", asp_num), aspects.IndexOf("{$endasp " + str + "}", asp_num) - aspects.IndexOf("{$asp " + str + "}", asp_num) + ("{$endasp " + str + "}").Length);
                        asp_num = aspects.IndexOf("{$endasp " + str + "}", asp_num) + ("{$endasp " + str + "}").Length;

                        int d = CurrentFileText.IndexOf("{$asp " + str + "}", asp_num_file);
                        int c = CurrentFileText.IndexOf("{$endasp " + str + "}", asp_num_file);
                        if (d >= 0 && c >= 0)
                        {
                            CurrentFileText = CurrentFileText.Substring(0, asp_num_file) + CurrentFileText.Substring(asp_num_file, d - asp_num_file) + CurrentAspectText + CurrentFileText.Substring(c + ("{$endasp " + str + "}").Length);

                            c = d + CurrentAspectText.Length;

                            asp_num_file = c + ("{$endasp " + str + "}").Length;
                        }
                        else
                        {
                            break;
                        }
                        StreamWriter swrt = new StreamWriter(Path.GetDirectoryName(CurrentFileName) + "\\" + Path.GetFileNameWithoutExtension(CurrentFileName) + ".pas", false, Encoding.UTF8);
                        swrt.Write(CurrentFileText);
                        swrt.Close();
                    }
                }

            return sw.ToString();
        }

        //end aspects

        public void ProcessFile(string FileName)
        {
            ProcessedFileNames.Add(Path.GetFileName(FileName));
            string Text = (string)SourceFilesProvider(FileName, SourceFileOperation.GetText);
            CurrentFileName = FileName;
            //SourceText = Text;
            //string[] s ={ "\r\n" };
            //SourceLines = Text.Split(s, StringSplitOptions.None);           
            preprocessor_parser.Reset();
            preprocessor_parser.Errors = Errors;
            preprocessor_parser.parser.prepr = this;
            compilation_unit cu = preprocessor_parser.BuildTree(FileName, Text, PascalABCCompiler.Parsers.ParseMode.Normal) as compilation_unit;
            CurrentFileName = FileName;
            
            //if (cu.compiler_directives.Count != 0)
            //    CompilerDirectives.AddRange(cu.compiler_directives);
            
            ProcessedFileNames.Remove(FileName);
        }

        /*
        public void WriteToStream(int line1, int line2)
        {
            int startl = 1;
            int endl = SourceLines.Length;
            if (line1 >= 0)
                startl = line1;
            if (line2 >= 0)
                endl = line2;
            if (endl - startl < 0)
                return;
            int Length = 0;
            for (int i = startl; i <= endl; i++)
            {
                tw.WriteLine(SourceLines[i - 1]);
                Length += SourceLines[i - 1].Length + newLineLength;
            }
            SourceContext nsc = new SourceContext(CurrentLineNumber, 1, CurrentLineNumber + endl - startl, int.MaxValue, Position, Position + Length);
            SourceContext rsc = new SourceContext(startl, 1, endl, int.MaxValue);
            rsc.FileName = CurrentFileName;
            SourceContextMap.AddArea(nsc, rsc);
            CurrentLineNumber += endl - startl + 1;
            Position += Length;
        }
        
        public void WriteToStream(int line1, int line2, int col1, int col2)
        {
            int startl = 1;
            int endl = SourceLines.Length;
            if (line1 >= 0)
                startl = line1;
            if (line2 >= 0)
                endl = line2;
            if (endl - startl < 0)
                return;
            int Length = 0;
            for (int i = startl; i <= endl; i++)
            {
                string s = (SourceLines[i - 1]).Substring(col1 - 1, col2 - col1 + 1);
                Length += s.Length + newLineLength;
            }
            SourceContext nsc = new SourceContext(CurrentLineNumber, col1 - 1, CurrentLineNumber + endl - startl, int.MaxValue, Position, Position + Length);
            SourceContext rsc = new SourceContext(startl, 1, endl, int.MaxValue);
            rsc.FileName = CurrentFileName;
            SourceContextMap.AddArea(nsc, rsc);
            CurrentLineNumber += endl - startl + 1;
            Position += Length;
        }
        */

        public void WriteToStream(SourceContext sc, string text)
        {
            tw.WriteLine(text);
            sc.FileName = CurrentFileName;
            SourceContextMap.AddArea(new SourceContext(CurrentLineNumber, 1, CurrentLineNumber + sc.end_position.line_num - sc.begin_position.line_num, sc.end_position.column_num, Position, Position + text.Length), sc);
            CurrentLineNumber += 1;// sc.end_position.line_num - sc.begin_position.line_num + 1;
            Position += text.Length;

        }

    }

    public class StatesManager
    {
        private Stack<int> if_stack;
        private Stack<int> endif_stack;
        bool allow_write;
        bool allow_else;

        public StatesManager()
        {
            allow_write = true;
            allow_else = false;
            if_stack = new Stack<int>();
            endif_stack = new Stack<int>();
        }

        public void Reset()
        {
            allow_write = true;
            if_stack.Clear();
            endif_stack.Clear();
        }

        public void AddCondition(int i)
        {
            allow_else = true;
            if_stack.Push(i);
            endif_stack.Push(i);
        }

        public int GetIfCount()
        {
            return if_stack.Count;
        }

        public int GetEndifCount()
        {
            return endif_stack.Count;
        }

        public int DeleteCondition(bool endif)
        {
            int i = 0;
            if (endif)
            {
                allow_else = true;
                i = endif_stack.Pop();
            }
            else
                allow_else = false;
            if (if_stack.Count != 0)
                return if_stack.Pop();
            else
            {
                //error
                return i;
            }
        }

        public int LastCondition()
        {
            if (if_stack.Count != 0)
                return if_stack.Peek();
            else
                return 0;
        }

        public int LastEndifCondition()
        {
            if (endif_stack.Count != 0)
                return endif_stack.Peek();
            else
                return 0;
        }

        public bool AllowWrite()
        {
            return allow_write;
        }

        public bool AllowElse()
        {
            return allow_else;
        }

        public void ChangeState(bool new_mode)
        {
            allow_write = new_mode;
        }

        public void ChangeState(bool new_mode, int new_if)
        {
            allow_write = new_mode;
            AddCondition(new_if);
        }

        public void ChangeState(bool new_mode, bool endif)
        {
            allow_write = new_mode;
            int i = DeleteCondition(endif);
            // return i;
        }

    }

    //Aspects
    public class Aspect
    {
        private string AspName;
        private string AspAuthor = "name";
        private string AspVersion = "0.1";        
        private List<SourceContext> begin_list;
        private List<SourceContext> end_list;
        bool allow_mod;
        bool saved_params;

        public Aspect(string name)
        {
            this.AspName = name;
            begin_list = new List<SourceContext>();
            end_list = new List<SourceContext>();
            allow_mod = true;
        }

        public Aspect(string name, string auth, string ver)
        {
            this.AspName = name;
            this.AspAuthor = auth;
            this.AspVersion = ver;
            begin_list = new List<SourceContext>();
            end_list = new List<SourceContext>();
            allow_mod = true;
        }

        public void AddBegin(SourceContext sc)
        {
            if (!begin_list.Contains(sc))
                begin_list.Add(sc);
        }

        public void AddEnd(SourceContext sc)
        {
            if (!end_list.Contains(sc))
                end_list.Add(sc);
        }

        public bool IsSavedParams
        {
            set
            {
                IsSavedParams = value;
            }
            get
            {
                return saved_params;
            }
        }

        public string Author
        {
            set
            {
                AspAuthor = value;
            }
            get
            {
                return AspAuthor;
            }
        }

        public string Version
        {
            set
            {
                AspVersion = value;
            }
            get
            {
                return AspVersion;
            }
        }

        public List<SourceContext> GetBeginList()
        {
            return new List<SourceContext>(begin_list);
        }

        public List<SourceContext> GetEndList()
        {
            return new List<SourceContext>(end_list);
        }

        public void Reset()
        {
            allow_mod = true;
            begin_list.Clear();
            end_list.Clear();
        }
    }
    //end aspects

}