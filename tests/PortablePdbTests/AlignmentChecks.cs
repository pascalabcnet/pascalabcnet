using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using PascalABCCompiler.NETGenerator;

internal static class AlignmentChecks
{
    internal static void Run()
    {
        var assembly = new MetadataBuilder();
        assembly.AddModule(0, assembly.GetOrAddString("Fixture.dll"), assembly.GetOrAddGuid(Guid.NewGuid()), default, default);
        assembly.AddTypeDefinition(TypeAttributes.NotPublic, default, assembly.GetOrAddString("<Module>"), default,
            MetadataTokens.FieldDefinitionHandle(1), MetadataTokens.MethodDefinitionHandle(1));
        var signature = assembly.GetOrAddBlob(new byte[] { 0, 0, 1 }); // static void()
        assembly.AddMethodDefinition(MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.PinvokeImpl,
            MethodImplAttributes.IL, assembly.GetOrAddString("Native"), signature, -1, MetadataTokens.ParameterHandle(1));
        assembly.AddMethodDefinition(MethodAttributes.Public | MethodAttributes.Static, MethodImplAttributes.IL,
            assembly.GetOrAddString("Managed"), signature, 0, MetadataTokens.ParameterHandle(1));

        var symbols = new MetadataBuilder();
        var document = symbols.AddDocument(symbols.GetOrAddDocumentName("Путь с пробелами.pas"), default, default, default);
        // Simulate .NET 10: Native has no debug row; only Managed gets one.
        symbols.AddMethodDebugInformation(document, symbols.GetOrAddBlob(new byte[] { 0, 0, 0, 1, 42, 1 }));
        symbols.AddLocalVariable(LocalVariableAttributes.None, 0, symbols.GetOrAddString("variable"));
        symbols.AddLocalConstant(symbols.GetOrAddString("constant"), symbols.GetOrAddBlob(new byte[] { 8, 42, 0, 0, 0 }));
        var imports = new BlobBuilder();
        var ns = symbols.GetOrAddBlob(System.Text.Encoding.UTF8.GetBytes("System"));
        var alias = symbols.GetOrAddBlob(System.Text.Encoding.UTF8.GetBytes("alias"));
        foreach (ImportDefinitionKind kind in Enum.GetValues<ImportDefinitionKind>())
        {
            imports.WriteCompressedInteger((int)kind);
            void Ns() => imports.WriteCompressedInteger(MetadataTokens.GetHeapOffset(ns));
            void Alias() => imports.WriteCompressedInteger(MetadataTokens.GetHeapOffset(alias));
            switch (kind)
            {
                case ImportDefinitionKind.ImportNamespace: Ns(); break;
                case ImportDefinitionKind.ImportAssemblyNamespace: imports.WriteCompressedInteger(1); Ns(); break;
                case ImportDefinitionKind.ImportType: imports.WriteCompressedInteger(CodedIndex.TypeDefOrRefOrSpec(MetadataTokens.TypeReferenceHandle(1))); break;
                case ImportDefinitionKind.ImportXmlNamespace: Alias(); Ns(); break;
                case ImportDefinitionKind.ImportAssemblyReferenceAlias: Alias(); break;
                case ImportDefinitionKind.AliasAssemblyReference: Alias(); imports.WriteCompressedInteger(1); break;
                case ImportDefinitionKind.AliasNamespace: Alias(); Ns(); break;
                case ImportDefinitionKind.AliasAssemblyNamespace: Alias(); imports.WriteCompressedInteger(1); Ns(); break;
                case ImportDefinitionKind.AliasType: Alias(); imports.WriteCompressedInteger(CodedIndex.TypeDefOrRefOrSpec(MetadataTokens.TypeReferenceHandle(1))); break;
            }
        }
        var importScope = symbols.AddImportScope(default, symbols.GetOrAddBlob(imports));
        var method = MetadataTokens.MethodDefinitionHandle(2);
        symbols.AddLocalScope(method, importScope, MetadataTokens.LocalVariableHandle(1), MetadataTokens.LocalConstantHandle(1), 0, 10);
        symbols.AddLocalScope(method, importScope, MetadataTokens.LocalVariableHandle(2), MetadataTokens.LocalConstantHandle(2), 1, 1);
        symbols.AddStateMachineMethod(method, MetadataTokens.MethodDefinitionHandle(1));
        var customKind = Guid.NewGuid();
        symbols.AddCustomDebugInformation(document, symbols.GetOrAddGuid(customKind), symbols.GetOrAddBlob(new byte[] { 1, 2, 3 }));

        var metadataImage = new BlobBuilder();
        new MetadataRootBuilder(assembly).Serialize(metadataImage, 1, 0);
        using var metadataProvider = MetadataReaderProvider.FromMetadataImage(metadataImage.ToImmutableArray());
        var methods = metadataProvider.GetMetadataReader();
        var aligned = PortablePdbAlignment.Align(methods, symbols, assembly.GetRowCounts(), default);
        Check(ReferenceEquals(aligned, PortablePdbAlignment.Align(methods, aligned, assembly.GetRowCounts(), default)), "aligned symbols are unchanged");
        var image = new BlobBuilder();
        new PortablePdbBuilder(aligned, assembly.GetRowCounts(), default).Serialize(image);
        using var provider = MetadataReaderProvider.FromPortablePdbImage(image.ToImmutableArray());
        var reader = provider.GetMetadataReader();
        Check(reader.MethodDebugInformation.Count == 2, "external method retains its empty row");
        Check(!reader.GetMethodDebugInformation(MetadataTokens.MethodDefinitionHandle(1)).GetSequencePoints().Any(), "external method has no sequence points");
        Check(reader.GetMethodDebugInformation(method).GetSequencePoints().Single().StartLine == 42, "managed method retains its exact source line");
        Check(reader.GetMethodDebugInformation(method).GetStateMachineKickoffMethod() == MetadataTokens.MethodDefinitionHandle(1), "state machine references retained");
        Check(reader.GetLocalScope(MetadataTokens.LocalScopeHandle(1)).GetLocalVariables().Count == 1 &&
            reader.GetLocalScope(MetadataTokens.LocalScopeHandle(2)).GetLocalVariables().Count == 0, "nonempty and empty local scopes retained");
        Check(reader.GetString(reader.GetLocalVariable(MetadataTokens.LocalVariableHandle(1)).Name) == "variable" &&
            reader.GetString(reader.GetLocalConstant(MetadataTokens.LocalConstantHandle(1)).Name) == "constant", "local names retained");
        var restoredImports = reader.GetImportScope(MetadataTokens.ImportScopeHandle(1)).GetImports().ToArray();
        Check(restoredImports.Length == 9, "all nine import kinds retained");
        Check(System.Text.Encoding.UTF8.GetString(reader.GetBlobBytes(restoredImports[0].TargetNamespace)) == "System" &&
            System.Text.Encoding.UTF8.GetString(reader.GetBlobBytes(restoredImports[3].Alias)) == "alias", "import blob heap references remapped");
        var custom = reader.GetCustomDebugInformation(reader.CustomDebugInformation.Single());
        Check(reader.GetGuid(custom.Kind) == customKind && reader.GetBlobBytes(custom.Value).SequenceEqual(new byte[] { 1, 2, 3 }), "custom debug data retained");
        Console.WriteLine("PASS PDB alignment preserves documents, imports, locals, scopes, state machines and custom debug data");
    }

    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
    }
}
