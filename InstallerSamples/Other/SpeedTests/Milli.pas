// Производительность заполнения статических и динамических массивов
const n = 20000;

var a: array [1..n,1..n] of real;

begin
  for var i:=1 to n do
  for var j:=1 to n do
    a[i,j] := 1;
  Println(MillisecondsDelta/1000);
  
  var m := MatrFill(n,n,1);
  Println(MillisecondsDelta/1000);
end.
