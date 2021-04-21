// #2172
procedure p1(p: ()->()); 
begin
  Assert(1=1)
end;

procedure p1(p: byte->());
begin
  Assert(1=2)
end;

begin
  p1(procedure-> exit());
end.