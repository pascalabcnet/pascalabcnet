// Интерпретатор выражений, записанных в виде символьной строки
{$reference System.Data.dll}
uses System.Data;

begin
  var s := '( 2.4 + 3 % 2)*6 - 1 ';
  DataTable.Create.Compute(s, '').ToString.Print;
  s := '(5 > 3) and (2 < 4)';
  Print(DataTable.Create.Compute(s, ''))
end.