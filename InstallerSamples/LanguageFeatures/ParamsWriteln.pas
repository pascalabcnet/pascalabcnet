// Создание процедуры MyWriteln с переменным числом параметров

procedure MyWriteln(params args: array of object);
begin
  foreach var x in args do
    Write(x);
  WriteLn;
end;

var 
  a: integer := 777;
  b: boolean := True;
  r: real := 3.1415;

begin
  MyWriteln(a,' ',b,' ',r);
end.