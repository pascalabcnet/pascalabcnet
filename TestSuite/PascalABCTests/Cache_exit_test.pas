[Cache]
function f1(q: byte): byte;
begin
  var a := q;
  Result := 1;
  if a > 2 then
  begin  
    Result := 2;
    exit;
  end;
  //exit(5);
  Result := 3;
  a := 5;
  exit;
end;

begin
  Assert(f1(3)=f1(3));
end.