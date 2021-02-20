using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using LexScanner;

namespace sss
{
    class mymain
    {
        static void Main(string[] args)
        {
            Parser parser = new Parser();
            parser.scanner = new Scanner(new FileStream(@"..\..\Progrs\minmax.obr", FileMode.Open));
            bool b = parser.Parse();
            Console.WriteLine(b);
            Console.ReadKey();
        }
    }
}
