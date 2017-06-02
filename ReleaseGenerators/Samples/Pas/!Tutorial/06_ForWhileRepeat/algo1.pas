// Алгоритм определения простоты числа
var 
  N: integer;
  IsPrime: boolean;

begin
  writeln('Введите число: ');
  readln(N);

  IsPrime := True;
  for var i:=2 to round(sqrt(N)) do // если число составное, то один из его сомножителей <= (sqrt(N)) 
    if N mod i = 0 then
    begin
      IsPrime := False;
      break;
    end;
  
  if IsPrime then
    writeln('Число ',N,' простое')
  else writeln('Число ',N,' составное');  
end.