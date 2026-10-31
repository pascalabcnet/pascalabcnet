using System.Drawing;
using System.Windows.Forms;

namespace VisualPascalABC
{
    // Shared LightPT color protocol. Called on the UI thread by each shell's
    // existing output callback; it does not activate windows or move focus.
    internal static class ColoredOutput
    {
        private static Color GetColor(char code)
        {
            switch ((int)code)
            {
                case 65535: return Color.Green;
                case 65534: return Color.Red;
                case 65533: return Color.OrangeRed;
                case 65532: return Color.Magenta;
                case 65531: return Color.Gray;
                default: return Color.Black; // 65530 resets the color.
            }
        }

        internal static void Append(TextBoxBase textBox, string text)
        {
            var richTextBox = textBox as RichTextBox;
            int start = 0;
            for (int index = 0; index < text.Length; index++)
            {
                if (text[index] < (char)65530)
                    continue;
                if (index > start)
                    textBox.AppendText(text.Substring(start, index - start));
                if (richTextBox != null)
                {
                    richTextBox.Select(richTextBox.TextLength, 0);
                    richTextBox.SelectionColor = GetColor(text[index]);
                }
                start = index + 1;
            }
            if (start < text.Length)
                textBox.AppendText(text.Substring(start));
        }
    }
}
