// Сравнение производительности обычного алгоритма накопления суммы  
// и метода, использующего лямбда-выражение
begin
  var n := 100000000;
  var q := Range(1,n).Select(x->1/x).Sum();
  Println(q,MillisecondsDelta);
    
  var s := 0.0;
  for var i:=1 to n do
    s += 1.0/i;

  Println(s,MillisecondsDelta);
end.