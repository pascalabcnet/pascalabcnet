type A = class
end;

function integer.Gen(self: A): sequence of real; extensionmethod;
begin
  yield 1;
end;

begin
  var t := new A();
  foreach var x in A.Gen do
    Print(x);
end.