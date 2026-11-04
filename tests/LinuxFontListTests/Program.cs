using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

internal static class Program
{
    private static void Check(bool value, string message)
    { if (!value) throw new Exception(message); Console.WriteLine("PASS " + message); }
    [STAThread]
    private static int Main(string[] args)
    {
        try
        {
            string path = Path.GetFullPath(args[0]);
            AppDomain.CurrentDomain.AssemblyResolve += (sender, e) => {
                string file = Path.Combine(Path.GetDirectoryName(path), new AssemblyName(e.Name).Name + ".dll");
                return File.Exists(file) ? Assembly.LoadFrom(file) : null;
            };
            var type = Assembly.LoadFrom(path).GetType("VisualPascalABC.FontComboBox", true);
            var flags = BindingFlags.Static | BindingFlags.NonPublic;
            var namesMethod = type.GetMethod("LinuxFontNames", flags);
            var resolve = type.GetMethod("ResolveLinuxFamilyName", flags);
            var names = (string[])namesMethod.Invoke(null, new object[] {
                new[] { "Noto Sans", "Noto Sans", "noto sans", "Droid Sans", "Ubuntu", "monospace", "", null }, "Saved Font" });
            Check(names.Count(name => name.Equals("Noto Sans", StringComparison.OrdinalIgnoreCase)) == 1, "duplicate families and case variants appear once");
            Check(names.Count(name => name.Equals("Monospace", StringComparison.OrdinalIgnoreCase)) == 1 && names.Contains("Monospace"), "Monospace is present once with a stable display name");
            Check(names.Contains("Saved Font") && !names.Any(string.IsNullOrWhiteSpace), "current family is retained and empty names are excluded");
            Check(names.SequenceEqual(names.OrderBy(name => name, StringComparer.OrdinalIgnoreCase)), "font list is sorted by family name");
            Check((string)resolve.Invoke(null, new object[] { "Noto Sans Mono" }) == "Noto Sans Mono", "ordinary family names are unchanged");
            string monoName = (string)resolve.Invoke(null, new object[] { "Monospace" });
            Check(monoName == FontFamily.GenericMonospace.Name, "Monospace resolves through the platform generic monospace family");
            using (var family = new FontFamily(monoName))
            using (var mono = new Font(family, 11))
            using (var current = new Font(FontFamily.GenericSansSerif, 11))
            using (var combo = (ComboBox)Activator.CreateInstance(type))
            {
                var populate = type.GetMethod("Populate");
                populate.Invoke(combo, new object[] { false, current });
                var actual = combo.Items.Cast<string>().ToArray();
                Check(actual.Distinct(StringComparer.OrdinalIgnoreCase).Count() == actual.Length, "actual Mono/Windows font enumeration is deduplicated");
                Check(actual.SequenceEqual(actual.OrderBy(name => name, StringComparer.OrdinalIgnoreCase)), "actual populated control is sorted");
                Check((string)combo.SelectedItem == current.FontFamily.Name, "current editor family stays selected");
                int count = combo.Items.Count;
                combo.SelectedItem = "Monospace";
                populate.Invoke(combo, new object[] { false, mono });
                Check((string)combo.SelectedItem == "Monospace", "reopening settings retains selected Monospace for the same resolved family");
                populate.Invoke(combo, new object[] { false, current });
                Check(combo.Items.Count == count && (string)combo.SelectedItem == current.FontFamily.Name, "repeated population adds no duplicates and follows font changes");
                using (var restored = new Font(new FontFamily(monoName), 11))
                    Check(restored.FontFamily.Name == mono.FontFamily.Name, "stored real family name restores the same font");
            }
            return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error); return 1; }
    }
}
