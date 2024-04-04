// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using VisualPascalABC;
using PascalABCCompiler;
using System.Text.RegularExpressions;
namespace VisualPascalABC
{
    public class SymbolsViewerSymbol
    {
        SourceLocation sl;
        int imageIndex;
        public SymbolsViewerSymbol(SourceLocation sl, int imageIndex)
        {
            this.sl = sl ;
            this.imageIndex = imageIndex;
        }
        public SourceLocation SourceLocation
        {
            get
            {
                return sl;
            }
        }
        public int ImageIndex
        {
            get
            {
                return imageIndex;
            }
        }
    }
    public class SymbolsViwer
    {
        public string OutputFormat = "{0} - ({1},{2}): {3}";
        ListView listView;
        ImageList imageList;
        public bool showInThread;
        VisualPascalABCPlugins.InvokeDegegate beginInvoke;
        PascalABCCompiler.SourceFilesProviderDelegate sourceFilesProvider;
        VisualEnvironmentCompiler.ExecuteSourceLocationActionDelegate ExecuteSourceLocationAction;
        public SymbolsViwer(ListView listView, ImageList imageList, bool showInThread, VisualPascalABCPlugins.InvokeDegegate beginInvoke,
            PascalABCCompiler.SourceFilesProviderDelegate sourceFilesProvider, VisualEnvironmentCompiler.ExecuteSourceLocationActionDelegate ExecuteSourceLocationAction)
        {
            this.listView = listView;
            this.imageList = imageList;
            this.showInThread = showInThread;
            this.beginInvoke = beginInvoke;
            this.sourceFilesProvider = sourceFilesProvider;
            this.ExecuteSourceLocationAction = ExecuteSourceLocationAction;
            listView.MouseDoubleClick += new MouseEventHandler(listView_MouseDoubleClick);
        }

        void listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                SymbolsViewerSymbol sym = listView.SelectedItems[0].Tag as SymbolsViewerSymbol;
                if(sym.SourceLocation!=null)
                    ExecuteSourceLocationAction(sym.SourceLocation, VisualPascalABCPlugins.SourceLocationAction.GotoBeg);
            }
        }

        string GetTextLineFromNumber(int lineNum, string Text)
        {
            int i=0,b=0,e=-1;
            do
            {
                b = e + 1;
                e = Text.IndexOf('\n', b);
                if (i == lineNum)
                    if (e >= 0)
                        return Text.Substring(b, e - b - 1);
                    else
                        return Text.Substring(b, Text.Length - b - 1);
                i++;
            }
            while (e != -1);
            /*MatchCollection mc = Regex.Matches(Text, ".*\n", RegexOptions.Compiled);
            if (lineNum < mc.Count)
                return mc[lineNum].Value.Replace(Environment.NewLine, "");*/
            return "";
        }

        List<SymbolsViewerSymbol> symbolList;
        delegate void lviAddDelegate(List<ListViewItem> ListViewItems);
        void add_items_to_list(List<ListViewItem> ListViewItems)
        {
            listView.Items.AddRange(ListViewItems.ToArray());
        }
        void show(object state)
        {
            List<ListViewItem> ListViewItems = new List<ListViewItem>();
            foreach (SymbolsViewerSymbol sym in symbolList)
            {
                string text = (string)sourceFilesProvider(sym.SourceLocation.FileName, PascalABCCompiler.SourceFileOperation.GetText);
                if (text == null)
                    continue;
                string txt = string.Format(OutputFormat, sym.SourceLocation.FileName, sym.SourceLocation.BeginPosition.Line, sym.SourceLocation.BeginPosition.Column, GetTextLineFromNumber(sym.SourceLocation.BeginPosition.Line - 1, text));
                txt = txt.Replace('\t', ' ');
                ListViewItem lvi = new ListViewItem();
                lvi.Tag = sym;
                lvi.ImageIndex = sym.ImageIndex;
                lvi.SubItems.Add(txt);
                ListViewItems.Add(lvi);
            }
            beginInvoke(new lviAddDelegate(add_items_to_list), ListViewItems);
            if (listView.Items.Count > 0)
                listView.Items[0].Selected = true;
            symbolList = null;
        }
        
        delegate void Invoke_with_param(object obj);
        	
        private void SetSmallImageListInternal(object obj)
        {
        	listView.SmallImageList = obj as ImageList;
        }
        
        public void Show(List<SymbolsViewerSymbol> symbolList)
        {
            this.symbolList = symbolList;
            listView.Items.Clear();
            //listView.SmallImageList = imageList;
            listView.Invoke(new Invoke_with_param(SetSmallImageListInternal),imageList);
            if (showInThread)
            {
                if (!System.Threading.ThreadPool.QueueUserWorkItem(show))
                    show(null);
            }
            else
                show(null);
        }
        public void Clear()
        {
            listView.Clear();
        }

    }
}
