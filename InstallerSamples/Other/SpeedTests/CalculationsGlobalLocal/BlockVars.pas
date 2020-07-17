// Сравнение скорости работы глобальных и внутриблочных переменных.
// См. также файл GlobalVars.pas
begin
  var s := 0.0;
  var i: real := 1;
  while i<1000000000 do
  begin
    s += 1/i;
    i += 1;
  end;  
  writeln(s);
  writeln('Время расчета = ',Milliseconds/1000,' с');
end.
