// Алгоритм определения простоты числа
var 
  N: integer;
  IsSimple: boolean;

begin
  writeln('Введите число: ');
  readln(N);

  IsSimple := True;
  for var i:=2 to round(sqrt(N)) do // если число составное, то один из его сомножителей <= (sqrt(N)) 
    if N mod i = 0 then
    begin
      IsSimple := False;
      break;
    end;
  
  if IsSimple then
    writeln('Число ',N,' простое')
  else writeln('Число ',N,' составное');  
end.