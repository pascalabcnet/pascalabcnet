// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Drawing;

namespace VisualPascalABCPlugins
{
    public delegate void PluginGUIItemExecuteDelegate();

    public class PluginGUIItem : IPluginGUIItem
    {
        string text;
        string hint;
        Image image;
        Color imageTransparentColor;
        PluginGUIItemExecuteDelegate executeDelegate;
        System.Windows.Forms.Keys shortcutKeys = System.Windows.Forms.Keys.None;
        string shortcutKeyDisplayString = null;

        public object menuItem { get; set; }
        public object toolStripButton { get; set; }

        public PluginGUIItem(string text, string hint, Image image, Color imageTransparentColor, PluginGUIItemExecuteDelegate executeDelegate)
        {
            this.text = text;
            this.hint = hint;
            this.image = image;
            this.imageTransparentColor = imageTransparentColor;
            this.executeDelegate = executeDelegate;
        }
        public PluginGUIItem(string text, string hint, Image image, Color imageTransparentColor, PluginGUIItemExecuteDelegate executeDelegate, System.Windows.Forms.Keys shortcutKeys, string shortcutKeyDisplayString)
        {
            this.text = text;
            this.hint = hint;
            this.image = image;
            this.imageTransparentColor = imageTransparentColor;
            this.executeDelegate = executeDelegate;
            this.shortcutKeys = shortcutKeys;
            this.shortcutKeyDisplayString = shortcutKeyDisplayString;
        }

        public string Text
        {
            get { return text; }
        }
        public string Hint
        {
            get { return hint; }
            set { hint = value; }
        }
        public Image Image
        {
            get { return image; }
        }
        public Color ImageTransparentColor
        {
            get { return imageTransparentColor; }
        }
        public void Execute()
        {
            executeDelegate();
        }
        public System.Windows.Forms.Keys ShortcutKeys
        {
            get { return shortcutKeys; }
        }
        public string ShortcutKeyDisplayString
        {
            get { return shortcutKeyDisplayString; }
        }
    }
}