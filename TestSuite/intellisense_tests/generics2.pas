function f1<T1, T2, T3>: (T1, T2, T3);begin end;
function f2<T1, T2, T3>(a: byte): (T1, T2, T3);begin end;
begin
  var a{@var a: (byte,word,single);@} := f1&<byte, word, single>;
  var b{@var b: function(a: byte): (byte,word,single);@} := f2&<byte, word, single>;
  var c{@var c: (byte,word,single);@} := f1&<byte, word, single, char>;
end.