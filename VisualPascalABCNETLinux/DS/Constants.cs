// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System.IO;
namespace VisualPascalABC
{
    public static class Constants
    {
        public static string SystemDirectoryIdent = "%PABCSYSTEM%";
        public static string WorkingDirectoryIdent = "%WORKINGDIRECTORY%";
        public static string OutputDirectoryIdent = "%OUTPUTDIRECTORY%";
        public static string LibSourceDirectoryIdent = "%LIBSOURCEDIRECTORY%";

        //public static string DefaultWorkingDirectory = SystemDirectoryIdent + @"\WorkDirectory";
        public static string DefaultWorkingDirectory =
            !Tools.IsUnix() ?
                (System.IO.Path.GetPathRoot(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Windows)) + "PABCWork.NET") :
                //System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/" + "PABCWork.NET";
                "~/" + "PABCWork.NET";
        public static string DefaultOutputDirectory = DefaultWorkingDirectory + @"\Output";

        public static string DefaultLibSourceDirectory = SystemDirectoryIdent + @"\LibSource";

        public static string HelpFileName = "%PABCSYSTEM%\\PascalABCNET.chm";
        public static string DotNetHelpFileName = "%PABCSYSTEM%\\Documentation_NET40.chm";
        public static string HelpExamplesFileName = "%PABCSYSTEM%\\PascalABCNETExamples.chm";
        public static string HelpExamplesMapFileName = "%PABCSYSTEM%\\HelpExamples.ini";
        //public static string HelpExamplesDirectory = DefaultWorkingDirectory+"\\Samples";
        public static string HelpExamplesDirectory =  @"/usr/lib/pascalabcnet/Samples";
        public static string HelpTutorialExamplesDirectory = DefaultWorkingDirectory + "\\Samples\\!Tutorial";

        
        public static int CompletionWindowWidth = 200;
        public static int CompletionWindowMaxListLength = 10;
        public static string CompletionWindowDeclarationViewWindowFontName = "Tahoma";
        public static string CompletionWindowCodeCompletionListViewFontName = "Tahoma";
        public static string CompletionInsightWindowFontName = "Tahoma";
		public static string ProjectExtension = ".pabcproj";
        public static bool FileNamesCaseSensetive = false;

        static Constants()
        {
            if (Tools.IsUnix())
            {
                DefaultWorkingDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/" + "PABCWork.NET";
                HelpFileName = "%PABCSYSTEM%/PascalABCNET.chm";
                DotNetHelpFileName = "%PABCSYSTEM%/Documentation_NET40.chm";
                HelpExamplesFileName = "%PABCSYSTEM%/PascalABCNETExamples.chm";
                HelpExamplesMapFileName = "%PABCSYSTEM%/HelpExamples.ini";
                //HelpExamplesDirectory = DefaultWorkingDirectory + "/Samples";
                HelpExamplesDirectory = @"/usr/lib/pascalabcnet/Samples";
                HelpTutorialExamplesDirectory = DefaultWorkingDirectory + "/Samples/!Tutorial";
                DefaultOutputDirectory = DefaultWorkingDirectory + @"/Output";
                DefaultLibSourceDirectory = SystemDirectoryIdent + @"/LibSource";
            }
            else
            {
                string PABCWorkIniFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName) + @"\pabcworknet.ini";
                //System.Windows.Forms.MessageBox.Show(PABCWorkIniFile);
                if (File.Exists(PABCWorkIniFile))
                {
                    var d = File.ReadAllText(PABCWorkIniFile);
                    if (Directory.Exists(d))
                    {
                        DefaultWorkingDirectory = d;
                        HelpExamplesDirectory = DefaultWorkingDirectory + @"\Samples";
                        HelpTutorialExamplesDirectory = DefaultWorkingDirectory + @"\Samples\!Tutorial";
                        DefaultOutputDirectory = DefaultWorkingDirectory + @"\Output";
                    }
                }   
            }
        }
    }
}
