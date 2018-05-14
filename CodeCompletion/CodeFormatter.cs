// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
/*
 * algoritm formatirovanija sledujushij:
 * po sintaksicheskomu derevu stroitsja generiruetsja tekst programmy
 * vse chto nahoditsja mezhdu uzlami sintaksicheskogo dereva takzhe uchityvaetsja
 * s pomoshuj razlichnyh flagov proizvoditsja sootvetstvujushee formatirovanie
 * naprimer. posle procedury stavitsja pustaja stroka
 * dalee pod kommentariem budet ponimatsja obobshennyj kommentarii, kotorye vkluchajut v sebja
 * takzhe znaki operacii, drugie simvoly, kotorye ne okajamlajut tekuchij token
 * zdes ochen vazhny source konteksty
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using PascalABCCompiler.SyntaxTree;
using System.IO;

namespace CodeFormatters
{
    class FormatterOptions
    {
        public int SpaceBetweenArguments = 1;
        public int AlwaysNewLineForBeginEndBlock = 1;
        public int SpaceBetweenIdentifiers = 1;
        public int AggressiveMode = 0;
    }

    public class CodeFormatter : PascalABCCompiler.SyntaxTree.AbstractVisitor/*, PascalABCCompiler.Parsers.ICodeFormatter*/
    {
        private StringBuilder sb = new StringBuilder();
        private int tab = 2;//tabulacija
        private int off = 0;//tekushij otstup
        private Stack<int> region_offsets = new Stack<int>();
        private bool in_procedure = false;//flag, v procedure li my
        private bool attr_on_new_line = true;
        private bool in_class = false;
        private bool has_implementation = false;
        private bool init_part = false;
        private NumberFormatInfo nfi;
        private Dictionary<string, string> keywords = new Dictionary<string, string>();
        private int[] line_lens;//massiv dlin strok, ispolzuetsja dlja podscheta pozicii v tekste (Position v sintaksicheskom dereve nepravilnye ;) )
        private string Text;//tekst programmy
        private syntax_tree_node prev_sn = null;//predydushij obrabotannyj uzel dereva
        private int cur_src_off = 0;
        private bool add_space_before = false;//flag, stavit li probel pered kommentariem (esli nuzhno)
        private bool is_lambda_parameters = false;
        private bool add_space_after = false;//stavit li probel posle kommentarija (esli nuzhno)
        //private bool add_tab_after = false;
        private bool add_newline_before = false;//dobavlat novuju stroku pered kommentariem (esli nuzhno)
        private bool add_newline_after = false;//dobavit novuju stroku posle kommentarija (esli nuzhno)
        private bool insert_newline_after_prev = false;//stavit pustuju stroku posle konstrukcii
        private bool insert_newline_after_prev_semicolon = false;//vstavit pustuju stroku dlja sluchaja, kogda ot predydushej konstrukcii
                                                                 //ostalsja razdiletel, kotoryj ne vhodit v etu konstrukciju. primer blok var a: integer; type t = integer; pered type nado stavit pustuju stroku, no pri etom ne zabyt
                                                                 //dobavit tochku s zapjatoj
        private bool read_from_beg_pos = false;
        private bool dec_offset_before_comment = false;
        private bool add_new_line_else_specific = false;
        private int keyword_offset = 0;
        private int caret_line = 0;
        private int caret_col = 0;
        private int caret_pos = 0;
        private int output_caret_line = 0;
        private int output_caret_col = 0;
        private bool caret_syn_node_found = false;
        private Dictionary<string, string> nodes_with_beg_pos = new Dictionary<string, string>();
        private bool use_strong_formatting = false;
        private bool skip_spaces_before = false;
        private bool skip_spaces_after = false;
        private int addit_pos_for_multiline = 0;
        private Stack<syntax_tree_node> multi_line_nodes = new Stack<syntax_tree_node>();
        private FormatterOptions options;
        

        public CodeFormatter(int tab)
        {
            nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = ".";
            this.tab = tab;
            nodes_with_beg_pos.Add("for_node","for_node");
            nodes_with_beg_pos.Add("function_header", "function_header");

            ReadOptions();
        }

        private void ReadOptions()
        {
            options = new FormatterOptions();
            try
            {
                string PABCWorkIniFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName) + @"\pabcworknet.ini";
                //System.Windows.Forms.MessageBox.Show(PABCWorkIniFile);
                string FormatterOptionsFile = "FormatterOptions.ini";
                if (File.Exists(PABCWorkIniFile))
                {
                    string PABCWorkDir = File.ReadAllText(PABCWorkIniFile);
                    if (File.Exists(Path.Combine(PABCWorkDir, "FormatterOptions.ini")))
                        FormatterOptionsFile = Path.Combine(PABCWorkDir, "FormatterOptions.ini");
                }
                string[] lines = File.ReadAllLines(FormatterOptionsFile);
                for (int i = 0; i < lines.Length; i++)
                {
                    int pos = lines[i].IndexOf(';');
                    if (pos != -1)
                        lines[i] = lines[i].Substring(0, pos);
                }
                Type t = options.GetType();
                for (int i = 0; i < lines.Length; i++)
                {
                    string key = lines[i].Substring(0, lines[i].IndexOf('='));
                    string val = lines[i].Substring(lines[i].IndexOf('=') + 1);
                    t.GetField(key).SetValue(options, Convert.ToInt32(val));
                }
            }
            catch
            {
            }
        }

        public string FormatTree(string Text, compilation_unit cu, int cursor_line, int cursor_col)
        {
            PascalABCCompiler.Parsers.CommentBinder binder = new PascalABCCompiler.Parsers.CommentBinder();
            //comments = binder.Bind(cu, Text);
            this.Text = Text;
            string[] lines = Text.Split('\n');
            line_lens = new int[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                if (i > 0)
                    line_lens[i] = lines[i].Length + line_lens[i - 1] + 1;
                else
                    line_lens[i] = lines[i].Length + 1;
            }
            caret_line = cursor_line;
            caret_col = cursor_col;
            caret_pos = GetPosition(cursor_line, cursor_col);
            visit_node(cu);
            return sb.ToString();
        }

        public int Line
        {
            get
            {
                return output_caret_line;
            }
        }

        public int Column
        {
            get
            {
                return output_caret_col;
            }
        }

        private int GetPosition(int line, int col)
        {
            if (line >= 2)
                return line_lens[line - 2] + col - 1;
            return col - 1;
        }

        private void SetKeywordOffset(string s)
        {
            keyword_offset = s.Length;
            add_space_before = true;
        }

        private void multiline_stack_push(syntax_tree_node sn)
        {
            if (!in_one_row(sn))
                multi_line_nodes.Push(sn);
        }

        private void multiline_stack_pop(syntax_tree_node sn)
        {
            if (!in_one_row(sn) && multi_line_nodes.Count > 0)
                multi_line_nodes.Pop();
        }

        private bool in_one_row(syntax_tree_node sn)
        {
            if (options.AlwaysNewLineForBeginEndBlock == 0)
                return true;
            if (sn.source_context == null)
                return true;
            return sn.source_context.begin_position.line_num == sn.source_context.end_position.line_num;
        }

        private bool in_same_row(syntax_tree_node sn, syntax_tree_node sn2)
        {
            if (sn.source_context == null || sn2.source_context == null)
                return false;
            return sn.source_context.end_position.line_num == sn2.source_context.begin_position.line_num;
        }

        private int distance(syntax_tree_node sn1, syntax_tree_node sn2)
        {
            return GetPosition(sn2.source_context.begin_position.line_num, sn2.source_context.begin_position.column_num)-GetPosition(sn1.source_context.end_position.line_num, sn1.source_context.end_position.column_num)-1;
        }

        private bool is_forward(procedure_header ph)
        {
            if (ph.proc_attributes == null)
                return false;
            foreach (procedure_attribute pa in ph.proc_attributes.proc_attributes)
                if (pa.attribute_type == proc_attribute.attr_forward)
                    return true;
            return false;
        }

        private void WriteAmpersandIfNeed(ident id)
        {
            if (id.source_context != null)
            {
                int pos = GetPosition(id.source_context.begin_position.line_num, id.source_context.begin_position.column_num);
                if (Text[pos] == '&')
                    sb.Append("&");
            }
        }

        private void WriteNode(syntax_tree_node sn)
        {
            if (sn.source_context != null)
            {
                int start_pos = GetPosition(sn.source_context.begin_position.line_num, sn.source_context.begin_position.column_num);
                int end_pos = GetPosition(sn.source_context.end_position.line_num, sn.source_context.end_position.column_num);
                sb.Append(Text.Substring(start_pos, end_pos - start_pos + 1));
            }
        }

        private void WriteNodeWithIndent(syntax_tree_node sn)
        {
            int start_pos = GetPosition(sn.source_context.begin_position.line_num, sn.source_context.begin_position.column_num);
            int end_pos = GetPosition(sn.source_context.end_position.line_num, sn.source_context.end_position.column_num);
            string node_text = prepare_comment(Text.Substring(start_pos, end_pos - start_pos + 1), false);
            sb.Append(node_text);
        }

        private void WriteNode(syntax_tree_node sn, int offset)
        {
            int start_pos = GetPosition(sn.source_context.begin_position.line_num, sn.source_context.begin_position.column_num) + offset;
            int end_pos = GetPosition(sn.source_context.end_position.line_num, sn.source_context.end_position.column_num);
            bool tmp_add_space_after = add_space_after;
            add_space_after = false;
            WriteCommentWithIndent(Text.Substring(start_pos, end_pos - start_pos + 1), true);
            add_space_after = tmp_add_space_after;
            //sb.Append(new string(' ', off) + Text.Substring(start_pos, end_pos - start_pos + 1));
        }

        private string prepare_comment(string s, bool off_first_line=true)
        {
            s = s.TrimStart(' ', '\n', '\t', '\r');
            if (s.Contains("\n"))
            {
                string[] rows = s.Split('\n');
                for (int i = 0; i < rows.Length; i++)
                {
                    if (i != 0 || off_first_line)
                        rows[i] = new string(' ', off) + rows[i].TrimStart(' ', '\t');
                }
                return string.Join("\n", rows);
            }
            return s;
        }

        private string GetPossibleComment(syntax_tree_node sn, int off, syntax_tree_node next_sn)
        {
            int pos = GetPosition(sn.source_context.end_position.line_num, sn.source_context.end_position.column_num) + off;
            int next_pos = GetPosition(next_sn.source_context.begin_position.line_num, next_sn.source_context.begin_position.column_num);
            string comm = Text.Substring(pos, next_pos - pos + 1);
            return comm;
        }

        private bool has_comment(string s)
        {
            return s.Contains("{") || s.Contains("(*") || s.Contains("//");
        }

        private int GetSpacesBefore(string s)
        {
            int i = 0;
            while (i < s.Length && s[i] == ' ')
                i++;
            return i;
        }

        private void WriteCommentWithIndent(string comm, bool before)
        {
            string[] lines = null;
            if (insert_newline_after_prev && !insert_newline_after_prev_semicolon && before)
            {
                lines = comm.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                if (lines.Length == 2)
                    comm = comm + "\r\n";
                else if (lines.Length < 2)
                    comm = comm + "\r\n" + "\r\n";
                insert_newline_after_prev = false;
            }
            if (add_newline_before)
            {
                if (comm.IndexOf("\r\n") == -1)
                    comm = comm + "\r\n";
                //if (!comm.Trim(' ','\t').StartsWith("\r\n"))
                //    comm = "\r\n" + comm;
                add_newline_before = false;
            }
            else if (add_newline_after)
            {
                if (!comm.Trim(' ', '\t').EndsWith("\r\n"))
                    comm += "\r\n";
                add_newline_after = false;
            }
            lines = comm.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            if (lines.Length > 1)
            {
                int min_off = 0;
                for (int i = 1; i < lines.Length; i++)
                {
                    if (!add_new_line_else_specific && !(dec_offset_before_comment && i < lines.Length - 1))
                        if (multi_line_nodes.Count == 0 && lines[i].Trim() != "")
                        {
                            int ind = 0;
                            string tmp = lines[i];
                            while (ind < tmp.Length && (tmp[ind] == ' ' || tmp[ind] == '\t'))
                            {
                                ind++;
                            }
                            if (ind < off)
                            {
                                min_off = Math.Max(off - ind, min_off);
                            }
                        }
                }
                for (int i = 1; i < lines.Length; i++)
                {
                    if (add_new_line_else_specific)
                    {
                        if (i < lines.Length - 1 || lines[i].TrimStart(' ', '\t') != "")
                            lines[i] = new string(' ', off - tab) + lines[i].TrimStart(' ', '\t');
                        else
                            lines[i] = new string(' ', off) + lines[i].TrimStart(' ', '\t');
                    }
                    else if (dec_offset_before_comment && i < lines.Length - 1)
                        lines[i] = new string(' ', off + tab) + lines[i].TrimStart(' ', '\t');
                    else
                    {
                        if (multi_line_nodes.Count > 0)
                        {
                            if (multi_line_nodes.Count == 1)
                            {
                                if (!(multi_line_nodes.Peek() is function_lambda_definition))
                                    lines[i] = new string(' ', addit_pos_for_multiline) + lines[i];
                                else
                                    lines[i] = new string(' ', off) + lines[i].Trim();
                            }
                                
                            else
                            {
                                if (!(multi_line_nodes.Peek() is function_lambda_definition))
                                {
                                    if (off > lines[i].Length)
                                        lines[i] = new string(' ', addit_pos_for_multiline + off) + lines[i];
                                    else
                                        lines[i] = new string(' ', addit_pos_for_multiline) + lines[i];
                                }
                                else
                                    lines[i] = new string(' ', off) + lines[i].Trim();
                            }
                                
                        }
                        else
                        {
                            int ind = 0;
                            string tmp = lines[i];
                            while (ind < tmp.Length && (tmp[ind] == ' ' || tmp[ind] == '\t'))
                            {
                                ind++;
                            }
                            if (ind < off)
                            {
                                addit_pos_for_multiline = off - ind;
                            }
                            else
                                addit_pos_for_multiline = 0;
                            string ln = lines[i].TrimStart(' ', '\t');
                            if (ln.StartsWith("{$region"))
                            {
                                region_offsets.Push(off);
                                lines[i] = new string(' ', off) + ln;
                            }
                            else if (ln.StartsWith("{$endregion"))
                            {
                                int region_off = off;
                                if (region_offsets.Count != 0)
                                    region_off = region_offsets.Pop();
                                lines[i] = new string(' ', region_off) + ln;
                            }
                            else if (i < lines.Length - 1)
                            {
                                if (lines[i].Trim() != "")
                                    lines[i] = new string(' ', min_off) + lines[i];
                                else
                                    lines[i] = new string(' ', off);
                            }
                            else
                                lines[i] = new string(' ', off) + lines[i].TrimStart(' ', '\t');
                        }
                    }
                }
            }
            if (lines.Length <= 2 && insert_newline_after_prev_semicolon && before)
            {
                if (lines.Length == 1)
                {
                    lines = new string[3] { lines[0], new string(' ', off), new string(' ', off) };
                }
                else
                {
                    lines = new string[3] { lines[0], new string(' ', off), lines[1] };
                }
            }
            string s = string.Join("\r\n", lines);
            if (add_space_before)
            {
                if (s.Length > 0 && !char.IsWhiteSpace(s[0]) && s[0] != '{' && !s.StartsWith("//") && !s.StartsWith("(*") && s[0] != ';' && !char.IsWhiteSpace(s[0]) && (sb.Length == 0 || !char.IsWhiteSpace(sb[sb.Length-1])))
                    sb.Append(' ');
            }
            else
                s = s.TrimStart(' ', '\t');
            sb.Append(s);
            if (add_space_after)
            {
                if (s.Length > 0)
                {
                    if (!char.IsWhiteSpace(s[s.Length - 1]) && s[s.Length - 1] != '}' && !s.EndsWith("//") && !s.EndsWith("*)"))
                        sb.Append(' ');
                }
                else
                    sb.Append(' ');
            }
            else
                s = s.TrimEnd(' ', '\t');
            add_space_after = false;
            add_space_before = false;
            add_newline_after = false;
            dec_offset_before_comment = false;
            add_new_line_else_specific = false;
            if (before)
            {
                insert_newline_after_prev = false;
                insert_newline_after_prev_semicolon = false;
            }
        }

        private void WritePossibleCommentBefore(syntax_tree_node sn)
        {
            int pos = GetPosition(sn.source_context.begin_position.line_num, sn.source_context.begin_position.column_num);
            int prev_pos = 0;
            if (prev_sn != null)
                if (read_from_beg_pos)
                {
                    prev_pos = GetPosition(prev_sn.source_context.begin_position.line_num, prev_sn.source_context.begin_position.column_num) + keyword_offset;
                }
                else
                    prev_pos = GetPosition(prev_sn.source_context.end_position.line_num, prev_sn.source_context.end_position.column_num)+1;
            if (prev_pos < pos)
            {
                string comm = Text.Substring(prev_pos, pos - prev_pos);
                if (options.AggressiveMode == 1)
                {
                    if (comm.EndsWith(" ") && comm.IndexOf("\n") == -1)
                    {
                        comm = comm.TrimEnd(' ');
                        if (comm.Length == 0 || char.IsLetterOrDigit(comm[comm.Length - 1]) || add_space_before)
                            comm += " ";
                    }
                    if (comm.StartsWith(" ") && comm.IndexOf("\n") == -1)
                    {
                        comm = comm.TrimStart(' ');
                        if (comm.Length == 0 || char.IsLetterOrDigit(comm[0]) || add_space_after)
                            comm = " " + comm;
                    }
                }
                WriteCommentWithIndent(comm, true);
                read_from_beg_pos = false;
                keyword_offset = 0;
            }
            else if (prev_pos == pos)
            {
                keyword_offset = 0;
                read_from_beg_pos = false;
                if (insert_newline_after_prev)
                {
                    sb.AppendLine();
                    sb.AppendLine();
                    insert_newline_after_prev = false;
                }
            }
            skip_spaces_after = false;
            skip_spaces_before = false;
            add_space_after = false;
            add_space_before = false;
        }

        private void WritePossibleCommentAfter(syntax_tree_node sn)
        {
            int prev_pos = GetPosition(prev_sn.source_context.end_position.line_num, prev_sn.source_context.end_position.column_num) + 1;
            int pos = GetPosition(sn.source_context.end_position.line_num, sn.source_context.end_position.column_num) + 1;
            if (prev_pos < pos && cur_src_off < pos)
            {
                string comm = Text.Substring(prev_pos, pos - prev_pos);
                if (sn is program_module || sn is unit_module)
                    comm = Text.Substring(prev_pos);
                if (comm.StartsWith(" "))
                    add_space_before = true;
                WriteCommentWithIndent(comm, false);
                cur_src_off = pos;
            } 
        }

        private void IncOffset()
        {
            off += tab;
        }

        private void IncOffset(int tab)
        {
            off += tab;
        }

        private void DecOffset()
        {
            off -= tab;
            if (off < 0)
                off = 0;
        }

        private void DecOffset(int tab)
        {
            off -= tab;
            if (off < 0)
                off = 0;
        }

        private bool NewLineNeeded(statement_list sl)
        {
            return true;
        }

        private string GetUnaryOperator(Operators op)
        {
            switch (op)
            {
                case Operators.Plus: return "+";
                case Operators.Minus: return "-";
                case Operators.LogicalNOT: return "not ";
            }
            throw new NotSupportedException();
        }

        private string GetOperator(Operators op)
        {
            switch (op)
            {
                case Operators.Plus: return " + ";
                case Operators.Minus: return " - ";
                case Operators.Multiplication: return " * ";
                case Operators.Division: return " / ";
                case Operators.IntegerDivision: return " div ";
                case Operators.In: return " in ";
                case Operators.Equal: return " = ";
                case Operators.NotEqual: return " <> ";
                case Operators.ModulusRemainder: return " mod ";
                case Operators.Less: return " < ";
                case Operators.LessEqual: return " <= ";
                case Operators.Greater: return " > ";
                case Operators.GreaterEqual: return " >= ";
                case Operators.Is: return " is ";
                case Operators.As: return " as ";
                case Operators.LogicalAND: return " and ";
                case Operators.LogicalOR: return " or ";
                case Operators.BitwiseAND: return " and ";
                case Operators.BitwiseOR: return " or ";
                case Operators.BitwiseXOR: return " xor ";
                case Operators.BitwiseLeftShift: return " shl ";
                case Operators.BitwiseRightShift: return " shr ";
                case Operators.AssignmentAddition: return " += ";
                case Operators.AssignmentMultiplication: return " *= ";
                case Operators.AssignmentSubtraction: return " -= ";
                case Operators.AssignmentDivision: return " /= ";
            }
            throw new NotSupportedException();
        }

        private string GetParamKind(parametr_kind kind)
        {
            switch (kind)
            {
                case parametr_kind.const_parametr: return "const";
                case parametr_kind.out_parametr: return "out";
                case parametr_kind.params_parametr: return "params";
                case parametr_kind.var_parametr: return "var";
            }
            return "";
        }

        private string GetAccessModifier(access_modifer acc_mod)
        {
            switch (acc_mod)
            {
                case access_modifer.internal_modifer: return "internal";
                case access_modifer.private_modifer: return "private";
                case access_modifer.protected_modifer: return "protected";
                case access_modifer.public_modifer: return "public";
                case access_modifer.published_modifer: return "published";
                
            }
            return "";
        }

        private bool has_attributes(procedure_attributes_list attrs)
        {
            if (attrs.proc_attributes.Count == 0)
                return false;
            else if (attrs.proc_attributes.Count == 1)
            {
                if (attrs.proc_attributes[0].source_context == null)
                    return false;
                else
                    return true;
            }
            else
                return true;

        }

        private string GetClassKeyword(class_keyword key)
        {
            switch (key)
            {
                case class_keyword.Class: return "class";
                case class_keyword.Interface: return "interface";
                case class_keyword.Record: return "record";
                case class_keyword.TemplateClass: return "template class";
                case class_keyword.TemplateInterface: return "template interface";
                case class_keyword.TemplateRecord: return "template record";           
            }
            throw new NotSupportedException();
        }

        private string prepare_ident(string ident)
        {
            return ident;
        }

        private bool has_members(class_body_list body)
        {
            return body.class_def_blocks.Count != 0;
        }

        private void visit_node(syntax_tree_node sn)
        {
            if (sn != null && !(sn is empty_statement))
            {
                if (!caret_syn_node_found && sn.source_context != null && GetPosition(sn.source_context.begin_position.line_num, sn.source_context.begin_position.column_num) >= caret_pos)
                {
                    caret_syn_node_found = true;
                    if (GetPosition(sn.source_context.begin_position.line_num, sn.source_context.begin_position.column_num) == caret_pos)
                    {
                        output_caret_line = sn.source_context.begin_position.line_num;
                        output_caret_col = sn.source_context.begin_position.column_num;
                    }
                    else
                    if (prev_sn != null && prev_sn.source_context != null)
                    {
                        output_caret_line = prev_sn.source_context.begin_position.line_num;
                        output_caret_col = prev_sn.source_context.begin_position.column_num;
                    }
                    else if (sn.source_context != null)
                    {
                        output_caret_line = sn.source_context.begin_position.line_num;
                        output_caret_col = sn.source_context.begin_position.column_num;
                    }
                }
                if (!(sn is block) && !(sn is declarations) && !(sn is class_body_list && has_members(sn as class_body_list)) && 
                    !(sn is class_members) && !(sn is procedure_attributes_list) && !(sn is property_accessors) &&
                    !(sn is exception_block) && !(sn is exception_handler_list))
                {
                    if (sn.source_context != null)
                        WritePossibleCommentBefore(sn);
                    if (sn.source_context != null)
                        prev_sn = sn;
                    if (sn is variable_definitions || sn is array_type || sn is set_type_definition
                        || sn is repeat_node || sn is if_node || sn is while_node || sn is for_node
                        || sn is foreach_stmt || sn is var_statement || sn is try_stmt || sn is goto_statement
                        || sn is with_statement || sn is case_node || sn is function_header || sn is procedure_header
                        || sn is constructor || sn is destructor || sn is type_declarations || sn is consts_definitions_list
                        || sn is label_definitions || sn is class_definition || sn is uses_list || sn is uses_closure  || sn is unit_name || sn is program_name ||
                        sn is new_expr || sn is raise_stmt || sn is interface_node || sn is implementation_node
                        || sn is lock_stmt || sn is loop_stmt || sn is simple_property || sn is read_accessor_name || sn is write_accessor_name
                        || sn is formal_parameters || sn is bracket_expr || sn is record_const || sn is array_const || sn is exception_handler
                        || sn is try_handler_finally || sn is try_handler_except || sn is external_directive || sn is where_definition
                        || (sn is simple_const_definition && in_class && !in_procedure) || (sn is typed_const_definition && in_class && !in_procedure)
                        )
                        read_from_beg_pos = true;
                }
                sn.visit(this);
                if (!(sn is block) && !(sn is declarations) && !(sn is class_body_list) && !(sn is class_members))
                {
                    if (sn.source_context != null)
                        WritePossibleCommentAfter(sn);
                    if (sn.source_context != null /*&& !(sn is exception_handler)*/)
                        prev_sn = sn;
                }
            }
        }

        public override void DefaultVisit(syntax_tree_node sn)
        {
            throw new NotImplementedException();
        }

        #region IVisitor Member

        public override void visit(syntax_tree_node n)
        { }

        public override void visit(no_type_foreach _no_type_foreach)
        { }

        public override void visit(statement_list _statement_list)
        {
            bool tmp_init_part = false;
            if (_statement_list.left_logical_bracket != null)
            {
                /*if (options.AggressiveMode == 1)
                {
                    add_newline_before = true;
                }*/
                visit_node(_statement_list.left_logical_bracket);
            }
            if (!in_one_row(_statement_list) && _statement_list.left_logical_bracket != null && _statement_list.subnodes.Count > 0 && _statement_list.subnodes[0].source_context != null && _statement_list.left_logical_bracket.source_context.end_position.line_num == _statement_list.subnodes[0].source_context.begin_position.line_num)
                add_newline_after = true;
            else
                add_space_after = true;
            if (init_part)
            {
                init_part = false;
                tmp_init_part = true;
            }
            foreach (statement stmt in _statement_list.subnodes)
            {
                /*if (options.AggressiveMode == 1 && !(stmt is empty_statement))
                {
                    add_newline_before = true;
                }*/
                   
                if (stmt is assign)
                {
                    assign ass_stmt = stmt as assign;
                    if (ass_stmt.to is ident && (ass_stmt.to as ident).source_context == null)
                    {
                        visit_node(ass_stmt.from);
                        continue;
                    }
                }
                visit_node(stmt);
            }
            DecOffset();
            if (!tmp_init_part)
                if (_statement_list.right_logical_bracket != null)
                    if (!(_statement_list.left_logical_bracket != null && _statement_list.left_logical_bracket is token_info
                        && (_statement_list.left_logical_bracket as token_info).text.ToLower() == "finalization"))
                    {
                        dec_offset_before_comment = true;
                        add_space_after = true;
                        //if (options.AggressiveMode == 1)
                        //    add_newline_before = true;
                        visit_node(_statement_list.right_logical_bracket);
                    }
        }

        public override void visit(assign _assign)
        {
            multiline_stack_push(_assign);
            visit_node(_assign.to);
            //sb.Append(" := ");
            add_space_before = true;
            add_space_after = true;
            visit_node(_assign.from);
            multiline_stack_pop(_assign);
        }

        public override void visit(bin_expr _bin_expr)
        {
            multiline_stack_push(_bin_expr);
            visit_node(_bin_expr.left);
            add_space_before = true;
            add_space_after = true;
            visit_node(_bin_expr.right);
            multiline_stack_pop(_bin_expr);
        }

        public override void visit(un_expr _un_expr)
        {
            sb.Append(GetUnaryOperator(_un_expr.operation_type));
            visit_node(_un_expr.subnode);
        }

        public override void visit(bool_const _bool_const)
        {
            WriteNode(_bool_const);
        }

        public override void visit(int32_const _int32_const)
        {
            WriteNode(_int32_const);
        }

        public override void visit(double_const _double_const)
        {
            WriteNode(_double_const);
        }

        public override void visit(ident _ident)
        {
            WriteAmpersandIfNeed(_ident);
            sb.Append(prepare_ident(_ident.name));
        }

        public override void visit(named_type_reference _named_type_reference)
        {
            for (int i = 0; i < _named_type_reference.names.Count; i++)
            {
                visit_node(_named_type_reference.names[i]);
            }
        }

        public override void visit(variable_definitions _variable_definitions)
        {
            bool is_event = false;
            for (int i = 0; i < _variable_definitions.var_definitions.Count; i++)
            {
                if (_variable_definitions.var_definitions[i].is_event)
                    is_event = true;
            }
            if (is_event)
                sb.Append("event");
            else
                sb.Append("var");
            IncOffset();
            for (int i = 0; i < _variable_definitions.var_definitions.Count; i++)
            {
                if (i == 0)
                {
                    add_newline_before = true;
                    if (is_event)
                        keyword_offset = "event".Length;
                    else
                        keyword_offset = "var".Length;
                }
                if (options.AggressiveMode == 1)
                    add_newline_before = true;
                var_def_statement vds = _variable_definitions.var_definitions[i];
                //next_sn = (i < _variable_definitions.var_definitions.Count - 1) ? _variable_definitions.var_definitions[i + 1] : null;
                visit_node(vds);
            }
            DecOffset();
            if (!in_procedure)
                insert_newline_after_prev_semicolon = true;
        }

        public override void visit(ident_list _ident_list)
        {
            for (int i = 0; i < _ident_list.idents.Count; i++)
            {
                if (i > 0 && options.SpaceBetweenIdentifiers == 1)
                    add_space_after = true;
                visit_node(_ident_list.idents[i]);
            }
        }

        public override void visit(var_def_statement _var_def_statement)
        {
            /*if (_var_def_statement.attributes != null)
            {
                visit_node(_var_def_statement.attributes);
            }*/
            if (_var_def_statement.is_event || _var_def_statement.var_attr == definition_attribute.Static)
                read_from_beg_pos = true;
            visit_node(_var_def_statement.vars);
            if (_var_def_statement.vars_type != null)
            {
                //sb.Append(": ");
                add_space_after = true;
                visit_node(_var_def_statement.vars_type);
            }
            if (_var_def_statement.inital_value != null)
            {
                //sb.Append(" := ");
                add_space_after = true;
                add_space_before = true;
                visit_node(_var_def_statement.inital_value);
            }
        }

        public override void visit(declaration _declaration)
        {
            throw new NotImplementedException();
        }

        public override void visit(declarations _declarations)
        {
            foreach (declaration decl in _declarations.defs)
            {
                if ((decl is procedure_definition || decl is short_func_definition) && in_procedure)
                {
                    //insert_newline_after_prev_semicolon = true;
                    IncOffset();
                }
                visit_node(decl);
                if ((decl is procedure_definition || decl is short_func_definition) && in_procedure)
                    DecOffset();
            }
        }

        public override void visit(program_name _program_name)
        {
            //sb.Append("program ");
            sb.Append("program");
            SetKeywordOffset("program");
            visit_node(_program_name.prog_name);
            insert_newline_after_prev_semicolon = true;
            //sb.AppendLine(";");
        }

        public override void visit(string_const _string_const)
        {
            int pos = GetPosition(_string_const.source_context.begin_position.line_num, _string_const.source_context.begin_position.column_num);
            if (Text[pos] == '$')
                sb.Append('$');
            sb.Append("'");
            sb.Append(_string_const.Value.Replace("'","''"));
            sb.Append("'");
        }

        public override void visit(expression_list _expression_list)
        {
            for (int i = 0; i < _expression_list.expressions.Count; i++)
            {
                if (i > 0 && options.SpaceBetweenArguments == 1)
                    add_space_after = true;
                visit_node(_expression_list.expressions[i]);
            }
        }

        public override void visit(dereference _dereference)
        {
            throw new NotImplementedException();
        }

        public override void visit(roof_dereference _roof_dereference)
        {
            visit_node(_roof_dereference.dereferencing_value);
            //sb.Append('^');
        }

        public override void visit(indexer _indexer)
        {
            visit_node(_indexer.dereferencing_value);
            //sb.Append("[");
            visit_node(_indexer.indexes);
            //sb.Append("]");
        }

        public override void visit(for_node _for_node)
        {
            //WritePossibleMultilineComment();
            sb.Append("for");
            SetKeywordOffset("for");
            visit_node(_for_node.loop_variable);
            if (_for_node.type_name != null)
            {
                add_space_after = true;
                visit_node(_for_node.type_name);
            }
            add_space_after = true;
            add_space_before = true;
            visit_node(_for_node.initial_value);
            add_space_before = true;
            add_space_after = true;
            visit_node(_for_node.finish_value);
            add_space_before = true;
            //WriteKeyword(" do ");
            bool need_off = !(_for_node.statements is statement_list);
            if (!in_one_row(_for_node.statements))
                add_newline_after = true;
            if (need_off)
            IncOffset();
            visit_node(_for_node.statements);
            if (need_off)
            DecOffset();
        }

        public override void visit(repeat_node _repeat_node)
        {
            WriteKeyword("repeat");
            SetKeywordOffset("repeat");
            IncOffset();
            //add_newline_after = true;
            visit_node(_repeat_node.statements);
            sb.Append("until");
            //sb.Append(" ");
            add_space_before = true;
            multiline_stack_push(_repeat_node.expr);
            visit_node(_repeat_node.expr);
            multiline_stack_pop(_repeat_node.expr);
        }

        public override void visit(while_node _while_node)
        {
            sb.Append("while");
            SetKeywordOffset("while");
            multiline_stack_push(_while_node.expr);
            visit_node(_while_node.expr);
            multiline_stack_pop(_while_node.expr);
            if (!in_one_row(_while_node.statements))
                add_newline_after = true;
            add_space_before = true;
            //WriteKeyword(" do");
            bool need_off = !(_while_node.statements is statement_list);
            if (need_off)
            IncOffset();
            visit_node(_while_node.statements);
            if (need_off)
            DecOffset();
        }

        private void WriteKeyword(string s)
        {
            sb.Append(s);
        }

        public override void visit(if_node _if_node)
        {
            sb.Append("if"); //sb.Append(" ");
            SetKeywordOffset("if");
            visit_node(_if_node.condition);
            if (!in_one_row(_if_node.condition))
                add_newline_after = true;
            add_space_before = true;
            //WriteKeyword(" then");
            bool need_off = true;
            if (_if_node.then_body is statement_list)
                need_off = false;
            if (need_off)
                IncOffset();
            visit_node(_if_node.then_body);
            if (need_off)
                DecOffset();
            if (_if_node.else_body != null)
            {
                //WriteKeyword("else");
                need_off = true;
                add_space_before = true;
                if (_if_node.else_body is statement_list)
                    need_off = false;
                else if (!(_if_node.else_body is if_node))
                    add_new_line_else_specific = true;
                else
                    need_off = false;
                if (need_off)
                    IncOffset();
                visit_node(_if_node.else_body);
                if (need_off)
                    DecOffset();
            }
        }

        public override void visit(ref_type _ref_type)
        {
            sb.Append("^");
            visit_node(_ref_type.pointed_to);
        }

        public override void visit(diapason _diapason)
        {
            visit_node(_diapason.left);
            visit_node(_diapason.right);
        }

        private bool is_multi_dyn_arr(indexers_types _indexers_types)
        {
            for (int i = 0; i < _indexers_types.indexers.Count; i++)
            {
                if (_indexers_types.indexers[i] != null)
                    return false;
            }
            return true;
        }

        public override void visit(indexers_types _indexers_types)
        {
            if (is_multi_dyn_arr(_indexers_types))
            {
                WriteNode(_indexers_types);
            }
            else
            {
                //sb.Append("[");
                for (int i = 0; i < _indexers_types.indexers.Count; i++)
                {
                    if (i > 0)
                        add_space_after = true;
                    if (_indexers_types.indexers[i] != null)
                        visit_node(_indexers_types.indexers[i]);
                }
            }
        }

        public override void visit(array_type _array_type)
        {
            if (_array_type.indexers != null)
            {
                skip_spaces_after = true;
                visit_node(_array_type.indexers);
                add_space_after = false;
                add_space_before = false;
            }
            visit_node(_array_type.elements_type);
        }

        public override void visit(label_definitions _label_definitions)
        {
            sb.Append("label");
            SetKeywordOffset("label");
            visit_node(_label_definitions.labels);
        }

        public override void visit(procedure_attribute _procedure_attribute)
        {
            if (_procedure_attribute.source_context != null)
                switch (_procedure_attribute.attribute_type)
                {
                    case proc_attribute.attr_abstract: sb.Append("abstract"); return;
                    case proc_attribute.attr_forward: sb.Append("forward"); return;
                    case proc_attribute.attr_overload: sb.Append("overload"); return;
                    case proc_attribute.attr_override: sb.Append("override"); return;
                    case proc_attribute.attr_reintroduce: sb.Append("reintroduce"); return;
                    case proc_attribute.attr_virtual: sb.Append("virtual"); return;
                    case proc_attribute.attr_static: sb.Append("static"); return;
                    case proc_attribute.attr_extension: sb.Append("extensionmethod"); return;
                }
            return;
        }

        public override void visit(typed_parameters _typed_parametres)
        {
            if (_typed_parametres.attributes != null)
            {
                attr_on_new_line = false;
                visit_node(_typed_parametres.attributes);
                attr_on_new_line = true;
            }
            if (_typed_parametres.param_kind != parametr_kind.none)
            {
                string s = GetParamKind(_typed_parametres.param_kind);
                sb.Append(s);
                SetKeywordOffset(s);
                read_from_beg_pos = true;
            }
            //sb.Append(GetParamKind(_typed_parametres.param_kind));
            visit_node(_typed_parametres.idents);
            add_space_after = true;
            //sb.Append(": ");
            if (!(_typed_parametres.vars_type is lambda_inferred_type))
                visit_node(_typed_parametres.vars_type);
            if (_typed_parametres.inital_value != null)
            {
                //sb.Append(" := ");
                add_space_before = true;
                add_space_after = true;
                visit_node(_typed_parametres.inital_value);
            }
        }

        public override void visit(formal_parameters _formal_parametres)
        {
            if (!is_lambda_parameters)
            {
                sb.Append("(");
                keyword_offset = 1;
            }
            else
                is_lambda_parameters = true;
            for (int i = 0; i < _formal_parametres.params_list.Count; i++)
            {
                if (i > 0)
                    add_space_after = true;
                visit_node(_formal_parametres.params_list[i]);
            }
        }

        public override void visit(procedure_attributes_list _procedure_attributes_list)
        {
            for (int i=0; i<_procedure_attributes_list.proc_attributes.Count; i++)
            {
                if (_procedure_attributes_list.proc_attributes[i].source_context == null)
                    continue;
                add_space_after = true;
                visit_node(_procedure_attributes_list.proc_attributes[i]);
            }
        }

        public override void visit(procedure_header _procedure_header)
        {
            multiline_stack_push(_procedure_header);
            sb.Append("procedure");
            SetKeywordOffset("procedure");
            if (_procedure_header.name != null)
                visit_node(_procedure_header.name);
            else
                add_space_before = false;
            if (_procedure_header.template_args != null && !(_procedure_header.name.meth_name is operator_name_ident))
            {
                sb.Append("<");
                visit_node(_procedure_header.template_args);
                //sb.Append(">");
            }
            if (_procedure_header.parameters != null)
            {
                visit_node(_procedure_header.parameters);
            }
            
            if (_procedure_header.proc_attributes != null && has_attributes(_procedure_header.proc_attributes))
            {
                if (is_forward(_procedure_header))
                    _procedure_header.source_context = new SourceContext(_procedure_header.source_context.begin_position.line_num, _procedure_header.source_context.begin_position.column_num, _procedure_header.proc_attributes.source_context.end_position.line_num, _procedure_header.proc_attributes.source_context.end_position.column_num);
                visit_node(_procedure_header.proc_attributes);
            }
            if (_procedure_header.where_defs != null)
                visit_node(_procedure_header.where_defs);
            keyword_offset = 0;
            read_from_beg_pos = false;
            multiline_stack_pop(_procedure_header);
            //add_newline_before = true;
        }

        public override void visit(function_header _function_header)
        {
            //if (_function_header.attributes != null)
            //    visit_node(_function_header.attributes);
            multiline_stack_push(_function_header);

            sb.Append("function");
            SetKeywordOffset("function");
            if (_function_header.name != null)
                visit_node(_function_header.name);
            else
                add_space_before = false;
            if (_function_header.template_args != null && !(_function_header.name.meth_name is operator_name_ident))
            {
                sb.Append("<");
                visit_node(_function_header.template_args);
            }
            if (_function_header.parameters != null)
            {
                visit_node(_function_header.parameters);
            }
            //sb.Append(": ");
            add_space_after = true;
            visit_node(_function_header.return_type);
            
            if (_function_header.proc_attributes != null && has_attributes(_function_header.proc_attributes))
            {
                if (is_forward(_function_header))
                    _function_header.source_context = new SourceContext(_function_header.source_context.begin_position.line_num, _function_header.source_context.begin_position.column_num, _function_header.proc_attributes.source_context.end_position.line_num, _function_header.proc_attributes.source_context.end_position.column_num);
                visit_node(_function_header.proc_attributes);
            }
            if (_function_header.where_defs != null)
            {
                visit_node(_function_header.where_defs);
            }
            keyword_offset = 0;
            read_from_beg_pos = false;
            multiline_stack_pop(_function_header);
            //add_newline_before = true;
        }

        public override void visit(procedure_definition _procedure_definition)
        {
            if (_procedure_definition.proc_body is external_directive)
                multiline_stack_push(_procedure_definition);
            visit_node(_procedure_definition.proc_header);
            bool tmp_in_proc = in_procedure;
            in_procedure = true;
            if (_procedure_definition.proc_body is external_directive)
                add_space_after = true;
            if (/*options.AggressiveMode == 1 && _procedure_definition.proc_body != null ||*/ (!in_one_row(_procedure_definition) && in_same_row(_procedure_definition.proc_header, _procedure_definition.proc_body)))
            { 
                if (distance(_procedure_definition.proc_header, _procedure_definition.proc_body) > 0)
                    add_newline_after = true;
                else
                    sb.AppendLine();
            }
            if (_procedure_definition.proc_body is block && (_procedure_definition.proc_body as block).program_code.left_logical_bracket == null)
            {
                add_space_before = true;
                add_space_after = true;
            }
            visit_node(_procedure_definition.proc_body);
            in_procedure = tmp_in_proc;
            //if (in_procedure)
            //    DecOffset();
            if (_procedure_definition.proc_body is external_directive || is_forward(_procedure_definition.proc_header))
                insert_newline_after_prev_semicolon = true;
            else
                insert_newline_after_prev_semicolon = true;
            if (_procedure_definition.proc_body is external_directive)
                multiline_stack_pop(_procedure_definition);
        }

        public override void visit(type_declaration _type_declaration)
        {
            visit_node(_type_declaration.type_name);
            //sb.Append(" = ");
            add_space_after = true;
            add_space_before = true;
            visit_node(_type_declaration.type_def);
        }

        public override void visit(type_declarations _type_declarations)
        {
            sb.Append("type");
            IncOffset();
            for (int i = 0; i < _type_declarations.types_decl.Count; i++)
            {
                if (i == 0)
                {
                    add_newline_before = true;
                    keyword_offset = "type".Length;
                }
                type_declaration td = _type_declarations.types_decl[i];
                //next_sn = (i < _variable_definitions.var_definitions.Count - 1) ? _variable_definitions.var_definitions[i + 1] : null;
                visit_node(td);
            }
            DecOffset();
            if (!in_procedure)
                insert_newline_after_prev_semicolon = true;
        }

        public override void visit(simple_const_definition _simple_const_definition)
        {
            if (in_class && !in_procedure)
            {
                sb.Append("const");
                SetKeywordOffset("const");
                add_space_before = true;
            }
            visit_node(_simple_const_definition.const_name);
            //sb.Append(" = ");
            add_space_before = true;
            add_space_after = true;
            visit_node(_simple_const_definition.const_value);
            //sb.AppendLine(";");
        }

        public override void visit(typed_const_definition _typed_const_definition)
        {
            if (in_class && !in_procedure)
            {
                sb.Append("const");
                SetKeywordOffset("const");
                add_space_before = true;
            }
            visit_node(_typed_const_definition.const_name);
            //sb.Append(" : ");
            add_space_after = true;
            visit_node(_typed_const_definition.const_type);
            //sb.Append(" = ");
            add_space_before = true;
            add_space_after = true;
            visit_node(_typed_const_definition.const_value);
            //sb.AppendLine(";");
        }

        public override void visit(consts_definitions_list _consts_definitions_list)
        {
            sb.Append("const");
            IncOffset();
            for (int i = 0; i < _consts_definitions_list.const_defs.Count; i++)
            {
                if (i == 0)
                {
                    add_newline_before = true;
                    keyword_offset = "const".Length;
                }
                if (options.AggressiveMode == 1)
                    add_newline_before = true;
                const_definition cnst = _consts_definitions_list.const_defs[i];
                //next_sn = (i < _variable_definitions.var_definitions.Count - 1) ? _variable_definitions.var_definitions[i + 1] : null;
                visit_node(cnst);
            }
            DecOffset();
            if (!in_procedure)
                insert_newline_after_prev_semicolon = true;
        }

        public override void visit(unit_name _unit_name)
        {
            if (_unit_name.HeaderKeyword == UnitHeaderKeyword.Unit)
            {
                sb.Append("unit");
                SetKeywordOffset("unit");
            }
            else if (_unit_name.HeaderKeyword == UnitHeaderKeyword.Library)
            {
                sb.Append("library");
                SetKeywordOffset("library");
            }
            else if (_unit_name.HeaderKeyword == UnitHeaderKeyword.Namespace)
            {
                sb.Append("namespace");
                SetKeywordOffset("namespace");
            }
            visit_node(_unit_name.idunit_name);
            //sb.AppendLine(";");
        }

        public override void visit(unit_or_namespace _unit_or_namespace)
        {
            for (int i = 0; i < _unit_or_namespace.name.idents.Count; i++)
            {
                visit_node(_unit_or_namespace.name.idents[i]);
            }
        }

        public override void visit(uses_unit_in _uses_unit_in)
        {
            for (int i = 0; i < _uses_unit_in.name.idents.Count; i++)
            {
                visit_node(_uses_unit_in.name.idents[i]);
            }
            add_space_before = true;
            visit_node(_uses_unit_in.in_file);
        }

        public override void visit(uses_list _uses_list)
        {
            //sb.Append("uses");
            IncOffset(tab + "uses".Length - 1);
            for (int i = 0; i < _uses_list.units.Count; i++)
            {
                if (i > 0)
                    add_space_after = true;
                visit_node(_uses_list.units[i]);
            }
            insert_newline_after_prev = true;
            DecOffset(tab + "uses".Length - 1);
        }

        public override void visit(unit_module _unit_module)
        {
            has_implementation = _unit_module.implementation_part != null;
            if (_unit_module.attributes != null)
                visit_node(_unit_module.attributes);
            visit_node(_unit_module.unit_name);
            insert_newline_after_prev = true;
            if (_unit_module.interface_part != null)
                visit_node(_unit_module.interface_part);
            if (_unit_module.implementation_part != null)
                visit_node(_unit_module.implementation_part);
            init_part = true;
            if (_unit_module.initialization_part != null)
            {
                if (_unit_module.initialization_part.source_context != null)
                _unit_module.initialization_part.source_context = new SourceContext(_unit_module.initialization_part.left_logical_bracket.source_context.begin_position.line_num,
                    _unit_module.initialization_part.left_logical_bracket.source_context.begin_position.column_num,_unit_module.initialization_part.source_context.end_position.line_num,
                    _unit_module.initialization_part.source_context.end_position.column_num);
                visit_node(_unit_module.initialization_part);
            }
            init_part = true;
            if (_unit_module.finalization_part != null)
            {
                if (_unit_module.finalization_part.source_context != null)
                _unit_module.finalization_part.source_context = new SourceContext(_unit_module.finalization_part.left_logical_bracket.source_context.begin_position.line_num,
                   _unit_module.finalization_part.left_logical_bracket.source_context.begin_position.column_num, _unit_module.finalization_part.source_context.end_position.line_num,
                   _unit_module.finalization_part.source_context.end_position.column_num);
                visit_node(_unit_module.finalization_part);
            }
            //sb.Append(".");
        }

        public override void visit(program_module _program_module)
        {
            if (_program_module.program_name != null)
                visit_node(_program_module.program_name);
            if (_program_module.used_units != null)
                visit_node(_program_module.used_units);
            if (_program_module.program_block != null)
                visit_node(_program_module.program_block); 
            //sb.Append(".");
        }

        public override void visit(hex_constant _hex_constant)
        {
            sb.Append("$");
            sb.Append(_hex_constant.val.ToString());
        }

        public override void visit(get_address _get_address)
        {
            sb.Append("@");
            visit_node(_get_address.address_of);
        }

        public override void visit(case_variant _case_variant)
        {
            visit_node(_case_variant.conditions);
            //sb.Append(": ");
            add_space_after = true;
            bool one_row = in_one_row(_case_variant.exec_if_true);
            if (!one_row)
            {
                add_newline_after = true;
                IncOffset();
            }
            visit_node(_case_variant.exec_if_true);
            if (!one_row)
            {
                DecOffset();
            }
            //sb.AppendLine(";");
        }

        public override void visit(case_node _case_node)
        {
            sb.Append("case");
            SetKeywordOffset("case");
            visit_node(_case_node.param);
            //sb.AppendLine(" of");
            add_space_before = true;
            IncOffset();
            if (_case_node.conditions != null)
                visit_node(_case_node.conditions);
            
            DecOffset();
            if (_case_node.else_statement != null)
            {
                //IncOffset();
                add_new_line_else_specific = true;
                IncOffset();
                visit_node(_case_node.else_statement);
                //DecOffset();
            }
        }

        public override void visit(method_name _method_name)
        {
            if (_method_name.ln != null)
            {
                foreach (ident id in _method_name.ln)
                    visit_node(id);
            }
            else
            {
                if (_method_name.class_name != null)
                    visit_node(_method_name.class_name);
                if (_method_name.explicit_interface_name != null)
                    visit_node(_method_name.explicit_interface_name);
                if (_method_name.meth_name != null)
                    visit_node(_method_name.meth_name);
            }
        }

        public override void visit(dot_node _dot_node)
        {
            visit_node(_dot_node.left);
            visit_node(_dot_node.right);
        }

        public override void visit(empty_statement _empty_statement)
        {
            //sb.Append("");
        }

        public override void visit(goto_statement _goto_statement)
        {
            sb.Append("goto");
            SetKeywordOffset("goto");
            visit_node(_goto_statement.label);
        }

        public override void visit(labeled_statement _labeled_statement)
        {
            visit_node(_labeled_statement.label_name);
            add_space_after = true;
            visit_node(_labeled_statement.to_statement);
        }

        public override void visit(with_statement _with_statement)
        {
            sb.Append("with");
            SetKeywordOffset("with");
            multiline_stack_push(_with_statement.do_with);
            visit_node(_with_statement.do_with);
            multiline_stack_pop(_with_statement.do_with);
            //sb.Append(" do");
            add_space_before = true;
            bool need_off = !(_with_statement.what_do is statement_list);
            if (need_off)
                IncOffset();
            if (!in_one_row(_with_statement.what_do))
                add_newline_after = true;
            visit_node(_with_statement.what_do);
            if (need_off)
                DecOffset();
        }

        public override void visit(method_call _method_call)
        {
            multiline_stack_push(_method_call);
            visit_node(_method_call.dereferencing_value);
            
            if (_method_call.parameters != null)
            {
                //sb.Append("(");
                visit_node(_method_call.parameters);
                //sb.Append(")");
            }
            multiline_stack_pop(_method_call);
        }

        public override void visit(pascal_set_constant _pascal_set_constant)
        {
            if (_pascal_set_constant.values != null)
            {
                sb.Append("[");
                visit_node(_pascal_set_constant.values);
            }
            else
                WriteNode(_pascal_set_constant);
            //sb.Append("]");
        }

        public override void visit(array_const _array_const)
        {
            if (_array_const.elements != null)
            {
                multiline_stack_push(_array_const);
                sb.Append("(");
                keyword_offset = 1;

                visit_node(_array_const.elements);
                multiline_stack_pop(_array_const);
            }
            else
            {
                WriteNode(_array_const);
                read_from_beg_pos = false;
            }
        }

        public override void visit(write_accessor_name _write_accessor_name)
        {
            sb.Append("write");
            SetKeywordOffset("write");
            if (_write_accessor_name.accessor_name != null)
                visit_node(_write_accessor_name.accessor_name);
            else
                read_from_beg_pos = false;
        }

        public override void visit(read_accessor_name _read_accessor_name)
        {
            sb.Append("read");
            SetKeywordOffset("read");
            if (_read_accessor_name.accessor_name != null)
                visit_node(_read_accessor_name.accessor_name);
            else
                read_from_beg_pos = false;
        }

        public override void visit(property_accessors _property_accessors)
        {
            //MikhailoMMX - теперь исходный порядок Accessor'ов учитывается
            syntax_tree_node FirstAccessor = _property_accessors.read_accessor;
            syntax_tree_node SecondAccessor = _property_accessors.write_accessor;
            if (FirstAccessor != null && SecondAccessor != null)
                if ((FirstAccessor.source_context.begin_position.line_num>SecondAccessor.source_context.begin_position.line_num)
                    || ((FirstAccessor.source_context.begin_position.line_num == SecondAccessor.source_context.begin_position.line_num)
                        && (FirstAccessor.source_context.begin_position.column_num > SecondAccessor.source_context.begin_position.column_num)))
                {
                    syntax_tree_node t = FirstAccessor;
                    FirstAccessor = SecondAccessor;
                    SecondAccessor = t;
                }
            //\MikhailoMMX
            if (FirstAccessor != null)
            {
                visit_node(FirstAccessor);
            }
            if (SecondAccessor != null)
            {
                add_space_after = true;
                visit_node(SecondAccessor);
            }
        }

        public override void visit(simple_property _simple_property)
        {
            multiline_stack_push(_simple_property);
            if (_simple_property.attr != definition_attribute.Static)
            {
                sb.Append("property");
                SetKeywordOffset("property");
            }
            visit_node(_simple_property.property_name);
            if (_simple_property.parameter_list != null)
            {
                visit_node(_simple_property.parameter_list);
            }
            add_space_after = true;
            visit_node(_simple_property.property_type);
            add_space_after = true;
            if (_simple_property.accessors != null)
                visit_node(_simple_property.accessors);
            if (_simple_property.array_default != null)
                visit_node(_simple_property.array_default);
            add_space_before = false;
            add_space_after = false;
            multiline_stack_pop(_simple_property);
        }

        public override void visit(class_members _class_members)
        {
            if (_class_members.access_mod != null && _class_members.access_mod.source_context != null)
            {
                bool already_off = true;
                declaration first_decl = null;
                if (_class_members.members.Count > 0)
                    first_decl = _class_members.members[0];
                if (first_decl is short_func_definition)
                    first_decl = (first_decl as short_func_definition).procdef;
                if (first_decl != null && first_decl.source_context != null && _class_members.access_mod.source_context.end_position.line_num == first_decl.source_context.begin_position.line_num)
                    IncOffset();
                else
                    already_off = false;
                visit_node(_class_members.access_mod);
                if (first_decl != null && !(_class_members.members[0] is short_func_definition))
                    sb.Append(" ");
                if (!already_off)
                    IncOffset();
            }
            else
                IncOffset();
            foreach (declaration decl in _class_members.members)
            {
                visit_node(decl);
            }
            DecOffset();
        }

        public override void visit(access_modifer_node _access_modifer_node)
        {
            sb.Append(GetAccessModifier(_access_modifer_node.access_level));
        }

        public override void visit(class_body_list _class_body)
        {
            in_class = true;
            if (_class_body.class_def_blocks.Count == 0)
                sb.Append("end");
            else
                foreach (class_members cmem in _class_body.class_def_blocks)
                    visit_node(cmem);
            in_class = false;
        }

        public override void visit(class_definition _class_definition)
        {
            bool class_pred = true;
            if (_class_definition.class_parents != null)
            {
                class_pred = false;
                visit_node(_class_definition.class_parents);
            }

            if (_class_definition.where_section != null)
            {
                class_pred = false;
                add_space_after = true;
                visit_node(_class_definition.where_section);
            }
            if (_class_definition.body != null)
            {
                if (!((_class_definition.body.class_def_blocks.Count == 0 || _class_definition.body.class_def_blocks[0].members != null && _class_definition.body.class_def_blocks[0].members.Count == 0) && _class_definition.class_parents == null && _class_definition.where_section == null))
                {
                    class_pred = false;
                    visit_node(_class_definition.body);
                }
                else
                {
                    read_from_beg_pos = false;
                    class_pred = false;
                    WriteNodeWithIndent(_class_definition);
                }
            }
            if (class_pred)
            {
                read_from_beg_pos = false;
                WriteNode(_class_definition);
            }
            insert_newline_after_prev = false;
            insert_newline_after_prev_semicolon = false;
        }

        public override void visit(set_type_definition _set_type_definition)
        {
            visit_node(_set_type_definition.of_type);
        }

        public override void visit(record_const_definition _record_const_definition)
        {
            visit_node(_record_const_definition.name);
            //sb.Append(": ");
            add_space_before = false;
            add_space_after = true;
            visit_node(_record_const_definition.val);
        }

        public override void visit(record_const _record_const)
        {
            multiline_stack_push(_record_const);
            sb.Append("(");
            keyword_offset = 1;
            for (int i = 0; i < _record_const.rec_consts.Count; i++)
            {
                if (i > 0)
                    add_space_after = true;
                visit_node(_record_const.rec_consts[i]);
            }
            multiline_stack_pop(_record_const);
        }

        public override void visit(record_type _record_type)
        {
            throw new NotImplementedException();
        }

        public override void visit(enum_type_definition _enum_type_definition)
        {
            sb.Append("(");
            visit_node(_enum_type_definition.enumerators);
            //sb.Append(")");
        }

        public override void visit(char_const _char_const)
        {
            sb.Append("'");
            sb.Append(_char_const.cconst.ToString());
            sb.Append("'");
        }

        public override void visit(raise_statement _raise_statement)
        {
            sb.Append("raise");
            sb.Append(" ");
            visit_node(_raise_statement.excep);
        }

        public override void visit(sharp_char_const _sharp_char_const)
        {
            sb.Append("#");
            sb.Append(_sharp_char_const.char_num.ToString());
        }

        public override void visit(literal_const_line _literal_const_line)
        {
            foreach (literal l in _literal_const_line.literals)
                visit_node(l);
        }

        public override void visit(string_num_definition _string_num_definition)
        {
            visit_node(_string_num_definition.name);
            visit_node(_string_num_definition.num_of_symbols);
        }

        public override void visit(variant _variant)
        {
            throw new NotImplementedException();
        }

        public override void visit(variant_list _variant_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(variant_type _variant_type)
        {
            throw new NotImplementedException();
        }

        public override void visit(variant_types _variant_types)
        {
            throw new NotImplementedException();
        }

        public override void visit(variant_record_type _variant_record_type)
        {
            throw new NotImplementedException();
        }

        public override void visit(procedure_call _procedure_call)
        {
            visit_node(_procedure_call.func_name);
        }

        public override void visit(class_predefinition _class_predefinition)
        {
            throw new NotImplementedException();
        }

        public override void visit(nil_const _nil_const)
        {
            sb.Append("nil");
        }

        public override void visit(file_type_definition _file_type_definition)
        {
            if (_file_type_definition.elem_type != null)
            {
                //sb.Append("file of ");
                visit_node(_file_type_definition.elem_type);
            }
            else
            {
                sb.Append("file");
            }
        }

        public override void visit(constructor _constructor)
        {
            if (_constructor.name == null && _constructor.template_args == null && _constructor.parameters == null && _constructor.where_defs == null && (_constructor.proc_attributes == null || !has_attributes(_constructor.proc_attributes)))
            {
                WriteNode(_constructor);
                read_from_beg_pos = false;
                keyword_offset = 0;
                return;
            }
            multiline_stack_push(_constructor);
            if (!_constructor.class_keyword)
            {
                sb.Append("constructor");
                SetKeywordOffset("constructor");
            }
            if (_constructor.name != null)
            {
                //add_space_before = true;
                visit_node(_constructor.name);
            }
            else
                add_space_before = false;
            if (_constructor.template_args != null)
            {
                sb.Append("<");
                visit_node(_constructor.template_args);
            }
            if (_constructor.parameters != null)
            {
                visit_node(_constructor.parameters);
            }
            if (_constructor.where_defs != null)
                visit_node(_constructor.where_defs);
            if (_constructor.proc_attributes != null && has_attributes(_constructor.proc_attributes))
                visit_node(_constructor.proc_attributes);
            read_from_beg_pos = false;
            keyword_offset = 0;
            multiline_stack_pop(_constructor);
        }

        public override void visit(destructor _destructor)
        {
            multiline_stack_push(_destructor);
            sb.Append("destructor");
            SetKeywordOffset("destructor");
            if (_destructor.name != null)
            {
                add_space_before = true;
                visit_node(_destructor.name);
            }
            if (_destructor.template_args != null)
            {
                sb.Append("<");
                visit_node(_destructor.template_args);
            }
            if (_destructor.parameters != null)
            {
                visit_node(_destructor.parameters);
            }
            if (_destructor.where_defs != null)
                visit_node(_destructor.where_defs);
            if (_destructor.proc_attributes != null && has_attributes(_destructor.proc_attributes))
                visit_node(_destructor.proc_attributes);
            read_from_beg_pos = false;
            keyword_offset = 0;
            multiline_stack_pop(_destructor);
        }

        public override void visit(inherited_method_call _inherited_method_call)
        {
            visit_node(_inherited_method_call.method_name);
            if (_inherited_method_call.exprs != null)
            {
                sb.Append("(");
                visit_node(_inherited_method_call.exprs);
                sb.Append(")");
            }
        }

        public override void visit(typecast_node _typecast_node)
        {
            visit_node(_typecast_node.expr);
            if (_typecast_node.cast_op == op_typecast.is_op || _typecast_node.cast_op == op_typecast.as_op)
            {
                add_space_before = true;
                add_space_after = true;
            }

            visit_node(_typecast_node.type_def);
        }

        public override void visit(interface_node _interface_node)
        {
            if (_interface_node.source_context != null && has_implementation)
            {
                sb.Append("interface");
                SetKeywordOffset("interface");
                insert_newline_after_prev_semicolon = true;
            }
            else if (_interface_node.source_context == null)
            {
                read_from_beg_pos = false;
                //return;
            }
            visit_node(_interface_node.uses_modules);
            if (_interface_node.interface_definitions != null)
                visit_node(_interface_node.interface_definitions);
        }

        public override void visit(implementation_node _implementation_node)
        {
            if (_implementation_node.source_context != null)
            {
                sb.Append("implementation");
                SetKeywordOffset("implementation");
                insert_newline_after_prev_semicolon = true;
            }
            visit_node(_implementation_node.uses_modules);
            if (_implementation_node.implementation_definitions != null)
                visit_node(_implementation_node.implementation_definitions);
        }

        public override void visit(diap_expr _diap_expr)
        {
            throw new NotImplementedException();
        }

        public override void visit(block _block)
        {
            if (_block.defs != null)
                visit_node(_block.defs);
            visit_node(_block.program_code);
        }

        public override void visit(proc_block _proc_block)
        {
            throw new NotImplementedException();
        }

        public override void visit(array_of_named_type_definition _array_of_named_type_definition)
        {
            throw new NotImplementedException();
        }

        public override void visit(array_of_const_type_definition _array_of_const_type_definition)
        {
            throw new NotImplementedException();
        }

        public override void visit(literal _literal)
        {
            throw new NotImplementedException();
        }

        public override void visit(case_variants _case_variants)
        {
            foreach (case_variant cv in _case_variants.variants)
                visit_node(cv);
        }

        public override void visit(diapason_expr _diapason_expr)
        {
            visit_node(_diapason_expr.left);
            visit_node(_diapason_expr.right);
        }

        public override void visit(var_def_list_for_record _var_def_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(record_type_parts _record_type_parts)
        {
            throw new NotImplementedException();
        }

        public override void visit(property_array_default _property_array_default)
        {
            WriteNode(_property_array_default);
        }

        public override void visit(property_interface _property_interface)
        {
            throw new NotImplementedException();
        }

        public override void visit(property_parameter _property_parameter)
        {
            visit_node(_property_parameter.names);
            add_space_after = true;
            visit_node(_property_parameter.type);
        }

        public override void visit(property_parameter_list _property_parameter_list)
        {
            for (int i=0; i<_property_parameter_list.parameters.Count; i++)
            {
                if (i > 0)
                    add_space_after = true;
                visit_node(_property_parameter_list.parameters[i]);
            }
        }

        public override void visit(inherited_ident _inherited_ident)
        {
            sb.Append("inherited ");
            sb.Append(_inherited_ident.name);
        }

        public override void visit(format_expr _format_expr)
        {
            visit_node(_format_expr.expr);
            visit_node(_format_expr.format1);
            if (_format_expr.format2 != null)
            {
                visit_node(_format_expr.format2);
            }
        }

        public override void visit(initfinal_part _initfinal_part)
        {
            throw new NotImplementedException();
        }

        public override void visit(token_info _token_info)
        {
            if (string.Compare(_token_info.text, "begin", true) == 0)
            {
                WriteKeyword("begin");
                IncOffset();
            }
            else if (string.Compare(_token_info.text, "initialization", true) == 0)
            {
                WriteKeyword("initialization");
                IncOffset();
            }
            else if (string.Compare(_token_info.text, "finalization", true) == 0)
            {
                WriteKeyword("finalization");
                IncOffset();
            }
            else if (string.Compare(_token_info.text, "finally", true) == 0)
            {
                WriteKeyword("finally");
                IncOffset();
            }
            else if (string.Compare(_token_info.text, "end", true) == 0)
            {
                sb.Append("end");
            } 
        }

        public override void visit(raise_stmt _raise_stmt)
        {
            sb.Append("raise");
            SetKeywordOffset("raise");
            if (_raise_stmt.expr != null)
                visit_node(_raise_stmt.expr);
            else
                add_space_before = false;
        }

        public override void visit(op_type_node _op_type_node)
        {
            throw new NotImplementedException();
        }

        public override void visit(file_type _file_type)
        {
            sb.Append("file");
            if (_file_type.file_of_type != null)
            {
                sb.Append(" of ");
                visit_node(_file_type.file_of_type);
            }
            else
            {
            }
        }

        public override void visit(known_type_ident _known_type_ident)
        {
            throw new NotImplementedException();
        }

        public override void visit(exception_handler ex)
        {
            sb.Append("on");
            SetKeywordOffset("on");
            if (ex.variable != null)
            {
                visit_node(ex.variable);
                add_space_after = true;
            }
            visit_node(ex.type_name);
            add_space_before = true;
            if (!(ex.statements is statement_list))
                IncOffset();
            visit_node(ex.statements);
            if (!(ex.statements is statement_list))
                DecOffset();
        }

        public override void visit(exception_ident _exception_ident)
        {
            throw new NotImplementedException();
        }

        public override void visit(exception_handler_list _exception_handler_list)
        {
            foreach (exception_handler ex in _exception_handler_list.handlers)
            {
                visit_node(ex);
            }
        }

        public override void visit(exception_block _exception_block)
        {
            if (_exception_block.handlers != null)
            {
                IncOffset();
                visit_node(_exception_block.handlers);
                DecOffset();
            }
            if (_exception_block.stmt_list != null)
            {
                IncOffset();
                visit_node(_exception_block.stmt_list);
            }
            if (_exception_block.else_stmt_list != null)
            {
                add_space_before = true;
                if (!(_exception_block.else_stmt_list.Count == 1 && _exception_block.else_stmt_list.list[0] is empty_statement))
                {
                    add_new_line_else_specific = true;
                    IncOffset();
                    visit_node(_exception_block.else_stmt_list);
                }
                else
                {
                    add_newline_after = false;
                }
                    
                //DecOffset();
            }
        }

        public override void visit(try_handler _try_handler)
        {
            throw new NotImplementedException();
        }

        public override void visit(try_handler_finally _try_handler_finally)
        {
            _try_handler_finally.stmt_list.source_context = new SourceContext(_try_handler_finally.source_context.LeftSourceContext, _try_handler_finally.source_context.RightSourceContext);
            visit_node(_try_handler_finally.stmt_list);
        }

        public override void visit(try_handler_except _try_handler_except)
        {
            sb.Append("except");
            SetKeywordOffset("except");
            add_newline_after = true;
            visit_node(_try_handler_except.except_block);
            add_newline_after = false;
        }

        public override void visit(try_stmt _try_stmt)
        {
            WriteKeyword("try");
            SetKeywordOffset("try");
            add_newline_after = true;
            IncOffset();
            visit_node(_try_stmt.stmt_list);
            if (_try_stmt.handler is try_handler_except)
            {
                try_handler_except hndlr = _try_stmt.handler as try_handler_except;
                visit_node(_try_stmt.handler);
                if (hndlr.except_block.stmt_list != null && hndlr.except_block.stmt_list.subnodes.Count == 1 && hndlr.except_block.stmt_list.subnodes[0] is empty_statement)
                {
                    WriteNode(_try_stmt.handler, "except".Length);
                    read_from_beg_pos = false;
                    add_space_before = false;
                    add_space_after = false;
                }

            }
            else if (_try_stmt.handler is try_handler_finally)
            {
                try_handler_finally hndlr = _try_stmt.handler as try_handler_finally;
                visit_node(_try_stmt.handler);
            }
            read_from_beg_pos = false;
        }

        public override void visit(inherited_message _inherited_message)
        {
            sb.Append("inherited");
        }

        public override void visit(external_directive _external_directive)
        {
            sb.Append("external");
            SetKeywordOffset("external");
            if (_external_directive.modulename != null)
            {
                visit_node(_external_directive.modulename);
            }
            if (_external_directive.name != null)
            {
                add_space_before = true;
                add_space_after = true;
                visit_node(_external_directive.name);
            }
            add_space_before = false;
            add_space_after = false;
            read_from_beg_pos = false;
        }

        public override void visit(using_list _using_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(jump_stmt _jump_stmt)
        {
            throw new NotImplementedException();
        }

        public override void visit(loop_stmt loop)
        {
            sb.Append("loop");
            SetKeywordOffset("loop");

            multiline_stack_push(loop.count);
            visit_node(loop.count);
            multiline_stack_pop(loop.count);
            if (!in_one_row(loop.stmt))
                add_newline_after = true;
            add_space_before = true;
            //WriteKeyword(" do");
            bool need_off = !(loop.stmt is statement_list);
            if (need_off)
                IncOffset();
            visit_node(loop.stmt);
            if (need_off)
                DecOffset();
        }

        public override void visit(foreach_stmt _foreach_stmt)
        {
            WriteKeyword("foreach");
            SetKeywordOffset("foreach");
            visit_node(_foreach_stmt.identifier);
            if (_foreach_stmt.type_name != null)
            {
                add_space_after = true;
                visit_node(_foreach_stmt.type_name);
            }
            add_space_after = true;
            add_space_before = true;
            visit_node(_foreach_stmt.in_what);
            if (!in_one_row(_foreach_stmt.stmt))
                add_newline_after = true;
            add_space_before = true;
            bool need_off = !(_foreach_stmt.stmt is statement_list);
            if (need_off)
                IncOffset();
            visit_node(_foreach_stmt.stmt);
            if (need_off)
            DecOffset();
        }

        public override void visit(addressed_value_funcname _addressed_value_funcname)
        {
            throw new NotImplementedException();
        }

        public override void visit(named_type_reference_list _named_type_reference_list)
        {
            for (int i = 0; i < _named_type_reference_list.types.Count; i++)
            {
                if (i > 0)
                    add_space_after = true;
                visit_node(_named_type_reference_list.types[i]);
            }
        }

        public override void visit(template_param_list _template_param_list)
        {
            sb.Append("<");
            for (int i = 0; i < _template_param_list.params_list.Count; i++)
            {
                if (i > 0)
                    add_space_after = true;
                visit_node(_template_param_list.params_list[i]);
            }
        }

        public override void visit(template_type_reference _template_type_reference)
        {
            visit_node(_template_type_reference.name);
            visit_node(_template_type_reference.params_list);
        }

        public override void visit(int64_const _int64_const)
        {
            WriteNode(_int64_const);
        }

        public override void visit(uint64_const _uint64_const)
        {
            WriteNode(_uint64_const);
        }

        public override void visit(new_expr _new_expr)
        {
            sb.Append("new");
            SetKeywordOffset("new");
            visit_node(_new_expr.type);
            if (_new_expr.params_list != null && _new_expr.params_list.expressions[0].source_context != null)
            {
                skip_spaces_after = true;
                visit_node(_new_expr.params_list);
            }
            if (_new_expr.array_init_expr != null)
                visit_node(_new_expr.array_init_expr);
        }

        public override void visit(where_type_specificator_list _type_definition_list)
        {
            for (int i = 0; i < _type_definition_list.defs.Count; i++)
            {
                if (i > 0)
                    add_space_after = true;
                visit_node(_type_definition_list.defs[i]);
            }
        }

        public override void visit(where_definition _where_definition)
        {
            sb.Append("where");
            SetKeywordOffset("where");
            visit_node(_where_definition.names);
            add_space_after = true;
            visit_node(_where_definition.types);
        }

        public override void visit(where_definition_list _where_definition_list)
        {
            foreach (where_definition wd in _where_definition_list.defs)
            {
                visit_node(wd);
            }
        }

        public override void visit(sizeof_operator _sizeof_operator)
        {
            sb.Append("sizeof");
            SetKeywordOffset("sizeof");
            add_space_before = false;
            read_from_beg_pos = true;
            if (_sizeof_operator.expr != null)
                visit_node(_sizeof_operator.expr);
            else if (_sizeof_operator.type_def != null)
                visit_node(_sizeof_operator.type_def);
        }

        public override void visit(typeof_operator _typeof_operator)
        {
            sb.Append("typeof");
            SetKeywordOffset("typeof");
            add_space_before = false;
            read_from_beg_pos = true;
            visit_node(_typeof_operator.type_name);
        }

        public override void visit(compiler_directive _compiler_directive)
        {
            throw new NotImplementedException();
        }

        public override void visit(operator_name_ident _operator_name_ident)
        {
            WriteNode(_operator_name_ident);
        }

        public override void visit(var_statement _var_statement)
        {
            sb.Append("var");
            SetKeywordOffset("var");
            visit_node(_var_statement.var_def);
        }

        public override void visit(question_colon_expression _question_colon_expression)
        {
            visit_node(_question_colon_expression.condition);
            add_space_before = true;
            add_space_after = true;
            visit_node(_question_colon_expression.ret_if_true);
            add_space_before = true;
            add_space_after = true;
            visit_node(_question_colon_expression.ret_if_false);
        }

        public override void visit(expression_as_statement _expression_as_statement)
        {
            visit_node(_expression_as_statement.expr);
        }

        public override void visit(c_scalar_type _c_scalar_type)
        {
            throw new NotImplementedException();
        }

        public override void visit(c_module _c_module)
        {
            throw new NotImplementedException();
        }

        public override void visit(declarations_as_statement _declarations_as_statement)
        {
            throw new NotImplementedException();
        }

        public override void visit(array_size _array_size)
        {
            throw new NotImplementedException();
        }

        public override void visit(enumerator _enumerator)
        {
            visit_node(_enumerator.name);
            if (_enumerator.value != null)
            {
                visit_node(_enumerator.value);
            }
        }

        public override void visit(enumerator_list _enumerator_list)
        {
            for (int i = 0; i < _enumerator_list.enumerators.Count; i++)
            {
                if (i > 0)
                    add_space_after = true;
                visit_node(_enumerator_list.enumerators[i]);
            }
        }

        public override void visit(c_for_cycle _c_for_cycle)
        {
            throw new NotImplementedException();
        }

        public override void visit(switch_stmt _switch_stmt)
        {
            throw new NotImplementedException();
        }

        public override void visit(type_definition_attr_list _type_definition_attr_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(type_definition_attr _type_definition_attr)
        {
            throw new NotImplementedException();
        }

        public override void visit(lock_stmt _lock_stmt)
        {
            sb.Append("lock");
            SetKeywordOffset("lock");
            visit_node(_lock_stmt.lock_object);
            add_newline_after = true;
            add_space_before = true;
            bool need_off = !(_lock_stmt.stmt is statement_list);
            if (!in_one_row(_lock_stmt.stmt))
                add_newline_after = true;
            if (need_off)
                IncOffset();
            visit_node(_lock_stmt.stmt);
            if (need_off)
                DecOffset();
        }

        public override void visit(compiler_directive_list _compiler_directive_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(compiler_directive_if _compiler_directive_if)
        {
            throw new NotImplementedException();
        }

        public override void visit(documentation_comment_list _documentation_comment_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(documentation_comment_tag _documentation_comment_tag)
        {
            throw new NotImplementedException();
        }

        public override void visit(documentation_comment_tag_param _documentation_comment_tag_param)
        {
            throw new NotImplementedException();
        }

        public override void visit(documentation_comment_section _documentation_comment_section)
        {
            throw new NotImplementedException();
        }

        public override void visit(token_taginfo _token_taginfo)
        {
            throw new NotImplementedException();
        }

        public override void visit(declaration_specificator _declaration_specificator)
        {
            sb.Append(_declaration_specificator.name);
        }

        public override void visit(ident_with_templateparams _ident_with_templateparams)
        {
            visit_node(_ident_with_templateparams.name);
            visit_node(_ident_with_templateparams.template_params);
        }

        public override void visit(template_type_name _template_type_name)
        {
            sb.Append(prepare_ident(_template_type_name.name));
            sb.Append("<");
            visit_node(_template_type_name.template_args);
        }

        public override void visit(default_operator _default_operator)
        {
            sb.Append("default");
            SetKeywordOffset("default");
            add_space_before = false;
            read_from_beg_pos = true;
            //sb.Append("(");
            visit_node(_default_operator.type_name);
        }

        public override void visit(bracket_expr _bracket_expr)
        {
            sb.Append("(");
            keyword_offset = 1;
            visit_node(_bracket_expr.expr);
        }

        public override void visit(attribute _attribute)
        {
            if (_attribute.qualifier != null)
                visit_node(_attribute.qualifier);
            visit_node(_attribute.type);
            if (_attribute.arguments != null)
            {
                //sb.Append("(");
                visit_node(_attribute.arguments);
                //sb.Append(")");
            }
        }

        public override void visit(simple_attribute_list _simple_attribute_list)
        {
            sb.Append("[");
            for (int i = 0; i < _simple_attribute_list.attributes.Count; i++)
            {
                visit_node(_simple_attribute_list.attributes[i]);
                if (i < _simple_attribute_list.attributes.Count - 1)
                    sb.Append(",");
            }
        }

        public override void visit(attribute_list _attribute_list)
        {
            foreach (simple_attribute_list sal in _attribute_list.attributes)
                visit_node(sal);
        }

        public override void visit(function_lambda_definition _function_lambda_definition)
        {
            multiline_stack_push(_function_lambda_definition);
            read_from_beg_pos = true;
            if (_function_lambda_definition.formal_parameters != null)
                WritePossibleCommentBefore(_function_lambda_definition.formal_parameters);
            add_space_before = false;
            if (_function_lambda_definition.formal_parameters != null)
            {
                
                is_lambda_parameters = true;
                visit_node(_function_lambda_definition.formal_parameters);
                keyword_offset = 0;
                read_from_beg_pos = false;
            }
            if (_function_lambda_definition.formal_parameters != null && Text[GetPosition(_function_lambda_definition.formal_parameters.source_context.end_position.line_num, _function_lambda_definition.formal_parameters.source_context.end_position.column_num)+1] != ')')
                add_space_before = true;
            
            int tmp_off = off;
            visit_node(_function_lambda_definition.proc_body);
            multiline_stack_pop(_function_lambda_definition);
            off = tmp_off;
        }
        public override void visit(function_lambda_call _function_lambda_call)
        {
            //
        }
        public override void visit(semantic_check _semantic_check)
        {
        }
		
		public override void visit(lambda_inferred_type lit) //lroman//
        {
        }
        public override void visit(same_type_node stn) //SS 22/06/13//
        {
        }
        public override void visit(name_assign_expr _name_assign_expr) // SSM 27.06.13
        {
            multiline_stack_push(_name_assign_expr);
            if (_name_assign_expr.name.source_context.Eq(_name_assign_expr.expr.source_context))
            {
                visit_node(_name_assign_expr.expr);
            }
            else
            {
                visit_node(_name_assign_expr.name);
                //sb.Append(" := ");
                add_space_before = true;
                add_space_after = true;
                visit_node(_name_assign_expr.expr);
            }
            multiline_stack_pop(_name_assign_expr);
        }
        public override void visit(name_assign_expr_list _name_assign_expr_list) // SSM 27.06.13
        {
            if (_name_assign_expr_list.name_expr != null)
            for (int i = 0; i < _name_assign_expr_list.name_expr.Count; i++)
            {
                if (i > 0 && options.SpaceBetweenIdentifiers == 1)
                    add_space_after = true;
                visit_node(_name_assign_expr_list.name_expr[i]);
            }
        }
        public override void visit(unnamed_type_object _unnamed_type_object) // SSM 27.06.13
        {
            //sb.Append("new");
            //SetKeywordOffset("new");
            read_from_beg_pos = true;
            visit_node(_unnamed_type_object.ne_list);
        }
        public override void visit(semantic_type_node stn) // SSM 
        {
        }
        public override void visit(matching_expression _matching_expression)
        {
            visit_node(_matching_expression.left);
            visit_node(_matching_expression.right);
        }
        public override void visit(short_func_definition _short_func_definition)
        {
            multiline_stack_push(_short_func_definition);
            
            add_space_after = true;
            if (_short_func_definition.Parent is class_members && (_short_func_definition.Parent as class_members).access_mod != null && (_short_func_definition.Parent as class_members).access_mod.source_context != null)
                add_space_before = true;
            add_newline_after = false;
            visit_node(_short_func_definition.procdef.proc_header);
            bool tmp_in_procedure = in_procedure;
            in_procedure = true;
            //sb.Append(":=");
            add_space_before = true;
            add_space_after = true;
            visit_node(_short_func_definition.procdef.proc_body);
            //if (in_one_row(_short_func_definition.procdef.proc_body))
                IncOffset();
            in_procedure = tmp_in_procedure;
            multiline_stack_pop(_short_func_definition);
        }

        public override void visit(sequence_type _sequence_type)
        {
            sb.Append("sequence of ");
            visit_node(_sequence_type.elements_type);
        }

        public override void visit(modern_proc_type _modern_proc_type)
        {
            if (_modern_proc_type.aloneparam != null)
            {
                visit_node(_modern_proc_type.aloneparam);
                add_space_before = false;
                add_space_after = false;
                visit_node(_modern_proc_type.res);
            }
            else
            {
                if (_modern_proc_type.el == null && _modern_proc_type.res == null)
                {
                    int beg_pos = GetPosition(_modern_proc_type.source_context.begin_position.line_num, _modern_proc_type.source_context.begin_position.column_num);
                    int end_pos = GetPosition(_modern_proc_type.source_context.end_position.line_num, _modern_proc_type.source_context.end_position.column_num);
                    sb.Append(Text.Substring(beg_pos, end_pos - beg_pos + 1));
                }
                else
                {
                    if (_modern_proc_type.el != null && _modern_proc_type.el.enumerators != null)
                    {
                        sb.Append("(");
                        for (int i = 0; i < _modern_proc_type.el.enumerators.Count; i++)
                        {
                            visit_node(_modern_proc_type.el.enumerators[i]);
                        }
                    }
                    else
                        read_from_beg_pos = true;
                    add_space_before = false;
                    add_space_after = false;
                    visit_node(_modern_proc_type.res);
                }
            }
        }

        public override void visit(tuple_node _tuple_node)
        {
            sb.Append("(");
            visit_node(_tuple_node.el);
        }

        public override void visit(uses_closure uc)
        {
            foreach (var ul in uc.listunitsections)
            {
                visit_node(ul);
            }
        }

        public override void visit(assign_tuple at)
        {
            //multiline_stack_push(at);
            visit_node(at.vars);
            add_space_after = true;
            visit_node(at.expr);
            //multiline_stack_pop(at);
        }

        public override void visit(addressed_value_list al)
        {
            sb.Append("(");
            var i = 0;
            foreach (var av in al.variables)
            {
                visit_node(av);
                add_space_after = true;
                i++;
            }
        }

        public override void visit(yield_node yn)
        {
            sb.Append("yield ");
            visit_node(yn.ex);
        }

        public override void visit(yield_sequence_node yn)
        {
            sb.Append("yield sequence ");
            visit_node(yn.ex);
        }

        public override void visit(assign_var_tuple _assign_var_tuple)
        {
            read_from_beg_pos = true;
            for (int i = 0; i < _assign_var_tuple.idents.idents.Count; i++)
            {
                if (options.SpaceBetweenArguments == 1 && i > 0)
                {
                    add_space_after = true;
                }  
                visit_node(_assign_var_tuple.idents.idents[i]);
            }
            sb.Append(")");
            add_space_after = true;
            add_space_before = true;
            prev_sn = _assign_var_tuple.idents;
            visit_node(_assign_var_tuple.expr);
        }

        public override void visit(slice_expr _slice_expr)
        {
            visit_node(_slice_expr.v);
            if (_slice_expr.from != null)
                visit_node(_slice_expr.from);
            if (_slice_expr.to != null)
                visit_node(_slice_expr.to);
            if (_slice_expr.step != null)
                visit_node(_slice_expr.step);          
        }

        public override void visit(slice_expr_question _slice_expr_question)
        {
            visit_node(_slice_expr_question.v);
            if (_slice_expr_question.from != null)
                visit_node(_slice_expr_question.from);
            if (_slice_expr_question.to != null)
                visit_node(_slice_expr_question.to);
            if (_slice_expr_question.step != null)
                visit_node(_slice_expr_question.step);
        }

        public override void visit(is_pattern_expr _is_pattern_expr)
        {
            if (_is_pattern_expr.left != null)
            {
                add_space_after = true;
                visit_node(_is_pattern_expr.left);
            }
            if (_is_pattern_expr.right != null)
            {
                add_space_before = true;
                visit_node(_is_pattern_expr.right);
            }
        }

        public override void visit(type_pattern _type_pattern)
        {
            if (_type_pattern.type != null)
                visit_node(_type_pattern.type);
            //if (_type_pattern.identifier != null)
            //    visit_node(_type_pattern.identifier);
        }

        public override void visit(match_with _match_with)
        {
            visit_node(_match_with.expr);
            IncOffset();
            add_space_before = true;
            visit_node(_match_with.case_list);
            //add_space_before = false;
            DecOffset();
        }

        public override void visit(pattern_cases _pattern_cases)
        {
            foreach (var patternCase in _pattern_cases.elements)
                visit_node(patternCase);
        }

        public override void visit(pattern_case _pattern_case)
        {
            visit_node(_pattern_case.pattern);

            if (_pattern_case.condition != null)
            {
                add_space_before = true;
                visit_node(_pattern_case.condition);
            }

            add_space_before = true;
            visit_node(_pattern_case.case_action);
        }

        public override void visit(deconstructor_pattern _deconstructor_pattern)
        {
            // TODO Patterns: format
            //visit_node(_deconstructor_pattern.type);
            //foreach (var parameter in _deconstructor_pattern.parameters)
            //{
            //    visit_node(parameter.identifier);
            //    if (parameter.type != null)
            //        visit_node(parameter.type);
            //}
        }

        #endregion
    }
}