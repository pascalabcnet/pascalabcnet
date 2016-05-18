function Gen: sequence of real;
begin
  yield 1;
end;


begin
  foreach var x in Gen() do
    write(x, ' ');
end.