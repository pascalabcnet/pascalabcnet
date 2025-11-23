// Процедурная переменная как параметр
procedure for_each(a: array of real; p: procedure(var r: real));
begin
  for var i := 0 to a.Length-1 do
    p(a[i]);
end;

procedure mult2(var r: real);
begin
  r := 2*r
end;

procedure print(var r: real);
begin
  write(r,' ');
end;

var a: array of real := (1,2,3,6,7);

begin
  writeln('Содержимое массива: ');
  for_each(a,print); 
  writeln;
  for_each(a,mult2);
  writeln('Содержимое массива после умножения его элеметов на 2: ');
  for_each(a,print);
end.