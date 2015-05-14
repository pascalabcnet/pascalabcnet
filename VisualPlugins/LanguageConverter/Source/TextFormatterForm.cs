using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VisualPascalABCPlugins;

namespace Converter
{
    public partial class TextFormatterForm : Form
    {
        public TextFormatter TextFormatter;
        internal LanguageConverter_VisualPascalABCPlugin Plugin;
        private Dictionary<string, string> indentsBlockBegin;
        private Dictionary<string, string> indentsBlockEnd;
        private Dictionary<string, string> indentsBodyBlock;
        private Dictionary<string, string> indentsBetweenWords;
        public TextFormatterForm(TextFormatter _textFormatter)
        {
            InitializeComponent();

            TextFormatter = _textFormatter;
            initIndents();
        }

        public TextFormatterForm()
        {
            InitializeComponent();

            //TextFormatter = new TextFormatter();
            initIndents();            
        }        

        private void initIndents()
        {
            indentsBlockBegin = new Dictionary<string, string>();
            indentsBlockBegin.Add(comboBoxBlockBegin.Items[0].ToString(), Environment.NewLine);
            indentsBlockBegin.Add(comboBoxBlockBegin.Items[1].ToString(), "");

            indentsBlockEnd = new Dictionary<string, string>();
            indentsBlockEnd.Add(comboBoxBlockEnd.Items[0].ToString(), Environment.NewLine);
            indentsBlockEnd.Add(comboBoxBlockEnd.Items[1].ToString(), "");

            indentsBodyBlock = new Dictionary<string, string>();            
            indentsBodyBlock.Add(comboBoxBodyBlock.Items[0].ToString(), " ");

            indentsBetweenWords = new Dictionary<string, string>();
            indentsBetweenWords.Add(comboBoxBetweenWords.Items[0].ToString(), " "); 

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void labelBlockBody_Click(object sender, EventArgs e)
        {

        }

        private void TextFormatterForm_Shown(object sender, EventArgs e)
        {
            
        }

        public void SaveTextFormater(TextFormatter _TextFormatter)
        {
            _TextFormatter.Indents.BlockBegin = indentsBlockBegin[comboBoxBlockBegin.SelectedItem.ToString()];
            _TextFormatter.Indents.BlockEnd = indentsBlockEnd[comboBoxBlockEnd.SelectedItem.ToString()];
            StringBuilder bodyBlock = new StringBuilder();
            for (int i = 0; i < numericUpDown1.Value; i++)
                bodyBlock.Append(indentsBodyBlock[comboBoxBodyBlock.SelectedItem.ToString()]);
            _TextFormatter.Indents.BlockBody = bodyBlock.ToString();
            _TextFormatter.Indents.BetweenWords = indentsBetweenWords[comboBoxBetweenWords.SelectedItem.ToString()];
        }


        private void buttonOk_Click(object sender, EventArgs e)
        {
            SaveTextFormater(Plugin.Languages[cbLanguage.SelectedIndex].SourceTextBuilder.TextFormatter);            
            this.Close();
        }

        private void cbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            Plugin.currentLanguage = Plugin.Languages[cbLanguage.SelectedIndex];
        }

        internal void Init()
        {
            cbLanguage.Items.Clear();
            foreach (ISemanticNodeConverter snc in Plugin.Languages)
            {
                cbLanguage.Items.Add(snc.ToString());
            }
            comboBoxBlockBegin.SelectedItem = comboBoxBlockBegin.Items[0];
            comboBoxBlockEnd.SelectedItem = comboBoxBlockEnd.Items[0];
            comboBoxBodyBlock.SelectedItem = comboBoxBodyBlock.Items[0];
            comboBoxBetweenWords.SelectedItem = comboBoxBetweenWords.Items[0];
            cbLanguage.SelectedIndex = Plugin.Languages.IndexOf(Plugin.currentLanguage);
        }

        private void TextFormatterForm_Load(object sender, EventArgs e)
        {
        }
    }
}