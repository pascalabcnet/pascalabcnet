// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.Errors;
using System.IO;
using PascalABCCompiler;
using PascalABCCompiler.TreeConverter;
//using ICSharpCode.SharpDevelop.Dom;

namespace CodeCompletion
{
	//#if (DEBUG)
    public class CodeCompletionTester
    {
    	
    	public static void Test()
    	{
    		TestExpressionExtract();
            //TestVBNETExpressionExtract();
            //TestIntellisense(Path.Combine(@"c:\Work\Miks\_PABCNETGitHub\TestSuite", "intellisense_tests"));
    	}
    	
        public static void TestIntellisense(string dir)
        {
            //string dir = Path.Combine(@"c:\Work\Miks\_PABCNETGitHub\TestSuite", "intellisense_tests");
            var comp = new PascalABCCompiler.Compiler();
            var controller = new CodeCompletion.CodeCompletionController();
            CodeCompletion.DomSyntaxTreeVisitor.use_semantic_for_intellisense = true;
            CodeCompletion.CodeCompletionController.comp = comp;
            CodeCompletion.CodeCompletionController.SetParser(compiler_string_consts.pascalSourceFileExtension);
            CodeCompletion.CodeCompletionController.ParsersController = comp.ParsersController;
            var files = Directory.GetFiles(dir, "*.pas");
            var parser = comp.ParsersController;
            for (int i = 0; i < files.Length; i++)
            {
                var FileName = files[i];
                var content = File.ReadAllText(FileName);
                var dc = controller.Compile(FileName, content);
                
                string expr_without_brackets = null;
                var tmp = content;
                var ind = -1;
                ind = tmp.IndexOf("{@");
                while (ind != -1)
                {
                    var lines = tmp.Split(new string[] { System.Environment.NewLine }, System.StringSplitOptions.None);
                    var pos = ind-1;
                    var line = GetLineByPos(lines, pos);
                    var col = GetColByPos(lines, pos);
                    var desc = CodeCompletion.CodeCompletionTester.GetDescription(pos, tmp, line, col, FileName, dc, comp.ParsersController);
                    var should_desc = tmp.Substring(ind + 2, tmp.IndexOf("@}") - ind - 2);
                    if (desc == null)
                        desc = "";
                    desc = desc.Split(new string[] { "\n"},StringSplitOptions.None)[0].Trim();
                    assert(string.Compare(desc.Replace(", ", ","), should_desc.Replace(", ", ","), true) == 0, FileName+", should: "+should_desc+", is: "+desc);
                    tmp = tmp.Remove(ind, tmp.IndexOf("@}") + 2 - ind);
                    ind = tmp.IndexOf("{@");
                }

                ind = tmp.IndexOf("{[");
                while (ind != -1)
                {
                    var lines = tmp.Split(new string[] { System.Environment.NewLine }, System.StringSplitOptions.None);
                    var pos = ind - 1;
                    var line = GetLineByPos(lines, pos);
                    var col = GetColByPos(lines, pos);
                    var desc = CodeCompletion.CodeCompletionTester.GetIndexDescription(pos, tmp, line, col, FileName, dc, comp.ParsersController);
                    var should_desc = tmp.Substring(ind + 2, tmp.IndexOf("]}") - ind - 2);
                    if (desc == null)
                        desc = "";
                    desc = desc.Split(new string[] { "\n" }, StringSplitOptions.None)[0].Trim();
                    assert(string.Compare(desc.Replace(", ", ","), should_desc.Replace(", ", ","), true) == 0, FileName + ", should: " + should_desc + ", is: " + desc);
                    tmp = tmp.Remove(ind, tmp.IndexOf("]}") + 2 - ind);
                    ind = tmp.IndexOf("{[");
                }

                ind = tmp.IndexOf("{(");
                while (ind != -1)
                {
                    var lines = tmp.Split(new string[] { System.Environment.NewLine }, System.StringSplitOptions.None);
                    var pos = ind - 1;
                    var line = GetLineByPos(lines, pos);
                    var col = GetColByPos(lines, pos);
                    var desc = CodeCompletion.CodeCompletionTester.GetMethodDescription(pos, tmp, line, col, FileName, dc, comp.ParsersController, 1, 1);
                    var should_desc = tmp.Substring(ind + 2, tmp.IndexOf(")}") - ind - 2);
                    if (desc == null)
                        desc = "";
                    desc = desc.Split(new string[] { "\n" }, StringSplitOptions.None)[0].Trim();
                    assert(string.Compare(desc.Replace(", ", ","), should_desc.Replace(", ", ","), true) == 0, FileName + ", should: " + should_desc + ", is: " + desc);
                    tmp = tmp.Remove(ind, tmp.IndexOf(")}") + 2 - ind);
                    ind = tmp.IndexOf("{(");
                }
            }
        }

