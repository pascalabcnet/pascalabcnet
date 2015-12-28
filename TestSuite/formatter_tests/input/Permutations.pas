// Все перестановки
uses Arrays;

const n = 4;

var a: array of integer;

procedure Perm(m: integer);
begin
  if m=1 then
    a.Writeln;
  for var i:=0 to m-1 do
  begin
    Swap(a[i],a[m-1]); // ставим каждый на место последнего
    Perm(m-1);
    Swap(a[i],a[m-1]);
  end;  
end;

begin
  SetLength(a,n);
  for var i:=0 to n-1 do
    a[i] := i+1;
  Perm(n);
end. 