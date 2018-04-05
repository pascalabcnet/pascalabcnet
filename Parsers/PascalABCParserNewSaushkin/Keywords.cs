// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPPGParserScanner
{
    // Статический класс, определяющий ключевые слова языка
    public static class Keywords
    {
        private static Dictionary<string, int> keywords = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);

        public static Dictionary<string, string> keymap = new Dictionary<string, string>();

        public static string fname = "keywordsmap.pabc";
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
                    keymap = System.IO.File.ReadLines(fn,Encoding.Unicode).Select(s=>s.Split(new char[] { ' ' },StringSplitOptions.RemoveEmptyEntries)).ToDictionary(w => w[0], w => w[1]);
            }
            catch(Exception e)  // погасить любые исключения
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
            keywords.Add(Convert("or"), (int)Tokens.tkOr);
            keywords.Add(Convert("xor"), (int)Tokens.tkXor);
            keywords.Add(Convert("and"), (int)Tokens.tkAnd);
            keywords.Add(Convert("div"), (int)Tokens.tkDiv);
            keywords.Add(Convert("mod"), (int)Tokens.tkMod);
            keywords.Add(Convert("shl"), (int)Tokens.tkShl);
            keywords.Add(Convert("shr"), (int)Tokens.tkShr);
            keywords.Add(Convert("not"), (int)Tokens.tkNot);
            keywords.Add(Convert("as"), (int)Tokens.tkAs);
            keywords.Add(Convert("in"), (int)Tokens.tkIn);
            keywords.Add(Convert("is"), (int)Tokens.tkIs);
            keywords.Add(Convert("implicit"), (int)Tokens.tkImplicit);
            keywords.Add(Convert("explicit"), (int)Tokens.tkExplicit);
            keywords.Add(Convert("sizeof"), (int)Tokens.tkSizeOf);
            keywords.Add(Convert("typeof"), (int)Tokens.tkTypeOf);
            keywords.Add(Convert("where"), (int)Tokens.tkWhere);
            keywords.Add(Convert("array"), (int)Tokens.tkArray);
            keywords.Add(Convert("begin"), (int)Tokens.tkBegin);
            keywords.Add(Convert("case"), (int)Tokens.tkCase);
            keywords.Add(Convert("class"), (int)Tokens.tkClass);
            keywords.Add(Convert("const"), (int)Tokens.tkConst);
            keywords.Add(Convert("constructor"), (int)Tokens.tkConstructor);
            keywords.Add(Convert("default"), (int)Tokens.tkDefault);
            keywords.Add(Convert("destructor"), (int)Tokens.tkDestructor);
            keywords.Add(Convert("downto"), (int)Tokens.tkDownto);
            keywords.Add(Convert("do"), (int)Tokens.tkDo);
            keywords.Add(Convert("else"), (int)Tokens.tkElse);
            keywords.Add(Convert("end"), (int)Tokens.tkEnd);
            keywords.Add(Convert("event"), (int)Tokens.tkEvent);
            keywords.Add(Convert("except"), (int)Tokens.tkExcept);
            keywords.Add(Convert("exports"), (int)Tokens.tkExports);
            keywords.Add(Convert("file"), (int)Tokens.tkFile);
            keywords.Add(Convert("finalization"), (int)Tokens.tkFinalization);
            keywords.Add(Convert("finally"), (int)Tokens.tkFinally);
            keywords.Add(Convert("for"), (int)Tokens.tkFor);
            keywords.Add(Convert("foreach"), (int)Tokens.tkForeach);
            keywords.Add(Convert("function"), (int)Tokens.tkFunction);
            keywords.Add(Convert("goto"), (int)Tokens.tkGoto);
            keywords.Add(Convert("if"), (int)Tokens.tkIf);
            keywords.Add(Convert("implementation"), (int)Tokens.tkImplementation);
            keywords.Add(Convert("inherited"), (int)Tokens.tkInherited);
            keywords.Add(Convert("initialization"), (int)Tokens.tkInitialization);
            keywords.Add(Convert("interface"), (int)Tokens.tkInterface);
            keywords.Add(Convert("label"), (int)Tokens.tkLabel);
            keywords.Add(Convert("lock"), (int)Tokens.tkLock);
            keywords.Add(Convert("loop"), (int)Tokens.tkLoop);
            keywords.Add(Convert("nil"), (int)Tokens.tkNil);
            keywords.Add(Convert("procedure"), (int)Tokens.tkProcedure);
            keywords.Add(Convert("of"), (int)Tokens.tkOf);
            keywords.Add(Convert("operator"), (int)Tokens.tkOperator);
            keywords.Add(Convert("property"), (int)Tokens.tkProperty);
            keywords.Add(Convert("raise"), (int)Tokens.tkRaise);
            keywords.Add(Convert("record"), (int)Tokens.tkRecord);
            keywords.Add(Convert("repeat"), (int)Tokens.tkRepeat);
            keywords.Add(Convert("set"), (int)Tokens.tkSet);
            keywords.Add(Convert("try"), (int)Tokens.tkTry);
            keywords.Add(Convert("type"), (int)Tokens.tkType);
            keywords.Add(Convert("then"), (int)Tokens.tkThen);
            keywords.Add(Convert("to"), (int)Tokens.tkTo);
            keywords.Add(Convert("until"), (int)Tokens.tkUntil);
            keywords.Add(Convert("uses"), (int)Tokens.tkUses);
            keywords.Add(Convert("var"), (int)Tokens.tkVar);
            keywords.Add(Convert("while"), (int)Tokens.tkWhile);
            keywords.Add(Convert("with"), (int)Tokens.tkWith);
            keywords.Add(Convert("program"), (int)Tokens.tkProgram);
            keywords.Add(Convert("template"), (int)Tokens.tkTemplate);
            keywords.Add(Convert("packed"), (int)Tokens.tkPacked);
            keywords.Add(Convert("resourcestring"), (int)Tokens.tkResourceString);
            keywords.Add(Convert("threadvar"), (int)Tokens.tkThreadvar);
            keywords.Add(Convert("sealed"), (int)Tokens.tkSealed);
            keywords.Add(Convert("partial"), (int)Tokens.tkPartial);
            keywords.Add(Convert("params"), (int)Tokens.tkParams);
            keywords.Add(Convert("unit"), (int)Tokens.tkUnit);
            keywords.Add(Convert("library"), (int)Tokens.tkLibrary);
            keywords.Add(Convert("external"), (int)Tokens.tkExternal);
            keywords.Add(Convert("name"), (int)Tokens.tkName);
            keywords.Add(Convert("private"), (int)Tokens.tkPrivate);
            keywords.Add(Convert("protected"), (int)Tokens.tkProtected);
            keywords.Add(Convert("public"), (int)Tokens.tkPublic);
            keywords.Add(Convert("internal"), (int)Tokens.tkInternal);
            keywords.Add(Convert("read"), (int)Tokens.tkRead);
            keywords.Add(Convert("write"), (int)Tokens.tkWrite);
            keywords.Add(Convert("on"), (int)Tokens.tkOn);
            keywords.Add(Convert("forward"), (int)Tokens.tkForward);
            keywords.Add(Convert("abstract"), (int)Tokens.tkAbstract);
            keywords.Add(Convert("overload"), (int)Tokens.tkOverload);
            keywords.Add(Convert("reintroduce"), (int)Tokens.tkReintroduce);
            keywords.Add(Convert("override"), (int)Tokens.tkOverride);
            keywords.Add(Convert("virtual"), (int)Tokens.tkVirtual);
            keywords.Add(Convert("extensionmethod"), (int)Tokens.tkExtensionMethod);
            keywords.Add(Convert("new"), (int)Tokens.tkNew);
            keywords.Add(Convert("auto"), (int)Tokens.tkAuto);
            keywords.Add(Convert("sequence"), (int)Tokens.tkSequence);
            keywords.Add(Convert("yield"), (int)Tokens.tkYield);
            keywords.Add(Convert("namespace"), (int)Tokens.tkNamespace);
            keywords.Add(Convert("typeclass"), (int)Tokens.tkTypeclass);
            keywords.Add(Convert("instance"), (int)Tokens.tkInstance);
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
                return (int)Tokens.tkIdentifier;
        }
    }

}
