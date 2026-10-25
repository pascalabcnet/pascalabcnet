#if PABCNET_MODERN
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Buffers.Binary;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace PascalABCCompiler.NETGenerator
{
    // PersistedAssemblyBuilder in .NET 10 omits MethodDebugInformation rows for
    // bodyless methods. Portable PDB requires one row for EVERY MethodDef.
    // Preserve all other debug tables and insert only the missing empty rows.
    internal static class PortablePdbAlignment
    {
        internal static MetadataBuilder Align(MetadataReader methods, MetadataBuilder symbols,
            ImmutableArray<int> rowCounts, MethodDefinitionHandle entryPoint)
        {
            int methodCount = methods.MethodDefinitions.Count;
            if (symbols.GetRowCount(TableIndex.MethodDebugInformation) == methodCount)
                return symbols; // Also supports a runtime which has fixed its writer.

            var debugImage = new BlobBuilder();
            new PortablePdbBuilder(symbols, rowCounts, entryPoint).Serialize(debugImage);
            using var debugProvider = MetadataReaderProvider.FromPortablePdbImage(debugImage.ToImmutableArray());
            var source = debugProvider.GetMetadataReader();
            if (methods.MethodDefinitions.Count(h => methods.GetMethodDefinition(h).RelativeVirtualAddress != 0) !=
                source.MethodDebugInformation.Count)
                throw new InvalidOperationException("Unexpected Portable PDB method row layout.");

            var target = new MetadataBuilder();
            BlobHandle Blob(BlobHandle h) => h.IsNil ? default : target.GetOrAddBlob(source.GetBlobBytes(h));
            StringHandle Text(StringHandle h) => h.IsNil ? default : target.GetOrAddString(source.GetString(h));
            GuidHandle Guid(GuidHandle h) => h.IsNil ? default : target.GetOrAddGuid(source.GetGuid(h));
            foreach (var h in source.Documents)
            {
                var d = source.GetDocument(h);
                target.AddDocument(target.GetOrAddDocumentName(source.GetString(d.Name)), Guid(d.HashAlgorithm), Blob(d.Hash), Guid(d.Language));
            }
            int symbolRow = 1;
            foreach (var method in methods.MethodDefinitions)
            {
                if (methods.GetMethodDefinition(method).RelativeVirtualAddress == 0)
                    target.AddMethodDebugInformation(default, default);
                else
                {
                    var d = source.GetMethodDebugInformation(MetadataTokens.MethodDebugInformationHandle(symbolRow++));
                    target.AddMethodDebugInformation(d.Document, Blob(d.SequencePointsBlob));
                }
                // StateMachineMethod refers to MethodDef IDs, not the compacted debug rows.
                var kickoff = source.GetMethodDebugInformation(method).GetStateMachineKickoffMethod();
                if (!kickoff.IsNil) target.AddStateMachineMethod(method, kickoff);
            }
            foreach (var h in source.LocalVariables)
            {
                var v = source.GetLocalVariable(h);
                target.AddLocalVariable(v.Attributes, v.Index, Text(v.Name));
            }
            foreach (var h in source.LocalConstants)
            {
                var c = source.GetLocalConstant(h);
                target.AddLocalConstant(Text(c.Name), Blob(c.Signature));
            }
            foreach (var h in source.ImportScopes)
            {
                var scope = source.GetImportScope(h);
                var blob = new BlobBuilder();
                foreach (var import in scope.GetImports())
                {
                    blob.WriteCompressedInteger((int)import.Kind);
                    void Alias() => blob.WriteCompressedInteger(MetadataTokens.GetHeapOffset(Blob(import.Alias)));
                    void Namespace() => blob.WriteCompressedInteger(MetadataTokens.GetHeapOffset(Blob(import.TargetNamespace)));
                    void Assembly() => blob.WriteCompressedInteger(MetadataTokens.GetRowNumber(import.TargetAssembly));
                    void Type() => blob.WriteCompressedInteger(CodedIndex.TypeDefOrRefOrSpec(import.TargetType));
                    switch (import.Kind)
                    {
                        case ImportDefinitionKind.ImportNamespace: Namespace(); break;
                        case ImportDefinitionKind.ImportAssemblyNamespace: Assembly(); Namespace(); break;
                        case ImportDefinitionKind.ImportType: Type(); break;
                        case ImportDefinitionKind.ImportXmlNamespace: Alias(); Namespace(); break;
                        case ImportDefinitionKind.ImportAssemblyReferenceAlias: Alias(); break;
                        case ImportDefinitionKind.AliasAssemblyReference: Alias(); Assembly(); break;
                        case ImportDefinitionKind.AliasNamespace: Alias(); Namespace(); break;
                        case ImportDefinitionKind.AliasAssemblyNamespace: Alias(); Assembly(); Namespace(); break;
                        case ImportDefinitionKind.AliasType: Alias(); Type(); break;
                        default: throw new InvalidOperationException("Unknown Portable PDB import kind.");
                    }
                }
                target.AddImportScope(scope.Parent, target.GetOrAddBlob(blob));
            }
            // List starts of empty scopes point at the next scope's first row,
            // or one past the table, not at a nil handle.
            var scopes = source.LocalScopes.ToArray();
            var starts = new Dictionary<LocalScopeHandle, (int Variables, int Constants)>();
            int variableStart = source.LocalVariables.Count + 1, constantStart = source.LocalConstants.Count + 1;
            for (int i = scopes.Length - 1; i >= 0; i--)
            {
                var scope = source.GetLocalScope(scopes[i]);
                var variable = scope.GetLocalVariables().FirstOrDefault();
                var constant = scope.GetLocalConstants().FirstOrDefault();
                if (!variable.IsNil) variableStart = MetadataTokens.GetRowNumber(variable);
                if (!constant.IsNil) constantStart = MetadataTokens.GetRowNumber(constant);
                starts.Add(scopes[i], (variableStart, constantStart));
            }
            foreach (var h in scopes)
            {
                var scope = source.GetLocalScope(h);
                var start = starts[h];
                target.AddLocalScope(scope.Method, scope.ImportScope, MetadataTokens.LocalVariableHandle(start.Variables),
                    MetadataTokens.LocalConstantHandle(start.Constants), scope.StartOffset, scope.Length);
            }
            foreach (var h in source.CustomDebugInformation)
            {
                var d = source.GetCustomDebugInformation(h);
                target.AddCustomDebugInformation(d.Parent, Guid(d.Kind), Blob(d.Value));
            }
            return target;
        }

        internal static void BindCodeView(byte[] image, PEReader pe, BlobContentId pdbId)
        {
            // MetadataBuilder serialization consumes its heap builders. Read the final
            // PE once, write PDB from its actual MethodDefs, then bind only the PDB ID.
            // No IL, metadata tokens, section sizes or source coordinates change here.
            var header = pe.PEHeaders.PEHeader;
            if (header == null || !pe.PEHeaders.TryGetDirectoryOffset(header.DebugTableDirectory, out int tableOffset))
                throw new InvalidOperationException("Missing PE debug directory.");
            var entries = pe.ReadDebugDirectory();
            for (int i = 0; i < entries.Length; i++)
            {
                var entry = entries[i];
                if (entry.Type != DebugDirectoryEntryType.CodeView) continue;
                if (entry.DataSize < 24 || BinaryPrimitives.ReadUInt32LittleEndian(image.AsSpan(entry.DataPointer, 4)) != 0x53445352)
                    throw new InvalidOperationException("Invalid Portable PDB CodeView record.");
                pdbId.Guid.TryWriteBytes(image.AsSpan(entry.DataPointer + 4, 16));
                // IMAGE_DEBUG_DIRECTORY: TimeDateStamp is at byte 4 of its 28-byte row.
                BinaryPrimitives.WriteUInt32LittleEndian(image.AsSpan(tableOffset + i * 28 + 4, 4), pdbId.Stamp);
                return;
            }
            throw new InvalidOperationException("Missing Portable PDB CodeView record.");
        }
    }
}
#endif
