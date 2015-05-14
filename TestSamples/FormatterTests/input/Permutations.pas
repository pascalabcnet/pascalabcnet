// Генерация всех перестановок длины n
procedure swap(var a,b: integer);
var v: integer;
begin
  v:=a;
  a:=b;
  b:=v
end;

procedure Solve(n: integer);

const Sz=100;
type IArr=array [1..Sz] of integer;
var A: IArr;

  procedure PrintA;
  var i: integer;
  begin
    for i:=1 to n do
      write(a[i],' ');
    writeln;
  end;

  procedure FillA;
  var i: integer;
  begin
    for i:=1 to n do
      A[i]:=i;
  end;
  
  procedure Solve0(t: integer);
  var i: integer;
  begin
    for i:=t to n do
    begin
      swap(A[t],A[i]);
      if t=n then
        PrintA
      else Solve0(t+1);
      swap(A[t],A[i]);
    end;
  end;
  
begin // Solve
  FillA;
  Solve0(1);
end;
  
begin
  cls;
  Solve(4);
end.
