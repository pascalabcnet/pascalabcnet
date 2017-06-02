// Это - главная программа
// Именами из dll-библиотеки, написанной на PascalABC.NET, можно пользоваться, 
// не подключая пространства имен
{$reference mydll.dll}

begin
  PrintPascalABCNET;
  writeln(n);
  writeln(add(2,3));
end.