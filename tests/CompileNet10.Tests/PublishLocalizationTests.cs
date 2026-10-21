using System;
using System.IO;
using PascalABCCompiler;
using VisualPascalABCPlugins;

internal static partial class Program
{
    private static void CheckPublishLocalization()
    {
        var root = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
        while (root != null && !File.Exists(Path.Combine(root.FullName, "PascalABCNET.sln"))) root = root.Parent;
        if (root == null) throw new DirectoryNotFoundException("Repository root not found.");
        string previous = StringResources.ResDirectoryName;
        try
        {
            foreach (string kit in new[] { "bin", "bin-net10" })
            {
                string russian = Path.Combine(root.FullName, kit, "Lng", "Rus");
                string english = Path.Combine(root.FullName, kit, "Lng", "Eng");
                foreach (var resource in new[] {
                    new { Directory = russian, Caption = "Опубликовать" },
                    new { Directory = english, Caption = "Publish" } })
                {
                    string file = Path.Combine(resource.Directory, "VisualPascalABCNET_VEC.dat");
                    Check(File.Exists(file) && File.ReadAllText(file).Contains("PUBLISH_NET10=" + resource.Caption),
                        kit + " stores the publish translation in Lng: " + resource.Caption);
                }
                StringResources.ResDirectoryName = russian;
                using (var item = Net10MenuItems.CreatePublishItem())
                {
                    Check(item.Text == "Опубликовать", kit + " publish item starts in Russian");
                    // Same update used by StringResourcesLanguage.CurrentLanguageName.
                    StringResources.ResDirectoryName = english;
                    StringResources.UpdateObjectsText();
                    Check(item.Text == "Publish", kit + " existing publish item switches to English without restart");
                    StringResources.ResDirectoryName = russian;
                    StringResources.UpdateObjectsText();
                    Check(item.Text == "Опубликовать", kit + " existing publish item switches back to Russian");
                }
                StringResources.ResDirectoryName = english;
                using (var item = Net10MenuItems.CreatePublishItem())
                    Check(item.Text == "Publish" && item.Name == "CompileNet10Publish",
                        kit + " English startup preserves the publish command identity");
            }
        }
        finally
        {
            StringResources.ResDirectoryName = previous ?? Path.Combine(root.FullName, "bin", "Lng", "Rus");
            StringResources.UpdateObjectsText();
        }
    }
}
