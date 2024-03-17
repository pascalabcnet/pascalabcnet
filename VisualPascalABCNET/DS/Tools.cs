// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using PascalABCCompiler;
using PascalABCCompiler.TreeConverter;

namespace VisualPascalABC
{
    public class Tools
    {
        public static string GetTextFromClipboard()
        {
            IDataObject dataObject = Clipboard.GetDataObject();
            if (dataObject.GetDataPresent(DataFormats.Text))
            {
                // Yes it is, so display it in a text box.
                return (string)dataObject.GetData(DataFormats.Text);
            }
            return "";
        }

        public static bool CopyTextToClipboard(string stringToCopy)
        {
            if (stringToCopy.Length > 0)
            {   
                DataObject dataObject = new DataObject();
                dataObject.SetData(DataFormats.UnicodeText, true, stringToCopy);
                // Work around ExternalException bug. (SD2-426)
                // Best reproducable inside Virtual PC.
                try
                {
                    Clipboard.SetDataObject(dataObject, true, 10, 50);
                }
                catch (ExternalException)
                {
                    Application.DoEvents();
                    try
                    {
                        Clipboard.SetDataObject(dataObject, true, 10, 50);
                    }
                    catch (ExternalException) { }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string MakeFilter(string Filter, string Name, string[] Extensions)
        {
            string sf = PascalABCCompiler.FormatTools.ExtensionsToString(Extensions, "*", ";");
            sf = string.Format(VECStringResources.Get("DIALOGS_FILTER_PART_{0}{1}|{1}|"), Name, sf);
            if (sf.IndexOf(compiler_string_consts.pascalSourceFileExtension) >= 0)
                return sf + Filter;
            else
                return Filter + sf;

        }
        
        public static string MakeProjectFilter(string Filter, string Name, string[] Extensions)
        {
            string sf = PascalABCCompiler.FormatTools.ExtensionsToString(Extensions, "*", ";");
            sf = string.Format(VECStringResources.Get("DIALOGS_PROJECT_FILTER_PART_{0}{1}|{1}|"), Name, sf);
            if (sf.IndexOf(".pabcproj") >= 0)
                return sf + Filter;
            else
                return Filter + sf;

        }
        
        public static string MakeAllFilter(string AllFilter, string Name, string[] Extensions)
        {
            string sf = PascalABCCompiler.FormatTools.ExtensionsToString(Extensions, "*", ";");
            sf += ";";
            if (sf.IndexOf(".pas;") >= 0)
                return sf + AllFilter;
            else
                return AllFilter + sf;
        }
        public static string FinishMakeFilter(string Filter, string AllFilter)
        {
            if (AllFilter != "")
                AllFilter = AllFilter.Substring(0, AllFilter.Length - 1);
            if (Filter != "")
                Filter = string.Format(VECStringResources.Get("DIALOGS_FILTER_PARTALL_{0}|{0}|{1}"), AllFilter, Filter);
            return Filter + VECStringResources.Get("DIALOGS_FILTER_ALLFILES") + " (*.*)|*.*";
        }

        public static string FileNameToLower(string FileName)
        {
            if (!Constants.FileNamesCaseSensetive)
                return FileName.ToLower();
            return FileName;
        }

        public static string GetRangeDescription(int selectedItem, int itemCount)
        {
            return string.Format(PascalABCCompiler.StringResources.Get("CODE_COMPLETION_{0}_FROM_{1}"), selectedItem, itemCount);
        }

        public static bool IsUnix()
        {
            return System.Environment.OSVersion.Platform == System.PlatformID.Unix || System.Environment.OSVersion.Platform == System.PlatformID.MacOSX;
        }
    }
}
