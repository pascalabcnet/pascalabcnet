using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VeryBasicParserYacc;

namespace VeryBasicParser
{
    // Статический класс, определяющий ключевые слова языка
    public static class Keywords
    {
        private static Dictionary<string, int> keywords = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);

        public static Dictionary<string, string> keymap = new Dictionary<string, string>();

        public static string fname = "keywordsmap.vbl";
        public static void ReloadKeyMap()
        {
            try
            {
                if (keymap != null)
                {
                    keymap.Clear();
                    keymap = null;
                }
                var fn = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName), fname);
                if (System.IO.File.Exists(fn))
                    keymap = System.IO.File.ReadLines(fn, Encoding.Unicode).Select(s => s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToDictionary(w => w[0], w => w[1]);
            }
            catch (Exception e)  // погасить любые исключения
            {
                //var w = e.Message;
            }
        }
        public static string Convert(string s)
        {
            if (keymap == null || keymap.Count() == 0)
                return s;
            else if (!keymap.ContainsKey(s))
                return s;
            else return keymap[s];
        }

        public static void KeywordsAdd()
        {
            if (keymap == null || keymap.Count() == 0)
                ReloadKeyMap();
            keywords.Clear();
            keywords.Add(Convert("if"), (int)Tokens.IF);
            keywords.Add(Convert("elif"), (int)Tokens.ELIF);
            keywords.Add(Convert("else"), (int)Tokens.ELSE);
            keywords.Add(Convert("while"), (int)Tokens.WHILE);
            keywords.Add(Convert("for"), (int)Tokens.FOR);
            keywords.Add(Convert("in"), (int)Tokens.IN);
            keywords.Add(Convert("def"), (int)Tokens.DEF);
            keywords.Add(Convert("return"), (int)Tokens.RETURN);
            keywords.Add(Convert("break"), (int)Tokens.BREAK);
            keywords.Add(Convert("continue"), (int)Tokens.CONTINUE);
        }

        static Keywords()
        {
            KeywordsAdd();
        }

        public static int KeywordOrIDToken(string s)
        {
            //s = s.ToUpper();
            int keyword = 0;
            if (keywords.TryGetValue(s, out keyword))
                return keyword;
            else
                return (int)Tokens.ID;
        }
    }

}
