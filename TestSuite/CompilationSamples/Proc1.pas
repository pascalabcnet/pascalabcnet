// Параметры процедур

procedure Operations(a,b: integer);
begin
  writeln(a,' + ',b,' = ',a+b);
  writeln(a,' - ',b,' = ',a-b);
  writeln(a,' * ',b,' = ',a*b);
  writeln(a,' / ',b,' = ',a/b);
  writeln(a,' div ',b,' = ',a div b);
  writeln(a,' mod ',b,' = ',a mod b);
end;

begin
  Operations(5,3);
  writeln;
  Operations(7,4);
end.  