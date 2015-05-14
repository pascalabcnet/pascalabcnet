library Lib1;
function Test1: integer;
begin
  Result := 10;
end;

function Test2(a: integer; b: integer:=5): integer;
begin
  Result := a+b;
end;

function Test3(params arr: array of integer): integer;
begin
  Result := arr[0]+arr[1];
end;
end.