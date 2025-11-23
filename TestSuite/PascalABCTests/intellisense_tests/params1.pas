function f1<T>(params a: array of T): sequence of T := new T[0];

begin
  var a{@var a: sequence of IEnumerable<integer>;@} := f1(f1(0));
  
end.