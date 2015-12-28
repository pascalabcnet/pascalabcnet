//Процедуры с переменным числом параметров

procedure writeln(params args:array of object);
var i:integer;
begin
  write('>');
  for i:=0 to args.length-1 do
    System.Console.Write(args[i]);
  System.Console.WriteLine;
end;

var a:integer;

begin
  a:=10;
  Writeln('a=',a,';');
  
  readln;
end.