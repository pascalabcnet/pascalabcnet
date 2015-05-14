using System;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler;
using PascalABCCompiler.Errors;
using System.IO;
using PascalABCCompiler.CPreprocessor;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.CPreprocessor.Errors;
using System.Text.RegularExpressions;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.Parsers;

namespace PascalABCCompiler.CParser
{
    class DefineDirective
    {
        public string Name;
        public string FormalParams;
        string[] FormalParamsArray;
        public string Text;
        public char FormalParamsSeparator;
        public int FormalParamsCount
        {
            get
            {
                if (FormalParamsArray == null)
                    return 0;
                return FormalParamsArray.Length;
            }
        }

        
        public DefineDirective(string Name, string FormalParams, string Text, char FormalParamsSeparator)
        {
            this.Name = Name;
            this.FormalParamsSeparator = FormalParamsSeparator;
            this.FormalParams = FormalParams;
            if(FormalParams!=null)
                FormalParamsArray = FormalParams.Split(FormalParamsSeparator);
            this.Text = Text;
        }
        public string GetTextInstance(string[] FactParamsArray)
        {
            if (FactParamsArray.Length == 0 && FormalParamsArray == null)
                return Text;
            if(FactParamsArray.Length!=FormalParamsArray.Length)
                return null;
            string res = Text;
            for(int i=0;i<FactParamsArray.Length;i++)
                res = System.Text.RegularExpressions.Regex.Replace(res, @"\b" + FormalParamsArray[i] + @"\b", FactParamsArray[i]);
            return res;            
        }
    }
    public class Preprocessor : SyntaxTree.AbstractVisitor, IPreprocessor
    {
        public string DEFINE = "#define";
        public string UNDEF = "#undef";
        public string INCLUDE = "#include";
        public string IFDEF = "#ifdef";
        public string IFNDEF = "#ifndef";
        public string ELIF = "#elif";
        public string IF = "#if";
        public string ERROR = "#error";
        public static char FormalParamsSeparator = ',';
        public static bool AllowDefineFunctionsOverloading = true;

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

        List<compiler_directive> compilerDirectives=null;
        public List<compiler_directive> CompilerDirectives
        {
            get
            {
                return compilerDirectives;
            }
        }

