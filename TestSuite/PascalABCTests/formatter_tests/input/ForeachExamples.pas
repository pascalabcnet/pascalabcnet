// Пример иллюстрирует всевозможные типы контейнеров, 
// по которым можно перемещаться с помощью оператора foreach

uses System.Collections.Generic;

const n = 10;

var 
  x: integer;
  a: array [1..n] of integer;
  b: array of integer;
  s: set of integer;
  l: List<integer>;

begin
  for var i:=1 to n do 
    a[i] := Random(100);
  // Цикл foreach по статическому массиву
  foreach x in a do
    write(x,' ');
  writeln;  
    
  SetLength(b,n);
  for var i:=0 to n-1 do 
    b[i] := Random(100);
  
  // Цикл foreach по динамическому массиву
  foreach x in b do
    write(x,' ');
  writeln;  
  
  s := [2..5,10..14];
  foreach x in s do
    write(x,' ');
  writeln;  
  
  l := new List<integer>;
  l.AddRange(b);
  l.Reverse;
  foreach x in l do
    write(x,' ');
  writeln;  
  
end.