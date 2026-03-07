// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System.Collections.Generic;
using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.CoreUtils;

namespace Languages.Pascal.Frontend.Core
{
    public class ParserLambdaHelper
    {
        private readonly GeneratedNamesManager generatedNamesManager = new GeneratedNamesManager();
        public List<function_lambda_definition> lambdaDefinitions = new List<function_lambda_definition>();

        public string CreateLambdaName()
        {
            return generatedNamesManager.GenerateName(StringConstants.lambdaPrefix);
        }

        public bool IsLambdaName(ident id)
        {
            return id.name.StartsWith(StringConstants.lambdaPrefix);
        }
    }
}
