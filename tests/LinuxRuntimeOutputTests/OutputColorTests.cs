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
        using (var box = new RichTextBox())
        {
            box.Text = "old";
            box.Select(0, 3);
            append.Invoke(host, new object[] { box, "\ufffe" });
            append.Invoke(host, new object[] { box, "Ошибка\ufffa обычный текст" });
            if (box.Text != "oldОшибка обычный текст") throw new Exception("Unicode output changed");
            box.Select(0, 3);
            if (box.SelectionColor.ToArgb() != Color.Black.ToArgb()) throw new Exception("Existing selection recolored");
            box.Select(3, 6);
            if (box.SelectionColor.ToArgb() != Color.Red.ToArgb()) throw new Exception("Color lost between chunks");
            box.Select(9, 1);
            if (box.SelectionColor.ToArgb() != Color.Black.ToArgb()) throw new Exception("Black reset lost");
        }
        Console.WriteLine("PASS " + ide.GetName().Name + " output colors, black reset, fragmented input, Unicode and existing selection");
    }

    // Compare the old Windows implementation and the shared implementation
    // in separate processes, using the color codes supported by both.
    internal static void Snapshot(Assembly ide)
    {
        var type = ide.GetType("VisualPascalABC.Form1", true);
        var host = FormatterServices.GetUninitializedObject(type);
        GC.SuppressFinalize(host);
        var append = type.GetMethod("AppendTextInternalAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        foreach (string input in new[] { "", "обычный текст\n", "A\uffffG\ufffeR\ufffdO\ufffcM\ufffbS!", "\ufffe\uffffДа", "\ufffe" })
        {
            for (int split = 0; split <= input.Length; split++)
            {
                using (var box = new RichTextBox())
                {
                    box.CreateControl();
                    append.Invoke(host, new object[] { box, input.Substring(0, split) });
                    append.Invoke(host, new object[] { box, input.Substring(split) });
                    Console.Write(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(box.Text)));
                    for (int index = 0; index < box.TextLength; index++)
                    {
                        box.Select(index, 1);
                        Console.Write(" " + box.SelectionColor.ToArgb());
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
