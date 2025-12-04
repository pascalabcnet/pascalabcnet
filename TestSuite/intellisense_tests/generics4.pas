function f1<T>(a: T) := a;

function f2<T>(): T;begin end;

begin
  var a1{var a1: byte;} := f1&<byte>(word(0));
  var a2{var a2: byte;} := f2&<byte>();
  var a3{var a3: word;} := f1(word(0));
end.