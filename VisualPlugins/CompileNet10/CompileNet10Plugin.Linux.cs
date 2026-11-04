#if PABC_LINUX_PLUGIN
using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace VisualPascalABCPlugins
{
    public sealed partial class CompileNet10_VisualPascalABCPlugin
    {
        private Net10LinuxLanguageSession linuxLanguage;
        private ContextMenuStrip linuxNavigationMenu;
        private void InitializeLinuxLanguageServices()
        {
            var events = workbench.ServiceContainer.DocumentService as IWorkbenchDocumentEvents;
            if (events == null) return;
            linuxLanguage = new Net10LinuxLanguageSession(workbench.MainForm, events,
                () => platformItem?.Checked == true && workbench.UserOptions.AllowCodeCompletion,
                () => {
                    var settings = Net10RuntimeSettings.Load(AppDomain.CurrentDomain.BaseDirectory);
                    return new Net10LanguageClient(settings.RuntimeDirectory, settings.DotnetPath,
                        PascalABCCompiler.StringResourcesLanguage.CurrentTwoLetterISO, manualNuGetRestore: true);
                },
                (workbench.ServiceContainer as IWorkbenchIntelliSenseUI)?.CreateIntelliSenseSession(),
                () => workbench.UserOptions.CodeCompletionDot,
                () => workbench.UserOptions.CodeCompletionParams,
                () => workbench.UserOptions.CodeCompletionHint, NavigateLinux,
                workbench.ServiceContainer.DocumentService,
                workbench.ServiceContainer as IWorkbenchDocumentSymbolsUI,
                () => platformItem?.Checked == true,
                () => workbench.UserOptions.ShowQuickClassBrowserPanel);
        }

        private void NavigateLinux(List<Net10NavigationTarget> targets)
        {
            linuxNavigationMenu?.Dispose();
            linuxNavigationMenu = null;
            if (targets.Count == 1) { OpenLinuxNavigationTarget(targets[0]); return; }
            var menu = new ContextMenuStrip { Font = System.Drawing.SystemFonts.MenuFont };
            linuxNavigationMenu = menu;
            foreach (var target in targets)
            {
                var item = menu.Items.Add(target.MetadataTitle ??
                    Path.GetFileName(target.FileName) + ":" + target.Line + ":" + target.Column);
                item.Click += (sender, e) => {
                    if (platformItem?.Checked == true && workbench.UserOptions.AllowCodeCompletion)
                        OpenLinuxNavigationTarget(target);
                };
            }
            menu.Closed += (sender, e) => {
                if (ReferenceEquals(linuxNavigationMenu, menu)) linuxNavigationMenu = null;
                menu.Dispose();
            };
            menu.Show(workbench.MainForm, workbench.MainForm.PointToClient(Control.MousePosition));
        }

        private void OpenLinuxNavigationTarget(Net10NavigationTarget target)
        {
            if (target.MetadataText != null)
                workbench.ServiceContainer.FileService.OpenTabWithText(target.MetadataTitle + " [.NET 10]", target.MetadataText);
            workbench.VisualEnvironmentCompiler.ExecuteSourceLocationAction(
                new PascalABCCompiler.SourceLocation(target.FileName, target.Line, target.Column, target.Line, target.Column),
                SourceLocationAction.GotoBeg);
        }
    }

}
#endif
