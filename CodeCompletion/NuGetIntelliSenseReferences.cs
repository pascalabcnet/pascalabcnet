#if PABCNET_MODERN
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Languages.Facade;
using PascalABCCompiler;
using PascalABCCompiler.Errors;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.SyntaxTree;
using PascalABCNet.PackageServices;
using Requirement = PascalABCNet.PackageServices.PackageRequirement;

namespace CodeCompletion
{
    // Prepare the complete analysis graph before constructing reflection-backed DOMs.
    internal sealed class NuGetIntelliSenseReferences
    {
        private readonly PackageRestoreService restoreService = new PackageRestoreService();
        private readonly Dictionary<string, CacheEntry> cache = new Dictionary<string, CacheEntry>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, ParsedUnit> parsed = new Dictionary<string, ParsedUnit>(StringComparer.OrdinalIgnoreCase);
        internal readonly HashSet<string> SourceFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        internal IReadOnlyList<string> Resolve(compilation_unit root, Action beforeReferenceReload)
        {
            SourceFiles.Clear();
            var batches = new List<compilation_unit>();
            var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            Collect(root);
            string fileName = Path.GetFullPath(root.file_name);
            var explicitPaths = new List<string>();
            foreach (var unit in batches)
                foreach (var directive in unit.compiler_directives ?? new List<compiler_directive>())
                    if (string.Equals(directive.Name?.text, "reference", StringComparison.OrdinalIgnoreCase))
                    {
                        string name = (directive.Directive?.text ?? "").Trim('\'', '"');
                        string path = Compiler.GetReferenceFileName(name, Path.GetDirectoryName(unit.file_name));
                        if (path == null) throw new AssemblyNotFound(unit.file_name, name, directive.source_context);
                        explicitPaths.Add(Path.GetFullPath(path));
                    }
            string signature = string.Join("\n", batches.SelectMany(unit =>
                (unit.compiler_directives ?? new List<compiler_directive>())
                .Where(d => string.Equals(d.Name?.text, "package", StringComparison.OrdinalIgnoreCase))
                .Select(d => unit.file_name + "|" + d.Directive?.text))) + "\n" + string.Join("\n", explicitPaths);
            if (cache.TryGetValue(fileName, out var previous) && previous.Signature == signature && previous.Paths.All(File.Exists))
            {
                Register(previous.Paths, beforeReferenceReload);
                return previous.Paths;
            }
            var requirements = new Dictionary<string, Requirement>(StringComparer.OrdinalIgnoreCase);
            var origins = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            ResolvedPackageGraph graph = null;
            foreach (var unit in batches)
            {
                bool added = false;
                foreach (var directive in unit.compiler_directives ?? new List<compiler_directive>())
                {
                    if (!string.Equals(directive.Name?.text, "package", StringComparison.OrdinalIgnoreCase)) continue;
                    string[] parts = (directive.Directive?.text ?? "").Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 2) continue; // Incomplete directive while typing.
                    if (requirements.TryGetValue(parts[0], out var existing))
                    {
                        if (!string.Equals(existing.Version, parts[1], StringComparison.OrdinalIgnoreCase))
                            throw Failure("PACKAGE_VERSION_ALREADY_REQUESTED{0}{1}{2}{3}{4}", unit.file_name,
                                parts[0], parts[1], unit.file_name, existing.Version, origins[parts[0]]);
                        continue;
                    }
                    requirements.Add(parts[0], new Requirement(parts[0], parts[1]));
                    origins[parts[0]] = unit.file_name;
                    added = true;
                }
                if (!added) continue;
                var next = restoreService.RestoreAsync(new PackageRestoreRequest {
                    Packages = requirements.Values.OrderBy(p => p.Id, StringComparer.OrdinalIgnoreCase).ToArray(),
                    ProgramFilePath = fileName
                }).GetAwaiter().GetResult();
                if (graph != null)
                    foreach (var selected in graph.Packages)
                    {
                        var replacement = next.Packages.FirstOrDefault(p => string.Equals(p.Id, selected.Id, StringComparison.OrdinalIgnoreCase));
                        if (replacement == null)
                            throw Failure("PACKAGE_DEPENDENCY_REMOVAL{0}{1}{2}{3}", unit.file_name,
                                selected.Id, selected.Version, origins[selected.Id], unit.file_name);
                        if (replacement.Version != selected.Version)
                            throw Failure("PACKAGE_DEPENDENCY_REPLACEMENT{0}{1}{2}{3}{4}", unit.file_name,
                                selected.Id, selected.Version, origins[selected.Id], unit.file_name, replacement.Version);
                    }
                foreach (var package in next.Packages)
                {
                    if (!origins.ContainsKey(package.Id)) origins.Add(package.Id, unit.file_name);
                    foreach (var dependency in package.Dependencies)
                    {
                        var selected = next.Packages.FirstOrDefault(p => string.Equals(p.Id, dependency.Id, StringComparison.OrdinalIgnoreCase));
                        if (selected != null && !NuGet.Versioning.VersionRange.Parse(dependency.VersionRange)
                                .Satisfies(NuGet.Versioning.NuGetVersion.Parse(selected.Version)))
                            throw Failure("PACKAGE_DEPENDENCY_RANGE_UNSATISFIED{0}{1}{2}{3}", unit.file_name,
                                package.Id, dependency.Id, dependency.VersionRange, selected.Version);
                    }
                }
                graph = next;
            }
            var paths = new List<string>(explicitPaths);
            if (graph != null)
                foreach (var package in graph.Packages)
                {
                    if (package.CompileAssemblies.Count != 0 && package.RuntimeAssemblies.Count == 0)
                        throw Failure("PACKAGE_REQUIRES_RUNTIME_ASSEMBLY{0}", fileName, package.Id);
                    paths.AddRange(package.RuntimeAssemblies);
                }
            string[] result = paths.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
            Register(result, beforeReferenceReload);
            cache[fileName] = new CacheEntry(signature, result);
            return result;

