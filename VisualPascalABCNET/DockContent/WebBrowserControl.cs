// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace VisualPascalABC
{
    public partial class WebBrowserControl : DockContent
    {
        public WebBrowserControl()
        {
            InitializeComponent();
        }

        public void OpenPage(string title, string url)
        {
            this.TabText = title;
            webBrowser1.Navigate(new Uri(url));
        }

        private void WebBrowserControl_Activated(object sender, EventArgs e)
        {
            VisualPABCSingleton.MainForm.BottomTabsVisible = false;
            VisualPABCSingleton.MainForm.BrowserTabSelected = true;
            VisualPABCSingleton.MainForm.CurrentWebBrowserControl = this;
            VisualPABCSingleton.MainForm.LastSelectedTab = VisualPABCSingleton.MainForm.CurrentCodeFileDocument;
        }

        private void WebBrowserControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            VisualPABCSingleton.MainForm.CloseBrowserTab(this.TabText);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            this.tbUrl.Text = this.webBrowser1.Url.AbsoluteUri;
            
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.webBrowser1.Document.Focus();
            var footer = this.webBrowser1.Document.GetElementById("ux-footer");
            if (footer != null)
                footer.Style += ";display:none;";
        }

        private void tbUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string url = this.tbUrl.Text;
                if (url != "")
                {
                    if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                        url = "http://" + url;
                    try
                    { 
                        this.webBrowser1.Navigate(new Uri(url));
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (this.webBrowser1.CanGoBack)
                this.webBrowser1.GoBack();
        }
    }
}
