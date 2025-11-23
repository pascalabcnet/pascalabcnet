function test: sequence of char;
begin
  yield 'a';
end;

function test2<T>: sequence of array of T;
begin
  yield new T[2];
end;
begin
  foreach var c in test do
    writeln(c{@var c: char;@});
  foreach var o{@var o: array of real;@} in test2&<real> do;
end.