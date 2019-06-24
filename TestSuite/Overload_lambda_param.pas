procedure p(o: integer);
begin
  Assert(1=666)
end;

procedure p(f: ()->integer);
begin
  Assert(1=1)
end;

begin
  p(()->5); 
end.