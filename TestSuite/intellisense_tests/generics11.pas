function f1<T>(a: T) := new List<T>;

begin
  var o{@var o: List<List<byte>>;@} := f1(f1(byte(0)));
end.