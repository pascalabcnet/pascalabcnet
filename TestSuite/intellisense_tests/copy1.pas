begin
  var a1: array of byte;
  var a2{@var a2: array of byte;@} := Copy(a1);//тут для a2 будет показывать тип string
  var b1{@var b1: array[,] of byte;@}: array[,] of byte;
  var b2 := Copy(b1);//тут так же для b2
  var s{@var s: string;@} := Copy('aaa',0,10);
end.