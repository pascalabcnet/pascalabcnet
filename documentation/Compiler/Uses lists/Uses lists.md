
Owner: Alexander Zemlyak

- **Получение списков зависимостей из синтаксического дерева юнита**
1. *interfaceUsesList*  может содержать юниты, пространства имен .NET и явные пространства имен (узлы SyntaxTree.unit_or_namepace)
2. *references* содержит библиотеки dll (узлы TreeRealization.unit_node)
3. *namespaces* содержит явные пользовательские пространства имен, подключенные с помощью директивы {$includenamespace} (узлы SyntaxTree.syntax_namespace_node)

```csharp
CreateDependencyListsForCurrentUnit(currentUnit, currentDirectory, out var interfaceUsesList, out var references, out var namespaces);
```

- **Списки зависимостей, содержащиеся в CompilationUnit**
1. *possibleNamespaces* может содержать пространства имен .NET, стандартные паскалевские юниты из папки Lib, а также явные пространства имен, если они есть в секции uses 
2. *InterfaceUsedUnits* содержит все, что есть в *possibleNamespaces +* пользовательские юниты и dll, т.е., грубо говоря, все виды программных единиц, являющиеся зависимостями CompilationUnit
3. *InterfaceUsedDirectUnits* содержит только пользовательские юниты
4. *ImplementationUsedUnits* и *ImplementationUsedDirectUnits* устроены аналогично предыдущим двум

```csharp
public class CompilationUnit
{
...
internal List<SyntaxTree.unit_or_namespace> possibleNamespaces = new List<PascalABCCompiler.SyntaxTree.unit_or_namespace>();

public Dictionary<unit_node, CompilationUnit> InterfaceUsedDirectUnits { get; } = new Dictionary<unit_node, CompilationUnit>();

public unit_node_list InterfaceUsedUnits { get; } = new unit_node_list();

public Dictionary<unit_node, CompilationUnit> ImplementationUsedDirectUnits { get; } = new Dictionary<unit_node, CompilationUnit>();

public unit_node_list ImplementationUsedUnits { get; } = new unit_node_list();
...
}
```

<aside>
💡 Узел using_list является устаревшим, так как он относился к ключевому слову using.

</aside>