        CPreprocessorLanguageParser CPreprocessorParser;
        SourceContextMap SourceContextMap;
        TextWriter tw;
        string SourceText;
        string[] SourceLines;
        int CurrentLineNumber = 1;
        int Position = 0;
        List<Error> Errors;
        string CurrentFileName;
        string[] SearchPaches;
        List<string> ProcessedFileNames = new List<string>();
        Dictionary<string, Dictionary<int, DefineDirective>> DefineDirectives = new Dictionary<string, Dictionary<int, DefineDirective>>();
        void AddDefineDirective(DefineDirective d)
        {
            Dictionary<int,DefineDirective> Defs = null;
            if (!DefineDirectives.ContainsKey(d.Name))
            {
                Defs = new Dictionary<int, DefineDirective>();
                DefineDirectives.Add(d.Name, Defs);
            }
            else
            {
                Defs = DefineDirectives[d.Name];
                if (!AllowDefineFunctionsOverloading)
                    Defs.Clear();
            }
            if (Defs.ContainsKey(d.FormalParamsCount))
                Defs[d.FormalParamsCount] = d;
            else
                Defs.Add(d.FormalParamsCount, d);
        }
        string GetDefineDirectiveInstance(string name)
        {
            Dictionary<int, DefineDirective> Defs = null;
            if (!DefineDirectives.ContainsKey(name))
            {
                return null;
            }
            if (DefineDirectives[name].ContainsKey(0))
                return DefineDirectives[name][0].Text;
            return null;
        }
        string GetDefineDirectiveInstance(string name, string formal_params)
        {

            Dictionary<int, DefineDirective> Defs = null;
            if (!DefineDirectives.ContainsKey(name))
            {
                return null;
            }
            Defs = DefineDirectives[name];
            string[] factparams = new string[0];
            if (formal_params != null)
                factparams = formal_params.Split(FormalParamsSeparator);
            if (Defs.ContainsKey(factparams.Length))
            {
                return Defs[factparams.Length].GetTextInstance(factparams);
            }
            return null;
        }
        bool DefineDirectiveDefinded(string name)
        {
            return DefineDirectives.ContainsKey(name);
        }
        public Preprocessor(SourceFilesProviderDelegate SourceFilesProvider)
        {
            this.SourceFilesProvider = SourceFilesProvider;
            CPreprocessorParser = new CPreprocessorLanguageParser();
            CPreprocessorParser.Reset();
        }
        public string Build(string[] fileNames,string[] SearchPaches, List<Error> errors, SourceContextMap sourceContextMap)
        {
            compilerDirectives = new List<compiler_directive>();
            StringWriter sw = new StringWriter();
            tw = sw;
            Errors = errors;
            SourceContextMap = sourceContextMap;
            this.SearchPaches=SearchPaches;
            ProcessedFileNames.Clear();
            DefineDirectives.Clear();
            foreach (string name in fileNames)
            {
                ProcessFile(name);
                if (errors.Count > 0)
                    return null;
            }
            return sw.ToString();
        }
        void ProcessFile(string FileName)
        {
            ProcessedFileNames.Add(FileName);
            CPreprocessorParser.Errors = Errors;
            string Text = (string)SourceFilesProvider(FileName, SourceFileOperation.GetText);
            compilation_unit cu = CPreprocessorParser.BuildTree(FileName, Text, SearchPaches, PascalABCCompiler.Parsers.ParseMode.Normal) as compilation_unit;
            if (Errors.Count > 0)
                return;
            if (cu == null)
                Errors.Add(new CompilerInternalError("CPreprocessor", new Exception("cu==null")));
            if (cu != null)
                compilerDirectives.AddRange(cu.compiler_directives);
            //irectivesVisitor dv = new DirectivesVisitor(tw, Text, FileName, SourceContextMap, SourceFilesProvider, Errors);
            CurrentFileName = FileName;
            SourceText = Text;
            string[] s ={ "\r\n" };
            SourceLines = Text.Split(s, StringSplitOptions.None);
            cu.visit(this);
            ProcessedFileNames.Remove(FileName);
        }

