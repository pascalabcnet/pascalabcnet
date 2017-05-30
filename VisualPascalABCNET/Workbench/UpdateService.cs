﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using VisualPascalABCPlugins;
using System.Net;
using System.Reflection;

namespace VisualPascalABC
{
    class WorkbenchUpdateService : IWorkbenchUpdateService
    {
        public WorkbenchUpdateService()
        {

        }

        public void CheckForUpdates()
        {
            int status = 1;//1 - up to date, 0 - not up to date, -1 error
            string newVersion = null;
            string curVersion = null;
            try
            {
                WebClient client = new WebClient();
                newVersion = client.DownloadString("http://pascalabc.net/downloads/pabcversion.txt").Trim();
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
                    MessageBox.Show(PascalABCCompiler.StringResources.Get("VP_MF_VERSION_IS_UP_DO_DATE"), PascalABCCompiler.StringResources.Get("VP_MF_UPDATE_CHECK"), MessageBoxButtons.OK);
                    break;
                case 0:
                    if (MessageBox.Show(PascalABCCompiler.StringResources.Get("VP_MF_UPDATE_AVAILABLE") + Environment.NewLine + 
                        string.Format(PascalABCCompiler.StringResources.Get("VP_MF_UPDATE_AVAILABLE_CURRENT_VESION{0}"), curVersion) + Environment.NewLine +
                        string.Format(PascalABCCompiler.StringResources.Get("VP_MF_UPDATE_AVAILABLE_NEW_VESION{0}"), newVersion), 
                        PascalABCCompiler.StringResources.Get("VP_MF_UPDATE_CHECK"), 
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
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
