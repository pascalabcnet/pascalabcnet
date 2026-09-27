using System;
using System.IO;
using PascalABCCompiler.Errors;

namespace PascalABCCompiler.References
{
    internal sealed class ReferenceStagingContext
    {
        public ReferenceStagingContext(string outputDirectory, bool overwriteOutputFile)
        {
            OutputDirectory = outputDirectory;
            OverwriteOutputFile = overwriteOutputFile;
        }

        public string OutputDirectory { get; }
        public bool OverwriteOutputFile { get; }
    }

    internal sealed class StagedReference
    {
        public StagedReference(ResolvedReference resolvedReference, string fileName)
        {
            ResolvedReference = resolvedReference;
            FileName = fileName;
        }

        public ResolvedReference ResolvedReference { get; }
        public string FileName { get; }
    }

    internal interface IReferenceStager
    {
        StagedReference Stage(ResolvedReference reference, ReferenceStagingContext context);
    }

    /// <summary>
    /// Preserves the historical copy-local behaviour for a directly referenced
    /// assembly. Transitive runtime dependencies are intentionally not staged.
    /// </summary>
    internal sealed class ClassicReferenceStager : IReferenceStager
    {
        public StagedReference Stage(ResolvedReference reference, ReferenceStagingContext context)
        {
            if (!reference.CopyLocal)
                return new StagedReference(reference, reference.FileName);

            try
            {
                string outputFileName = Path.GetFullPath(
                    Path.Combine(context.OutputDirectory, Path.GetFileName(reference.FileName)));

                if (reference.FileName != outputFileName)
                {
                    if (context.OverwriteOutputFile)
                        File.Copy(reference.FileName, outputFileName, true);
                    else if (!File.Exists(outputFileName))
                        File.Copy(reference.FileName, outputFileName, false);
                }

                return new StagedReference(reference, outputFileName);
            }
            catch (ArgumentException)
            {
                throw new InvalidAssemblyPathError(reference.Specification.DiagnosticFileName,
                    reference.Specification.SourceContext);
            }
        }
    }
}
