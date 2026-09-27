namespace PascalABCCompiler.References
{
    /// <summary>
    /// Prepares one compiler reference for use by the remaining compilation
    /// stages. A provider owns the complete preparation policy: it decides how
    /// the reference is resolved and how the selected assembly is staged.
    ///
    /// Compiler deliberately depends on this higher-level operation instead of
    /// calling a resolver and a stager separately. This keeps the classic DLL
    /// rules together and leaves one replacement point for a future SDK/NuGet
    /// provider, whose resolution and staging rules will be different.
    /// </summary>
    internal interface IReferenceProvider
    {
        PreparedReference Prepare(ReferenceSpec reference, ReferenceStagingContext context);
    }

    /// <summary>
    /// Implements the historical {$reference} pipeline:
    /// first locate the assembly using the classic lookup order, then preserve
    /// the classic copy-local behaviour for the located file.
    ///
    /// This class only combines the two existing operations. It intentionally
    /// does not change lookup paths, copying rules or error diagnostics.
    /// </summary>
    internal sealed class ClassicReferenceProvider : IReferenceProvider
    {
        private readonly IReferenceResolver resolver;
        private readonly IReferenceStager stager;

        public ClassicReferenceProvider()
            : this(new ClassicReferenceResolver(), new ClassicReferenceStager())
        {
        }

        internal ClassicReferenceProvider(IReferenceResolver resolver, IReferenceStager stager)
        {
            this.resolver = resolver;
            this.stager = stager;
        }

        public PreparedReference Prepare(ReferenceSpec reference, ReferenceStagingContext context)
        {
            ResolvedReference resolvedReference = resolver.Resolve(reference);
            return stager.Stage(resolvedReference, context);
        }
    }
}