        public static void TestRename(string dir)
        {
            //string dir = Path.Combine(@"c:\Work\Miks\_PABCNETGitHub\TestSuite", "intellisense_tests");
            var comp = new PascalABCCompiler.Compiler();
            var controller = new CodeCompletion.CodeCompletionController();
            CodeCompletion.CodeCompletionController.comp = comp;
            CodeCompletion.CodeCompletionController.SetParser(compiler_string_consts.pascalSourceFileExtension);
            CodeCompletion.CodeCompletionController.ParsersController = comp.ParsersController;
            var files = Directory.GetFiles(dir, "*.pas");
            var parser = comp.ParsersController;
            for (int i = 0; i < files.Length; i++)
            {
                var FileName = files[i];
                var content = File.ReadAllText(FileName);
                var dc = controller.Compile(FileName, content);

                var tmp = content;
                var ind = -1;
                ind = content.IndexOf("{!}");
                var shouldPositions = new List<Position>();
                var lines = content.Split(new string[] { System.Environment.NewLine }, System.StringSplitOptions.None);
                var cu = controller.ParseOnlySyntaxTree(FileName, content);

                while (ind != -1)
                {
                    var pos = ind + 3;
                    var line = GetLineByPos(lines, pos);
                    var col = GetColByPos(lines, pos);
                    shouldPositions.Add(new Position(line+1, col+1, 0, 0, null));
                    ind = content.IndexOf("{!}", pos);
                }
                ind = content.IndexOf("{@}");
                if (ind != -1)
                {
                    var pos = ind + 3;
                    var line = GetLineByPos(lines, pos);
                    var col = GetColByPos(lines, pos);
                    shouldPositions.Add(new Position(line + 1, col + 1, 0, 0, null));
                    string expr_without_brackets = null;
                    PascalABCCompiler.Parsers.KeywordKind keyw = PascalABCCompiler.Parsers.KeywordKind.None;
                    string full_expr = CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.FindExpressionFromAnyPosition(pos, content, line, col, out keyw, out expr_without_brackets);
                    List<PascalABCCompiler.Errors.Error> Errors = new List<PascalABCCompiler.Errors.Error>();
                    var errors = new List<PascalABCCompiler.Errors.Error>();
                    expression expr = comp.ParsersController.GetExpression("test" + System.IO.Path.GetExtension(FileName), full_expr, errors, new List<PascalABCCompiler.Errors.CompilerWarning>());
                    //expression expr = new ident(expr_without_brackets.Replace("{@}","").Replace("new ","").Trim());
                    var fnd_scope = dc.GetSymDefinition(expr, line, col, keyw);
                    var rf = new CodeCompletion.ReferenceFinder(fnd_scope, dc.visitor.entry_scope, cu, FileName, true);
                    var positions = rf.FindPositions();
                    compare_positions(FileName, shouldPositions.ToArray(), positions);
                }

                ind = content.IndexOf("{@@}");
                if (ind != -1)
                {
                    var pos = ind + 5;
                    var line = GetLineByPos(lines, pos);
                    var col = GetColByPos(lines, pos);
                    shouldPositions.Add(new Position(line + 1, col, 0, 0, null));
                    string expr_without_brackets = null;
                    PascalABCCompiler.Parsers.KeywordKind keyw = PascalABCCompiler.Parsers.KeywordKind.None;
                    string full_expr = CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.FindExpressionFromAnyPosition(pos, content, line, col, out keyw, out expr_without_brackets);
                    List<PascalABCCompiler.Errors.Error> Errors = new List<PascalABCCompiler.Errors.Error>();
                    var errors = new List<PascalABCCompiler.Errors.Error>();
                    expression expr = comp.ParsersController.GetExpression("test" + System.IO.Path.GetExtension(FileName), full_expr, errors, new List<PascalABCCompiler.Errors.CompilerWarning>());
                    //expression expr = new ident(expr_without_brackets.Replace("{@}","").Replace("new ","").Trim());
                    var fnd_scope = dc.GetSymDefinition(expr, line, col, keyw);
                    var rf = new CodeCompletion.ReferenceFinder(fnd_scope, dc.visitor.entry_scope, cu, FileName, true);
                    var positions = rf.FindPositions();
                    compare_positions(FileName, shouldPositions.ToArray(), positions);
                }
            }
        }

        private static int position_comparer(Position x, Position y)
        {
            if (x.line > y.line)
                return 1;
            if (x.line < y.line)
                return -1;
            if (x.column > y.column)
                return 1;
            if (x.column < y.column)
                return -1;
            return 0;
        }

        private static void compare_positions(string FileName, Position[] shouldPositions, Position[] isPositions)
        {
            Array.Sort(shouldPositions, position_comparer);
            Array.Sort(isPositions, position_comparer);
            assert(isPositions.Length == shouldPositions.Length, FileName + ", should: " + shouldPositions.Length + ", is: " + isPositions.Length);
            if (isPositions.Length == shouldPositions.Length)
                for (int i = 0; i < shouldPositions.Length; i++)
                {
                    assert(isPositions[i].line == shouldPositions[i].line && isPositions[i].column == shouldPositions[i].column,
                        FileName + ", should: " + "(" + shouldPositions[i].line + "," + shouldPositions[i].column + ")" + ", is: " + "(" + isPositions[i].line + "," + isPositions[i].column + ")" + isPositions.Length);
                }
        }

        private static int GetLineByPos(string[] lines, int pos)
        {
            var cum_pos = 0;
            for (int i = 0; i<lines.Length; i++)
            { 
                for (int j=0; j<lines[i].Length; j++)
                {
                    if (cum_pos == pos)
                    {
                        return i;// i + 1 ;
                    }
                    cum_pos++;
                }
                cum_pos += Environment.NewLine.Length;
            }
            return -1;
        }

