// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System.Collections.Generic;
using PascalABCCompiler.TreeRealization;

namespace PascalABCCompiler.TreeConverter
{
#if PABCNET_MODERN
    // Memoized conversions belong to the current reference set, not to the
    // permanent semantic nodes (e.g. SystemLibrary.integer_type).
    internal sealed class CompilerConversionCacheState
    {
        private readonly Dictionary<compiled_type_node, TypeConversions> types =
            new Dictionary<compiled_type_node, TypeConversions>(ReferenceEqualityComparer.Instance);

        internal bool HasEntries { get { return types.Count != 0; } }

        internal TypeConversions ForType(compiled_type_node owner)
        {
            TypeConversions conversions;
            if (!types.TryGetValue(owner, out conversions))
            {
                conversions = new TypeConversions();
                types.Add(owner, conversions);
            }
            return conversions;
        }

        internal sealed class TypeConversions
        {
            internal readonly Dictionary<type_node, function_node> ImplicitTo = new Dictionary<type_node, function_node>();
            internal readonly Dictionary<type_node, function_node> ImplicitFrom = new Dictionary<type_node, function_node>();
            internal readonly Dictionary<type_node, function_node> ExplicitTo = new Dictionary<type_node, function_node>();
            internal readonly Dictionary<type_node, function_node> ExplicitFrom = new Dictionary<type_node, function_node>();
        }
    }
#endif
}
