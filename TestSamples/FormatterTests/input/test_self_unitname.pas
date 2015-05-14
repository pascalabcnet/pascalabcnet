unit test_self_unitname;

interface

procedure p;
var x:integer;

implementation

procedure p;
begin
  if false then
    test_self_unitname.p;
  writeln(test_self_unitname.x);
  
end;

end.