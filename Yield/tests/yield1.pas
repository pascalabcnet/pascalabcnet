function Gen: sequence of integer;
begin
  yield 1;
  yield 5;
end;

begin
  var q := Gen();
  foreach var x in q do
    Print(x);
end.