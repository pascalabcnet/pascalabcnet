// 3.11.1. ArrRandomInteger и MatrRandomInteger с диапазоном
begin
  var a := ArrRandomInteger(10,5..10);
  a.Println;
  var m := MatrRandomInteger(3,4,2..9);
  m.Println(3);
end.