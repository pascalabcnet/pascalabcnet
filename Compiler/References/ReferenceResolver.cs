using System;
using System.IO;
using PascalABCCompiler.Errors;

namespace PascalABCCompiler.References
{
    /// <summary>
    /// A reference as it was written in a source file, plus the context required
    /// to report errors using the existing compiler diagnostics.
    /// </summary>
    internal sealed class ReferenceSpec
    {
        public ReferenceSpec(string text, string sourceDirectory, string diagnosticFileName,
            SyntaxTree.SourceContext sourceContext)
        {
            Text = text;
            SourceDirectory = sourceDirectory;
            DiagnosticFileName = diagnosticFileName;
            SourceContext = sourceContext;
        }

        public string Text { get; }
        public string SourceDirectory { get; }
        public string DiagnosticFileName { get; }
        public SyntaxTree.SourceContext SourceContext { get; }
    }

    internal sealed class ResolvedReference
    {
        public ResolvedReference(ReferenceSpec specification, string fileName, bool copyLocal)
        {
            Specification = specification;
            FileName = fileName;
            CopyLocal = copyLocal;
        }

        public ReferenceSpec Specification { get; }
        public string FileName { get; }
        public bool CopyLocal { get; }
    }

    internal interface IReferenceResolver
    {
        ResolvedReference Resolve(ReferenceSpec reference);
    }

    /// <summary>
    /// Preserves the historical {$reference} lookup behaviour.
    /// Keeping it behind IReferenceResolver lets the SDK/NuGet resolver be added
    /// without putting another set of lookup rules into Compiler.GetReferences.
    /// </summary>
    internal sealed class ClassicReferenceResolver : IReferenceResolver
    {
        public ResolvedReference Resolve(ReferenceSpec reference)
        {
            string fileName = reference.Text.Trim();

            if (!Compiler.CheckPathValid(fileName))
                throw new InvalidAssemblyPathError(reference.DiagnosticFileName, reference.SourceContext);

            if (Compiler.standart_assembly_dict.ContainsKey(fileName))
                return Resolved(reference, Compiler.standart_assembly_dict[fileName], false);

            // Preserve the special historical lookup order for PABCRtl.dll.
            if (fileName == StringConstants.pabc_rtl_dll_name)
            {
                string standardAssembly = Compiler.get_assembly_path(fileName, true);
                if (standardAssembly != null && File.Exists(standardAssembly))
                    return Resolved(reference, standardAssembly, false);
            }

            try
            {
                string sourceFileName = Path.Combine(reference.SourceDirectory, fileName);
                if (File.Exists(sourceFileName))
                    return Resolved(reference, sourceFileName, true);

                string assemblyFileName = Compiler.get_assembly_path(fileName, false);
                if (assemblyFileName != null && File.Exists(assemblyFileName))
                    return Resolved(reference, assemblyFileName, false);

                throw new AssemblyNotFound(reference.DiagnosticFileName, fileName, reference.SourceContext);
            }
            catch (ArgumentException)
            {
                throw new InvalidAssemblyPathError(reference.DiagnosticFileName, reference.SourceContext);
            }
        }

        private static ResolvedReference Resolved(ReferenceSpec specification, string fileName,
            bool copyLocal)
        {
            return new ResolvedReference(specification, fileName, copyLocal);
        }
    }
}
