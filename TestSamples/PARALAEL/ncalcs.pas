//Допустим надо провести N паралельных вычичлений на отрезках функции

function f(l,r:real):real;
begin
  {logic}
end;

var N: integer;

procedure Calc(l,r:real); async;
begin
  PrintResult.valueList.Add(f(l,r));  
  if GetResult.valueList.Count = N then
    PrintResult.calcFinished := true;
end;

procedure PrintResult;
asyncparam 
  valueList := new Dictonary<real,real>;
  calcFinished := false;
begin
  foreach r:real in valueList.Keys do
    writeln(r, ':' , valueList[r]);
end;

begin
  readln(N);
  var l := 0;
  var step := 10 / N;
  for var i:=1 to n do begin
    Calc(l, l + step);
    l += step;
  end;
  PrintResult;
end;