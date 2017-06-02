// Использование динамического массива. 
var a: array of integer;

begin
  writeln('SetLength при повторном вызове не портит старые элементы:');
  SetLength(a,5);
  for var i:=0 to a.Length-1 do
    a[i] := i+1;
  a.Println;

  SetLength(a,10);
  a.Println;
end.