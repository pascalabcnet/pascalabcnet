// Пример иллюстрирует возможности оператора foreach
uses System.Collections.Generic;

var 
  a: array [1..5] of integer := (1,3,5,7,9);
  s: set of integer;
  l: List<integer>;

begin
  write('foreach по обычному массиву: ':35);
  foreach x: integer in a do
    write(x,' ');
  writeln;  
  
  s := [2..5,10..14];
  write('foreach по множеству: ':35);
  foreach x: integer in s do
    write(x,' ');
  writeln;  
  
  l := new List<integer>;
  l.Add(7); l.Add(2); l.Add(5);
  write('foreach по динамическому массиву: ':35);
  foreach x: integer in l do
    write(x,' ');
end.