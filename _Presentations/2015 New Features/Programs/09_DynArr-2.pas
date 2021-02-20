const n = 10;

type IntArr = array [1..n] of integer;

procedure SquareElems(const a: IntArr; 
  n: integer; var Result: IntArr);
begin
  for var i:=1 to n do
    Result[i] := sqr(a[i]);
end;

const m = 4;

var a,Result: IntArr;
    i: integer;
begin
  a[1] := 2; a[2] := 5; 
  a[3] := 7; a[4] := 10;
  SquareElems(a,m,Result);
  for i:=1 to m do
    write(Result[i],' ');
end.