            void Collect(compilation_unit unit)
            {
                string path = Path.GetFullPath(unit.file_name);
                if (!visited.Add(path)) return;
                SourceFiles.Add(path);
                batches.Add(unit);
                var language = LanguageProvider.Instance.SelectLanguageByExtensionSafe(path);
                if (language == null) return;
                var uses = unit is program_module program ? new[] { program.used_units } :
                    unit is unit_module module ? new[] { module.interface_part?.uses_modules, module.implementation_part?.uses_modules } :
                    Array.Empty<uses_list>();
                foreach (var list in uses)
                {
                    if (list == null) continue;
                    foreach (var item in list.units.AsEnumerable().Reverse())
                    {
                        string name;
                        if (item is uses_unit_in located) name = located.in_file.Value;
                        else
                        {
                            if (item.name?.idents.Count != 1) continue;
                            name = item.name.idents[0].name;
                            if (language.LanguageInformation.SpecialModulesAliases != null &&
                                language.LanguageInformation.SpecialModulesAliases.TryGetValue(name, out var alias)) name = alias;
                        }
                        string source = CodeCompletionNameHelper.FindSourceFileName(name, out _, language, Path.GetDirectoryName(path));
                        if (source != null) CollectSource(source);
                        else
                        {
                            string pcu = CodeCompletionController.comp.FindPCUFileName(name, Path.GetDirectoryName(path), out _, language);
                            if (pcu != null) CollectPcu(pcu, language);
                            else SourceFiles.Add(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(path),
                                Path.HasExtension(name) ? name : name + ".pas")));
                        }
                    }
                }
            }

            void CollectSource(string source)
            {
                source = Path.GetFullPath(source);
                if (visited.Contains(source)) return;
                SourceFiles.Add(source);
                string text = CodeCompletionController.comp.GetSourceFileText(source);
                if (!parsed.TryGetValue(source, out var entry) || entry.Text != text)
                {
                    var errors = new List<Error>();
                    var parser = LanguageProvider.Instance.SelectLanguageByExtension(source).Parser;
                    var tree = parser.GetCompilationUnit(source, text, errors, new List<CompilerWarning>(), ParseMode.Normal, true);
                    if (tree == null) tree = parser.GetCompilationUnit(source, text, errors, new List<CompilerWarning>(), ParseMode.Special, true);
                    if (tree == null && errors.Count > 0) throw errors[0];
                    parsed[source] = entry = new ParsedUnit(text, tree);
                }
                if (entry.Tree != null) Collect(entry.Tree);
            }

            void CollectPcu(string path, ILanguage language)
            {
                path = Path.GetFullPath(path);
                if (!visited.Add(path)) return;
                SourceFiles.Add(path);
                var directives = IntellisensePCUReader.ReadReferenceDirectives(path, out var modules);
                batches.Add(new unit_module { file_name = path, compiler_directives = directives });
                foreach (string name in modules.Reverse())
                {
                    if (language.SystemUnitNames.Contains(Path.GetFileNameWithoutExtension(name), StringComparer.OrdinalIgnoreCase)) continue;
                    string source = CodeCompletionNameHelper.FindSourceFileName(name, out _, language, Path.GetDirectoryName(path));
                    if (source != null) CollectSource(source);
                    else
                    {
                        string child = CodeCompletionController.comp.FindPCUFileName(name, Path.GetDirectoryName(path), out _, language);
                        if (child != null) CollectPcu(child, language);
                    }
                }
            }
        }

        private static void Register(IEnumerable<string> paths, Action beforeReload)
        {
            var all = paths.ToArray();
            PascalABCCompiler.NetHelper.NetHelper.ValidateIntelliSenseReferencePaths(all);
            PascalABCCompiler.NetHelper.NetHelper.RegisterIntelliSenseReferencePaths(all, beforeReload);
        }
        private static Exception Failure(string key, string file, params object[] values) =>
            new PackageRestoreFailed(string.Format(StringResources.Get("COMPILATIONERROR_" + key), values), file, null);
        private sealed class ParsedUnit
        {
            internal readonly string Text;
            internal readonly compilation_unit Tree;
            internal ParsedUnit(string text, compilation_unit tree) { Text = text; Tree = tree; }
        }
        private sealed class CacheEntry
        {
            internal readonly string Signature;
            internal readonly string[] Paths;
            internal CacheEntry(string signature, string[] paths) { Signature = signature; Paths = paths; }
        }
    }
}
#endif
