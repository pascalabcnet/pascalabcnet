uses u_overloads3; 
function f(a: integer): integer := 10;

begin
  var r := f(0);
  assert(r = 10);
end.