using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using VisualPascalABCPlugins;
using System.Net;
using System.Reflection;
using Microsoft.Win32;

namespace VisualPascalABC
{
    class WorkbenchUpdateService : IWorkbenchUpdateService
    {
        public WorkbenchUpdateService()
        {

        }

        public bool IsDotnet71Installed()
        {
            if (Environment.OSVersion.Version.Major < 6)
                return true;
            try
            {
                using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full"))
                {
                    return key != null && ((key.GetValue("Version") as string).StartsWith("4.7") || (key.GetValue("Version") as string).StartsWith("4.8"));
                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }
            
        public void CheckForUpdates()
        {
            int status = 1;//1 - up to date, 0 - not up to date, -1 error
            string newVersion = null;
            string curVersion = null;
            if (!IsDotnet71Installed())
            {
                if (MessageBox.Show(PascalABCCompiler.StringResources.Get("VP_MF_DOTNET_AVAILABLE"),
                        PascalABCCompiler.StringResources.Get("VP_MF_DOTNET_UPDATE_CHECK"),
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    WorkbenchServiceFactory.OperationsService.AddTabWithUrl(".NET Framework", PascalABCCompiler.StringResources.Get("VP_MF_FRAMEWORK_DOWNLOAD_PAGE"));
                }
            }
            try
            {
                WebClient client = new WebClient();
                newVersion = client.DownloadString("https://pascalabc.net/downloads/pabcversion.txt").Trim();
                curVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                if ((new Version(curVersion)).CompareTo(new Version(newVersion)) == -1)
                    status = 0;
                
            }
            catch
            {
                status = -1;
            }
            switch (status)
            {
                case 1:
                    MessageBox.Show(PascalABCCompiler.StringResources.Get("VP_MF_VERSION_IS_UP_TO_DATE"), PascalABCCompiler.StringResources.Get("VP_MF_UPDATE_CHECK"), MessageBoxButtons.OK);
                    break;
                case 0:
                    if (MessageBox.Show(PascalABCCompiler.StringResources.Get("VP_MF_UPDATE_AVAILABLE") + Environment.NewLine + 
                        string.Format(PascalABCCompiler.StringResources.Get("VP_MF_UPDATE_AVAILABLE_CURRENT_VESION{0}"), curVersion) + Environment.NewLine +
                        string.Format(PascalABCCompiler.StringResources.Get("VP_MF_UPDATE_AVAILABLE_NEW_VESION{0}"), newVersion), 
                        PascalABCCompiler.StringResources.Get("VP_MF_UPDATE_CHECK"), 
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //System.Diagnostics.Process.Start(PascalABCCompiler.StringResources.Get("VP_MF_PABC_DOWNLOAD_PAGE"));
                        // SSM 24/04/19 - вернул назад, т.к. обновление закрывало оболочку из-за некорректно написанного асинхронного вызова. Обойдёмся без асинхронного вызова. Слишком много пользователей пострадало.
                        WorkbenchServiceFactory.OperationsService.AddTabWithUrl("PascalABC.NET", PascalABCCompiler.StringResources.Get("VP_MF_PABC_DOWNLOAD_PAGE"));
                    }
                    break;
                case -1:
                    MessageBox.Show(PascalABCCompiler.StringResources.Get("VP_MF_UPDATE_CHECK_ERROR"), PascalABCCompiler.StringResources.Get("VP_MF_UPDATE_CHECK"), MessageBoxButtons.OK);
                    break;
            }
        }
    }
}
