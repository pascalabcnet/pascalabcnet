// Игра "Спички"
const InitialCount=15;

var
  /// Текущее количество спичек
  Count: integer;
  /// Количество спичек, которое берет игрок
  Num: integer;
  /// Номер текущего игрока
  Player: integer;

begin
  Player := 1;
  Count := InitialCount;
  
  repeat
    if Player=1 then
    begin
      var Correct: boolean;
      repeat
        write('Ваш ход. На столе ',Count,' спичек. ');
        write('Сколько спичек Вы берете? ');
        readln(Num);
        Correct := (Num>=1) and (Num<=3) and (Num<=Count);
        if not Correct then
          writeln('Неверно! Повторите ввод!');
      until Correct;
    end
    else
    begin
      Num := Random(1,3);
      if Num>Count then 
        Num := Count;
      writeln('Мой ход. Я взял ',Num,' спичек');
    end;
    Count -= Num;
    if Player=1 then 
      Player := 2
    else Player := 1;
  until Count=0;
  
  if Player=1 then
    writeln('Вы победили!')
  else writeln('Вы проиграли!');
end.
