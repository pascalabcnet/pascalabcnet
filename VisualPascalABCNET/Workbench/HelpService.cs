// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using VisualPascalABCPlugins;
using System.Threading;
using PascalABCCompiler.Parsers;
using System.Text;

namespace VisualPascalABC
{
    partial class Form1 : IMessageFilter, IHelpService
    {
        private Dictionary<int, string> HelpPrograms;
        string HelpFileName = null;
        string HelpExamplesFileName = null;
        string HelpExamplesMapFileName = null;
        string HelpExamplesDirectory = null;
        string HelpTutorialExamplesDirectory = null;
        private string DotNetHelpFileName = null;
        private CodeSampleManager sampleManager;

        private void OpenHelpProgram(string name, bool compileandexecute)
        {
            VisualPascalABC.VisualPascalABCProgram.MainForm.Activate();
            ExecuteVisualEnvironmentCompilerAction(VisualEnvironmentCompilerAction.OpenFile, name);
            if (compileandexecute)
                ExecuteVisualEnvironmentCompilerAction(VisualEnvironmentCompilerAction.Run, null);
        }

        private void InitHelpProgramsDictionary() // Èíèöèàëèçàöèÿ êàðòû ïðîãðàìì, ïðåäíàçíà÷åííûõ äëÿ çàïóñêà èç ñïðàâêè
        {
            HelpPrograms = new Dictionary<int, string>();
            try
            {
                using (StreamReader sr = new StreamReader(HelpExamplesMapFileName))
                {
                    string s;
                    while (!sr.EndOfStream)
                    {
                        s = sr.ReadLine();
                        int p = s.IndexOf('=');
                        int num;
                        if (p >= 0 && int.TryParse(s.Substring(0, p), out num))
                        {
                            string name = s.Substring(p + 1);
                            HelpPrograms[num] = name;
                        }
                    }
                }

            }
            catch { }
            sampleManager = new CodeSampleManager();
        }

        public void OpenMSDN()
        {
            AddTabWithUrl(MainDockPanel, PascalABCCompiler.StringResources.Get("VP_MF_M_DOTNET_HELP"), "http://msdn.microsoft.com/ru-ru/library/w0x726c2%28v=vs.100%29.aspx");
        }

        private string getMSDNUrlByName(string s, Position pos)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("http://msdn.microsoft.com/ru-ru/library/");
            string title = pos.full_metadata_title;
            if (pos.full_metadata_title.IndexOf('`') != -1 && pos.full_metadata_title.IndexOf('.') != -1)
                title = pos.full_metadata_title.Substring(0, pos.full_metadata_title.LastIndexOf('.'));
            sb.Append(title);
            sb.Append("(v=vs.100).aspx");
            return sb.ToString();
        }

