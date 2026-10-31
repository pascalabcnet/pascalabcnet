using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows.Forms;

internal static class OutputColorTests
{
    internal static void Run(Assembly ide)
    {
        var type = ide.GetType("VisualPascalABC.Form1", true);
        var host = FormatterServices.GetUninitializedObject(type);
        GC.SuppressFinalize(host);
        var append = type.GetMethod("AppendTextInternalAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        string input = "A\uffffG\ufffeR\ufffdO\ufffcM\ufffbS\ufffaB!";
        Color[] colors = { Color.Black, Color.Green, Color.Red, Color.OrangeRed,
            Color.Magenta, Color.Gray, Color.Black, Color.Black };
        for (int split = 0; split <= input.Length; split++)
        {
            using (var box = new RichTextBox())
            {
                box.CreateControl();
                append.Invoke(host, new object[] { box, input.Substring(0, split) });
                append.Invoke(host, new object[] { box, input.Substring(split) });
                if (box.Text != "AGROMSB!") throw new Exception("Color markers leaked into output");
                for (int index = 0; index < colors.Length; index++)
                {
                    box.Select(index, 1);
                    if (box.SelectionColor.ToArgb() != colors[index].ToArgb())
                        throw new Exception("Wrong color at index " + index + ", chunk boundary " + split);
                }
            }
        }
        using (var box = new TextBox())
        {
            append.Invoke(host, new object[] { box, input });
            if (box.Text != "AGROMSB!") throw new Exception("Plain output must hide protocol markers");
        }
        Console.WriteLine("PASS actual Linux output colors, black reset, marker-only chunks and plain text fallback");
    }
}
