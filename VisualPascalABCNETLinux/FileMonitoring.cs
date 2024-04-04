// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace VisualPascalABC
{
    public class FileChangeWatcher : IDisposable
    {
        FileSystemWatcher watcher;
        string fileName;
        Hashtable activeWatchers = new Hashtable();
        bool wasChangedExternally = false;

        public FileChangeWatcher(string path)
        {
            VisualPABCSingleton.MainForm.Activated += MainForm_Activated;
            fileName = path;
            activeWatchers.Add(this, this);
            SetWatcher();
        }

        public void Dispose()
        {
            activeWatchers.Remove(this);
            VisualPABCSingleton.MainForm.Activated -= MainForm_Activated;
            if (watcher != null)
            {
                watcher.Dispose();
                watcher = null;
            }
        }

        bool enabled = true;

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                SetWatcher();
            }
        }

        void SetWatcher()
        {
            if (watcher != null)
            {
                watcher.EnableRaisingEvents = false;
            }

            if (!enabled)
                return;
            try
            {
                if (watcher == null)
                {
                    watcher = new FileSystemWatcher();
                    watcher.SynchronizingObject = VisualPABCSingleton.MainForm;
                    watcher.Changed += OnFileChangedEvent;
                    watcher.Created += OnFileChangedEvent;
                    watcher.Renamed += OnFileChangedEvent;
                }
                watcher.Path = Path.GetDirectoryName(fileName);
                watcher.Filter = Path.GetFileName(fileName);
                watcher.EnableRaisingEvents = true;
            }
            catch (PlatformNotSupportedException)
            {
                if (watcher != null)
                {
                    watcher.Dispose();
                }
                watcher = null;
            }

        }

        void OnFileChangedEvent(object sender, FileSystemEventArgs e)
        {
            if (!wasChangedExternally)
            {
                wasChangedExternally = true;
            }
        }

        void showChangedFileMessage()
        {
            string mes = null;
            if (!File.Exists(fileName))
            {
                mes = Form1StringResources.Get("FILE_NOT_EXIST_MESSAGE");
                if (MessageBox.Show(fileName + "\n\n" + mes, Form1StringResources.Get("CHANGED_FILE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    WorkbenchServiceFactory.FileService.SetFileAsChanged(fileName);
                }
                else
                {
                    WorkbenchServiceFactory.FileService.CloseFile(fileName);
                }
                return;
            }

            mes = Form1StringResources.Get("FILE_CHANGED_MESSAGE");
            if (MessageBox.Show(fileName + "\n\n" + mes, Form1StringResources.Get("CHANGED_FILE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                WorkbenchServiceFactory.FileService.ReloadFile(fileName);
            }
            else
            {
                WorkbenchServiceFactory.FileService.SetFileAsChanged(fileName);
            }
        }

        void MainForm_Activated(object sender, EventArgs e)
        {
            try
            {
                if (wasChangedExternally)
                {
                    wasChangedExternally = false;

                    WorkbenchServiceFactory.Workbench.MainForm.BeginInvoke((Action)showChangedFileMessage);
                    
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}

