// Вызов статического метода add класса Class1, помещенного в пространство имен ClassLibrary1
// Класс Class1 помещен в библиотеку ClassLibrary1.dll, откомпилированную на C#

{$reference ClassLibrary1.dll}
uses ClassLibrary1;

begin
  writeln(Class1.add(2,3));  
end.