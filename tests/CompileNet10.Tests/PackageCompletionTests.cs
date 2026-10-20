using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using VisualPascalABC;
using VisualPascalABCPlugins;

internal static partial class Program
{
    private static void CheckPackageDirectiveCompletion()
    {
        var name = Net10PackageDirectiveContext.Parse("{$package No", 12);
        Check(name != null && !name.IsVersion && name.Prefix == "No" && !name.HasClosingBrace,
            "package name is recognized before the directive is closed");

        const string versionText = "  {$package NodaTime 3.2.}";
        var version = Net10PackageDirectiveContext.Parse(versionText, versionText.Length - 1);
        Check(version != null && version.IsVersion && version.PackageId == "NodaTime" &&
            version.Prefix == "3.2." && version.HasClosingBrace,
            "package version is recognized without inserting a second closing brace");

        Check(Net10PackageDirectiveContext.Parse("", 0) == null &&
            Net10PackageDirectiveContext.Parse("begin {$package No", 18) == null &&
            Net10PackageDirectiveContext.Parse("{$reference No", 14) == null,
            "package completion does not activate outside its directive");

        var images = new CodeCompletionImagesProvider();
        var suggestions = new object[] {
            new Dictionary<string, object> {
                ["Id"] = "NodaTime", ["Version"] = "3.2.4",
                ["Description"] = "Date and time library", ["Source"] = "nuget.org"
            }
        };
        var provider = new Net10PackageCompletionProvider(suggestions, images, name);
        Check(provider.Items.Length == 1 && provider.Items[0].Text.Contains("NodaTime") &&
            provider.Items[0].Description.Contains("Date and time library"),
            "package list keeps its short row and separate description");
        var popularityOrder = new[] {
            new DefaultCompletionData("Z.Popular", null, 0),
            new DefaultCompletionData("A.Rare", null, 0)
        };
        using (var list = new PABCNETCodeCompletionListView(popularityOrder, true, null, true))
        {
            list.ImageList = images.ImageList;
            list.SelectIndex(0);
            Check(list.SelectedCompletionData.Text == "Z.Popular",
                "package list keeps relevance and download order instead of alphabetic sorting");
        }
        var versions = new Net10PackageCompletionProvider(new object[] { "3.2.4", "3.1.0" }, images, version);
        Check(versions.Items.Length == 2 && versions.Items[0].Text == "3.2.4",
            "version completion keeps server ordering");

        var longName = new Net10PackageCompletionProvider(new object[] {
            new Dictionary<string, object> {
                ["Id"] = "Microsoft.Extensions.DependencyInjection.Abstractions",
                ["Version"] = "10.0.0", ["Description"] = "Dependency injection", ["Source"] = "nuget.org"
            }
        }, images, name);
        using (var parent = new Form())
        using (var editor = new EditorDocument())
        {
            editor.TextEditor.Document.TextContent = "{$package Mic";
            editor.TextEditor.ActiveTextAreaControl.Caret.Position = editor.TextEditor.Document.OffsetToPosition(
                editor.TextEditor.Document.TextLength);
            var constructor = Array.Find(typeof(PABCNETCodeCompletionWindow).GetConstructors(
                BindingFlags.NonPublic | BindingFlags.Instance), item => item.GetParameters().Length == 8);
            using (var window = (PABCNETCodeCompletionWindow)constructor.Invoke(new object[] {
                longName, longName.Items, parent, editor.TextEditor, false, true, images, true
            }))
                Check(window.Width > Constants.CompletionWindowWidth &&
                    window.Width <= Math.Max(Constants.CompletionWindowWidth,
                        Screen.GetWorkingArea(window).Width / 2),
                    "package popup fits long IDs while keeping room for the description on High-DPI screens: " +
                    window.Width + " / " + Screen.GetWorkingArea(window).Width);
        }

        using (var editor = new EditorDocument())
        {
            int calls = 0;
            CodeCompletionAllNamesAction.PackageCompletion = area =>
            {
                calls++;
                return area == editor.TextEditor.ActiveTextAreaControl.TextArea;
            };
            try
            {
                new CodeCompletionAllNamesAction().Execute(editor.TextEditor.ActiveTextAreaControl.TextArea);
                Check(calls == 1, "Ctrl+Space routes to the modern package provider before legacy completion");
            }
            finally { CodeCompletionAllNamesAction.PackageCompletion = null; }
        }

        using (var editor = new EditorDocument())
        {
            var area = editor.TextEditor.ActiveTextAreaControl.TextArea;
            editor.TextEditor.Document.TextContent = "{$package ";
            area.Caret.Position = editor.TextEditor.Document.OffsetToPosition(editor.TextEditor.Document.TextLength);
            var legacy = typeof(VisualPascalABC.Form1).Assembly.GetType("VisualPascalABC.CodeCompletionKeyHandler", true);
            var handler = legacy.GetMethod("Attach", BindingFlags.Public | BindingFlags.Static).Invoke(null,
                new object[] { editor.TextEditor });
            CodeCompletionAllNamesAction.PackageDirectiveAtCaret = candidate => candidate == area;
            try
            {
                bool consumed = (bool)legacy.GetMethod("TextAreaKeyEventHandler", BindingFlags.NonPublic | BindingFlags.Instance)
                    .Invoke(handler, new object[] { 'M' });
                Check(!consumed && legacy.GetField("codeCompletionWindow").GetValue(handler) == null,
                    "legacy automatic completion leaves package directive typing to the modern plugin");
            }
            finally
            {
                CodeCompletionAllNamesAction.PackageDirectiveAtCaret = null;
                legacy.GetMethod("Detach", BindingFlags.Public | BindingFlags.Static).Invoke(null,
                    new object[] { editor.TextEditor });
            }
        }
    }
}
