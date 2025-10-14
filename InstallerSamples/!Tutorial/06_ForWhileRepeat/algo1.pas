// Алгоритм определения простоты числа

begin
  var N := ReadInteger('Введите число:');
  var IsPrime := True;
  for var i := 2 to Round(Sqrt(N)) do // если число составное, то один из его сомножителей ≤ sqrt(N)
    if N mod i = 0 then
    begin
      IsPrime := False;
      break;
    end;
  if IsPrime then
    Println('Число', N, 'простое') else Println('Число', N, 'составное');
end.
