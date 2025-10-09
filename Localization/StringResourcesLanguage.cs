// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.IO;

namespace PascalABCCompiler
{

    public class StringResourcesLanguage
    {
        private static Dictionary<string, string> AccessibleLanguagesHashtable = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private static Dictionary<string, string> AccessibleTwoLetterISOHashtable = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private static Dictionary<string, string> AccessibleLCIDHashtable = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private static List<string> accessibleLanguages = new List<string>();
        private static string DefaultLanguage = null;
        private static string DefaultTwoLetterISO = null;
        private static List<string> twoLetterISOLanguages = new List<string>();
        public static List<string> AccessibleLanguages
        {
            get { return accessibleLanguages; }
        }
        public static List<string> TwoLetterISOLanguages
        {
        	get { return twoLetterISOLanguages; }
        }
        private static string configDirectory = null;
        public static string ConfigDirectory
        {
            get { return configDirectory; }
            set
            {
                configDirectory = value;
                Load();
            }
        }
        public static int CurrentLanguageIndex
        {
            get
            {
                return accessibleLanguages.IndexOf(DefaultLanguage);
            }
        }
        private static void Load()
        {
            DirectoryInfo dir = new DirectoryInfo(configDirectory);
            if (!dir.Exists) return;
            DirectoryInfo[] dirs=(dir).GetDirectories();
            foreach (DirectoryInfo di in dirs)
            {
            	string name = Path.Combine(di.FullName, ".LanguageName");
                if (File.Exists(name))
                {
                    using (StreamReader sr = new StreamReader(name, System.Text.Encoding.GetEncoding(1251)))
                    { 
                        string lname = sr.ReadLine();
                        string twoLetterISO = sr.ReadLine();
                        string LCID = sr.ReadLine();
                        if (lname == null || twoLetterISO == null || LCID == null)
                            continue;
                        if (!sr.EndOfStream && sr.ReadLine() == "default")
                        {
                            DefaultLanguage = lname;
                            DefaultTwoLetterISO = twoLetterISO;
                        }
                        AccessibleLanguagesHashtable.Add(lname, di.FullName+Path.DirectorySeparatorChar/*"\\"*/);
                        AccessibleTwoLetterISOHashtable.Add(lname, twoLetterISO);
                        AccessibleLCIDHashtable.Add(twoLetterISO, LCID);
                        accessibleLanguages.Add(lname);
                        twoLetterISOLanguages.Add(twoLetterISO);
                    }
                }
            }
            /*StreamReader sr = new StreamReader(configFileName, System.Text.Encoding.GetEncoding(1251));
            string line,lname;
            int ind;
            accessibleLanguages.Clear();
            AccessibleLanguagesHashtable.Clear();
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line != "")
                {
                    ind = line.IndexOf("=");
                    lname=line.Substring(0, ind);
                    AccessibleLanguagesHashtable.Add(lname, Path.GetDirectoryName(configFileName) + "\\" + line.Substring(ind + 1, line.Length - ind - 1));
                    accessibleLanguages.Add(lname);
                }
            }
            sr.Close();*/
        }

        static StringResourcesLanguage()
        {
            
        }

        public static void LoadDefaultConfig()
        {
        	ConfigDirectory = Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName),"Lng");
            if (AccessibleLanguages.Count > 0)
                if (DefaultLanguage != null)
            	{
                    CurrentLanguageName = DefaultLanguage;
                    CurrentTwoLetterISO = DefaultTwoLetterISO;
            	}
                else
                {
                    CurrentLanguageName = AccessibleLanguages[0];
                    CurrentTwoLetterISO = twoLetterISOLanguages[0];
                }
        }

        private static string currentLanguageName = null;
        public static string CurrentLanguageName
        {
            get{return currentLanguageName;}
            set
            {
                if (!AccessibleLanguagesHashtable.ContainsKey(value)) return;
                currentLanguageName = value;
                currentTwoLetterISO = AccessibleTwoLetterISOHashtable[value];
                currentLCID = AccessibleLCIDHashtable[currentTwoLetterISO];
                StringResources.ResDirectoryName = AccessibleLanguagesHashtable[currentLanguageName];
                StringResources.UpdateObjectsText();
            }
        }

        private static string currentLCID = null;
        public static string CurrentLCID
        {
            get { return currentLCID; }
        }

        private static string currentTwoLetterISO = null;
        public static string CurrentTwoLetterISO
        {
            get{return currentTwoLetterISO;}
            set
            {
                currentTwoLetterISO = value;
            }
        }

        public static string GetLCIDByTwoLetterISO(string iso)
        {
            AccessibleLCIDHashtable.TryGetValue(iso, out var result);
            return result;
        }
    }
}