        private static int GetColByPos(string[] lines, int pos)
        {
            var cum_pos = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (cum_pos == pos)
                    {
                        return j;// + 1;
                    }
                    cum_pos++;
                }
                cum_pos += Environment.NewLine.Length;
            }
            return -1;
        }

        public static string GetDescription(int pos, string content, int line, int col, string FileName, DomConverter dc, PascalABCCompiler.Parsers.Controller controller)
        {
            string expr_without_brackets = null;
            PascalABCCompiler.Parsers.KeywordKind keyw;
            var expr = CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.FindExpressionFromAnyPosition(pos, content, line, col, out keyw, out expr_without_brackets);
            if (expr == null)
              expr = expr_without_brackets;
            var errors = new List<PascalABCCompiler.Errors.Error>();
            var warnings = new List<CompilerWarning>();
            var tree = controller.GetExpression("test" + Path.GetExtension(FileName), expr, errors, warnings);
            var desc = dc.GetDescription(tree, FileName, expr_without_brackets, controller, line, col, keyw, false);
            return desc;
        }

        public static string GetIndexDescription(int pos, string content, int line, int col, string FileName, DomConverter dc, PascalABCCompiler.Parsers.Controller controller)
        {
            string expr_without_brackets = null;
            PascalABCCompiler.Parsers.KeywordKind keyw;
            var expr = CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.FindExpressionFromAnyPosition(pos, content, line, col, out keyw, out expr_without_brackets);
            if (expr == null)
                expr = expr_without_brackets;
            var errors = new List<PascalABCCompiler.Errors.Error>();
            var warnings = new List<CompilerWarning>();
            var tree = controller.GetExpression("test" + Path.GetExtension(FileName), expr, errors, warnings);
            var desc = dc.GetIndex(tree, line, col);
            if (desc != null && desc.Length > 0)
                return desc[0];
            return "";
        }

        public static string GetMethodDescription(int pos, string content, int line, int col, string FileName, DomConverter dc, PascalABCCompiler.Parsers.Controller controller,
                                                int num_param, int cur_param_num)
        {
            string expr_without_brackets = null;
            PascalABCCompiler.Parsers.KeywordKind keyw;
            var expr = CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.FindExpressionFromAnyPosition(pos, content, line, col, out keyw, out expr_without_brackets);
            if (expr == null)
                expr = expr_without_brackets;
            var errors = new List<PascalABCCompiler.Errors.Error>();
            var warnings = new List<CompilerWarning>();
            var tree = controller.GetExpression("test" + Path.GetExtension(FileName), expr, errors, warnings);
            int defaultIndex = 0;
            int param_count = 0;
            var desc = dc.GetNameOfMethod(tree, expr, line, col, num_param, ref defaultIndex, cur_param_num, out param_count);
            if (desc != null && desc.Length > 0)
                return desc[0];
            return "";
        }

        private static void assert(bool cond, string message=null)
    	{
#if DEBUG
            if (message != null)
    		    System.Diagnostics.Debug.Assert(cond, message);
            else
                System.Diagnostics.Debug.Assert(cond);
#else
            /*if (message != null)
                System.Diagnostics.Trace.Assert(cond, message);
            else
                System.Diagnostics.Trace.Assert(cond);*/
#endif
            
        }
    	
    	private static void TestVBNETExpressionExtract()
    	{
    		string s;
    		int off=0;
    		int line=0;
    		int col=0;
    		PascalABCCompiler.Parsers.KeywordKind keyw;
    		PascalABCCompiler.Parsers.IParser parser = CodeCompletionController.ParsersController.SelectParser(".vb");
    		
    		string test_str = "System.Console";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s==test_str);
    		
    		test_str = "System(2+3,-Test34).Console";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s==test_str);
    		
    		test_str = "(a+b*c-e)";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s==test_str);
    		
    		test_str = "If test";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim(' ','\n','\t')=="test");
    		
    		test_str = "test\r\n (abc)";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim(' ','\n','\t')=="(abc)");
    	}
    	
    	private static void TestExpressionExtract()
    	{
    		string s;
    		int off=0;
    		int line=0;
    		int col=0;
    		PascalABCCompiler.Parsers.KeywordKind keyw;
            LanguageIntegration.LanguageIntegrator.ReloadAllParsers();
    		IParser parser = CodeCompletionController.ParsersController.SelectParser(compiler_string_consts.pascalSourceFileExtension);
    		
    		string test_str = "System.Console";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s==test_str);
    		
    		test_str = "System(2+3,-Test34).Console";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s==test_str);
    		
    		test_str = "(a+b*c-e)";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s==test_str);
    		
    		test_str = "a[2,3+b[2-3*b(2*Test-&var,nil)]][56,89,(44)]";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s==test_str);
    		
    		test_str = "begin test";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim(' ','\n','\t')=="test");
    		
    		test_str = "begin test[4]";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim(' ','\n','\t')=="test[4]");
    		
    		test_str = "begin sin(2)";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim(' ','\n','\t')=="sin(2)");
    		
    		test_str = "repeat test";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim(' ','\n','\t')=="test");
    		
    		test_str = "while test";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim(' ','\n','\t')=="test");
    		
    		test_str = "Test(new Text2, new Text3(2,3),inherited Create).a[new Text3]";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s==test_str);
    		
    		test_str = "Test[new Text2, new Text3(2,3),inherited Create].a[new Text3]";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s==test_str);
    		
    		test_str = "Str(a:2)";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s==test_str);
    		
    		test_str = "Str(a:2:5)";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s==test_str);
    		
    		test_str = "&Begin.&Var.&else.&for";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s==test_str);
    		
    		test_str = "a[s<5]";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s==test_str);
    		
    		test_str = "a[s>5]";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s==test_str);
    		
    		test_str = "a(s<5)";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s==test_str);
    		
    		test_str = "a(s>5)";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s==test_str);
    		
    		test_str = "ab{2 hsdjsdh 'ddd'}";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s==test_str);
    		
    		test_str = "TClass&<integer>.abc";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s==test_str);
    		
    		test_str = "TClass&<integer>";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s==test_str);
    		
    		test_str = "Test[new {dsdsdsd} Text2, new Text3{sdsd}(2,{''''''} 3),inherited {zez snj }Create]{...}.a{^^}[new Text3]";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s==test_str);
    		
    		test_str = "begin \n \t\t \n bb     \t";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim('\n',' ','\t')=="bb");
    		
    		test_str = "Test\n[new {dsdsdsd\n} Text2, new \t Text3(2,{''''''} 3),inherited {zez snj }Create]{...}.a{^^}[new Text3]";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s==test_str);
    		
    		test_str = "test//abcd\ntest.abcd";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim('\n',' ','\t')=="test.abcd");
    		
    		test_str = "test(2,3) < ppp";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim('\n',' ','\t')=="ppp");
    		
    		test_str = "test(2,3) + ppp";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim('\n',' ','\t')=="ppp");
    		
    		test_str = "test(2,3) * ppp";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim('\n',' ','\t')=="ppp");
    		
    		test_str = "test(2,3) div ppp";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim('\n',' ','\t')=="ppp");
    		
    		test_str = "test(2,3) - ppp";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim('\n',' ','\t')=="ppp");
    		
    		test_str = "test(2,3) shl ppp";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim('\n',' ','\t')=="ppp");
    		
    		test_str = "(s as string)";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s==test_str);
    		
    		test_str = "begin (s as string)";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim('\n',' ','\t')=="(s as string)");
    		
    		test_str = "test(2,3) shr ppp";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim('\n',' ','\t')=="ppp");
    		
    		test_str = "test(2,3) > ppp";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim('\n',' ','\t')=="ppp");
    		
    		test_str = "test(2,3) or ppp";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim('\n',' ','\t')=="ppp");
    		
    		test_str = "test(2,3) and ppp";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim('\n',' ','\t')=="ppp");
    		
    		test_str = "test(2,3) xor ppp";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim('\n',' ','\t')=="ppp");
    		
    		test_str = "test(2,3) mod ppp";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim('\n',' ','\t')=="ppp");
    		
    		test_str = "typeof(char)";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim('\n',' ','\t')==test_str);
    		
    		test_str = "sizeof(char)";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpression(off,test_str,line,col,out keyw);
    		assert(s.Trim('\n',' ','\t')==test_str);

            test_str = "()->(obj as string).Trim";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpression(off, test_str, line, col, out keyw);
            assert(s.Trim('\n', ' ', '\t') == "(obj as string).Trim");

            test_str = "Seq(0)\n.f1//комментарий\n.Print";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpression(off, test_str, line, col, out keyw);
            assert(s.Trim('\n', ' ', '\t') == "Seq(0)\n.f1\n.Print");

            test_str = "$'is {a}'";
            off = test_str.Length-2;
            s = parser.LanguageInformation.FindExpression(off, test_str, line, col, out keyw);
            assert(s.Trim('\n', ' ', '\t') == "a");

            test_str = "seq1.Where(i ->(i = 1) or (i = 2)).JoinIntoString";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpression(off, test_str, line, col, out keyw);
            assert(s.Trim('\n', ' ', '\t') == "seq1.Where(i ->(i = 1) or (i = 2)).JoinIntoString");

            test_str = "$'{f1(s0)}'";
            off = test_str.Length - 3;
            s = parser.LanguageInformation.FindExpression(off, test_str, line, col, out keyw);
            assert(s.Trim('\n', ' ', '\t') == "s0");

            test_str = "f1&<byte>\n.ToString";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpression(off, test_str, line, col, out keyw);
            assert(s.Trim('\n', ' ', '\t') == test_str);
            
            test_str = "a[2:]";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpression(off, test_str, line, col, out keyw);
            assert(s.Trim('\n', ' ', '\t') == test_str);

            test_str = "' '' '";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpression(off, test_str, line, col, out keyw);
            assert(s.Trim('\n', ' ', '\t') == test_str);

            test_str = "' '' '' '";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpression(off, test_str, line, col, out keyw);
            assert(s.Trim('\n', ' ', '\t') == test_str);

            test_str = "' ' '";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpression(off, test_str, line, col, out keyw);
            assert(s.Trim('\n', ' ', '\t') == "");

            test_str = "(2..4)";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpression(off, test_str, line, col, out keyw);
            assert(s.Trim('\n', ' ', '\t') == test_str);



            int num_param = 0;
    		//testirovanie nazhatija skobki
    		test_str = "writeln";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s == test_str);
    		assert(num_param==0);
    		
    		test_str = "Console.WriteLine";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s == test_str);
    		assert(num_param==0);
    		
    		test_str = "begin writeln";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s.Trim('\n',' ','\t') == "writeln");
    		assert(num_param==0);
    		
    		test_str = "Console{dsdsdsd}.WriteLine{''''''sds}";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s == test_str);
    		assert(num_param==0);
    		
    		test_str = "test(2,3).mmm";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s == test_str);
    		assert(num_param==0);
    		
    		test_str = "test[2<3].a";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s == test_str);
    		assert(num_param==0);
    		
    		test_str = "test[2>3].a";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s == test_str);
    		assert(num_param==0);
    		
    		test_str = "test[2>3] .a";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s == test_str);
    		assert(num_param==0);
    		
    		test_str = "23 < test";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s.Trim('\n',' ','\t') == "test");
    		assert(num_param==0);
    		
    		test_str = "23 > test";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s.Trim('\n',' ','\t') == "test");
    		assert(num_param==0);
    		
    		test_str = "(a<b) + test";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s.Trim('\n',' ','\t') == "test");
    		assert(num_param==0);
    		
    		test_str = "test2[sin(2,3),cos(2)]";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s == test_str);
    		
    		test_str = "sin{sdsd}(2,3).ttt";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s == test_str);
    		assert(num_param==0);
    		
    		test_str = "new System.Collections.Generic.List<integer>";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s == test_str);
    		assert(num_param==0);
    		
    		test_str = "TClass&<T>.bbb";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s == test_str);
    		assert(num_param==0);

            test_str = "()->(obj as string).Trim";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpressionForMethod(off, test_str, line, col, '(', ref num_param);
            assert(s == "(obj as string).Trim");
            assert(num_param == 0);

            test_str = "test; \n  sin";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s.Trim(' ','\n','\t') == "sin");
    		assert(num_param==0);
    		
    		test_str = "new TClass";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s == test_str);
    		assert(num_param==0);
    		
    		test_str = "Proc1({}2+3,'aasd',Proc2(a[34,{}2],new TClass(f('ssd')))).zzz";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s == test_str);
    		assert(num_param==0);
    		
    		test_str = "'abc'.tostring('aa','bb{()').ggg";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s == test_str);
    		assert(num_param==0);
    		
    		test_str = "Test([],[1,2,3],{cdsds} \tsin";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s.Trim(' ','\n','\t') == "{cdsds} \tsin");
    		assert(num_param==0);
    		
    		test_str = "test(2+3,aa.bb";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s == "aa.bb");
    		
    		test_str = "test(2<3).bb";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s == test_str);
    		assert(num_param==0);
    		
    		test_str = "sin(new TClass<integer>,TClass&<real>).pp";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s == test_str);
    		assert(num_param==0);
    		
    		test_str = "raise new TClass";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s.Trim(' ','\n','\t') == "new TClass");
    		assert(num_param==0);
    		
    		test_str = "begin test(2)";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s.Trim(' ','\n','\t') == "test(2)");
    		assert(num_param==0);
    		
    		test_str = "//aaaa\nConsole.WriteLine";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s.Trim(' ','\n','\t') == "Console.WriteLine");
    		assert(num_param==0);

            test_str = "System.Math.DivRem";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpressionForMethod(off, test_str, line, col, '(', ref num_param);
            assert(s.Trim(' ', '\n', '\t') == "System.Math.DivRem");
            assert(num_param == 0);

            test_str = "seq1.Where(i ->(i = 1) or (i = 2)).JoinIntoString";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpressionForMethod(off, test_str, line, col, '(', ref num_param);
            assert(s.Trim('\n', ' ', '\t') == "seq1.Where(i ->(i = 1) or (i = 2)).JoinIntoString");

            test_str = "seq1.Where(i ->(i = '1') or (i = '2')).JoinIntoString";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpressionForMethod(off, test_str, line, col, '(', ref num_param);
            assert(s.Trim('\n', ' ', '\t') == "seq1.Where(i ->(i = '1') or (i = '2')).JoinIntoString");

            test_str = "f1&<array of byte>";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpressionForMethod(off, test_str, line, col, '(', ref num_param);
            assert(s.Trim('\n', ' ', '\t') == "f1&<array of byte>");

            test_str = "f1&<sequence of byte>";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpressionForMethod(off, test_str, line, col, '(', ref num_param);
            assert(s.Trim('\n', ' ', '\t') == "f1&<sequence of byte>");

            test_str = "&var.&uses.&procedure";
    		off = test_str.Length;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,'(',ref num_param);
    		assert(s == test_str);

            test_str = "s.OrderBy";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpressionForMethod(off, test_str, line, col, '(', ref num_param);
            assert(s == test_str);

            test_str = "f1&<byte>\n.ToString";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpressionForMethod(off, test_str, line, col, '(', ref num_param);
            assert(s.Trim('\n', ' ', '\t') == "f1&<byte>\n.ToString");

            test_str = "begin ''.PadLeft";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpressionForMethod(off, test_str, line, col, '(', ref num_param);
            assert(s.Trim('\n', ' ', '\t') == "''.PadLeft");

            test_str = "begin var s := ''.PadLeft";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpressionForMethod(off, test_str, line, col, '(', ref num_param);
            assert(s.Trim('\n', ' ', '\t') == "''.PadLeft");

            test_str = "begin \n var s := ''.PadLeft";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpressionForMethod(off, test_str, line, col, '(', ref num_param);
            assert(s.Trim('\n', ' ', '\t') == "''.PadLeft");

            test_str = "writeln(23);\n ''.PadLeft";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpressionForMethod(off, test_str, line, col, '(', ref num_param);
            assert(s.Trim('\n', ' ', '\t') == "''.PadLeft");

            test_str = "writeln(23);\n ''.PadLeft(2).ToString";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpressionForMethod(off, test_str, line, col, '(', ref num_param);
            assert(s.Trim('\n', ' ', '\t') == "''.PadLeft(2).ToString");



            //testirovanie nazhatija zapjatoj
            test_str = ";test(3,aa.bb";
    		off = test_str.Length;
    		num_param = 1;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,',',ref num_param);
    		assert(s == "test");
    		assert(num_param == 3);
    		
    		test_str = ";test(a[2,3,4],sin(2+3),aa.bb";
    		off = test_str.Length;
    		num_param = 1;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,',',ref num_param);
    		assert(s == "test");
    		assert(num_param == 4);
    		
    		test_str = "Console.WriteLine(a[2,3,4]{ds},[1,2+3,5,'aa)'],sin(2+3,4+f(3)),aa.bb,(23)";
    		off = test_str.Length;
    		num_param = 1;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,',',ref num_param);
    		assert(s == "Console.WriteLine");
    		assert(num_param == 6);
    		
    		test_str = "Console.WriteLine(new TClass(2,3";
    		off = test_str.Length;
    		num_param = 1;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,',',ref num_param);
    		assert(s == "new TClass");
    		assert(num_param == 3);
    		
    		test_str = "Console.WriteLine(new{} TClass(2+f(2,[]),3";
    		off = test_str.Length;
    		num_param = 1;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,',',ref num_param);
    		assert(s == "new{} TClass");
    		assert(num_param == 3);
    		
    		test_str = "begin \n\t sin(";
    		off = test_str.Length;
    		num_param = 1;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,',',ref num_param);
    		assert(s.Trim(' ','\n','\t') == "sin");
    		assert(num_param == 2);
    		
    		test_str = "sin(2)";
    		off = test_str.Length;
    		num_param = 1;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,',',ref num_param);
    		assert(s.Trim(' ','\n','\t') == "");
    		
    		test_str = "begin f(2)+sin(2,3,4)";
    		off = test_str.Length;
    		num_param = 1;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,',',ref num_param);
    		assert(s.Trim(' ','\n','\t') == "");
    		
    		test_str = ";\n [sin(f(3)),a[3]";
    		off = test_str.Length;
    		num_param = 1;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,',',ref num_param);
    		assert(s.Trim(' ','\n','\t') == "");
    		
    		test_str = ";\n (sin)";
    		off = test_str.Length;
    		num_param = 1;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,',',ref num_param);
    		assert(s.Trim(' ','\n','\t') == "");
    		
    		test_str = "max(2<3";
    		off = test_str.Length;
    		num_param = 1;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,',',ref num_param);
    		assert(s.Trim(' ','\n','\t') == "max");
    		assert(num_param == 2);
    		
    		test_str = "max(2>3";
    		off = test_str.Length;
    		num_param = 1;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,',',ref num_param);
    		assert(s.Trim(' ','\n','\t') == "max");
    		assert(num_param == 2);
    		
    		test_str = "max(Test&<integer>(2,3)";
    		off = test_str.Length;
    		num_param = 1;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,',',ref num_param);
    		assert(s.Trim(' ','\n','\t') == "max");
    		assert(num_param == 2);
    		
    		test_str = "new TextABC(60,110,110,'Hello!',RGB(x";
    		off = test_str.Length;
    		num_param = 1;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,',',ref num_param);
    		assert(s.Trim(' ','\n','\t') == "RGB");
    		assert(num_param == 2);
    		
    		test_str = "new TextABC(60,110,110,'Hello!',new RGB(x";
    		off = test_str.Length;
    		num_param = 1;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,',',ref num_param);
    		assert(s.Trim(' ','\n','\t') == "new RGB");
    		assert(num_param == 2);
    		
    		test_str = "new TextABC(new RGB(x";
    		off = test_str.Length;
    		num_param = 1;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,',',ref num_param);
    		assert(s.Trim(' ','\n','\t') == "new RGB");
    		assert(num_param == 2);
    		
    		test_str = "new TextABC(new RGB(x,(2)";
    		off = test_str.Length;
    		num_param = 1;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,',',ref num_param);
    		assert(s.Trim(' ','\n','\t') == "new RGB");
    		assert(num_param == 3);
    		
    		test_str = "new TextABC(new RGB(x(2)";
    		off = test_str.Length;
    		num_param = 1;
    		s = parser.LanguageInformation.FindExpressionForMethod(off,test_str,line,col,',',ref num_param);
    		assert(s.Trim(' ','\n','\t') == "new RGB");
    		assert(num_param == 2);

            test_str = "Power(10 div 2";
            off = test_str.Length;
            num_param = 1;
            s = parser.LanguageInformation.FindExpressionForMethod(off, test_str, line, col, ',', ref num_param);
            assert(s.Trim(' ', '\n', '\t') == "Power");
            assert(num_param == 2);

            

            string str = null;
    		//mouse hover
    		test_str = "sin(2)";
    		off = 1;
    		s = parser.LanguageInformation.FindExpressionFromAnyPosition(off,test_str,line,col,out keyw,out str);
    		assert(s.Trim('\n',' ','\t')=="sin(2)");
    		
    		test_str = "sin(2,3,4)";
    		off = 1;
    		s = parser.LanguageInformation.FindExpressionFromAnyPosition(off,test_str,line,col,out keyw,out str);
    		assert(s.Trim('\n',' ','\t')=="sin(2,3,4)");
    		
    		test_str = "sin {sdsd'} (2,3,4)";
    		off = 1;
    		s = parser.LanguageInformation.FindExpressionFromAnyPosition(off,test_str,line,col,out keyw,out str);
    		assert(s.Trim('\n',' ','\t')==test_str);
    		
    		test_str = "sin (df,'3)+2','{')";
    		off = 1;
    		s = parser.LanguageInformation.FindExpressionFromAnyPosition(off,test_str,line,col,out keyw,out str);
    		assert(s.Trim('\n',' ','\t')==test_str);
    		
    		test_str = "cos()";
    		off = 1;
    		s = parser.LanguageInformation.FindExpressionFromAnyPosition(off,test_str,line,col,out keyw,out str);
    		assert(s.Trim('\n',' ','\t')==test_str);
    		
    		test_str = "cos{()}";
    		off = 1;
    		s = parser.LanguageInformation.FindExpressionFromAnyPosition(off,test_str,line,col,out keyw,out str);
    		assert(s.Trim('\n',' ','\t')=="cos");
    		
    		test_str = "cos//()";
    		off = 1;
    		s = parser.LanguageInformation.FindExpressionFromAnyPosition(off,test_str,line,col,out keyw,out str);
    		assert(s.Trim('\n',' ','\t')=="cos");
    		
    		test_str = "cos(//)";
    		off = 1;
    		s = parser.LanguageInformation.FindExpressionFromAnyPosition(off,test_str,line,col,out keyw,out str);
    		assert(s.Trim('\n',' ','\t')=="cos");
    		
    		test_str = "cos('//')";
    		off = 1;
    		s = parser.LanguageInformation.FindExpressionFromAnyPosition(off,test_str,line,col,out keyw,out str);
    		assert(s.Trim('\n',' ','\t')==test_str);
    		
    		test_str = "cos({//})";
    		off = 1;
    		s = parser.LanguageInformation.FindExpressionFromAnyPosition(off,test_str,line,col,out keyw,out str);
    		assert(s.Trim('\n',' ','\t')==test_str);
    		
    		test_str = "cos(Math.Cos(x)+1)";
    		off = 1;
    		s = parser.LanguageInformation.FindExpressionFromAnyPosition(off,test_str,line,col,out keyw,out str);
    		assert(s.Trim('\n',' ','\t')==test_str);

            test_str = "new t1<byte>(2)";
            off = 5;
            s = parser.LanguageInformation.FindExpressionFromAnyPosition(off, test_str, line, col, out keyw, out str);
            assert(s.Trim('\n', ' ', '\t') == "new t1<byte>(2)");

            test_str = "new t1<List<byte>>(2)";
            off = 5;
            s = parser.LanguageInformation.FindExpressionFromAnyPosition(off, test_str, line, col, out keyw, out str);
            assert(s.Trim('\n', ' ', '\t') == "new t1<List<byte>>(2)");

            test_str = "t1&<byte>.x";
            off = test_str.Length;
            s = parser.LanguageInformation.FindExpressionFromAnyPosition(off, test_str, line, col, out keyw, out str);
            assert(s.Trim('\n', ' ', '\t') == "t1&<byte>.x");

            test_str = "Arr(0).Select&<integer,ft>";
            off = 8;
            s = parser.LanguageInformation.FindExpressionFromAnyPosition(off, test_str, line, col, out keyw, out str);
            assert(s.Trim('\n', ' ', '\t') == "Arr(0).Select&<integer,ft>");

            //----
            Type[] types = typeof(int).Assembly.GetExportedTypes();
            int i = 0;
            int j = 0;
            StreamWriter sw = new StreamWriter("mscorlib.txt");
            foreach (Type t in types)
            {
                sw.WriteLine(parser.LanguageInformation.GetCompiledTypeRepresentation(t, t, ref i, ref j));
            }
            
            sw.Close();
            types = typeof(System.Diagnostics.Process).Assembly.GetExportedTypes();
            sw = new StreamWriter(typeof(System.Diagnostics.Process).Assembly.ManifestModule.ScopeName+".txt");
            foreach (Type t in types)
            {
                sw.WriteLine(parser.LanguageInformation.GetCompiledTypeRepresentation(t, t, ref i, ref j));
            }

            sw.Close();

            types = typeof(System.Data.Constraint).Assembly.GetExportedTypes();
            sw = new StreamWriter(typeof(System.Data.Constraint).Assembly.ManifestModule.ScopeName + ".txt");
            foreach (Type t in types)
            {
                sw.WriteLine(parser.LanguageInformation.GetCompiledTypeRepresentation(t, t, ref i, ref j));
            }

            sw.Close();

            types = typeof(System.Xml.XmlDocument).Assembly.GetExportedTypes();
            sw = new StreamWriter(typeof(System.Xml.XmlDocument).Assembly.ManifestModule.ScopeName + ".txt");
            foreach (Type t in types)
            {
                sw.WriteLine(parser.LanguageInformation.GetCompiledTypeRepresentation(t, t, ref i, ref j));
            }
            sw.Close();
    	}
    }

    public class FormatterTester
    {
        private static string GetTestSuiteDir()
        {
            var dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);
            var ind = dir.LastIndexOf("bin");
            return dir.Substring(0, ind) + "TestSuite";
        }

        public static void Test()
		{
			string test_dir = Path.Combine(GetTestSuiteDir(), @"formatter_tests");
			string output_dir = Path.Combine(test_dir, @"output");
            Directory.CreateDirectory(output_dir);
			string[] files = Directory.GetFiles(test_dir+@"\input","*.pas");
            StreamWriter log = new StreamWriter(Path.Combine(output_dir, @"log.txt"), false, Encoding.GetEncoding(1251));
            SyntaxTreeComparer stc = new SyntaxTreeComparer();
            foreach (string s in files)
            {
                string Text = new StreamReader(s,System.Text.Encoding.GetEncoding(1251)).ReadToEnd();
                List<Error> Errors = new List<Error>();
                List<CompilerWarning> Warnings = new List<CompilerWarning>();
                compilation_unit cu = CodeCompletionController.ParsersController.GetCompilationUnitForFormatter(s, Text, Errors, Warnings);
                if (Errors.Count == 0)
                {
                    CodeFormatters.CodeFormatter cf = new CodeFormatters.CodeFormatter(2);
                    Text = cf.FormatTree(Text, cu, 1, 1);
                    StreamWriter sw = new StreamWriter(Path.Combine(output_dir, Path.GetFileName(s)), false, Encoding.GetEncoding(1251));
                    sw.Write(Text);
                    sw.Close();
                    Errors.Clear();
                    Warnings.Clear();
                    compilation_unit cu2 = CodeCompletionController.ParsersController.GetCompilationUnitForFormatter(Path.Combine(output_dir, Path.GetFileName(s)), Text, Errors, Warnings);
                    if (Errors.Count > 0)
                    {
                        for (int i = 0; i < Errors.Count; i++)
                            log.WriteLine(Errors[i].ToString());
                    }
                    else
                        try
                        {
                            stc.Compare(cu, cu2);
                        }
                        catch(SyntaxNodesNotEqual ex)
                        {
                            log.WriteLine("SyntaxTreeNotEquals " + s + " " + ex.left?.ToString() + " and " + ex.right?.ToString() + Environment.NewLine);
                        }
                        catch (Exception ex)
                        {
                            log.WriteLine("SyntaxTreeNotEquals " + ex.Message  + " " + s + Environment.NewLine);
                        }
                }
            }
            files = Directory.GetFiles(output_dir, "*.pas");
            foreach (string s in files)
            {
                var sr = new StreamReader(s, System.Text.Encoding.GetEncoding(1251));
                string Text = sr.ReadToEnd();
                sr.Close();
                List<Error> Errors = new List<Error>();
                List<CompilerWarning> Warnings = new List<CompilerWarning>();
                compilation_unit cu = CodeCompletionController.ParsersController.GetCompilationUnitForFormatter(s, Text, Errors, Warnings);
                CodeFormatters.CodeFormatter cf = new CodeFormatters.CodeFormatter(2);
                string Text2 = cf.FormatTree(Text, cu, 1, 1);
                if (Text.Replace("\r\n","\n") != Text2.Replace("\r\n","\n"))
                {
                    int line = 1;
                    Text = Text.Replace("\r\n", "\n");
                    Text2 = Text2.Replace("\r\n", "\n");
                    for (int i = 0; i < Math.Min(Text.Length, Text2.Length); i++)
                    {
                        if (Text[i] != Text2[i])
                        {
                            log.WriteLine("Invalid formatting of File " + s + ", line "+line);
                            break;
                        }
                        else if (Text[i] == '\n')
                        {
                            line++;
                        }
                    }  
                }
                
                string shouldFileName = Path.Combine(test_dir + @"\should",Path.GetFileName(s));
                if (File.Exists(shouldFileName))
                {
                    sr = new StreamReader(shouldFileName, System.Text.Encoding.GetEncoding(1251));
                    string shouldText = sr.ReadToEnd();
                    sr.Close();
                    if (Text.Replace("\r\n","\n") != shouldText.Replace("\r\n","\n"))
                    {
                        sr = new StreamReader(s, System.Text.Encoding.UTF8);
                        Text = sr.ReadToEnd();
                        sr.Close();
                        sr = new StreamReader(shouldFileName, System.Text.Encoding.UTF8);
                        shouldText = sr.ReadToEnd();
                        sr.Close();
                    }
                        
                    if (Text.Replace("\r\n","\n") != shouldText.Replace("\r\n","\n"))
                        log.WriteLine("Invalid formatting of File (text not equal) " + s);
                }
            }
            log.Close();
        }
    }
   // #endif
}

