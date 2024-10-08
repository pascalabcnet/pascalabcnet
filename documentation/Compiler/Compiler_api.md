# <a id="PascalABCCompiler_Compiler"></a> Class Compiler

Namespace: [PascalABCCompiler](PascalABCCompiler.md)  
Assembly: Compiler.dll  

```csharp
public class Compiler : MarshalByRefObject, ICompiler
```

#### Inheritance

[object](https://learn.microsoft.com/dotnet/api/system.object) ← 
[MarshalByRefObject](https://learn.microsoft.com/dotnet/api/system.marshalbyrefobject) ← 
[Compiler](PascalABCCompiler.Compiler.md)

#### Implements

[ICompiler](PascalABCCompiler.ICompiler.md)

#### Inherited Members

[MarshalByRefObject.MemberwiseClone\(bool\)](https://learn.microsoft.com/dotnet/api/system.marshalbyrefobject.memberwiseclone), 
[MarshalByRefObject.GetLifetimeService\(\)](https://learn.microsoft.com/dotnet/api/system.marshalbyrefobject.getlifetimeservice), 
[MarshalByRefObject.InitializeLifetimeService\(\)](https://learn.microsoft.com/dotnet/api/system.marshalbyrefobject.initializelifetimeservice), 
[MarshalByRefObject.CreateObjRef\(Type\)](https://learn.microsoft.com/dotnet/api/system.marshalbyrefobject.createobjref), 
[object.ToString\(\)](https://learn.microsoft.com/dotnet/api/system.object.tostring), 
[object.Equals\(object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\)), 
[object.Equals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\-system\-object\)), 
[object.ReferenceEquals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.referenceequals), 
[object.GetHashCode\(\)](https://learn.microsoft.com/dotnet/api/system.object.gethashcode), 
[object.GetType\(\)](https://learn.microsoft.com/dotnet/api/system.object.gettype), 
[object.MemberwiseClone\(\)](https://learn.microsoft.com/dotnet/api/system.object.memberwiseclone)

## Constructors

### <a id="PascalABCCompiler_Compiler__cctor"></a> Compiler\(\)

```csharp
private static Compiler()
```

### <a id="PascalABCCompiler_Compiler__ctor"></a> Compiler\(\)

```csharp
public Compiler()
```

### <a id="PascalABCCompiler_Compiler__ctor_PascalABCCompiler_ICompiler_PascalABCCompiler_SourceFilesProviderDelegate_PascalABCCompiler_ChangeCompilerStateEventDelegate_"></a> Compiler\(ICompiler, SourceFilesProviderDelegate, ChangeCompilerStateEventDelegate\)

```csharp
public Compiler(ICompiler comp, SourceFilesProviderDelegate SourceFilesProvider, ChangeCompilerStateEventDelegate ChangeCompilerState)
```

#### Parameters

`comp` [ICompiler](PascalABCCompiler.ICompiler.md)

`SourceFilesProvider` SourceFilesProviderDelegate

`ChangeCompilerState` [ChangeCompilerStateEventDelegate](PascalABCCompiler.ChangeCompilerStateEventDelegate.md)

### <a id="PascalABCCompiler_Compiler__ctor_PascalABCCompiler_SourceFilesProviderDelegate_PascalABCCompiler_ChangeCompilerStateEventDelegate_"></a> Compiler\(SourceFilesProviderDelegate, ChangeCompilerStateEventDelegate\)

```csharp
public Compiler(SourceFilesProviderDelegate SourceFilesProvider, ChangeCompilerStateEventDelegate ChangeCompilerState)
```

#### Parameters

`SourceFilesProvider` SourceFilesProviderDelegate

`ChangeCompilerState` [ChangeCompilerStateEventDelegate](PascalABCCompiler.ChangeCompilerStateEventDelegate.md)

## Fields

### <a id="PascalABCCompiler_Compiler_BadNodesInSyntaxTree"></a> BadNodesInSyntaxTree

```csharp
private Hashtable BadNodesInSyntaxTree
```

#### Field Value

 [Hashtable](https://learn.microsoft.com/dotnet/api/system.collections.hashtable)

### <a id="PascalABCCompiler_Compiler_CodeGeneratorsController"></a> CodeGeneratorsController

```csharp
public Controller CodeGeneratorsController
```

#### Field Value

 Controller

### <a id="PascalABCCompiler_Compiler_CompiledVariables"></a> CompiledVariables

```csharp
public List<var_definition_node> CompiledVariables
```

#### Field Value

 [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<var\_definition\_node\>

### <a id="PascalABCCompiler_Compiler_DLLCache"></a> DLLCache

```csharp
private Dictionary<string, CompilationUnit> DLLCache
```

#### Field Value

 [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<[string](https://learn.microsoft.com/dotnet/api/system.string), [CompilationUnit](PascalABCCompiler.CompilationUnit.md)\>

### <a id="PascalABCCompiler_Compiler_PCUReadersAndWritersClosed"></a> PCUReadersAndWritersClosed

```csharp
private bool PCUReadersAndWritersClosed
```

#### Field Value

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="PascalABCCompiler_Compiler_RecompileList"></a> RecompileList

```csharp
public Hashtable RecompileList
```

#### Field Value

 [Hashtable](https://learn.microsoft.com/dotnet/api/system.collections.hashtable)

### <a id="PascalABCCompiler_Compiler_StandardModules"></a> StandardModules

```csharp
private List<string> StandardModules
```

#### Field Value

 [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<[string](https://learn.microsoft.com/dotnet/api/system.string)\>

### <a id="PascalABCCompiler_Compiler_SyntaxTreeToSemanticTreeConverter"></a> SyntaxTreeToSemanticTreeConverter

```csharp
public SyntaxTreeToSemanticTreeConverter SyntaxTreeToSemanticTreeConverter
```

#### Field Value

 [SyntaxTreeToSemanticTreeConverter](PascalABCCompiler.TreeConverter.SyntaxTreeToSemanticTreeConverter.md)

### <a id="PascalABCCompiler_Compiler_UnitsToCompileDelayedList"></a> UnitsToCompileDelayedList

список отложенной компиляции реализации (она будет откомпилирована в Compile, а не в СompileUnit)

```csharp
private List<CompilationUnit> UnitsToCompileDelayedList
```

#### Field Value

 [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<[CompilationUnit](PascalABCCompiler.CompilationUnit.md)\>

### <a id="PascalABCCompiler_Compiler_UnitsTopologicallySortedList"></a> UnitsTopologicallySortedList

```csharp
public List<CompilationUnit> UnitsTopologicallySortedList
```

#### Field Value

 [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<[CompilationUnit](PascalABCCompiler.CompilationUnit.md)\>

### <a id="PascalABCCompiler_Compiler__clear_after_compilation"></a> \_clear\_after\_compilation

```csharp
private bool _clear_after_compilation
```

#### Field Value

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="PascalABCCompiler_Compiler_assemblyResolveScope"></a> assemblyResolveScope

```csharp
private AssemblyResolveScope assemblyResolveScope
```

#### Field Value

 AssemblyResolveScope

### <a id="PascalABCCompiler_Compiler_beginOffset"></a> beginOffset

Начало основной программы

```csharp
public int beginOffset
```

#### Field Value

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

### <a id="PascalABCCompiler_Compiler_currentCompilationUnit"></a> currentCompilationUnit

```csharp
private CompilationUnit currentCompilationUnit
```

#### Field Value

 [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

### <a id="PascalABCCompiler_Compiler_errorsList"></a> errorsList

```csharp
private List<Error> errorsList
```

#### Field Value

 [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<Error\>

### <a id="PascalABCCompiler_Compiler_firstCompilationUnit"></a> firstCompilationUnit

```csharp
private CompilationUnit firstCompilationUnit
```

#### Field Value

 [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

### <a id="PascalABCCompiler_Compiler_internalDebug"></a> internalDebug

```csharp
private CompilerInternalDebug internalDebug
```

#### Field Value

 [CompilerInternalDebug](PascalABCCompiler.CompilerInternalDebug.md)

### <a id="PascalABCCompiler_Compiler_linesCompiled"></a> linesCompiled

```csharp
private uint linesCompiled
```

#### Field Value

 [uint](https://learn.microsoft.com/dotnet/api/system.uint32)

### <a id="PascalABCCompiler_Compiler_pABCCodeHealth"></a> pABCCodeHealth

```csharp
private int pABCCodeHealth
```

#### Field Value

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

### <a id="PascalABCCompiler_Compiler_pcuCompilationUnits"></a> pcuCompilationUnits

```csharp
private static Dictionary<string, CompilationUnit> pcuCompilationUnits
```

#### Field Value

 [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<[string](https://learn.microsoft.com/dotnet/api/system.string), [CompilationUnit](PascalABCCompiler.CompilationUnit.md)\>

### <a id="PascalABCCompiler_Compiler_project"></a> project

```csharp
private ProjectInfo project
```

#### Field Value

 [ProjectInfo](PascalABCCompiler.ProjectInfo.md)

### <a id="PascalABCCompiler_Compiler_semanticTree"></a> semanticTree

```csharp
private program_node semanticTree
```

#### Field Value

 program\_node

### <a id="PascalABCCompiler_Compiler_semanticTreeConvertersController"></a> semanticTreeConvertersController

```csharp
private SemanticTreeConvertersController semanticTreeConvertersController
```

#### Field Value

 [SemanticTreeConvertersController](PascalABCCompiler.SemanticTreeConverters.SemanticTreeConvertersController.md)

### <a id="PascalABCCompiler_Compiler_sourceFilesProvider"></a> sourceFilesProvider

```csharp
private SourceFilesProviderDelegate sourceFilesProvider
```

#### Field Value

 SourceFilesProviderDelegate

### <a id="PascalABCCompiler_Compiler_standartAssemblyPath"></a> standartAssemblyPath

```csharp
private static string standartAssemblyPath
```

#### Field Value

 [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_standart_assembly_dict"></a> standart\_assembly\_dict

```csharp
public static Dictionary<string, string> standart_assembly_dict
```

#### Field Value

 [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<[string](https://learn.microsoft.com/dotnet/api/system.string), [string](https://learn.microsoft.com/dotnet/api/system.string)\>

### <a id="PascalABCCompiler_Compiler_state"></a> state

```csharp
private CompilerState state
```

#### Field Value

 [CompilerState](PascalABCCompiler.CompilerState.md)

### <a id="PascalABCCompiler_Compiler_supportedProjectFiles"></a> supportedProjectFiles

```csharp
private SupportedSourceFile[] supportedProjectFiles
```

#### Field Value

 [SupportedSourceFile](PascalABCCompiler.SupportedSourceFile.md)\[\]

### <a id="PascalABCCompiler_Compiler_supportedSourceFiles"></a> supportedSourceFiles

```csharp
private SupportedSourceFile[] supportedSourceFiles
```

#### Field Value

 [SupportedSourceFile](PascalABCCompiler.SupportedSourceFile.md)\[\]

### <a id="PascalABCCompiler_Compiler_unitTable"></a> unitTable

```csharp
private CompilationUnitHashTable unitTable
```

#### Field Value

 [CompilationUnitHashTable](PascalABCCompiler.CompilationUnitHashTable.md)

### <a id="PascalABCCompiler_Compiler_varBeginOffset"></a> varBeginOffset

Положение первых переменных в пространстве имен основной программы

```csharp
public int varBeginOffset
```

#### Field Value

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

### <a id="PascalABCCompiler_Compiler_warnings"></a> warnings

```csharp
private List<CompilerWarning> warnings
```

#### Field Value

 [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<CompilerWarning\>

## Properties

### <a id="PascalABCCompiler_Compiler_Banner"></a> Banner

```csharp
public static string Banner { get; }
```

#### Property Value

 [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_BeginOffset"></a> BeginOffset

```csharp
public int BeginOffset { get; }
```

#### Property Value

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

### <a id="PascalABCCompiler_Compiler_ClearAfterCompilation"></a> ClearAfterCompilation

```csharp
public bool ClearAfterCompilation { get; set; }
```

#### Property Value

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="PascalABCCompiler_Compiler_CompilerOptions"></a> CompilerOptions

```csharp
public CompilerOptions CompilerOptions { get; set; }
```

#### Property Value

 [CompilerOptions](PascalABCCompiler.CompilerOptions.md)

### <a id="PascalABCCompiler_Compiler_CompilerType"></a> CompilerType

```csharp
public CompilerType CompilerType { get; }
```

#### Property Value

 [CompilerType](PascalABCCompiler.CompilerType.md)

### <a id="PascalABCCompiler_Compiler_ErrorsList"></a> ErrorsList

```csharp
public List<Error> ErrorsList { get; }
```

#### Property Value

 [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<Error\>

### <a id="PascalABCCompiler_Compiler_GetUnitFileNameCache"></a> GetUnitFileNameCache

```csharp
public Dictionary<Tuple<string, string>, string> GetUnitFileNameCache { get; }
```

#### Property Value

 [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<[Tuple](https://learn.microsoft.com/dotnet/api/system.tuple\-2)<[string](https://learn.microsoft.com/dotnet/api/system.string), [string](https://learn.microsoft.com/dotnet/api/system.string)\>, [string](https://learn.microsoft.com/dotnet/api/system.string)\>

### <a id="PascalABCCompiler_Compiler_InternalDebug"></a> InternalDebug

```csharp
public CompilerInternalDebug InternalDebug { get; set; }
```

#### Property Value

 [CompilerInternalDebug](PascalABCCompiler.CompilerInternalDebug.md)

### <a id="PascalABCCompiler_Compiler_LanguageProvider"></a> LanguageProvider

```csharp
private LanguageProvider LanguageProvider { get; }
```

#### Property Value

 LanguageProvider

### <a id="PascalABCCompiler_Compiler_LinesCompiled"></a> LinesCompiled

```csharp
public uint LinesCompiled { get; }
```

#### Property Value

 [uint](https://learn.microsoft.com/dotnet/api/system.uint32)

### <a id="PascalABCCompiler_Compiler_PABCCodeHealth"></a> PABCCodeHealth

Здоровье кода на всякий случай выносим в интерфейс компилятора
Реально оно будет использоваться только при запуске из под оболочки (Remote Compiler)

```csharp
public int PABCCodeHealth { get; }
```

#### Property Value

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

### <a id="PascalABCCompiler_Compiler_PCUFileNamesDictionary"></a> PCUFileNamesDictionary

```csharp
public Dictionary<Tuple<string, string>, Tuple<string, int>> PCUFileNamesDictionary { get; }
```

#### Property Value

 [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<[Tuple](https://learn.microsoft.com/dotnet/api/system.tuple\-2)<[string](https://learn.microsoft.com/dotnet/api/system.string), [string](https://learn.microsoft.com/dotnet/api/system.string)\>, [Tuple](https://learn.microsoft.com/dotnet/api/system.tuple\-2)<[string](https://learn.microsoft.com/dotnet/api/system.string), [int](https://learn.microsoft.com/dotnet/api/system.int32)\>\>

### <a id="PascalABCCompiler_Compiler_SemanticTree"></a> SemanticTree

```csharp
public IProgramNode SemanticTree { get; }
```

#### Property Value

 IProgramNode

### <a id="PascalABCCompiler_Compiler_SemanticTreeConvertersController"></a> SemanticTreeConvertersController

```csharp
public SemanticTreeConvertersController SemanticTreeConvertersController { get; }
```

#### Property Value

 [SemanticTreeConvertersController](PascalABCCompiler.SemanticTreeConverters.SemanticTreeConvertersController.md)

### <a id="PascalABCCompiler_Compiler_ShortVersion"></a> ShortVersion

```csharp
public static string ShortVersion { get; }
```

#### Property Value

 [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_SourceFileNamesDictionary"></a> SourceFileNamesDictionary

```csharp
public Dictionary<Tuple<string, string>, Tuple<string, int>> SourceFileNamesDictionary { get; }
```

#### Property Value

 [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<[Tuple](https://learn.microsoft.com/dotnet/api/system.tuple\-2)<[string](https://learn.microsoft.com/dotnet/api/system.string), [string](https://learn.microsoft.com/dotnet/api/system.string)\>, [Tuple](https://learn.microsoft.com/dotnet/api/system.tuple\-2)<[string](https://learn.microsoft.com/dotnet/api/system.string), [int](https://learn.microsoft.com/dotnet/api/system.int32)\>\>

### <a id="PascalABCCompiler_Compiler_SourceFilesProvider"></a> SourceFilesProvider

```csharp
public SourceFilesProviderDelegate SourceFilesProvider { get; }
```

#### Property Value

 SourceFilesProviderDelegate

### <a id="PascalABCCompiler_Compiler_State"></a> State

```csharp
public CompilerState State { get; }
```

#### Property Value

 [CompilerState](PascalABCCompiler.CompilerState.md)

### <a id="PascalABCCompiler_Compiler_SupportedProjectFiles"></a> SupportedProjectFiles

```csharp
public SupportedSourceFile[] SupportedProjectFiles { get; }
```

#### Property Value

 [SupportedSourceFile](PascalABCCompiler.SupportedSourceFile.md)\[\]

### <a id="PascalABCCompiler_Compiler_SupportedSourceFiles"></a> SupportedSourceFiles

```csharp
public SupportedSourceFile[] SupportedSourceFiles { get; set; }
```

#### Property Value

 [SupportedSourceFile](PascalABCCompiler.SupportedSourceFile.md)\[\]

### <a id="PascalABCCompiler_Compiler_UnitTable"></a> UnitTable

```csharp
public CompilationUnitHashTable UnitTable { get; }
```

#### Property Value

 [CompilationUnitHashTable](PascalABCCompiler.CompilationUnitHashTable.md)

### <a id="PascalABCCompiler_Compiler_VarBeginOffset"></a> VarBeginOffset

```csharp
public int VarBeginOffset { get; }
```

#### Property Value

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

### <a id="PascalABCCompiler_Compiler_Version"></a> Version

```csharp
public static string Version { get; }
```

#### Property Value

 [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_VersionDateTime"></a> VersionDateTime

```csharp
public static DateTime VersionDateTime { get; }
```

#### Property Value

 [DateTime](https://learn.microsoft.com/dotnet/api/system.datetime)

### <a id="PascalABCCompiler_Compiler_Warnings"></a> Warnings

```csharp
public List<CompilerWarning> Warnings { get; }
```

#### Property Value

 [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<CompilerWarning\>

## Methods

### <a id="PascalABCCompiler_Compiler_AddCodeGenerationErrorToErrorList_System_Exception_"></a> AddCodeGenerationErrorToErrorList\(Exception\)

```csharp
private void AddCodeGenerationErrorToErrorList(Exception err)
```

#### Parameters

`err` [Exception](https://learn.microsoft.com/dotnet/api/system.exception)

### <a id="PascalABCCompiler_Compiler_AddCurrentUnitAndItsReferencesToUsesLists_PascalABCCompiler_TreeRealization_unit_node_list_System_Collections_Generic_Dictionary_PascalABCCompiler_TreeRealization_unit_node_PascalABCCompiler_CompilationUnit__PascalABCCompiler_SyntaxTree_unit_or_namespace_PascalABCCompiler_CompilationUnit_PascalABCCompiler_TreeRealization_unit_node_list_"></a> AddCurrentUnitAndItsReferencesToUsesLists\(unit\_node\_list, Dictionary<unit\_node, CompilationUnit\>, unit\_or\_namespace, CompilationUnit, unit\_node\_list\)

```csharp
private void AddCurrentUnitAndItsReferencesToUsesLists(unit_node_list unitsFromUsesSection, Dictionary<unit_node, CompilationUnit> directUnitsFromUsesSection, unit_or_namespace currentUnitNode, CompilationUnit currentUnit, unit_node_list references)
```

#### Parameters

`unitsFromUsesSection` unit\_node\_list

`directUnitsFromUsesSection` [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<unit\_node, [CompilationUnit](PascalABCCompiler.CompilationUnit.md)\>

`currentUnitNode` unit\_or\_namespace

`currentUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

`references` unit\_node\_list

### <a id="PascalABCCompiler_Compiler_AddDeclarationsAndReferencedUnitsToNamespaces_System_Collections_Generic_List_PascalABCCompiler_SyntaxTree_unit_or_namespace__System_String_PascalABCCompiler_SyntaxTree_unit_module_PascalABCCompiler_SyntaxTree_syntax_namespace_node_"></a> AddDeclarationsAndReferencedUnitsToNamespaces\(List<unit\_or\_namespace\>, string, unit\_module, syntax\_namespace\_node\)

```csharp
private void AddDeclarationsAndReferencedUnitsToNamespaces(List<unit_or_namespace> namespace_modules, string file, unit_module unitModule, syntax_namespace_node namespaceNode)
```

#### Parameters

`namespace_modules` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<unit\_or\_namespace\>

`file` [string](https://learn.microsoft.com/dotnet/api/system.string)

`unitModule` unit\_module

`namespaceNode` syntax\_namespace\_node

### <a id="PascalABCCompiler_Compiler_AddDocumentationToNodes_PascalABCCompiler_CompilationUnit_System_String_"></a> AddDocumentationToNodes\(CompilationUnit, string\)

```csharp
private Dictionary<syntax_tree_node, string> AddDocumentationToNodes(CompilationUnit currentUnit, string text)
```

#### Parameters

`currentUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

`text` [string](https://learn.microsoft.com/dotnet/api/system.string)

#### Returns

 [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<syntax\_tree\_node, [string](https://learn.microsoft.com/dotnet/api/system.string)\>

### <a id="PascalABCCompiler_Compiler_AddErrorToErrorListConsideringPosition_PascalABCCompiler_Errors_Error_"></a> AddErrorToErrorListConsideringPosition\(Error\)

```csharp
private void AddErrorToErrorListConsideringPosition(Error err)
```

#### Parameters

`err` Error

### <a id="PascalABCCompiler_Compiler_AddInternalErrorToErrorList_PascalABCCompiler_Errors_CompilerInternalError_"></a> AddInternalErrorToErrorList\(CompilerInternalError\)

```csharp
private void AddInternalErrorToErrorList(CompilerInternalError internalError)
```

#### Parameters

`internalError` CompilerInternalError

### <a id="PascalABCCompiler_Compiler_AddNamespacesToMainDefinitions_PascalABCCompiler_SyntaxTree_unit_module_PascalABCCompiler_SyntaxTree_program_module_System_Collections_Generic_Dictionary_System_String_PascalABCCompiler_SyntaxTree_syntax_namespace_node__"></a> AddNamespacesToMainDefinitions\(unit\_module, program\_module, Dictionary<string, syntax\_namespace\_node\>\)

```csharp
private void AddNamespacesToMainDefinitions(unit_module mainLibrary, program_module main_program, Dictionary<string, syntax_namespace_node> namespaces)
```

#### Parameters

`mainLibrary` unit\_module

`main_program` program\_module

`namespaces` [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<[string](https://learn.microsoft.com/dotnet/api/system.string), syntax\_namespace\_node\>

### <a id="PascalABCCompiler_Compiler_AddNamespacesToMainUsesList_PascalABCCompiler_SyntaxTree_unit_module_PascalABCCompiler_SyntaxTree_program_module_System_Collections_Generic_List_PascalABCCompiler_SyntaxTree_unit_or_namespace__"></a> AddNamespacesToMainUsesList\(unit\_module, program\_module, List<unit\_or\_namespace\>\)

```csharp
private void AddNamespacesToMainUsesList(unit_module mainLibrary, program_module main_program, List<unit_or_namespace> namespaceModules)
```

#### Parameters

`mainLibrary` unit\_module

`main_program` program\_module

`namespaceModules` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<unit\_or\_namespace\>

### <a id="PascalABCCompiler_Compiler_AddNamespacesToUsingList_PascalABCCompiler_TreeRealization_using_namespace_list_System_Collections_Generic_List_PascalABCCompiler_SyntaxTree_unit_or_namespace__System_Boolean_System_Collections_Generic_Dictionary_System_String_PascalABCCompiler_SyntaxTree_syntax_namespace_node__"></a> AddNamespacesToUsingList\(using\_namespace\_list, List<unit\_or\_namespace\>, bool, Dictionary<string, syntax\_namespace\_node\>\)

```csharp
public void AddNamespacesToUsingList(using_namespace_list usingList, List<unit_or_namespace> possibleNamespaces, bool mightContainUnits, Dictionary<string, syntax_namespace_node> namespaces)
```

#### Parameters

`usingList` using\_namespace\_list

`possibleNamespaces` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<unit\_or\_namespace\>

`mightContainUnits` [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

`namespaces` [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<[string](https://learn.microsoft.com/dotnet/api/system.string), syntax\_namespace\_node\>

### <a id="PascalABCCompiler_Compiler_AddNamespacesToUsingList_PascalABCCompiler_TreeRealization_using_namespace_list_PascalABCCompiler_SyntaxTree_using_list_"></a> AddNamespacesToUsingList\(using\_namespace\_list, using\_list\)

```csharp
public void AddNamespacesToUsingList(using_namespace_list using_list, using_list ul)
```

#### Parameters

`using_list` using\_namespace\_list

`ul` using\_list

### <a id="PascalABCCompiler_Compiler_AddReferencesToNetSystemLibraries_PascalABCCompiler_CompilationUnit_System_Collections_Generic_List_PascalABCCompiler_TreeRealization_compiler_directive__"></a> AddReferencesToNetSystemLibraries\(CompilationUnit, List<compiler\_directive\>\)

Добавляет ссылки на стандартные системные dll .NET - версия с директивами уровня семантики

```csharp
private void AddReferencesToNetSystemLibraries(CompilationUnit compilationUnit, List<compiler_directive> directives)
```

#### Parameters

`compilationUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

`directives` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<compiler\_directive\>

### <a id="PascalABCCompiler_Compiler_AddReferencesToNetSystemLibraries_PascalABCCompiler_CompilationUnit_System_Collections_Generic_List_PascalABCCompiler_SyntaxTree_compiler_directive__"></a> AddReferencesToNetSystemLibraries\(CompilationUnit, List<compiler\_directive\>\)

Добавляет ссылки на стандартные системные dll .NET - версия с директивами уровня синтаксиса

```csharp
private void AddReferencesToNetSystemLibraries(CompilationUnit compilationUnit, List<compiler_directive> directives)
```

#### Parameters

`compilationUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

`directives` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<compiler\_directive\>

### <a id="PascalABCCompiler_Compiler_AddStandardUnitsToInterfaceUsesSection_PascalABCCompiler_CompilationUnit_"></a> AddStandardUnitsToInterfaceUsesSection\(CompilationUnit\)

```csharp
public void AddStandardUnitsToInterfaceUsesSection(CompilationUnit currentUnit)
```

#### Parameters

`currentUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

### <a id="PascalABCCompiler_Compiler_AddWarnings_System_Collections_Generic_List_PascalABCCompiler_Errors_CompilerWarning__"></a> AddWarnings\(List<CompilerWarning\>\)

```csharp
public void AddWarnings(List<CompilerWarning> WarningList)
```

#### Parameters

`WarningList` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<CompilerWarning\>

### <a id="PascalABCCompiler_Compiler_AsyncClosePCUWriters"></a> AsyncClosePCUWriters\(\)

```csharp
private void AsyncClosePCUWriters()
```

### <a id="PascalABCCompiler_Compiler_CalculateLinesCompiled_System_Collections_Generic_List_PascalABCCompiler_Errors_Error__PascalABCCompiler_SyntaxTree_compilation_unit_"></a> CalculateLinesCompiled\(List<Error\>, compilation\_unit\)

```csharp
private void CalculateLinesCompiled(List<Error> errorList, compilation_unit unitSyntaxTree)
```

#### Parameters

`errorList` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<Error\>

`unitSyntaxTree` compilation\_unit

### <a id="PascalABCCompiler_Compiler_CalculatePascalProgramHealth_PascalABCCompiler_SyntaxTree_compilation_unit_"></a> CalculatePascalProgramHealth\(compilation\_unit\)

```csharp
private void CalculatePascalProgramHealth(compilation_unit unitSyntaxTree)
```

#### Parameters

`unitSyntaxTree` compilation\_unit

### <a id="PascalABCCompiler_Compiler_ChangeCompilerStateEvent_PascalABCCompiler_ICompiler_PascalABCCompiler_CompilerState_System_String_"></a> ChangeCompilerStateEvent\(ICompiler, CompilerState, string\)

```csharp
private void ChangeCompilerStateEvent(ICompiler sender, CompilerState State, string FileName)
```

#### Parameters

`sender` [ICompiler](PascalABCCompiler.ICompiler.md)

`State` [CompilerState](PascalABCCompiler.CompilerState.md)

`FileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_CheckErrorsAndThrowTheFirstOne"></a> CheckErrorsAndThrowTheFirstOne\(\)

```csharp
private void CheckErrorsAndThrowTheFirstOne()
```

### <a id="PascalABCCompiler_Compiler_CheckForDuplicatesInUsesSection_System_Collections_Generic_List_PascalABCCompiler_SyntaxTree_unit_or_namespace__"></a> CheckForDuplicatesInUsesSection\(List<unit\_or\_namespace\>\)

Бросает ошибку если находит дупликаты в секции uses

```csharp
private void CheckForDuplicatesInUsesSection(List<unit_or_namespace> usesList)
```

#### Parameters

`usesList` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<unit\_or\_namespace\>

### <a id="PascalABCCompiler_Compiler_CheckForRTLErrorsAndClearAllErrorsIfFound"></a> CheckForRTLErrorsAndClearAllErrorsIfFound\(\)

```csharp
private bool CheckForRTLErrorsAndClearAllErrorsIfFound()
```

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="PascalABCCompiler_Compiler_CheckPathValid_System_String_"></a> CheckPathValid\(string\)

```csharp
public static bool CheckPathValid(string path)
```

#### Parameters

`path` [string](https://learn.microsoft.com/dotnet/api/system.string)

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="PascalABCCompiler_Compiler_ClearAll_System_Boolean_"></a> ClearAll\(bool\)

```csharp
public void ClearAll(bool close_pcu = true)
```

#### Parameters

`close_pcu` [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="PascalABCCompiler_Compiler_ClosePCUReadersAndWriters"></a> ClosePCUReadersAndWriters\(\)

```csharp
private void ClosePCUReadersAndWriters()
```

### <a id="PascalABCCompiler_Compiler_ClosePCUWriters"></a> ClosePCUWriters\(\)

```csharp
private void ClosePCUWriters()
```

### <a id="PascalABCCompiler_Compiler_CombinePathsRelatively_System_String_System_String_"></a> CombinePathsRelatively\(string, string\)

```csharp
public static string CombinePathsRelatively(string path1, string path2)
```

#### Parameters

`path1` [string](https://learn.microsoft.com/dotnet/api/system.string)

`path2` [string](https://learn.microsoft.com/dotnet/api/system.string)

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_Compile_PascalABCCompiler_CompilerOptions_"></a> Compile\(CompilerOptions\)

```csharp
public string Compile(CompilerOptions CompilerOptions)
```

#### Parameters

`CompilerOptions` [CompilerOptions](PascalABCCompiler.CompilerOptions.md)

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_Compile"></a> Compile\(\)

```csharp
public string Compile()
```

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_CompileCS"></a> CompileCS\(\)

```csharp
public string CompileCS()
```

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_CompileCurrentUnitImplementation_System_String_PascalABCCompiler_CompilationUnit_System_Collections_Generic_Dictionary_PascalABCCompiler_SyntaxTree_syntax_tree_node_System_String__"></a> CompileCurrentUnitImplementation\(string, CompilationUnit, Dictionary<syntax\_tree\_node, string\>\)

```csharp
private void CompileCurrentUnitImplementation(string UnitFileName, CompilationUnit currentUnit, Dictionary<syntax_tree_node, string> docs)
```

#### Parameters

`UnitFileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

`currentUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

`docs` [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<syntax\_tree\_node, [string](https://learn.microsoft.com/dotnet/api/system.string)\>

### <a id="PascalABCCompiler_Compiler_CompileCurrentUnitInterface_System_String_PascalABCCompiler_CompilationUnit_System_Collections_Generic_Dictionary_PascalABCCompiler_SyntaxTree_syntax_tree_node_System_String__"></a> CompileCurrentUnitInterface\(string, CompilationUnit, Dictionary<syntax\_tree\_node, string\>\)

```csharp
private void CompileCurrentUnitInterface(string UnitFileName, CompilationUnit currentUnit, Dictionary<syntax_tree_node, string> docs)
```

#### Parameters

`UnitFileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

`currentUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

`docs` [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<syntax\_tree\_node, [string](https://learn.microsoft.com/dotnet/api/system.string)\>

### <a id="PascalABCCompiler_Compiler_CompileImplementationDependencies_System_String_PascalABCCompiler_CompilationUnit_System_Collections_Generic_List_PascalABCCompiler_SyntaxTree_unit_or_namespace__System_Collections_Generic_Dictionary_System_String_PascalABCCompiler_SyntaxTree_syntax_namespace_node__PascalABCCompiler_TreeRealization_common_unit_node_System_Boolean__"></a> CompileImplementationDependencies\(string, CompilationUnit, List<unit\_or\_namespace\>, Dictionary<string, syntax\_namespace\_node\>, common\_unit\_node, out bool\)

Компилирует модули из секции uses текущего модуля реализации рекурсивно

```csharp
private void CompileImplementationDependencies(string currentPath, CompilationUnit currentUnit, List<unit_or_namespace> implementationUsesList, Dictionary<string, syntax_namespace_node> namespaces, common_unit_node commonUnitNode, out bool shouldReturnCurrentUnit)
```

#### Parameters

`currentPath` [string](https://learn.microsoft.com/dotnet/api/system.string)

`currentUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

`implementationUsesList` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<unit\_or\_namespace\>

`namespaces` [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<[string](https://learn.microsoft.com/dotnet/api/system.string), syntax\_namespace\_node\>

`commonUnitNode` common\_unit\_node

`shouldReturnCurrentUnit` [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="PascalABCCompiler_Compiler_CompileInterfaceDependencies_PascalABCCompiler_TreeRealization_unit_node_list_System_Collections_Generic_Dictionary_PascalABCCompiler_TreeRealization_unit_node_PascalABCCompiler_CompilationUnit__PascalABCCompiler_SyntaxTree_unit_or_namespace_System_String_System_String_PascalABCCompiler_CompilationUnit_System_Collections_Generic_List_PascalABCCompiler_SyntaxTree_unit_or_namespace__PascalABCCompiler_TreeRealization_unit_node_list_System_Collections_Generic_Dictionary_System_String_PascalABCCompiler_SyntaxTree_syntax_namespace_node__System_Boolean__"></a> CompileInterfaceDependencies\(unit\_node\_list, Dictionary<unit\_node, CompilationUnit\>, unit\_or\_namespace, string, string, CompilationUnit, List<unit\_or\_namespace\>, unit\_node\_list, Dictionary<string, syntax\_namespace\_node\>, out bool\)

Компилирует модули из секции uses интерфейса текущего модуля рекурсивно

```csharp
private void CompileInterfaceDependencies(unit_node_list unitsFromUsesSection, Dictionary<unit_node, CompilationUnit> directUnitsFromUsesSection, unit_or_namespace currentUnitNode, string unitFileName, string currentPath, CompilationUnit currentUnit, List<unit_or_namespace> interfaceUsesList, unit_node_list references, Dictionary<string, syntax_namespace_node> namespaces, out bool shouldReturnCurrentUnit)
```

#### Parameters

`unitsFromUsesSection` unit\_node\_list

`directUnitsFromUsesSection` [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<unit\_node, [CompilationUnit](PascalABCCompiler.CompilationUnit.md)\>

`currentUnitNode` unit\_or\_namespace

`unitFileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

`currentPath` [string](https://learn.microsoft.com/dotnet/api/system.string)

`currentUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

`interfaceUsesList` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<unit\_or\_namespace\>

`references` unit\_node\_list

`namespaces` [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<[string](https://learn.microsoft.com/dotnet/api/system.string), syntax\_namespace\_node\>

`shouldReturnCurrentUnit` [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

#### Exceptions

 [CycleUnitReference](PascalABCCompiler.Errors.CycleUnitReference.md)

### <a id="PascalABCCompiler_Compiler_CompileReference_PascalABCCompiler_TreeRealization_unit_node_list_PascalABCCompiler_TreeRealization_compiler_directive_"></a> CompileReference\(unit\_node\_list, compiler\_directive\)

```csharp
private CompilationUnit CompileReference(unit_node_list dlls, compiler_directive reference)
```

#### Parameters

`dlls` unit\_node\_list

`reference` compiler\_directive

#### Returns

 [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

### <a id="PascalABCCompiler_Compiler_CompileUnit_PascalABCCompiler_TreeRealization_unit_node_list_System_Collections_Generic_Dictionary_PascalABCCompiler_TreeRealization_unit_node_PascalABCCompiler_CompilationUnit__PascalABCCompiler_SyntaxTree_unit_or_namespace_System_String_"></a> CompileUnit\(unit\_node\_list, Dictionary<unit\_node, CompilationUnit\>, unit\_or\_namespace, string\)

Компилирует основную программу и все используемые ей юниты рекурсивно

```csharp
public CompilationUnit CompileUnit(unit_node_list unitsFromUsesSection, Dictionary<unit_node, CompilationUnit> directUnitsFromUsesSection, unit_or_namespace currentUnitNode, string previousPath)
```

#### Parameters

`unitsFromUsesSection` unit\_node\_list

Вспомогательная переменная для заполнения CompilationUnit.interfaceUsedUnits и 
   CompilationUnit.implementationUsedUnits (здесь могут содержаться юниты и dll)

`directUnitsFromUsesSection` [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<unit\_node, [CompilationUnit](PascalABCCompiler.CompilationUnit.md)\>

Вспомогательная переменная для заполнения CompilationUnit.interfaceUsedDirectUnits и 
    CompilationUnit.implementationUsedDirectUnits

`currentUnitNode` unit\_or\_namespace

Синтаксический узел текущего модуля (или пространства имен)

`previousPath` [string](https://learn.microsoft.com/dotnet/api/system.string)

Директория родительского модуля

#### Returns

 [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

Скомпилированный юнит

### <a id="PascalABCCompiler_Compiler_CompileUnitsFromDelayedList"></a> CompileUnitsFromDelayedList\(\)

```csharp
private void CompileUnitsFromDelayedList()
```

### <a id="PascalABCCompiler_Compiler_ConstructMainSemanticTree_PascalABCCompiler_NETGenerator_CompilerOptions_"></a> ConstructMainSemanticTree\(CompilerOptions\)

```csharp
private program_node ConstructMainSemanticTree(CompilerOptions compilerOptions)
```

#### Parameters

`compilerOptions` CompilerOptions

#### Returns

 program\_node

### <a id="PascalABCCompiler_Compiler_ConstructSyntaxTree_System_String_PascalABCCompiler_CompilationUnit_System_String_"></a> ConstructSyntaxTree\(string, CompilationUnit, string\)

```csharp
private compilation_unit ConstructSyntaxTree(string unitFileName, CompilationUnit currentUnit, string sourceText)
```

#### Parameters

`unitFileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

`currentUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

`sourceText` [string](https://learn.microsoft.com/dotnet/api/system.string)

#### Returns

 compilation\_unit

### <a id="PascalABCCompiler_Compiler_ConstructSyntaxTreeAndRunSugarConversions_System_String_PascalABCCompiler_CompilationUnit_System_Collections_Generic_Dictionary_PascalABCCompiler_SyntaxTree_syntax_tree_node_System_String___"></a> ConstructSyntaxTreeAndRunSugarConversions\(string, CompilationUnit, out Dictionary<syntax\_tree\_node, string\>\)

Строит синтаксическое дерево, бросает первую из найденных ошибок (если они есть) и запускает сахарные преобразования

```csharp
private void ConstructSyntaxTreeAndRunSugarConversions(string unitFileName, CompilationUnit currentUnit, out Dictionary<syntax_tree_node, string> docs)
```

#### Parameters

`unitFileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

`currentUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

`docs` [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<syntax\_tree\_node, [string](https://learn.microsoft.com/dotnet/api/system.string)\>

### <a id="PascalABCCompiler_Compiler_ConvertSyntaxTree_PascalABCCompiler_SyntaxTree_compilation_unit_System_Collections_Generic_List_PascalABCCompiler_SyntaxTreeConverters_ISyntaxTreeConverter__"></a> ConvertSyntaxTree\(compilation\_unit, List<ISyntaxTreeConverter\>\)

```csharp
private compilation_unit ConvertSyntaxTree(compilation_unit syntaxTree, List<ISyntaxTreeConverter> converters)
```

#### Parameters

`syntaxTree` compilation\_unit

`converters` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<ISyntaxTreeConverter\>

#### Returns

 compilation\_unit

### <a id="PascalABCCompiler_Compiler_CreateDependencyListsForCurrentUnit_PascalABCCompiler_CompilationUnit_System_String_System_Collections_Generic_List_PascalABCCompiler_SyntaxTree_unit_or_namespace___PascalABCCompiler_TreeRealization_unit_node_list__System_Collections_Generic_Dictionary_System_String_PascalABCCompiler_SyntaxTree_syntax_namespace_node___"></a> CreateDependencyListsForCurrentUnit\(CompilationUnit, string, out List<unit\_or\_namespace\>, out unit\_node\_list, out Dictionary<string, syntax\_namespace\_node\>\)

```csharp
private void CreateDependencyListsForCurrentUnit(CompilationUnit currentUnit, string currentDirectory, out List<unit_or_namespace> interfaceUsesList, out unit_node_list references, out Dictionary<string, syntax_namespace_node> namespaces)
```

#### Parameters

`currentUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

`currentDirectory` [string](https://learn.microsoft.com/dotnet/api/system.string)

`interfaceUsesList` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<unit\_or\_namespace\>

`references` unit\_node\_list

`namespaces` [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<[string](https://learn.microsoft.com/dotnet/api/system.string), syntax\_namespace\_node\>

### <a id="PascalABCCompiler_Compiler_CurrentUnitIsNotMainProgram"></a> CurrentUnitIsNotMainProgram\(\)

Возвращает true, если текущий компилируемый модуль не является основной программой (program_module)

```csharp
private bool CurrentUnitIsNotMainProgram()
```

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="PascalABCCompiler_Compiler_DebugOutputFileCreationUsingPDB"></a> DebugOutputFileCreationUsingPDB\(\)

```csharp
private void DebugOutputFileCreationUsingPDB()
```

### <a id="PascalABCCompiler_Compiler_DisablePABCRtlIfUsingDotnet5_System_Collections_Generic_List_PascalABCCompiler_TreeRealization_compiler_directive__"></a> DisablePABCRtlIfUsingDotnet5\(List<compiler\_directive\>\)

```csharp
private void DisablePABCRtlIfUsingDotnet5(List<compiler_directive> directives)
```

#### Parameters

`directives` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<compiler\_directive\>

### <a id="PascalABCCompiler_Compiler_FillNetCompilerOptionsFromCompilerDirectives_PascalABCCompiler_NETGenerator_CompilerOptions_System_Collections_Generic_Dictionary_System_String_System_Collections_Generic_List_PascalABCCompiler_TreeRealization_compiler_directive___"></a> FillNetCompilerOptionsFromCompilerDirectives\(CompilerOptions, Dictionary<string, List<compiler\_directive\>\>\)

```csharp
private void FillNetCompilerOptionsFromCompilerDirectives(CompilerOptions netCompilerOptions, Dictionary<string, List<compiler_directive>> compilerDirectives)
```

#### Parameters

`netCompilerOptions` CompilerOptions

`compilerDirectives` [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<[string](https://learn.microsoft.com/dotnet/api/system.string), [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<compiler\_directive\>\>

### <a id="PascalABCCompiler_Compiler_FillNetCompilerOptionsFromProject_PascalABCCompiler_NETGenerator_CompilerOptions_"></a> FillNetCompilerOptionsFromProject\(CompilerOptions\)

```csharp
private void FillNetCompilerOptionsFromProject(CompilerOptions netCompilerOptions)
```

#### Parameters

`netCompilerOptions` CompilerOptions

### <a id="PascalABCCompiler_Compiler_FindFileWithExtensionInDirs_System_String_System_Int32__System_String___"></a> FindFileWithExtensionInDirs\(string, out int, params string\[\]\)

```csharp
private string FindFileWithExtensionInDirs(string fileName, out int foundDirIndex, params string[] dirs)
```

#### Parameters

`fileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

`foundDirIndex` [int](https://learn.microsoft.com/dotnet/api/system.int32)

`dirs` [string](https://learn.microsoft.com/dotnet/api/system.string)\[\]

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_FindPCUFileName_System_String_System_String_System_Int32__"></a> FindPCUFileName\(string, string, out int\)

```csharp
public string FindPCUFileName(string fileName, string currentPath, out int folderPriority)
```

#### Parameters

`fileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

`currentPath` [string](https://learn.microsoft.com/dotnet/api/system.string)

`folderPriority` [int](https://learn.microsoft.com/dotnet/api/system.int32)

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_FindPositionForSemanticErrorInTheErrorList_PascalABCCompiler_Errors_Error_"></a> FindPositionForSemanticErrorInTheErrorList\(Error\)

```csharp
private int FindPositionForSemanticErrorInTheErrorList(Error err)
```

#### Parameters

`err` Error

#### Returns

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

### <a id="PascalABCCompiler_Compiler_FindSourceFileName_System_String_System_String_System_Int32__"></a> FindSourceFileName\(string, string, out int\)

```csharp
public string FindSourceFileName(string fileName, string currentPath, out int folderPriority)
```

#### Parameters

`fileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

`currentPath` [string](https://learn.microsoft.com/dotnet/api/system.string)

`folderPriority` [int](https://learn.microsoft.com/dotnet/api/system.int32)

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_FindSourceFileNameInDirs_System_String_System_Int32__System_String___"></a> FindSourceFileNameInDirs\(string, out int, params string\[\]\)

```csharp
public string FindSourceFileNameInDirs(string fileName, out int foundDirIndex, params string[] Dirs)
```

#### Parameters

`fileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

`foundDirIndex` [int](https://learn.microsoft.com/dotnet/api/system.int32)

`Dirs` [string](https://learn.microsoft.com/dotnet/api/system.string)\[\]

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_Free"></a> Free\(\)

```csharp
public void Free()
```

### <a id="PascalABCCompiler_Compiler_GenUnitDocumentation_PascalABCCompiler_CompilationUnit_System_String_"></a> GenUnitDocumentation\(CompilationUnit, string\)

```csharp
private Dictionary<syntax_tree_node, string> GenUnitDocumentation(CompilationUnit currentUnit, string SourceText)
```

#### Parameters

`currentUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

`SourceText` [string](https://learn.microsoft.com/dotnet/api/system.string)

#### Returns

 [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<syntax\_tree\_node, [string](https://learn.microsoft.com/dotnet/api/system.string)\>

### <a id="PascalABCCompiler_Compiler_GenerateILCode_PascalABCCompiler_TreeRealization_program_node_PascalABCCompiler_NETGenerator_CompilerOptions_System_Collections_Generic_List_System_String__"></a> GenerateILCode\(program\_node, CompilerOptions, List<string\>\)

```csharp
private void GenerateILCode(program_node programNode, CompilerOptions compilerOptions, List<string> resourceFiles)
```

#### Parameters

`programNode` program\_node

`compilerOptions` CompilerOptions

`resourceFiles` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<[string](https://learn.microsoft.com/dotnet/api/system.string)\>

### <a id="PascalABCCompiler_Compiler_GetCompilerDirectives_System_Collections_Generic_List_PascalABCCompiler_CompilationUnit__"></a> GetCompilerDirectives\(List<CompilationUnit\>\)

Формирует словарь директив компилятора, собирая их из всех переданных модулей

```csharp
private Dictionary<string, List<compiler_directive>> GetCompilerDirectives(List<CompilationUnit> Units)
```

#### Parameters

`Units` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<[CompilationUnit](PascalABCCompiler.CompilationUnit.md)\>

#### Returns

 [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<[string](https://learn.microsoft.com/dotnet/api/system.string), [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<compiler\_directive\>\>

#### Exceptions

 [DuplicateDirective](PascalABCCompiler.Errors.DuplicateDirective.md)

### <a id="PascalABCCompiler_Compiler_GetDirectivesAsSemanticNodes_System_Collections_Generic_List_PascalABCCompiler_SyntaxTree_compiler_directive__System_String_"></a> GetDirectivesAsSemanticNodes\(List<compiler\_directive\>, string\)

преобразует в директивы семантического уровня | в syntax_tree_visitor такая же функция  EVA

```csharp
private List<compiler_directive> GetDirectivesAsSemanticNodes(List<compiler_directive> compilerDirectives, string unitFileName)
```

#### Parameters

`compilerDirectives` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<compiler\_directive\>

`unitFileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

#### Returns

 [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<compiler\_directive\>

### <a id="PascalABCCompiler_Compiler_GetImplementationSyntaxUsingList_PascalABCCompiler_SyntaxTree_compilation_unit_"></a> GetImplementationSyntaxUsingList\(compilation\_unit\)

получение списка using - legacy code !!!

```csharp
private using_list GetImplementationSyntaxUsingList(compilation_unit cu)
```

#### Parameters

`cu` compilation\_unit

#### Returns

 using\_list

### <a id="PascalABCCompiler_Compiler_GetImplementationUsesSection_PascalABCCompiler_SyntaxTree_compilation_unit_"></a> GetImplementationUsesSection\(compilation\_unit\)

```csharp
private List<unit_or_namespace> GetImplementationUsesSection(compilation_unit unitSyntaxTree)
```

#### Parameters

`unitSyntaxTree` compilation\_unit

#### Returns

 [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<unit\_or\_namespace\>

### <a id="PascalABCCompiler_Compiler_GetIncludedFilesFromDirectives_PascalABCCompiler_CompilationUnit_System_Collections_Generic_List_PascalABCCompiler_TreeRealization_compiler_directive__"></a> GetIncludedFilesFromDirectives\(CompilationUnit, List<compiler\_directive\>\)

```csharp
private static List<string> GetIncludedFilesFromDirectives(CompilationUnit compilationUnit, List<compiler_directive> directives)
```

#### Parameters

`compilationUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

`directives` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<compiler\_directive\>

#### Returns

 [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<[string](https://learn.microsoft.com/dotnet/api/system.string)\>

### <a id="PascalABCCompiler_Compiler_GetInterfaceUsesSection_PascalABCCompiler_SyntaxTree_compilation_unit_"></a> GetInterfaceUsesSection\(compilation\_unit\)

Возвращает список зависимостей из интерфейсной части модуля (или основной программы)

```csharp
public List<unit_or_namespace> GetInterfaceUsesSection(compilation_unit unitSyntaxTree)
```

#### Parameters

`unitSyntaxTree` compilation\_unit

#### Returns

 [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<unit\_or\_namespace\>

### <a id="PascalABCCompiler_Compiler_GetInterfaceUsingList_PascalABCCompiler_SyntaxTree_compilation_unit_"></a> GetInterfaceUsingList\(compilation\_unit\)

получение списка using - legacy code !!!

```csharp
public using_list GetInterfaceUsingList(compilation_unit cu)
```

#### Parameters

`cu` compilation\_unit

#### Returns

 using\_list

### <a id="PascalABCCompiler_Compiler_GetLocationFromTreenode_PascalABCCompiler_SyntaxTree_syntax_tree_node_System_String_"></a> GetLocationFromTreenode\(syntax\_tree\_node, string\)

```csharp
private location GetLocationFromTreenode(syntax_tree_node tn, string FileName)
```

#### Parameters

`tn` syntax\_tree\_node

`FileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

#### Returns

 location

### <a id="PascalABCCompiler_Compiler_GetNamespace_PascalABCCompiler_SyntaxTree_unit_or_namespace_"></a> GetNamespace\(unit\_or\_namespace\)

```csharp
public using_namespace GetNamespace(unit_or_namespace _name_space)
```

#### Parameters

`_name_space` unit\_or\_namespace

#### Returns

 using\_namespace

### <a id="PascalABCCompiler_Compiler_GetNamespace_PascalABCCompiler_TreeRealization_using_namespace_list_System_String_PascalABCCompiler_SyntaxTree_unit_or_namespace_System_Boolean_System_Collections_Generic_Dictionary_System_String_PascalABCCompiler_SyntaxTree_syntax_namespace_node__"></a> GetNamespace\(using\_namespace\_list, string, unit\_or\_namespace, bool, Dictionary<string, syntax\_namespace\_node\>\)

Формирует узел семантического дерева, соответствующий пространству имен (.NET или пользовательскому)

```csharp
private using_namespace GetNamespace(using_namespace_list usingList, string fullNamespaceName, unit_or_namespace name_space, bool mightBeUnit, Dictionary<string, syntax_namespace_node> namespaces)
```

#### Parameters

`usingList` using\_namespace\_list

`fullNamespaceName` [string](https://learn.microsoft.com/dotnet/api/system.string)

`name_space` unit\_or\_namespace

`mightBeUnit` [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

`namespaces` [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<[string](https://learn.microsoft.com/dotnet/api/system.string), syntax\_namespace\_node\>

#### Returns

 using\_namespace

#### Exceptions

 [UnitNotFound](PascalABCCompiler.Errors.UnitNotFound.md)

 NamespaceNotFound

### <a id="PascalABCCompiler_Compiler_GetNamespaceSyntaxTree_System_String_"></a> GetNamespaceSyntaxTree\(string\)

```csharp
private compilation_unit GetNamespaceSyntaxTree(string fileName)
```

#### Parameters

`fileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

#### Returns

 compilation\_unit

### <a id="PascalABCCompiler_Compiler_GetReferenceFileName_System_String_System_String_"></a> GetReferenceFileName\(string, string\)

```csharp
public static string GetReferenceFileName(string FileName, string curr_path = null)
```

#### Parameters

`FileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

`curr_path` [string](https://learn.microsoft.com/dotnet/api/system.string)

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_GetReferenceFileName_System_String_PascalABCCompiler_SyntaxTree_SourceContext_System_String_System_Boolean_"></a> GetReferenceFileName\(string, SourceContext, string, bool\)

```csharp
private string GetReferenceFileName(string FileName, SourceContext sc, string curr_path, bool overwrite)
```

#### Parameters

`FileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

`sc` SourceContext

`curr_path` [string](https://learn.microsoft.com/dotnet/api/system.string)

`overwrite` [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_GetReferences_PascalABCCompiler_CompilationUnit_"></a> GetReferences\(CompilationUnit\)

```csharp
public unit_node_list GetReferences(CompilationUnit compilationUnit)
```

#### Parameters

`compilationUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

#### Returns

 unit\_node\_list

### <a id="PascalABCCompiler_Compiler_GetResourceFilesFromCompilerDirectives_System_Collections_Generic_Dictionary_System_String_System_Collections_Generic_List_PascalABCCompiler_TreeRealization_compiler_directive___"></a> GetResourceFilesFromCompilerDirectives\(Dictionary<string, List<compiler\_directive\>\>\)

```csharp
private List<string> GetResourceFilesFromCompilerDirectives(Dictionary<string, List<compiler_directive>> compilerDirectives)
```

#### Parameters

`compilerDirectives` [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<[string](https://learn.microsoft.com/dotnet/api/system.string), [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<compiler\_directive\>\>

#### Returns

 [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<[string](https://learn.microsoft.com/dotnet/api/system.string)\>

### <a id="PascalABCCompiler_Compiler_GetSourceCode_System_String_PascalABCCompiler_CompilationUnit_"></a> GetSourceCode\(string, CompilationUnit\)

```csharp
private string GetSourceCode(string UnitFileName, CompilationUnit currentUnit)
```

#### Parameters

`UnitFileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

`currentUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_GetSourceContext_PascalABCCompiler_TreeRealization_compiler_directive_"></a> GetSourceContext\(compiler\_directive\)

```csharp
private SourceContext GetSourceContext(compiler_directive directive)
```

#### Parameters

`directive` compiler\_directive

#### Returns

 SourceContext

### <a id="PascalABCCompiler_Compiler_GetSourceFileText_System_String_"></a> GetSourceFileText\(string\)

```csharp
public string GetSourceFileText(string FileName)
```

#### Parameters

`FileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_GetUnitFileName_PascalABCCompiler_SyntaxTree_unit_or_namespace_System_String_"></a> GetUnitFileName\(unit\_or\_namespace, string\)

```csharp
public string GetUnitFileName(unit_or_namespace unitNode, string currentPath)
```

#### Parameters

`unitNode` unit\_or\_namespace

`currentPath` [string](https://learn.microsoft.com/dotnet/api/system.string)

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_GetUnitFileName_System_String_System_String_System_String_PascalABCCompiler_SyntaxTree_SourceContext_"></a> GetUnitFileName\(string, string, string, SourceContext\)

```csharp
public string GetUnitFileName(string unitName, string usesPath, string currentPath, SourceContext sourceContext)
```

#### Parameters

`unitName` [string](https://learn.microsoft.com/dotnet/api/system.string)

`usesPath` [string](https://learn.microsoft.com/dotnet/api/system.string)

`currentPath` [string](https://learn.microsoft.com/dotnet/api/system.string)

`sourceContext` SourceContext

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_GetUnitPath_PascalABCCompiler_CompilationUnit_PascalABCCompiler_CompilationUnit_"></a> GetUnitPath\(CompilationUnit, CompilationUnit\)

```csharp
public static string GetUnitPath(CompilationUnit u1, CompilationUnit u2)
```

#### Parameters

`u1` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

`u2` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_HasIncludeNamespaceDirective_PascalABCCompiler_CompilationUnit_"></a> HasIncludeNamespaceDirective\(CompilationUnit\)

```csharp
private bool HasIncludeNamespaceDirective(CompilationUnit unit)
```

#### Parameters

`unit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="PascalABCCompiler_Compiler_HasOnlySyntaxErrors_System_Collections_Generic_List_PascalABCCompiler_Errors_Error__"></a> HasOnlySyntaxErrors\(List<Error\>\)

```csharp
private bool HasOnlySyntaxErrors(List<Error> errors)
```

#### Parameters

`errors` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<Error\>

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="PascalABCCompiler_Compiler_InitializeCompilerOptionsRelatedToStandardUnits_PascalABCCompiler_SyntaxTree_compilation_unit_"></a> InitializeCompilerOptionsRelatedToStandardUnits\(compilation\_unit\)

Устанавливает значения опций DisableStandardUnits и UseDllForSystemUnits

```csharp
private void InitializeCompilerOptionsRelatedToStandardUnits(compilation_unit unitSyntaxTree)
```

#### Parameters

`unitSyntaxTree` compilation\_unit

### <a id="PascalABCCompiler_Compiler_InitializeNewUnit_System_String_System_String_PascalABCCompiler_CompilationUnit__System_Collections_Generic_Dictionary_PascalABCCompiler_SyntaxTree_syntax_tree_node_System_String___"></a> InitializeNewUnit\(string, string, ref CompilationUnit, out Dictionary<syntax\_tree\_node, string\>\)

Получение исходного кода модуля, заполнение документации,
генерация синтаксического дерева,
обработка синтаксических ошибок

```csharp
private void InitializeNewUnit(string unitFileName, string UnitId, ref CompilationUnit currentUnit, out Dictionary<syntax_tree_node, string> docs)
```

#### Parameters

`unitFileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

`UnitId` [string](https://learn.microsoft.com/dotnet/api/system.string)

`currentUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

`docs` [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<syntax\_tree\_node, [string](https://learn.microsoft.com/dotnet/api/system.string)\>

### <a id="PascalABCCompiler_Compiler_InitializeProjectInfoAndFillCompilerOptionsFromIt"></a> InitializeProjectInfoAndFillCompilerOptionsFromIt\(\)

```csharp
private void InitializeProjectInfoAndFillCompilerOptionsFromIt()
```

### <a id="PascalABCCompiler_Compiler_InternalParseText_Languages_Facade_ILanguage_System_String_System_String_System_Collections_Generic_List_PascalABCCompiler_Errors_Error__System_Collections_Generic_List_PascalABCCompiler_Errors_CompilerWarning__System_Collections_Generic_List_System_String__System_Boolean_"></a> InternalParseText\(ILanguage, string, string, List<Error\>, List<CompilerWarning\>, List<string\>, bool\)

```csharp
private compilation_unit InternalParseText(ILanguage language, string fileName, string text, List<Error> errorList, List<CompilerWarning> warnings, List<string> definesList = null, bool calculateHealth = true)
```

#### Parameters

`language` ILanguage

`fileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

`text` [string](https://learn.microsoft.com/dotnet/api/system.string)

`errorList` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<Error\>

`warnings` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<CompilerWarning\>

`definesList` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<[string](https://learn.microsoft.com/dotnet/api/system.string)\>

`calculateHealth` [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

#### Returns

 compilation\_unit

### <a id="PascalABCCompiler_Compiler_IsDll_PascalABCCompiler_SyntaxTree_compilation_unit_"></a> IsDll\(compilation\_unit\)

Проверяет, является ли модуль dll по соответствующей директиве

```csharp
public static bool IsDll(compilation_unit unitSyntaxTree)
```

#### Parameters

`unitSyntaxTree` compilation\_unit

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="PascalABCCompiler_Compiler_IsDll_PascalABCCompiler_SyntaxTree_compilation_unit_PascalABCCompiler_SyntaxTree_compiler_directive__"></a> IsDll\(compilation\_unit, out compiler\_directive\)

Проверяет, является ли модуль dll по соответствующей директиве и возвращает эту директиву выходным параметром

```csharp
public static bool IsDll(compilation_unit unitSyntaxTree, out compiler_directive dllDirective)
```

#### Parameters

`unitSyntaxTree` compilation\_unit

`dllDirective` compiler\_directive

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="PascalABCCompiler_Compiler_IsDocumentationNeeded_PascalABCCompiler_SyntaxTree_compilation_unit_"></a> IsDocumentationNeeded\(compilation\_unit\)

```csharp
private bool IsDocumentationNeeded(compilation_unit unitSyntaxTree)
```

#### Parameters

`unitSyntaxTree` compilation\_unit

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="PascalABCCompiler_Compiler_IsPossibleNetNamespaceOrStandardPasFile_PascalABCCompiler_SyntaxTree_unit_or_namespace_System_Boolean_System_String_"></a> IsPossibleNetNamespaceOrStandardPasFile\(unit\_or\_namespace, bool, string\)

```csharp
private bool IsPossibleNetNamespaceOrStandardPasFile(unit_or_namespace name_space, bool addToStandardModules, string currentPath)
```

#### Parameters

`name_space` unit\_or\_namespace

`addToStandardModules` [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

`currentPath` [string](https://learn.microsoft.com/dotnet/api/system.string)

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="PascalABCCompiler_Compiler_MatchSyntaxErrorsToBadNodes_PascalABCCompiler_CompilationUnit_"></a> MatchSyntaxErrorsToBadNodes\(CompilationUnit\)

```csharp
private void MatchSyntaxErrorsToBadNodes(CompilationUnit currentUnit)
```

#### Parameters

`currentUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

### <a id="PascalABCCompiler_Compiler_NeedRecompiled_System_String_System_String___PascalABCCompiler_PCU_PCUReader_"></a> NeedRecompiled\(string, string\[\], PCUReader\)

```csharp
public bool NeedRecompiled(string pcu_name, string[] included, PCUReader pr)
```

#### Parameters

`pcu_name` [string](https://learn.microsoft.com/dotnet/api/system.string)

`included` [string](https://learn.microsoft.com/dotnet/api/system.string)\[\]

`pr` [PCUReader](PascalABCCompiler.PCU.PCUReader.md)

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="PascalABCCompiler_Compiler_ParseText_System_String_System_String_System_Collections_Generic_List_PascalABCCompiler_Errors_Error__System_Collections_Generic_List_PascalABCCompiler_Errors_CompilerWarning__"></a> ParseText\(string, string, List<Error\>, List<CompilerWarning\>\)

```csharp
public compilation_unit ParseText(string fileName, string text, List<Error> errorList, List<CompilerWarning> warnings)
```

#### Parameters

`fileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

`text` [string](https://learn.microsoft.com/dotnet/api/system.string)

`errorList` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<Error\>

`warnings` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<CompilerWarning\>

#### Returns

 compilation\_unit

### <a id="PascalABCCompiler_Compiler_PrebuildMainSemanticTreeActions_PascalABCCompiler_NETGenerator_CompilerOptions__System_Collections_Generic_List_System_String___"></a> PrebuildMainSemanticTreeActions\(out CompilerOptions, out List<string\>\)

Сохраняет документацию для модулей;
Выясняет тип выходного файла, целевой фреймворк, платформу;
Заполняет опции .NET компиляции согласно директивам и/или информации из проекта;
Находит ресурсные файлы из директив

```csharp
private void PrebuildMainSemanticTreeActions(out CompilerOptions netCompilerOptions, out List<string> resourceFiles)
```

#### Parameters

`netCompilerOptions` CompilerOptions

`resourceFiles` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<[string](https://learn.microsoft.com/dotnet/api/system.string)\>

### <a id="PascalABCCompiler_Compiler_PreloadReference_PascalABCCompiler_TreeRealization_compiler_directive_"></a> PreloadReference\(compiler\_directive\)

```csharp
private Assembly PreloadReference(compiler_directive reference)
```

#### Parameters

`reference` compiler\_directive

#### Returns

 [Assembly](https://learn.microsoft.com/dotnet/api/system.reflection.assembly)

### <a id="PascalABCCompiler_Compiler_PrepareFinalMainFunctionForExe_PascalABCCompiler_TreeRealization_program_node_"></a> PrepareFinalMainFunctionForExe\(program\_node\)

```csharp
public void PrepareFinalMainFunctionForExe(program_node mainSemanticTree)
```

#### Parameters

`mainSemanticTree` program\_node

### <a id="PascalABCCompiler_Compiler_PrepareUserNamespacesUsedInTheCurrentUnit_PascalABCCompiler_CompilationUnit_"></a> PrepareUserNamespacesUsedInTheCurrentUnit\(CompilationUnit\)

```csharp
private Dictionary<string, syntax_namespace_node> PrepareUserNamespacesUsedInTheCurrentUnit(CompilationUnit compilationUnit)
```

#### Parameters

`compilationUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

#### Returns

 [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<[string](https://learn.microsoft.com/dotnet/api/system.string), syntax\_namespace\_node\>

### <a id="PascalABCCompiler_Compiler_ReadDLL_System_String_PascalABCCompiler_SyntaxTree_SourceContext_"></a> ReadDLL\(string, SourceContext\)

```csharp
public CompilationUnit ReadDLL(string FileName, SourceContext sc = null)
```

#### Parameters

`FileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

`sc` SourceContext

#### Returns

 [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

### <a id="PascalABCCompiler_Compiler_ReadPCU_System_String_"></a> ReadPCU\(string\)

```csharp
public CompilationUnit ReadPCU(string FileName)
```

#### Parameters

`FileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

#### Returns

 [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

### <a id="PascalABCCompiler_Compiler_Reload"></a> Reload\(\)

```csharp
public void Reload()
```

### <a id="PascalABCCompiler_Compiler_Reset"></a> Reset\(\)

```csharp
private void Reset()
```

### <a id="PascalABCCompiler_Compiler_RunSemanticChecks_System_String_PascalABCCompiler_CompilationUnit_"></a> RunSemanticChecks\(string, CompilationUnit\)

Семантические проверки по директивам и по типу файла

```csharp
private void RunSemanticChecks(string unitFileName, CompilationUnit currentUnit)
```

#### Parameters

`unitFileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

`currentUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

### <a id="PascalABCCompiler_Compiler_SaveDocumentationsForUnits"></a> SaveDocumentationsForUnits\(\)

```csharp
private void SaveDocumentationsForUnits()
```

### <a id="PascalABCCompiler_Compiler_SavePCU_PascalABCCompiler_CompilationUnit_"></a> SavePCU\(CompilationUnit\)

```csharp
public void SavePCU(CompilationUnit Unit)
```

#### Parameters

`Unit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

### <a id="PascalABCCompiler_Compiler_SaveUnitCheckInParsers"></a> SaveUnitCheckInParsers\(\)

Передаем парсерам возможность проверить, компилируется ли в данный момент модуль 
(нужно, если нет ключевого слова unit или подобного в языке)

```csharp
private void SaveUnitCheckInParsers()
```

### <a id="PascalABCCompiler_Compiler_SemanticCheckCurrentUnitMustBeUnitModule_System_String_PascalABCCompiler_CompilationUnit_System_Boolean_"></a> SemanticCheckCurrentUnitMustBeUnitModule\(string, CompilationUnit, bool\)

```csharp
private void SemanticCheckCurrentUnitMustBeUnitModule(string UnitFileName, CompilationUnit currentUnit, bool isDll)
```

#### Parameters

`UnitFileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

`currentUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

`isDll` [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="PascalABCCompiler_Compiler_SemanticCheckDLLDirectiveOnlyForLibraries_PascalABCCompiler_SyntaxTree_compilation_unit_System_Boolean_PascalABCCompiler_SyntaxTree_compiler_directive_"></a> SemanticCheckDLLDirectiveOnlyForLibraries\(compilation\_unit, bool, compiler\_directive\)

Проверка, что директива dll только в Library - требует передачи директивы dll

```csharp
private void SemanticCheckDLLDirectiveOnlyForLibraries(compilation_unit unitSyntaxTree, bool isDll, compiler_directive dllDirective)
```

#### Parameters

`unitSyntaxTree` compilation\_unit

`isDll` [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

`dllDirective` compiler\_directive

### <a id="PascalABCCompiler_Compiler_SemanticCheckDisableStandardUnitsDirectiveInUnit_PascalABCCompiler_SyntaxTree_compilation_unit_"></a> SemanticCheckDisableStandardUnitsDirectiveInUnit\(compilation\_unit\)

Ошибка указания директивы DisableStandardUnits в подключенном модулей

```csharp
private void SemanticCheckDisableStandardUnitsDirectiveInUnit(compilation_unit unitSyntaxTree)
```

#### Parameters

`unitSyntaxTree` compilation\_unit

### <a id="PascalABCCompiler_Compiler_SemanticCheckIsUserNamespace_PascalABCCompiler_SyntaxTree_compilation_unit_"></a> SemanticCheckIsUserNamespace\(compilation\_unit\)

```csharp
private void SemanticCheckIsUserNamespace(compilation_unit unitSyntaxTree)
```

#### Parameters

`unitSyntaxTree` compilation\_unit

### <a id="PascalABCCompiler_Compiler_SemanticCheckNamespacesOnlyInProjects_PascalABCCompiler_CompilationUnit_"></a> SemanticCheckNamespacesOnlyInProjects\(CompilationUnit\)

```csharp
private void SemanticCheckNamespacesOnlyInProjects(CompilationUnit currentUnit)
```

#### Parameters

`currentUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

### <a id="PascalABCCompiler_Compiler_SemanticCheckNoIncludeNamespaceDirectivesInUnit_PascalABCCompiler_CompilationUnit_"></a> SemanticCheckNoIncludeNamespaceDirectivesInUnit\(CompilationUnit\)

```csharp
private void SemanticCheckNoIncludeNamespaceDirectivesInUnit(CompilationUnit currentUnit)
```

#### Parameters

`currentUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

### <a id="PascalABCCompiler_Compiler_SemanticCheckNoLoopDependenciesOfInterfaces_PascalABCCompiler_CompilationUnit_System_String_PascalABCCompiler_SyntaxTree_unit_or_namespace_System_String_"></a> SemanticCheckNoLoopDependenciesOfInterfaces\(CompilationUnit, string, unit\_or\_namespace, string\)

```csharp
private void SemanticCheckNoLoopDependenciesOfInterfaces(CompilationUnit currentUnit, string unitFileName, unit_or_namespace usedUnitNode, string currentPath)
```

#### Parameters

`currentUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

`unitFileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

`usedUnitNode` unit\_or\_namespace

`currentPath` [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_SemanticCheckUsesInIsNotNamespace_PascalABCCompiler_SyntaxTree_unit_or_namespace_PascalABCCompiler_CompilationUnit_"></a> SemanticCheckUsesInIsNotNamespace\(unit\_or\_namespace, CompilationUnit\)

```csharp
private void SemanticCheckUsesInIsNotNamespace(unit_or_namespace currentUnitNode, CompilationUnit currentUnit)
```

#### Parameters

`currentUnitNode` unit\_or\_namespace

`currentUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

### <a id="PascalABCCompiler_Compiler_SetOutputFileTypeOption_System_Collections_Generic_Dictionary_System_String_System_Collections_Generic_List_PascalABCCompiler_TreeRealization_compiler_directive___"></a> SetOutputFileTypeOption\(Dictionary<string, List<compiler\_directive\>\>\)

```csharp
private void SetOutputFileTypeOption(Dictionary<string, List<compiler_directive>> compilerDirectives)
```

#### Parameters

`compilerDirectives` [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<[string](https://learn.microsoft.com/dotnet/api/system.string), [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<compiler\_directive\>\>

### <a id="PascalABCCompiler_Compiler_SetOutputPlatformOption_PascalABCCompiler_NETGenerator_CompilerOptions_System_Collections_Generic_Dictionary_System_String_System_Collections_Generic_List_PascalABCCompiler_TreeRealization_compiler_directive___"></a> SetOutputPlatformOption\(CompilerOptions, Dictionary<string, List<compiler\_directive\>\>\)

```csharp
private void SetOutputPlatformOption(CompilerOptions netCompilerOptions, Dictionary<string, List<compiler_directive>> compilerDirectives)
```

#### Parameters

`netCompilerOptions` CompilerOptions

`compilerDirectives` [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<[string](https://learn.microsoft.com/dotnet/api/system.string), [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<compiler\_directive\>\>

### <a id="PascalABCCompiler_Compiler_SetSupportedProjectFiles"></a> SetSupportedProjectFiles\(\)

```csharp
private void SetSupportedProjectFiles()
```

### <a id="PascalABCCompiler_Compiler_SetSupportedSourceFiles"></a> SetSupportedSourceFiles\(\)

```csharp
private void SetSupportedSourceFiles()
```

### <a id="PascalABCCompiler_Compiler_SetTargetTypeOption_PascalABCCompiler_NETGenerator_CompilerOptions_"></a> SetTargetTypeOption\(CompilerOptions\)

```csharp
private void SetTargetTypeOption(CompilerOptions netCompilerOptions)
```

#### Parameters

`netCompilerOptions` CompilerOptions

### <a id="PascalABCCompiler_Compiler_SetUseDLLForSystemUnits_System_String_System_Collections_Generic_List_PascalABCCompiler_SyntaxTree_unit_or_namespace__System_Int32_"></a> SetUseDLLForSystemUnits\(string, List<unit\_or\_namespace\>, int\)

Если в программе в секции uses есть не про-во имен и не стандартный модуль, то использование PABCRtl.dll отменяется

```csharp
private void SetUseDLLForSystemUnits(string currentDirectory, List<unit_or_namespace> usesList, int lastUnitIndex)
```

#### Parameters

`currentDirectory` [string](https://learn.microsoft.com/dotnet/api/system.string)

`usesList` [List](https://learn.microsoft.com/dotnet/api/system.collections.generic.list\-1)<unit\_or\_namespace\>

`lastUnitIndex` [int](https://learn.microsoft.com/dotnet/api/system.int32)

### <a id="PascalABCCompiler_Compiler_SourceFileExists_System_String_"></a> SourceFileExists\(string\)

```csharp
private bool SourceFileExists(string FileName)
```

#### Parameters

`FileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="PascalABCCompiler_Compiler_SourceFileGetLastWriteTime_System_String_"></a> SourceFileGetLastWriteTime\(string\)

```csharp
private DateTime SourceFileGetLastWriteTime(string FileName)
```

#### Parameters

`FileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

#### Returns

 [DateTime](https://learn.microsoft.com/dotnet/api/system.datetime)

### <a id="PascalABCCompiler_Compiler_StartCompile"></a> StartCompile\(\)

```csharp
public void StartCompile()
```

### <a id="PascalABCCompiler_Compiler_ToString"></a> ToString\(\)

```csharp
public override string ToString()
```

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_TryThrowInvalidPath_System_String_PascalABCCompiler_SyntaxTree_SourceContext_"></a> TryThrowInvalidPath\(string, SourceContext\)

```csharp
public static void TryThrowInvalidPath(string path, SourceContext loc)
```

#### Parameters

`path` [string](https://learn.microsoft.com/dotnet/api/system.string)

`loc` SourceContext

### <a id="PascalABCCompiler_Compiler_UnitHasPCU_PascalABCCompiler_TreeRealization_unit_node_list_System_Collections_Generic_Dictionary_PascalABCCompiler_TreeRealization_unit_node_PascalABCCompiler_CompilationUnit__PascalABCCompiler_SyntaxTree_unit_or_namespace_System_String__PascalABCCompiler_CompilationUnit__"></a> UnitHasPCU\(unit\_node\_list, Dictionary<unit\_node, CompilationUnit\>, unit\_or\_namespace, ref string, ref CompilationUnit\)

```csharp
private bool UnitHasPCU(unit_node_list unitsFromUsesSection, Dictionary<unit_node, CompilationUnit> directUnitsFromUsesSection, unit_or_namespace currentUnitNode, ref string UnitFileName, ref CompilationUnit currentUnit)
```

#### Parameters

`unitsFromUsesSection` unit\_node\_list

`directUnitsFromUsesSection` [Dictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary\-2)<unit\_node, [CompilationUnit](PascalABCCompiler.CompilationUnit.md)\>

`currentUnitNode` unit\_or\_namespace

`UnitFileName` [string](https://learn.microsoft.com/dotnet/api/system.string)

`currentUnit` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="PascalABCCompiler_Compiler_WaitCallback_ClosePCUWriters_System_Object_"></a> WaitCallback\_ClosePCUWriters\(object\)

```csharp
private void WaitCallback_ClosePCUWriters(object state)
```

#### Parameters

`state` [object](https://learn.microsoft.com/dotnet/api/system.object)

### <a id="PascalABCCompiler_Compiler_buildImplementationUsesList_PascalABCCompiler_CompilationUnit_"></a> buildImplementationUsesList\(CompilationUnit\)

```csharp
private unit_node_list buildImplementationUsesList(CompilationUnit cu)
```

#### Parameters

`cu` [CompilationUnit](PascalABCCompiler.CompilationUnit.md)

#### Returns

 unit\_node\_list

### <a id="PascalABCCompiler_Compiler_get_assembly_path_System_String_System_Boolean_"></a> get\_assembly\_path\(string, bool\)

```csharp
public static string get_assembly_path(string name, bool search_for_intellisense)
```

#### Parameters

`name` [string](https://learn.microsoft.com/dotnet/api/system.string)

`search_for_intellisense` [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_get_standart_assembly_path_System_String_"></a> get\_standart\_assembly\_path\(string\)

```csharp
public static string get_standart_assembly_path(string name)
```

#### Parameters

`name` [string](https://learn.microsoft.com/dotnet/api/system.string)

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

### <a id="PascalABCCompiler_Compiler_pr_ChangeState_System_Object_PascalABCCompiler_PCU_PCUReaderWriterState_System_Object_"></a> pr\_ChangeState\(object, PCUReaderWriterState, object\)

```csharp
private void pr_ChangeState(object Sender, PCUReaderWriterState State, object obj)
```

#### Parameters

`Sender` [object](https://learn.microsoft.com/dotnet/api/system.object)

`State` [PCUReaderWriterState](PascalABCCompiler.PCU.PCUReaderWriterState.md)

`obj` [object](https://learn.microsoft.com/dotnet/api/system.object)

### <a id="PascalABCCompiler_Compiler_semanticTreeConvertersController_ChangeState_PascalABCCompiler_SemanticTreeConverters_SemanticTreeConvertersController_State_PascalABCCompiler_SemanticTreeConverters_ISemanticTreeConverter_"></a> semanticTreeConvertersController\_ChangeState\(State, ISemanticTreeConverter\)

```csharp
private void semanticTreeConvertersController_ChangeState(SemanticTreeConvertersController.State State, ISemanticTreeConverter SemanticTreeConverter)
```

#### Parameters

`State` [SemanticTreeConvertersController](PascalABCCompiler.SemanticTreeConverters.SemanticTreeConvertersController.md).[State](PascalABCCompiler.SemanticTreeConverters.SemanticTreeConvertersController.State.md)

`SemanticTreeConverter` [ISemanticTreeConverter](PascalABCCompiler.SemanticTreeConverters.ISemanticTreeConverter.md)

### <a id="PascalABCCompiler_Compiler_OnChangeCompilerState"></a> OnChangeCompilerState

```csharp
public event ChangeCompilerStateEventDelegate OnChangeCompilerState
```

#### Event Type

 [ChangeCompilerStateEventDelegate](PascalABCCompiler.ChangeCompilerStateEventDelegate.md)

