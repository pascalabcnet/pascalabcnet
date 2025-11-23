function f1<T>: T;begin end;

begin
  var a: byte := f1&<byte>();
  assert(a = 0);
end.