function f1<T1, T2, T3>(a: (T1, T2, T3)): (T1, T2, T3);begin end;

begin
  var a{@var a: (byte,word,single);@} := f1((byte(0), word(0), single(0)));
end.