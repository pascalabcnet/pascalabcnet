type A = class
  j,k: integer;
end;

function Gen: sequence of integer;
var j,k: real;
begin
  var i := 1;
  yield i*j;
end;

begin
end.