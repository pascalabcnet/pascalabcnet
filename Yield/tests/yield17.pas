type A = class
  function Gen: sequence of real;
end;

function A.Gen: sequence of real;
begin
  yield 1;
end;

begin
  var t := new A();
  foreach var x in t.Gen do
    Print(x);
end.