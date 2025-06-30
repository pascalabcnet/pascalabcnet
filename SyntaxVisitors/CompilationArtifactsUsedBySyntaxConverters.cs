// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System.Collections.Generic;

namespace PascalABCCompiler
{
    public struct CompilationArtifactsUsedBySyntaxConverters
    {
        public CompilationArtifactsUsedBySyntaxConverters(Dictionary<string, HashSet<string>> namesFromUsedUnits) 
        {
            NamesFromUsedUnits = namesFromUsedUnits;
        }

        public Dictionary<string, HashSet<string>> NamesFromUsedUnits { get; }
    }
}
