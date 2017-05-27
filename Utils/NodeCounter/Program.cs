// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PascalABCCompiler.PascalABCNewParser;
using PascalABCCompiler.Parsers;
using System.IO;
using PascalABCCompiler.SyntaxTree;

namespace NodeCounter
{
    class Program
    {
        static List<string> GetFiles(string path)
        {
            List<string> res = new List<string>();
            foreach (string x in Directory.GetFiles(path, "*.pas"))
                res.Add(File.ReadAllText(x));

            foreach (string x in Directory.GetDirectories(path))
                res.AddRange(GetFiles(x));

            return res;
        }

        static void Main(string[] args)
        {
            PascalABCNewLanguageParser parser = new PascalABCNewLanguageParser();

            string root = @"C:\PABCWork.NET\Samples"; //args[0];
            var files = GetFiles(root);
            var visitor = new CountVisitor();
            foreach (var x in files)
            {
                dynamic res = parser.BuildTree("ProgramFile", x, ParseMode.Normal);

                var executer = new ExecuteVisitor(visitor);
                if (res != null)
                    executer.visit(res);
            }
            List<KeyValuePair<string, int>> myList = visitor.nodeCount.ToList();

            myList.Sort((firstPair, nextPair) =>
            {
                return nextPair.Value.CompareTo(firstPair.Value);
            }
            );

            StreamWriter resFile = new StreamWriter("NodesCount.txt");
            foreach (KeyValuePair<string, int> x in myList)
                if (x.Value > 0)
                    resFile.WriteLine(x.Key + " - " + x.Value);
            resFile.Close();
            //Console.ReadKey();

        }
    }
}
