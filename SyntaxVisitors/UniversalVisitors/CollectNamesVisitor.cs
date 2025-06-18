// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler.Errors;
using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class CollectNameVisitor : BaseChangeVisitor
    {
        public Dictionary<string, HashSet<string>> UnitNamesToSymbols { get; set; }
        private string thisUnitName;
        public string ThisUnitName 
        { 
            get => thisUnitName;
            set 
            {
                thisUnitName = value;
                UnitNamesToSymbols[thisUnitName] = new HashSet<string>();
            }
        }

        public CollectNameVisitor() { }

        public override void visit(procedure_header procedure_Header)
        {
            if (procedure_Header == null || procedure_Header.name == null) return;
            string foundName = procedure_Header.name.ToString();
            //throw new NotImplementedException(ThisUnitName + " " + procedure_Header.name.ToString());
            UnitNamesToSymbols[ThisUnitName].Add(foundName);
        }

        public override void visit(function_header function_Header)
        {
            if (function_Header == null || function_Header.name == null) return;
            string foundName = function_Header.name.ToString();
            //throw new NotImplementedException(ThisUnitName + " " + procedure_Header.name.ToString());
            UnitNamesToSymbols[ThisUnitName].Add(foundName);
        }
    }
}
