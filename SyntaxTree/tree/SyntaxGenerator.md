# Генерация кода для узлов со списками по шаблону

Переменные шаблона записываются внутри символов `<#` и `#>` и могут иметь следующий вид:
- `<#NodeName#>` - название класса синтаксического узла
- `<#ListElementType#>` - тип элементов списка
- `<#ListName#>` - название поля, имеющиего тип `List<T>`

Шаблоны генерации находятся в папке [SyntaxTemplates](https://github.com/pascalabcnet/pascalabcnet/tree/master/SyntaxTree/tree/SyntaxTemplates):

1. [SyntaxWithListConstructors](https://github.com/pascalabcnet/pascalabcnet/blob/master/SyntaxTree/tree/SyntaxTemplates/SyntaxWithListConstructors.txt) - шаблон генерации конструкторов.  
2. [SyntaxWithListMethods](https://github.com/pascalabcnet/pascalabcnet/blob/master/SyntaxTree/tree/SyntaxTemplates/SyntaxWithListMethods.txt) - шаблон генерации методов.

На этапе генерации в каждый синтаксический узел, имеющий список дочерних узлов, добавляется код, созданный по данным шаблонам. 
