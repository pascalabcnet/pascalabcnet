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

    internal sealed class ReferenceResolutionContext
    {
        public ReferenceResolutionContext(string outputDirectory, bool overwriteOutputFile)
        {
            OutputDirectory = outputDirectory;
            OverwriteOutputFile = overwriteOutputFile;
        }

        public string OutputDirectory { get; }
        public bool OverwriteOutputFile { get; }
    }

    internal sealed class ResolvedReference
    {
        public ResolvedReference(ReferenceSpec specification, string fileName)
        {
            Specification = specification;
            FileName = fileName;
        }

        public ReferenceSpec Specification { get; }
        public string FileName { get; }
    }

    internal interface IReferenceResolver
    {
        ResolvedReference Resolve(ReferenceSpec reference, ReferenceResolutionContext context);
    }

    /// <summary>
    /// Preserves the historical {$reference} lookup and copy-local behaviour.
    /// Keeping it behind IReferenceResolver lets the SDK/NuGet resolver be added
    /// without putting another set of lookup rules into Compiler.GetReferences.
    /// </summary>
    internal sealed class ClassicReferenceResolver : IReferenceResolver
    {
        public ResolvedReference Resolve(ReferenceSpec reference, ReferenceResolutionContext context)
        {
            string fileName = reference.Text.Trim();

            if (!Compiler.CheckPathValid(fileName))
                throw new InvalidAssemblyPathError(reference.DiagnosticFileName, reference.SourceContext);

            if (Compiler.standart_assembly_dict.ContainsKey(fileName))
                return Resolved(reference, Compiler.standart_assembly_dict[fileName]);

            // Preserve the special historical lookup order for PABCRtl.dll.
            if (fileName == StringConstants.pabc_rtl_dll_name)
            {
                string standardAssembly = Compiler.get_assembly_path(fileName, true);
                if (standardAssembly != null && File.Exists(standardAssembly))
                    return Resolved(reference, standardAssembly);
            }

            try
            {
                string sourceFileName = Path.Combine(reference.SourceDirectory, fileName);
                if (File.Exists(sourceFileName))
                {
                    string outputFileName = Path.GetFullPath(
                        Path.Combine(context.OutputDirectory, Path.GetFileName(sourceFileName)));

                    if (sourceFileName != outputFileName)
                    {
                        if (context.OverwriteOutputFile)
                            File.Copy(sourceFileName, outputFileName, true);
                        else if (!File.Exists(outputFileName))
                            File.Copy(sourceFileName, outputFileName, false);
                    }

                    return Resolved(reference, outputFileName);
                }

                string assemblyFileName = Compiler.get_assembly_path(fileName, false);
                if (assemblyFileName != null && File.Exists(assemblyFileName))
                    return Resolved(reference, assemblyFileName);

                throw new AssemblyNotFound(reference.DiagnosticFileName, fileName, reference.SourceContext);
            }
            catch (ArgumentException)
            {
                throw new InvalidAssemblyPathError(reference.DiagnosticFileName, reference.SourceContext);
            }
        }

        private static ResolvedReference Resolved(ReferenceSpec specification, string fileName)
        {
            return new ResolvedReference(specification, fileName);
        }
    }
}
