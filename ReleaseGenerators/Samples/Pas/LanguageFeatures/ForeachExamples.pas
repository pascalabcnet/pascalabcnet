// Пример иллюстрирует всевозможные типы контейнеров, 
// по которым можно перемещаться с помощью оператора foreach

const n = 10;

var 
  a: array [1..n] of integer;
  b: array of integer;
  s: set of integer;
  l: List<integer>;

begin
  for var i:=1 to n do 
    a[i] := Random(100);
  // Цикл foreach по статическому массиву
  foreach var x in a do
    Print(x);
  writeln;  
    
  SetLength(b,n);
  for var i:=0 to n-1 do 
    b[i] := Random(100);
  
  // Цикл foreach по динамическому массиву
  foreach var x in b do
    Print(x);
  writeln;  
  
  s := [2..5,10..14];
  // Цикл foreach по множеству
  foreach var x in s do
    Print(x);
  writeln;  
  
  l := new List<integer>;
  l.AddRange(b);
  l.Reverse;
  // Цикл foreach по списку
  foreach var x in l do
    Print(x);
  writeln;  
  
end.