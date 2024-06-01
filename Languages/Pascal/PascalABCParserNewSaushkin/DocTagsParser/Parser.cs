// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Parsers;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PascalABCCompiler;


namespace Languages.Pascal.Frontend.Documentation
{

    public class PascalDocTagsLanguageParser : IDocParser
    {
        List<string> sectionNames = new List<string>();

        public PascalDocTagsLanguageParser()
        {
            filesExtensions = new string[1];
            filesExtensions[0] = ".pasdt" + StringConstants.hideParserExtensionPostfixChar;
            sectionNames.Add("summary");
            sectionNames.Add("returns");
        }

        string[] filesExtensions;
        public string[] FilesExtensions
        {
            get
            {
                return filesExtensions;
            }
        }

        documentation_comment_section parse_section(string text)
        {
            documentation_comment_section dcs = new documentation_comment_section();
            if (text.StartsWith("////"))
                return dcs;
            text = Regex.Replace(text, @"\r\n(\s)*///", "\r\n", RegexOptions.Compiled);
            text = Regex.Replace(text, @"(\s)*///", "", RegexOptions.Compiled);
            /*foreach (string section_name in sectionNames)
            {
                string pattern = "<" + section_name + @"(\w+='\w+')*>" + "(.|\r\n)*" + "</" + section_name + ">";
                MatchCollection mc = Regex.Matches(text, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                foreach (Match m in mc)
                {
                    documentation_comment_tag dt = new documentation_comment_tag();
                    dt.name = section_name;
                    string res = m.Value;
                    int i = res.IndexOf('>');
                    dt.text = res.Substring(i + 1, res.IndexOf("</" + section_name) - i - 1);
                    dcs.tags.Add(dt);
                }
            }*/
            dcs.text = text;
            return dcs;
        }

        public documentation_comment_list BuildTree(string Text)
        {
            MatchCollection mc = Regex.Matches(Text, @"(([\f\t\v\x85\p{Z}])*///.*\r\n)*([\f\t\v\x85\p{Z}])*///.*", RegexOptions.Compiled);
            documentation_comment_list dcl = new documentation_comment_list();
            if (mc.Count > 0)
            {
                int i = 0;
                int mci = 0, curmindex = mc[0].Index;
                int line_num = 1;
                int col = 1;
                documentation_comment_section dcs = null;
                int dcs_count = 0;
                int dcs_length = 0;
                while (true)
                {
                    if (Text[i] == '\n')
                    {
                        line_num++;
                    }
                    if (dcs != null && dcs_count == dcs_length)
                    {
                        dcs.source_context = new SourceContext(dcs.source_context.begin_position.line_num, dcs.source_context.begin_position.column_num, line_num - 1, col);
                        dcs = null;
                    }
                    if (Text[i] == '\n')
                    {
                        col = 0;
                    }
                    if (curmindex == i)
                    {
                        dcs = parse_section(mc[mci].Value);
                        if (dcs.tags.Count > 0 || dcs.text != null)
                        {
                            dcs.source_context = new SourceContext(line_num, col, -1, -1);
                            dcl.sections.Add(dcs);
                            dcs_count = 0;
                            dcs_length = mc[mci].Length;
                        }
                        mci++;
                        if (mci < mc.Count)
                            curmindex = mc[mci].Index;
                        else
                            curmindex = -1;
                    }
                    i++;
                    col++;
                    if (dcs != null)
                        dcs_count++;
                    if (i == Text.Length || (curmindex == -1 && dcs == null))
                        break;
                }
            }
            return dcl;
        }

        public string Name
        {
            get
            {
                return "DocTags";
            }
        }
        public string Version
        {
            get
            {
                return "0.9";
            }
        }

        public override string ToString()
        {
            return "Documentation Comments Tag Parser v" + Version;
        }

    }
}
