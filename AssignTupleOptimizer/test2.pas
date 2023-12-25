var a : Integer := 3;
procedure p();
begin
  var b := 5;
  for var i := 3 to 4 do 
  begin
    var a := 4;
    (a,b) := (b,a);
  end;
  (a, b) := (b, a);
end;

begin 
  
  var f: (integer, integer) -> integer := (x, a) -> x*x + a;
  writeln(a);
  p();
  writeln(a);
end.