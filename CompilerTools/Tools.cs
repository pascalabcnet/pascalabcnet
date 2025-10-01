// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections.Specialized;

namespace PascalABCCompiler
{
    public class Tools
    {

        public static SourceLocation ConvertSourceContextToSourceLocation(string FileName, PascalABCCompiler.SyntaxTree.SourceContext sc)
        {
            if (sc.FileName != null)
                FileName = sc.FileName;
            return new SourceLocation(FileName,
                    sc.begin_position.line_num, sc.begin_position.column_num,
                    sc.end_position.line_num, sc.end_position.column_num);
        }
        public static string ReplaceAllKeys(string str, Dictionary<string, string> hashtable)
        {
            foreach (string key in hashtable.Keys)
                if (hashtable[key] != null)
                    str = str.Replace(key, hashtable[key]);
            return str;
        }


        //ssyy
        public static string GetFullMethodHeaderString(SemanticTree.IFunctionNode member)
        {
            string s = member.name;
            SemanticTree.IParameterNode[] ps = member.parameters;
            if (ps.Length > 0)
            {
                s += '(';
                s += ps[0].type.name;
                for (int i = 1; i < ps.Length; i++)
                {
                    s += "," + ps[i].type.name;
                }
                s += ')';
            }
            if (member.return_value_type != null)
            {
                s += ":" + member.return_value_type.name;
            }
            return s;
        }
        //\ssyy

        public static string GetExecutablePath()
        {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);
        }

        public static string ChangeEncoding(string Text, System.Text.Encoding FromEncoding, System.Text.Encoding ToEncoding)
        {
            return ToEncoding.GetString(FromEncoding.GetBytes(Text));
        }

        public static string[] SplitString(string str, string separator)
        {
            List<string> args = new List<string>();
            if (str != null)
            {
                string buffer = str;
                int i;
                do
                {
                    i = buffer.IndexOf(separator);
                    if (i > 0)
                    {
                        args.Add(buffer.Substring(0, i));
                        buffer = buffer.Remove(0, i + separator.Length);
                    }
                } while (i > 0);
                if (args.Count == 0)
                    args.Add(str);
                else
                    if (buffer != "")
                        args.Add(buffer);
            }
            return args.ToArray();
        }

        public static bool CheckFileNameValid(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;
            char[] invalidFileChars = Path.GetInvalidFileNameChars();
            foreach (char ch in invalidFileChars)
            {
                if (fileName.IndexOf(ch) != -1)
                    return false;
            }
            return true;
        }

        public static string RelativePathTo(

            string fromDirectory, string toPath)
        {

            if (fromDirectory == null)

                throw new ArgumentNullException("fromDirectory");



            if (toPath == null)

                throw new ArgumentNullException("toPath");



            bool isRooted = Path.IsPathRooted(fromDirectory)

                && Path.IsPathRooted(toPath);



            if (isRooted)
            {

                bool isDifferentRoot = string.Compare(

                    Path.GetPathRoot(fromDirectory),

                    Path.GetPathRoot(toPath), true) != 0;



                if (isDifferentRoot)

                    return toPath;

            }


            
            StringCollection relativePath = new StringCollection();

            string[] fromDirectories = fromDirectory.Split(
                Path.DirectorySeparatorChar);

            string[] toDirectories = toPath.Split(
                Path.DirectorySeparatorChar);

            int length = Math.Min(
                fromDirectories.Length,
                toDirectories.Length);

            int lastCommonRoot = -1;



            // find common root

            for (int x = 0; x < length; x++)
            {

                if (string.Compare(fromDirectories[x],

                    toDirectories[x], true) != 0)

                    break;



                lastCommonRoot = x;

            }

            if (lastCommonRoot == -1)

                return toPath;



            // add relative folders in from path

            for (int x = lastCommonRoot + 1; x < fromDirectories.Length; x++)

                if (fromDirectories[x].Length > 0)

                    relativePath.Add("..");



            // add to folders to path

            for (int x = lastCommonRoot + 1; x < toDirectories.Length; x++)

                relativePath.Add(toDirectories[x]);



            // create relative path

            string[] relativeParts = new string[relativePath.Count];

            relativePath.CopyTo(relativeParts, 0);



            string newPath = string.Join(

                Path.DirectorySeparatorChar.ToString(),

                relativeParts);



            return newPath;

        }




    }
}

