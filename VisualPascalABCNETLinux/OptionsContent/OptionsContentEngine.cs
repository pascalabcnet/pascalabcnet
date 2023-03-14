// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace VisualPascalABC.OptionsContent
{
    public class OptionsContentEngine
    {
        List<IOptionsContent> contentList = new List<IOptionsContent>();
        List<IOptionsContent> showedContent = new List<IOptionsContent>();
        public List<IOptionsContent> ContentList
        {
            get
            {
                return contentList;
            }
        }
        OptionsForm optionsWindow;
        Form ownerForm;
        public OptionsContentEngine(Form ownerForm)
        {
            this.ownerForm = ownerForm;
        }
        public void AddContent(IOptionsContent content)
        {
            contentList.Add(content);
        }
        public void ShowDialog()
        {
            if (optionsWindow == null)
            {
                optionsWindow = new OptionsForm(this);
                optionsWindow.Owner = ownerForm;
            }
            optionsWindow.ShowDialog();
        }
        public void UpdateOptionsForm()
        {
            if (optionsWindow != null)
                optionsWindow.UpdateOptionsForm();
        }
        internal void Action(IOptionsContent content, OptionsContentAction action)
        {
            content.Action(action);
            if (action == OptionsContentAction.Show)
                showedContent.Add(content);
        }
        internal void Action(OptionsContentAction action)
        {
            foreach (IOptionsContent c in showedContent)
                c.Action(action);
            if (action == OptionsContentAction.Ok || action == OptionsContentAction.Cancel)
                showedContent.Clear();
        }

    }
}