        SourceContext GetFragment(compiler_directive cd1, compiler_directive cd2)
        {
            int beg = 0, end = SourceText.Length;
            int begl = 1, begc = 1, endl = int.MaxValue, endc = int.MaxValue;
            if (cd1 != null)
            {
                beg = cd1.source_context.Position + cd1.source_context.Length;
                begl = cd1.source_context.end_position.line_num + 1;
                begc = 1;
                //endl = begl;
                //endc = begc; 
            }
            if (cd2 != null)
            {
                end = cd2.source_context.Position;
                endl = cd2.source_context.begin_position.line_num;
                endc = cd2.source_context.begin_position.column_num;
            }
            SourceContext nsc = new SourceContext(begl, begc, endl, endc, beg, end);
            nsc.FileName = CurrentFileName;
            return nsc;
        }
        int newLineLength = Environment.NewLine.Length;
        void WriteTextFragment(int line1, int line2)
        {
            /*SourceContext tf = GetFragment(cd1, cd2);
            if (tf.Length == 0) return;
            string s = SourceText.Substring(tf.Position, tf.Length);
            tw.Write(s);
            SourceContext nsc = new SourceContext(CurrentLineNumber, tf.begin_position.column_num, CurrentLineNumber + tf.end_position.line_num - tf.begin_position.line_num, tf.end_position.column_num, Position, Position + s.Length);
            SourceContextMap.AddArea(nsc, tf);
            CurrentLineNumber += tf.end_position.line_num - tf.begin_position.line_num;
            Position += s.Length;*/
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
                ProcessDefineDirectives(i - 1);
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
        void WriteTextFragment(SourceContext sc, string text)
        {
            text = ProcessDefineDirectives(text);
            tw.Write(text);
            sc.FileName = CurrentFileName;
            SourceContextMap.AddArea(new SourceContext(CurrentLineNumber, sc.begin_position.column_num, CurrentLineNumber + sc.end_position.line_num - sc.begin_position.line_num, sc.end_position.column_num, Position, Position + text.Length), sc);
            CurrentLineNumber += sc.end_position.line_num - sc.begin_position.line_num + 1;
            Position += text.Length;
        }

        void prepareDirectivesList(List<compiler_directive> cds)
        {
            compiler_directive last = null;
            foreach (compiler_directive cd in cds)
            {
                if (last != null)
                {
                    WriteTextFragment(last.source_context.end_position.line_num + 1, cd.source_context.begin_position.line_num - 1);
                }
                cd.visit(this);
                last = cd;
            }
        }
        public override void visit(compilation_unit _compilation_unit)
        {
            if (_compilation_unit.compiler_directives.Count == 0)
            {
                if (SourceText.Length > 0)
                {
                    if (SourceText.Length<=2 || (SourceText.Substring(SourceText.Length - newLineLength, newLineLength) != Environment.NewLine))
                        SourceText += Environment.NewLine;
                    WriteTextFragment(_compilation_unit.source_context, SourceText);
                }
                return;
            }
            WriteTextFragment(-1, _compilation_unit.compiler_directives[0].source_context.begin_position.line_num-1);
            prepareDirectivesList(_compilation_unit.compiler_directives);
            WriteTextFragment(_compilation_unit.compiler_directives[_compilation_unit.compiler_directives.Count - 1].source_context.end_position.line_num+1, -1);
        }
        string FindInDirectoris(string[] Dirs, string FileName)
        {
            if (Dirs == null)
                return null;
            foreach (string d in Dirs)
            {
                if ((bool)SourceFilesProvider(d + "\\" + FileName, SourceFileOperation.Exists))
                    return d;
            }
            return null;
        }
        string getFirstIdent(string s)
        {
            bool identnow = false;
            int startindex = 0;
            string ident = null;
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if ((char.IsLetterOrDigit(c) || c=='_') && !identnow)
                {
                    identnow = true;
                    startindex = i;
                }
                else
                    if (identnow)
                        ident = s.Substring(startindex, i - startindex - 1);
            }
            return ident;
        }
        public override void visit(compiler_directive_list _compiler_directive_list)
        {
            if (_compiler_directive_list.directives.Count == 0)
            {
                WriteTextFragment(_compiler_directive_list.source_context, SourceText.Substring(_compiler_directive_list.source_context.Position, _compiler_directive_list.source_context.Length));
                return;
            }
            WriteTextFragment(_compiler_directive_list.source_context.begin_position.line_num, _compiler_directive_list.directives[0].source_context.begin_position.line_num - 1);
            prepareDirectivesList(_compiler_directive_list.directives);
            WriteTextFragment(_compiler_directive_list.directives[_compiler_directive_list.directives.Count - 1].source_context.end_position.line_num + 1, _compiler_directive_list.source_context.end_position.line_num);
        }
        