        private bool isDotNetItem(string s, ref Position position)
        {
            /*if (!File.Exists(DotNetHelpFileName))
                return false;*/
            switch (s.ToLower())
            {
                case "integer":
                case "real":
                case "byte":
                case "shortint":
                case "smallint":
                case "boolean":
                case "single":
                case "word":
                case "longword":
                case "uint64":
                case "longint":
                case "int64":
                case "char":
                case "string":
                    return false;
            }
            if (CodeCompletion.CodeCompletionController.CurrentParser == null) return false;
            List<Position> poses = CodeCompletionActionsManager.GetDefinitionPosition(CurrentSyntaxEditor.TextEditor.ActiveTextAreaControl.TextArea, true);
            if (poses == null || poses.Count == 0) return false;
            foreach (Position pos in poses)
                if (pos.from_metadata)
                {
                    position = pos;
                    return true;
                }
            return false;
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x0052) // îáðàáîòêà ñîîáùåíèÿ WM_TCARD (äëÿ âûçîâà äåéñòâèé ïî ãèïåðññûëêàì â Helpå (ÑÑÌ 1.04.08)
            {
                if (HelpPrograms == null)
                    return false;
                string name = PascalABCCompiler.Tools.ReplaceAllKeys(Constants.WorkingDirectoryIdent, WorkbenchStorage.StandartDirectories);
                name += "\\Samples\\";
                if (HelpPrograms.ContainsKey((int)m.WParam))
                {
                    name += HelpPrograms[(int)m.WParam];
                    try
                    {
                        OpenHelpProgram(name, true);
                    }
                    catch { }
                }
            }
            return false;
        }

        public void ExecShowHelpF1()
        {
            if (checkForDotNetItem())
                __showDotNetHelp();
            else if (!ThreadPool.QueueUserWorkItem(__showhelpf1))
                __showhelpf1(null);
        }

        bool checkForDotNetItem()
        {
            ICSharpCode.TextEditor.TextLocation cp = CurrentSyntaxEditor.TextEditor.ActiveTextAreaControl.Caret.Position;
            ICSharpCode.TextEditor.Document.TextWord tw = CurrentSyntaxEditor.TextEditor.ActiveTextAreaControl.TextArea.Document.GetLineSegment(cp.Line).GetWord(cp.Column);

            if (tw != null)
            {
                Position pos = default(Position);
                if (isDotNetItem(tw.Word, ref pos))
                {
                    return true;
                }
            }
            return false;
        }


        void __showDotNetHelp()
        {
            ICSharpCode.TextEditor.TextLocation cp = CurrentSyntaxEditor.TextEditor.ActiveTextAreaControl.Caret.Position;
            ICSharpCode.TextEditor.Document.TextWord tw = CurrentSyntaxEditor.TextEditor.ActiveTextAreaControl.TextArea.Document.GetLineSegment(cp.Line).GetWord(cp.Column);

            if (tw != null)
            {
                Position pos = default(Position);
                if (isDotNetItem(tw.Word, ref pos))
                {
                    if (pos.full_metadata_title != null)
                    {
                        AddTabWithUrl(MainDockPanel, pos.full_metadata_title, getMSDNUrlByName(tw.Word, pos));
                    }
                }
            }
        }

        void __showhelpf1(object state)
        {
            ICSharpCode.TextEditor.TextLocation cp = CurrentSyntaxEditor.TextEditor.ActiveTextAreaControl.Caret.Position;
            ICSharpCode.TextEditor.Document.TextWord tw = CurrentSyntaxEditor.TextEditor.ActiveTextAreaControl.TextArea.Document.GetLineSegment(cp.Line).GetWord(cp.Column);

            if (tw != null)
            {
                MetadataType metadata_type = MetadataType.Class;
                /*if (isDotNetItem(tw.Word, ref metadata_type))
                {
                    //Help.ShowHelp(this, DotNetHelpFileName, HelpNavigator.KeywordIndex);
                    //Help.ShowHelp(this, DotNetHelpFileName, HelpNavigator.KeywordIndex, tw.Word);
                    //Help.ShowHelp(this, DotNetHelpFileName);
                    //Help.ShowHelp(this, DotNetHelpFileName, HelpNavigator.KeywordIndex);
                    //Help.ShowHelpIndex(this, DotNetHelpFileName);
                    //Help.ShowHelp(this, DotNetHelpFileName,HelpNavigator.KeywordIndex, tw.Word);
                    AddTabWithUrl(MainDockPanel, tw.Word, getMSDNUrlByName(tw.Word));
                    Help.ShowHelp(this, DotNetHelpFileName, HelpNavigator.TableOfContents);

                    switch (metadata_type)
                    {
                        case MetadataType.Class: Help.ShowHelp(this, DotNetHelpFileName, HelpNavigator.KeywordIndex, tw.Word + " êëàññ"); break;
                        case MetadataType.Constructor: Help.ShowHelp(this, DotNetHelpFileName, HelpNavigator.KeywordIndex, tw.Word + "." + tw.Word + " êîíñòðóêòîðû"); break;
                        case MetadataType.Delegate: Help.ShowHelp(this, DotNetHelpFileName, HelpNavigator.KeywordIndex, tw.Word + " äåëåãàò"); break;
                        case MetadataType.Enumeration: Help.ShowHelp(this, DotNetHelpFileName, HelpNavigator.KeywordIndex, tw.Word + " ïåðå÷èñëåíèå"); break;
                        case MetadataType.EnumerationMember: Help.ShowHelp(this, DotNetHelpFileName, HelpNavigator.KeywordIndex, tw.Word + " ýëåìåíò ïåðå÷èñëåíèÿ"); break;
                        case MetadataType.Event: Help.ShowHelp(this, DotNetHelpFileName, HelpNavigator.KeywordIndex, tw.Word + " ñîáûòèå"); break;
                        case MetadataType.Field: Help.ShowHelp(this, DotNetHelpFileName, HelpNavigator.KeywordIndex, tw.Word + " ïîëå"); break;
                        case MetadataType.Interface: Help.ShowHelp(this, DotNetHelpFileName, HelpNavigator.KeywordIndex, tw.Word + " èíòåðôåéñ"); break;
                        case MetadataType.Method: Help.ShowHelp(this, DotNetHelpFileName, HelpNavigator.KeywordIndex, tw.Word + " ìåòîä"); break;
                        case MetadataType.Property: Help.ShowHelp(this, DotNetHelpFileName, HelpNavigator.KeywordIndex, tw.Word + " ñâîéñòâî"); break;
                        case MetadataType.Struct: Help.ShowHelp(this, DotNetHelpFileName, HelpNavigator.KeywordIndex, tw.Word + " çàïèñü"); break;
                    }

                }
                else*/
                {
                    Help.ShowHelp(this, HelpFileName, HelpNavigator.TableOfContents);
                    Help.ShowHelp(this, HelpFileName, HelpNavigator.KeywordIndex, tw.Word);
                }
            }
            else
            {
                Help.ShowHelp(this, HelpFileName, HelpNavigator.KeywordIndex, "PascalABC"); // ôèêòèâíûé âûçîâ. Íóæåí ÷òîáû îêíî ñïðàâêè íå ïîêàçûâàëîñü Always on Top
                Help.ShowHelp(this, HelpFileName);
            }
        }

        void __showgettingstarted(object state)
        {
            Help.ShowHelp(this, HelpFileName, HelpNavigator.Topic, @"Common\features.html"); //this íå ïåðåïðàâëÿòü íà null - ñîîáùèòü ìíå ïðè îøèáêå - ÑÑÌ
        }

        void __showhelp(object state)
        {
            Help.ShowHelp(null, HelpFileName);//zdes dolzhen stojat null, inache gluki pri perekluchenii okon
        }

        void __showdotnethelp(object state)
        {
            Help.ShowHelp(null, DotNetHelpFileName);
        }

        void __showhelpinqueue()
        {
            if (!ThreadPool.QueueUserWorkItem(__showhelp))
                __showhelp(null);
        }
    }
}

