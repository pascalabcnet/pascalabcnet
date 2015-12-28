// Использование динамического массива. 
var a: array of integer;

begin
  writeln('При присваивании динамических массивов две переменные начинают указывать на одну память:');
  var n := 20;
  a := new integer[n]; // Другой способ выделения памяти
  for var i:=0 to a.Length-1 do
    a[i] := i;

  writeln('До присваивания b[5]: a[5]=',a[5]);
  
  var b: array of integer;
  b := a;
  b[5] := 666;
  writeln('После присваивания b[5]: a[5]=',a[5]);
end.