        private static CParser.CLanguageParser ExpressionParser = null;
        public override void visit(compiler_directive_if _compiler_directive_if)
        {

            if (_compiler_directive_if.Name.text == IFDEF || _compiler_directive_if.Name.text == IFNDEF)
            {
                string ident = getFirstIdent(_compiler_directive_if.Directive.text);
                bool defined = DefineDirectives.ContainsKey(ident);
                if (_compiler_directive_if.Name.text == IFNDEF)
                    defined = !defined;
                if (defined)
                {
                    if (_compiler_directive_if.if_part != null)
                        _compiler_directive_if.if_part.visit(this);
                }
                else
                {
                    if (_compiler_directive_if.elseif_part != null)
                        _compiler_directive_if.elseif_part.visit(this);
                }
                return;
            }
            if (_compiler_directive_if.Name.text == ELIF || _compiler_directive_if.Name.text == IF)
            {
                if (ExpressionParser == null)
                {
                    ExpressionParser = new CLanguageParser();
                    ExpressionParser.Reset();
                    ExpressionParser.Errors = new List<Error>();
                }
                ExpressionParser.Errors.Clear();
                string expr_source = ProcessDefineDirectives(_compiler_directive_if.Directive.text);
                expression expr = ExpressionParser.BuildTree(CurrentFileName, expr_source, null, PascalABCCompiler.Parsers.ParseMode.Expression) as expression;
                ConstantExpressionToInt32ConstantConvertor conv = new ConstantExpressionToInt32ConstantConvertor();
                int32_const ic = conv.Convert(expr);
                if (ic == null || ExpressionParser.Errors.Count > 0)
                {
                    Errors.Add(new InvalidIntegerExpression(CurrentFileName, _compiler_directive_if.Directive.source_context.LeftSourceContext));
                    return;
                }
                if (ic.val != 0)
                {
                    if (_compiler_directive_if.if_part != null)
                        _compiler_directive_if.if_part.visit(this);
                }
                else
                {
                    if (_compiler_directive_if.elseif_part != null)
                        _compiler_directive_if.elseif_part.visit(this);
                }
                //Errors.Add(new PascalABCCompiler.CParser.Errors.nonterminal_token_return_null(CurrentFileName, _compiler_directive_if.source_context, _compiler_directive_if, _compiler_directive_if.Name + " <constant_expression>"));
                return;
            }
        }
        public override void visit(compiler_directive _compiler_directive)
        {
            if (_compiler_directive.Name.text == INCLUDE)
            {
                string FileName = _compiler_directive.Directive.text.TrimStart(' ');
                try
                {
                    int i1=0,i2=0;
                    if ((i1=FileName.IndexOf("<")) >= 0 && (i2=FileName.IndexOf(">")) >= 0)
                    {
                        FileName = FileName.Substring(i1 + 1, i2 - i1 - 1);
                        string dir = FindInDirectoris(SearchPaches, FileName);
                        if (dir != null)
                        {
                            FileName = dir + "\\" + FileName;
                        }
                        else
                        {
                            Errors.Add(new IncludeFileNotFound(CurrentFileName, _compiler_directive.source_context, null, FileName));
                            return;
                        }
                    }
                    else
                    {
                        FileName = FileName.Split('"')[1];
                        if (Path.GetDirectoryName(FileName) == String.Empty)
                            FileName = Path.GetDirectoryName(CurrentFileName) + "\\" + FileName;
                        if (!(bool)SourceFilesProvider(FileName, SourceFileOperation.Exists))
                        {
                            Errors.Add(new IncludeFileNotFound(CurrentFileName, _compiler_directive.source_context, null, FileName));
                            return;
                        }
                    }
                    string b = CurrentFileName;
                    string t = SourceText;
                    string[] sl = SourceLines;
                    if (ProcessedFileNames.Contains(FileName))
                    {
                        Errors.Add(new CircularInclude(CurrentFileName, _compiler_directive.source_context, null, FileName));
                        return;
                    }
                    ProcessFile(FileName);
                    CurrentFileName = b;
                    SourceText = t;
                    SourceLines = sl;
                    //ProcessDefineDirectives(_compiler_directive.source_context.end_position.line_num + 1);

                }
                catch (Exception)
                {
                    Errors.Add(new ErrorInIncludeSyntax(CurrentFileName, _compiler_directive.source_context));
                }
                return;
            }
            if (_compiler_directive.Name.text == DEFINE)
            {
                string directive = _compiler_directive.Directive.text.Replace("\\\r\n", "");
                directive = directive.TrimStart(' ');
                directive = directive.Replace("\r\n", "");
                string formal_params=null;
                string defname = null;
                string deftext = string.Empty;
                bool formal_params_now = false;
                int defname_start_index=-1;
                int formal_prarams_start_index = -1;
                int i = 0;
                while (true)
                {
                    if (i == directive.Length)
                    {
                        Errors.Add(new ErrorInDefineSyntax(CurrentFileName, _compiler_directive.source_context));
                        return;
                    }
                    char c = directive[i];
                    if (Char.IsWhiteSpace(c))
                    {
                        if (formal_params_now)
                        {
                            i++;
                            continue;
                        }
                        if (defname_start_index >= 0)
                        {
                            defname = directive.Substring(defname_start_index, i - defname_start_index);
                            deftext = directive.Substring(i + 1);
                            break;
                        }
                    }
                    if (Char.IsDigit(c) && defname_start_index == -1)
                    {
                        Errors.Add(new ErrorInDefineSyntax(CurrentFileName, _compiler_directive.source_context));
                        return;
                    }
                    if (Char.IsLetterOrDigit(c) || c=='_')
                    {
                        if(defname_start_index == -1)
                            defname_start_index = i;
                        if (i == directive.Length - 1 && !formal_params_now)
                        {
                            defname = directive.Substring(defname_start_index, i - defname_start_index+1);
                            deftext = string.Empty;
                            break;
                        }
                        i++;
                        continue;
                    }
                    if (c == '(' && !formal_params_now)
                    {
                        formal_params_now = true;
                        defname = directive.Substring(defname_start_index, i - defname_start_index);
                        formal_prarams_start_index = i + 1;
                        i++;
                        continue;
                    }
                    if (c == ')' && formal_params_now)
                    {
                        formal_params = directive.Substring(formal_prarams_start_index, i - formal_prarams_start_index);
                        deftext = directive.Substring(i + 1);
                        break;
                    }
                    if (c == ',' && formal_params_now)
                    {
                        i++;
                        continue;
                    }
                    Errors.Add(new ErrorInDefineSyntax(CurrentFileName, _compiler_directive.source_context));
                    break;
                }
                deftext = ProcessDefineDirectives(deftext);
                AddDefineDirective(new DefineDirective(defname,formal_params,deftext, FormalParamsSeparator));
                return;
            }
            if (_compiler_directive.Name.text == UNDEF)
            {
                string dir = _compiler_directive.Directive.text;
                string ident = getFirstIdent(dir);
                if (ident != null)
                    if (DefineDirectives.ContainsKey(ident))
                        DefineDirectives.Remove(ident);
                return;
            }
            if (_compiler_directive.Name.text == ERROR)
            {
                Errors.Add(new ErrorDirective(CurrentFileName, _compiler_directive.source_context, _compiler_directive.Directive.text.Replace("\r\n", "")));
                return;
            }
            Errors.Add(new InvalidPreprocessorCommand(CurrentFileName, _compiler_directive.source_context, _compiler_directive.Name.text));
        }
        string trunc_left_paren(string val)
        {
            int c=0;
            for (int i = 0; i < val.Length; i++)
                if (val[i] == '(')
                    c++;
                else
                    if (val[i] == ')')
                    {
                        c--;
                        if (c == 0)
                            return val.Substring(0, i+1);
                    }
            return val;
        }
        void ProcessDefineDirectives(int LineBeg,int LineEnd)
        {
            for (int i=LineBeg;i<=LineEnd;i++)
                ProcessDefineDirectives(i);
        }
        void ProcessDefineDirectives(int Line)
        {
            SourceLines[Line] = ProcessDefineDirectives(SourceLines[Line]);
        }
        class string_fragment
        {
            public int index, length;
            public string_fragment(int index, int length)
            {
                this.index = index;
                this.length = length;
            }
            public string_fragment(Match m)
            {
                this.index = m.Index;
                this.length = m.Length;
            }
        }
        bool match_in_string_fragment(string_fragment m, string_fragment sf)
        {
            return (m.index >= sf.index) && (m.index + m.length <= sf.index + sf.length);
        }
        List<string_fragment> splitToStrings(string line)
        {
            List<string_fragment> res = new List<string_fragment>();
            if(line.IndexOf('"')==-1 && line.IndexOf('\'') ==-1)
                return res;
            MatchCollection mc = Regex.Matches(line, "(\"(?=[^\"]*\").*?\")|('(?=[^\']*\').*?\')", RegexOptions.Compiled);
            foreach (Match m in mc)
                res.Add(new string_fragment(m.Index, m.Length));
            return res;
        }
        bool in_string_fragment_list(List<string_fragment> list, string_fragment m)
        {
            foreach (string_fragment sf in list)
                if (match_in_string_fragment(m, sf)) 
                    return true;
            return false;
        }
        static Dictionary<int, string> _emptyStrings = new Dictionary<int, string>();
        string GetEmpty(int count)
        {
            if(_emptyStrings.ContainsKey(count))
                return _emptyStrings[count];
            string s = new string(' ', count);
            _emptyStrings.Add(count, s);
            return s;
        }
        string deleteStrings(string line)
        {
            MatchCollection mc = Regex.Matches(line, "(\"(?=[^\"]*\").*?\")|('(?=[^\']*\').*?\')", RegexOptions.Compiled);
            if (mc.Count == 0) 
                return line;
            StringBuilder sb = new StringBuilder();
            int last = 0;
            foreach (Match m in mc)
            {
                sb.Append(line.Substring(last, m.Index - last) + GetEmpty(m.Length));
                last = m.Index + m.Length;
            }
            sb.Append(line.Substring(last, line.Length - last));
            return sb.ToString();
        }
        string ProcessDefineDirectives(string SourceLine)
        {
            if (DefineDirectives.Count == 0)
                return SourceLine;
            foreach (string dname in DefineDirectives.Keys)
            {
                string SourceLineNoStrings = deleteStrings(SourceLine);
                SortedList<int, Match> mcl = new SortedList<int, Match>();
                MatchCollection mc = Regex.Matches(SourceLineNoStrings, @"\b" + dname + @"\b", RegexOptions.Compiled);
                foreach (Match m in mc)
                    mcl.Add(m.Index,m);
                mc = Regex.Matches(SourceLineNoStrings, @"\b" + dname + @"\("+@"(?=[^\)]*\)).*?"+@"\)", RegexOptions.Compiled);
                foreach (Match m in mc)
                    if (mcl.ContainsKey(m.Index))
                        mcl[m.Index] = m;
                    else
                        mcl.Add(m.Index,m);
                if (mcl.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    int last = 0;
                    foreach(int i in mcl.Keys)
                    {
                        Match m = mcl[i];
                        string fp = null;
                        string val = m.Value;
                        if (m.Value.IndexOf('(') > 0)
                        {
                            val = trunc_left_paren(m.Value);
                            fp = val.Substring(dname.Length + 1, val.Length - dname.Length - 2);
                        }
                        string di = GetDefineDirectiveInstance(dname, fp);
                        if (di == null)
                        {
                            di = GetDefineDirectiveInstance(dname);
                            if (di != null)
                                val = dname;
                        }
                        if (di != null)
                        {
                            sb.Append(SourceLine.Substring(last, m.Index - last) + di);
                            last = m.Index + val.Length;
                        }
                    }
                    sb.Append(SourceLine.Substring(last, SourceLine.Length - last));
                    SourceLine = sb.ToString();
                }
                /*
                if (d.FormalParams == null)
                {
                    MatchCollection mc = Regex.Matches(SourceLineNoStrings, @"\b" + d.Name + @"\b", RegexOptions.Compiled);
                    if (mc.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        int last = 0;
                        foreach (Match m in mc)
                        {
                            sb.Append(SourceLine.Substring(last, m.Index - last) + d.Text);
                            last = m.Index + m.Length;
                        }
                        sb.Append(SourceLine.Substring(last, SourceLine.Length - last));
                        SourceLine = sb.ToString();
                    }
                }
                else
                {
                    MatchCollection mc = Regex.Matches(SourceLineNoStrings, @"\b" + d.Name + @"\(.*\)", RegexOptions.Compiled);
                    if (mc.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        int last = 0;
                        foreach (Match m in mc)
                        {
                            string val = trunc_left_paren(m.Value);
                            string rp = val.Substring(d.Name.Length + 1, val.Length - d.Name.Length - 2);
                            string inst = d.GetTextInstance(rp);
                            if (inst != null)
                            {
                                sb.Append(SourceLine.Substring(last, m.Index - last) + inst);
                                last = m.Index + val.Length;
                            }
                        }
                        sb.Append(SourceLine.Substring(last, SourceLine.Length - last));
                        SourceLine = sb.ToString();
                    }
                }
                 */
            }
            return SourceLine;
        }

    }

    class TextFragment
    {
        public int StratIndex, Count;
        public TextFragment(int stratIndex, int count)
        {
            StratIndex = stratIndex;
            Count = count;
        }
    }
    
}
