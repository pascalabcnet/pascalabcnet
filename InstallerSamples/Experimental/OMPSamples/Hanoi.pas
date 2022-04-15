//Демонстрация использования параллельных секций на примере задачи о ханойских башнях
// Вывод решения закомментирован, так как он занимает большую часть времени

//параллельная процедура  
procedure MovePiramidParallel(n: integer; f, t, w: integer);
begin
  if n = 0 then
    exit;
  {$omp parallel sections}
  begin
    MovePiramidParallel(n - 1, f, w, t);
    //writelnFormat('Переложить диск с {0} на {1}', f, t);
    MovePiramidParallel(n - 1, w, t, f);
  end;
end;
//последовательная процедура   
procedure MovePiramid(n: integer; f, t, w: integer);
begin
  if n = 0 then
    exit;
  MovePiramid(n - 1, f, w, t);
  //writelnFormat('Переложить диск с {0} на {1}', f, t);
  MovePiramid(n - 1, w, t, f);
  
end;

begin
  var m0 := Milliseconds;
  MovePiramid(27, 1, 2, 3);
  writeln('Последовательное выполнение: ', Milliseconds - m0, 'ms');
  
  var m1 := Milliseconds;
  MovePiramidParallel(27, 1, 2, 3);
  writeln('Параллельное выполнение: ', Milliseconds - m1, 'ms');
end.