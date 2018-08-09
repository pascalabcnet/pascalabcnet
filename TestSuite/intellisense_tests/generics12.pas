function f1<T>(a: T) := new T[0];

begin
  var o{@var o: array of array of byte;@} := f1(f1(byte(0)));
  
end.