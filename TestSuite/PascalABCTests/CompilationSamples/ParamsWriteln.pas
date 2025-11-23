// Создание процедуры MyWriteln с переменным числом параметров

procedure MyWriteln(params args: array of object);
begin
  foreach x: Object in args do
    System.Console.Write(x);
  System.Console.WriteLine;
end;

var 
  a: integer := 777;
  b: boolean := True;
  r: real := 3.1415;

begin
  MyWriteln(a,' ',b,' ',r);
end.