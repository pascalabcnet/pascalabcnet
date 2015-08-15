type A = class
  j,k: integer;
end;

function Gen(a,b: real): sequence of integer;
var j,k: real;
begin
  var i := 1;
  while i<5 do
  begin
    yield i*i;
    i += 1;
  end;
end;

begin
end.