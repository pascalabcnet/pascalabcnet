function f0(x: byte) := x;

procedure p1(o: object) := exit;
procedure p1(o: word) := exit;

begin
  p1(f0); 
  Assert(True)
end.